using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class GameBoyVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Pixelated Visor");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 6;
            item.value = Item.sellPrice(silver: 5);
            item.rare = ItemRarityID.Green;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawHair = true;

        public override void UpdateVanity(Player player, EquipType type) =>
            player.GamerPlayer().gameBoyVisor = true;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Lens);
            recipe.AddIngredient(ItemID.GreenStainedGlass);
            recipe.AddIngredient(ItemID.Wire, 20);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
