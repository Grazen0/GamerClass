using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class Glasses3D : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3D Glasses");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 6;
            item.value = Item.sellPrice(silver: 50);
            item.rare = ItemRarityID.LightRed;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            player.GetModPlayer<GamerPlayer>().glasses3D = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Goggles);
            recipe.AddIngredient(ItemID.BlackLens);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddIngredient(ItemID.Sapphire);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
