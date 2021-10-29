using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DustyLaserCannon
{
    public class ShoddyBeam : ModProjectile
    {
        private int blinkTimer = 5;
        private float extraOpacity = 1f;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Lighting.AddLight(projectile.Center, Color.White.ToVector3() * projectile.Opacity * 0.2f);

            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 60, 0);
        }

        private void RunBlinkTimer()
        {
            if (--blinkTimer <= 0)
            {
                if (extraOpacity == 0f)
                {
                    blinkTimer = Main.rand.Next(3, 4);
                    extraOpacity = 1f;
                } else
                {
                    blinkTimer = Main.rand.Next(2, 4);
                    extraOpacity = 0f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == projectile.owner)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ShoddyExplosion"), projectile.Center);
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShoddyExplosion>(), 0, 0f, projectile.owner);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            RunBlinkTimer();

            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, texture.Height / 2);
            Color color = GetAlpha(lightColor) ?? lightColor;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color * projectile.Opacity * extraOpacity,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
