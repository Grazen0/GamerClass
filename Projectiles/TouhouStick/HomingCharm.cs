using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.TouhouStick
{
    public class HomingCharm : ModProjectile
    {
        private readonly float RangeSQ = (float)Math.Pow(1000, 2);
        private const float rotationSpeed = MathHelper.TwoPi / 180f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 32;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 420;
            projectile.scale = 0.8f;
            projectile.alpha = 255;
            projectile.extraUpdates = 1;
            projectile.timeLeft = 360 * projectile.extraUpdates;
            projectile.GamerProjectile().gamer = true;
        }

        public int CurrentTarget
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            projectile.rotation += rotationSpeed;
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 40, 50);

            FindTarget();

            if (CurrentTarget != -1 && Main.npc[CurrentTarget].active)
            {
                NPC target = Main.npc[CurrentTarget];

                float speed = 12f;
                float inertia = 20f;
                Vector2 direction = Vector2.Normalize(target.Center - projectile.Center);
                projectile.velocity = (projectile.velocity * (inertia - 1) + direction * speed) / inertia;
            }

            if (Main.rand.NextBool(16))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.PurificationPowder);
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 0.5f;
                dust.fadeIn *= 0.2f;
            }
        }

        private void FindTarget()
        {
            if (CurrentTarget > -1) return;

            int currentNPC = -1;
            float currentDistanceSQ = -1f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy()) continue;

                float distanceSQ = (npc.Center - projectile.Center).LengthSquared();
                if (distanceSQ > RangeSQ) continue;

                if (currentDistanceSQ != -1 && distanceSQ > currentDistanceSQ) continue;

                bool inSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                if (!inSight) continue;

                currentNPC = i;
                currentDistanceSQ = distanceSQ;

            }

            CurrentTarget = currentNPC;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            for (int d = 0; d < 5; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.PurificationPowder);
                dust.velocity *= 2f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor, flip: false);
            return false;
        }
    }
}
