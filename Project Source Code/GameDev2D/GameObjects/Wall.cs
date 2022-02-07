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
    class Wall : GameObject
    {

        /// <summary>
        /// The wall constructor.
        /// </summary>
        /// <param name="pos">Position to place the object.</param>
        /// <param name="sprite">The image to display for the object.</param>
        /// <param name="game1">Reference to the game object.</param>
        public Wall(Vector2 pos, Texture2D sprite, Game1 game1) 
            : base(Type.Wall, pos, sprite, game1)
        {

        }

        /// <summary>
        /// The wall constructor. Including rotation of the wall.
        /// </summary>
        /// <param name="pos">Position to place the object.</param>
        /// <param name="sprite">The image to display for the object.</param>
        /// <param name="rotation">The rotation to set the wall to.</param>
        /// <param name="game1">Reference to the game object.</param>
        public Wall(Vector2 pos, Texture2D sprite, float rotation, Game1 game1)
            : base(Type.Wall, pos, rotation, sprite, game1)
        {

        }
    }
}
