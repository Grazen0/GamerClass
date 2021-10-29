using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Placeable
{
    public class TouhouMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Maiden's Capriccio)");
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
            item.createTile = ModContent.TileType<Tiles.TouhouMusicBox>();
            item.rare = ItemRarityID.LightRed;
            item.value = Item.sellPrice(gold: 1);
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddIngredient(ItemID.GoldCoin, 1);
            recipe.AddIngredient(ItemID.LightShard, 2);
            recipe.AddIngredient(ItemID.DarkShard, 2);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
