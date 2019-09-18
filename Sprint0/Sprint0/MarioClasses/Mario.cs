﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace Sprint0.MarioClasses
{
    class Mario : ISprite
    {
        public Texture2D SpriteSheets { get; set; }//useless variable

        public enum ActionType {
            [Description ("Crouch")]
            Crouch,
            [Description("Other")]
            Other
        }

        public enum PowerType {
            [Description("Standard")]
            Standard,
            [Description("Super")]
            Super,
            [Description("Died")]
            Died
        }

        #region Textures
        //{Stand, Jump, Walk, Crouch}
        private Texture2D[] StandardMario;
        private Texture2D[] SuperMario;
        private Texture2D[] FireMario;
        #endregion Textures

        private ActionType actionType;
        private PowerType powerType;

        #region ActionSprites
        private ISprite IdleSprite;
        private ISprite JumpSprite;
        private ISprite CrouchSprite;
        private ISprite WalkingSprite;
        private ISprite DiedSprite;
        private ISprite currentMarioAction;
        #endregion ActionSprites

        #region Action States
        private ActionState CurrentAction;
        //{Idle, Jump, Walk, RunningJump, Crouch}
        private ActionState[] ActionStates;
        #endregion Action States

        #region PowerState
        private PowerState CurrentPower;
        //{Standard, Super, Fire, Died}
        private PowerState[] PowerStates;
        #endregion PowerState

        public bool IsLeft { get; set; }

        public Vector2 Position
        {
            get
            {
                return Location;
            }
        }

        private Vector2 Location;

        public Mario(Texture2D[] standardSheets, Texture2D[] superSheet, 
            Texture2D[] fireSheet, Texture2D diedSheet, Vector2 location)
        {
            StandardMario = standardSheets;
            SuperMario = superSheet;
            FireMario = fireSheet;
            DiedSprite = new AnimatedSprite(diedSheet, new Point(1, 1));
            SetActionSprites();
            SetActionStates();
            SetPowerStates();
            IsLeft = false;
            actionType = ActionType.Other;
            powerType = PowerType.Standard;
            Location = location;
        }
        #region ISprite Methods
        public void Update(GameTime gameTime)
        {
            currentMarioAction.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, bool isLeft)
        {
            currentMarioAction.Draw(spriteBatch, Location, IsLeft);
        }

        public Vector2 GetHeightAndWidth()
        {
            return currentMarioAction.GetHeightAndWidth();
        }
        #endregion ISprite Methods

        #region Action Command Receiver Method
        public void MoveRight() {
            if (powerType != PowerType.Died)
                CurrentAction.Right(this);
        }

        public void MoveLeft() {
            if (powerType != PowerType.Died)
                CurrentAction.Left(this);
        }

        public void MoveUp() {
            if (powerType != PowerType.Died)
                CurrentAction.Up(this);
        }

        public void MoveDown() {
            if (powerType != PowerType.Died)
                CurrentAction.Down(this);
        }
        #endregion Action Command Receiver Method

        #region Action Change
        public void ChangeToIdle()
        {
            CurrentAction = ActionStates[0];
            currentMarioAction = IdleSprite;
            if (powerType == PowerType.Super && actionType == ActionType.Crouch)
                // The difference of height between standing and crouch.
                Location.Y -= 10;
            actionType = ActionType.Other;
        }

        public void ChangeToJump()
        {
            CurrentAction = ActionStates[1];
            currentMarioAction = JumpSprite;
        }

        public void ChangeToWalk()
        {
            CurrentAction = ActionStates[2];
            currentMarioAction = WalkingSprite;
        }

        public void ChangeToCrouch()
        {
            CurrentAction = ActionStates[4];
            currentMarioAction = CrouchSprite;
            if (powerType == PowerType.Super)
                //The difference of height between standing and crouch.
                Location.Y += 10;
            actionType = ActionType.Crouch;
        }

        public void ChangeToRunningJump()
        {
            CurrentAction = ActionStates[3];
            currentMarioAction = JumpSprite;
        }
        #endregion Action Change

        #region Power Command Receiver Method
        // try update
        public void MoveStandard() { ChangeToStandard();
        }
        public void MoveSuper() { ChangeToSuper();
            }
        public void MoveFire() { ChangeToFire();
        }
        public void MoveDestroy() { CurrentPower.Destroy(this);
        }
        #endregion Power Command Receiver Method

        #region Power Change
        public void ChangeToSuper()
        {
            CurrentPower = PowerStates[1];
            IdleSprite.SpriteSheets = SuperMario[0];
            JumpSprite.SpriteSheets = SuperMario[1];
            WalkingSprite.SpriteSheets = SuperMario[2];
            CrouchSprite.SpriteSheets = SuperMario[3];

            if(powerType  != PowerType.Super)
            {
                Location.Y -=16;
                powerType = PowerType.Super;
            }

        }
        public void ChangeToStandard()
        {
            if(actionType == ActionType.Crouch)  
                ChangeToIdle();
            CurrentPower = PowerStates[0];
            IdleSprite.SpriteSheets = StandardMario[0];
            JumpSprite.SpriteSheets = StandardMario[1];
            WalkingSprite.SpriteSheets = StandardMario[2];
            CrouchSprite.SpriteSheets = StandardMario[3];
            
            if(powerType == PowerType.Died)

                currentMarioAction = IdleSprite;

            else if (powerType ==PowerType.Super)
            {
                Location.Y +=16;
            }
            powerType = PowerType.Standard;

        }
        public void ChangeToFire()
        {
            CurrentPower = PowerStates[2];
            IdleSprite.SpriteSheets = FireMario[0];
            JumpSprite.SpriteSheets = FireMario[1];
            WalkingSprite.SpriteSheets = FireMario[2];
            CrouchSprite.SpriteSheets = FireMario[3];

            if(powerType ==PowerType.Died)
                currentMarioAction = IdleSprite;
            if(powerType != PowerType.Super)
            {
                Location.Y -=16;
                powerType = PowerType.Super;
            }

        }
        public void ChangeToDied()
        {
            CurrentPower = PowerStates[3];
            currentMarioAction = DiedSprite;
            if(powerType == PowerType.Super)
                Location.Y +=16;
            powerType = PowerType.Died;
        }
        #endregion Power Change

        // give Block to justify
       public bool IsSuper()
        {
            return (powerType == PowerType.Super);
        }


        private void SetActionSprites()
        {
            IdleSprite = new AnimatedSprite(StandardMario[0], new Point(1, 1));
            JumpSprite = new AnimatedSprite(StandardMario[1], new Point(1, 1));
            WalkingSprite = new AnimatedSprite(StandardMario[2], new Point(1, 3));
            CrouchSprite = new AnimatedSprite(StandardMario[3], new Point(1, 1));
            currentMarioAction = IdleSprite;
        }

        private void SetActionStates()
        {
            ActionStates = new ActionState[5] { new IdleState(), new JumpState(), new WalkState(), new RunningJumpState(), new CrouchState() };
            CurrentAction = ActionStates[0];
        }

        private void SetPowerStates()
        {
            PowerStates = new PowerState[4] {new StandardState(), new SuperState(), new FireState(), new DiedState() };
            CurrentPower = PowerStates[0];
        }

    }
}
