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
    class Bullet : GameObject
    {
        // the object movement speed
        private float speed = 0.5f;
        // has the bullet been fired by the player
        public bool firedByPlayer;

        /// <summary>
        /// The bullet constructor.
        /// </summary>
        /// <param name="pos">Position to place the object.</param>
        /// <param name="Rotation">Initial object rotation.</param>
        /// <param name="sprite">The object image.</param>
        /// <param name="isPlayer">Whether the object was fired by the player.</param>
        /// <param name="game1">Reference to the game object.</param>
        public Bullet(Vector2 pos, float Rotation, Texture2D sprite, bool isPlayer, Game1 game1)
            : base(Type.Bullet, pos, Rotation, sprite, game1)
        {
            firedByPlayer = isPlayer;
        }

        /// <summary>
        /// Override the update method to handle the specific actions related to the bullet object.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);

            // get the elapsed time
            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;

            // update the relative position
            position.X += (float)Math.Cos(rotation - Math.PI / 2) * speed * timeLapse;
            position.Y += (float)Math.Sin(rotation - Math.PI / 2) * speed * timeLapse;

            // set the object to be removed at the end of the 
            // current update if it has moved outside the bounds
            if (position.X > 1200 || position.X < -200 || position.Y > 900 || position.Y < -200)
            {
                gameRef.destroyObject(this);    
            }
        }

        /// <summary>
        /// Handle the collision event with this object.
        /// </summary>
        /// <param name="obj">The object that was collided with.</param>
        public override void onCollision(GameObject obj)
        {
            // Nothing happens if the object is colliding with the 
            // same type of individual that fired it
            // enemies can't be hit by enemy bullets
            // player can't be hit with their own bullets
            if ((obj.type == Type.Player && firedByPlayer) || (!firedByPlayer && obj.type == Type.Enemy)) return;
            
            // If the bullet was from an enemy the game is over
            if (obj.type == Type.Player)
            {
                gameRef.setGameState(Game1.GameState.GameOverLose);          
            }

            // if the object is an enemy destroy the enemy
            if (obj.type == Type.Enemy)
            {
                gameRef.destroyObject(obj);   
            }

            // all collisions otherwise result in the
            // bullet being destroyed.
            gameRef.destroyObject(this);               
        }
    }
}
