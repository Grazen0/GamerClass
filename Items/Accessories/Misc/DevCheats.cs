using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Accessories.Motherboards
{
    public class DevCheats : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Developer's Cheats");
            Tooltip.SetDefault("'You probably shouldn't have this'");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 40;
            item.rare = ItemRarityID.Purple;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.GetModPlayer<GamerPlayer>();

            modPlayer.ramUsageMult = 0f;
            modPlayer.maxRam2 += 1000;
        }
    }
}
