using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles
{
    public class JetpackBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.friendly = true;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();

            Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.6f);
        }

        public override void Kill(int timeLeft)
        {
            for (int l = 0; l < 4; l++)
            {
                Vector2 direction = Main.rand.NextVector2CircularEdge(1f, 1f);
                float maxSpeed = Main.rand.NextFloat(3f, 5f);
                float dusts = Main.rand.Next(2, 8);
                float scale = Main.rand.NextFloat(1f, 1.2f);

                for (int d = 0; d < dusts; d++)
                {
                    float speed = maxSpeed - (d * 0.4f);

                    Dust dust = Dust.NewDustPerfect(projectile.Center, 64);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.scale = scale;
                    dust.velocity = direction * speed;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, texture.Height / 2);

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
