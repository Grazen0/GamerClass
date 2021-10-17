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
        public override string Texture => "Terraria/Item_" + ItemID.Uzi;

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
            Tooltip.SetDefault("'Ten kills on the board right now'");
        }

        public override void SafeSetDefaults()
        {
            item.noMelee = true;
            item.damage = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 100;
            item.shoot = ProjectileID.BulletHighVelocity;
            item.knockBack = 1;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Reload
                item.UseSound = SoundID.Item98;
                item.useTime = item.useAnimation = 124;

                return player.GetModPlayer<GamerPlayer>().FindAndRemoveAmmo(ModContent.ItemType<Ammo.Magazine>());
            }
            else
            {
                // Shoot
                item.useTime = item.useAnimation = 10;
                item.UseSound = SoundID.Item11;

                return charge > 0 && base.CanUseItem(player);
            }

        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "ScarCharge", $"Charge: {charge}/{MaxCharge}"));
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
            } else
            {
                // Shoot
                charge--;
                return true;
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
    }
}
