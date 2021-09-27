using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Items.Accessories.Motherboards
{
    public abstract class RamAccessory : ModItem
    {
        public virtual void SafeSetDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();

            item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            var (index, _) = FindDifferentEquipedRamAccessory(player);

            if (slot < 10)
            {
                if (index != -1)
                {
                    return slot == index;
                }
            }

            return base.CanEquipAccessory(player, slot);
        }

        protected (int index, Item accessory) FindDifferentEquipedRamAccessory(Player player)
        {
            int maxAccessoryIndex = 5 + player.extraAccessorySlots;
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                Item otherAccessory = player.armor[i];

                if (!otherAccessory.IsAir && !item.IsTheSameAs(otherAccessory) && otherAccessory.modItem is RamAccessory)
                {
                    return (i, otherAccessory);
                }
            }

            return (-1, null);
        }

        public override bool CanRightClick()
        {
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            for (int i = 13; i < 13 + maxAccessoryIndex; i++)
            {
                if (Main.LocalPlayer.armor[i].type == item.type) return false;
            }

            if (FindDifferentEquipedRamAccessory(Main.LocalPlayer).accessory != null)
            {
                return true;
            }

            return base.CanRightClick();
        }

        public override void RightClick(Player player)
        {
            var (index, accessory) = FindDifferentEquipedRamAccessory(player);
            if (accessory != null)
            {
                Main.LocalPlayer.QuickSpawnClonedItem(accessory);
                Main.LocalPlayer.armor[index] = item.Clone();
            }
        }
    }
}
