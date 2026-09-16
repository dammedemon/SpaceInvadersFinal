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
        private Vector2 playerPos;
        private int stopX;
        private Texture2D bulletTex;
        private int playerSpeed = 5;
        
        private KeyboardState previousKeyboardState;
        private List<Bullet> bullets;

        public Player(Texture2D playerTex, Vector2 playerPos, int stopX, Texture2D bulletTex, List<Bullet> bullets)
        {
            this.playerTex = playerTex;
            this.playerPos = playerPos;
            this.stopX = stopX;
            this.bulletTex = bulletTex;
            this.bullets = bullets;
        }
        public void Update()
        {
            // Left & right movement 
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && playerPos.X > 0)
            {
                playerPos.X -= playerSpeed;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && playerPos.X < stopX)
            {
                playerPos.X += playerSpeed;
            }

            // Shooting & making sure its semi automatic
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                Vector2 bulletPos = new Vector2(playerPos.X + playerTex.Width / 2 - bulletTex.Width / 2, playerPos.Y);
                Bullet bullet = new Bullet(bulletTex, bulletPos);
                bullets.Add(bullet);
            }
            
            previousKeyboardState = currentKeyboardState;
        }

        public Vector2 Position => playerPos;

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(playerTex, playerPos, Color.White);
        }
    }
}