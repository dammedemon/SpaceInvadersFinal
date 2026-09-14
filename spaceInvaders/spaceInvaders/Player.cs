using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace spaceInvaders
{
    class Player
    {
        public Texture2D playerTex;
        public Vector2 playerPos;
        public int stopX;
        public int playerSpeed = 5;

        public Player(Texture2D playerTex, Vector2 playerPos, int stopX)
        {
            this.playerTex = playerTex;
            this.playerPos = playerPos;
            this.stopX = stopX;
        }
        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && playerPos.X > 0)
            {
                playerPos.X -= playerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && playerPos.X < stopX)
            {
                playerPos.X += playerSpeed;
            }
        }
    }
}