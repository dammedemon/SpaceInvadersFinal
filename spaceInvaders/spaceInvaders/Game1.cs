using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace spaceInvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch sb;
    
    enum GameState 
    {
        MainMenu,
        InGame,   
        GameOver
    }

    Player player;
    EnemyManager enemyManager;
    public List<Bullet> bullets = new List<Bullet>();

    public int windowWidth = 488; 
    public int windowHeight = 612;
    
    public int score = 0;
    public int livesLeft = 3;

    SpriteFont spriteFont;
    Vector2 scoreTextPos;
    Vector2 livesTextPos;

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

        spriteFont = Content.Load<SpriteFont>("SpriteFont");

        
        scoreTextPos = new Vector2(0, 0);
        livesTextPos = new Vector2(400, 0);
        
        // Loads the texture of the bullet
        Texture2D bulletTex = Content.Load<Texture2D>("space_bulletTex");
       
        // Spawning player
        Texture2D playerTex = Content.Load<Texture2D>("space__0006_Player");
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height);
        
        int stopX = windowWidth - playerTex.Width;
        player = new Player(playerTex, playerPos, stopX, bulletTex, bullets);
        
        // Spawning enemies
        Texture2D enemyTex = Content.Load<Texture2D>("space__0002_B1");
        enemyManager = new EnemyManager(enemyTex);
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

        // Updating enemy position
        enemyManager.Update(gameTime, windowWidth, windowHeight, bullets);

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

        enemyManager.Draw(sb);

        sb.DrawString(spriteFont, "Score: " + score, scoreTextPos, Color.Lime); 
        sb.DrawString(spriteFont, "Lives: " + livesLeft, livesTextPos, Color.Lime);
        
        sb.End();
        base.Draw(gameTime);
    }
}
