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
        
        // Loads the texture of the bullet
        Texture2D bulletTex = Content.Load<Texture2D>("bullet_SI-1");
       
        // Spawning player
        Texture2D playerTex = Content.Load<Texture2D>("Ship_01-1");
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height);
        
        int stopX = Window.ClientBounds.Width - playerTex.Width;
        player = new Player(playerTex, playerPos, stopX, bulletTex, bullets);
        
        // Spawning enemies
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

        // Text you see at the end of the game
        endText = Content.Load<SpriteFont>("endText");
        endTextPos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2); 
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        
        player.Update();

        // Updating bullet
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

        // Updating enemy
        foreach (var enemy in enemies)
        {
            enemy.Update(gameTime);
        }

        // Looping through each enemy and bullet to see if they collide to then remove them and add points
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

        // Removes a life if the bottom of an enemy reaches the bottom of the screen
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].hitBox.Y + enemies[i].hitBox.Height > windowHeight)
            {
                enemies.RemoveAt(i);
                livesLeft--;
            }
        }

        // Removes all bullets and enemies from screen if lives reach zero or goes below
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

        // Draws each bullet   
        foreach (var bullet in bullets)
        {
            bullet.Draw(sb);
        }

        // Draws each enemy
        foreach (var enemy in enemies)
        {
            enemy.Draw(sb);    
        }
        
        // Draws text on screen if you lose
        if (livesLeft <= 0)
        {
            sb.DrawString(endText, "You lost! Final score: " + score, endTextPos, Color.Red);
        }
        
        sb.End();
        base.Draw(gameTime);
    }
}
