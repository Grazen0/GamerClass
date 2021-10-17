using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles
{
    public class FlyingHammer : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 14;
            projectile.hostile = false;
            projectile.friendly = true;
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

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color * projectile.Opacity,
                projectile.rotation,
                origin,
                projectile.scale,
                projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
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
