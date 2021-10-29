using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles
{
    public class FlyingHammer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 14;
            projectile.friendly = true;
            projectile.GamerProjectile().gamer = true;
        }

        public float RotationTimer
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (++RotationTimer > 4)
            {
                RotationTimer = 0;
                projectile.rotation += MathHelper.PiOver2 * projectile.direction;
            }

            projectile.velocity.Y = MathHelper.Min(projectile.velocity.Y + 0.3f, 25f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = texture.Size() / 2;
            Color color = GetAlpha(lightColor) ?? lightColor;
            SpriteEffects spriteEffects = projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
            {
                Color trailColor = color * 0.5f;
                trailColor *= (float)(ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[projectile.type];

                spriteBatch.Draw(
                    texture,
                    projectile.oldPos[i] + (projectile.Size / 2) - Main.screenPosition, 
                    null, 
                    trailColor,
                    projectile.oldRot[i],
                    origin,
                    projectile.scale,
                    spriteEffects,
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
                spriteEffects,
                0f);

            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Asphalt);
            }
        }
    }
}
