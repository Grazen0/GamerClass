using GamerClass.Projectiles.DetachedGlove;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace GamerClass.Items.Weapons
{
    public class DetachedGlove : GamerWeapon
    {
        public override string Texture => "Terraria/Item_" + ItemID.TheUndertaker;

        public int shot = 0;

        public override ModItem Clone(Item item)
        {
            var clone = (DetachedGlove)base.Clone(item);
            clone.shot = shot;
            return clone;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Detached Glove");
            Tooltip.SetDefault("'A cup's favorite weapon'");
        }

        public override void SafeSetDefaults()
        {
            item.noMelee = true;
            item.damage = 50;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<PeaShot>();
            item.shootSpeed = 20f;
            item.useTime = item.useAnimation = 15;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Switch shot
                item.useTime = item.useAnimation = 60;

                return true;
            } else
            {
                item.useTime = item.useAnimation = 20;

                return base.CanUseItem(player);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var (shotName, color) = shot switch
            {
                0 => ("Peashooter", Color.LightBlue),
                1 => ("Spread", Color.IndianRed),
                _ => ("[INVALID]", Color.Red)
            };

            TooltipLine tt = new TooltipLine(mod, "DetachedGloveShotType", "Current shot: " + shotName)
            {
                overrideColor = color
            };

            tooltips.Add(tt);
            base.ModifyTooltips(tooltips);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                // Switch shot type
                shot = (shot + 1) % 2;
                return false;
            } else
            {
                switch (shot)
                {
                    case 0: // Peashooter
                        type = ModContent.ProjectileType<PeaShot>();
                        break;
                    case 1: // Spread
                        type = ModContent.ProjectileType<SpreadShot>();

                        float spread = MathHelper.PiOver4 * 0.8f;

                        for (int side = -1; side <= 1; side += 2)
                        {
                            for (int i = 1; i <= 2; i++)
                            {
                                Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(side * i * spread);
                                if (i == 1) velocity *= 0.6f;

                                Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
                            }
                        }

                        break;
                }

                return true;
            }
        }

        public override bool AltFunctionUse(Player player) => true;

        public override TagCompound Save() => new TagCompound()
        {
            ["shot"] = shot
        };

        public override void Load(TagCompound tag) => shot = tag.GetInt("shot");

        public override void LoadLegacy(BinaryReader reader) => shot = reader.ReadInt16();

        public override void NetSend(BinaryWriter writer) => writer.WriteVarInt(shot);

        public override void NetRecieve(BinaryReader reader) => shot = reader.ReadVarInt();
    }
}
