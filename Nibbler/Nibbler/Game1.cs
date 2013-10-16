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

namespace Nibbler
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Snake player1;
        Apple apple;

        private bool paused = false;
        private bool pauseKeyDown = false;
        private bool deathPause = false;

        public Game1()
        {
            this.Window.Title = "Snake";
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player1 = new Snake();

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player1.LoadContent(this.Content);
            NewApple();
        }

        private void NewApple()
        {
            apple = new Apple();
            apple.LoadContent(this.Content, graphics);
            
            // Prevents apple from spawning on Snake
            foreach (Vector2 position in player1.Positions)
                if (position == apple.Position)
                    NewApple();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Allows the game to pause
            checkPauseKey(keyState);
            if (!paused)
                Simulate(gameTime);

            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        protected void Simulate(GameTime gameTime)
        {
            player1.Update(gameTime, graphics);
            if (player1.BoundingBox[0].Intersects(apple.BoundingBox))
            {
                player1.AddPart(this.Content);
                NewApple();
                player1.score += 10;
            }
        }

        /// <summary>
        /// Code for pausing game.
        /// </summary>
        /// <param name="userPause">True if user hits pause key</param>
        private void BeginPause(bool userPause)
        {
            paused = true;
            deathPause = !userPause;
        }

        private void EndPause()
        {
            paused = false;
        }

        private void checkPauseKey(KeyboardState keyState)
        {
            bool pauseKeyDownThisFrame = keyState.IsKeyDown(Keys.Escape);
            if (!pauseKeyDown && pauseKeyDownThisFrame)
            {
                if (!paused)
                    BeginPause(true);
                else
                    EndPause();
            }
            pauseKeyDown = pauseKeyDownThisFrame;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            apple.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
