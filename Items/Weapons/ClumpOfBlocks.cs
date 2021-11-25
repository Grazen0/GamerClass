using GamerClass.Projectiles.Weapons.ClumpOfBlocks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace GamerClass.Items.Weapons
{
    public class ClumpOfBlocks : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clump of Blocks");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 25;
            item.knockBack = 4f;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = SoundID.Item1;
            item.useTime = item.useAnimation = 26;
            item.shoot = ModContent.ProjectileType<ZBlock>();
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(silver: 60);

            ramUsage = 3;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            var blockType = new WeightedRandom<int>();

            blockType.Add(type);
            blockType.Add(ModContent.ProjectileType<SBlock>());
            blockType.Add(ModContent.ProjectileType<TBlock>());
            blockType.Add(ModContent.ProjectileType<OBlock>());
            blockType.Add(ModContent.ProjectileType<JBlock>());
            blockType.Add(ModContent.ProjectileType<LBlock>());
            blockType.Add(ModContent.ProjectileType<IBlock>());

            type = blockType;

            position.Y -= 600f;
            position.X = Main.MouseWorld.X;

            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, Main.rand.Next(4));
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.GrayBrick, 4);
            recipe.AddIngredient(ItemID.RedBrick, 4);
            recipe.AddIngredient(ItemID.SandstoneBrick, 4);
            recipe.AddIngredient(ItemID.IceBrick, 4);
            recipe.AddIngredient(ItemID.SnowBrick, 4);
            recipe.AddIngredient(ItemID.MudstoneBlock, 4);
            recipe.AddIngredient(ItemID.MeteoriteBrick, 4);
            recipe.AddIngredient(ItemID.MolotovCocktail, 5);
            recipe.AddRecipeGroup("GamerClass:EvilScale", 10);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
