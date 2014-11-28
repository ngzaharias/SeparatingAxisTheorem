#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace SAT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D circleTex, triangleTex;
        Color colourA, colourB;
        CircleCollider2D circleA, circleB;
        TriangleCollider2D triangleA, triangleB;
        Vector2 positionA, positionB;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            circleA = new CircleCollider2D(Vector2.Zero, 50.0f);
            circleB = new CircleCollider2D(Vector2.Zero, 50.0f);
            triangleA = new TriangleCollider2D(Vector2.Zero, new Vector2[3] { new Vector2(-50, -50), new Vector2(50, -50), new Vector2(50, 50) });
            triangleB = new TriangleCollider2D(Vector2.Zero, new Vector2[3] { new Vector2(-50, -50), new Vector2(50, -50), new Vector2(50, 50) });

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

            // TODO: use this.Content to load your game content here
            circleTex = Content.Load<Texture2D>("circle.png");
            triangleTex = Content.Load<Texture2D>("triangle.png");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            positionA = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            positionB = new Vector2(200);
            if (circleA.Collision(positionA, triangleB, positionB))
                if (triangleB.Collision(positionB, circleA, positionA))
                    colourA = colourB = Color.Red;
                else
                    colourA = colourB = Color.White;
            else
                colourA = colourB = Color.White;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(circleTex, positionA - new Vector2(circleA.Radius), colourA);
            spriteBatch.Draw(triangleTex, positionB - new Vector2(50), colourB);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
