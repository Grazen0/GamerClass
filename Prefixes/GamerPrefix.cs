using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using GamerClass.Items.Weapons;

namespace GamerClass.Prefixes
{
    public class GamerPrefix : ModPrefix
    {
        public static List<byte> GamerPrefixes;

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
                GamerPrefixes = new List<byte>();

                // Bad prefixes
                AddGamerPrefix(mod, "Glitched", new GamerPrefix(damageMult: 0.8f, knockbackMult: 0.9f, critBonus: -10, ramUsageMult: 1.2f));
                AddGamerPrefix(mod, "Laggy", new GamerPrefix(useTimeMult: 0.8f, ramUsageMult: 0.95f));
                AddGamerPrefix(mod, "Dusty", new GamerPrefix(damageMult: 0.95f, useTimeMult: 0.85f, ramUsageMult: 1.1f));

                // Eh prefixes
                AddGamerPrefix(mod, "Heated", new GamerPrefix(damageMult: 1.1f, ramUsageMult: 1.2f));
                AddGamerPrefix(mod, "Frantic", new GamerPrefix(useTimeMult: 1.15f, ramUsageMult: 1.15f));
                AddGamerPrefix(mod, "Berserk", new GamerPrefix(useTimeMult: 1.2f, critBonus: 5, ramUsageMult: 1.25f));

                // Good prefixes
                AddGamerPrefix(mod, "Polished", new GamerPrefix(damageMult: 1.05f, useTimeMult: 1.05f, critBonus: 5));
                AddGamerPrefix(mod, "Blazing", new GamerPrefix(useTimeMult: 1.15f, critBonus: 5));
                AddGamerPrefix(mod, "Cool", new GamerPrefix(useTimeMult: 1.1f, ramUsageMult: 0.8f));
                AddGamerPrefix(mod, "Goated", new GamerPrefix(damageMult: 1.15f, knockbackMult: 1.2f));
                AddGamerPrefix(mod, "Broken", new GamerPrefix(damageMult: 1.1f, useTimeMult: 1.15f, knockbackMult: 1.15f, ramUsageMult: 0.8f)); // We do a little trolling
                AddGamerPrefix(mod, "OP", new GamerPrefix(damageMult: 1.15f, useTimeMult: 1.1f, critBonus: 5, knockbackMult: 1.15f, ramUsageMult: 0.85f));
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

        private static void AddGamerPrefix(Mod mod, string name, GamerPrefix prefix)
        {
            mod.AddPrefix(name, prefix);
            GamerPrefixes.Add(mod.PrefixType(name));
        }
    }
}
