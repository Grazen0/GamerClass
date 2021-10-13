using System;
using Terraria;
using Terraria.ModLoader;
using GamerClass.Items.Weapons;

namespace GamerClass.Prefixes
{
    public class GamerPrefix : ModPrefix
    {
        public float damageMult = 1;
        public int critBonus = 0;
        public float useTimeMult = 1f;
        public float ramMult = 1f;

        public override PrefixCategory Category => PrefixCategory.Custom;

        public GamerPrefix()
        {

        }

        public GamerPrefix(float damageMult = 1f, int critBonus = 0, float useTimeMult = 1f, float ramMult = 1f)
        {
            this.damageMult = damageMult;
            this.critBonus = critBonus;
            this.useTimeMult = useTimeMult;
            this.ramMult = ramMult;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("Polished", new GamerPrefix(damageMult = 1.1f, critBonus = 5));
                mod.AddPrefix("Dusty", new GamerPrefix(useTimeMult = 0.85f, critBonus = -5));
            }

            return false;
        }

        public override void Apply(Item item)
        {
            // TODO: Apply ram usage mult
        }

        public override void ModifyValue(ref float valueMult)
        {
            float reforgeValue = damageMult + (critBonus / 100f) + useTimeMult;
            valueMult *= 1f + reforgeValue * 0.4f;
        }

        public override bool CanRoll(Item item) => item.modItem is GamerWeapon;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = this.damageMult;
            critBonus = this.critBonus;
            useTimeMult = this.useTimeMult;
        }

        public override void ValidateItem(Item item, ref bool invalid)
        {
            if (
                (damageMult != 1f && item.damage == Math.Round(item.damage * damageMult)) ||
                (useTimeMult != 1f && item.useTime == Math.Round(item.useTime * useTimeMult))
                )
            {
                invalid = true;
            }
        }
    }
}
