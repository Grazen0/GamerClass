using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Accessories.Misc
{
    [AutoloadEquip(EquipType.Head)]
    public class SwearHeadphones : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Offensive Headphones");
            Tooltip.SetDefault("Just don't get hit...");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.buyPrice(gold: 15);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GamerPlayer().swearing = true;
        }
    }
}
