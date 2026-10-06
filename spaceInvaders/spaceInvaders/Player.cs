using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace spaceInvaders
{
    class Player
    {
        private Texture2D playerTex;
        private Point playerSize;
        private Vector2 playerPos;
        private float playerSpeed = 2.5f;
        private int playerStopX;

        public Rectangle playerRec;
        
        private KeyboardState previousKeyboardState;
        private List<Bullet> bullets;
        private Texture2D bulletTex;
        

        public Vector2 Position => playerPos;

        public Player(Texture2D playerTex, Vector2 playerPos, int playerStopX, Texture2D bulletTex, List<Bullet> bullets, Point playerSize, Rectangle playerRec)
        {
            this.playerTex = playerTex;
            this.playerPos = playerPos;
            this.playerStopX = playerStopX;
            this.bulletTex = bulletTex;
            this.bullets = bullets;
            this.playerSize = playerSize; 
            this.playerRec = playerRec;
        }
        public void Update()
        {
            // Left & right movement 
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && playerPos.X > 0)
            {
                playerPos.X -= playerSpeed;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && playerPos.X < playerStopX)
            {
                playerPos.X += playerSpeed;
            }

            // Shooting, making sure its semi automatic
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                Vector2 bulletPos = new Vector2(playerPos.X + playerSize.X / 2 - bulletTex.Width / 2, playerPos.Y);
                Bullet bullet = new Bullet(bulletTex, bulletPos);
                bullets.Add(bullet);
            }
            
            previousKeyboardState = currentKeyboardState;
            playerRec.Location = playerPos.ToPoint(); 
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTex, playerRec, Color.White);
        }
    }
}