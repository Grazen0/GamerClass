using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class FriskShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Determined Shirt");
            Tooltip.SetDefault("4% increased gamer critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 18;
            item.value = Item.sellPrice(silver: 1);
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GamerPlayer().gamerCrit += 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Silk, 16);
            recipe.AddIngredient(ItemID.FallenStar, 5);

            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
