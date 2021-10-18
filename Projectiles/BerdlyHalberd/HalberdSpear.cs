using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.BerdlyHalberd
{
    public class HalberdSpear : ModProjectile
    {
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

            drawOffsetX = 20;
            drawOriginOffsetY = 20;
        }

        public float MovementFactor
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);

            projectile.direction = projectile.spriteDirection = player.direction;
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation;


            projectile.position.X = ownerMountedCenter.X - projectile.width / 2f;
            projectile.position.Y = ownerMountedCenter.Y - projectile.height / 2f;


            if (!player.frozen)
            {
                if (MovementFactor == 0f)
                {
                    MovementFactor = 3f;
                    projectile.netUpdate = true;
                }

                if (player.itemAnimation < player.itemAnimationMax / 2)
                    MovementFactor -= 0.4f;
                else
                    MovementFactor += 0.4f;
            }

            projectile.position += projectile.velocity * MovementFactor;

            if (player.itemAnimation == 0)
            {
                projectile.Kill();
            }

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;

            if (projectile.spriteDirection == -1)
            {
                projectile.rotation += MathHelper.PiOver2;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = new Vector2(5f, 4f);

            if (projectile.spriteDirection == -1)
            {
                origin.Y = texture.Height - origin.Y;
            }

            Color color = GetAlpha(Color.White) ?? Color.White;

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
    }
}
