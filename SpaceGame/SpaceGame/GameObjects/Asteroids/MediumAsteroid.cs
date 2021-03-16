using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.Helpers;

namespace SpaceGame.GameObjects.Asteroids
{
    public class MediumAsteroid : Asteroid
    {
        public override int Worth { get; } = 50;

        public override Point Size { get; } = new Point(40, 40);

        protected override Texture2D _texture { get; init; }

        public MediumAsteroid(Vector2 startPosition, IPlayingFieldManager playingFieldManager, TextureProvider textureProvider, Viewport viewport) : base(playingFieldManager, textureProvider, viewport, 0.1f)
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
            var chunk = new SmallAsteroid(Position, _playingFieldManager, _textureProvider, _viewport);
            var chunk2 = new SmallAsteroid(Position, _playingFieldManager, _textureProvider, _viewport);
            _playingFieldManager.AddAsteroid(chunk);
            _playingFieldManager.AddAsteroid(chunk2);

            _playingFieldManager.RemoveAsteroid(this);
        }
    }
}
