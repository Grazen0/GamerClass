using Terraria;
using Terraria.ModLoader;
using GamerClass.NPCs;

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
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GamerGlobalNPC>().karma = true;
        }
    }
}
