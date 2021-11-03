using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs.FriskSet
{
    public class Kindness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kindness");
            Description.SetDefault("20% increased movement speed");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 1.2f;
        }
    }
}