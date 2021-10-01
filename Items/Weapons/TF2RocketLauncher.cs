using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using GamerClass.Projectiles.TF2RocketLauncher;

namespace GamerClass.Items.Weapons
{
    public class TF2RocketLauncher : GamerWeapon
    {
        public override int RamUsage => 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soldier's Rocket Launcher");
            Tooltip.SetDefault("Uses rockets as ammo\n'Great against red spies in the base'");
        }

        public override void SafeSetDefaults()
        {
            item.width = item.height = 52;
            item.noMelee = true;
            item.damage = 150;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item11;
            item.useTime = item.useAnimation = 30;
            item.knockBack = 4;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<TF2Rocket>();
            item.shootSpeed = 20f;
            item.useAmmo = AmmoID.Rocket;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            Vector2 frontDirection = Vector2.Normalize(velocity);

            position += frontDirection.RotatedBy(-MathHelper.PiOver2) * 10f * player.direction;

            Vector2 muzzleOffset = frontDirection * 20f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0) || true)
            {
                position += muzzleOffset;
            }

            for (int d = 0; d < 6; d++)
            {
                float spread = MathHelper.PiOver4 * 0.7f;
                Vector2 direction = frontDirection.RotatedBy(Main.rand.NextFloat(-spread, spread));

                int id = Dust.NewDust(position + frontDirection * 28f, 1, 1, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                Main.dust[id].velocity = direction * Main.rand.NextFloat(4f, 8f);
                Main.dust[id].noGravity = true;
            }

            float rocketJump = player.altFunctionUse == 2 ? 1f : 0f;
            Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI, rocketJump);

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-60f, -2f);
        }
    }
}
