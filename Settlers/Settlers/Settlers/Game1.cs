using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Settlers
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Változók
        private GraphicsDeviceManager graphics; // Form kirajzolásához szükséges
        private SpriteBatch spriteBatch; // Kirajzoláshoz szükség változó
        private Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>(); //Játék textúráit tárolja
        private List<Button> GameMenuButtons = new List<Button>();  //Épület elhelyezéséhez szükséges gombok
        private MouseState ms = new MouseState(); //Egér jelenlegi állapotát tárolja
        private MouseState prevMS = new MouseState(); //Egér utolsó állapotát tárolja
        private KeyboardState ks = new KeyboardState(); //Lenyomott gombot tárolja
        private KeyboardState prevKs = new KeyboardState(); // Az utolsó lenyomott gombot tárolja
        private GameState gs = new GameState(); // A játék állását tárolja
        private List<Building> buildings = new List<Building>(); // Lehelyezett épületeket tárolja
        private Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>(); // Az összes felhasználható fontot tárolja
        private List<Output> outputs = new List<Output>(); //Az összes szöveg, ami játékban található
        private int actualWorkers = Globals.STARTWORKERS; //A "munkanélküli" munkások száma
        private int allWorkers = Globals.STARTWORKERS; //Az összes elérhetõ munkás száma
        Dictionary<BaseMaterial, int> basematerials = new Dictionary<BaseMaterial, int>(); //Nyersanyagokat és a mennyiségüket tárolja
        private int yOutput = 410; // Játékon belüli szövegekhez lévõ segéd változó
        Texture2D background; // A menûk háttérképe
        private bool errorOrNot; // Van-e hiba a játék betöltésekor
        private bool cannotLoad = false; // Betudja-e tölteni a mentést
        private bool wantToContinue = false; // Játékos teljesitette a célt, akarja-e folytatni
        private bool colorized = false; // Az épület termelési területe, ki van a rajzolva 

        Map map; // Pályát tárolja
        private MySqlConnectionHandler connector; // Adatbázis kapcsolat, ezen keresztül érjük el az adatbázist
        #endregion
        #region Menu gombok
        Button startButton;
        Button exitButton;
        Button loadButton;
        #endregion

        #region Szünet gombok
        Button continueButton;
        Button saveButton;
        #endregion

        #region Játék elötti és Játék végén lévõ gombok
        Button beforeStartButton;
        Button beforeExitButton;
        Button endContinueButton;
        #endregion

        /// <summary>
        /// A játék elinditásához szükséges változók
        /// Felveszi az adatbázis kapcsolatot
        /// Betölti a Content mappát
        /// Kirajzolja a Formot
        /// Megjeleniti az egeret
        /// </summary>
        public Game1()
        {
            connector = new MySqlConnectionHandler();
            errorOrNot = connector.TryOpen();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1100;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
        }
        /// <summary>
        /// Megadjuk, hogy a játék elindulásakor a menü induljon el
        /// </summary>
        protected override void Initialize()
        {
            gs = GameState.Menu;
            base.Initialize();
        }
        /// <summary>
        /// Betölti az összes Texturát, amit a játékon belül felhasználunk
        /// Fontokat, szövegeket
        /// Végre hajt, egy ellenõrzést, hogy ha nem jött létre adatbázis kapcsolat ne töltse be az elemeket!
        /// </summary>
        protected override void LoadContent()
        {
            Directory.GetFiles("Content/Fonts", "*.xnb", SearchOption.AllDirectories).ToList().ForEach(x => fonts.Add(Path.GetFileNameWithoutExtension(x), Content.Load<SpriteFont>(Path.Combine(Path.GetDirectoryName(x).Substring(8), Path.GetFileNameWithoutExtension(x)))));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (errorOrNot)
            {
                try
                {
                    Directory.GetFiles("Content/Textures", "*.xnb", SearchOption.AllDirectories).ToList().ForEach(x => Textures.Add(Path.GetFileNameWithoutExtension(x), Content.Load<Texture2D>(Path.Combine(Path.GetDirectoryName(x).Substring(8), Path.GetFileNameWithoutExtension(x)))));
                    startButton = new Button(420, 70, 300, 100, Textures["startNotP"], Textures["startP"]);
                    exitButton = new Button(420, 340, 300, 100, Textures["exitNotP"], Textures["exitP"]);
                    beforeStartButton = new Button(720, 450, 300, 100, Textures["startNotP"], Textures["startP"]);
                    beforeExitButton = new Button(120, 450, 300, 100, Textures["exitNotP"], Textures["exitP"]);
                    loadButton = new Button(420, 210, 300, 100, Textures["loadNotP"], Textures["loadP"]);
                    continueButton = new Button(420, 70, 300, 100, Textures["continueNotP"], Textures["continueP"]);
                    endContinueButton = new Button(720, 450, 300, 100, Textures["continueNotP"], Textures["continueP"]);
                    saveButton = new Button(420, 210, 300, 100, Textures["saveNotP"], Textures["saveP"]);
                    background = Textures["woodenBackground"];
                    outputs.Add(new Output("load", $"There is nothing to load! Database currently empty!", new Vector2(180, 480), Color.Yellow, fonts["Errorfont"], false));
                    outputs.Add(new Output("worker", $"Workers: {actualWorkers - (buildings.Count(x => x.HasWorker == true))}/{allWorkers}", new Vector2(910, yOutput), Color.Black, fonts["MyFont"], true));
                    outputs.Add(new Output("before", $"Welcome in 'The Settlers' game!\n This is a strategy game in the middle age! You have to place buildings, gathering resources!\n The game will end, if you collect 50 meat, 100 bread and 25 worker!", new Vector2(50, 100), Color.White, fonts["BeforeFont"], false));
                    outputs.Add(new Output("ending", $"Well done! The demo is over!\n You can choose, if you want to go back to game press 'Continue' button.\n If you want to close the game press 'Exit' button!", new Vector2(50, 100), Color.White, fonts["BeforeFont"], false));
                }
                catch (Exception ex)
                {
                    errorOrNot = false;
                }
                if (errorOrNot)
                {
                    basematerials = this.connector.GetBaseMaterial();
                    foreach (var item in basematerials)
                    {
                        yOutput += 20;
                        outputs.Add(new Output(item.Key.Name, $"{item.Key.Name}: {item.Value}", new Vector2(910, yOutput), Color.Black, fonts["MyFont"], false));
                    }
                }
                else
                    gs = GameState.Error;
                outputs.Add(new Output("error", $"Error message:\n Can't load the images!\n Please reinstall the game!", new Vector2(150, 100), Color.Yellow, fonts["Errorfont"], false));

            }
            else
            {
                gs = GameState.Error;
                outputs.Add(new Output("error", $"Error message:\n Cannot create the connection!\n Please reinstall the game!", new Vector2(150, 100), Color.Yellow, fonts["Errorfont"], true));

            }

        }

        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Az Update metodusban fut maga a játék, minden amit a felhasználó csinál a futás alatt azt itt dolgozza fel
        /// IsActive segitségével tudjuk, hogy a felhasználó épp a programmal foglalkozik, vagy épp egy másik ablak van-e megnyitva
        /// Több részre van felosztva, a szerint hogy a játék éppen melyik játék állásban van!
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                prevMS = ms;
                ms = Mouse.GetState();
                prevKs = ks;
                ks = Keyboard.GetState();
                if (gs == GameState.Menu)
                {
                    if (startButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.BeforePlaying;
                    }
                    if (exitButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.Exit;
                        Exit();
                    }
                    if (loadButton.LeftClick(ms, prevMS))
                    {
                        if (connector.SelectTiles() > 0)
                        {
                            #region Pálya és a texturák betöltése
                            this.map = new Map();
                            map.Tiles = connector.GetTiles();
                            this.map.InitLoadedTiles(Textures["grass"], Textures["tree"], Textures["stone"], Textures["buildingmenu"]);
                            this.GameMenuButtons = this.map.InitInGameMenu(Textures);
                            #endregion
                            #region Épület Betöltés
                            buildings = connector.GetBuilding();
                            string s = "";
                            foreach (var item in buildings)
                            {
                                s = item.GetTextureName(item.BuildingType);
                                item.Production = connector.GetProductions(item);
                                item.Bounds = item.Rectangle;
                                item.Texture = Textures[s];
                                if ((item.BuildingType == BuildingTypeEnum.Stonequarry) || (item.BuildingType == BuildingTypeEnum.Woodcutter))
                                {
                                    item.ProductionSum(map);
                                }
                            }
                            #endregion

                            basematerials = connector.GetSavedMaterial();
                            gs = GameState.Playing;


                        }
                        else
                        {
                            cannotLoad = true;
                        }
                    }
                    if (startButton.MouseOver(ms)) { startButton.ChangeState(2); } else { startButton.ChangeState(1); }
                    if (exitButton.MouseOver(ms)) { exitButton.ChangeState(2); } else { exitButton.ChangeState(1); }
                    if (loadButton.MouseOver(ms)) { loadButton.ChangeState(2); } else { loadButton.ChangeState(1); }

                }
                else if (gs == GameState.Playing)
                {
                    #region Épület létrehozás
                    string[] s = null;
                    int[] bMID = null;
                    bool canCreate = false;
                    bool cannotCreate = false;
                    foreach (var item in GameMenuButtons)
                    {
                        s = Textures.FirstOrDefault(x => x.Value == item.Texture).Key.Split('_');
                        if (item.MouseOver(ms)) { item.ChangeState(2); } else { item.ChangeState(1); }
                        if (item.LeftClick(ms, prevMS))
                        {
                            bMID = connector.GetBuildingTypeCreate(item.buildingType);
                            foreach (var material in basematerials)
                            {
                                if (material.Key.Name == "Wood")
                                {
                                    if (material.Value >= bMID[0])
                                        canCreate = true;
                                    else
                                        canCreate = false;

                                }
                                if (material.Key.Name == "Stone")
                                {
                                    if (material.Value >= bMID[1])
                                        canCreate = true;
                                    else
                                        canCreate = false;
                                }
                            }
                            if (canCreate)
                            {
                                if (buildings != null || buildings.Count > 0)
                                {
                                    foreach (var b in buildings)
                                    {
                                        if ((int)b.Status == 0)
                                            cannotCreate = true;
                                    }
                                }
                                if (!cannotCreate)
                                {
                                    Building building = new Building((buildings == null || buildings.Count == 0) ? 1 : buildings.Max(x => x.ID) + 1, new Rectangle(0, 0, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), Textures[s[1]], BuildingStatus.Placing, item.buildingType, false);
                                    building.Production = connector.GetProductions(building);
                                    buildings.Add(building);
                                    BaseMaterial wood = basematerials.FirstOrDefault(x => x.Key.Name == "Wood").Key;
                                    if (basematerials.TryGetValue(wood, out int oldValue))
                                    {
                                        basematerials[wood] = oldValue - bMID[0];
                                    }
                                    BaseMaterial stone = basematerials.FirstOrDefault(x => x.Key.Name == "Stone").Key;
                                    if (basematerials.TryGetValue(stone, out oldValue))
                                    {
                                        basematerials[stone] = oldValue - bMID[1];
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Épület mozgatás
                    if (this.buildings.Count() != 0)
                    {
                        this.buildings.ForEach(x => x.Update());
                        bool canBePlacing = false;

                        var move = buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing);
                        if (move != null)
                        {
                            if (ks.IsKeyDown(Keys.W) && map.CheckTile(Direction.Up, move.Step(Direction.Up)))
                            {
                                colorized = false;
                                buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).ColorizeProductionArea(colorized, map, spriteBatch);
                                move.MoveBuilding(Direction.Up);
                            }
                            if (ks.IsKeyDown(Keys.S) && map.CheckTile(Direction.Down, move.Step(Direction.Down)))
                            {
                                colorized = false;
                                buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).ColorizeProductionArea(colorized, map, spriteBatch);
                                move.MoveBuilding(Direction.Down);
                            }
                            if (ks.IsKeyDown(Keys.D) && map.CheckTile(Direction.Right, move.Step(Direction.Right)))
                            {
                                colorized = false;
                                buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).ColorizeProductionArea(colorized, map, spriteBatch);
                                move.MoveBuilding(Direction.Right);
                            }
                            if (ks.IsKeyDown(Keys.A) && map.CheckTile(Direction.Left, move.Step(Direction.Left)))
                            {
                                colorized = false;
                                buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).ColorizeProductionArea(colorized, map, spriteBatch);
                                move.MoveBuilding(Direction.Left);
                            }
                            if (ks.IsKeyDown(Keys.Enter))
                            {
                                colorized = false;
                                buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).ColorizeProductionArea(colorized, map, spriteBatch);
                                canBePlacing = map.PlaceBuilding(move.Bounds);
                                if (canBePlacing)
                                {
                                    if ((buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).BuildingType == BuildingTypeEnum.Woodcutter) || (buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).BuildingType == BuildingTypeEnum.Stonequarry))
                                        buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).ProductionSum(map);

                                    buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).Status = BuildingStatus.Construction;

                                }
                            }
                            if (ks.IsKeyDown(Keys.R))
                            {
                                colorized = true;
                                buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).ColorizeProductionArea(colorized, map, spriteBatch);
                            }
                            if (ks.IsKeyDown(Keys.Delete))
                            {
                                Building removeBuilding = buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing);
                                bMID = connector.GetBuildingTypeCreate(removeBuilding.BuildingType);
                                buildings.Remove(removeBuilding);
                                BaseMaterial wood = basematerials.FirstOrDefault(x => x.Key.Name == "Wood").Key;
                                if (basematerials.TryGetValue(wood, out int oldValue))
                                {
                                    basematerials[wood] = oldValue + bMID[0];
                                }
                                BaseMaterial stone = basematerials.FirstOrDefault(x => x.Key.Name == "Stone").Key;
                                if (basematerials.TryGetValue(stone, out oldValue))
                                {
                                    basematerials[stone] = oldValue + bMID[1];
                                }
                            }
                        }
                    }
                    #endregion
                    if (ks.IsKeyDown(Keys.Escape))
                        gs = GameState.Pause;
                    actualWorkers = allWorkers - (buildings.Count(x => x.HasWorker == true));
                    this.outputs.ForEach(x =>
                    {
                        if (x.IsItWorker)
                        {
                            x.Update(null, 0, actualWorkers, allWorkers);
                        }
                        else
                        {
                            foreach (var item in basematerials)
                            {
                                if (item.Key.Name == x.Name)
                                {
                                    x.Update(item.Key, item.Value, actualWorkers, allWorkers);
                                }
                            }
                        }
                    });
                    this.buildings.ForEach(x =>
                    {
                        if (x.Status == BuildingStatus.Ready && x.BuildingType == BuildingTypeEnum.House && x.IsItEmpty == false)
                        {
                            allWorkers = x.Production.HouseUpdate(x, allWorkers);
                        }
                        if (x.Status == BuildingStatus.Ready && x.BuildingType != BuildingTypeEnum.House)
                        {
                            x.Production.Update(x, basematerials, (int)gameTime.ElapsedGameTime.TotalMilliseconds);
                        }
                        if (x.Status == BuildingStatus.Construction)
                        {
                            x.UpdateBuidlingStatus((int)gameTime.ElapsedGameTime.TotalMilliseconds);
                        }
                        if (!x.HasWorker)
                        {
                            x.UpdateWorkerStatus(actualWorkers);
                        }
                    });
                    if (!wantToContinue)
                    {
                        if (basematerials[basematerials.Keys.FirstOrDefault(x => x.Name == "Meat")] >= 50)
                        {
                            if (basematerials[basematerials.Keys.FirstOrDefault(x => x.Name == "Bread")] >= 100)
                            {
                                if (allWorkers >= 25)
                                {
                                    gs = GameState.EndGame;
                                }
                            }
                        }
                    }

                }
                else if (gs == GameState.Pause)
                {
                    if (continueButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.Playing;
                    }
                    if (exitButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.Exit;
                        Exit();
                    }
                    if (saveButton.LeftClick(ms, prevMS))
                    {
                        #region Nyersanyagok mentése
                        if (basematerials.Count() != 0 && basematerials != null)
                        {
                            connector.DeleteMaterials();
                            foreach (var item in basematerials)
                            {
                                connector.SaveMaterials(item.Key.ID, item.Value);
                            }
                        }
                        #endregion
                        #region Épületek mentése
                        if (buildings.Count() != 0 && buildings != null)
                        {
                            connector.DeleteBuilding();
                            connector.DeleteTiles();
                            foreach (var item in buildings)
                            {
                                connector.InsertBuilding(item);
                            }
                        }
                        #endregion
                        #region Tileok mentése
                        int idTile = 1;
                        string line = "";
                        foreach (var item in map.Tiles)
                        {
                            if (item.State != TileState.Menu && item != null)
                            {
                                if (idTile <= 59)
                                {
                                    line += $"{item.Rectangle.X},{item.Rectangle.Y},{(int)item.State};";
                                }
                                else
                                {
                                    line += $"{item.Rectangle.X},{item.Rectangle.Y},{(int)item.State};";
                                    connector.InsertTiles(line);
                                    line = "";
                                    idTile = 0;
                                }
                                idTile++;
                            }
                        }
                        #endregion
                    }
                    if (continueButton.MouseOver(ms)) { continueButton.ChangeState(2); } else { continueButton.ChangeState(1); }
                    if (saveButton.MouseOver(ms)) { saveButton.ChangeState(2); } else { saveButton.ChangeState(1); }
                    if (exitButton.MouseOver(ms)) { exitButton.ChangeState(2); } else { exitButton.ChangeState(1); }
                }
                else if (gs == GameState.BeforePlaying)
                {

                    if (beforeStartButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.Playing;
                        this.map = new Map();
                        this.map.InitTiles(Textures["grass"], Textures["tree"], Textures["stone"], Textures["buildingmenu"]);
                        this.GameMenuButtons = this.map.InitInGameMenu(Textures);
                    }
                    if (beforeExitButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.Menu;
                    }
                    if (beforeStartButton.MouseOver(ms)) { beforeStartButton.ChangeState(2); } else { beforeStartButton.ChangeState(1); }
                    if (beforeExitButton.MouseOver(ms)) { beforeExitButton.ChangeState(2); } else { beforeExitButton.ChangeState(1); }
                }
                else if (gs == GameState.EndGame)
                {
                    if (endContinueButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.Playing;
                        wantToContinue = true;
                    }
                    if (beforeExitButton.LeftClick(ms, prevMS))
                    {
                        gs = GameState.Exit;
                        Exit();
                    }
                    if (endContinueButton.MouseOver(ms)) { endContinueButton.ChangeState(2); } else { endContinueButton.ChangeState(1); }
                    if (beforeExitButton.MouseOver(ms)) { beforeExitButton.ChangeState(2); } else { beforeExitButton.ChangeState(1); }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Itt történik minden elem kirajzolása a képernyõre, szintén a játék állástól függöen
        /// Itt rajzoljuk ki a gombokat, hátteret, szövegeket, a pályát és az épületeket
        /// Draw metódus szintén meghivja magát minden lefutás után
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (gs == GameState.Menu)
            {

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                startButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
                loadButton.Draw(spriteBatch);
                if (cannotLoad)
                {
                    this.outputs.ForEach(x =>
                    {
                        if (x.Name == "load")
                        {
                            x.Draw(spriteBatch);
                        }
                    });
                }
            }
            else if (gs == GameState.Playing)
            {

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                map.Draw(spriteBatch);
                this.buildings.ForEach(x =>
                {
                    x.Draw(spriteBatch);
                });
                this.outputs.ForEach(x =>
                {
                    if (x.Name != "error" && x.Name != "load" && x.Name != "before" && x.Name != "ending")
                    {
                        x.Draw(spriteBatch);
                    }
                });

            }
            if (gs == GameState.Pause)
            {

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                continueButton.Draw(spriteBatch);
                saveButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }
            if (gs == GameState.Error)
            {
                spriteBatch.Draw(new Texture2D(GraphicsDevice, 400, 400), new Vector2(0, 0), Color.Multiply(Color.White, 100));
                this.outputs.FirstOrDefault(x => x.Name == "error").Draw(spriteBatch);
            }
            if (gs == GameState.BeforePlaying)
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                beforeStartButton.Draw(spriteBatch);
                beforeExitButton.Draw(spriteBatch);
                this.outputs.FirstOrDefault(x => x.Name == "before").Draw(spriteBatch);
            }
            if (gs == GameState.EndGame)
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                endContinueButton.Draw(spriteBatch);
                beforeExitButton.Draw(spriteBatch);
                this.outputs.FirstOrDefault(x => x.Name == "ending").Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
