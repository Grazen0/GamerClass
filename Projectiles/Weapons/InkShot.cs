using GamerClass.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.Weapons
{
    public class InkShot : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.None;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 8;
            projectile.friendly = true;
            projectile.extraUpdates = 6;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hide = true;
            projectile.alpha = 150;
            projectile.GamerProjectile().gamer = true;
        }

        public float GravityDelay
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (projectile.GamerProjectile().FirstTick)
            {
                for (int d = 0; d < 5; d++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Cloud, Scale: 1.6f, newColor: Color.Blue);
                    dust.velocity = Vector2.Normalize(projectile.velocity).RotatedByRandom(MathHelper.ToRadians(75)) * dust.velocity.Length() * 2f;
                    dust.noGravity = true;
                }
            }

            if (++GravityDelay > 5f)
            {
                projectile.velocity.Y += 0.02f;

                if (projectile.velocity.Y > 5f)
                    projectile.velocity.Y = 5f;
            }

            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 15, 0);

            int dustWidth = (int)(projectile.width * projectile.Opacity);
            int dustHeight = (int)(projectile.height * projectile.Opacity);
            Vector2 offset = -new Vector2(dustWidth / 2, dustHeight / 2);

            for (int d = 0; d < 4; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center + offset, dustWidth, dustHeight, DustID.Cloud, Scale: 1.2f * projectile.Opacity, newColor: Color.Blue);
                dust.velocity *= 0f;
                dust.noGravity = true;
            }

            for (int d = 0; d < 1; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center + offset, dustWidth, dustHeight, DustID.Cloud, Scale: 1.8f * projectile.Opacity, newColor: Color.Blue);
                dust.velocity = Vector2.Normalize(projectile.velocity).RotatedByRandom(MathHelper.PiOver4 / 2) * dust.velocity.Length() * 1.5f;
                dust.noGravity = true;
            }
        }

        private void ModifyDamage()
        {
            projectile.damage = (int)(projectile.damage * 0.75f);

            if (projectile.damage < 1)
                projectile.damage = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Inked>(), 300);
            ModifyDamage();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Inked>(), 300);
            ModifyDamage();
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/InkShotSplat"), projectile.Center);

            for (int d = 0; d < 30; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Cloud, Scale: 1.6f, newColor: Color.Blue);
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
        }
    }
}
