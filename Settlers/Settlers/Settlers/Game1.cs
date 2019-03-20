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

        Map map;
        private MySqlConnectionHandler connector;
        #region Menu gombok
        Button startButton;
        Button exitButton;
        Button loadButton;
        #endregion

        #region Sz�net gombok
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
                    buildings = connector.GetBuilding();
                    string s = "";
                    foreach (var item in buildings)
                    {
                        s = item.GetTextureName(item.BuildingType);
                        item.Bounds = item.Rectangle;
                        item.Texture = Textures[s];
                    }
                    gs = GameState.Playing;

                    this.map = new Map();
                    this.map.InitTiles(Textures["grass"], Textures["tree"], Textures["stone"], Textures["buildingmenu"]);
                    this.GameMenuButtons = this.map.InitInGameMenu(Textures);
                }
                if (startButton.MouseOver(ms)) { startButton.ChangeState(2); } else { startButton.ChangeState(1); }
                if (exitButton.MouseOver(ms)) { exitButton.ChangeState(2); } else { exitButton.ChangeState(1); }
                if (loadButton.MouseOver(ms)) { loadButton.ChangeState(2); } else { loadButton.ChangeState(1); }

            }
            else if (gs == GameState.Playing)
            {
                string[] s = null;

                foreach (var item in GameMenuButtons)
                {
                    s = Textures.FirstOrDefault(x => x.Value == item.Texture).Key.Split('_');
                    if (item.MouseOver(ms)) { item.ChangeState(2); } else { item.ChangeState(1); }
                    if (item.LeftClick(ms, prevMS))
                    {
                        buildings.Add(new Building((buildings == null || buildings.Count == 0) ? 1 : buildings.Max(x => x.ID) + 1, new Rectangle(0, 0, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), Textures[s[1]], BuildingStatus.Placing, item.buildingType));
                    }
                }
                this.map.Update(ms, prevMS, GameMenuButtons, Textures);
                if (this.buildings.Count() != 0)
                {
                    this.buildings.ForEach(x => x.Update());

                    var asd = buildings.FirstOrDefault(x => x.Status == BuildingStatus.Placing);
                    if (asd != null)
                    {
                        if (ks.IsKeyDown(Keys.W) && map.CheckTile(Direction.Up, asd.Step(Direction.Up)))
                            asd.MoveBuilding(Direction.Up);
                        if (ks.IsKeyDown(Keys.S) && map.CheckTile(Direction.Down, asd.Step(Direction.Down)))
                            asd.MoveBuilding(Direction.Down);
                        if (ks.IsKeyDown(Keys.D) && map.CheckTile(Direction.Right, asd.Step(Direction.Right)))
                            asd.MoveBuilding(Direction.Right);
                        if (ks.IsKeyDown(Keys.A) && map.CheckTile(Direction.Left, asd.Step(Direction.Left)))
                            asd.MoveBuilding(Direction.Left);
                        if (ks.IsKeyDown(Keys.Enter))
                        {
                            map.PlaceBuilding(asd.Bounds);
                            asd.Status = BuildingStatus.Construction;
                        }
                    }

                }

                if (ks.IsKeyDown(Keys.Escape))
                    gs = GameState.Pause;

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
                    connector.InsertBuilding(buildings);
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
