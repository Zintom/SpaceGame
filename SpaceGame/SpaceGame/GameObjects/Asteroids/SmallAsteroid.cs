using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.Helpers;

namespace SpaceGame.GameObjects.Asteroids
{
    public class SmallAsteroid : Asteroid
    {
        public override int Worth { get; } = 100;

        public override Point Size { get; } = new Point(24, 24);

        protected override Texture2D _texture { get; init; }

        public SmallAsteroid(Vector2 startPosition, IPlayingFieldManager playingFieldManager, TextureProvider textureProvider, Viewport viewport) : base(playingFieldManager, textureProvider, viewport, 0.1f)
        {
            Position = startPosition;

            int textureChoice = MainGame.RNG.NextRandom();

            _texture = textureChoice % 2 == 0 ? textureProvider.SmallAsteroid1 : textureProvider.SmallAsteroid2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(Position.ToPoint(), Size), Color.White);
        }

        public override void Hit()
        {
            _playingFieldManager.RemoveAsteroid(this);
        }
    }
}
