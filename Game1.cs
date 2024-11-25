using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootingGallery;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D targetSprite;
    private Texture2D crosshairSprite;
    private Texture2D backgroundSprite;
    private SpriteFont gameFont;

    private Vector2 targetPosition = new(300, 300);
    private const int targetRadius = 45;
    private const int crosshairRadius = 25;

    private MouseState mouseState;
    private bool mouseReleased = true;

    private int score = 0;
    private double timer = 20;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        targetSprite = Content.Load<Texture2D>("target");
        crosshairSprite = Content.Load<Texture2D>("crosshair");
        backgroundSprite = Content.Load<Texture2D>("sky");
        gameFont = Content.Load<SpriteFont>("galleryFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (timer > 0)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (timer < 0) {
            timer = 0;
        }

        mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed && mouseReleased == true)
        {
            mouseReleased = false;

            float mouseTargetDistance = Vector2.Distance(targetPosition, mouseState.Position.ToVector2());

            if (mouseTargetDistance < targetRadius && timer > 0)
            {
                score++;

                int offset = 50 + targetRadius;
                Random random = new();

                targetPosition.X = random.Next(offset, graphics.PreferredBackBufferWidth - offset);
                targetPosition.Y = random.Next(offset + 30, graphics.PreferredBackBufferHeight - offset);
            }
            else if (timer > 0)
            {
                score--;
            }
        }

        if (mouseState.LeftButton == ButtonState.Released)
        {
            mouseReleased = true;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();

        spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
        spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(10, 5), Color.White);
        spriteBatch.DrawString(gameFont, "Time left: " + Math.Ceiling(timer).ToString(), new Vector2(10, 40), Color.White);

        if (timer > 0)
        {
            spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
        }

        spriteBatch.Draw(crosshairSprite, new Vector2(mouseState.X - crosshairRadius, mouseState.Y - crosshairRadius), Color.White);

        spriteBatch.End();

        base.Draw(gameTime);
    }
}
