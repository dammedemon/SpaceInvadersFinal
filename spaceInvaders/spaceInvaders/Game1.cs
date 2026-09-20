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
    
    public const int windowWidth = 750; 
    public const int windowHeight = 1000;
    
    int score;
    int livesLeft = 3;

    SpriteFont endText;
    Vector2 endTextPos;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Determine window size 
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
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height);
        
        int stopX = Window.ClientBounds.Width - playerTex.Width;
        player = new Player(playerTex, playerPos, stopX, bulletTex, bullets);
        
        Texture2D enemyTex = Content.Load<Texture2D>("single_alien");
        for (int column = 0; column < 6; column++)
        {
            for (int row = 0; row < 3; row++)
            {
                Vector2 enemyPos = new Vector2(column * 110 + 50, row * 110 + 50);
                Enemy enemy = new Enemy(enemyTex, enemyPos);
                enemies.Add(enemy);
            }
        }

        endText = Content.Load<SpriteFont>("endText");
        endTextPos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2); 
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        player.Update();

        // Updating bullet position and hitbox
        foreach (var bullet in bullets)
        {
            bullet.Update();
        }

        // Removing bullets if they cross outside of the game window
        for (int k = bullets.Count - 1; k >= 0; k--)
        {
            if (bullets[k].Position.Y <= 0)
            {
                bullets.Remove(bullets[k]);
            }
        }

        // Updating enemy position and hitbox
        foreach (var enemy in enemies)
        {
            enemy.Update();
        }

        // Handling bullet collision and removing bullets and enemies as they collide and adding points
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            for (int k = bullets.Count - 1; k >= 0; k--)
            {
                if (enemies[i].hitBox.Intersects(bullets[k].hitBox))
                {
                    enemies.Remove(enemies[i]);
                    bullets.Remove(bullets[k]);
                    score += 100;
                    break;
                }
            }
        }

        // Removes a life if an enemy reaches the bottom of the screen
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].hitBox.Y + enemies[i].hitBox.Height > windowHeight)
            {
                enemies.RemoveAt(i);
                livesLeft--;
            }
        }

        
        if (livesLeft <= 0)
        {
            bullets.Clear();
            enemies.Clear();
        }

        // Updating window title so score and lives change as they update
        Window.Title = "Space Invaders! | Score: " + score + " | Lives: " + livesLeft;
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
        
        if (livesLeft <= 0)
        {
            sb.DrawString(endText, "You lost", endTextPos, Color.Red);
        }
        
        sb.End();
        base.Draw(gameTime);
    }
}
