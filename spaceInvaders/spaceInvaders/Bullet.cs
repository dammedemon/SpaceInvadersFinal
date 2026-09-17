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

        public Rectangle hitBox;

        public Bullet(Texture2D bulletTex, Vector2 bulletPos)
        {
            this.bulletTex = bulletTex;
            this.bulletPos = bulletPos;

            hitBox = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, bulletTex.Width, bulletTex.Height);
        }

        public void Update()
        {
            bulletPos.Y -= bulletSpeed;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(bulletTex, bulletPos, Color.White);
        }
    }
}