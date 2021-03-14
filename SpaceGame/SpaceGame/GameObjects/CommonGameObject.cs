using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.GameObjects
{
    public abstract class CommonGameObject
    {

        public Vector2 Position { get; set; } = Vector2.Zero;

        public abstract Point Size { get; }

        protected readonly Viewport _viewport;

        public CommonGameObject(Viewport viewport)
        {
            _viewport = viewport;
        }

        public virtual void Update(GameTime gameTime)
        {
            KeepInViewPortBounds();
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        private void KeepInViewPortBounds()
        {
            if (Position.X < 0)
            {
                Position = new Vector2(_viewport.Width, Position.Y);
            }
            if (Position.Y < 0)
            {
                Position = new Vector2(Position.X, _viewport.Height);
            }
            if (Position.X > _viewport.Width)
            {
                Position = new Vector2(0, Position.Y);
            }
            if (Position.Y > _viewport.Height)
            {
                Position = new Vector2(Position.X, 0);
            }
        }

    }
}
