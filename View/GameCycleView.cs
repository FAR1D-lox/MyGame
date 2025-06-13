using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using MyGame.Model.Objects.MapObjects;
using MyGame.Model.ObjectTypes;
using MyGame.Presenter;
using System;
using System.Collections.Generic;
using System.Threading;
using static MyGame.Presenter.GameState;

namespace MyGame.View
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<int, ISolidObject> SolidObjects = new();
        private Dictionary<int, NoSolidObject> NoSolidObjects = new();
        private Dictionary<int, IAttackObject> AttackObjects = new();
        private Dictionary<int, BackgroundObject> BackgroundObjects = new();

        private Dictionary<int, IWindow> WindowObjects = new();
        private Dictionary<int, IButton> ButtonObjects = new();
        
        private Dictionary<int, Texture2D> Textures = new();

        private Vector2 VisualShift = Vector2.Zero;

        private ButtonState MouseLeftButtonState;
        private Vector2 MousePosition;

        private GameState GameState;

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<InputData> ControlInputStates = delegate { };

        public void LoadGameCycleParameters(
            Dictionary<int, ISolidObject> SolidObjects,
            Dictionary<int, NoSolidObject> NoSolidObjects,
            Dictionary<int, IAttackObject> AttackObjects,
            Dictionary<int, BackgroundObject> BackgroundObjects,
            Dictionary<int, IWindow> WindowObjects,
            Dictionary<int, IButton> ButtonObjects,
            Vector2 POWShift, GameState GameState)
        {
            this.SolidObjects = SolidObjects;
            this.NoSolidObjects = NoSolidObjects;
            this.AttackObjects = AttackObjects;
            this.BackgroundObjects = BackgroundObjects;

            this.WindowObjects = WindowObjects;
            this.ButtonObjects = ButtonObjects;
            if ((this.GameState == Menu || this.GameState == RestartWindow || this.GameState == Win) && GameState == Running)
                VisualShift = new Vector2(
                    -_graphics.PreferredBackBufferWidth * 0.15f,
                    _graphics.PreferredBackBufferHeight * 0.3f);
            else
                VisualShift += POWShift;
            this.GameState = GameState;
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
            Textures.Add((byte)Factory.ObjectTypes.stone, Content.Load<Texture2D>("Stone"));
            Textures.Add((byte)Factory.ObjectTypes.stoneNoSolid, Content.Load<Texture2D>("Stone"));
            Textures.Add((byte)Factory.ObjectTypes.playerVerticalAttack, Content.Load<Texture2D>("SplashFrames"));
            Textures.Add((byte)Factory.ObjectTypes.playerHorisontalAttack, Content.Load<Texture2D>("SideAttackFrames"));
            Textures.Add((byte)Factory.ObjectTypes.enemyAttack, Content.Load<Texture2D>("EnemyAttackFrames"));
            Textures.Add((byte)Factory.ObjectTypes.portal, Content.Load<Texture2D>("Portal"));
            Textures.Add((byte)Factory.ObjectTypes.loseWindow, Content.Load<Texture2D>("LoseWindow"));
            Textures.Add((byte)Factory.ObjectTypes.restartButton1, Content.Load<Texture2D>("RestartButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.continueButton, Content.Load<Texture2D>("ContinueButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.exitToMenuButton, Content.Load<Texture2D>("ExitButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.pauseButton, Content.Load<Texture2D>("PauseButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.pauseWindow, Content.Load<Texture2D>("PauseWindow"));
            Textures.Add((byte)Factory.ObjectTypes.beginGameButton, Content.Load<Texture2D>("BeginGameButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.leaveGameButton, Content.Load<Texture2D>("LeaveGameButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.winWindow, Content.Load<Texture2D>("WinWindow"));
            Textures.Add((byte)Factory.ObjectTypes.restartButton2, Content.Load<Texture2D>("RestartButton2Frames"));
            Textures.Add((byte)Factory.ObjectTypes.clouds, Content.Load<Texture2D>("Clouds"));
            Textures.Add((byte)Factory.ObjectTypes.sun, Content.Load<Texture2D>("Sun"));
            Textures.Add((byte)Factory.ObjectTypes.japanHouse, Content.Load<Texture2D>("JapanHouse"));
        }

        protected override void Update(GameTime gameTime)
        {
            MouseLeftButtonState = Mouse.GetState().LeftButton;
            MousePosition = Mouse.GetState().Position.ToVector2() + VisualShift;
            
            var keyBoardResult = Keyboard.GetState().GetPressedKeys();

            foreach (var button in ButtonObjects.Values)
            {
                button.CheckCursorHover(MousePosition);
            }

            ControlInputStates.Invoke
            (
                this, new InputData
                {
                    PressedKeys = keyBoardResult,
                    MouseLeftButtonState = MouseLeftButtonState,
                    MousePosition = MousePosition,
                }
            );

            base.Update(gameTime);
            CycleFinished.Invoke(this, new EventArgs());
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            base.Draw(gameTime);
            _spriteBatch.Begin();

            foreach (var obj in BackgroundObjects.Values)
            {
                AnimateMapObjects(obj);
            }
            foreach (var obj in NoSolidObjects.Values)
            {
                AnimateMapObjects(obj);
            }
            foreach (var obj in SolidObjects.Values)
            {
                AnimateMapObjects(obj);
            }
            foreach (var obj in AttackObjects.Values)
            {
                AnimateMapObjects(obj);
            }

            foreach (var obj in WindowObjects.Values)
            {
                DrawObject(new Rectangle(0, 0, obj.Width, obj.Height), obj);
            }
            foreach (var obj in ButtonObjects.Values)
            {
                DrawObject(obj.Animate(Textures[obj.ImageId].Width), obj);
            }
            
            _spriteBatch.End();
        }
        private void AnimateMapObjects(IMapObject obj)
        {
            if (GameState == Running && obj is IAnimationMapObject animatedObj)
                DrawObject(animatedObj.Animate(Textures[obj.ImageId].Width), obj);
            else
                DrawObject(new Rectangle(0, 0, obj.Width, obj.Height), obj);
        }

        private void DrawObject(Rectangle frame, IObject obj)
        {
            _spriteBatch.Draw
                (
                    Textures[obj.ImageId],
                    obj.Pos - VisualShift,
                    frame,
                    Color.White
                );
        }
        public void ExitGame()
        {
            Exit();
        }
    }
}
