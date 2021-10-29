using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class RoundaboutShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.friendly = true;
            projectile.timeLeft = 600;
            projectile.scale = 1.5f;
            projectile.GamerProjectile().gamer = true;
        }

        public float ForceDirection => projectile.ai[0];

        public float MoveTimer
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            Vector2 direction = ForceDirection.ToRotationVector2();
            projectile.velocity += direction * 0.4f;

            float length = projectile.velocity.Length();
            float maxVelocity = 20f;

            if (length > maxVelocity)
            {
                projectile.velocity *= maxVelocity / length;
            }

            int forceSide = Math.Sign(direction.X);
            if (forceSide == 0) forceSide = 1;

            if (MoveTimer > 0)
            {
                MoveTimer--;

                float distance = MoveTimer * 0.015f;
                projectile.position += distance * direction.RotatedBy(MathHelper.PiOver2 * -forceSide);
            }

            SpawnDusts();
            UpdateVisuals(forceSide);
        }

        private void SpawnDusts()
        {
            if (Main.rand.NextBool(6))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Frost, Scale: 1f);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length();
            }
        }

        private void UpdateVisuals(int direction)
        {
            projectile.rotation = ForceDirection;
            projectile.spriteDirection = direction;

            if (++projectile.frameCounter > 5)
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
            int points = 5;
            int dustsPerPoint = 20;
            float separation = MathHelper.TwoPi / points;
            float explosionSize = Main.rand.NextFloat(2.5f, 3f);

            Vector2 baseDirection = Main.rand.NextFloat(separation).ToRotationVector2();

            for (int point = 0; point < points; point++)
            {
                Vector2 basePointDirection = baseDirection.RotatedBy(point * separation);

                for (int ring = 0; ring < 3; ring++)
                {
                    float speed = explosionSize * (1f - ring * 0.1f);

                    for (int d = 0; d < dustsPerPoint; d++)
                    {
                        Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Ultrabright, Scale: 0.8f);
                        dust.noGravity = true;
                        dust.velocity = basePointDirection.RotatedBy(d * 0.04f) * speed;
                    }
                }
            }

            for (int d = 0; d < 8; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Ultrabright, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width, frameHeight) / 2;
            Color color = projectile.GetAlpha(lightColor);
            SpriteEffects spriteEffects = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                spriteEffects,
                0f);

            return false;
        }
    }
}
