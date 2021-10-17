using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.TouhouStick
{
    public class OrangeCharm : ModProjectile
    {
        private readonly float rotationSpeed = MathHelper.TwoPi / 180f;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 32;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 360;
            projectile.scale = 0.8f;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            projectile.rotation += rotationSpeed;
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 40, 0);

            if (Main.rand.NextBool(20))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.OrangeTorch);
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 0.5f;
                dust.fadeIn = 1.2f;
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            for (int d = 0; d < 4; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.OrangeTorch);
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = texture.Size() / 2f;

            Color color = GetAlpha(lightColor) ?? lightColor;

            // Afterimages
            int trails = 5;
            for (int i = 1; i <= trails; i++)
            {
                int reverseIndex = trails - i + 1;
                Vector2 position = projectile.Center - projectile.velocity * reverseIndex * 0.22f;

                spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    color * (projectile.Opacity * i * 0.06f),
                    projectile.rotation - rotationSpeed * i,
                    origin,
                    projectile.scale,
                    SpriteEffects.None,
                    0f);
            }

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
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
