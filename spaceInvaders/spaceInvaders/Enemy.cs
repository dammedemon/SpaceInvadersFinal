using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace spaceInvaders
{
    public class Enemy
    {
        private Texture2D enemyTex;
        private Vector2 enemyPos;
        private int enemySpeed = 30;
        private int moveDownSpeed = 30;
        private int stopX;

        public Vector2 Position => enemyPos;

        public Rectangle hitBox;

        private float timer;

        public Enemy(Texture2D enemyTex, Vector2 enemyPos)
        {
            this.enemyTex = enemyTex;
            this.enemyPos = enemyPos;

            hitBox = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemyTex.Width, enemyTex.Height);
        }

        public void Update(GameTime gameTime, int enemyDirection)
        {
            // Keeps hitbox at enemy location
            hitBox.Location = enemyPos.ToPoint();
            
            // Moves the enemy when timer equals one or higher then resets timer
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= 0.5)
            {
                enemyPos.X += enemySpeed * enemyDirection;
                timer = 0;
            }
        }

        public void MoveDown()
        {
            enemyPos.Y += moveDownSpeed;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(enemyTex, enemyPos, Color.White);
        }
    }
}