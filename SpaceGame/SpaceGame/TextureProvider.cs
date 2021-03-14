using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public class TextureProvider
    {

        public Texture2D Ship { get; private set; }

        public Texture2D LargeAsteroid1 { get; private set; }
        public Texture2D LargeAsteroid2 { get; private set; }

        public Texture2D SmallAsteroid1 { get; private set; }
        public Texture2D SmallAsteroid2 { get; private set; }

        public SpriteFont GameFont { get; private set; }

        public TextureProvider(ContentManager content)
        {
            Ship = content.Load<Texture2D>("spaceship");

            LargeAsteroid1 = content.Load<Texture2D>("large_asteroid_1");
            LargeAsteroid2 = content.Load<Texture2D>("large_asteroid_2");

            SmallAsteroid1 = content.Load<Texture2D>("small_asteroid_1");
            SmallAsteroid2 = content.Load<Texture2D>("small_asteroid_2");

            GameFont = content.Load<SpriteFont>("GameFont");
        }

    }
}
