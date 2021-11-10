using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class ChargeShotBig : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 28;
            projectile.friendly = true;
            projectile.timeLeft = 480;
            projectile.scale = 1.5f;
            projectile.alpha = 255;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 60, 0);

            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AmberBolt, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 2;
            }

            Lighting.AddLight(projectile.Center, Color.Orange.ToVector3());
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.Center);

            Rectangle hitbox = projectile.Hitbox;
            hitbox.X -= 32;
            hitbox.Width += 64;
            hitbox.Y -= 32;
            hitbox.Height += 64;

            GamerUtils.AreaDamage(projectile.damage / 2, projectile.knockBack / 2, projectile.Center, hitbox, npc => npc.immune[projectile.owner] <= 0);

            float baseRotation = Main.rand.NextFloat(MathHelper.Pi);
            for (int e = 0; e < 2; e++)
            {
                List<Dust> dusts = GamerUtils.DustExplosion(8, projectile.Center, DustID.AmberBolt, 2.8f + e * 1.2f, baseRotation, dustPerPoint: 8, dustScale: 1.5f);
                dusts.ForEach(d => d.fadeIn = 1.1f);
            }
            
            for (int g = 0; g < 5; g++)
            {
                Gore gore = Gore.NewGoreDirect(projectile.Center, Main.rand.NextVector2Unit(MathHelper.Pi, MathHelper.TwoPi), Main.rand.Next(61, 64));
            }

            for (int d = 0; d < 16; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.AmberBolt);
                dust.noGravity = true;
                dust.velocity *= 2.5f;
                dust.fadeIn = 1.2f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, frameHeight / 2);
            Color color = projectile.GetAlpha(lightColor);

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                color * projectile.Opacity,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
