using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace spaceInvaders
{
    class Enemy
    {
        private Texture2D enemyTex;
        private Vector2 enemyPos;
        private int enemySpeed = 1;

        public Rectangle hitBox;

        public Enemy(Texture2D enemyTex, Vector2 enemyPos)
        {
            this.enemyTex = enemyTex;
            this.enemyPos = enemyPos;

            hitBox = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemyTex.Width, enemyTex.Height);
        }

        public void Update()
        {
            enemyPos.Y += enemySpeed;

            hitBox.X = (int)enemyPos.X;
            hitBox.Y = (int)enemyPos.Y;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(enemyTex, enemyPos, Color.White);
        }
    }
}