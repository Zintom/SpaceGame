using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.GameObjects;
using SpaceGame.GameObjects.Asteroids;
using SpaceGame.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public interface IPlayingFieldManager
    {
        public void AddAsteroid(Asteroid asteroid);

        public void RemoveAsteroid(Asteroid asteroid);

        public void AddBullet(Bullet bullet);
    }

    public class PlayingFieldManager : IPlayingFieldManager
    {

        private readonly TextureProvider _textureProvider;
        private readonly Viewport _viewport;

        public int Points { get; private set; } = 0;

        public int Level { get; private set; } = 1;

        public int Lives { get; private set; } = 0;

        public bool InGame { get; private set; } = false;

        public Ship Player { get; private set; }

        private readonly List<Bullet> _bullets = new List<Bullet>();

        private readonly List<Asteroid> _asteroids = new List<Asteroid>();

        public PlayingFieldManager(TextureProvider textureProvider, Viewport viewport)
        {
            _textureProvider = textureProvider;
            _viewport = viewport;
            SpawnAsteroids();

            Player = new Ship(_textureProvider, viewport, this);

            // Center the player in the screen.
            CenterObjectInScreen(Player);
        }

        public void StartGame()
        {
            Lives = 3;
            InGame = true;
        }

        private void CenterObjectInScreen(CommonGameObject obj)
        {
            obj.Position = new Vector2(_viewport.Width / 2 - obj.Size.X / 2,
                                          _viewport.Height / 2 - obj.Size.Y / 2);
        }

        private void NextLevel()
        {
            Level++;
            SpawnAsteroids();
        }

        private void SpawnAsteroids()
        {
            //TODO Level dependent code

            int largeCount = MainGame.RNG.NextRandom(4, 5);
            for(int i = 0; i < largeCount; i++)
            {
                _asteroids.Add(new LargeAsteroid(this, _textureProvider, _viewport));
            }
        }

        public void Update(GameTime gameTime)
        {
            if(_asteroids.Count == 0)
            {
                NextLevel();
                return;
            }

            // If we're not in-game, just update the asteroids floating about
            // as in the original game.
            if (!InGame)
            {
                for (int i = 0; i < _asteroids.Count; i++)
                {
                    _asteroids[i]?.Update(gameTime);
                }

                return;
            }

            Player.Update(gameTime);

            for (int i = 0; i < _asteroids.Count; i++)
            {
                _asteroids[i]?.Update(gameTime);
            }

            if (CollidesWithAsteroid(Player, out Asteroid? collidedWithPlayer))
            {
                Lives--;
                Player.Reset();

                if (Lives <= 0)
                {
                    InGame = false;
                    Points = 0;
                    Lives = 0;
                    Level = 1;
                    _asteroids.Clear();

                    return;
                }

                CenterObjectInScreen(Player);
            }

            for (int i = 0; i < _bullets.Count; i++)
            {
                Bullet bullet = _bullets[i];
                bullet.Update(gameTime);

                // Bullet collision
                if (CollidesWithAsteroid(bullet, out Asteroid? collidedWithBullet))
                {
                    Points += collidedWithBullet.Worth;
                    collidedWithBullet.Hit();
                    RemoveBullet(bullet);

                    continue;
                }

                // Delete the bullet if it flys out of bounds.
                bool outsideBounds = !_viewport.Bounds.Contains(bullet.Position);
                if (outsideBounds)
                {
                    RemoveBullet(bullet);
                    continue;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _asteroids.Count; i++)
            {
                _asteroids[i]?.Draw(spriteBatch);
            }

            if(!InGame)
            {
                return;
            }

            Player.Draw(spriteBatch);

            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i]?.Draw(spriteBatch);
            }
        }

        void IPlayingFieldManager.AddAsteroid(Asteroid asteroid)
        {
            _asteroids.Add(asteroid);
        }

        void IPlayingFieldManager.RemoveAsteroid(Asteroid asteroid)
        {
            _asteroids.Remove(asteroid);
        }

        void IPlayingFieldManager.AddBullet(Bullet bullet)
        {
            _bullets.Add(bullet);
        }

        public void RemoveBullet(Bullet bullet)
        {
            _bullets.Remove(bullet);
        }

        public bool CollidesWithAsteroid(CommonGameObject obj, [NotNullWhen(true)] out Asteroid? asteroid)
        {
            var bulletBounds = new Rectangle(obj.Position.ToPoint(), obj.Size);

            for (int i = 0; i < _asteroids.Count; i++)
            {
                var asteroidBounds = new Rectangle(_asteroids[i].Position.ToPoint(), _asteroids[i].Size);

                if (asteroidBounds.Intersects(bulletBounds))
                {
                    asteroid = _asteroids[i];
                    return true;
                }
            }

            asteroid = null;
            return false;
        }
    }
}
