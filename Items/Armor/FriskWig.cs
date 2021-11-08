using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FriskWig : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Determined Wig");
            Tooltip.SetDefault("4% increased gamer critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 18;
            item.value = Item.sellPrice(silver: 1);
            item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) =>
            body.type == ModContent.ItemType<FriskShirt>() && legs.type == ModContent.ItemType<FriskPants>();

        public override void UpdateEquip(Player player)
        {
            player.GamerPlayer().gamerCrit += 4;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus =
                "Gamer critical strikes will spawn a soul,\n" +
                "pick souls up to get short-lasting buffs";

            player.GamerPlayer().friskSet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Cobweb, 60);
            recipe.AddIngredient(ItemID.BrownDye);
            recipe.AddIngredient(ItemID.FallenStar, 3);

            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
