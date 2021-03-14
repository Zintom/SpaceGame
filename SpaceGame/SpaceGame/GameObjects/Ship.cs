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

        public override Point Size { get; } = new Point(20, 24);

        private static Vector2 Origin;

        /// <summary>
        /// The rotation of the "ship", in Radians (0 == Right).
        /// </summary>
        private double _vehicleRotation = 0f;

        /// <summary>
        /// The velocity of the ship.
        /// </summary>
        private Vector2 _velocity = Vector2.Zero;

        private readonly float TopSpeed = 6f;

        private readonly float Thrust = 0.0025f;

        private readonly float RotationSpeed = 0.005f;

        private double TimeLastShotTaken = 0f;

        private KeyboardState oldKeyState;
        private GamePadState oldGamePadState;

        private readonly IPlayingFieldManager _playingField;

        public Ship(TextureProvider textureProvider, Viewport viewport, IPlayingFieldManager playingFieldManager) : base(viewport)
        {
            _texture = textureProvider.Ship;
            Origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);

            _playingField = playingFieldManager;

            oldKeyState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var keyState = Keyboard.GetState();
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            var gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);

            // Shoot

            if (gameTime.TotalGameTime.TotalMilliseconds - TimeLastShotTaken > 100 &&
                (keyState.IsKeyDown(Keys.Space) && !oldKeyState.IsKeyDown(Keys.Space))
                || gamePadCapabilities.IsConnected && gamePadState.IsButtonDown(Buttons.A) && !oldGamePadState.IsButtonDown(Buttons.A))
            {
                TimeLastShotTaken = gameTime.TotalGameTime.TotalMilliseconds;
                double shipDirection = _vehicleRotation - MathHelper.ToRadians(90);
                Vector2 shipDirectionVector = new Vector2((float)Math.Cos(shipDirection), (float)Math.Sin(shipDirection));

                Vector2 bulletStartingPosition = Position; // The center of our ship
                bulletStartingPosition -= new Vector2(1, 0); // Offset by -1 as a bullet is 2,2.
                bulletStartingPosition += shipDirectionVector * (Size.Y / 2); // Push the position to the nose of the ship.

                var bullet = new Bullet(_viewport, _playingField, bulletStartingPosition, shipDirectionVector * 0.5f);
                _playingField.AddBullet(bullet);
            }

            // Movement

            if (keyState.IsKeyDown(Keys.A))
            {
                _vehicleRotation -= RotationSpeed * deltaTime;
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                _vehicleRotation += RotationSpeed * deltaTime;
            }
            else if (gamePadState.IsConnected)
            {
                _vehicleRotation += gamePadState.ThumbSticks.Left.X * RotationSpeed * deltaTime;
            }

            // Add thrust velocity is space is being pressed.
            if (keyState.IsKeyDown(Keys.W)
                || gamePadState.IsConnected && gamePadState.IsButtonDown(Buttons.RightTrigger))
            {
                // '0' in radians points right, in order to thrust upwards to we take 90 degrees off the rotation.
                double thrustDirection = _vehicleRotation - MathHelper.ToRadians(90);

                // Convert the thrust direction into a unit vector so that it
                // can be added to the momentum vector.
                var thrustUnitVector = new Vector2((float)Math.Cos(thrustDirection), (float)Math.Sin(thrustDirection));

                _velocity += thrustUnitVector * Thrust * deltaTime;
            }
            // Remove velocity if space is not being pressed.
            else
            {
                _velocity = Vector2.Lerp(_velocity, Vector2.Zero, 0.001f * deltaTime);
                
                // If speed is less than half a pixel per second then round down otherwise you get tiny little jumps in pixels
                // even when you appear stationary.
                if(_velocity.Length() <= 0.05f)
                {
                    _velocity = Vector2.Zero;
                }
            }

            // The Speed Cap Vector = Momentum Unit Vector * Top Speed
            Vector2 speedCap = Vector2.Normalize(_velocity) * TopSpeed;

            if (_velocity.Length() > speedCap.Length())
            {
                _velocity = speedCap;
            }

            Position += _velocity;

            //Debug.WriteLine($"Velocity Vector: {Vector2.Round(_velocity)}, Speed:  {_velocity.Length()}");

            base.Update(gameTime);

            oldKeyState = keyState;
            oldGamePadState = gamePadState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(MainGame.Blank, new Rectangle(Position.ToPoint(), Size), null, Color.Red, _vehicleRotation, new Vector2(MainGame.Blank.Width / 2f, MainGame.Blank.Height / 2f), SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, new Rectangle(Position.ToPoint(), Size), null, Color.White, (float)_vehicleRotation, Origin, SpriteEffects.None, 1);
            
            // Draw momentum visualization.
            //spriteBatch.Draw(_texture, Position, null, Color.Yellow, (float)Math.Atan2(_momentumVector.Y, _momentumVector.X) + MathHelper.ToRadians(90), Origin, 0.25f, SpriteEffects.None, 1);
        }

    }
}
