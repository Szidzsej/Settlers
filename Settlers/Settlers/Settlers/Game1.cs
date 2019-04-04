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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private List<Button> GameMenuButtons = new List<Button>();
        private MouseState ms = new MouseState();
        private MouseState prevMS = new MouseState();
        private KeyboardState ks = new KeyboardState();
        private KeyboardState prevKs = new KeyboardState();
        private GameState gs = new GameState();
        private List<Building> buildings = new List<Building>();
        private Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private List<Output> outputs = new List<Output>();
        private int workers = Globals.STARTWORKERS;
        Dictionary<BaseMaterial, int> basematerials = new Dictionary<BaseMaterial, int>();
        private int yOutput = 410;

        Map map;
        private MySqlConnectionHandler connector;
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

        public Game1()
        {
            connector = new MySqlConnectionHandler();
            connector.TryOpen();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1100;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gs = GameState.Menu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Directory.GetFiles("Content/Textures", "*.xnb", SearchOption.AllDirectories).ToList().ForEach(x => Textures.Add(Path.GetFileNameWithoutExtension(x), Content.Load<Texture2D>(Path.Combine(Path.GetDirectoryName(x).Substring(8), Path.GetFileNameWithoutExtension(x)))));
            Directory.GetFiles("Content/Fonts","*.xnb",SearchOption.AllDirectories).ToList().ForEach(x=>fonts.Add(Path.GetFileNameWithoutExtension(x),Content.Load<SpriteFont>(Path.Combine(Path.GetDirectoryName(x).Substring(8), Path.GetFileNameWithoutExtension(x)))));
            startButton = new Button(100, 70, 200, 100, Textures["startNotP"], Textures["startP"]);
            exitButton = new Button(100, 340, 200, 100, Textures["exitNotP"], Textures["exitP"]);
            loadButton = new Button(100, 210, 200, 100, Textures["betoltesNotP"], Textures["betoltesP"]);
            continueButton = new Button(100, 70, 200, 100, Textures["folytatasNotP"], Textures["folytatasP"]);
            saveButton = new Button(100, 210, 200, 100, Textures["mentesNotP"], Textures["mentesP"]);

            outputs.Add(new Output("worker", $"Workers: {workers - (buildings.Count(x=> x.HasWorker == true))}/{workers}", new Vector2(910, yOutput), Color.Black, fonts["MyFont"]));
            basematerials = this.connector.GetBaseMaterial();
            foreach (var item in basematerials)
            {
                yOutput += 20;
                outputs.Add(new Output(item.Key.Name,$"{item.Key.Name}: {item.Value}",new Vector2(910, yOutput),Color.Black,fonts["MyFont"]));
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            prevMS = ms;
            ms = Mouse.GetState();
            prevKs = ks;
            ks = Keyboard.GetState();
            if (gs == GameState.Menu)
            {
                if (startButton.LeftClick(ms, prevMS))
                {
                    gs = GameState.Playing;

                    this.map = new Map();
                    this.map.InitTiles(Textures["grass"], Textures["tree"], Textures["stone"], Textures["buildingmenu"]);
                    this.GameMenuButtons = this.map.InitInGameMenu(Textures);
                }
                if (exitButton.LeftClick(ms, prevMS))
                {
                    gs = GameState.Exit;
                    Exit();
                }
                if (loadButton.LeftClick(ms,prevMS))
                {
                    #region Épület Betöltés
                    buildings = connector.GetBuilding();
                    string s = "";
                    foreach (var item in buildings)
                    {
                        s = item.GetTextureName(item.BuildingType);
                        item.Bounds = item.Rectangle;
                        item.Texture = Textures[s];
                    }
                    #endregion

                    basematerials = connector.GetSavedMaterial();
                    gs = GameState.Playing;

                    #region Pálya és a texturák betöltése
                    this.map = new Map();
                    map.Tiles = connector.GetTiles();
                    this.map.InitLoadedTiles(Textures["grass"], Textures["tree"], Textures["stone"], Textures["buildingmenu"]);
                    this.GameMenuButtons = this.map.InitInGameMenu(Textures);
                    #endregion
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
                                {
                                    canCreate = true;
                                }
                                else
                                {
                                    canCreate = false;
                                }
                                
                            }
                            if (material.Key.Name == "Stone")
                            {
                                if (material.Value >= bMID[1])
                                {
                                    canCreate = true;
                                }
                                else
                                {
                                    canCreate = false;
                                }
                            }
                        }
                        if (canCreate)
                        {
                            if (buildings != null || buildings.Count > 0)
                            {
                                foreach (var b in buildings)
                                {
                                    if ((int)b.Status == 0)
                                    {
                                        cannotCreate = true;
                                    }
                                }
                            }
                            if (!cannotCreate)
                            {
                                buildings.Add(new Building((buildings == null || buildings.Count == 0) ? 1 : buildings.Max(x => x.ID) + 1, new Rectangle(0, 0, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), Textures[s[1]], BuildingStatus.Placing, item.buildingType, false));
                                var wood = basematerials.FirstOrDefault(x => x.Key.Name == "Wood").Key;
                                if (basematerials.TryGetValue(wood, out int oldValue))
                                {
                                    basematerials[wood] = oldValue - bMID[0];
                                }
                                var stone = basematerials.FirstOrDefault(x => x.Key.Name == "Stone").Key;
                                if (basematerials.TryGetValue(stone, out oldValue))
                                {
                                    basematerials[stone] = oldValue - bMID[1];
                                }
                            }
                        }
                    }
                }
                #endregion
                this.map.Update(ms, prevMS, GameMenuButtons, Textures);
                #region Épület mozgatás
                if (this.buildings.Count() != 0)
                {
                    this.buildings.ForEach(x => x.Update());

                    var move = buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing);
                    if (move != null)
                    {
                        if (ks.IsKeyDown(Keys.W) && map.CheckTile(Direction.Up, move.Step(Direction.Up)))
                            move.MoveBuilding(Direction.Up);
                        if (ks.IsKeyDown(Keys.S) && map.CheckTile(Direction.Down, move.Step(Direction.Down)))
                            move.MoveBuilding(Direction.Down);
                        if (ks.IsKeyDown(Keys.D) && map.CheckTile(Direction.Right, move.Step(Direction.Right)))
                            move.MoveBuilding(Direction.Right);
                        if (ks.IsKeyDown(Keys.A) && map.CheckTile(Direction.Left, move.Step(Direction.Left)))
                            move.MoveBuilding(Direction.Left);
                        if (ks.IsKeyDown(Keys.Enter))
                        {
                            map.PlaceBuilding(move.Bounds);
                            buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing).Status = BuildingStatus.Construction;
                        }
                    }
                }
                #endregion
                if (ks.IsKeyDown(Keys.Escape))
                    gs = GameState.Pause;
                this.outputs.ForEach(x =>
                {
                    foreach (var item in basematerials)
                    {
                        if (item.Key.Name == x.Name)
                        {
                            x.Update(item.Key, item.Value);
                        }
                    }
                });
            }
            else if (gs == GameState.Pause)
            {
                if (continueButton.LeftClick(ms,prevMS))
                {
                    gs = GameState.Playing;
                }
                if (exitButton.LeftClick(ms, prevMS))
                {
                    gs = GameState.Exit;
                    Exit();
                }
                if(saveButton.LeftClick(ms,prevMS))
                {
                    #region Nyersanyagok mentése
                    if (basematerials.Count() !=0 && basematerials != null)
                    {
                        connector.DeleteMaterials();
                        foreach (var item in basematerials)
                        {
                            connector.SaveMaterials(item.Key.ID, item.Value);
                        }
                    }
                    #endregion
                    #region Épületek mentése
                    if (buildings.Count() !=0 && buildings != null)
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.DarkGray);
            if (gs == GameState.Menu)
            {
                startButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
                loadButton.Draw(spriteBatch);
            }
            else if (gs == GameState.Playing)
            {
                map.Draw(spriteBatch);
                this.buildings.ForEach(x =>
                {
                    x.Draw(spriteBatch);
                });
                this.outputs.ForEach(x =>
                {
                    x.Draw(spriteBatch);
                });
                
            }
            if (gs == GameState.Pause)
            {
                continueButton.Draw(spriteBatch);
                saveButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }


            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
