﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FinalSprint.FactoryClasses;
using FinalSprint.ItemClasses;
using FinalSprint.LevelLoader;
using FinalSprint.MarioClasses;

namespace FinalSprint
{
    public class Scene : IDisposable
    {
        Stage stage;
        SpriteBatch spriteBatch;
        private int level;
        public ArrayList CharacterList { get; private set; }
        public ArrayList FireBallList { get; private set; }
        public Camera Camera { get; private set; }
        private List<Layer> Layers;
        public MarioCharacter Mario { get; internal set; }
        public Stage Stage
        {
            set { stage = value; }
            get { return stage; }
        }
        public Scene(Stage stage)
        {
            this.stage = stage;         
        }

        public void Add(ICharacter character)
        {
            CharacterList.Add(character);
        }
        public void Initalize(int levelIndex)
        {
            CharacterList = new ArrayList();
            FireBallList = new ArrayList();
            level = levelIndex;
            stage.Initialize();

        }

        public void LoadContent()
        {
            Camera = new Camera(Sprint5Main.Game.GraphicsDevice.Viewport);
            Layers = new List<Layer>
            {
                new Layer(Camera) { Parallax = new Vector2(0.2f, 1.0f) }, //cloud
                new Layer(Camera) { Parallax = new Vector2(0.8f, 1.0f) }, //hill
                new Layer(Camera) { Parallax = new Vector2(1.0f, 1.0f) }, //mario，bush
                new Layer(Camera) { Parallax = new Vector2(1.0f, 1.0f) }, //item，enemy，block
                new Layer(Camera) { Parallax = new Vector2(1.0f, 1.0f) } // fireball
            };

            stage.SpriteLocationReader(level, CharacterList, Layers);
            spriteBatch = new SpriteBatch(stage.Game.GraphicsDevice);
            Layers[2].Sprites.Add(Mario);
            Layers[3].Sprites = CharacterList;
            Layers[4].Sprites = FireBallList;
            stage.LoadContent(CharacterList, FireBallList);
            Camera.Limits = new Rectangle(0, 0, (int)stage.CameraBoundary.X, (int)stage.CameraBoundary.Y);


        }

        public void Update(GameTime gameTime)
        {
            stage.Update(gameTime);
            Camera.LookAt(Mario.Parameters.Position); // it should always look at mario
        }

        public void Draw()
        {
            Sprint5Main.Game.GraphicsDevice.Clear(stage.BackgroundColor);
            foreach (Layer layer in Layers)
                layer.Draw(spriteBatch);
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                spriteBatch.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static void CopyDataOfParameter(MoveParameters parameter, MoveParameters newParameter)
        {
            if (parameter is null || newParameter is null)
                throw new ArgumentNullException(nameof(parameter));
            newParameter.IsHidden = parameter.IsHidden;
            newParameter.IsLeft = parameter.IsLeft;
            newParameter.SetPosition(parameter.Position.X, parameter.Position.Y);
            newParameter.SetVelocity(Math.Abs(parameter.Velocity.X), parameter.Velocity.Y);
            newParameter.HasGravity = parameter.HasGravity;
        }

        public void DisableVPipes(ArrayList pipePosition)
        {
            if (pipePosition is null)
                throw new ArgumentNullException(nameof(pipePosition));
            foreach(ICharacter character in CharacterList)
            {
                if (pipePosition.Contains(character.GetMinPosition.X) && character.Type == Sprint5Main.CharacterType.Pipe)
                {
                    PipeCharacter pipe = (PipeCharacter)character;
                    pipe.MarioGetInside();
                }
            }
        }

        public void ByPassMario()
        {
            float XPosition = stage.CameraBoundary.X;
            //select the nearest VPipe which are on the right of Mario.
            foreach(ICharacter character in CharacterList)
            {
                if(character.Type == Sprint5Main.CharacterType.Pipe)
                {
                    PipeCharacter pipe = (PipeCharacter)character;
                    if ((pipe.PType == PipeCharacter.PipeType.VPipe) &&
                        (Mario.GetMaxPosition.X < pipe.GetMinPosition.X) && (pipe.GetMinPosition.X < XPosition))
                        XPosition = pipe.GetMinPosition.X;
                }
            }
            //Move Mario to the next VPipe
            Mario.Parameters.SetPosition(XPosition + 2, Mario.Parameters.Position.Y);
            //Add this pipe into list to disable later.
            Mario.DivedPipe.Add(XPosition);
        }

    }
}
