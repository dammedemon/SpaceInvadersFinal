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
        private float playerSpeed = 2.5f;
        private int stopX;
        
        private KeyboardState previousKeyboardState;
        private List<Bullet> bullets;
        private Texture2D bulletTex;

        public Vector2 Position => playerPos;

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

            // Shooting, making sure its semi automatic & only one bullet at a time
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space) && bullets.Count == 0)
            {
                Vector2 bulletPos = new Vector2(playerPos.X + playerTex.Width / 2 - bulletTex.Width / 2, playerPos.Y);
                Bullet bullet = new Bullet(bulletTex, bulletPos);
                bullets.Add(bullet);
            }
            
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTex, playerPos, Color.LimeGreen);
        }
    }
}