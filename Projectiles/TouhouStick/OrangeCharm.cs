using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.TouhouStick
{
    public class OrangeCharm : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 16;
            projectile.friendly = true;
            projectile.alpha = 250;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 80, 50);

            if (Main.rand.NextBool(20))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.RedTorch, Scale: 1.5f);
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 0.5f;
                dust.fadeIn = 0.5f;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/TouhouStickHit"), projectile.Center);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/TouhouStickHit"), projectile.Center);
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 8; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.RedTorch, Scale: 1.5f);
                dust.fadeIn = 0.8f;
                dust.noGravity = true;
                dust.velocity *= 5f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = texture.Size() / 2;

            int trails = 8;
            for (int i = trails; i > 0; i--)
            {
                Vector2 offset = projectile.velocity * i * 0.2f;
                Color trailColor = color * (0.4f - i * 0.04f);
                float trailScale = projectile.scale - i * 0.05f;

                spriteBatch.Draw(
                    texture,
                    projectile.Center - offset - Main.screenPosition,
                    null,
                    trailColor,
                    projectile.rotation,
                    origin,
                    trailScale,
                    SpriteEffects.None,
                    0f);
            }

            this.DrawCentered(spriteBatch, lightColor, flip: false);

            return false;
        }
    }
}
