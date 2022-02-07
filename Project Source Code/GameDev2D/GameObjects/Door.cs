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
    class Door : GameObject
    {
        // direction of animation
        private bool clockwise;
        // whether the object should be moving
        private bool move;
        // the rotation speed
        private float rotatespeed = 0.001f;

        /// <summary>
        /// Basic constructor for clockwise movement
        /// </summary>
        /// <param name="pos">The position to place the object.</param>
        /// <param name="sprite">The image to display for the object.</param>
        /// <param name="game1">The reference to the game object.</param>
        public Door(Vector2 pos, Texture2D sprite, Game1 game1)
            : this(pos, sprite, 0, true, game1)
        {

        }

        /// <summary>
        /// The full constructor for the door object.
        /// </summary>
        /// <param name="pos">The position to place the object.</param>
        /// <param name="sprite">The image to display for the object.</param>
        /// <param name="rotation">The object rotation.</param>
        /// <param name="clockwise">Direction of motion.</param>
        /// <param name="game1">The game reference.</param>
        public Door(Vector2 pos, Texture2D sprite, float rotation, bool clockwise, Game1 game1)
            : base(Type.Door, pos, rotation, sprite, game1)
        {
            this.clockwise = clockwise;
            move = false;
            objCenter = new Vector2(0, sprite.Height / 2);
        }

        /// <summary>
        /// Update the game object if it should be moving.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            if(!move) return;

            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;

            // Move in the correct direction
            if (clockwise)
            {
                if (rotation >= MathHelper.PiOver2)
                {
                    rotation = MathHelper.PiOver2;
                    move = false;
                    return;
                }
                rotation += rotatespeed * timeLapse;
            }
            else
            {
                if (rotation <= MathHelper.PiOver2)
                {
                    rotation = MathHelper.PiOver2;
                    move = false;
                    return;
                }
                rotation -= rotatespeed * timeLapse;
            }
        }

        /// <summary>
        /// Force the object to begin moving.
        /// </summary>
        public void makeMove()
        {
            move = true;
        }
    }
}