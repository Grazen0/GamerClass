using GamerClass.Projectiles.CaduceusStaff;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class CaduceusStaff : GamerWeapon
    {
        public override string Texture => "Terraria/Item_" + ItemID.LaserMachinegun;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Merciful Staff");
        }

        public override void SafeSetDefaults()
        {
            item.noMelee = true;
            item.damage = 6;
            item.crit = 0;
            item.knockBack = 0f;
            item.channel = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useTime = item.useAnimation = 10;
            item.shoot = ModContent.ProjectileType<CaduceusRayHeal>();
            item.shootSpeed = 1f;
        }

        public override void GetWeaponCrit(Player player, ref int crit) => crit = 0;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, -1);
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            item.shoot = player.altFunctionUse == 2 ?
                ModContent.ProjectileType<CaduceusRayDamage>() :
                ModContent.ProjectileType<CaduceusRayHeal>();

            return base.CanUseItem(player);
        }

        public override bool AltFunctionUse(Player player) => true;
    }
}
