using GamerClass.Buffs;
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
                const int damageOverTime = 6;

                if (npc.life - damageOverTime <= 1)
                {
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<Karma>());
                    if (buffIndex != -1)
                        npc.DelBuff(buffIndex);

                    npc.life = 1;
                }
                else
                {
                    if (npc.lifeRegen > 0)
                        npc.lifeRegen = 0;

                    npc.lifeRegen -= damageOverTime * 8;

                    if (damage < damageOverTime)
                        damage = damageOverTime;
                }

            }
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (karma) return new Color(133, 29, 140);
            if (inked) return Color.Lerp(drawColor, Color.Blue, 0.6f);

            return null;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (inked && Main.rand.NextBool(8))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.BubbleBlock, newColor: Color.Blue);
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
