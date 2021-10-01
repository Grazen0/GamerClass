using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using GamerClass.Buffs;
using Microsoft.Xna.Framework;

namespace GamerClass.NPCs
{
    public class GamerGlobalNPC : GlobalNPC
    {
        private readonly Color karmaColor = new Color(133, 29, 140);
        public override bool InstancePerEntity => true;

        public bool karma;

        public override void ResetEffects(NPC npc)
        {
            karma = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (karma)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= 48;

                if (damage < 6)
                {
                    damage = 6;
                }
            }
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (karma) return new Color(133, 29, 140);

            return null;
        }
    }
}
