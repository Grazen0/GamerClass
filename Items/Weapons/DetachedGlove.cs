using GamerClass.Projectiles.DetachedGlove;
using Microsoft.Xna.Framework;
using System;
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
            Tooltip.SetDefault("Right-click to switch shot type\n'A cup's weapon of choice'");
        }

        public override void SafeSetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.noMelee = true;
            item.damage = 60;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, $"Sounds/Item/PeaShot");
            item.shoot = ModContent.ProjectileType<PeaShot>();
            item.shootSpeed = 40f;
            item.useTime = item.useAnimation = 10;
            item.autoReuse = true;

            ramUsage = 2;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = item.useAnimation = 45;
                item.UseSound = SoundID.Item4;

                return true;
            }
            else
            {
                item.UseSound = SoundID.Item1;
                string soundName = "";

                switch (shot)
                {
                    case 0: // Peashooter
                        item.useTime = item.useAnimation = 8;
                        soundName = "PeaShot";
                        ramUsage = 1;
                        break;
                    case 1: // Spread
                        item.useTime = item.useAnimation = 9;
                        soundName = "SpreadShot";
                        ramUsage = 1;
                        break;
                    case 2: // Chaser
                        item.useTime = item.useAnimation = 10;
                        soundName = "ChaserShot";
                        ramUsage = 2;
                        break;
                    case 3: // Lobber
                        item.useTime = item.useAnimation = 20;
                        soundName = "LobberShot";
                        ramUsage = 3;
                        break;
                    case 4: // Charge
                        item.channel = true;
                        item.UseSound = null;
                        item.useTime = item.useAnimation = 15;
                        soundName = "ChargeShot";
                        ramUsage = 3;
                        break;
                    case 5: // Roundabout
                        item.useTime = item.useAnimation = 16;
                        soundName = "RoundaboutShot";
                        ramUsage = 3;
                        break;
                }

                if (soundName != "")
                    item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/" + soundName);

                return base.CanUseItem(player);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string shotName = "[INVALID]";
            Color color = Color.Red;

            switch (shot)
            {
                case 0:
                    shotName = "Peashooter";
                    color = Color.LightBlue;
                    break;
                case 1:
                    shotName = "Spread";
                    color = Color.IndianRed;
                    break;
                case 2:
                    shotName = "Chaser";
                    color = Color.LightGreen;
                    break;
                case 3:
                    shotName = "Lobber";
                    color = Color.SteelBlue;
                    break;
                case 4:
                    shotName = "Charge";
                    color = Color.Orange;
                    break;
                case 5:
                    shotName = "Roundabout";
                    color = Color.CadetBlue;
                    break;
            }

            tooltips.Add(new TooltipLine(mod, "DetachedGloveShotType", "Current shot: " + shotName)
            {
                overrideColor = color
            });

            base.ModifyTooltips(tooltips);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                // Switch shot type
                shot = (shot + 1) % 6;
                player.itemRotation = -MathHelper.PiOver2 * player.direction;

                int dustType = -1;
                Color dustColor = Color.White;

                switch (shot)
                {
                    case 0: // Peashooter
                        dustType = DustID.Frost;
                        break;
                    case 1: // Spread
                        dustType = DustID.BubbleBurst_White;
                        dustColor = new Color(1f, 0.1f, 0.1f);
                        break;
                    case 2: // Chaser
                        dustType = DustID.GreenFairy;
                        break;
                    case 3: // Lobber
                        dustType = DustID.Venom;
                        break;
                    case 4: // Charge
                        dustType = DustID.AmberBolt;
                        break;
                    case 5: // Roundabout
                        dustType = DustID.IceTorch;
                        break;
                }

                Vector2 direction = -Vector2.UnitY;
                int dusts = 45;
                float separation = MathHelper.TwoPi / dusts;

                for (int d = 0; d < dusts; d++)
                {
                    Dust dust = Dust.NewDustPerfect(position, dustType, newColor: dustColor, Scale: 1.5f);
                    dust.noGravity = true;
                    dust.velocity = direction * 3f;

                    if (shot == 5)
                    {
                        dust.velocity.Y *= 0.75f;
                    }

                    direction = direction.RotatedBy(separation);
                }
                
                for (int d = 0; d < 8; d++)
                {
                    Dust dust = Dust.NewDustPerfect(position, dustType, Scale: 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 2.5f;
                }

                return false;
            }
            else
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                Vector2 frontDirection = Vector2.Normalize(velocity);
                Vector2 fingerOffset = frontDirection * 38f;

                if (Collision.CanHit(position, 0, 0, position + fingerOffset, 0, 0))
                {
                    position += fingerOffset;
                }

                bool shoot = true;

                int dustType = -1;
                Vector2 dustPosition = position;
                Color dustColor = Color.White;
                float dustScale = 1f;
                float explosionSize = 2f;

                switch (shot)
                {
                    case 0: // Peashooter
                        dustType = DustID.Frost;
                        dustScale = 0.8f;
                        explosionSize = 2.5f;

                        float speedMultiply = Main.rand.NextFloat(0.8f, 0.9f);
                        speedX *= speedMultiply;
                        speedY *= speedMultiply;

                        position += frontDirection.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-12f, 12f);
                        break;
                    case 1: // Spread
                        type = ModContent.ProjectileType<SpreadShot>();
                        damage = (int)(damage * 0.36f);
                        knockBack /= 2;

                        dustType = DustID.BubbleBurst_White;
                        dustColor = new Color(1f, 0.1f, 0.1f);

                        speedX *= 0.8f;
                        speedY *= 0.8f;

                        float spread = MathHelper.PiOver4 * 0.35f;

                        for (int i = -2; i <= 2; i++)
                        {
                            if (i == 0) continue;

                            Vector2 shotVelocity = new Vector2(speedX, speedY).RotatedBy(i * spread) * 0.9f;

                            int abs = Math.Abs(i);
                            if (abs == 1) 
                                shotVelocity *= 0.65f;

                            Projectile.NewProjectile(position, shotVelocity, type, damage, knockBack, player.whoAmI, abs);
                        }

                        break;
                    case 2: // Chaser
                        type = ModContent.ProjectileType<ChaserShot>();
                        damage = (int)(damage * 1.1f);
                        knockBack *= 1.5f;

                        dustType = 140;
                        explosionSize = 2.2f;

                        Projectile.NewProjectile(position, new Vector2(speedX, speedY) * 0.4f, type, damage, knockBack, player.whoAmI, -1f);
                        shoot = false;
                        break;
                    case 3: // Lobber
                        type = ModContent.ProjectileType<LobberShot>();
                        damage = (int)(damage * 2.8f);
                        knockBack *= 2f;

                        dustType = DustID.Venom;

                        speedX *= 0.4f;
                        speedY *= 0.4f;
                        break;
                    case 4: // Charge
                        type = ModContent.ProjectileType<ChargeShotChannel>();
                        knockBack *= 0.75f;
                        break;
                    case 5: // Roundabout
                        type = ModContent.ProjectileType<RoundaboutShot>();
                        damage *= 2;
                        knockBack *= 1.2f;

                        dustType = DustID.Ultrabright;

                        float forceDirection = (-velocity).ToRotation();
                        velocity *= Main.rand.NextFloat(0.45f, 0.5f);

                        Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI, forceDirection, 90f);

                        shoot = false;
                        break;
                }

                if (dustType != -1) 
                    GamerUtils.DustExplosion(7, dustPosition, dustType, explosionSize, dustScale: dustScale, color: dustColor);

                return shoot;
            }
        }

        public override Vector2? HoldoutOffset() => new Vector2(10f, 4f);

        public override bool AltFunctionUse(Player player) => true;

        public override void NetSend(BinaryWriter writer) => writer.WriteVarInt(shot);

        public override void NetRecieve(BinaryReader reader) => shot = reader.ReadVarInt();
    }
}
