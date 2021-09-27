using Terraria.ID;

namespace GamerClass.Items.Weapons
{
    public class ExampleGamerWeapon : GamerWeapon
    {
        public override string Texture => "Terraria/Item_" + ItemID.Excalibur;
        public override int RamUsage => 10;

        public override void SafeSetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 20;
            item.width = item.height = 48;
            item.damage = 50;
            item.knockBack = 4.5f;
            item.autoReuse = true;
        }
    }
}
