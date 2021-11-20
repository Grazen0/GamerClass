using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons.DustyLaserCannon
{
    public class ShoddyExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 42;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.light = 0.3f;
            projectile.timeLeft = 30;
            projectile.GamerProjectile().gamer = true;
        }

        private void RunAnimation()
        {
            if (++projectile.frameCounter > 5)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }

        public override bool CanDamage() => false;

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            RunAnimation();

            int pad = 6;
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type] - pad;

            Vector2 origin = new Vector2(texture.Width, frameHeight) / 2;
            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * (frameHeight + pad), frameHeight, texture.Width);
            Color color = projectile.GetAlpha(lightColor);

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                color * projectile.Opacity,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
