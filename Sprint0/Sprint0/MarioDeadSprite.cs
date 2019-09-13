﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    class MarioDeadSprite : ISprite
    {
        private SpriteBatch spriteBatch;
        private Texture2D frame;
        private Vector2 spritePosition;
        private int moveDirectionY = -1;
      
        public MarioDeadSprite(Vector2 position, SpriteBatch batch, Texture2D f)
        {
            spriteBatch = batch;
            frame = f;
            Visibility = false;
            this.spritePosition = new Vector2(0, 0);
            this.spritePosition.X = position.X;
            this.spritePosition.Y = position.Y;
        }

        public bool Visibility { get; set; }

        public void Draw()
        {
            spriteBatch.Draw(frame, this.spritePosition, Color.White);        
        }

        public void Update(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            this.spritePosition.Y += this.moveDirectionY * 10;
            if (this.spritePosition.Y <= 10 || this.spritePosition.Y >= 3 * graphicsDevice.Viewport.Height / 4 - 10)
                this.moveDirectionY *= -1;
        }

        public void SwitchVisibility()
        {
            this.Visibility = !this.Visibility;
        }
    }
}
