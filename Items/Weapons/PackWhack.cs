using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class PackWhack : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weaponized Backpack");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 26;
            item.knockBack = 2f;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.useTime = item.useAnimation = 35;
            item.shoot = ModContent.ProjectileType<Projectiles.FlyingBackpack>();
            item.shootSpeed = 15f;
            item.value = Item.sellPrice(silver: 5);
            item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Silk, 25);
            recipe.AddIngredient(ItemID.WhiteString, 4);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
