using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.GameObjects
{
    public class Bullet : CommonGameObject
    {
        public override Point Size { get; } = new Point(2);

        private Vector2 _velocity;

        private IPlayingFieldManager _playingField;

        public Bullet(Viewport viewport, IPlayingFieldManager playingFieldManager, Vector2 position, Vector2 velocity) : base(viewport)
        {
            Position = position;
            _velocity = velocity;
            _playingField = playingFieldManager;
        }

        public override void Update(GameTime gameTime)
        {
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_playingField.CollidesWithAsteroid(this, out Asteroid? collided))
            {
                collided.Hit();
                _playingField.RemoveBullet(this);
            }

            bool outsideBounds = !_viewport.Bounds.Contains(Position);

            if (outsideBounds)
            {
                _playingField.RemoveObject(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MainGame.Blank, new Rectangle(Position.ToPoint(), Size), Color.White);
        }
    }
}
