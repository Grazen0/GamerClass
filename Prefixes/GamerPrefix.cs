using System;
using Terraria;
using Terraria.ModLoader;
using GamerClass.Items.Weapons;

namespace GamerClass.Prefixes
{
    public class GamerPrefix : ModPrefix
    {
        public float damageMult = 1f;
        public float knockbackMult = 1f;
        public float useTimeMult = 1f;
        public int critBonus = 0;
        public float ramUsageMult;

        public override PrefixCategory Category => PrefixCategory.Custom;

        public GamerPrefix()
        {

        }

        public GamerPrefix(float damageMult = 1f, float knockbackMult = 1f, float useTimeMult = 1f, int critBonus = 0, float ramUsageMult = 1f)
        {
            this.damageMult = damageMult;
            this.knockbackMult = knockbackMult;
            this.useTimeMult = useTimeMult;
            this.critBonus = critBonus;
            this.ramUsageMult = ramUsageMult;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("Dusty", new GamerPrefix(useTimeMult: 0.85f, critBonus: -5));
                mod.AddPrefix("Polished", new GamerPrefix(damageMult: 1.1f, useTimeMult: 5f));
                mod.AddPrefix("Heated", new GamerPrefix(ramUsageMult: 1.15f));
            }

            return false;
        }

        public override void Apply(Item item)
        {
            if (item.modItem is GamerWeapon gamerWeapon)
            {
                gamerWeapon.ramUsageMult = ramUsageMult;
            }
        }

        public override void ModifyValue(ref float valueMult)
        {
            float reforgeValue = (damageMult - 1f) + (critBonus / 100f) + (useTimeMult - 1f);
            valueMult *= 1f + reforgeValue * 0.2f;
        }

        public override bool CanRoll(Item item) => item.modItem is GamerWeapon;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = this.damageMult;
            knockbackMult = this.knockbackMult;
            useTimeMult = this.useTimeMult;
            critBonus = this.critBonus;
        }

        public override void ValidateItem(Item item, ref bool invalid)
        {
            GamerWeapon gamerWeapon = item.modItem as GamerWeapon;
            if (
                (damageMult != 1f && item.damage == Math.Round(item.damage * damageMult)) ||
                (useTimeMult != 1f && item.useTime == Math.Round(item.useTime * useTimeMult)) || 
                (ramUsageMult != 1f && gamerWeapon.RamUsage == Math.Round(gamerWeapon.RamUsage * ramUsageMult))
                )
            {
                invalid = true;
            }
        }
    }
}
