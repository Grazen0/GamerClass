using GamerClass.Buffs;
using GamerClass.Prefixes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace GamerClass.Items.Weapons
{
    public abstract class GamerWeapon : ModItem
    {
        public override bool CloneNewInstances => true;
        public int ramUsage = 0;
        public float ramUsageMult = 1f;

        public int TotalRamUsage => (int)Math.Round(ramUsage * ramUsageMult);

        private static SoundEffectInstance soundInstance = null;

        public override ModItem Clone(Item item)
        {
            var clone = (GamerWeapon)base.Clone(item);
            clone.ramUsageMult = ramUsageMult;
            return clone;
        }

        public virtual void SafeSetDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            item.crit = 4;

            SafeSetDefaults();

            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.summon = false;
            item.thrown = false;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= player.GetModPlayer<GamerPlayer>().gamerDamageMult;
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            knockback += player.GetModPlayer<GamerPlayer>().gamerKnockback;
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit += player.GetModPlayer<GamerPlayer>().gamerCrit;
        }

        public override bool CanUseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<GamerPlayer>();
            bool canUseItem = !modPlayer.gamerCooldown;

            bool ramConsumed = modPlayer.ConsumeRam(TotalRamUsage, item.useTime);

            if (!ramConsumed && (soundInstance == null || soundInstance.State != SoundState.Playing))
            {
                soundInstance = Main.PlaySound(SoundID.NPCHit34);

                for (int d = 0; d < 8; d++)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Fire, Scale: 2.5f);
                    dust.velocity.X *= 4f;
                }
            }


            return canUseItem;
        }

        public override void HoldItem(Player player)
        {
            GamerPlayer modPlayer = player.GetModPlayer<GamerPlayer>();
            float ramRadius = (float)modPlayer.usedRam / modPlayer.maxRam;

            if (!modPlayer.gamerCooldown && ramRadius > 0.6f)
            {
                int chance = (int)(10 * (1f - ramRadius)) + 2;
                if (chance < 1)
                    chance = 1;

                if (Main.rand.NextBool(chance))
                {
                    float spread = MathHelper.PiOver4 * 0.8f;
                    int size = 10;

                    Vector2 position = player.Center;
                    position.X -= size / 2 - player.direction * 10;
                    position.Y -= size / 2 - 5;

                    Vector2 direction = -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-spread, spread));

                    Dust dust = Dust.NewDustDirect(position, size, size, DustID.Smoke);
                    dust.noGravity = true;
                    dust.velocity = direction * Main.rand.NextFloat(3f, 6f);
                    dust.scale *= 1.5f;
                    dust.fadeIn = 1.2f;
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Damage tooltip 
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] words = tt.text.Split(' ');
                tt.text = words.First() + " gamer " + words.Last();
            }

            // Modifier tooltip
            if (item.prefix > 0 && ramUsageMult != 1f)
            {
                string operand = ramUsageMult > 1f ? "+" : "-";
                TooltipLine modifierTooltip = new TooltipLine(mod, "PrefixRAMUsage", operand + Math.Round((ramUsageMult - 1f) * 100f) + "% RAM usage")
                {
                    isModifier = true,
                    isModifierBad = ramUsageMult > 1f
                };
                tooltips.Add(modifierTooltip);
            }

            // RAM usage tooltip
            string ramText;
            if (TotalRamUsage > 0)
            {
                string suffix = "B";
                float roundedRAM = TotalRamUsage;
                if (TotalRamUsage >= 1e12)
                {
                    suffix = "TB";
                    roundedRAM /= 1e12f;
                }
                else if (TotalRamUsage >= 1e9)
                {
                    suffix = "GB";
                    roundedRAM /= 1e9f;
                }
                else if (TotalRamUsage >= 1e6)
                {
                    suffix = "MB";
                    roundedRAM /= 1e6f;
                }
                else if (TotalRamUsage >= 1000)
                {
                    suffix = "KB";
                    roundedRAM /= 1000f;
                }

                ramText = $"Uses {roundedRAM}{suffix} of RAM";
            }
            else
            {
                ramText = "Does not require RAM";
            }

            TooltipLine ramTooltip = new TooltipLine(mod, "RAM Usage", ramText)
            {
                overrideColor = Color.Yellow
            };
            tooltips.Add(ramTooltip);
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            var val = new WeightedRandom<int>();

            GamerPrefix.GamerPrefixes.ForEach(t => val.Add(t));

            val.Add(PrefixID.Quick);
            val.Add(PrefixID.Deadly);
            val.Add(PrefixID.Agile);
            val.Add(PrefixID.Nimble);
            val.Add(PrefixID.Murderous);
            val.Add(PrefixID.Slow);
            val.Add(PrefixID.Sluggish);
            val.Add(PrefixID.Lazy);
            val.Add(PrefixID.Annoying);
            val.Add(PrefixID.Nasty);

            return val;
        }

        public override bool NewPreReforge()
        {
            ramUsageMult = 1f;
            return true;
        }
    }
}
