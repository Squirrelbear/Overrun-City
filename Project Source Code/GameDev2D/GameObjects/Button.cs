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
    class Button : GameObject
    {
        // The number on the button
        private int number;
        // The index of the button
        private int buttonID;
        // The collection of sprites that may be used to display the 
        private ArrayList textures;

        /// <summary>
        /// The button constructor.
        /// </summary>
        /// <param name="buttonID">The index of the button.</param>
        /// <param name="pos">Position to place the object.</param>
        /// <param name="textures">The object images.</param>
        /// <param name="game1">Reference to the game object.</param>
        public Button(int buttonID, Vector2 pos, ArrayList textures, Game1 game1)
            : base(Type.Button, pos, (Texture2D)textures[0], game1)
        {
            this.textures = textures;
            this.buttonID = buttonID;
            number = 0;
        }

        /// <summary>
        /// Handle the event that the button is collided with by a bullet.
        /// </summary>
        /// <param name="obj">The object that has been collided with.</param>
        public override void onCollision(GameObject obj)
        {
            // if object is a bullet shuffle the number to the next value
            if (obj.type == GameObject.Type.Bullet)
            {
                number++;
                if (number > 9) number = 0;
                currentSprite = (Texture2D)textures[number];
            }
        }

        /// <summary>
        /// Get the number displayed on the button.
        /// </summary>
        /// <returns>The displayed number.</returns>
        public int getNumber()
        {
            return number;
        }

        /// <summary>
        /// Get the index of the button.
        /// </summary>
        /// <returns>The index.</returns>
        public int getButtonID()
        {
            return buttonID;
        }
    }
}
