using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles
{
    public class Yuyufumo : ModProjectile
    {
        private readonly float Range = (float)Math.Pow(600f, 2);

        public float RotatePosition
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = MathHelper.WrapAngle(value);
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 32;
            projectile.friendly = true;
            projectile.scale = 0.2f;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.netImportant = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (!player.active)
                projectile.active = false;

            if (player.dead)
                player.GamerPlayer().yuyufumo = false;

            if (player.GamerPlayer().yuyufumo)
                projectile.timeLeft = 2;

            // Animation
            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                projectile.frame = ++projectile.frame % Main.projFrames[projectile.type];
            }

            // Movement
            Vector2 targetPosition = player.Center - new Vector2(player.direction * 64f, 48f);
            targetPosition += -Vector2.UnitY.RotatedBy(RotatePosition) * 16f;

            if (Vector2.DistanceSquared(projectile.Center, targetPosition) > Range)
                projectile.Center = targetPosition;
            else
                projectile.Center = Vector2.Lerp(projectile.Center, targetPosition, 0.08f);

            // Other visuals
            projectile.spriteDirection = -player.direction;
            projectile.rotation = projectile.velocity.X * 0.2f;
            projectile.scale = MathHelper.Min(projectile.scale + 0.15f, 1f);

            RotatePosition += MathHelper.TwoPi / 120f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = sourceRectangle.Size() / 2;
            SpriteEffects spriteEffects = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                spriteEffects,
                0f);

            return false;
        }
    }
}
