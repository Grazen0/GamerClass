using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Placeable
{
    public class ZeldaMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Gerudo Valley)");
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
            item.createTile = ModContent.TileType<Tiles.ZeldaMusicBox>();
            item.rare = ItemRarityID.Pink;
            item.value = Item.sellPrice(gold: 2);
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.HerosHat);
            recipe.AddIngredient(ItemID.Wood, 30);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.SoulofFright);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
