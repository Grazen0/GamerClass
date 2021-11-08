using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class FriskPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Determined Pants");
            Tooltip.SetDefault("5% increased gamer damage");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 18;
            item.value = Item.sellPrice(silver: 1);
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GamerPlayer().gamerDamageMult += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.FallenStar, 4);

            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
