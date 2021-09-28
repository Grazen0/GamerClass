﻿using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using GamerClass.Buffs;

namespace GamerClass.Items
{
    public abstract class GamerWeapon : ModItem
    {
        public override bool CloneNewInstances => true;

        public abstract int RamUsage
        {
            get;
        }

        private static SoundEffectInstance soundInstance = null;

        public virtual void SafeSetDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();

            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.summon = false;
            item.crit = 4;
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
            bool canUseItem = !player.GetModPlayer<GamerPlayer>().gamerCooldown;

            if (!canUseItem && (soundInstance == null || soundInstance.State != SoundState.Playing))
            {
                soundInstance = Main.PlaySound(SoundID.NPCHit34);
            }

            return canUseItem;
        }

        public override void HoldItem(Player player)
        {
            GamerPlayer modPlayer = player.GetModPlayer<GamerPlayer>();
            float ramRadius = (float)modPlayer.usedRam / modPlayer.maxRam;

            if (!modPlayer.gamerCooldown && ramRadius > 0.6f && Main.rand.NextBool(1))
            {
                float spread = MathHelper.PiOver4 * 0.8f;
                int size = 10;

                Vector2 position = player.Center;
                position.X -= size / 2 - player.direction * 10;
                position.Y -= size / 2 - 5;

                Vector2 direction = -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-spread, spread));

                int id = Dust.NewDust(position, size, size, DustID.Smoke);
                Main.dust[id].noGravity = true;
                Main.dust[id].velocity = direction * Main.rand.NextFloat(3f, 6f);
                Main.dust[id].scale *= 1.5f;
                Main.dust[id].fadeIn = 1.2f;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] words = tt.text.Split(' ');
                tt.text = words.First() + " gamer " + words.Last();
            }

            string ramText;
            if (RamUsage > 0)
            {
                string suffix = "B";
                float roundedRAM = RamUsage;
                if (RamUsage >= 1e12)
                {
                    suffix = "TB";
                    roundedRAM /= 1e12f;
                }
                else if (RamUsage >= 1e9)
                {
                    suffix = "GB";
                    roundedRAM /= 1e9f;
                }
                else if (RamUsage >= 1e6)
                {
                    suffix = "MB";
                    roundedRAM /= 1e6f;
                }
                else if (RamUsage >= 1000)
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
                overrideColor = Color.LightYellow
            };
            tooltips.Add(ramTooltip);
        }

        public override bool UseItem(Player player)
        {
            GamerPlayer modPlayer = player.GetModPlayer<GamerPlayer>();

            modPlayer.ramRegenTimer = -item.useTime;
            modPlayer.ramRegenRate = 1f;
            modPlayer.usedRam += RamUsage;

            if (modPlayer.usedRam > modPlayer.maxRam)
            {
                modPlayer.usedRam = modPlayer.maxRam;
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

                if (Main.myPlayer == player.whoAmI)
                {
                    soundInstance = Main.PlaySound(SoundID.NPCHit53);
                }
            }

            return true;
        }
    }
}