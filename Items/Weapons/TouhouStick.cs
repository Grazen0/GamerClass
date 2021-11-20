using GamerClass.Projectiles.Weapons.TouhouStick;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class TouhouStick : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Gohei");
            Tooltip.SetDefault("Shoots a barrage of paper charms\nRight click to shoot needles");
        }

        public override void SafeSetDefaults()
        {
            item.width = 38;
            item.height = 40;
            item.noMelee = true;
            item.damage = 16;
            item.knockBack = 0.5f;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/TouhouStick");
            item.rare = ItemRarityID.LightRed;
            item.autoReuse = true;
            item.useAnimation = 20;
            item.useTime = 5;
            item.shoot = ModContent.ProjectileType<OrangeCharm>();
            item.shootSpeed = 50f;

            ramUsage = 2;
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);

            // Main shot
            for (int side = -1; side <= 1; side += 2)
            {
                Vector2 offset = Vector2.Normalize(velocity).RotatedBy(MathHelper.PiOver2) * side * 20f;
                Projectile.NewProjectile(position + offset, velocity, type, damage, knockBack, player.whoAmI);
            }

            // Special shot
            float velocityLength = velocity.Length();

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile orb = Main.projectile[i];
                if (!orb.active || orb.owner != player.whoAmI || orb.type != ModContent.ProjectileType<YinYangOrb>()) continue;

                if (player.altFunctionUse == 2)
                {
                    Vector2 offset = orb.velocity.RotatedBy(MathHelper.PiOver2) * 16f;

                    for (int side = -1; side <= 1; side += 2)
                    {
                        Projectile.NewProjectile(
                            orb.Center + offset * side,
                            orb.velocity * velocityLength * 1.2f,
                            ModContent.ProjectileType<Needle>(),
                            (int)(damage * 0.75f),
                            knockBack,
                            player.whoAmI);
                    }
                }
                else
                {
                    Projectile.NewProjectile(
                        orb.Center,
                        orb.velocity.RotatedBy(MathHelper.ToRadians(12) * orb.ai[0]) * velocityLength * 0.75f,
                        ModContent.ProjectileType<HomingCharm>(),
                        damage / 2,
                        knockBack,
                        player.whoAmI, 
                        -1);
                }
            }

            return false;
        }

        public override void SafeHoldItem(Player player)
        {
            int yinYangOrb = ModContent.ProjectileType<YinYangOrb>();
            if (Main.myPlayer != player.whoAmI || player.ownedProjectileCounts[yinYangOrb] >= 2) return;

            Vector2 direction = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.UnitX);

            for (int side = -1; side <= 1; side += 2)
            {
                Projectile.NewProjectile(player.Center, direction, yinYangOrb, item.damage, item.knockBack, player.whoAmI, side);
            }
        }

        public override bool AltFunctionUse(Player player) => true;
    }
}
