using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDev_2DGame
{
    class LevelFinish : GameObject
    {
        /// <summary>
        /// Defines a level finish point
        /// </summary>
        /// <param name="pos">The position of the object.</param>
        /// <param name="sprite">The sprite that is displayed for the object.</param>
        /// <param name="game1">A reference to the game.</param>
        public LevelFinish(Vector2 pos, Texture2D sprite, Game1 game1) 
            : base(Type.LevelFinish, pos, sprite, game1)
        {

        }
    }
}
