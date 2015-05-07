using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Timers;

namespace gyroMove
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int[] raw;
        Texture2D mano, cielo, mainTitle;
        SpriteFont punt;
        static Rectangle myPos;
        static Globos globos;
        public int puntuacion;

        enum GameState
        {
            MainMenu,
            Playing,
        }

        GameState CurrentGameState = GameState.MainMenu;
        cButtonPlay btnPlay;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            puntuacion = 0;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mano = Content.Load<Texture2D>("mano");
            cielo = Content.Load<Texture2D>("sky");
            mainTitle = Content.Load<Texture2D>("mainTitle");
            punt = Content.Load<SpriteFont>("puntuacion");
            globos = new Globos(Content);
            globos.initGlobos();

            myPos = new Rectangle(400, 300, 50, 50);
            btnPlay = new cButtonPlay(Content.Load<Texture2D>("playB"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(450,300));
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
        }

        bool checkExitKey(KeyboardState keyboardState, GamePadState gamePadState)
        {
            // Check to see whether ESC was pressed on the keyboard 
            // or BACK was pressed on the controller.
            if (keyboardState.IsKeyDown(Keys.Escape) ||
                gamePadState.Buttons.Back == ButtonState.Pressed)
            {
                 //Program.readThread.Abort();
                Program._continue = false;
                Program._serialPort.Close();
                Exit();
                return true;
            }
            return false;
        }

        protected override void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            // Check to see if the user has exited
            if (checkExitKey(keyboardState, gamePadState))
            {
                base.Update(gameTime);
                return;
            }

            MouseState mouse = Mouse.GetState();
            

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    IsMouseVisible = true;
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    
                    break;

                case GameState.Playing:
                    IsMouseVisible = false;
                    raw = Program.check();
                    int MaxX = graphics.GraphicsDevice.Viewport.Width - 50;
                    int MaxY = graphics.GraphicsDevice.Viewport.Height - 50;
           

                    if (raw[0] == 1)
                    {
                        if (myPos.X >= MaxX)
                        {
                            myPos.X = MaxX;
                        }
                        else
                        {
                            myPos.X += 1;
                        }
                    }
                    else if (raw[0] == 2)
                    {
                        if (myPos.X <= 0)
                        {
                            myPos.X = 0;
                        }
                        else
                        {
                            myPos.X -= 1;
                        }
                    }

                    if (raw[1] == 1)
                    {
                        if (myPos.Y >= MaxY)
                        {
                            myPos.Y = MaxY;
                        }
                        else
                        {
                            myPos.Y += 1;
                        }
                    }
                    else if (raw[1] == 2)
                    {
                        if (myPos.Y <= 0)
                        {
                            myPos.Y = 0;
                        }
                        else
                        {
                            myPos.Y -= 1;
                        }
                    }

                    foreach (Globo globo in globos.getBallons())
                    {
                        if (globo.getRect().Intersects(myPos))
                        {
                            puntuacion += 100;
                            globo.destroy();
                            globos.setGlobos();
                        }
                    }
                    
                    break;
            }

            base.Update(gameTime);
        }

        void Game1_Exiting(object sender, EventArgs e)
        {
            //Program.readThread.Abort();
            //Program._serialPort.Close();
            // Add any code that must execute before the game ends.
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(cielo, new Rectangle(0, 0, 800, 600), Color.White);
                    spriteBatch.Draw(mainTitle, new Vector2(70, 70), Color.White);
                    btnPlay.Draw(spriteBatch);
                    break;

                case GameState.Playing:
                    spriteBatch.Draw(cielo, new Rectangle(0,0,800,600), Color.White);
                    spriteBatch.Draw(mano, myPos, Color.White);
                    globos.drawGlobos(spriteBatch);
                    spriteBatch.DrawString(punt, ("Score: " + puntuacion.ToString()), new Vector2(200, 0), Color.Black);
                    break;
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
