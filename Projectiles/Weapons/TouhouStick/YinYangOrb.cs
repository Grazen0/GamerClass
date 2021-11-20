using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons.TouhouStick
{
    public class YinYangOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.scale = 0.2f;
            projectile.timeLeft = 2;
            projectile.GamerProjectile().gamer = true;
        }

        public float Side => projectile.ai[0];

        public float Distance
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public float SyncTimer
        {
            get => projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public float DeathTimer
        {
            get => projectile.localAI[1];
            set => projectile.localAI[1] = value;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            UpdatePlayer(player);

            projectile.timeLeft = 2;
            projectile.rotation += MathHelper.TwoPi / 90f;

            if (!player.dead
                && player.ownedProjectileCounts[projectile.type] == 2
                && player.HeldItem.type == ModContent.ItemType<Items.Weapons.TouhouStick>())
            {
                if (DeathTimer > 0f)
                    DeathTimer--;
                projectile.scale = MathHelper.Min(projectile.scale + 0.12f, 1f);
            }
            else
            {
                DeathTimer++;
                projectile.scale = MathHelper.Max(projectile.scale - 0.2f, 0f);
            }

            if (DeathTimer > 5f)
                projectile.Kill();
        }

        private void UpdatePlayer(Player player)
        {
            if (projectile.owner == Main.myPlayer)
            {
                float rotation = projectile.velocity.ToRotation();
                float targetRotation = (Main.MouseWorld - player.MountedCenter).ToRotation();

                if (targetRotation > rotation + MathHelper.Pi)
                    targetRotation -= MathHelper.TwoPi;
                else if (targetRotation < rotation - MathHelper.Pi)
                    targetRotation += MathHelper.TwoPi;

                projectile.velocity = MathHelper.Lerp(rotation, targetRotation, 0.1f).ToRotationVector2();

                Distance = MathHelper.Lerp(Distance, 52f, 0.15f);

                projectile.Center = player.Center + projectile.velocity.RotatedBy(MathHelper.PiOver2 * Side) * Distance;
                
                if (Main.myPlayer == projectile.owner && ++SyncTimer > 20f)
                {
                    projectile.netUpdate = true;
                    SyncTimer = 0f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            this.DrawCentered(spriteBatch, lightColor, flip: false);
            return false;
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool CanDamage() => false;
    }
}
