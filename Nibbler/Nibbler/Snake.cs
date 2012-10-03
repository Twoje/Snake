using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nibbler
{
    class Snake
    {
        List<Texture2D> body = new List<Texture2D>();
        List<Vector2> positions = new List<Vector2>();
        public List<Vector2> Positions
        {
            get
            {
                return positions;
            }
        }

        Direction direction = Direction.Down;
        Vector2 velocity = Vector2.Zero; // Used to change position
        double elapsedGameTime;

        SpriteFont font;
        public int score = 0;

        string ASSET_NAME = "snakeBlock";
        float START_POSITION_X = 64,
            START_POSITION_Y = 64;
        float MOVEMENT_SPEED = 16; // Affects how far the snake moves per frame (should keep it at size of snakeBlock sprite
        double frameSpeed = 50; // Affects speed of snake
        int START_LENGTH = 3; // Start length of snake

        public List<Rectangle> BoundingBox
        {
            get
            {
                List<Rectangle> boundingBox = new List<Rectangle>();
                for (int i = 0; i < body.Count; i++)
                {
                    boundingBox.Add(new Rectangle((int)positions[i].X,
                                                  (int)positions[i].Y,
                                                  body[i].Width,
                                                  body[i].Height));
                }
                return boundingBox;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            // Place snake at start position
            positions.Add(new Vector2(START_POSITION_X, START_POSITION_Y));
            
            // Load Snake head
            Texture2D head = contentManager.Load<Texture2D>(ASSET_NAME);
            body.Add(head);
            font = contentManager.Load<SpriteFont>("Score");

            for (int i = 0; i < START_LENGTH - 1; i++ )
            {
                positions.Add(new Vector2(START_POSITION_X, START_POSITION_Y - body[0].Height));
                body.Add(head);
            }

        }

        public void AddPart(ContentManager contentManager)
        {
            Texture2D part = contentManager.Load<Texture2D>(ASSET_NAME);
            body.Add(part);
            positions.Add(new Vector2(positions[positions.Count - 1].X, positions[positions.Count - 1].Y));
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            // Updates movement only every frameSpeed'th of a second
            elapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedGameTime >= frameSpeed)
            {
                direction = UpdateDirection();
                UpdateMovement(graphics);
                elapsedGameTime -= frameSpeed;
            }
        }

        private Direction UpdateDirection()
        {
            KeyboardState keyState = Keyboard.GetState();

            if(keyState.IsKeyDown(Keys.Up) && direction != Direction.Down)
                direction = Direction.Up;
            else if(keyState.IsKeyDown(Keys.Down) && direction != Direction.Up)
                direction = Direction.Down;
            else if(keyState.IsKeyDown(Keys.Left) && direction != Direction.Right)
                direction = Direction.Left;
            else if(keyState.IsKeyDown(Keys.Right) && direction != Direction.Left)
                direction = Direction.Right;

            return direction;
        }

        private void UpdateMovement(GraphicsDeviceManager graphics)
        {
            // Update velocity based on direction
            if (direction == Direction.Up)
                velocity = new Vector2(0, MOVEMENT_SPEED * -1);
            else if (direction == Direction.Down)
                velocity = new Vector2(0, MOVEMENT_SPEED);
            else if (direction == Direction.Left)
                velocity = new Vector2(MOVEMENT_SPEED * -1, 0);
            else if (direction == Direction.Right)
                velocity = new Vector2(MOVEMENT_SPEED, 0);

            // Resets the SNAKE if the snake crosses its own path
            for (int i = 2; i < positions.Count; i++)
                if (positions[0] + velocity == positions[i])
                {
                    positions[0] = new Vector2(START_POSITION_X, START_POSITION_Y);
                    body.RemoveRange(START_LENGTH, body.Count - START_LENGTH);
                    BoundingBox.RemoveRange(START_LENGTH, BoundingBox.Count - START_LENGTH);
                    positions.RemoveRange(START_LENGTH, positions.Count - START_LENGTH);
                    score = 0;
                }

            // Update position of snake
            for (int i = positions.Count - 1; i > 0; i--)
            {
                positions[i] = positions[i - 1];
            }

            positions[0] += velocity;

            // If player goes past edge of screen, start at other side of screen
            if (positions[0].X < 0 && direction == Direction.Left)
                positions[0] = new Vector2(graphics.GraphicsDevice.Viewport.Width - body[0].Width, positions[0].Y);
            else if (positions[0].X > graphics.GraphicsDevice.Viewport.Width - body[0].Width && direction == Direction.Right)
                positions[0] = new Vector2(0, positions[0].Y);
            else if (positions[0].Y < 0 && direction == Direction.Up)
                positions[0] = new Vector2(positions[0].X, graphics.GraphicsDevice.Viewport.Height - body[0].Height);
            else if (positions[0].Y > graphics.GraphicsDevice.Viewport.Height - body[0].Height && direction == Direction.Down)
                positions[0] = new Vector2(positions[0].X, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < body.Count; i++)
                spriteBatch.Draw(body[i], positions[i], Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.Black);
        }
    }
}
