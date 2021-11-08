using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class LinkArmorPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Hero's Pants");
            Tooltip.SetDefault("Increases gamer damage by 6%");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(silver: 90);
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

    public override void UpdateEquip(Player player)
    {
        player.GamerPlayer().gamerDamageMult += 0.06f;
    }
}
}
