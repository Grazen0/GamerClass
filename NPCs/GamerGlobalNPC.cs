using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.NPCs
{
    public class GamerGlobalNPC : GlobalNPC
    {
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

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (Main.hardMode && type == NPCID.Clothier)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.PeakEvolution>());
                nextSlot++;
            }
        }
    }
}
