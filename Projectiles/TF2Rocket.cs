using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles
{
    public class TF2Rocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rocket");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 30, 0);

            if (projectile.alpha < 200)
                SpawnDust();
        }

        private void SpawnDust()
        {
            Vector2 position = projectile.position - projectile.rotation.ToRotationVector2() * 40f;

            // Trail dust
            Dust dust;
            for (int d = 0; d < 3; d++)
            {
                dust = Dust.NewDustDirect(position, projectile.width, projectile.height, DustID.Smoke, Alpha: 100);
                dust.velocity *= 0.1f;
            }

            // Fire trail
            dust = Dust.NewDustDirect(position, projectile.width, projectile.height, DustID.Fire, Alpha: 100);
            dust.velocity *= 0.1f;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.Center);

            int size = 144;
            Rectangle hitbox = new Rectangle((int)projectile.Center.X - size / 2, (int)projectile.Center.Y - size / 2, size, size);
            GamerUtils.AreaDamage(projectile.damage / 2, projectile.knockBack / 2, projectile.Center, hitbox, npc => npc.immune[projectile.owner] <= 0);

            // Pog rocket jump
            float range = 150f;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                Vector2 toPlayer = player.Center - projectile.Center;
                float distance = toPlayer.Length();

                if (distance <= range)
                {
                    float force = (1f - (distance / range)) * 30f;
                    player.velocity = Vector2.Normalize(toPlayer) * force;
                }
            }

            // Dust and gore stuff
            for (int g = 0; g < 6; g++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                int id = Gore.NewGore(projectile.Center, velocity, GoreID.ChimneySmoke2, 1f);
                Main.gore[id].timeLeft /= 4;
            }

            for (int d = 0; d < 20; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, Scale: 4f);
                dust.noGravity = true;
                dust.velocity *= 12f;
            }

            for (int d = 0; d < 10; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Smoke, Scale: 2f);
                dust.noGravity = true;
                dust.velocity *= 6f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, texture.Height / 2);

            Color color = projectile.GetAlpha(lightColor);

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color * projectile.Opacity,
                projectile.rotation,
                origin,
                projectile.scale,
                projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically,
                0f);

            return false;
        }
    }
}
