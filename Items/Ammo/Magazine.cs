using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Ammo
{
    public class Magazine : ModItem
    {
        public override string Texture => "Terraria/Item_" + ItemID.Coal;

        public override void SetDefaults()
        {
            item.ammo = item.type;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
