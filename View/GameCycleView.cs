using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using MyGame.Model.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyGame.View
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<int, IMapObject> MapObjects = new();
        private Dictionary<int, ILabel> LabelObjects = new();
        private Dictionary<int, IButton> ButtonObjects = new();
        private Dictionary<int, Texture2D> Textures = new();

        private Vector2 VisualShift = Vector2.Zero;

        private ButtonState MouseLeftButtonState;
        private Vector2 MousePosition;

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<InputData> ControlInputStates = delegate { };

        public void LoadGameCycleParameters(Dictionary<int, IMapObject> MapObjects, Dictionary<int, ILabel> LabelObjects, Dictionary<int, IButton> ButtonObjects, Vector2 POWShift)
        {
            this.MapObjects = MapObjects;
            this.LabelObjects = LabelObjects;
            this.ButtonObjects = ButtonObjects;
            if (POWShift == new Vector2(float.MinValue, float.MinValue))
                VisualShift = new Vector2(
                    -_graphics.PreferredBackBufferWidth * 0.5f,
                    -_graphics.PreferredBackBufferHeight * 0.2f);
            else
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
            Textures.Add((byte)Factory.ObjectTypes.stone, Content.Load<Texture2D>("Stone"));
            Textures.Add((byte)Factory.ObjectTypes.stoneNoSolid, Content.Load<Texture2D>("Stone"));
            Textures.Add((byte)Factory.ObjectTypes.playerVerticalAttack, Content.Load<Texture2D>("SplashFrames"));
            Textures.Add((byte)Factory.ObjectTypes.playerHorisontalAttack, Content.Load<Texture2D>("SideAttackFrames"));
            Textures.Add((byte)Factory.ObjectTypes.enemyAttack, Content.Load<Texture2D>("EnemyAttackFrames"));
            Textures.Add((byte)Factory.ObjectTypes.loseWindow, Content.Load<Texture2D>("LoseWindow"));
            Textures.Add((byte)Factory.ObjectTypes.restartButton, Content.Load<Texture2D>("RestartButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.continueButton, Content.Load<Texture2D>("ContinueButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.exitToMenuButton, Content.Load<Texture2D>("ExitButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.pauseButton, Content.Load<Texture2D>("PauseButtonFrames"));
            Textures.Add((byte)Factory.ObjectTypes.pauseWindow, Content.Load<Texture2D>("PauseWindow"));
        }

        protected override void Update(GameTime gameTime)
        {
            MouseLeftButtonState = Mouse.GetState().LeftButton;
            MousePosition = Mouse.GetState().Position.ToVector2() + VisualShift;
            
            var keyBoardResult = Keyboard.GetState().GetPressedKeys();

            ControlInputStates.Invoke
            (
                this, new InputData
                {
                    PressedKeys = keyBoardResult,
                    MouseLeftButtonState = MouseLeftButtonState,
                    MousePosition = MousePosition
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

            foreach (var obj in MapObjects.Values)
            {
                if (obj is IMapObject)
                {
                    if (obj is IAnimationMapObject animatedObj)
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
            }
            foreach (var obj in LabelObjects.Values)
            {
                _spriteBatch.Draw
                (
                    Textures[obj.ImageId],
                    obj.Pos - VisualShift,
                    Color.White
                );
            }
            foreach (var obj in ButtonObjects.Values)
            {
                _spriteBatch.Draw
                (
                    Textures[obj.ImageId],
                    obj.Pos - VisualShift,
                    obj.Animate(Textures[obj.ImageId].Width),
                    Color.White
                );
            }
            _spriteBatch.End();
        }
    }
}
