using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Accessories.Motherboards
{
    public class WoodenBoard : RamAccessory
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Motherboard");
            Tooltip.SetDefault("'It barely holds itself together'");
        }

        public override void SafeSetDefaults()
        {
            item.value = Item.sellPrice(silver: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GamerPlayer().maxRam2 += 32;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
            recipe.AddIngredient(ItemID.StoneBlock, 4);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 6);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
