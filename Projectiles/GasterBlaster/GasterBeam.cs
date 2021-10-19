using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using GamerClass.Buffs;

namespace GamerClass.Projectiles.GasterBlaster
{
    public class GasterBeam : ModProjectile
    {
        private readonly int Segments = 100;
        private bool init = true;

        private float xScale = 0f;

        private Vector2 Unit => projectile.rotation.ToRotationVector2() * projectile.height;

        public int OwnerProjectile => (int)projectile.ai[0];

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 48;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = 60;
        }

        public override void AI()
        {
            if (init)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GasterBlaster"), projectile.Center);
                init = false;
            }

            UpdateMovement();
            UpdateVisuals();
            CastLights();
        }

        private void UpdateMovement()
        {
            Projectile owner = Main.projectile[OwnerProjectile];
            
            if (owner.active)
            {
                projectile.position += owner.velocity;
            } else
            {
                projectile.Kill();
            }
        }

        private void UpdateVisuals()
        {
            projectile.rotation = projectile.velocity.ToRotation();

            float fadeOutDuration = 20f;

            if (projectile.timeLeft > fadeOutDuration)
            {
                // Fade in
                int fadeInDuration = 6;
                float scaleInSpeed = 1f / fadeInDuration;
                int fadeInSpeed = (int)(255f * scaleInSpeed);

                xScale = MathHelper.Min(xScale + scaleInSpeed, 1f);
                projectile.alpha = (int)MathHelper.Max(projectile.alpha - fadeInSpeed, 0);
            }
            else
            {
                // Fade out
                float scaleOutSpeed = 1f / fadeOutDuration;
                int fadeOutSpeed = (int)(255f * scaleOutSpeed);

                xScale = MathHelper.Max(xScale - scaleOutSpeed, 0f);
                projectile.alpha = (int)MathHelper.Min(projectile.alpha + fadeOutSpeed, 255);
            }
        }

        private void CastLights()
        {
            DelegateMethods.v3_1 = new Vector3(0.5f, 0.5f, 0.5f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + (Unit * Segments), projectile.width * xScale * 0.8f, DelegateMethods.CastLight);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), // objectPosition
                targetHitbox.Size(), // objectDimensions
                projectile.Center, // lineStart
                projectile.Center + Unit * Segments, // lineEnd
                projectile.width * xScale, // lineWidth
                ref point); // collisionPoint
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int margin = 6;
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type] - margin;

            for (int i = 0; i < Segments; i++)
            {
                int frameIndex;
                if (i == Segments - 1)
                {
                    // Beam end
                    frameIndex = 2;
                }
                else if (i > 0)
                {
                    // Beam body
                    frameIndex = 1;
                }
                else
                {
                    // Beam head
                    frameIndex = 0;
                }

                Vector2 position = projectile.Center + (Unit * i);

                Vector2 origin = new Vector2(texture.Width / 2, 0f);

                Rectangle sourceRectangle = new Rectangle(0, frameIndex * (frameHeight + margin), texture.Width, frameHeight);

                spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    sourceRectangle,
                    Color.White * projectile.Opacity,
                    projectile.rotation - MathHelper.PiOver2,
                    origin,
                    new Vector2(xScale, 1f),
                    SpriteEffects.None,
                    0f);
            }

            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            
        }

        public override bool ShouldUpdatePosition() => false;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
            target.AddBuff(ModContent.BuffType<Karma>(), 360, true);
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + (Unit * Segments), projectile.width * xScale, DelegateMethods.CutTiles);
        }
    }
}
