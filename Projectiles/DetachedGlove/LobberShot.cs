using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class LobberShot : ModProjectile
    {
        private readonly float RotationSpeed = MathHelper.TwoPi / 60f;

        private float spriteScale = 0f;
        private float jumpCooldown = 0f;
        private int jumps = 0;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 18;
            projectile.friendly = true;
            projectile.timeLeft = 600;
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

                    for (int d = 0; d < 8; d++)
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
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = texture.Size() / 2;
            Color color = GetAlpha(lightColor) ?? lightColor;

            // Afterimages
            if (projectile.alpha < 100)
            {
                int trails = 4;
                for (int i = 1; i <= trails; i++)
                {
                    int reverseIndex = trails - i + 1;
                    Vector2 position = projectile.Center - projectile.velocity * reverseIndex * 0.3f;

                    spriteBatch.Draw(
                        texture,
                        position - Main.screenPosition,
                        null,
                        color * (projectile.Opacity * i * 0.08f),
                        projectile.rotation - RotationSpeed * projectile.direction * reverseIndex,
                        origin,
                        projectile.scale * spriteScale * 0.9f,
                        SpriteEffects.None,
                        0f);
                }
            }

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color * projectile.Opacity,
                projectile.rotation,
                origin,
                projectile.scale * spriteScale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
