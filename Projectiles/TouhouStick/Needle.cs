using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.TouhouStick
{
    public class Needle : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 6;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
            projectile.timeLeft = 360;
            projectile.scale = 0.8f;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 50, 50);

            if (Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.WhiteTorch);
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 0.5f;
                dust.fadeIn = 1.3f;
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            for (int d = 0; d < 2; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.WhiteTorch);
                dust.velocity *= 3f;
                dust.fadeIn = 1.2f;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
            Color color = GetAlpha(lightColor) ?? lightColor;

            // Afterimages
            int trails = 7;
            for (int i = 1; i <= trails; i++)
            {
                int reverseIndex = trails - i + 1;
                Vector2 position = projectile.Center - projectile.velocity * reverseIndex * 0.2f;

                spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    color * projectile.Opacity * 0.1f,
                    projectile.rotation,
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
