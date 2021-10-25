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
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.friendly = true;
            projectile.timeLeft = 300;
            projectile.scale = 1.8f;
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
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width - 8, frameHeight / 2);
            Color color = GetAlpha(lightColor) ?? lightColor;
            Vector2 scale = new Vector2(xScale, 1f) * projectile.scale;

            // Afterimages
            if (xScale > 0.8f)
            {
                int trails = 8;
                for (int i = 1; i <= trails; i++)
                {
                    int reverseIndex = trails - i + 1;
                    Vector2 position = projectile.Center - projectile.velocity * reverseIndex * 0.3f;

                    spriteBatch.Draw(
                        texture,
                        position - Main.screenPosition,
                        sourceRectangle,
                        color * (projectile.Opacity * i * 0.06f),
                        projectile.rotation,
                        origin,
                        scale * 0.9f,
                        SpriteEffects.None,
                        0f);
                }
            }

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                color * projectile.Opacity,
                projectile.rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
