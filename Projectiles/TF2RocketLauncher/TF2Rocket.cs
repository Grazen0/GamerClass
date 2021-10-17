using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.TF2RocketLauncher
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
            for (int d = 0; d < 3; d++)
            {
                int dustId = Dust.NewDust(position, projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100);
                Main.dust[dustId].velocity *= 0.1f;
            }

            // Fire trail
            int fireId = Dust.NewDust(position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100);
            Main.dust[fireId].velocity *= 0.1f;
        }

        public override void Kill(int timeLeft)
        {
            // Create explosion
            Projectile.NewProjectile(
                projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<RocketExplosion>(),
                projectile.damage,
                projectile.knockBack,
                projectile.owner);

            // Pog rocket jump
            float range = 150f;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                Vector2 toPlayer = player.Center - projectile.Center;
                float distance = toPlayer.Length();

                if (distance <= range)
                {
                    float force = (1f - (distance / range)) * 40f;
                    player.velocity = Vector2.Normalize(toPlayer) * force;
                }
            }

            Main.PlaySound(SoundID.Item14, projectile.Center);

            // Dust and gore stuff
            for (int g = 0; g < 6; g++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                int id = Gore.NewGore(projectile.Center, velocity, GoreID.ChimneySmoke2, 1f);
                Main.gore[id].timeLeft /= 4;
            }

            for (int d = 0; d < 20; d++)
            {
                int id = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 0, default, 4f);
                Main.dust[id].noGravity = true;
                Main.dust[id].velocity *= 12f;
            }

            for (int d = 0; d < 10; d++)
            {
                int id = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 0, default, 2f);
                Main.dust[id].noGravity = true;
                Main.dust[id].velocity *= 6f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, texture.Height / 2);

            Color color = GetAlpha(lightColor) ?? lightColor;

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
