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

    private MouseState mouseState;
    private bool mouseReleased = true;

    private int score = 0;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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

        mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed && mouseReleased == true)
        {
            mouseReleased = false;

            float mouseTargetDistance = Vector2.Distance(targetPosition, mouseState.Position.ToVector2());

            if (mouseTargetDistance < targetRadius)
            {
                score++;

                int offset = 50 + targetRadius;
                Random random = new();

                targetPosition.X = random.Next(offset, graphics.PreferredBackBufferWidth - offset);
                targetPosition.Y = random.Next(offset, graphics.PreferredBackBufferHeight - offset);
            }
            else
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
        spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
