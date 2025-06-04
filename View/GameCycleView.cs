using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyGame.View
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<int, IMapObject> Objects = new();
        private Dictionary<int, Texture2D> Textures = new();

        private Vector2 VisualShift = Vector2.Zero;

        private Direction Direction;
        private MouseClick mouseLeftButtonState;

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        public event EventHandler<GameTimeEventArgs> CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };

        public void LoadGameCycleParameters(Dictionary<int, IMapObject> Objects, Vector2 POWShift)
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
            Textures.Add((byte)Factory.ObjectTypes.enemy, Content.Load<Texture2D>("EnemyFrames"));
            Textures.Add((byte)Factory.ObjectTypes.grass, Content.Load<Texture2D>("Grass"));
            Textures.Add((byte)Factory.ObjectTypes.dirt, Content.Load<Texture2D>("Dirt"));
            Textures.Add((byte)Factory.ObjectTypes.dirtNoSolid, Content.Load<Texture2D>("Dirt"));
            Textures.Add((byte)Factory.ObjectTypes.playerVerticalAttack, Content.Load<Texture2D>("SplashFrames"));
            Textures.Add((byte)Factory.ObjectTypes.playerHorisontalAttack, Content.Load<Texture2D>("SideAttackFrames"));
            Textures.Add((byte)Factory.ObjectTypes.enemyAttack, Content.Load<Texture2D>("EnemyAttackFrames"));
        }

        protected override void Update(GameTime gameTime)
        {
            mouseLeftButtonState = Controller.MouseController(Mouse.GetState().LeftButton);
            
            var keyBoardResult = Controller.KeyBoardController(Keyboard.GetState().GetPressedKeys());
            if (keyBoardResult is Direction direction)
                Direction = direction;
            else
                Exit();

            PlayerMoved.Invoke
            (
                this, new ControlsEventArgs
                {
                    Direction = Direction,
                    MouseLeftButtonState = mouseLeftButtonState
                }
            );

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
                if (obj is IAnimationObject animatedObj)
                {
                    _spriteBatch.Draw
                    (
                        Textures[obj.ImageId],
                        obj.Pos - VisualShift,
                        animatedObj.Animate(Textures[obj.ImageId].Width),
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
