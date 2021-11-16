using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items
{
    public class PhantomFood : ModItem
    {
        public override string Texture => "Terraria/Item_" + ItemID.BowlofSoup;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a legendary creature of fluffy origins");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 32;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 20;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PhantomFood").WithVolume(0.2f);
            item.shoot = ModContent.ProjectileType<Projectiles.Yuyufumo>();
            item.buffType = ModContent.BuffType<Buffs.Yuyufumo>();
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(gold: 5);
        }

        public override void UseStyle(Player player)
        {
            if (Main.myPlayer == player.whoAmI && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.BowlofSoup);
            recipe.AddIngredient(ItemID.CookedFish);
            recipe.AddIngredient(ItemID.CookedShrimp);
            recipe.AddIngredient(ItemID.PumpkinPie);
            recipe.AddIngredient(ItemID.Sashimi);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);

            recipe.AddTile(TileID.CookingPots);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
