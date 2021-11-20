using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Dyes
{
    public class HaloShieldDye : ModItem
    {
        public override string Texture => "Terraria/Item_" + ItemID.BrightYellowDye;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energy Shield Dye");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 20;
            item.maxStack = 99;
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(gold: 1, silver: 50);
        }
    }
}
