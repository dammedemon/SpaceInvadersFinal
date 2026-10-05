using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace spaceInvaders
{
    public class EnemyManager
    {
        private int enemyColumns = 8;
        private int enemyRows = 5;
        
        public int livesLeft = 3;
        public int score = 0;

        private int enemyDirection = 1;
        public Enemy[,] enemies;

        SpriteFont spriteFont;
        Vector2 scoreTextPos;
        Vector2 livesTextPos;

        public EnemyManager(Texture2D enemyTex, SpriteFont spriteFont, Vector2 scoreTextPos, Vector2 livesTextPos)
        {
            this.spriteFont = spriteFont;
            this.scoreTextPos = scoreTextPos;
            this.livesTextPos = livesTextPos;
            
            enemies = new Enemy[enemyRows, enemyColumns];
            
            for (int row = 0; row < enemyRows; row++) 
            { 
                for (int column = 0; column < enemyColumns; column++) 
                { 
                    int x = column * 35 + 20;             
                    int y = row * 22 + 50; 
                    Vector2 enemyPos = new Vector2(x, y);
                    enemies[row, column] = new Enemy(enemyTex, enemyPos); 
                } 
            }
        }

        public void Update(GameTime gameTime, int windowWidth, int windowHeight, List<Bullet> bullets)
        {
            
            // Updates the the enemies that aren't null
            for (int row = 0; row < enemies.GetLength(0); row++)
            {
                for (int column = 0; column < enemies.GetLength(1); column++)
                {
                    if (enemies[row, column] != null)
                    {
                            enemies[row, column].Update(gameTime, enemyDirection);
                    }
                }
            }
            
            // When the enemy hits a wall it goes down one step and changes direction
            float rightMostX = 0; // Variable for the right most x coordinate of the entire enemy block

            for (int row = 0; row < enemies.GetLength(0); row++)
            {
                for (int column = 0; column < enemies.GetLength(1); column++)
                {
                    if (enemies[row, column] != null)
                    {
                        if (enemies[row, column].hitBox.Right > rightMostX) // If statement to update the variable to always be the highest X coordinate 
                        {
                            rightMostX = enemies[row, column].hitBox.Right; 
                        }
                    }
                }
            }

            if (rightMostX >= windowWidth && enemyDirection == 1) // If the right side of the enemy block reaches an edge and they're moving right, it goes down a space.
            {
                for (int row = 0; row < enemies.GetLength(0); row++)
                {
                    for (int column = 0; column < enemies.GetLength(1); column++)
                    {
                        if (enemies[row, column] != null)
                        {
                            enemies[row, column].MoveDown(); // calls for movedown function in enemy class 
                            enemyDirection = -1; // Changes enemyDirection so enemy block starts moving left 
                        }
                    }
                }
            }
            
            // Same as above but for the left side
            float leftMostX = windowWidth; 

            for (int row = 0; row < enemies.GetLength(0); row++)
            {
                for (int column = 0; column < enemies.GetLength(1); column++)
                {
                    if (enemies[row, column] != null)
                    {
                        if (enemies[row, column].hitBox.Left < leftMostX)
                        {
                            leftMostX = enemies[row, column].hitBox.Left;
                        }
                    }
                }
            }

            if (leftMostX <= 0 && enemyDirection == -1)
            {
                for (int row = 0; row < enemies.GetLength(0); row++)
                {
                    for (int column = 0; column < enemies.GetLength(1); column++)
                    {
                        if (enemies[row, column] != null)
                        {   
                            enemies[row, column].MoveDown();
                            enemyDirection = 1;
                        }
                    }
                }
            }

            // Looping through each enemy and bullet to see if they collide to then remove them both and add points
            for (int row = 0; row < enemies.GetLength(0); row++)
            {
                for (int column = 0; column < enemies.GetLength(1); column++)
                {
                    if (enemies[row, column] != null) // If the enemy exists -> check for collision between bullet and enemy
                    {
                        for (int k = bullets.Count - 1; k >= 0; k--)
                        {
                            if (enemies[row, column].hitBox.Intersects(bullets[k].hitBox))
                            {
                                enemies[row, column] = null;
                                bullets.Remove(bullets[k]);
                                score += 100;
                                break;
                            }
                        }
                    }
                }
            }

            // Removes a life if the bottom of an enemy reaches the bottom of the screen
            for (int row = 0; row < enemies.GetLength(0); row++)
            {
                for (int column = 0; column < enemies.GetLength(1); column++)
                {
                    if (enemies[row, column] != null)
                    {
                        if (enemies[row, column].hitBox.Y + enemies[row,column].hitBox.Height > windowHeight)
                        {   
                            enemies[row, column] = null;
                            livesLeft--;
                        }
                    }
                }
            }
        }

        // Method to see if all enemies have been defeated to then change the game state to game over
        public bool AllEnemiesDefeated()
        {
            for (int row = 0; row < enemies.GetLength(0); row++)
            {
                for (int column = 0; column < enemies.GetLength(1); column++)
                {
                    if (enemies[row, column] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        // Draws out all the enemies that aren't null & score and lives left
        public void Draw(SpriteBatch sb)
        {
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

            sb.DrawString(spriteFont, "Score: " + score, scoreTextPos, Color.Lime); 
            sb.DrawString(spriteFont, "Lives: " + livesLeft, livesTextPos, Color.Lime);
        }
    }
}