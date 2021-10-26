using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.MinecraftBow
{
    public class HeldBow : ModProjectile
    {
        private readonly float DistanceOffset = 14f;
        private readonly float MaxCharge = 40f;

        public int ProjectileType => (int)projectile.ai[0];
        public float charge = 0f;
        public bool charging = true;
        public float shakeTimer = 0f;
        public float shakeDirection = -1f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.position = player.Center + projectile.velocity * DistanceOffset;

            UpdatePlayer(player);
            ChargeBow(player);
            UpdateShake();
        }

        private void ChargeBow(Player player)
        {
            if (!player.channel)
            {
                if (charging)
                {
                    ReleaseArrow();
                    charging = false;
                }
            } else
            {
                player.GetModPlayer<GamerPlayer>().ramRegenTimer = -2;

                if (charge < MaxCharge) charge++;
                projectile.timeLeft = 15;
            }

        }

        private void UpdatePlayer(Player player)
        {
            if (charging && Main.myPlayer == projectile.owner)
            {
                Vector2 toCursor = Vector2.Normalize(Main.MouseWorld - player.Center);
                projectile.velocity = toCursor;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;

                projectile.netUpdate = true;
            }

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
        }

        private void UpdateShake()
        {
            if (!charging)
            {
                shakeDirection = 0f;
            } else
            {
                float timerTarget = 25f - (charge / MaxCharge * 20f);
                if (++shakeTimer > timerTarget)
                {
                    shakeTimer -= timerTarget;
                    shakeDirection *= -1f;
                }
            }
        }

        private void ReleaseArrow()
        {
            if (Main.myPlayer == projectile.owner)
            {
                float chargeRadius = charge / MaxCharge;
                int damage = projectile.damage + (int)(Math.Pow(1.115f, charge));
                Vector2 position = Main.player[projectile.owner].Center + projectile.velocity * 8f;
                Vector2 velocity = projectile.velocity * (2f + (chargeRadius * 25f));

                Projectile.NewProjectile(position, velocity, ProjectileType, damage, projectile.knockBack, projectile.owner);

                Main.PlaySound(SoundID.Item5);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameCount = Main.projFrames[projectile.type];

            float chargeRadius = charge / MaxCharge;
            Vector2 position = Main.player[projectile.owner].Center + projectile.velocity * 5f;
            position += projectile.velocity.RotatedBy(MathHelper.PiOver2) * shakeDirection;

            int frameHeight = texture.Height / frameCount;
            int frame = charging ? (int)(chargeRadius * (frameCount - 2)) : frameCount - 1;

            Rectangle sourceRectangle = new Rectangle(0, frame * frameHeight, texture.Width, frameHeight);

            Vector2 origin = sourceRectangle.Size() / 2;
            float rotation = projectile.velocity.ToRotation() + MathHelper.Pi - MathHelper.PiOver4;

            spriteBatch.Draw(
                texture,
                position - Main.screenPosition,
                sourceRectangle,
                Color.White * projectile.Opacity,
                rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        public override bool CanDamage()
        {
            return false;
        }
    }
}
