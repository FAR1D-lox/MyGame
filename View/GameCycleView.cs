using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using System;
using System.Collections.Generic;

namespace MyGame.View
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<int, IObject> Objects = new();
        private Dictionary<int, Texture2D> Textures = new();

        private Vector2 VisualShift = Vector2.Zero;

        private IGameplayModel.Direction direction;
        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        public event EventHandler<GameTimeEventArgs> CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };

        public void LoadGameCycleParameters(Dictionary<int, IObject> Objects, Vector2 POWShift)
        {
            this.Objects = Objects;
            VisualShift += POWShift;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.ApplyChanges();
            VisualShift.X -= _graphics.PreferredBackBufferWidth * 0.5f;
            VisualShift.Y -= _graphics.PreferredBackBufferHeight * 0.7f;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Add((byte)Factory.ObjectTypes.player, Content.Load<Texture2D>("PlayerFrames"));
            Textures.Add((byte)Factory.ObjectTypes.enemy, Content.Load<Texture2D>("Square"));
            Textures.Add((byte)Factory.ObjectTypes.grass, Content.Load<Texture2D>("Grass"));
            Textures.Add((byte)Factory.ObjectTypes.dirt, Content.Load<Texture2D>("Dirt"));
            Textures.Add((byte)Factory.ObjectTypes.dirtNoSolid, Content.Load<Texture2D>("Dirt"));

        }

        protected override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Length > 0)
            {
                var k = keys[0];
                Keys k1 = new Keys();
                if (keys.Length == 2)
                {
                    k1 = keys[1];
                }
                switch (k)
                {
                    case Keys.Escape:
                        {
                            Exit();
                            break;
                        }
                    case Keys.Space:
                        {
                            if (k1 == Keys.A)
                            {
                                direction = IGameplayModel.Direction.leftUp;
                                PlayerMoved.Invoke
                                (
                                    this, new ControlsEventArgs
                                    {
                                        direction = IGameplayModel.Direction.leftUp
                                    }
                                );
                            }
                            else if (k1 == Keys.D)
                            {
                                direction = IGameplayModel.Direction.rightUp;
                                PlayerMoved.Invoke
                                (
                                    this, new ControlsEventArgs
                                    {
                                        direction = IGameplayModel.Direction.rightUp
                                    }
                                );
                            }
                            else
                            {
                                direction = IGameplayModel.Direction.up;
                                PlayerMoved.Invoke
                                (
                                    this, new ControlsEventArgs
                                    {
                                        direction = IGameplayModel.Direction.up
                                    }
                                );
                            }
                            break;
                        }
                    case Keys.A:
                        {
                            if (k1 == Keys.Space)
                            {
                                direction = IGameplayModel.Direction.leftUp;
                                PlayerMoved.Invoke
                                (
                                    this, new ControlsEventArgs
                                    {
                                        direction = IGameplayModel.Direction.leftUp
                                    }
                                );
                            }
                            else
                            {
                                direction = IGameplayModel.Direction.left;
                                PlayerMoved.Invoke
                                (
                                    this, new ControlsEventArgs
                                    {
                                        direction = IGameplayModel.Direction.left
                                    }
                                );
                            }
                            break;
                        }
                    case Keys.D:
                        {
                            if (k1 == Keys.Space)
                            {
                                direction = IGameplayModel.Direction.rightUp;
                                PlayerMoved.Invoke
                                (
                                    this, new ControlsEventArgs
                                    {
                                        direction = IGameplayModel.Direction.rightUp
                                    }
                                );
                            }
                            else
                            {
                                direction = IGameplayModel.Direction.right;
                                PlayerMoved.Invoke
                                (
                                    this, new ControlsEventArgs
                                    {
                                        direction = IGameplayModel.Direction.right
                                    }
                                );
                            }
                            break;
                        }
                }
            }


            base.Update(gameTime);
            CycleFinished.Invoke(this, new GameTimeEventArgs(gameTime));

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            base.Draw(gameTime);
            _spriteBatch.Begin();

            foreach (var obj in Objects.Values)
            {
                if (obj is IAnimation animatedObj)
                {
                    _spriteBatch.Draw
                    (
                        Textures[obj.ImageId],
                        obj.Pos - VisualShift,
                        animatedObj.Animate(Textures[obj.ImageId].Width, Textures[obj.ImageId].Height),
                        Color.White
                    );
                }
                else
                {
                    _spriteBatch.Draw
                    (
                        Textures[obj.ImageId],
                        obj.Pos - VisualShift,
                        Color.White
                    );
                }
            }
            _spriteBatch.End();
        }
    }
}
