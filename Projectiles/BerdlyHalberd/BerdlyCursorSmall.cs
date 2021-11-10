using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.BerdlyHalberd
{
    public class BerdlyCursorSmall : ModProjectile
    {
        private const int MaxLife = 240;
        private readonly float RangeSQ = (float)Math.Pow(1000, 2);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 6;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = MaxLife;
            projectile.scale = 1.2f;
            projectile.hide = true;
            projectile.GamerProjectile().gamer = true;
        }

        public float InitialRotation => projectile.ai[0];

        public int CurrentTarget
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            if (projectile.velocity == Vector2.Zero)
            {
                if (projectile.timeLeft < MaxLife - 20)
                {
                    projectile.velocity = projectile.rotation.ToRotationVector2() * 10f;
                }

                projectile.rotation = InitialRotation;
            }
            else if (projectile.timeLeft < MaxLife - 35)
            {
                projectile.rotation = projectile.velocity.ToRotation();

                if (CurrentTarget == -1)
                {
                    CurrentTarget = TargetNPC();
                }

                if (CurrentTarget != -1)
                {
                    NPC target = Main.npc[CurrentTarget];

                    if (target.active)
                    {
                        float speed = 20f;
                        float inertia = 15f;

                        Vector2 direction = Vector2.Normalize(target.Center - projectile.Center);
                        projectile.velocity = (projectile.velocity * (inertia - 1) + direction * speed) / inertia;
                    }
                    else
                    {
                        CurrentTarget = -1;
                    }
                }
            }
        }

        private int TargetNPC()
        {
            int currentNPC = -1;
            float currentDistanceSQ = -1f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.CanBeChasedBy()) continue;

                if (!Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    continue;

                float distanceSQ = (npc.Center - projectile.Center).LengthSquared();

                if ((currentDistanceSQ == -1f || distanceSQ < currentDistanceSQ) && distanceSQ < RangeSQ)
                {
                    currentDistanceSQ = distanceSQ;
                    currentNPC = i;
                }
            }

            return currentNPC;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) =>
            drawCacheProjsBehindProjectiles.Add(index);

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 10, 0.5f);

            Vector2 baseDirection = -Vector2.Normalize(projectile.velocity);

            for (int d = 0; d < 10; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Scale: 1.6f);
                dust.velocity = baseDirection.RotatedByRandom(MathHelper.PiOver2) * dust.velocity.Length() * 3f;
                dust.noGravity = true;
                dust.noLight = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor, flip: false);
            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
