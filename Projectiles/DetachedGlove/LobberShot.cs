using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class LobberShot : ModProjectile
    {
        private const float RotationSpeed = MathHelper.TwoPi / 60f;

        private float spriteScale = 0f;
        private float jumpCooldown = 0f;
        private int jumps = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 18;
            projectile.friendly = true;
            projectile.timeLeft = 600;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            if (jumpCooldown > 0)
                jumpCooldown--;

            projectile.rotation += RotationSpeed * projectile.direction;
            spriteScale = MathHelper.Min(spriteScale + 0.3f, 1f);

            projectile.velocity.Y = MathHelper.Min(projectile.velocity.Y + 0.5f, 25f);

            if (jumps >= 4)
                projectile.Kill();

            if (Main.rand.NextBool(16))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Venom, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (
                Math.Abs(projectile.velocity.X) < Math.Abs(oldVelocity.X)
                || projectile.velocity.Y != oldVelocity.Y && projectile.oldVelocity.Y < 0f
                )
            {
                return true;
            }

            if (projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 0f)
            {
                projectile.velocity.Y = -MathHelper.Max(projectile.oldVelocity.Y * 0.8f, 4f);

                if (jumpCooldown <= 0)
                {
                    jumpCooldown = 5;
                    jumps++;

                    for (int d = 0; d < 4; d++)
                    {
                        Vector2 position = projectile.position;
                        position.Y += projectile.height;

                        Dust dust = Dust.NewDustDirect(position, projectile.width, 0, DustID.Venom);
                        dust.noGravity = false;
                        dust.velocity = Main.rand.NextVector2Unit(MathHelper.Pi + MathHelper.PiOver4, MathHelper.TwoPi - MathHelper.PiOver4) * 2f;
                    }
                }
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            int separation = 2;
            int ringLength = 360 / separation;

            float maxSpeed = Main.rand.NextFloat(2.8f, 3.6f);

            for (int ring = 0; ring < 2; ring++)
            {
                float speed = maxSpeed;
                if (ring == 0)
                    speed *= Main.rand.NextFloat(0.3f, 0.6f);

                for (int d = 0; d < ringLength; d++)
                {

                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Venom);
                    dust.velocity = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(d * separation)) * speed;
                    dust.noGravity = true;
                }
            }

            for (int d = 0; d < 10; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Venom, Scale: 1.5f);
                dust.noGravity = true;
                dust.velocity *= 1.6f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor, flip: false);
            return false;
        }
    }
}
