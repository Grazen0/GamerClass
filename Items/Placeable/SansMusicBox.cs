using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Placeable
{
    public class SansMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Megalovania)");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 24;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.SansMusicBox>();
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(gold: 1);
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Bone, 40);
            recipe.AddIngredient(ItemID.Cobweb, 20);
            recipe.AddIngredient(ItemID.IceTorch, 5);

            recipe.AddTile(TileID.BoneWelder);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
