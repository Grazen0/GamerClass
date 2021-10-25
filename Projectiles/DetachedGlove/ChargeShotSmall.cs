using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class ChargeShotSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 10;
            projectile.friendly = true;
            projectile.timeLeft = 300;
            projectile.scale = 1.5f;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 80, 0);

            if (++projectile.frameCounter > 5)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AmberBolt);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 2;
            }

            Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() * 0.6f);
        }

        public override void Kill(int timeLeft)
        {
            int lines = 7;
            float separation = MathHelper.TwoPi / lines;
            Vector2 direction = Main.rand.NextVector2CircularEdge(1f, 1f);

            for (int l = 0; l < lines; l++)
            {
                for (int d = 0; d < 6; d++)
                {
                    float speed = 4f - (d * 0.3f);

                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.AmberBolt);
                    dust.scale = 0.8f;
                    dust.noGravity = true;
                    dust.velocity = direction * speed;
                }

                direction = direction.RotatedBy(separation);
            }

            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.AmberBolt);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, frameHeight / 2);
            Color color = GetAlpha(lightColor) ?? lightColor;

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
