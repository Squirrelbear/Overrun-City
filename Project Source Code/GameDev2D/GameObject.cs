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
    public class GameObject
    {
        /// <summary>
        /// The types that an object may be.
        /// </summary>
        public enum Type { Player = 1, Enemy = 2, Wall = 3, Bullet = 4, Button = 5, Door = 6, MovingCar = 7, LevelFinish = 8 };

        #region Instance Variables
        // The type of object that the inheriting object is.
        public Type type;

        // reference to the game's main class
        protected Game1 gameRef;

        // is the object visible
        protected bool visible;

        // the sprite's image, location, centre of object, and the rotation
        protected Texture2D currentSprite;
        protected Vector2 position;
        protected Vector2 objCenter;
        protected float rotation;

        // The variables needed for collision detection
        protected  Color [] objColor;
        protected Matrix transform;
        protected Rectangle rect;
        protected bool collidable;

        // the previous location that the object was located
        protected Vector2 lastPos;
        protected float lastRot;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a game object  using 0 rotation but otherwise the other game object defintion.
        /// </summary>
        /// <param name="newtype">The type of object to create.</param>
        /// <param name="pos">The position to place the object.</param>
        /// <param name="sprite">The image to set the sprite to.</param>
        /// <param name="game1">A pointer to the game.</param>
        public GameObject(Type newtype, Vector2 pos, Texture2D sprite, Game1 game1) 
            : this(newtype, pos, 0, sprite, game1)
        {
        }

        /// <summary>
        /// Create a game object with all of the required properties.
        /// </summary>
        /// <param name="newtype">The type of object to create.</param>
        /// <param name="pos">The position to place the object.</param>
        /// <param name="Rotation">The rotation for the object to be set to.</param>
        /// <param name="sprite">The image to set the sprite to.</param>
        /// <param name="game1">A pointer to the game.</param>
        public GameObject(Type newtype, Vector2 pos, float Rotation, Texture2D sprite, Game1 game1)
        {
            gameRef = game1;
            type = newtype;
            visible = true;
            position = pos;
            rotation = Rotation;
            currentSprite = sprite;
            objCenter = new Vector2(sprite.Width / 2, sprite.Height / 2);
            lastPos = pos;
            lastRot = rotation;
            collidable = true;

            objColor = new Color[currentSprite.Width * currentSprite.Height];
            currentSprite.GetData(objColor);
        }
        #endregion

        #region Methods that may be overridden
        /// <summary>
        /// Update the object.
        /// Store the previous location before updating the hitbox related variables.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void update(GameTime gameTime)
        {
            lastPos = new Vector2(position.X, position.Y);
            lastRot = rotation;

            transform = Transform(objCenter, rotation, position);
            rect = TransformRectangle(transform, getWidth(), getHeight());
        }

        /// <summary>
        /// Render the game object.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void draw(SpriteBatch spriteBatch)
        {
            // don't render if it isn't visible
            if (!visible) return;

            spriteBatch.Draw(currentSprite, position, null, 
                Color.White, rotation, objCenter, 1.0f, 
                SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Defines an overidable event for when objects collide.
        /// The default action is to do nothing. It will only be handled if it
        /// is overrided by a child class.
        /// </summary>
        /// <param name="obj">The object that was collided with.</param>
        public virtual void onCollision(GameObject obj)
        {           
        }
        #endregion

        #region Get methods
        /// <summary>
        /// Gets the transform of the object.
        /// </summary>
        /// <returns>The transform of the object.</returns>
        public Matrix getTransform()
        {
            return transform;
        }

        /// <summary>
        /// Gets the width of the object.
        /// </summary>
        /// <returns>The object width.</returns>
        public int getWidth()
        {
            return currentSprite.Width;
        }

        /// <summary>
        /// Get the height of the object.
        /// </summary>
        /// <returns>The object height.</returns>
        public int getHeight()
        {
            return currentSprite.Height;
        }

        /// <summary>
        /// Get the rectangle of the object.
        /// </summary>
        /// <returns>The rectangle.</returns>
        public Rectangle getRectangle()
        {
            return rect;
        }

        /// <summary>
        /// Get the object type.
        /// </summary>
        /// <returns>The object type.</returns>
        public Type getObjType()
        {
            return type;
        }

        /// <summary>
        /// Get the position of the object.
        /// </summary>
        /// <returns>The position.</returns>
        public Vector2 getPosition()
        {
            return position;
        }
        #endregion

        /* The following code has been taken from the code used in lectures for the collision detection.
         * It has been modified to allow for use based on objects.*/
        #region Collision related methods
        /// <summary>
        /// Get the pixel colour of a particular pixel number.
        /// </summary>
        /// <param name="pixelNum">The number of the pixel to get the colour of.</param>
        /// <returns>The colour of a pixel.</returns>
        public Color PixelColor(int pixelNum)
        {
            return objColor[pixelNum];
        }

        /// <summary>
        /// Calculates the translation matrix based on the center, rotation, and position of an object.
        /// </summary>
        /// <param name="center">The center of an object.</param>
        /// <param name="rotation">The rotation of an object.</param>
        /// <param name="position">The positin of an object.</param>
        /// <returns>The transformation matrix.</returns>
        protected Matrix Transform(Vector2 center, float rotation, Vector2 position)
        {
            // move to origin, scale (if desired), rotate, translate
            return Matrix.CreateTranslation(new Vector3(-center, 0.0f)) *
                // add scaling here if you want
                                            Matrix.CreateRotationZ(rotation) *
                                            Matrix.CreateTranslation(new Vector3(position, 0.0f));
        }
        
        /// <summary>
        /// Calculates the rectangle of the object based on the transform, width and height of the object.
        /// </summary>
        /// <param name="transform">The transform matrix that takes into account position and rotation.</param>
        /// <param name="width">The width of the object.</param>
        /// <param name="height">The height of the object.</param>
        /// <returns>The bounding box of the rectange.</returns>
        protected static Rectangle TransformRectangle(Matrix transform, int width, int height)
        {
            // Get each corner of texture
            Vector2 leftTop = new Vector2(0.0f, 0.0f);
            Vector2 rightTop = new Vector2(width, 0.0f);
            Vector2 leftBottom = new Vector2(0.0f, height);
            Vector2 rightBottom = new Vector2(width, height);

            // Transform each corner
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum corners
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
            Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
            Vector2.Max(leftBottom, rightBottom));

            // Return transformed rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }
        #endregion
    }
}
