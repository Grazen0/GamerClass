using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.BerdlyHalberd
{
    public class BerdlyCursorSmall : ModProjectile
    {
        private readonly float RangeSQ = (float)Math.Pow(1000, 2);

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 6;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 240;
            projectile.scale = 1.2f;
            projectile.hide = true;
        }

        public float MoveTimer
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public int CurrentTarget
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            if (projectile.velocity == Vector2.Zero)
            {
                if (++MoveTimer > 20)
                {
                    MoveTimer = 0;
                    projectile.velocity = projectile.rotation.ToRotationVector2() * 10f;
                }
            }
            else if (++MoveTimer > 15)
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
                    } else
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
            float spread = MathHelper.PiOver2;

            for (int d = 0; d < 10; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Scale: 1.6f);
                dust.velocity = baseDirection.RotatedBy(Main.rand.NextFloat(-spread, spread)) * dust.velocity.Length() * 3f;
                dust.noGravity = true;
                dust.noLight = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = texture.Size() / 2;
            Color color = GetAlpha(lightColor) ?? lightColor;

            // Afterimages
            int trails = 4;
            for (int i = 1; i <= trails; i++)
            {
                int reverseIndex = trails - i + 1;
                Vector2 position = projectile.Center - projectile.velocity * reverseIndex * 0.4f;

                spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    color * (projectile.Opacity * i * 0.06f),
                    projectile.rotation,
                    origin,
                    projectile.scale,
                    SpriteEffects.None,
                    0f);
            }

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
