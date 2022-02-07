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
using System.Diagnostics;

namespace GameDev_2DGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Three defined gamestates for while playing the game, and the win/loss scenarios.
        /// </summary>
        public enum GameState { Starting = 0, Playing = 1, GameOverLose = 2, GameOverWin = 3 };

        #region Instance Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // sprite variables
        Texture2D playersprite;
        Texture2D enemysprite;
        Texture2D bulletsprite;
        Texture2D background;
        Texture2D wallsprite;
        Texture2D doorsprite;
        ArrayList carsprites;
        ArrayList buttonsprites;
        Texture2D gameOverSprite;
        Texture2D winSprite;
        Texture2D beginSprite;
        Texture2D finishLevelSprite;

        // gameobject lists
        ArrayList gameObjects;
        ArrayList gameObjectsRemoval;
        ArrayList gameObjectsAdd;

        // input device current and previous states
        KeyboardState keybState;
        GamePadState gamepState;
        KeyboardState keybStateOld;
        GamePadState gamepStateOld;

        // solution to the puzzle
        int[] solution = { 1, 3, 3, 7 };

        // The current game state
        GameState state;

        // collision variables
        int threshold = 5;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // set the resolution to 1024x768 
            // (a standard 720p resolution that should be supported by most systems)
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load the button sprites
            buttonsprites = new ArrayList();
            for (int i = 0; i <= 9; i++)
            {
                buttonsprites.Add(Content.Load<Texture2D>("buttons/" + i));
            }

            // load the car sprites
            carsprites = new ArrayList();
            for (int i = 1; i <= 9; i++)
            {
                carsprites.Add(Content.Load<Texture2D>("cars/car" + i));
            }

            // load the window sprites
            gameOverSprite = Content.Load<Texture2D>("wnds/losewnd");
            winSprite = Content.Load<Texture2D>("wnds/winwnd");
            beginSprite = Content.Load<Texture2D>("wnds/beginwnd");

            // character related sprites
            playersprite = Content.Load<Texture2D>("characters/player");
            enemysprite = Content.Load<Texture2D>("characters/enemy");
            bulletsprite = Content.Load<Texture2D>("characters/bullet");

            // other sprites
            finishLevelSprite = Content.Load<Texture2D>("other/levelfinish");
            background = Content.Load<Texture2D>("other/background");
            wallsprite = Content.Load<Texture2D>("other/wall");
            doorsprite = Content.Load<Texture2D>("other/door");

            // prepare the list objects
            gameObjects = new ArrayList();
            gameObjectsRemoval = new ArrayList();
            gameObjectsAdd = new ArrayList();

            // generate the level
            reset();

            state = GameState.Starting;
        }

        /// <summary>
        /// Clear all of the object lists and add all the elements that need to appear in the level.
        /// </summary>
        private void reset()
        {
            gameObjects.Clear();
            gameObjectsRemoval.Clear();
            gameObjectsAdd.Clear();

            // create all of the buttons
            gameObjects.Add(new Button(0, new Vector2(50, 700), buttonsprites, this));
            gameObjects.Add(new Button(1, new Vector2(650, 50), buttonsprites, this));
            gameObjects.Add(new Button(2, new Vector2(320, 600), buttonsprites, this));
            gameObjects.Add(new Button(3, new Vector2(975, 700), buttonsprites, this));

            // first column of cars
            gameObjects.Add(new Wall(new Vector2(150, 190), (Texture2D)carsprites[0], MathHelper.PiOver2, this));
            gameObjects.Add(new Wall(new Vector2(150, 360), (Texture2D)carsprites[1], MathHelper.PiOver2, this));
            gameObjects.Add(new Wall(new Vector2(150, 530), (Texture2D)carsprites[2], MathHelper.PiOver2, this));
            gameObjects.Add(new Wall(new Vector2(150, 700), (Texture2D)carsprites[3], MathHelper.PiOver2, this));

            // middle top row
            gameObjects.Add(new Wall(new Vector2(300, 150), (Texture2D)carsprites[4], MathHelper.Pi, this));
            gameObjects.Add(new Wall(new Vector2(630, 150), (Texture2D)carsprites[5], MathHelper.Pi, this));

            // far column of cars
            // gameObjects.Add(new Wall(new Vector2(150 + 240 * 3, 30), (Texture2D)carsprites[1], MathHelper.Pi / 2, this));
            gameObjects.Add(new Wall(new Vector2(780, 70), (Texture2D)carsprites[6], MathHelper.PiOver2 * 3.0f, this));
            gameObjects.Add(new Wall(new Vector2(780, 360), (Texture2D)carsprites[7], MathHelper.PiOver2 * 3.0f, this));
            gameObjects.Add(new Wall(new Vector2(780, 530), (Texture2D)carsprites[2], MathHelper.PiOver2 * 3.0f, this));
            gameObjects.Add(new Wall(new Vector2(780, 700), (Texture2D)carsprites[6], MathHelper.PiOver2 * 3.0f, this));

            // moving car
            gameObjects.Add(new MovingCar(new Vector2(870, -90), carsprites, MathHelper.PiOver2, this));

            // add walls
            gameObjects.Add(new Wall(new Vector2(370, 590), wallsprite, 0, this));
            gameObjects.Add(new Wall(new Vector2(370, 690), wallsprite, 0, this));
            gameObjects.Add(new Wall(new Vector2(640, 690), wallsprite, 0, this));
            gameObjects.Add(new Wall(new Vector2(640, 590), wallsprite, 0, this));

            // add level finish point
            gameObjects.Add(new LevelFinish(new Vector2(520, 700), finishLevelSprite, this));

            // add doors
            gameObjects.Add(new Door(new Vector2(390, 480), doorsprite, this));
            gameObjects.Add(new Door(new Vector2(620, 480), doorsprite, MathHelper.Pi, false, this));

            // Create AI and player
            gameObjects.Add(new Enemy(new Vector2(450, 50), MathHelper.PiOver2 * 3.0f, enemysprite, this));
            gameObjects.Add(new Enemy(new Vector2(250, 200), MathHelper.PiOver2, enemysprite, this));
            gameObjects.Add(new Enemy(new Vector2(950, 620), 0, enemysprite, this));
            gameObjects.Add(new Enemy(new Vector2(320, 520), 0, enemysprite, this));
            gameObjects.Add(new Enemy(new Vector2(980, 250), MathHelper.PiOver2 * 3.0f, enemysprite, this));
            gameObjects.Add(new Player(new Vector2(50, 500), playersprite, this));
            
            state = GameState.Playing;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // get the latest input device states
            keybStateOld = keybState;
            gamepStateOld = gamepState;
            keybState = Keyboard.GetState();
            gamepState = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (gamepState.Buttons.Back == ButtonState.Pressed || keybState.IsKeyDown(Keys.Escape))
                this.Exit();

            // Changes between full screen and windowed mode
            if (!keybStateOld.IsKeyDown(Keys.F) && keybState.IsKeyDown(Keys.F))
                graphics.ToggleFullScreen();

            // reset the game to allow new game
            if ((gamepStateOld.Buttons.Start == ButtonState.Pressed && gamepState.Buttons.Start == ButtonState.Released) 
                || (keybStateOld.IsKeyDown(Keys.F2) && !keybState.IsKeyDown(Keys.F2)))
            {
                reset();
            }

            // update the objects in the level if it is required
            if(state == GameState.Playing) {
                updatePlayingState(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Update all of the objects that are in the level and manage collisions with events.
        /// </summary>
        /// <param name="gameTime">The time variable.</param>
        private void updatePlayingState(GameTime gameTime)
        {
            // the count for how many buttons are correct
            int solveCount = 0;

            // Apply each objects individual update events
            foreach (GameObject obj in gameObjects)
            {
                obj.update(gameTime);

                // increase the count of correct numbers for each correct
                if (obj.getObjType() == GameObject.Type.Button)
                {
                    Button b = (Button)obj;
                    if (solution[b.getButtonID()] == b.getNumber())
                    {
                        solveCount++;
                    }
                }
            }

            // The combination has been solved
            if (solveCount == 4)
            {
                // Apply each objects individual update events
                foreach (GameObject obj in gameObjects)
                {
                    if (obj.getObjType() == GameObject.Type.Door)
                    {
                        Door d = (Door)obj;
                        d.makeMove();
                    }
                }
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    GameObject A = (GameObject)gameObjects[i];
                    GameObject B = (GameObject)gameObjects[j];

                    if (A.getRectangle().Intersects(B.getRectangle())) // rough collision check
                    {
                        if (PixelCollision(A, B))
                        {
                            A.onCollision(B);
                            B.onCollision(A);
                        }
                    }
                }
            }

            // Now remove all the destroyed objects to clear up
            foreach (GameObject obj in gameObjectsRemoval)
            {
                gameObjects.Remove(obj);
            }
            // flush list
            gameObjectsRemoval.Clear();

            // Now add all new objects
            foreach (GameObject obj in gameObjectsAdd)
            {
                gameObjects.Add(obj);
            }
            // flush list
            gameObjectsAdd.Clear();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // draw the background
            spriteBatch.Draw(background, new Rectangle(0, 0, 1024, 768), Color.White);

            // draw each object in the scene
            foreach (GameObject obj in gameObjects)
            {
                obj.draw(spriteBatch);
            }

            // draw an appropriate overlay screen if required
            if (state != GameState.Playing)
            {
                Texture2D wnd;
                switch (state)
                {
                    case GameState.GameOverLose:
                        wnd = gameOverSprite;
                        break;
                    case GameState.GameOverWin:
                        wnd = winSprite;
                        break;
                    case GameState.Starting:
                        wnd = beginSprite;
                        break;
                    default:
                        wnd = beginSprite;
                        break;
                }

                spriteBatch.Draw(wnd,
                    new Rectangle((graphics.PreferredBackBufferWidth - wnd.Width) / 2,
                        (graphics.PreferredBackBufferHeight - wnd.Height) / 2, wnd.Width, wnd.Height),
                        Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Get/Set/Create/Destroy methods

        /// <summary>
        /// Register that a particular object needs to be destroyed.
        /// </summary>
        /// <param name="obj">The object to destroy.</param>
        public void destroyObject(GameObject obj)
        {
            gameObjectsRemoval.Add(obj);
        }

        /// <summary>
        /// Register that an object needs to be added to the scene.
        /// </summary>
        /// <param name="obj">Object to be added.</param>
        public void addObject(GameObject obj)
        {
            gameObjectsAdd.Add(obj);
        }

        /// <summary>
        ///  Create a bullet to be inserted into the scene.
        /// </summary>
        /// <param name="pos">The location to place the bullet.</param>
        /// <param name="rotation">The rotation of the bullet.</param>
        /// <param name="isPlayer">Whether the player shot the bullet or not.</param>
        public void addBullet(Vector2 pos, float rotation, bool isPlayer)
        {
            gameObjectsAdd.Add(new Bullet(pos, rotation, bulletsprite, isPlayer, this));
        }

        /// <summary>
        /// Get the current keyboard state.
        /// </summary>
        /// <returns>Current keyboard state.</returns>
        public KeyboardState getKeyboardState()
        {
            return keybState;
        }

        /// <summary>
        /// Get the previous keyboard state.
        /// </summary>
        /// <returns>Previous keyboard state.</returns>
        public KeyboardState getKeyboardStateOld()
        {
            return keybStateOld;
        }

        /// <summary>
        /// Get the current gamepad state.
        /// </summary>
        /// <returns>The current gamepad state.</returns>
        public GamePadState getGamePadState()
        {
            return gamepState;
        }

        /// <summary>
        /// Get the previous gamepad state.
        /// </summary>
        /// <returns>Previous gamepad state.</returns>
        public GamePadState getGamePadStateOld()
        {
            return gamepStateOld;
        }

        /// <summary>
        /// Set the game state.
        /// </summary>
        /// <param name="state">The state to set to.</param>
        public void setGameState(GameState state)
        {
            this.state = state;
        }
        #endregion

        /*  The following code has been based off the code from lectures.
 *  The code has been modified to instead take two game objects as parameters
 *  this simplifies the calling of the method.
 *  Also a threshold level has been set. For a collision to be true at least
 *  five pixels must be colliding. */

        /// <summary>
        /// Check whether a pixel collision exists between two game objects.
        /// </summary>
        /// <param name="A">First object.</param>
        /// <param name="B">Second object.</param>
        /// <returns>True, if there is a collision.</returns>
        public bool PixelCollision(GameObject A, GameObject B)
        {
            Matrix transformA = A.getTransform();
            int pixelWidthA = A.getWidth();
            int pixelHeightA = A.getHeight();

            Matrix transformB = B.getTransform();
            int pixelWidthB = B.getWidth();
            int pixelHeightB = B.getHeight();
            int threshold = 0;

            // set A transformation relative to B. B remains at x=0, y=0.
            Matrix AtoB = transformA * Matrix.Invert(transformB);

            // generate a perpendicular vectors to each rectangle side
            Vector2 columnStep, rowStep, rowStartPosition;

            columnStep = Vector2.TransformNormal(Vector2.UnitX, AtoB);
            rowStep = Vector2.TransformNormal(Vector2.UnitY, AtoB);

            // calculate the top left corner of A
            rowStartPosition = Vector2.Transform(Vector2.Zero, AtoB);

            // search each row of pixels in A. start at top and move down.
            for (int rowA = 0; rowA < pixelHeightA; rowA++)
            {
                // begin at the left
                Vector2 pixelPositionA = rowStartPosition;

                // for each column in the row (move left to right)
                for (int colA = 0; colA < pixelWidthA; colA++)
                {
                    // get the pixel position
                    int X = (int)Math.Round(pixelPositionA.X);
                    int Y = (int)Math.Round(pixelPositionA.Y);

                    // if the pixel is within the bounds of B
                    if (X >= 0 && X < pixelWidthB && Y >= 0 && Y < pixelHeightB)
                    {
                        // get colors of overlapping pixels
                        Color colorA = A.PixelColor(colA + rowA * pixelWidthA);
                        Color colorB = B.PixelColor(X + Y * pixelWidthB);

                        // if both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0 && (++threshold >= this.threshold))
                            return true; // collision
                    }
                    // move to the next pixel in the row of A
                    pixelPositionA += columnStep;
                }
                // move to the next row of A
                rowStartPosition += rowStep;
            }
            return false; // no collision
        }
    }
}
