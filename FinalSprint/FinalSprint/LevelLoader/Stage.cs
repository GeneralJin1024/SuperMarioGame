﻿using ConfigurationLibrary;
using Microsoft.Xna.Framework;
using FinalSprint.BlockClasses;
using FinalSprint.CollideDetection;
using FinalSprint.FactoryClasses;
using FinalSprint.MarioClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSprint.LevelLoader
{
    public class Stage
    {
        public Sprint5Main Game { get; set; }
        public static Vector2 Boundary { get; private set; }
        public static Vector2 MapBoundary { get; private set; } // In the whole game, The Tile Map Size are the same
        public Vector2 CameraBoundary { get; private set; } //The Boundary to limit cameras
        public Color BackgroundColor { get; private set; }
        readonly List<IController> controllerList;
        private int TimeSinceLastFrame;
        private int MillisecondsPerFrame;
        private CollisionDetector Collision;
        public bool Pulse { get; set; }
        public GraphicsDeviceManager GraphicsDevice
        {
            get { return Game.Graphics; }
        }

        public Stage (Sprint5Main game)
        {
            Game = game;
            controllerList = new List<IController>();           
        }

        public void Initialize()
        {
            //ConfigurationManager.RefreshSection("Configuration");
            //ConfigurationReaderAndWriter.ReadAllSettings();
            GraphicsDevice.PreferredBackBufferWidth = ConfigurationReaderAndWriter.ReadSetting("WindowWidth");
            //!= -1 ? ConfigurationReaderAndWriter.ReadSetting("WindowWidth") : graphicsDevice.GraphicsDevice.Viewport.Width;  // set this value to the desired width of your window
            GraphicsDevice.PreferredBackBufferHeight = ConfigurationReaderAndWriter.ReadSetting("WindowHeight");
            //!= -1 ? ConfigurationReaderAndWriter.ReadSetting("WindowHeight") : graphicsDevice.GraphicsDevice.Viewport.Height;   // set this value to the desired height of your window
            GraphicsDevice.ApplyChanges();
            MillisecondsPerFrame = 100;
            Boundary = new Vector2(GraphicsDevice.PreferredBackBufferWidth, GraphicsDevice.PreferredBackBufferHeight);
            MapBoundary = new Vector2(ConfigurationReaderAndWriter.ReadSetting("StageWidth"), ConfigurationReaderAndWriter.ReadSetting("StageHeight"));
            Pulse = false;
        }

        public void LoadContent(ArrayList spriteList, ArrayList fireBallList)
        {
            //nothing to do here           
            Collision = new CollisionDetector(Game.Scene.Mario, spriteList, fireBallList);
            controllerList.Add(new KeyboardController(Game.Scene.Mario, Game));
            controllerList.Add(new GamepadController(Game.Scene.Mario, Game));
        }
        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException(nameof(gameTime));
            foreach (IController controller in controllerList)
                controller.Update();
            if (!Pulse)
            {
                TimeSinceLastFrame += 2 * gameTime.ElapsedGameTime.Milliseconds;
                if (TimeSinceLastFrame > MillisecondsPerFrame)
                {
                    TimeSinceLastFrame -= MillisecondsPerFrame;
                    Collision.Update();
                }
            }

        }

        internal void SpriteLocationReader(int levelIndex, ArrayList spriteList, List<Layer> layers)
        {

            if (!((LevelSection)ConfigurationManager.GetSection("Level" + levelIndex) is LevelSection myLevelSection))
            {
                Console.WriteLine("Failed to load Level" + levelIndex);
            }
            else
            {
                CameraBoundary = StringToVecter2(myLevelSection.Stage[1].SpriteStartLocation);
                if (myLevelSection.Stage[2].SpriteName.Equals("Black", StringComparison.Ordinal))
                    BackgroundColor = Color.Black;
                else
                    BackgroundColor = Color.CornflowerBlue;
                Game.Scene.Mario = PlayerFactory.FactoryMethod2(myLevelSection.Player[1].SpriteName, 
                    StringToVecter2(myLevelSection.Player[1].SpriteStartLocation));
                for (int i = 1; i < myLevelSection.Backgrounds.Count; i++)
                {
                    BackgroundFactory.Instance.AddBackground(myLevelSection.Backgrounds[i].SpriteName, 
                        StringToVecter2(myLevelSection.Backgrounds[i].SpriteStartLocation), layers);
                    //backgroundList.Add(BackgroundFactory.Instance.FactoryMethod2(myLevelSection.Backgrounds[i].SpriteName, StringToVecter2(myLevelSection.Backgrounds[i].SpriteLocation)));
                }
                for (int i = 1; i < myLevelSection.Enemys.Count; i++)
                {
                    spriteList.AddRange(EnemyFactory.Instance.FactoryMethod(myLevelSection.Enemys[i].SpriteName,
                        StringToVecter2(myLevelSection.Enemys[i].SpriteStartLocation)));
                }
                for (int i = 1; i < myLevelSection.Items.Count; i++)
                {
                    spriteList.AddRange(ItemFactory.Instance.FactoryMethod(myLevelSection.Items[i].SpriteName, 
                        StringToVecter2(myLevelSection.Items[i].SpriteStartLocation), StringToVecter2(myLevelSection.Items[i].SpriteEndLocation)));
                }
                for (int i = 1; i < myLevelSection.Blocks.Count; i++)
                {
                    ArrayList blockList = new ArrayList();
                    ArrayList itemList = new ArrayList();
                    blockList.AddRange(BlockFactory.Instance.FactoryMethod(myLevelSection.Blocks[i].SpriteName, 
                        StringToVecter2(myLevelSection.Blocks[i].SpriteStartLocation), StringToVecter2(myLevelSection.Blocks[i].SpriteEndLocation)));
                    if (myLevelSection.Blocks[i].EmbeddedSpriteName.Length > 0)
                    {
                        itemList.AddRange(ItemFactory.Instance.FactoryMethod(myLevelSection.Blocks[i].EmbeddedSpriteName + "+{"
                            + myLevelSection.Blocks[i].EmbeddedSpriteNum + "}", StringToVecter2(myLevelSection.Blocks[i].SpriteStartLocation)));
                        foreach (BlockCharacter bc in blockList)
                        {
                            bc.LoadItems(itemList);
                        }
                        spriteList.AddRange(itemList);
                    }
                    spriteList.AddRange(blockList);                  
                }
            }
        }

        public static Vector2 CheckBoundary(Vector2 position, Vector2 heightAndWidth)
        {
            position.X = position.X >= 0 ? position.X : 0;
            position.Y = position.Y >= 0 ? position.Y : 0;
            position.X = position.X <= MapBoundary.X - heightAndWidth.Y ? position.X : MapBoundary.X - heightAndWidth.Y;
            position.Y = position.Y <= MapBoundary.Y - heightAndWidth.X ? position.Y : MapBoundary.Y - heightAndWidth.X;
            return position;
        }

        private Vector2 StringToVecter2(string pos)
        {
            int startInd = pos.IndexOf("X:", StringComparison.Ordinal) + 2;
            float aXPosition = float.Parse(pos.Substring(startInd, pos.IndexOf(" Y", StringComparison.Ordinal) - startInd), CultureInfo.CurrentCulture);
            startInd = pos.IndexOf("Y:", StringComparison.Ordinal) + 2;
            float aYPosition = float.Parse(pos.Substring(startInd, pos.IndexOf("}", StringComparison.Ordinal) - startInd), CultureInfo.CurrentCulture);
            return new Vector2(aXPosition, aYPosition);
        }

    }
}
