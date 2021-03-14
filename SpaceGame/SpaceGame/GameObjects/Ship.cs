using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.GameObjects
{
    public class Ship : CommonGameObject
    {

        private readonly Texture2D _texture;

        public static readonly Point Size = new Point(20, 24);

        private static Vector2 Origin;

        /// <summary>
        /// The rotation of the "ship", in Radians (0 == Right).
        /// </summary>
        private float _vehicleRotation = 0f;

        /// <summary>
        /// The velocity of the ship.
        /// </summary>
        private Vector2 _momentumVector = Vector2.Zero;

        private readonly float TopSpeed = 5f;

        private readonly float Thrust = 0.1f;

        public Ship(TextureProvider textureProvider, Viewport viewport) : base(viewport)
        {
            _texture = textureProvider.Ship;
            Origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            //TODO use delta time!
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A))
            {
                _vehicleRotation -= 0.1f;
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                _vehicleRotation += 0.1f;
            }

            // Add thrust velocity is space is being pressed.
            if (keyState.IsKeyDown(Keys.W))
            {
                // '0' in radians points right, in order to thrust upwards to we take 90 degrees off the rotation.
                float thrustDirection = _vehicleRotation - MathHelper.ToRadians(90);

                // Convert the thrust direction into a unit vector so that it
                // can be added to the momentum vector.
                var thrustUnitVector = new Vector2((float)Math.Cos(thrustDirection), (float)Math.Sin(thrustDirection));

                _momentumVector += thrustUnitVector * Thrust;
            }
            // Remove velocity if space is not being pressed.
            else
            {
                _momentumVector = Vector2.Lerp(_momentumVector, Vector2.Zero, 0.01f);
                
                // Round down if X or Y is less than 0.1f otherwise you get tiny little jumps in pixels
                // even when you appear stationary.
                if (Math.Abs(_momentumVector.X) <= 0.1f) _momentumVector.X = 0;
                if (Math.Abs(_momentumVector.Y) <= 0.1f) _momentumVector.Y = 0;
            }

            // The Speed Cap Vector = Momentum Unit Vector * Top Speed
            Vector2 speedCap = Vector2.Normalize(_momentumVector) * TopSpeed;

            if (_momentumVector.Length() > speedCap.Length())
            {
                _momentumVector = speedCap;
            }

            Position += _momentumVector;

            Debug.WriteLine($"Momentum Vector: {Vector2.Round(_momentumVector)}, Speed:  {_momentumVector.Length()}");

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(MainGame.Blank, new Rectangle(Position.ToPoint(), Size), null, Color.Red, _vehicleRotation, new Vector2(MainGame.Blank.Width / 2f, MainGame.Blank.Height / 2f), SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, new Rectangle(Position.ToPoint(), Size), null, Color.White, _vehicleRotation, Origin, SpriteEffects.None, 1);
            
            // Draw momentum visualization.
            //spriteBatch.Draw(_texture, Position, null, Color.Yellow, (float)Math.Atan2(_momentumVector.Y, _momentumVector.X) + MathHelper.ToRadians(90), Origin, 0.25f, SpriteEffects.None, 1);
        }

    }
}
