using GamerClass.Buffs;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass
{
    public class GamerPlayer : ModPlayer
    {
        public bool linkArmorBonus;

        public bool gamerCooldown;

        public int maxRam;
        public int usedRam;
        public int ramRegenTimer;
        public float ramRegenRate;

        public float gamerDamageMult;
        public float gamerKnockback;
        public float gamerUseTimeMult;
        public int gamerCrit;

        public override void ResetEffects() => ResetVariables();

        public override void UpdateDead() => ResetVariables();

        private void ResetVariables()
        {
            linkArmorBonus = false;
            gamerCooldown = false;
            maxRam = 5;
            gamerDamageMult = 1f;
            gamerKnockback = 0f;
            gamerUseTimeMult = 1f;
            gamerCrit = 0;
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (gamerCooldown)
            {
                float spread = MathHelper.PiOver2 * 0.8f;

                Vector2 position = player.position;
                position.Y -= player.height / 4;

                Vector2 direction = -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-spread, spread));

                Dust dust = Dust.NewDustDirect(position, player.width, player.height / 2, DustID.Smoke, Scale: 1.6f);
                dust.velocity = direction * 2f;

                if (Main.rand.NextBool(8))
                {
                    dust = Dust.NewDustDirect(position, player.width, (int)(player.height * 0.75f), DustID.Fire, Scale: 1.5f);
                    dust.velocity *= 2f;
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (!gamerCooldown)
            {
                if (usedRam > 0 && ++ramRegenTimer > 20f * ramRegenRate)
                {
                    usedRam = (int)MathHelper.Max(usedRam - 1, 0);
                    ramRegenTimer = 0;
                }
                else
                {
                    ramRegenRate *= 0.997f;
                }
            }
        }

        public bool FindAndRemoveAmmo(int type)
        {
            bool CheckSlot(int slot)
            {
                Item item = player.inventory[slot];
                if (item.type == type)
                {
                    if (item.stack > 1)
                    {
                        item.stack--;
                    }
                    else
                    {
                        item.TurnToAir();
                    }

                    return true;
                }

                return false;
            }

            // Search in ammo slots
            for (int i = 54; i < 58; i++)
            {
                if (CheckSlot(i)) return true;
            }

            // Search in main inventory
            for (int i = 0; i < 50; i++)
            {
                if (CheckSlot(i)) return true;
            }

            return false;
        }

        public bool ConsumeRam(int amount, int regenDelay)
        {
            if (gamerCooldown) return false;

            ramRegenTimer = -regenDelay;
            ramRegenRate = 1f;
            usedRam += amount;

            if (usedRam >= maxRam)
            {
                // RAM overheat
                usedRam = maxRam;
                player.AddBuff(ModContent.BuffType<GamerCooldown>(), 300);

                for (int d = 0; d < 20; d++)
                {
                    bool fire = Main.rand.NextBool(3);

                    int size = 20;
                    Vector2 position = player.Center - new Vector2(1f, 1f) * (size / 2);

                    int id = Dust.NewDust(position, size, size, fire ? DustID.Fire : DustID.Smoke);
                    Main.dust[id].noGravity = true;
                    Main.dust[id].fadeIn = 2f;
                    Main.dust[id].velocity *= fire ? 8f : 4f;
                    Main.dust[id].scale = 1f;
                }

                for (int g = 0; g < 6; g++)
                {
                    Gore gore = Gore.NewGoreDirect(player.position, Vector2.Zero, 99, Scale: 1.25f);
                    gore.velocity *= 0.75f;
                }

                Main.PlaySound(SoundID.NPCHit53);

                return false;
            }

            return true;
        }
    }
}
