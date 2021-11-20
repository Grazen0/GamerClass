using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons
{
    public class FlyingBackpack : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 16;
            projectile.friendly = true;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.velocity.X *= 0.95f;
            projectile.velocity.Y += 0.6f;

            float maxSpeed = 25f;
            if (projectile.velocity.Y > maxSpeed)
                projectile.velocity.Y = maxSpeed;

            projectile.rotation = projectile.velocity.ToRotation();

            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 17, Scale: 1.2f);
                if (dust.velocity.Y > 0f)
                    dust.velocity.Y *= -1f;

                dust.velocity *= 1.4f;

                dust.noGravity = false;
                dust.noLight = true;
            }

            int ringLength = 30;
            float separation = MathHelper.TwoPi / ringLength;

            for (int r = 0; r < 2; r++)
            {
                Vector2 velocity = Vector2.UnitY * (1.5f + r * 0.6f);
                for (int d = 0; d < ringLength; d++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, 200, Scale: 1.8f);
                    dust.velocity = velocity;
                    dust.velocity.X *= 2.5f;
                    dust.noGravity = true;
                    dust.noLight = true;

                    velocity = velocity.RotatedBy(separation);
                }
            }

            int range = 64;
            Rectangle hitbox = new Rectangle((int)projectile.Center.X - range, (int)projectile.Center.Y - 16, range * 2, 32);
            GamerUtils.AreaDamage(projectile.damage / 2, projectile.knockBack / 2, projectile.Center, hitbox, npc => npc.immune[projectile.owner] <= 0);

            hitbox.X += 8;
            hitbox.Width -= 16;
            Utils.PlotTileLine(new Vector2(hitbox.X, hitbox.Center.Y), new Vector2(hitbox.Right, hitbox.Center.Y), hitbox.Height, DelegateMethods.CutTiles);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor);
            return false;
        }
    }
}
