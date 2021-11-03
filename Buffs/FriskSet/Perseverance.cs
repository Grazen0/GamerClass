using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs.FriskSet
{
    public class Perseverance : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Perseverance");
            Description.SetDefault("5% increased damage reduction");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.05f;
        }
    }
}