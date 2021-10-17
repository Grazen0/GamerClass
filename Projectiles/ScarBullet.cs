using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles
{
    public class ScarBullet : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.None;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 2;
            projectile.friendly = true;
            projectile.extraUpdates = 20;
            projectile.timeLeft = 60 * projectile.extraUpdates;
        }

        public override void AI()
        {
            Vector2 direction = Vector2.Normalize(projectile.velocity);
            float offsetSpread = 2f;

            for (int d = 0; d < 3; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.BubbleBurst_White, Scale: 1f);

                dust.position += direction * Main.rand.NextFloat(-offsetSpread, offsetSpread);
                dust.rotation = Main.rand.NextFloat(MathHelper.PiOver2);
                dust.noGravity = true;
                dust.fadeIn = 0f;
                dust.velocity = direction.RotatedBy(MathHelper.PiOver2 * (Main.rand.NextBool() ? 1 : -1)) * dust.velocity.Length() * 0.1f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            float spread = MathHelper.PiOver2 * 0.8f;

            for (int d = 0; d < 5; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.BubbleBurst_White, Scale: 1.3f);
                dust.velocity = -projectile.velocity.RotatedBy(Main.rand.NextFloat(-spread, spread)) * dust.velocity.Length() * 0.3f;
                dust.noGravity = true;
                dust.fadeIn = 0f;
            }
        }
    }
}
