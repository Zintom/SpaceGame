using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceGame.GameObjects;
using SpaceGame.Helpers;
using System;
using System.Security.Cryptography;

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
        Ship _player;

        public static Texture2D Blank;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0d / 120d);
            IsFixedTimeStep = false;

            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.SynchronizeWithVerticalRetrace = false;
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
            _player = new Ship(_textureProvider, _graphics.GraphicsDevice.Viewport, _playingField);

            // Center the player in the screen.
            _player.Position = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - _player.Size.X / 2, 
                                          _graphics.GraphicsDevice.Viewport.Height / 2 - _player.Size.Y / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _playingField.Update(gameTime);
            _player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            frameCounter.Tick(gameTime);

            _spriteBatch.Begin();
            _playingField.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);

            _spriteBatch.DrawString(_textureProvider.GameFont, "FPS: " + frameCounter.TicksPerSecond, new Vector2(10, 10), Color.White);

            //_spriteBatch.Draw(Blank, new Rectangle(0, 0, 10, 10), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
