using Terraria.ModLoader;
using Terraria.ID;

namespace GamerClass.Items.Weapons
{
    public class PlumberHammer : GamerWeapon
    {
        public override int RamUsage => 100;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plumber's Hammer");
        }

        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 48;
            item.noMelee = true;
            item.damage = 20;
            item.useTime = item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noUseGraphic = true;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PlumberHammer");
            item.shoot = ModContent.ProjectileType<Projectiles.FlyingHammer>();
            item.shootSpeed = 15;
            item.autoReuse = true;
        }
    }
}
