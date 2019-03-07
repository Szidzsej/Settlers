using System;
using System.Collections.Generic;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
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
        private List<Building> Buildings = new List<Building>();
        Map map;

        Button startButton;
        Button exitButton;
        public Game1()
        {
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
            Directory.GetFiles("Content", "*.xnb", SearchOption.AllDirectories).ToList().ForEach(x => Textures.Add(Path.GetFileNameWithoutExtension(x), Content.Load<Texture2D>(Path.Combine(Path.GetDirectoryName(x).Substring(8), Path.GetFileNameWithoutExtension(x)))));

            startButton = new Button(100, 70, 200, 100, Textures["startNotP"], Textures["startP"]);
            exitButton = new Button(100, 340, 200, 100, Textures["exitNotP"], Textures["exitP"]);
            if (gs== GameState.Playing)
            {
                
            }
            
        }

        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


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
                    this.map.InitTiles(Textures["grass"], Textures["tree"], Textures["stone"],Textures["buildingmenu"]);
                    this.GameMenuButtons = this.map.InitInGameMenu(Textures);
                }
                if (exitButton.LeftClick(ms, prevMS))
                {
                    gs = GameState.Exit;
                    Exit();
                }
                if (startButton.MouseOver(ms)) { startButton.ChangeState(2); } else { startButton.ChangeState(1); }
                if (exitButton.MouseOver(ms)) { exitButton.ChangeState(2); } else { exitButton.ChangeState(1); }


            }
            else if (gs == GameState.Playing)
            {
                this.map.Update(ms, prevMS, GameMenuButtons,Textures);
                if (ks.IsKeyDown(Keys.W))
                    map.MoveBuilding(Direction.Up,Buildings.Count());
                if (ks.IsKeyDown(Keys.S))
                    map.MoveBuilding(Direction.Down, Buildings.Count());
                if (ks.IsKeyDown(Keys.D) )
                    map.MoveBuilding(Direction.Right, Buildings.Count());
                if (ks.IsKeyDown(Keys.A))
                    map.MoveBuilding(Direction.Left, Buildings.Count());
                if (ks.IsKeyDown(Keys.Enter))
                {
                    map.PlaceBuilding();
                }
            }
            else if (gs == GameState.Pause)
            {
                
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
            }
            else if (gs == GameState.Playing)
            {
                map.Draw(spriteBatch);
            }
            if (gs == GameState.Pause)
            {
                startButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }


            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
