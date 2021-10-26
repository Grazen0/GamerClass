using Terraria.ID;
using Terraria.ModLoader;
using GamerClass.Projectiles.TouhouStick;
using Terraria;
using Microsoft.Xna.Framework;

namespace GamerClass.Items.Weapons
{
    public class TouhouStick : GamerWeapon
    {
        public new int ramUsage = 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reimu's Gohei");
            Tooltip.SetDefault("Left click for ReimuA, right click for ReimuB\n'raymoo'");
        }

        public override void SafeSetDefaults()
        {
            item.width = 38;
            item.height = 40;
            item.noMelee = true;
            item.damage = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.LightRed;
            item.autoReuse = true;
            item.useAnimation = 24;
            item.useTime = 6;
            item.shoot = ModContent.ProjectileType<OrangeCharm>();
            item.shootSpeed = 30f;
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            Vector2 frontDirection = Vector2.Normalize(velocity);

            // Main shot
            for (int side = -1; side <= 1; side += 2)
            {
                for (int shot = 1; shot <= 2; shot++)
                {
                    float separation;
                    if (player.altFunctionUse == 2)
                    {
                        // ReimuB style
                        separation = 0.16f;
                    }
                    else
                    {
                        // ReimuA style
                        separation = shot == 1 ? 0.03f : 0.09f;
                    }

                    float angle = MathHelper.PiOver4 * shot * separation;
                    Vector2 orangeCharmVelocity = new Vector2(speedX, speedY).RotatedBy(angle * side);
                    
                    Projectile.NewProjectile(position, orangeCharmVelocity, type, damage, knockBack, player.whoAmI);
                }
            }

            // Special shot

            if (player.altFunctionUse == 2)
            {
                // ReimuB needles
                float needlesPerSide = 3;
                float needleSpacing = 12f;

                Vector2 perpendicular = frontDirection.RotatedBy(MathHelper.PiOver2);

                for (int side = -1; side <= 1; side += 2)
                {
                    Vector2 basePosition = player.Center + perpendicular * side * 10f;
                    if (side == -1)
                    {
                        basePosition -= perpendicular * needleSpacing * needlesPerSide;
                    }

                    for (int needle = 0; needle < needlesPerSide; needle++)
                    {
                        int animationIndex = (item.useAnimation - player.itemAnimation - 1) / item.useTime;

                        if (needle == 1 && animationIndex > 0) continue;
                        if (needle == 2 && animationIndex % 2 != 0) continue;


                        Vector2 needlePosition = basePosition + (perpendicular * needleSpacing * needle);
                        Vector2 needleVelocity = frontDirection * item.shootSpeed * Main.rand.NextFloat(1.4f, 1.6f);

                        Vector2 offset = frontDirection * Main.rand.NextFloat(64f);
                        if (Collision.CanHit(needlePosition, 0, 0, needlePosition + offset, 0, 0))
                        {
                            needlePosition += offset;
                        }

                        Projectile.NewProjectile(
                            needlePosition,
                            needleVelocity,
                            ModContent.ProjectileType<Needle>(),
                            (int)(damage * 1.2f),
                            knockBack, player.whoAmI);
                    }
                }
            }
            else
            {
                // ReimuA homing shots
                for (int side = -1; side <= 1; side += 2)
                {
                    Vector2 perpendicular = frontDirection.RotatedBy(MathHelper.PiOver2 * side);
                    Vector2 shotPosition = player.Center + perpendicular * 20f;

                    int animationIndex = (item.useAnimation - player.itemAnimation - 1) / item.useTime;

                    Vector2 baseDirection = frontDirection.RotatedBy(MathHelper.PiOver2 * -side * 0.9f);
                    Vector2 homingShotVelocity = baseDirection.RotatedBy(animationIndex * MathHelper.PiOver4 * 0.42f * side) * 30f;

                    Projectile.NewProjectile(
                        shotPosition, 
                        homingShotVelocity, 
                        ModContent.ProjectileType<HomingCharm>(), 
                        damage, 
                        knockBack, 
                        player.whoAmI, 
                        -1f);
                }
            }

            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/TouhouStick"));

            return player.altFunctionUse == 2;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
