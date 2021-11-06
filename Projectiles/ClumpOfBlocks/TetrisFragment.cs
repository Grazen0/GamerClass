using System;
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
                    color = Color.Red;
                    break;
                case 1:
                    color = Color.LimeGreen;
                    break;
                case 2:
                    color = Color.Magenta;
                    break;
                case 3:
                    color = Color.Yellow;
                    break;
                case 4:
                    color = Color.Blue;
                    break;
                case 5:
                    color = Color.Orange;
                    break;
                case 6:
                    color = Color.Cyan;
                    break;
            }

            Vector2 baseDirection = -Vector2.Normalize(projectile.velocity);

            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, newColor: color, Scale: 1.6f);
                dust.noGravity = true;
                dust.velocity = baseDirection.RotatedByRandom(MathHelper.PiOver2) * dust.velocity.Length() * 2f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            GamerUtils.DrawCentered(this, spriteBatch, lightColor, false);
            return false;
        }
    }
}
