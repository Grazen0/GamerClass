using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.TouhouStick
{
    public class HomingCharm : ModProjectile
    {
        private readonly float RangeSQ = (float)Math.Pow(1000, 2);

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 16;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.alpha = 200;
            projectile.GamerProjectile().gamer = true;
        }

        public int CurrentTarget
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 80, 50);

            if (CurrentTarget == -1)
            {
                FindTarget();
            }
            else
            {
                NPC target = Main.npc[CurrentTarget];

                if (target.active)
                {
                    Vector2 direction = Vector2.Normalize(target.Center - projectile.Center);
                    projectile.velocity = Vector2.Lerp(projectile.velocity, direction * 28f, 0.1f);
                }
                else
                {
                    CurrentTarget = -1;
                }
            }

            if (Main.rand.NextBool(30))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.PurificationPowder);
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 0.5f;
                dust.fadeIn = 0.2f;
            }
        }

        private void FindTarget()
        {
            if (CurrentTarget > -1) return;

            float currentDistanceSQ = -1f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy()) continue;

                float distanceSQ = Vector2.DistanceSquared(projectile.Center, npc.Center);

                bool inRange = distanceSQ <= RangeSQ;
                bool closest = currentDistanceSQ == -1f || distanceSQ < currentDistanceSQ;
                bool inSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);


                if (inRange && closest && inSight)
                {
                    CurrentTarget = i;
                    currentDistanceSQ = distanceSQ;
                }
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
            for (int d = 0; d < 5; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PurificationPowder);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor, flip: false);
            return false;
        }
    }
}
