using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.GameObjects;
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

        public void RemoveBullet(Bullet bullet);

        public void AddObject(CommonGameObject obj);

        public void RemoveObject(CommonGameObject obj);

        public bool CollidesWithAsteroid(Bullet bullet, [NotNullWhen(true)] out Asteroid? asteroid);
    }

    public class PlayingFieldManager : IPlayingFieldManager
    {

        private readonly TextureProvider _textureProvider;
        private readonly Viewport _viewport;

        public int Level { get; private set; } = 1;

        private readonly List<Bullet> _bullets = new List<Bullet>();

        private readonly List<Asteroid> _asteroids = new List<Asteroid>();

        private readonly List<CommonGameObject> _fieldObjects = new List<CommonGameObject>();

        public PlayingFieldManager(TextureProvider textureProvider, Viewport viewport)
        {
            _textureProvider = textureProvider;
            _viewport = viewport;
            SpawnAsteroids();
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

            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i]?.Update(gameTime);
            }

            for (int i = 0; i < _fieldObjects.Count; i++)
            {
                _fieldObjects[i]?.Update(gameTime);
            }

            for (int i = 0; i < _asteroids.Count; i++)
            {
                _asteroids[i]?.Update(gameTime);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i]?.Draw(spriteBatch);
            }

            for (int i = 0; i < _fieldObjects.Count; i++)
            {
                _fieldObjects[i]?.Draw(spriteBatch);
            }

            for (int i = 0; i < _asteroids.Count; i++)
            {
                _asteroids[i]?.Draw(spriteBatch);
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

        void IPlayingFieldManager.RemoveBullet(Bullet bullet)
        {
            _bullets.Remove(bullet);
        }

        void IPlayingFieldManager.AddObject(CommonGameObject obj)
        {
            _fieldObjects.Add(obj);
        }

        void IPlayingFieldManager.RemoveObject(CommonGameObject obj)
        {
            _fieldObjects.Remove(obj);
        }

        bool IPlayingFieldManager.CollidesWithAsteroid(Bullet bullet, [NotNullWhen(true)] out Asteroid? asteroid)
        {
            var bulletBounds = new Rectangle(bullet.Position.ToPoint(), bullet.Size);

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
