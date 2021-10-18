using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.BerdlyHalberd
{
    public class BerdlyCursor : ModProjectile
    {
        private Vector2 baseDirection = Vector2.Zero;
        private bool init = true;
        private int timer = 0;
        private float rotationOffset = 0f;
        private float cursorTimer = 0f;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 32;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 120;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }

        public int CursorIndex => (int)projectile.ai[0];

        public float SineFactor => projectile.ai[1];

        public override void AI()
        {
            if (init)
            {
                for (int d = 0; d < 6; d++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Scale: 4f);
                    dust.customData = projectile;
                    dust.velocity *= 2.5f;
                    dust.noGravity = true;
                    dust.noLight = false;
                }

                init = false;
            }

            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = projectile.velocity.ToRotation();
            }
            else
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Scale: 4f);
                dust.customData = projectile;
                dust.velocity *= 2.5f;
                dust.noGravity = true;
                dust.noLight = false;
            }

            if (baseDirection == Vector2.Zero)
            {
                baseDirection = projectile.velocity;
            }

            UpdateVelocity();

            if (CursorIndex == 1) SpawnCursors();

            timer++;
        }

        private void UpdateVelocity()
        {
            int moveDelay = CursorIndex * 3;

            if (timer >= moveDelay)
            {
                if (projectile.velocity == Vector2.Zero)
                {
                    projectile.velocity = baseDirection;
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/BerdlyCursor"));
                }

                int actualTimer = timer - moveDelay;
                float previousOffset = rotationOffset;

                rotationOffset = (float)Math.Sin(MathHelper.ToRadians(actualTimer * 5f)) * SineFactor;
                projectile.velocity = projectile.velocity.RotatedBy((-previousOffset + rotationOffset));
            }
            else
            {
                projectile.timeLeft++;
                projectile.velocity = Vector2.Zero;
            }
        }

        private void SpawnCursors()
        {
            if (++cursorTimer > 8)
            {
                cursorTimer = 0;

                if (Main.myPlayer == projectile.owner)
                {
                    for (int side = -1; side <= 1; side += 2)
                    {
                        Projectile cursor = Projectile.NewProjectileDirect(
                            projectile.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<BerdlyCursorSmall>(),
                            projectile.damage,
                            projectile.knockBack,
                            projectile.owner,
                            0f, -1f);

                        cursor.rotation = projectile.rotation + MathHelper.PiOver2 * side;
                        cursor.netUpdate = true;
                    }
                }
            }
        }

        public override bool CanDamage() => projectile.velocity != Vector2.Zero;

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Scale: 4f);
                dust.customData = projectile;
                dust.velocity *= 2.5f;
                dust.noGravity = true;
                dust.noLight = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = texture.Size() / 2;
            Color color = GetAlpha(lightColor) ?? lightColor;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
