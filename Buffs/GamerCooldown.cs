using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs
{
    public class GamerCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Overheat");
            Description.SetDefault("RAM cooling and gamer weapons disabled");

            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GamerPlayer>().gamerCooldown = true;
        }
    }
}
