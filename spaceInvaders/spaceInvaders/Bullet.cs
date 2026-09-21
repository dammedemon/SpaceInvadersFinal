using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace spaceInvaders
{
    class Bullet
    {
        private Texture2D bulletTex;
        private Vector2 bulletPos;
        private int bulletSpeed = 7;

        public Rectangle hitBox;

        public Vector2 Position => bulletPos;

        public Bullet(Texture2D bulletTex, Vector2 bulletPos)
        {
            this.bulletTex = bulletTex;
            this.bulletPos = bulletPos;

            hitBox = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, bulletTex.Width, bulletTex.Height);
        }

        public void Update()
        {
            // Keeps hitbox location at bullet location
            hitBox.Location = bulletPos.ToPoint();
            bulletPos.Y -= bulletSpeed;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(bulletTex, bulletPos, Color.White);
        }
    }
}