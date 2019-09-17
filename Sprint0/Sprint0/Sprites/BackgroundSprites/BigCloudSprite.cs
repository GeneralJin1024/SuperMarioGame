﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using Sprint0;
using System.Collections;

namespace sprint0.Background
{
    class BigCloudSprite : AnimatedSprite
    {
        private ArrayList location;
        public BigCloudSprite(Texture2D texture) : base(texture, new Point(1, 1))
        {
            location = new ArrayList();
            location.Add(new Vector2(100, 50));
            location.Add(new Vector2(450, 50));
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 Location, bool isLeft)
        {
            foreach (Vector2 vector in location)
            {
                base.Draw(spriteBatch, vector, isLeft);
            }
        }
    }
}