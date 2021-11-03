using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs.FriskSet
{
    public class Integrity : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Integrity");
            Description.SetDefault("10% increased jump speed");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.jumpSpeedBoost += 0.1f;
        }
    }
}