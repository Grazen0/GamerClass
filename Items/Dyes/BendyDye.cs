using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Dyes
{
    public class BendyDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink-Black Sepia Dye");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 20;
            item.maxStack = 99;
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(gold: 1, silver: 50);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.BlackInk);
            recipe.AddIngredient(ItemID.BlackPaint, 5);
            recipe.AddIngredient(ItemID.Ectoplasm, 2);
            recipe.AddIngredient(ItemID.Bottle);

            recipe.AddTile(TileID.AlchemyTable);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
