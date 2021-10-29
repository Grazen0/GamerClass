using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class PeaShot : ModProjectile
    {
        private float xScale = 05f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.friendly = true;
            projectile.timeLeft = 300;
            projectile.scale = 1.8f;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            xScale = MathHelper.Min(xScale + 0.01f, 1f);

            for (int d = 0; d < 2; d++)
            {
                Vector2 position = projectile.position - projectile.velocity * Main.rand.NextFloat(1f);
                Dust dust = Dust.NewDustDirect(position, projectile.width, projectile.height, DustID.IceTorch, Scale: 1.8f, Alpha: projectile.alpha);
                dust.noGravity = true;
                dust.alpha = projectile.alpha;
                dust.fadeIn = 0f;
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 2f;
            }

            if (++projectile.frameCounter > 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            Lighting.AddLight(projectile.Center, Color.Cyan.ToVector3() * 0.8f);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            int lines = 4;
            for (int l = 0; l < lines; l++)
            {

                Vector2 direction = Main.rand.NextVector2CircularEdge(1f, 1f);
                float maxSpeed = Main.rand.NextFloat(5f, 6f);
                float dusts = Main.rand.Next(2, 8);
                float scale = Main.rand.NextFloat(0.5f, 1.5f);

                for (int d = 0; d < dusts; d++)
                {
                    float speed = maxSpeed - (d * 0.4f);

                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Frost);
                    dust.noGravity = true;
                    dust.scale = scale;
                    dust.velocity = direction * speed;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor, flip: false);
            return false;
        }
    }
}
