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
        private bool init = true;
        private int timer = 0;
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
            projectile.GamerProjectile().gamer = true;
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

            int moveDelay = CursorIndex * 3;

            if (timer == moveDelay)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/BerdlyCursor"));

            SpawnDusts(moveDelay);
            Movement(moveDelay);

            if (CursorIndex == 1) SpawnCursors();

            timer++;
        }

        private void SpawnDusts(int moveDelay)
        {
            if (timer < moveDelay)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Scale: 4f);
                dust.customData = projectile;
                dust.velocity *= 2.5f;
                dust.noGravity = true;
                dust.noLight = false;
            }
        }

        private void Movement(int moveDelay)
        {
            if (timer >= moveDelay)
            {
                float sineValue = MathHelper.ToRadians((timer - moveDelay) * 5f) - MathHelper.Pi;

                if (sineValue < 0f) timer++; // Speed up timer

                float sine = (float)Math.Sin(sineValue);

                Vector2 realVelocity = projectile.velocity.RotatedBy(sine * SineFactor);

                projectile.position += realVelocity;
                projectile.rotation = realVelocity.ToRotation();
            }
            else
            {
                projectile.timeLeft++;
            }
        }

        private void SpawnCursors()
        {
            if (++cursorTimer > 6)
            {
                cursorTimer = 0;

                if (Main.myPlayer == projectile.owner)
                {
                    for (int side = -1; side <= 1; side += 2)
                    {
                        Projectile.NewProjectile(
                            projectile.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<BerdlyCursorSmall>(),
                            projectile.damage,
                            projectile.knockBack,
                            projectile.owner,
                            projectile.rotation + MathHelper.PiOver2 * side, 
                            -1f);
                    }
                }
            }
        }

        public override bool CanDamage() => timer >= CursorIndex * 3;

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

        public override bool ShouldUpdatePosition() => false;

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
