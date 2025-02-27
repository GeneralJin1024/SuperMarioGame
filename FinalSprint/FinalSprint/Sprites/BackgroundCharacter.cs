﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using FinalSprint;
using System.Collections;

namespace FinalSprint.Background
{
    class BackgroundCharacter : ICharacter
    {
        private readonly ISprite Background;
        public MoveParameters Parameters { get; set; }

        public Sprint5Main.CharacterType Type { get;}

        //void Update(GameTime gameTime) { }
        public BackgroundCharacter(Texture2D texture, Vector2 pos)
        {
            Parameters = new MoveParameters(false);
            Parameters.SetPosition(pos.X, pos.Y);
            Background = new AnimatedSprite(texture, new Point(1, 1), Parameters);
        }
        public Vector2 GetHeightAndWidth
        {
            get{ return Background.GetHeightAndWidth; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
        }

        public void Update(float timeOfFrame) { }
        public void MarioCollide(bool specialCase) { }

        public Vector2 GetMinPosition { get { return new Vector2(); } }

        public Vector2 GetMaxPosition { get { return new Vector2(); } }

        public void BlockCollide(bool isBottom) { }

    }
}