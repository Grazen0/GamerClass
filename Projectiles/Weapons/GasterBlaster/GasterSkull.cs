using GamerClass.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons.GasterBlaster
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
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 180;
            projectile.GamerProjectile().gamer = true;
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
            float moveDuration = 15f;
            float shootDelay = 3f;

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
                float acceleration = 0.6f;

                if (Main.myPlayer == projectile.owner && shoot)
                {
                    Vector2 offset = projectile.rotation.ToRotationVector2() * 38f;

                    Projectile.NewProjectile(
                        projectile.Center + offset,
                        offset,
                        ModContent.ProjectileType<GasterBeam>(),
                        projectile.damage / 4,
                        projectile.knockBack,
                        projectile.owner,
                        projectile.whoAmI);

                    int screenShake = ModContent.GetInstance<GamerConfig>().GasterBlasterScreenShake;
                    if (screenShake > 0)
                        Main.LocalPlayer.GamerPlayer().screenShake = screenShake;

                    shoot = false;
                }

                Vector2 frontDirection = projectile.rotation.ToRotationVector2();
                projectile.velocity -= frontDirection * acceleration;
            }

            Timer++;

            UpdateVisuals(moveDuration);
        }

        private void UpdateVisuals(float moveDuration)
        {
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
            Color color = projectile.GetAlpha(lightColor);

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                color,
                projectile.rotation - MathHelper.PiOver2,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
