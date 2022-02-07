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

namespace GameDev_2DGame
{
    class Enemy : GameObject
    {
        // The rate frequency of fire
        private float fireRate;
        // the progress since the last shot
        private float fireProgress;
        // the difficulty of the bot
        private int difficulty;

        /// <summary>
        /// Basic constructor for the enemy object.
        /// </summary>
        /// <param name="pos">The position of the object.</param>
        /// <param name="sprite">The image to display for the object.</param>
        /// <param name="game1">The reference to the game.</param>
        public Enemy(Vector2 pos, Texture2D sprite, Game1 game1)
            : base(Type.Enemy, pos, sprite, game1)
        {
            fireRate = 2000.0f;
            fireProgress = 0;
            difficulty = 1;
        }

        /// <summary>
        /// Basic constructor for the enemy object. That includes rotation.
        /// </summary>
        /// <param name="pos">The position of the object.</param>
        /// <param name="rotation">The rotation of the object.</param>
        /// <param name="sprite">The image to display for the object.</param>
        /// <param name="game1">The reference to the game.</param>
        public Enemy(Vector2 pos, float rotation, Texture2D sprite, Game1 game1)
            : base(Type.Enemy, pos, rotation, sprite, game1)
        {
            fireRate = 2000.0f;
            fireProgress = 0;
            difficulty = 1;
        }

        /// <summary>
        /// Update the enemy. Fire if required, and check if difficulty needs changing.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;

            // update firing progress
            fireProgress += timeLapse;
            if (fireProgress >= fireRate)
            {
                // calculate the position for and spawn a new bullet
                Vector2 trans = new Vector2(objCenter.X * (float)Math.Cos(rotation + MathHelper.Pi / 4) +
                        objCenter.Y * (float)Math.Sin(rotation + MathHelper.Pi / 4),
                        objCenter.X * (float)Math.Sin(rotation + MathHelper.Pi / 4) -
                        objCenter.Y * (float)Math.Cos(rotation + MathHelper.Pi / 4));

                Vector2 bulletPos = new Vector2(trans.X / 4 + position.X, trans.Y / 4 + position.Y);
                gameRef.addBullet(bulletPos, rotation, false);
                fireProgress = fireProgress % fireRate;
            }

            // Check if difficulty needs to be updated
            if (!gameRef.getKeyboardStateOld().IsKeyDown(Keys.G)
                    && !gameRef.getGamePadStateOld().IsButtonDown(Buttons.RightShoulder))
            {
                if (gameRef.getKeyboardState().IsKeyDown(Keys.G)
                    || gameRef.getGamePadState().IsButtonDown(Buttons.RightShoulder))
                {
                    difficulty++;
                    if (difficulty > 3)
                    {
                        difficulty = 1;
                    }

                    switch (difficulty)
                    {
                        case 1:
                            fireRate = 2000.0f;
                            break;
                        case 2:
                            fireRate = 1000.0f;
                            break;
                        case 3:
                            fireRate = 300.0f;
                            break;
                    }
                }
            }
        }
    }
}
