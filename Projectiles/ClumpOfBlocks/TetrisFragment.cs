using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.ClumpOfBlocks
{
    public class TetrisFragment : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 20;
            projectile.friendly = true;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.frame = (int)projectile.ai[0];

            projectile.velocity.X *= 0.98f;

            if (projectile.velocity.Y < 30f)
                projectile.velocity.Y += 1f;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10.WithVolume(0.5f), projectile.Center);

            Color color = Color.White;

            switch (projectile.ai[0])
            {
                case 0:
                    color = Color.IndianRed;
                    break;
                case 1:
                    color = Color.LightGreen;
                    break;
                case 2:
                    color = new Color(251, 130, 255); // Magenta-pink
                    break;
                case 3:
                    color = new Color(255, 224, 130); // Yellow-ish
                    break;
                case 4:
                    color = Color.CornflowerBlue;
                    break;
                case 5:
                    color = new Color(255, 164, 128); // Orange-ish
                    break;
                case 6:
                    color = Color.LightCyan;
                    break;
            }

            Vector2 baseDirection = -Vector2.Normalize(projectile.velocity);

            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, newColor: color, Scale: 1.5f);
                dust.noGravity = true;
                dust.velocity = baseDirection.RotatedByRandom(MathHelper.PiOver2) * dust.velocity.Length();
            }

            int dustsPerSide = 4;
            float size = 1.5f;

            for (int side = 0; side < 4; side++)
            {
                Vector2 baseVelocity = Vector2.UnitY.RotatedBy(side * MathHelper.PiOver2) * size;
                Vector2 cross = Vector2.Normalize(baseVelocity.RotatedBy(MathHelper.PiOver2));

                baseVelocity -= cross * size;

                for (int d = 0; d < dustsPerSide; d++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.BubbleBurst_White, newColor: color, Scale: 1.6f);
                    dust.noGravity = true;
                    dust.velocity = baseVelocity;

                    baseVelocity += cross * ((size * 2) / dustsPerSide);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            GamerUtils.DrawCentered(this, spriteBatch, lightColor, false);
            return false;
        }
    }
}
