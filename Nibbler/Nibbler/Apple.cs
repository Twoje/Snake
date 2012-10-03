using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Nibbler
{
    class Apple
    {
        Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        Texture2D appleSprite;

        string ASSET_NAME = "appleBlock";

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    appleSprite.Width,
                    appleSprite.Height);
            }
        }

        public void LoadContent(ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            Random rand = new Random();
            appleSprite = contentManager.Load<Texture2D>(ASSET_NAME);
            int APPLE_POSITION_X = rand.Next(graphics.GraphicsDevice.Viewport.Width - appleSprite.Width),
                APPLE_POSITION_Y = rand.Next(graphics.GraphicsDevice.Viewport.Height - appleSprite.Height);
            position = new Vector2(APPLE_POSITION_X - (APPLE_POSITION_X % appleSprite.Width), APPLE_POSITION_Y - (APPLE_POSITION_Y % appleSprite.Width));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(appleSprite, position, Color.White);
        }
    }
}
