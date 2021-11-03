using GamerClass.Buffs.FriskSet;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items
{
    public abstract class HumanSoul : ModItem
    {
        public override string Texture => "GamerClass/Items/HumanSoul";

        public int soulBuff;

        public sealed override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            item.width = item.height = 32;
        }

        public virtual void SafeSetDefaults() { }

        public override bool OnPickup(Player player)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/SoulGrab").WithVolume(0.3f));
            player.AddBuff(soulBuff, 300);

            int dusts = 20;
            float separation = MathHelper.TwoPi / dusts;
            Color color = GetAlpha(Color.White) ?? Color.White;
            Vector2 direction = Vector2.UnitY;

            for (int d = 0; d < dusts; d++)
            {
                Dust dust = Dust.NewDustPerfect(item.Center, DustID.Cloud, newColor: color, Scale: 1.5f);
                dust.velocity = direction * 4f;
                dust.noGravity = true;

                direction = direction.RotatedBy(separation);
            }

            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustPerfect(item.Center, DustID.Cloud, newColor: color, Scale: 1.2f);
                dust.velocity *= 2f;
                dust.noGravity = true;

                direction = direction.RotatedBy(separation);
            }

            return false;
        }

        public override void PostUpdate()
        {
            Color color = GetAlpha(Color.White) ?? Color.White;
            Lighting.AddLight(item.Center, color.ToVector3() * 0.8f * Main.essScale);

            if (Main.rand.NextBool(8))
            {
                Dust dust = Dust.NewDustDirect(item.position, item.width, item.height, DustID.PortalBolt, newColor: color);
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
            }
        }

        public override void GrabRange(Player player, ref int grabRange) => grabRange = (int)(grabRange * 1.5f);
    }

    public class RedSoul : HumanSoul
    {
        public override void SafeSetDefaults() => soulBuff = ModContent.BuffType<Determination>();

        public override Color? GetAlpha(Color lightColor) => Color.Red;
    }

    public class BlueSoul : HumanSoul
    {
        public override void SafeSetDefaults() => soulBuff = ModContent.BuffType<Integrity>();

        public override Color? GetAlpha(Color lightColor) => new Color(0, 60, 255);
    }

    public class GreenSoul : HumanSoul
    {
        public override void SafeSetDefaults() => soulBuff = ModContent.BuffType<Kindness>();

        public override Color? GetAlpha(Color lightColor) => new Color(0, 192, 0);
    }

    public class PurpleSoul : HumanSoul
    {
        public override void SafeSetDefaults() => soulBuff = ModContent.BuffType<Perseverance>();

        public override Color? GetAlpha(Color lightColor) => new Color(213, 53, 217);
    }

    public class YellowSoul : HumanSoul
    {
        public override void SafeSetDefaults() => soulBuff = ModContent.BuffType<Justice>();

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 0);
    }

    public class OrangeSoul : HumanSoul
    {
        public override void SafeSetDefaults() => soulBuff = ModContent.BuffType<Bravery>();

        public override Color? GetAlpha(Color lightColor) => new Color(252, 166, 0);
    }

    public class LightBlueSoul : HumanSoul
    {
        public override void SafeSetDefaults() => soulBuff = ModContent.BuffType<Patience>();

        public override Color? GetAlpha(Color lightColor) => new Color(66, 252, 255);
    }
}
