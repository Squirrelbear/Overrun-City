using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace GameDev_2DGame
{
    class Player : GameObject
    {
        private float speed;
        private float rotatespeed;

        /// <summary>
        /// The player constructor.
        /// </summary>
        /// <param name="pos">Position to place the object.</param>
        /// <param name="sprite">The image to display for the player.</param>
        /// <param name="game1">Reference to the game object.</param>
        public Player(Vector2 pos, Texture2D sprite, Game1 game1)
            : base(Type.Player, pos, sprite, game1)
        {
            speed = 0.2f;
            rotatespeed = 0.01f;
        }

        /// <summary>
        /// Update the player object's location based on the keyboard or 
        /// gamepad and fire shots if required.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;

            // get the current states of the input devices
            GamePadState gamepState = gameRef.getGamePadState();
            KeyboardState keybState = gameRef.getKeyboardState();

            // update the position based on the game pad
            if (gamepState.IsConnected)
            {
                rotation -= gamepState.ThumbSticks.Right.X * rotatespeed * timeLapse;
                rotation = rotation % (MathHelper.Pi * 2.0f);
                if (rotation > MathHelper.Pi * 2.0f) rotation -= MathHelper.Pi * 2.0f;
                if (rotation < 0) rotation += MathHelper.Pi * 2.0f;
                position.X += gamepState.ThumbSticks.Left.X * speed * timeLapse;
                position.Y -= gamepState.ThumbSticks.Left.Y * speed * timeLapse;
            }

            // update the position based on the WSAD keys
            if (keybState.IsKeyDown(Keys.W))
            {
                position.Y -= speed * timeLapse;
            }
            if (keybState.IsKeyDown(Keys.S))
            {
                position.Y += speed * timeLapse;
            }
            if (keybState.IsKeyDown(Keys.A))
            {
                position.X -= speed * timeLapse;
            }
            if (keybState.IsKeyDown(Keys.D))
            {
                position.X += speed * timeLapse;
            }

            // Force the position to be within the bounds of the window
            if (position.Y > 748)
            {
                position.Y -= speed * timeLapse;
            }
            if (position.Y < 20)
            {
                position.Y += speed * timeLapse;
            }
            if (position.X < 20)
            {
                position.X += speed * timeLapse;
            }
            if (position.X > 1004)
            {
                position.X -= speed * timeLapse;
            }

            // update the rotation by shifting the left or right based on the left or right
            if (keybState.IsKeyDown(Keys.Right) && !gameRef.getKeyboardStateOld().IsKeyDown(Keys.Right))
            {
                rotation += MathHelper.PiOver2;
                rotation = rotation % (MathHelper.Pi * 2.0f);
                if (rotation > MathHelper.Pi * 2.0f) rotation -= MathHelper.Pi * 2.0f;

                if (rotation < MathHelper.PiOver2)
                {
                    rotation = 0;
                }
                else if (rotation < MathHelper.Pi)
                {
                    rotation = MathHelper.PiOver2;
                }
                else if (rotation < MathHelper.PiOver2 * 3)
                {
                    rotation = MathHelper.Pi;
                }
                else
                {
                    rotation = MathHelper.PiOver2 * 3;
                }
            }
            if (keybState.IsKeyDown(Keys.Left) && !gameRef.getKeyboardStateOld().IsKeyDown(Keys.Left))
            {
                if (rotation > MathHelper.PiOver2 * 3.0f || rotation == 0.0f)
                {
                    rotation = MathHelper.PiOver2 * 3.0f;
                }
                else if (rotation > MathHelper.Pi)
                {
                    rotation = MathHelper.Pi;
                }
                else if (rotation > MathHelper.PiOver2)
                {
                    rotation = MathHelper.PiOver2;
                }
                else
                {
                    rotation = 0;
                }
            }

            // fire if the space or right trigger has been just pressed down
            if (keybState.IsKeyDown(Keys.Space) || gamepState.IsButtonDown(Buttons.RightTrigger))
            {
                if (!gameRef.getKeyboardStateOld().IsKeyDown(Keys.Space)
                    && !gameRef.getGamePadStateOld().IsButtonDown(Buttons.RightTrigger))
                {
                    // create and fire a bullet
                    Vector2 trans = new Vector2(objCenter.X * (float)Math.Cos(rotation + MathHelper.Pi / 4) +
                        objCenter.Y * (float)Math.Sin(rotation + MathHelper.Pi / 4),
                        objCenter.X * (float)Math.Sin(rotation + MathHelper.Pi / 4) -
                        objCenter.Y * (float)Math.Cos(rotation + MathHelper.Pi / 4));

                    Vector2 bulletPos = new Vector2(trans.X / 4 + position.X, trans.Y / 4 + position.Y);
                    gameRef.addBullet(bulletPos, rotation, true);
                }
            }

            transform = Transform(objCenter, rotation, position);
            rect = TransformRectangle(transform, getWidth(), getHeight());
        }

        /// <summary>
        /// Handle collisions involving the player.
        /// </summary>
        /// <param name="obj">The object that has been collided with.</param>
        public override void onCollision(GameObject obj)
        {
            base.onCollision(obj);

            // complete the game if reached finish
            if (obj.type == Type.LevelFinish)
            {
                gameRef.setGameState(Game1.GameState.GameOverWin);
                return;
            }

            // handle collisions that require the player be forced to 
            // stay at their previous location
            if (obj.type != Type.Enemy && obj.type != Type.Button)
            {
                position = lastPos;
                rotation = lastRot;

                transform = Transform(objCenter, rotation, position);
                rect = TransformRectangle(transform, getWidth(), getHeight());
            }
        }
    }
}
