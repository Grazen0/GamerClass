using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons.BerdlyHalberd
{
    public class HalberdSpear : ModProjectile
    {
        private bool shootCursors = true;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.scale = 1.5f;
            projectile.ownerHitCheck = true;
            projectile.tileCollide = false;
            projectile.hide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
            projectile.GamerProjectile().gamer = true;
        }

        public float MovementFactor
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float SineFactor => projectile.ai[1];

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.direction = projectile.spriteDirection = player.direction;
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation;

            UpdateMovement(player);
            SpawnDusts();

            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.position.X = ownerMountedCenter.X - projectile.width / 2f;
            projectile.position.Y = ownerMountedCenter.Y - projectile.height / 2f;

            projectile.position += projectile.velocity * MovementFactor;

            if (player.itemAnimation == 0)
            {
                projectile.Kill();
            }

            UpdateVisuals();
        }

        private void UpdateMovement(Player player)
        {
            if (player.frozen) return;

            if (MovementFactor == 0f)
            {
                MovementFactor = 3f;
                projectile.netUpdate = true;
            }

            if (player.itemAnimation < player.itemAnimationMax / 2)
            {
                if (shootCursors)
                {
                    if (Main.myPlayer == projectile.owner)
                    {
                        for (int i = 5; i >= 0; i--)
                        {
                            Projectile.NewProjectile(
                                projectile.Center,
                                projectile.velocity * 1.2f,
                                ModContent.ProjectileType<BerdlyCursor>(),
                                projectile.damage,
                                projectile.knockBack,
                                projectile.owner,
                                i, SineFactor * -projectile.direction);
                        }
                    }

                    shootCursors = false;
                }

                MovementFactor -= 0.3f;
            }
            else
            {
                MovementFactor += 0.3f;
            }
        }

        private void SpawnDusts()
        {
            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.CursedTorch, Scale: 1.5f);
                dust.alpha = 0;
                dust.velocity = Vector2.Normalize(projectile.velocity) * dust.velocity.Length() * 5f;
                dust.noGravity = false;
                dust.noLight = false;
            }
        }

        private void UpdateVisuals()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;

            if (projectile.spriteDirection == -1)
            {
                projectile.rotation += MathHelper.PiOver2;
            }
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) =>
            drawCacheProjsBehindProjectiles.Add(index);

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = new Vector2(5f, 4f);

            if (projectile.spriteDirection == -1)
            {
                origin.Y = texture.Height - origin.Y;
            }

            Color color = projectile.GetAlpha(lightColor);

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically,
                0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
