using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace spaceInvaders
{
    class Bullet
    {
        private Texture2D bulletTex;
        private Vector2 bulletPos;
        private int bulletSpeed = 10;

        public Bullet(Texture2D bulletTex, Vector2 bulletPos)
        {
            this.bulletTex = bulletTex;
            this.bulletPos = bulletPos;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                bulletPos.Y -= bulletSpeed;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(bulletTex, bulletPos, Color.White);
        }
    }
}