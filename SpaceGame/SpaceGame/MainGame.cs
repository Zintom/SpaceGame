using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceGame.GameObjects;
using SpaceGame.Helpers;
using System;
using System.Security.Cryptography;
using SpaceGame.Shaders;

#nullable disable
namespace SpaceGame
{
    public class MainGame : Game
    {
        public static readonly RandomNumberGenerator RNG = RandomNumberGenerator.Create();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TickCounter frameCounter = new TickCounter();
        private TextureProvider _textureProvider;

        PlayingFieldManager _playingField;

        public static Texture2D Blank;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0d / 120d);
            IsFixedTimeStep = false;

            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            Blank = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            Blank.SetData(new Color[1] { Color.White });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _textureProvider = new TextureProvider(Content);

            _playingField = new PlayingFieldManager(_textureProvider, _graphics.GraphicsDevice.Viewport);
            SpriteBatchExtensions.LoadEffects(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!_playingField.InGame && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _playingField.StartGame();
            }

            _playingField.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            frameCounter.Tick(gameTime);

            _spriteBatch.Begin();
            _playingField.Draw(_spriteBatch);

            Window.Title = $"SpaceGame by Zintom - FPS: {frameCounter.TicksPerSecond}";

            // Draw points
            _spriteBatch.DrawString(_textureProvider.GameFont, _playingField.Points.ToString().PadLeft(2, '0'), new Vector2(13, 10), Color.White);

            // Draw lives
            for (int i = 0; i < _playingField.Lives; i++)
            {
                _spriteBatch.Draw(_textureProvider.Ship, new Rectangle(13 + (i * 20), 50, 20, 24), Color.White);
            }

            if (!_playingField.InGame)
            {
                string pushEnterText = "Push Enter";
                var textSize = _textureProvider.GameFont.MeasureString(pushEnterText);
                _spriteBatch.DrawString(
                        _textureProvider.GameFont,
                        pushEnterText,
                        new Vector2((int)(GraphicsDevice.Viewport.Width / 2 - textSize.X / 2), (int)(GraphicsDevice.Viewport.Height / 4 - textSize.Y / 2)),
                        Color.White);
            }

            //_spriteBatch.Draw(Blank, new Rectangle(0, 0, 10, 10), Color.White);
            _spriteBatch.End();



            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, null);

            //_spriteBatch.Draw(new Texture2D(GraphicsDevice, 45, 75), chumpPos, Color.White);
            //_spriteBatch.Draw(chump, chumpPos, Color.White);

            //_spriteBatch.DrawCircle(GraphicsDevice, chumpPos, 100);d
            //_spriteBatch.DrawLineFx(GraphicsDevice, new Vector2(150, 150), Mouse.GetState().Position.ToVector2(), Color.White);
            //_spriteBatch.DrawLine(new Vector2(150, 150), Mouse.GetState().Position.ToVector2(), 1, Color.White);

            //_spriteBatch.DrawCircle(new Vector2(150, 150) - new Vector2(10), 20, Color.White);
            //_spriteBatch.DrawCircle(Mouse.GetState().Position.ToVector2() - new Vector2(10), 20, 0.5f, Color.Red);


            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
