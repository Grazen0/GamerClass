using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace GamerClass.Items.Weapons
{
    public class Scar : GamerWeapon
    {
        private readonly int MaxCharge = 30;
        private int charge = 0;

        public override ModItem Clone(Item item)
        {
            Scar clone = (Scar)base.Clone(item);
            clone.charge = charge;
            return clone;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scar");
            Tooltip.SetDefault("Requires magazines, right click to reload\n'Ten kills on the board right now'");
        }

        public override void SafeSetDefaults()
        {
            item.noMelee = true;
            item.damage = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 7;
            item.shoot = ModContent.ProjectileType<Projectiles.ScarBullet>();
            item.knockBack = 1;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Reload
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ScarReload");
                item.useTime = item.useAnimation = 124;

                return player.GetModPlayer<GamerPlayer>().FindAndRemoveAmmo(ModContent.ItemType<Ammo.Magazine>());
            }
            else
            {
                // Shoot
                item.useTime = item.useAnimation = 10;
                item.UseSound = charge > 0 ? SoundID.Item11 : SoundID.Item98;

                return base.CanUseItem(player);
            }

        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var tt = new TooltipLine(mod, "ScarCharge", $"Charge: {charge}/{MaxCharge}")
            {
                overrideColor = charge > 0 ? Color.LightGreen : Color.Red
            };

            tooltips.Add(tt);
            base.ModifyTooltips(tooltips);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                // Reload
                charge = MaxCharge;
                player.itemRotation = MathHelper.PiOver4 * 0.7f * player.direction;
                return false;
            }
            else
            {
                // Shoot
                Vector2 frontDirection = Vector2.Normalize(new Vector2(speedX, speedY));
                Vector2 muzzleOffset = frontDirection * 62f;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }

                if (charge > 0)
                {
                    charge--;
                    return true;
                } else
                {
                    float spread = MathHelper.PiOver4;
                    for (int d = 0; d < 3; d++)
                    {
                        Dust dust = Dust.NewDustPerfect(position, DustID.Smoke, Scale: 0.8f);
                        dust.velocity = frontDirection.RotatedBy(Main.rand.NextFloat(-spread, spread)) * dust.velocity.Length();
                    }

                    return false;
                }
            }
        }

        public override bool AltFunctionUse(Player player) => true;

        public override TagCompound Save() => new TagCompound()
        {
            ["charge"] = charge
        };

        public override void Load(TagCompound tag) => charge = tag.GetInt("charge");

        public override void LoadLegacy(BinaryReader reader) => charge = reader.ReadInt16();

        public override void NetSend(BinaryWriter writer) => writer.WriteVarInt(charge);

        public override void NetRecieve(BinaryReader reader) => charge = reader.ReadVarInt();

        public override Vector2? HoldoutOffset() => new Vector2(-20f, 2f);
    }
}
