using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace spaceInvaders;

public class Game1 : Game
{
    Player player;
    List<Bullet> bullets = new List<Bullet>();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    int score = 1000;
    int width = 500; 
    int height = 1000;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Determine window size & title
        Window.Title = "Space Invaders! | Score: " + score;
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D bulletTex = Content.Load<Texture2D>("bullet_SI-1");
        Texture2D playerTex = Content.Load<Texture2D>("Ship_01-1");
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height - 20);
        
        int stopX = Window.ClientBounds.Width - playerTex.Width;

        player = new Player(playerTex, playerPos, stopX, bulletTex, bullets);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        player.Update();

        foreach (var bullet in bullets)
        {
            bullet.Update();
        }

        base.Update(gameTime);
    }

     protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Purple);

        _spriteBatch.Begin();
        
        player.Draw(_spriteBatch);
        
        // Draws bullets on screen
        foreach (var bullet in bullets)
        {
            bullet.Draw(_spriteBatch);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
