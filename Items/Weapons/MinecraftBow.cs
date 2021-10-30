using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class MinecraftBow : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pixelated Bow");
            Tooltip.SetDefault("Uses arrows as ammo\n'Enchants not applicable'");
        }

        public override void SafeSetDefaults()
        {
            item.width = item.height = 15;
            item.noMelee = true;
            item.damage = 40;
            item.knockBack = 3f;
            item.channel = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<Projectiles.MinecraftBow.HeldBow>();
            item.shootSpeed = 14f;
            item.value = Item.sellPrice(copper: 50);
            item.useTime = item.useAnimation = 10;
            item.autoReuse = true;
            item.noUseGraphic = true;

            ramUsage = 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.WhiteString);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(ItemID.StoneBlock, 2);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
