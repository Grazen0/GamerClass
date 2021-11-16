using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class MasterChiefSuit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Military Space Suit");
            Tooltip.SetDefault("10% increased gamer damage and critical strike chance");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 6);
            item.rare = ItemRarityID.Yellow;
            item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.GamerPlayer();
            modPlayer.gamerDamageMult += 0.1f;
            modPlayer.gamerCrit += 10;
        }
    }
}