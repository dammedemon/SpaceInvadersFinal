using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace spaceInvaders
{
    class Enemy
    {
        private Texture2D enemyTex;
        private Vector2 enemyPos;
        private int enemySpeed = 30;

        public Vector2 Position => enemyPos;

        public Rectangle hitBox;

        private float timer;

        public Enemy(Texture2D enemyTex, Vector2 enemyPos)
        {
            this.enemyTex = enemyTex;
            this.enemyPos = enemyPos;

            hitBox = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemyTex.Width, enemyTex.Height);
        }

        public void Update(GameTime gameTime)
        {
            hitBox.Location = enemyPos.ToPoint();
            
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= 1)
            {
                enemyPos.Y += enemySpeed;
                timer = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(enemyTex, enemyPos, Color.White);
        }
    }
}