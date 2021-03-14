using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.GameObjects
{
    public class LargeAsteroid : Asteroid
    {
        public override Point Size { get; } = new Point(48, 48);

        protected override Texture2D _texture { get; init; }

        public LargeAsteroid(IPlayingFieldManager playingFieldManager, TextureProvider textureProvider, Viewport viewport) : base(playingFieldManager, textureProvider, viewport, 1f)
        {
            int textureChoice = randomGenerator.Next();

            _texture = textureChoice % 2 == 0 ? textureProvider.LargeAsteroid1 : textureProvider.LargeAsteroid2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(Position.ToPoint(), Size), Color.White);
        }
    }
}
