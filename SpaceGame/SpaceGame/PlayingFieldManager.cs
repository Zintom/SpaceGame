using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public interface IPlayingFieldManager
    {
        public void AddAsteroid(Asteroid asteroid);
    }

    public class PlayingFieldManager : IPlayingFieldManager
    {

        private static readonly Random randomGenerator = new Random((int)DateTimeOffset.Now.ToUnixTimeMilliseconds());

        private readonly TextureProvider _textureProvider;
        private readonly Viewport _viewport;

        public int Level { get; private set; } = 1;

        private readonly List<Asteroid> _asteroids = new List<Asteroid>();

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

            int largeCount = randomGenerator.Next(4, 5);
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

            foreach(var asteroid in _asteroids)
            {
                asteroid.Update(gameTime);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var asteroid in _asteroids)
            {
                asteroid.Draw(spriteBatch);
            }
        }

        void IPlayingFieldManager.AddAsteroid(Asteroid asteroid)
        {
            _asteroids.Add(asteroid);
        }
    }
}
