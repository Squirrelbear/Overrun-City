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
using System.Collections;

namespace GameDev_2DGame
{
    class MovingCar : GameObject
    {
        // The collection of sprites that could be displayed
        private ArrayList sprites;
        // A random generator
        private Random gen;
        // The initial position to start the car object
        private Vector2 startPos;
        // the movement speed of the object
        private float speed = 0.1f;

        /// <summary>
        /// The basic constructor for the moving car
        /// </summary>
        /// <param name="pos">The position to begin the car at</param>
        /// <param name="sprites">The images that the moving car can be</param>
        /// <param name="game1">The game reference</param>
        public MovingCar(Vector2 pos, ArrayList sprites, Game1 game1)
            : this(pos, sprites, 0, game1)
        {

        }

        /// <summary>
        /// The constructor that allows for object rotation for the moving car.
        /// </summary>
        /// <param name="pos">The position to begin the car at</param>
        /// <param name="sprites">The images that the moving car can be</param>
        /// <param name="rotation">The object rotation.</param>
        /// <param name="game1">The game reference</param>
        public MovingCar(Vector2 pos, ArrayList sprites, float rotation, Game1 game1)
            : base(Type.MovingCar, pos, rotation, (Texture2D)sprites[0], game1)
        {
            startPos = pos;
            this.sprites = sprites;
            gen = new Random();
            selectRandomCar();
        }

        /// <summary>
        /// Update the position of the moving car. 
        /// Reset to initial position and to a new 
        /// random car if it has passed out.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);

            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;
            position.Y += speed * timeLapse;
            if (position.Y > 800)
            {
                position = startPos;
                selectRandomCar();
            }
        }

        /// <summary>
        /// Select a random car and update the appropriate variables.
        /// </summary>
        private void selectRandomCar()
        {
            currentSprite = (Texture2D)sprites[gen.Next(sprites.Count)];
            objCenter = new Vector2(currentSprite.Width / 2, currentSprite.Height / 2);
            objColor = new Color[currentSprite.Width * currentSprite.Height];
            currentSprite.GetData(objColor);
        }

        /// <summary>
        /// Handle the collision between the player and the car.
        /// </summary>
        /// <param name="obj"></param>
        public override void onCollision(GameObject obj)
        {
            base.onCollision(obj);

            // Work around code to make it harder to
            // have a collision that leaves the player 
            // stuck and the game unplayable.
            if (obj.type != Type.Bullet && 
                 (obj.type != Type.Player || 
                 (obj.type == Type.Player && position.Y <= obj.getPosition().Y
                     && position.X - obj.getPosition().X < 15 && position.X - obj.getPosition().X > 0)))
            {
                position = lastPos;
                rotation = lastRot;

                transform = Transform(objCenter, rotation, position);
                rect = TransformRectangle(transform, getWidth(), getHeight());
            }
        }
    }
}
