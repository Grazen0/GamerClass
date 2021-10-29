using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public abstract class PackWhack : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weaponized Backpack");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 12;
            item.value = Item.sellPrice(silver: 20);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Silk, 15);
        }
    }
}
