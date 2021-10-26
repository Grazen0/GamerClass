using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using GamerClass.Projectiles.MinecraftBow;
using Microsoft.Xna.Framework;

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
            item.damage = 2;
            item.channel = true;
            item.useAmmo = AmmoID.Arrow;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = 10;
            item.shootSpeed = 14f;
            item.value = Item.sellPrice(copper: 50);
            item.useTime = item.useAnimation = 10;
            item.autoReuse = true;
            item.noUseGraphic = true;

            ramUsage = 3;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<HeldBow>(), item.damage, knockBack, player.whoAmI, type);

            return false;
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
