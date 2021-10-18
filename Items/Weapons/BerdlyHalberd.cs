using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class BerdlyHalberd : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Berdly's Halberd");
            Tooltip.SetDefault("'It smells like feathers'");
        }

        public override void SafeSetDefaults()
        {
            item.width = 42;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.height = 34;
            item.noMelee = true;
            item.damage = 110;
            item.useAnimation = 40;
            item.useTime = 24;
            item.shoot = ModContent.ProjectileType<Projectiles.BerdlyHalberd.HalberdSpear>();
            item.shootSpeed = 15f;
            item.knockBack = 6f;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] < 1;
    }
}
