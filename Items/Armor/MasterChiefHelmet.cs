using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class MasterChiefHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Military Space Helmet");
            Tooltip.SetDefault("15% increased gamer damage\n5% increased gamer critical strike chance");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 7, silver: 50);
            item.rare = ItemRarityID.Yellow;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.GamerPlayer();

            modPlayer.gamerCrit += 5;
            modPlayer.gamerDamageMult += 0.15f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<MasterChiefSuit>() && legs.type == ModContent.ItemType<MasterChiefLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            List<string> hotkeys = GamerClass.HaloShieldHotKey.GetAssignedKeys();
            string hotkey = hotkeys.Count == 0 ? "[NONE]" : hotkeys[0];

            player.setBonus = 
                $"Press {hotkey} to activate an energy shield that" +
                "\nincreases damage reduction and reduces movement speed" +
                "\nThis effect has a 1-minute cooldown";
            player.GamerPlayer().masterChiefSet = true;
        }
    }
}
