using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class PlumberHammer : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plumber's Hammer");
        }

        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 48;
            item.noMelee = true;
            item.damage = 30;
            item.knockBack = 2f;
            item.useTime = item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noUseGraphic = true;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PlumberHammer");
            item.shoot = ModContent.ProjectileType<Projectiles.FlyingHammer>();
            item.shootSpeed = 15;

            ramUsage = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(ItemID.WhiteString);
            recipe.AddIngredient(ItemID.PlatinumBar, 8);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            
            recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(ItemID.WhiteString);
            recipe.AddIngredient(ItemID.GoldBar, 8);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
