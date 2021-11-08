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
        public bool inked;

        public override void ResetEffects(NPC npc)
        {
            karma = false;
            inked = false;
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
            if (karma) return new Color(133, 29, 140, npc.alpha);
            if (inked) return Color.Lerp(drawColor, Color.Blue, 0.7f);

            return null;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (inked && Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.BlueTorch, Scale: 1.5f);
                dust.noLight = true;
                dust.velocity *= 0f;
            }
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
