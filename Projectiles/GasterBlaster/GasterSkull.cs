using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using GamerClass.Buffs;

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
            DisplayName.SetDefault("Gaster Blaster");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 36;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 180;
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
            float moveDuration = 20f;
            float shootDelay = 8f;

            #region Visuals
            int targetCounter;
            if (projectile.frame == 0)
            {
                targetCounter = (int)moveDuration - 9;
            }
            else if (projectile.frame < Main.projFrames[projectile.type] - 3)
            {
                targetCounter = 3;
            }
            else
            {
                targetCounter = 4;
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
                    Vector2 position = projectile.Center + direction * 38f;

                    Projectile.NewProjectile(
                        position, 
                        Vector2.Zero, 
                        ModContent.ProjectileType<GasterBeam>(), 
                        projectile.damage / 4, 
                        projectile.knockBack, 
                        projectile.owner,
                        projectile.rotation,
                        projectile.whoAmI);

                    shoot = false;
                }

                Vector2 frontDirection = projectile.rotation.ToRotationVector2();
                projectile.velocity -= frontDirection * 0.6f;
            }

            Timer++;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Karma>(), 360, true);
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
                Color.White * projectile.Opacity,
                projectile.rotation - MathHelper.PiOver2,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
