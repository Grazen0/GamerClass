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
        }

        public override void SafeSetDefaults()
        {
            // TODO: balance
            item.width = item.height = 15;
            item.noMelee = true;
            item.damage = 40;
            item.knockBack = 3f;
            item.channel = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<Projectiles.Weapons.MinecraftBow.HeldBow>();
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

            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddIngredient(ItemID.StoneBlock, 6);
            recipe.AddIngredient(ItemID.WhiteString);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
