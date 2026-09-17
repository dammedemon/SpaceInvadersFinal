using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace spaceInvaders
{
    class Enemy
    {
        private Texture2D enemyTex;
        private Vector2 enemyPos;
        private int enemySpeed;

        public Enemy(Texture2D enemyTex, Vector2 enemyPos, int enemySpeed)
        {
            this.enemyTex = enemyTex;
            this.enemySpeed = enemySpeed;
            this.enemyPos = new Vector2(posX, posY);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(enemyTex, enemyPos, Color.White);
        }
    }
}