using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons.CaduceusStaff
{
    public abstract class CaduceusRay : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.None;

        public abstract int PlayerBuff { get; }

        public int CurrentTarget
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float SyncTimer
        {
            get => projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.penetrate = -1;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.position = player.Center;
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.timeLeft = 2;

            if (ShouldKill(player))
                projectile.Kill();

            FindTarget(player);
            UpdatePlayer(player);

            if (CurrentTarget != -1)
            {
                NPC npc = Main.npc[CurrentTarget];
                npc.StrikeNPC(2, 0f, 0);
            }


            if (Main.myPlayer == projectile.owner && ++SyncTimer > 20f)
            {
                projectile.netUpdate = true;
                SyncTimer = 0f;
            }
        }

        private void FindTarget(Player player)
        {
            if (CurrentTarget != -1)
            {
                // Check that current target is in range
                NPC npc = Main.npc[CurrentTarget];
                if (!npc.active || Colliding(projectile.getRect(), npc.getRect()) != true)
                    CurrentTarget = -1;
            }

            if (CurrentTarget == -1)
            {
                // Find new target
                float currentDistanceSQ = -1f;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (!npc.active || !npc.CanBeChasedBy()) continue;

                    if (Colliding(projectile.getRect(), npc.getRect()) != true) continue;

                    float distanceSQ = Vector2.DistanceSquared(player.Center, npc.Center);

                    if (currentDistanceSQ == -1f || distanceSQ < currentDistanceSQ)
                    {
                        currentDistanceSQ = distanceSQ;
                        CurrentTarget = i;
                    }
                }
            }
        }

        private void UpdatePlayer(Player player)
        {
            if (Main.myPlayer == projectile.owner)
            {
                float rotation = projectile.velocity.ToRotation();
                float targetRotation = (Main.MouseWorld - player.MountedCenter).ToRotation();

                if (targetRotation > rotation + MathHelper.Pi)
                    targetRotation -= MathHelper.TwoPi;
                else if (targetRotation < rotation - MathHelper.Pi)
                    targetRotation += MathHelper.TwoPi;

                projectile.velocity = MathHelper.Lerp(rotation, targetRotation, 0.1f).ToRotationVector2();

                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
            }

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Method used to detect targets in range
            Player player = Main.player[projectile.owner];
            float point = 0f;

            Vector2 start = player.Center;
            Vector2 end = start + projectile.velocity * 392f;
            float radius = 160f;

            if (!Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, radius * 2f, ref point)
                && !GamerUtils.CheckAABBvCircleCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, radius)
                && !GamerUtils.CheckAABBvCircleCollision(targetHitbox.TopLeft(), targetHitbox.Size(), end, radius))
                return false;

            Vector2[] points = GetPointsTowards(projHitbox.Center.ToVector2(), 10f);

            for (int i = 0; i < points.Length - 1; i++)
            {
                if (!Collision.CanHitLine(points[i], 0, 0, points[i + 1], 0, 0))
                    return false;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (CurrentTarget == -1) return false;

            Vector2[] points = GetPointsTowards(Main.npc[CurrentTarget].Center, 2f);

            for (int i = 0; i < points.Length; i++)
                points[i] -= Main.screenPosition;

            Color color = projectile.GetAlpha(lightColor);

            if (points.Length > 1)
                DrawLaser(spriteBatch, points, color);

            Texture2D circle = mod.GetTexture("Textures/Circle");

            for (int i = 0; i < 8; i++)
            {
                float scale = 0.2f + i * 0.05f * (1f + (float)Math.Abs(Math.Sin(Main.GlobalTime * 4f)));
                spriteBatch.Draw(circle, points[0], null, color * 0.05f, 0f, circle.Size() / 2, scale, SpriteEffects.None, 0f);
            }

            if (points.Length > 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    float scale = 0.4f + i * 0.01f * (1f + (float)Math.Abs(Math.Sin(Main.GlobalTime * 5f)));
                    spriteBatch.Draw(circle, points[points.Length - 1], null, color * 0.1f, 0f, circle.Size() / 2, scale, SpriteEffects.None, 0f);
                }
            }

            return false;
        }

        protected abstract void DrawLaser(SpriteBatch spriteBatch, Vector2[] points, Color color);

        protected abstract bool ShouldKill(Player player);

        private Vector2[] GetPointsTowards(Vector2 target, float speed)
        {
            int size = 24;

            Vector2 position = Main.player[projectile.owner].Center + projectile.velocity * 60f;

            Vector2 velocity = projectile.velocity * speed * 3f;

            Rectangle hitbox = new Rectangle((int)target.X - size / 2, (int)target.Y - size / 2, size, size);

            var points = new List<Vector2>();

            do
            {
                points.Add(position);

                velocity = Vector2.Lerp(velocity, (target - position).SafeNormalize(Vector2.Zero) * speed, speed / 50f);
                position += velocity;
            } while (position.X < hitbox.X || position.X > hitbox.Right || position.Y < hitbox.Y || position.Y > hitbox.Bottom);

            return points.ToArray();
        }

        public override bool CanDamage() => false;

        public override bool ShouldUpdatePosition() => false;
    }
}
