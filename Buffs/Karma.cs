using GamerClass.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs
{
    public class Karma : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff";
            return true;
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Karmic Retribution");
            Description.SetDefault("You feel your sins crawling on your back");
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GamerGlobalNPC>().karma = true;
        }
    }
}
