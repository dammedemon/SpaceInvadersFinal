using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace spaceInvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch sb;
    
    Player player;
    List<Bullet> bullets = new List<Bullet>();
    List<Enemy> enemies = new List<Enemy>();
    public const int windowWidth = 500; 
    public const int windowHeight = 1000;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Determine window size & title
        Window.Title = "Space Invaders! | Score: ";
        _graphics.PreferredBackBufferWidth = windowWidth;
        _graphics.PreferredBackBufferHeight = windowHeight;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        sb = new SpriteBatch(GraphicsDevice);
        
        Texture2D bulletTex = Content.Load<Texture2D>("bullet_SI-1");
        Texture2D playerTex = Content.Load<Texture2D>("Ship_01-1");
        Texture2D enemyTex = Content.Load<Texture2D>("single_aline");
       
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height);
        int stopX = Window.ClientBounds.Width - playerTex.Width;
        player = new Player(playerTex, playerPos, stopX, bulletTex, bullets);
       
        for (int column = 0; column < 5; column++)
        {
            for (int row = 0; row < 3; row++)
            {
                Vector2 enemyPos = new Vector2(column * 100 + 50, row * 100 + 50);
                Enemy enemy = new Enemy(enemyTex, enemyPos);
                enemies.Add(enemy);
            }
        }
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
        GraphicsDevice.Clear(Color.Black);

        sb.Begin();
        player.Draw(sb); 
        
        foreach (var bullet in bullets)
        {
            bullet.Draw(sb);
        }

        foreach (var enemy in enemies)
        {
            enemy.Draw(sb);    
        }
        
        sb.End();
        base.Draw(gameTime);
    }
}
