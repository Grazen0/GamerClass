using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class LinkArmorHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Hero's Hat");
            Tooltip.SetDefault("Increases gamer damage by 6%");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = Item.sellPrice(silver: 90);
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GamerPlayer().gamerDamageMult += 0.06f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<LinkArmorShirt>() && legs.type == ModContent.ItemType<LinkArmorPants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus =
                "15% increased gamer damage, 10% increased gamer critical strike chance\n" +
                "Broken pots drop more loot";

            GamerPlayer modPlayer = player.GamerPlayer();
            modPlayer.linkArmorBonus = true;
            modPlayer.gamerDamageMult += 0.15f;
            modPlayer.gamerCrit += 10;
        }
    }
}
