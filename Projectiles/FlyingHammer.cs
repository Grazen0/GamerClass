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
            projectile.friendly = true;
            projectile.scale = 0.8f;
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

            projectile.velocity.Y = MathHelper.Min(projectile.velocity.Y + 0.4f, 25f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCenteredAndFlip(spriteBatch, lightColor);
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
