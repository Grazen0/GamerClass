using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.TouhouStick
{
    public class Needle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

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
            projectile.GamerProjectile().gamer = true;
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
            Color color = projectile.GetAlpha(lightColor);

            // Afterimages
            int trails = ProjectileID.Sets.TrailCacheLength[projectile.type];
            for (int i = 0; i < trails; i++)
            {
                spriteBatch.Draw(
                    texture,
                    projectile.oldPos[i] + projectile.Size / 2f - Main.screenPosition,
                    null,
                    color * 0.5f * (1f - ((float)i / trails)),
                    projectile.oldRot[i],
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
