using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class ChaserShot : ModProjectile
    {
        private readonly float RangeSQ = (float)Math.Pow(1000, 2);

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.friendly = true;
            projectile.timeLeft = 600;
            projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
            projectile.alpha = 60;
            projectile.GamerProjectile().gamer = true;
        }

        public int CurrentTarget
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (CurrentTarget == -1) FindTarget();

            if (CurrentTarget > -1)
            {
                NPC target = Main.npc[CurrentTarget];


                if (target.active)
                {
                    float speed = 12f;
                    float inertia = 12f;
                    Vector2 direction = Vector2.Normalize(target.Center - projectile.Center);
                    projectile.velocity = (projectile.velocity * (inertia - 1) + direction * speed) / inertia;
                } else
                {
                    CurrentTarget = -1;
                }
            }

            RunAnimation();
            SpawnDusts();

            Lighting.AddLight(projectile.Center, Color.LightGreen.ToVector3() * 0.8f);
        }

        private void RunAnimation()
        {
            if (++projectile.frameCounter > 2)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            projectile.rotation = projectile.velocity.ToRotation();
        }
        
        private void SpawnDusts()
        {
            for (int d = 0; d < 3; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.GreenFairy);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(projectile.velocity).RotatedByRandom(MathHelper.PiOver4 * 0.8f) * dust.velocity.Length();
            }
        }

        public override void Kill(int timeLeft)
        {
            int lines = 4;
            for (int l = 0; l < lines; l++)
            {

                Vector2 direction = Main.rand.NextVector2CircularEdge(1f, 1f);
                float maxSpeed = Main.rand.NextFloat(4f, 5f);
                float dusts = Main.rand.Next(2, 8);
                float scale = Main.rand.NextFloat(1f, 1.6f);

                for (int d = 0; d < dusts; d++)
                {
                    float speed = maxSpeed - (d * 0.4f);

                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.GreenFairy);
                    dust.noGravity = true;
                    dust.scale = scale;
                    dust.velocity = direction * speed;
                }
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

                float distanceSQ
                    = Vector2.DistanceSquared(npc.Center, projectile.Center);

                bool inRange = distanceSQ <= RangeSQ;
                bool closest = currentDistanceSQ == -1f || distanceSQ < currentDistanceSQ;
                bool inSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);

                if (inRange && closest && inSight)
                {
                    currentNPC = i;
                    currentDistanceSQ = distanceSQ;
                }
            }

            CurrentTarget = currentNPC;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor, flip: false);
            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
