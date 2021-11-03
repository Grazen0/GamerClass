using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs.FriskSet
{
    public class Justice : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Justice");
            Description.SetDefault("5% increased gamer damage");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GamerPlayer>().gamerDamageMult += 0.05f;
        }
    }
}
