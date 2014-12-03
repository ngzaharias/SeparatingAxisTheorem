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
	public class Shape
	{
		public Collider2D collider;
		public Color colour;
		public Vector2 offset;
		public Vector2 position;
		public Texture2D texture;
		bool collide = false;

		public void CollisionStuff(Shape other)
		{
			if (collider.Collision(position, other.collider, other.position))
			{
				if (other.collider.Collision(other.position, collider, position))
				{
					collide = true;
					other.collide = true;
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, position - offset, collide ? Color.Red : Color.White);
			collide = false;
		}
	}


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		List<Shape> shapes;
		Shape circle, triangle, square, house;

		int state = 0;

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
			graphics.PreferredBackBufferWidth = 1680;
			graphics.PreferredBackBufferHeight = 1050;
			//graphics.IsFullScreen = true;

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

			shapes = new List<Shape>();

            // TODO: use this.Content to load your game content here
			circle = new Shape();
			circle.collider = new CircleCollider2D(Vector2.Zero, 50.0f);
			circle.position = new Vector2(100, 200);
			circle.texture = Content.Load<Texture2D>("circle.png");
			circle.offset = new Vector2(50.0f);
			circle.colour = Color.White;
			shapes.Add(circle);

			triangle = new Shape();
			triangle.collider = new TriangleCollider2D(Vector2.Zero, new Vector2[3] { new Vector2(-50, -50), new Vector2(50, -50), new Vector2(50, 50) });
			triangle.position = new Vector2(250, 200);
			triangle.texture = Content.Load<Texture2D>("triangle.png");
			triangle.offset = new Vector2(50.0f);
			triangle.colour = Color.White;
			shapes.Add(triangle);

			square = new Shape();
			square.collider = new TriangleCollider2D(Vector2.Zero, new Vector2[4] { new Vector2(-50, -50), new Vector2(50, -50), new Vector2(50, 50), new Vector2(-50, 50) });
			square.position = new Vector2(400, 200);
			square.texture = Content.Load<Texture2D>("square.png");
			square.offset = new Vector2(50.0f);
			square.colour = Color.White;
			shapes.Add(square);

			house = new Shape();
			house.collider = new TriangleCollider2D(Vector2.Zero, new Vector2[5] { new Vector2(-50, 0), new Vector2(0, -50), new Vector2(50, 0), new Vector2(50, 50), new Vector2(-50, 50) });
			house.position = new Vector2(550, 200);
			house.texture = Content.Load<Texture2D>("house.png");
			house.offset = new Vector2(50.0f);
			house.colour = Color.White;
			shapes.Add(house);

			//spriteBatch.Draw(circleTex, circlePos - new Vector2(circle.Radius), circleColour);
			//spriteBatch.Draw(triangleTex, trianglePos - new Vector2(50), triangleColour);
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
			if (Keyboard.GetState().IsKeyDown(Keys.F1))
				state = 0;
			if (Keyboard.GetState().IsKeyDown(Keys.F2))
				state = 1;
			if (Keyboard.GetState().IsKeyDown(Keys.F3))
				state = 2;
			if (Keyboard.GetState().IsKeyDown(Keys.F4))
				state = 3;

			switch (state)
			{
				case 0:
					circle.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
					break;
				case 1:
					triangle.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
					break;
				case 2:
					square.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
					break;
				case 3:
					house.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
					break;
			}

			for (int i = 0; i < shapes.Count; ++i)
				for (int j = i + 1; j < shapes.Count; ++j)
					shapes[i].CollisionStuff(shapes[j]);

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
			foreach (Shape shape in shapes)
				shape.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
