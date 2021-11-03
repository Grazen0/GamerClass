using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs.FriskSet
{
    public class Bravery : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bravery");
            Description.SetDefault("5% increased gamer critical strike chance");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GamerPlayer>().gamerCrit += 5;
        }
    }
}
