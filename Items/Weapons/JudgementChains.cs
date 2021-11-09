using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class JudgementChains : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glove of Judgement");
            Tooltip.SetDefault("Summons a vertical chain of energy");
        }

        public override void SafeSetDefaults()
        {
            item.width = 48;
            item.height = 46;
            item.damage = 40;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3f;
            item.useTime = item.useAnimation = 25;
            item.shoot = ModContent.ProjectileType<Projectiles.JudgementChain>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.X = Main.MouseWorld.X;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Chain, 30);
            recipe.AddIngredient(ItemID.HellstoneBar, 12);
            recipe.AddIngredient(ItemID.Bone, 20);
            recipe.AddIngredient(ItemID.DemonTorch, 5);

            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
