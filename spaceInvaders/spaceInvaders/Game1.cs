using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;


namespace spaceInvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch sb;
    
    Player player;
    
    List<Bullet> bullets = new List<Bullet>();
    Enemy[,] enemies;
    
    public int windowWidth = 600; 
    public int windowHeight = 900;
    
    int score = 0;
    int livesLeft = 3;

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
        Texture2D playerTex = Content.Load<Texture2D>("space__0006_Player");
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height);
        
        int stopX = Window.ClientBounds.Width - playerTex.Width;
        player = new Player(playerTex, playerPos, stopX, bulletTex, bullets);
        
        // Spawning enemies
        Texture2D enemyTex = Content.Load<Texture2D>("space__0002_B1");
        enemies = new Enemy[5,6];
        for (int row = 0; row < 5; row++) 
        { 
            for (int column = 0; column < 6; column++) 
            { 
                int x = column * 50;             
                int y = row * 50; 
                Vector2 enemyPos = new Vector2(x,y);
                enemies[row,column] = new Enemy(enemyTex, enemyPos); 
            } 
        }
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
        for (int row = 0; row < enemies.GetLength(0); row++)
        {
            for (int column = 0; column < enemies.GetLength(1); column++)
            {
                if (enemies[row,column] != null)
                {
                    enemies[row,column].Update(gameTime);   
                }
            }
            
        }

        // Looping through each enemy and bullet to see if they collide to then remove them and add points
        for (int row = 0; row < enemies.GetLength(0); row++)
        {
            for (int column = 0; column < enemies.GetLength(1); column++)
            {
                if (enemies[row, column] == null)
                {
                    continue;
                }
                
                for (int k = bullets.Count - 1; k >= 0; k--)
                {
                    if (enemies[row,column].hitBox.Intersects(bullets[k].hitBox))
                    {
                        enemies[row, column] = null;
                        bullets.Remove(bullets[k]);
                        score += 100;
                        break;
                    }
                }
            }
        }

        // Removes a life if the bottom of an enemy reaches the bottom of the screen
        // for (int i = enemies.Count - 1; i >= 0; i--)
        // {
        //     if (enemies[i].hitBox.Y + enemies[i].hitBox.Height > windowHeight)
        //     {
        //         enemies.RemoveAt(i);
        //         livesLeft--;
        //     }
        // }

        // Removes all bullets and enemies from screen if lives reach zero or goes below
        // if (livesLeft <= 0)
        // {
        //     bullets.Clear();
        //     enemies.Clear();
        // }

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
        // foreach (var enemy in enemies)
        // {
        //     enemy.Draw(sb);    
        // }

        for (int row = 0; row < enemies.GetLength(0); row++) 
        { 
            for (int column = 0; column < enemies.GetLength(1); column++) 
            { 
                if (enemies[row, column] != null)
                {
                    enemies[row, column].Draw(sb);    
                }
            } 
        }
        
        sb.End();
        base.Draw(gameTime);
    }
}
