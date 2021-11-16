using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class MasterChiefLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Military Space Leggings");
            Tooltip.SetDefault("8% increased gamer critical strike chance\n10% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 4, silver: 50);
            item.rare = ItemRarityID.Yellow;
            item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GamerPlayer().gamerCrit += 8;
            player.moveSpeed += 0.1f;
        }
    }
}