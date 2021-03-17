using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Shaders
{
    public static class SpriteBatchExtensions
    {

        private static Texture2D? PixelTexture;

        /// <summary>
        /// A cache of textures for the DrawCircle function, the key being the diameter of the circle.
        /// </summary>
        private static readonly Dictionary<Point, Texture2D> TextureCache = new();
        private static readonly List<Point> TextureCacheKeys = new();
        private const int TextureCacheMaxSize = 10;

        public static Effect? PrimitiveEffects { get; set; }

        public static void LoadEffects(ContentManager content)
        {
            PrimitiveEffects = content.Load<Effect>("SpriteEffectTest");
        }

        private static void CreatePixelTexture(SpriteBatch spriteBatch)
        {
            PixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            PixelTexture.SetData(new Color[1] { Color.White });
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 position, float diameter, Color color)
        {
            DrawCircle(spriteBatch, position, diameter, 1, color);
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 position, float diameter, float depth, Color color)
        {
            if (PrimitiveEffects == null) throw new InvalidOperationException("You must call LoadEffects first.");

            float radius = diameter / 2;

            // Create a new texture which is the right size

            var size = new Point((int)diameter + 1, (int)diameter + 1);
            if (!TextureCache.TryGetValue(size, out Texture2D? tex))
            {
                tex = new Texture2D(spriteBatch.GraphicsDevice, (int)diameter + 1, (int)diameter + 1);
                TextureCache.Add(size, tex);
                TextureCacheKeys.Add(size);

                if (TextureCache.Count > TextureCacheMaxSize)
                {
                    Texture2D oldTexture = TextureCache[TextureCacheKeys[0]];

                    // Remove the oldest Texture2D from the cache.
                    TextureCache.Remove(TextureCacheKeys[0]);

                    // Dispose of it
                    oldTexture.Dispose();

                    // Remove the key from the keylist.
                    TextureCacheKeys.RemoveAt(0);
                }
            }

            PrimitiveEffects.Parameters["circleColor"].SetValue(color.ToVector4());
            PrimitiveEffects.Parameters["radius"].SetValue(radius);
            PrimitiveEffects.Parameters["diameter"].SetValue(diameter);

            PrimitiveEffects.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(tex, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, float thickness, float rotation, Vector2 origin, Color color)
        {
            if (PixelTexture == null)
            {
                CreatePixelTexture(spriteBatch);
            }

            float distance = Vector2.Distance(p1, p2);

            float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            spriteBatch.Draw(PixelTexture, p1, null, color, angle + rotation, Vector2.Zero, new Vector2(distance, thickness), SpriteEffects.None, 1);
        }

        [Obsolete("Really inefficient CPU heavy line drawing algorithm, use the other DrawLine function.")]
        public static void DrawLine(this SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Vector2 p1, Vector2 p2, Color color)
        {
            //Debug.WriteLine($"Width: {(int)Math.Abs(p2.X - p1.X)}, Height: {(int)Math.Abs(p2.Y - p1.Y)}");

            Point size = new Point((int)Math.Abs(p2.X - p1.X), (int)Math.Abs(p2.Y - p1.Y));

            if (size.X <= 0) size.X = 1;
            if (size.Y <= 0) size.Y = 1;

            Texture2D tex = new Texture2D(graphicsDevice, size.X, size.Y);

            Vector2 offset = new Vector2(p1.X < p2.X ? p1.X : p2.X, p1.Y < p2.Y ? p1.Y : p2.Y);

            p1 -= offset;
            p2 -= offset;

            Color[] colors = new Color[tex.Width * tex.Height];

            double angle = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
            Vector2 angleUnitVector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            StringBuilder builder = new("\n\n\n\nDrawn pixels:");

            Vector2 currentPosition = p1;
            while (true)
            {
                if (Vector2.Distance(p1, currentPosition) > Vector2.Distance(p1, p2)
                    || p1.X < 0
                    || p2.X < 0
                    || p1.Y < 0
                    || p2.Y < 0)
                {
                    break;
                }

                int pixelIndex = (int)currentPosition.Y * tex.Width + (int)currentPosition.X;
                if (pixelIndex > 0 && pixelIndex < colors.Length)
                {
                    ref Color pixel = ref colors[pixelIndex];
                    pixel.R = 255;
                    pixel.G = 255;
                    pixel.B = 255;
                    pixel.A = 255;

                    builder.Append($"X: {(int)currentPosition.X}({currentPosition.X}), Y: {(int)currentPosition.Y}({currentPosition.Y})\n");
                }

                // Walk vector
                currentPosition += angleUnitVector;
            }

            // To avoid weird artifact that appears only when p2.X < p1.X and p2.Y > p1.Y
            // Here we reset the artifact pixel back to blank.
            if (p2.X < p1.X && p2.Y > p1.Y)
            {
                // y * width + x
                int artifactPixelIndex = 1 * tex.Width + 0;
                if (colors.Length > artifactPixelIndex)
                {
                    ref Color pixel = ref colors[artifactPixelIndex];
                    pixel.R = 0;
                    pixel.G = 0;
                    pixel.B = 0;
                    pixel.A = 0;
                }
            }

            //Debug.WriteLine(builder);

            tex.SetData(colors);

            spriteBatch.Draw(tex, offset, Color.White);
        }

    }
}
