using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class RaccoonLeaf : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Leaf");
            Tooltip.SetDefault("Transforms the holder into a flying raccoon\nIncreases gamer damage by 20% and gravity by 80%");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 200;
            player.gravity *= 1.8f;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 6f;
            ascentWhenRising = 6f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 1.3f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 6f;
            acceleration *= 8f;
        }
    }
}
