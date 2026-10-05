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
    
    // Enum to handle the different game states
    enum GameState 
    {
        MainMenu,
        InGame,   
        GameOver
    }

    GameState currentGameState = GameState.MainMenu;
    
    // Declaring my different classes
    Player player;
    EnemyManager enemyManager;
    public List<Bullet> bullets = new List<Bullet>();

    Random rnd;
    Vector2[] posArray;

    // Declaring the window size
    public int windowWidth = 488; 
    public int windowHeight = 612;

    // Declaring variables for the tank animation in the main menu
    int frame;
    double frameTimer;
    double frameInterval;

    // Declaring all the textures and rectangles for the different game states
    Texture2D mainMenuBackground;
    Texture2D gameOverBackground;
    Texture2D inGameBackground;
    
    Texture2D startButton;
    Rectangle startButtonRect;

    Texture2D tankSpriteSheet;
    Rectangle srcRec;

    Texture2D ball;

    // Declaring the spritefont and text positions for score and lives left
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
        Window.Title = "Space Invaders!";
        _graphics.ApplyChanges();

        frameTimer = 100;
        frameInterval = 0;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        sb = new SpriteBatch(GraphicsDevice);
        rnd = new Random();

        // Loading the spritefont and positions for the lives and score
        spriteFont = Content.Load<SpriteFont>("SpriteFont");
        scoreTextPos = new Vector2(0, 0);
        livesTextPos = new Vector2(400, 0);
        
        // Loading all the textures that'll be used for the different game states
        mainMenuBackground = Content.Load<Texture2D>("space_light");
        startButton = Content.Load<Texture2D>("Startknapp");
        gameOverBackground = Content.Load<Texture2D>("Purple_space");
        inGameBackground = Content.Load<Texture2D>("space_background");
        startButtonRect = new Rectangle(Window.ClientBounds.Width / 2 - startButton.Width / 2, Window.ClientBounds.Height / 2 - startButton.Height / 2, startButton.Width, startButton.Height);

        tankSpriteSheet = Content.Load<Texture2D>("spriteSheet_Tanks");
        srcRec = new Rectangle(0, 288, 32, 32);

        ball = Content.Load<Texture2D>("ball");

        // Loads the texture of the bullet
        Texture2D bulletTex = Content.Load<Texture2D>("space_bulletTex");
       
        // Spawning player
        Texture2D playerTex = Content.Load<Texture2D>("space__0006_Player");
        Vector2 playerPos = new Vector2(Window.ClientBounds.Width / 2 - playerTex.Width / 2, Window.ClientBounds.Height - playerTex.Height);
        
        // Sets stopX value so player can't go past the bounds of the window
        int stopX = windowWidth - playerTex.Width;
        player = new Player(playerTex, playerPos, stopX, bulletTex, bullets);
        
        // Spawning enemies
        Texture2D enemyTex = Content.Load<Texture2D>("space__0002_B1");
        enemyManager = new EnemyManager(enemyTex, spriteFont, scoreTextPos, livesTextPos);

        // Spawning 5 random sprites in the game over screen
        int randStopX = windowWidth - ball.Width;
        int randStopY = windowHeight - ball.Height;

        posArray = new Vector2[5];

        for (int i = 0; i < 5; i++)
        {
            int randX = rnd.Next(0, randStopX);
            int randY = rnd.Next(0, randStopY);
            Vector2 pos = new Vector2(randX, randY);
            posArray[i] = pos;
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // switch statement to handle different game states
        switch (currentGameState)
        {
            case GameState.MainMenu:
                // Check if the start button is clicked and changes game state if it is
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Point mousePos = Mouse.GetState().Position;
                    if (startButtonRect.Contains(mousePos))
                    {
                        currentGameState = GameState.InGame;
                    }
                }
                break;
            
            case GameState.InGame:
                // Update player, enemies, and bullets when youre in game
                player.Update();
                enemyManager.Update(gameTime, windowWidth, windowHeight, bullets);
                
                // Updating each bullet
                foreach (var bullet in bullets) 
                {
                    bullet.Update();
                }
                
                // Removing bullets if they go off screen
                for (int k = bullets.Count - 1; k >= 0; k--)
                {
                    if (bullets[k].Position.Y <= 0)
                    {
                        bullets.Remove(bullets[k]);
                    }
                }

                // If you reach 0 lives or eliminate all enemies game state changes to game over
                if (enemyManager.livesLeft <= 0 || enemyManager.AllEnemiesDefeated())
                {
                    currentGameState = GameState.GameOver;
                }
                break;
            
            case GameState.GameOver:
                // Clears all bullets and sests each enemy to null when game is over
                bullets.Clear();
                for (int row = 0; row < enemyManager.enemies.GetLength(0); row++)
                {
                    for (int column = 0; column < enemyManager.enemies.GetLength(1); column++)
                    {
                        if (enemyManager.enemies[row, column] != null)
                        {
                            enemyManager.enemies[row, column] = null;
                        }
                    }
                }
                break;
        }

        // Tank animation in the main menu
        frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        if (frameTimer <= 0)
        {
            frameTimer = frameInterval;
            frame++;
            if (frame > 9)
            {
                frame = 0;
            }
            srcRec.X = frame * 32;
        }

        base.Update(gameTime);

    }

     protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        sb.Begin();

        // Draws the main menu and start button when the game state is main menu
        if (currentGameState == GameState.MainMenu)
        {
            sb.Draw(mainMenuBackground, Vector2.Zero, Color.White);
            Color buttonColor;

            // Darkens the start button if the mouse hovers over it
            if (startButtonRect.Contains(Mouse.GetState().Position))
            {
                buttonColor = Color.Gray;
            }
            else
            {
                buttonColor = Color.White;
            }

            sb.Draw(startButton, startButtonRect, buttonColor);
            sb.Draw(tankSpriteSheet, new Vector2(0,0), srcRec, Color.White);
        }
        
        // Draws everything the game needs when the game state is in game
        if (currentGameState == GameState.InGame)
        {
            sb.Draw(inGameBackground, Vector2.Zero, Color.White);
            player.Draw(sb);
            enemyManager.Draw(sb);
            foreach (var bullet in bullets)
            {
                bullet.Draw(sb);
            }
        }

        // Draws the game over background and text on screen
        if (currentGameState == GameState.GameOver)
        {   
            sb.Draw(gameOverBackground, Vector2.Zero, Color.White);
            
            foreach (var pos in posArray)
            {
                sb.Draw(ball, pos, Color.White);
            }

            sb.DrawString(spriteFont, "Game Over", new Vector2(windowWidth / 2 - 50, windowHeight / 3), Color.Black);
            sb.DrawString(spriteFont, "Final Score: " + enemyManager.score, new Vector2(windowWidth / 2 - 50, windowHeight / 3 + 50), Color.Black);
            sb.DrawString(spriteFont, "Press ESC to exit", new Vector2(windowWidth / 2 - 50, windowHeight / 3 + 100), Color.Black);
        }
            
        sb.End();
        base.Draw(gameTime);
    }
}
