using Terraria.ModLoader;

namespace GamerClass.Items.Ammo
{
    public class Magazine : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 18;
            item.ammo = item.type;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
