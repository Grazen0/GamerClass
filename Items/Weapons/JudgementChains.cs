using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class JudgementChains : GamerWeapon
    {
        public override string Texture => "Terraria/Item_" + ItemID.ChainGuillotines;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chains of Judgement");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 40;
            item.noMelee = true;
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

            recipe.AddIngredient(ItemID.Chain, 60);
            recipe.AddIngredient(ItemID.HellstoneBar, 16);
            recipe.AddIngredient(ItemID.DemonTorch, 10);

            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
