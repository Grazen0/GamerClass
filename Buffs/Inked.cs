using GamerClass.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Buffs
{
    public class Inked : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff_" + BuffID.Slimed;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Inked");
            Description.SetDefault("You are dripping ink");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GamerPlayer().inked = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GamerGlobalNPC>().inked = true;
        }
    }
}
