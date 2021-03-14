using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.Helpers;

namespace SpaceGame.GameObjects
{
    public class LargeAsteroid : Asteroid
    {
        public override Point Size { get; } = new Point(48, 48);

        protected override Texture2D _texture { get; init; }

        public LargeAsteroid(IPlayingFieldManager playingFieldManager, TextureProvider textureProvider, Viewport viewport) : base(playingFieldManager, textureProvider, viewport, 0.05f)
        {
            // Random starting position
            Position = new Vector2(MainGame.RNG.NextRandom(0, viewport.Width), MainGame.RNG.NextRandom(0, viewport.Height));

            int textureChoice = MainGame.RNG.NextRandom();

            _texture = textureChoice % 2 == 0 ? textureProvider.LargeAsteroid1 : textureProvider.LargeAsteroid2;
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
