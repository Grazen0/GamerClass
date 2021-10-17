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
            maxRam = 0;
            gamerDamageMult = 1f;
            gamerKnockback = 0f;
            gamerUseTimeMult = 1f;
            gamerCrit = 0;
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (gamerCooldown)
            {
                float spread = MathHelper.PiOver4;
                int size = 20;

                Vector2 position = player.Center;
                position.X -= size / 2;
                position.Y -= size / 2 + 32;

                Vector2 direction = -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-spread, spread));
                bool fire = Main.rand.NextBool(6);

                int id = Dust.NewDust(position, size, size, fire ? DustID.Fire : DustID.Smoke);
                Main.dust[id].noGravity = true;
                Main.dust[id].scale *= fire ? 3f : 2f;
                Main.dust[id].fadeIn = 1.4f;

                Main.dust[id].velocity = direction * 3f;
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
    }
}
