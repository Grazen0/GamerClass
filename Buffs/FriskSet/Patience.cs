using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs.FriskSet
{
    public class Patience : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Patience");
            Description.SetDefault("5% decreased RAM usage");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GamerPlayer>().ramUsageMult -= 0.05f;
        }
    }
}
