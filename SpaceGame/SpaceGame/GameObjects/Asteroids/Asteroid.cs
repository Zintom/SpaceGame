using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.Helpers;
using System;

namespace SpaceGame.GameObjects.Asteroids
{
    public abstract class Asteroid : CommonGameObject
    {

        /// <summary>
        /// How many points this asteroid is worth when destroyed.
        /// </summary>
        public abstract int Worth { get; }

        protected Vector2 Velocity { get; init; }

        protected abstract Texture2D _texture { get; init; }

        protected readonly TextureProvider _textureProvider;

        protected readonly IPlayingFieldManager _playingFieldManager;

        public Asteroid(IPlayingFieldManager playingFieldManager, TextureProvider textureProvider, Viewport viewport, float speed) : base(viewport)
        {
            double angle = MainGame.RNG.NextRandom();// % MathHelper.ToRadians(360);

            Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;

            _playingFieldManager = playingFieldManager;
            _textureProvider = textureProvider;
        }

        public abstract void Hit();

        public override void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

    }
}
