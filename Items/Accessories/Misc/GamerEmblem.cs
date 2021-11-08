using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Accessories.Misc
{
    public class GamerEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("15% increased gamer damage");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 28;
            item.value = Item.sellPrice(gold: 2);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GamerPlayer().gamerDamageMult += 0.15f;
        }

        public override void AddRecipes()
        {
            if (ModLoader.GetMod("Fargowiltas") != null)
            {
                ModRecipe recipe = new ModRecipe(mod);

                recipe.AddIngredient(ItemID.WallOfFleshBossBag);
                recipe.AddTile(TileID.Solidifier);
                recipe.SetResult(this);

                recipe.AddRecipe();
            }
        }
    }
}
