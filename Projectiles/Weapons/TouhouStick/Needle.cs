using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons.TouhouStick
{
    public class Needle : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 6;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
            projectile.alpha = 255;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 60, 50);

            if (Main.rand.NextBool(30))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.OrangeTorch);
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
            for (int d = 0; d < 2; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.OrangeTorch);
                dust.velocity *= 3f;
                dust.fadeIn = 1f;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = new Vector2(texture.Width - 10f, texture.Height / 2);

            int trails = 6;
            for (int i = trails; i > 0; i--)
            {
                Vector2 offset = projectile.velocity * i * 0.3f;
                Color trailColor = color * (0.4f - i * 0.04f);
                float trailScale = projectile.scale - i * 0.03f;

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

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
