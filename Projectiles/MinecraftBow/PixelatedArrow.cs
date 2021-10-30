using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.MinecraftBow
{
    public class PixelatedArrow : ModProjectile
    {
        private Vector2 stickOffset = Vector2.Zero;
        private float stickedRotation = 0f;
        private float stickRotation = 0f;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.GamerProjectile().gamer = true;
        }

        public float GravityDelay
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public int StickedTo
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {

            if (StickedTo == -1)
            {
                if (++GravityDelay >= 15f)
                {
                    projectile.velocity.Y += 0.1f;
                }

                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.PiOver4;

                if (projectile.velocity.Y > 16f)
                    projectile.velocity.Y = 16f;

                if (projectile.direction == -1)
                    projectile.rotation -= MathHelper.PiOver2;
            } else
            {
                projectile.tileCollide = false;

                if (projectile.timeLeft > 600)
                    projectile.timeLeft = 600;

                NPC npc = Main.npc[StickedTo];
                if (npc.active)
                {
                    projectile.Center = npc.Center + stickOffset;
                } else
                {
                    projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            float spread = MathHelper.PiOver4;
            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Stone, Scale: 1.2f);
                dust.velocity = Vector2.Normalize(-projectile.velocity).RotatedBy(Main.rand.NextFloat(-spread, spread)) * dust.velocity.Length() * 2f;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            StickedTo = target.whoAmI;
            stickOffset = projectile.Center - target.Center;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10);
            return base.OnTileCollide(oldVelocity);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, projectile.height / 2);
            SpriteEffects spriteEffects = projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            if (projectile.direction == -1)
                origin.Y += texture.Height - projectile.height;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                spriteEffects,
                0f);

            return false;
        }

        public override bool CanDamage() => StickedTo == -1;

        public override bool ShouldUpdatePosition() => StickedTo == -1;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (StickedTo == -1)
                return null;
            else
                return false;
        }
    }
}
