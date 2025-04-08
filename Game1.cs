using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Game1 : Game
    {
        Texture2D PlayerTexture;
        Vector2 PlayerPosition;
        float PlayerSpeed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int frameHeight = 64;
        int frameWidth = 64;
        Point currentFrame = new Point(0, 0);
        Point spriteSize = new Point(8, 2);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 400);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            PlayerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            PlayerSpeed = 100;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            PlayerTexture = Content.Load<Texture2D>("PlayerFrames");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            /*float updatedPlayerSpeed = PlayerSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.S))
                PlayerPosition.Y += updatedPlayerSpeed;
            if (keyState.IsKeyDown(Keys.W))
                PlayerPosition.Y -= updatedPlayerSpeed;
            if (keyState.IsKeyDown(Keys.A))
                PlayerPosition.X -= updatedPlayerSpeed;
            if (keyState.IsKeyDown(Keys.D))
                PlayerPosition.X += updatedPlayerSpeed;

            if (PlayerPosition.Y > _graphics.PreferredBackBufferHeight - PlayerTexture.Height / 2)
                PlayerPosition.Y = _graphics.PreferredBackBufferHeight - PlayerTexture.Height / 2;
            if (PlayerPosition.Y < PlayerTexture.Height / 2)
                PlayerPosition.Y = PlayerTexture.Height / 2;
            if (PlayerPosition.X > _graphics.PreferredBackBufferWidth - PlayerTexture.Width / 2)
                PlayerPosition.X = _graphics.PreferredBackBufferWidth - PlayerTexture.Width / 2;
            if (PlayerPosition.X < PlayerTexture.Width / 2)
                PlayerPosition.X = PlayerTexture.Width / 2;
*/
            ++currentFrame.X;
            if (currentFrame.X > spriteSize.X)
            {
                currentFrame.X = 0;
                ++currentFrame.Y;
                if (currentFrame.Y > spriteSize.Y)
                    currentFrame.Y = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
           /* _spriteBatch.Draw(PlayerTexture, PlayerPosition,
                null, Color.White, 0f, new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);*/
            _spriteBatch.Draw(PlayerTexture, PlayerPosition,
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
