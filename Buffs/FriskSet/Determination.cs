using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs.FriskSet
{
    public class Determination : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Determination");
            Description.SetDefault("+5 defense");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 5;
        }
    }
}
