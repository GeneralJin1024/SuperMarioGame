﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FinalSprint
{
    public class MoveParameters
    {
        public bool IsLeft { get; set; } //whether the object is facing left.
        public bool IsHidden { get; set; } // whether display the object on the screen.
        public bool HasGravity { get; set; } //whether the object has grativity(negative y acceleration)
        public bool InScreen { get; set; }
        public Vector2 Velocity { get { return _velocity; } }
        public Vector2 Position { get { return _position; } }
        public Color GetColor { get { return ChangeColor ? Color.Yellow : Color.White; } }
        public bool Flash
        {
            get { _flash = ChangeColor ? !_flash : false; return _flash; }
        }
        private bool _flash;
        public float TimeOfFrame { get; set; } // replace game time. change one frame when TimeOfFrame = 1.
        public bool ChangeColor { get; set; }
        private Vector2 _velocity;
        private Vector2 _position;
        private float grativity;
        public MoveParameters(bool hasGravity)
        {
            //Set position and velocity to initial value
            _velocity = new Vector2(0, 0);
            _position = new Vector2(0, 0);
            //when the object is create, display and face to right.
            IsLeft = false;
            IsHidden = false;
            TimeOfFrame = 0; //start time is 0
            HasGravity = hasGravity;
            InScreen = true; ChangeColor = false;
        }

        public void SetVelocity(float x, float y)
        {
            //x and y are all absolute value of velocity. The direction of velocity depends on where the object is facing.
            if (IsLeft)
                _velocity.X = -1 * x;
            else
                _velocity.X = x;
            _velocity.Y = y;
        }

        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }
        public void UpdatePositionAndVelocity(float rate)
        {
            grativity = HasGravity ? 2 : 0;
            _position.X += (_velocity.X * rate);
            _position.Y += (_velocity.Y * rate);
            _velocity.Y += grativity * rate;
        }
    }
}
