using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.GasterBlaster
{
    public class GasterSkull : ModProjectile
    {
        private bool init = true;
        private bool shoot = true;

        private float initialVelocity;
        private float totalRotation;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 57;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
        }

        public float Timer
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float InitialRotation
        {
            get => projectile.ai[1];
        }

        public override void AI()
        {
            float moveDuration = 30f;
            float shootDelay = 10f;

            #region Visuals
            int targetCounter;
            if (projectile.frame == 0)
            {
                targetCounter = (int)moveDuration;
            }
            else if (projectile.frame < Main.projFrames[projectile.type] - 3)
            {
                targetCounter = 3;
            }
            else
            {
                targetCounter = 5;
            }

            if (projectile.frameCounter++ > targetCounter)
            {
                if (projectile.frame == Main.projFrames[projectile.type] - 1)
                {
                    projectile.frame--;
                }
                else
                {
                    projectile.frame++;
                }
                projectile.frameCounter = 0;
            }
            #endregion

            if (init)
            {
                float velocityRotation = projectile.velocity.ToRotation();
                if (velocityRotation < 0f)
                {
                    velocityRotation = MathHelper.TwoPi + velocityRotation;
                }

                totalRotation = velocityRotation - InitialRotation;
                if (totalRotation > MathHelper.Pi)
                {
                    totalRotation = -MathHelper.TwoPi + totalRotation;
                }

                initialVelocity = projectile.velocity.Length();

                init = false;
            }

            if (Timer <= moveDuration)
            {
                float timeRadius = Timer / moveDuration;
                float sine = (float)Math.Sin(timeRadius * MathHelper.PiOver2);

                // Movement
                float newVelocity = initialVelocity * (1f - sine);
                projectile.velocity *= newVelocity / projectile.velocity.Length();

                // Rotation
                projectile.rotation = InitialRotation + (totalRotation * sine);
            }
            else if (Timer > moveDuration + shootDelay)
            {
                if (Main.myPlayer == projectile.owner && shoot)
                {
                    Vector2 direction = projectile.rotation.ToRotationVector2();
                    Projectile.NewProjectile(projectile.Center, direction * 4f, ProjectileID.IchorBullet, projectile.damage, projectile.knockBack, projectile.owner);
                    shoot = false;
                }
            }

            Timer++;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                GetAlpha(lightColor) ?? lightColor,
                projectile.rotation + MathHelper.PiOver2,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
