using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.GameObjects
{
    public abstract class Asteroid : CommonGameObject
    {
        protected static readonly Random randomGenerator = new Random((int)DateTimeOffset.Now.ToUnixTimeMilliseconds());

        public abstract Point Size { get; }

        protected Vector2 Velocity { get; init; }

        protected abstract Texture2D _texture { get; init; }

        protected readonly TextureProvider _textureProvider;

        protected readonly IPlayingFieldManager _playingFieldManager;

        public Asteroid(IPlayingFieldManager playingFieldManager, TextureProvider textureProvider, Viewport viewport, float speed) : base(viewport)
        {
            // Random starting position
            Position = new Vector2((float)randomGenerator.NextDouble(), (float)randomGenerator.NextDouble());

            double angle = randomGenerator.Next() % MathHelper.ToRadians(360);

            Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;

            _playingFieldManager = playingFieldManager;
            _textureProvider = textureProvider;
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity;

            base.Update(gameTime);
        }

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
