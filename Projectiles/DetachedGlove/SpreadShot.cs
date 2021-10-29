using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class SpreadShot : ModProjectile
    {
        private readonly Color dustColor = new Color(1f, 0.2f, 0.2f);

        private bool init = true;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 16;
            projectile.friendly = true;
            projectile.timeLeft = 10;
            projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
            projectile.alpha = 255;
            projectile.GamerProjectile().gamer = true;
        }

        public bool LongerLife => projectile.ai[0] == 1f;

        public override void AI()
        {
            if (init)
            {
                if (LongerLife)
                {
                    projectile.timeLeft += 6;
                }
                init = false;
            }

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 100, 0);

            if (++projectile.frameCounter > 2)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            if (Main.rand.NextBool(12))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, newColor: dustColor);
                dust.noGravity = true;
                dust.velocity = projectile.velocity * 0.06f;
            }

            Lighting.AddLight(projectile.Center, Color.DarkRed.ToVector3());
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 10, 0.6f);
            GamerUtils.DustExplosion(7, projectile.Center, DustID.BubbleBurst_White, 3f, baseRotation: Main.rand.NextFloat(MathHelper.Pi), color: dustColor);

            if (Main.rand.NextBool(3))
            {
                int dusts = 45;
                float separation = MathHelper.TwoPi / dusts;
                float speed = Main.rand.NextFloat(2.5f, 3f);

                for (int d = 0; d < dusts; d++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.BubbleBurst_White, newColor: dustColor);
                    dust.velocity = Vector2.UnitY.RotatedBy(d * separation) * speed;
                    dust.noGravity = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width, frameHeight) / 2;
            Color color = projectile.GetAlpha(lightColor);

            // Afterimages
            int trails = ProjectileID.Sets.TrailCacheLength[projectile.type];
            for (int i = 0; i < trails; i++)
            {
                spriteBatch.Draw(
                    texture,
                    projectile.oldPos[i] + projectile.Size / 2f - Main.screenPosition,
                    sourceRectangle,
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
