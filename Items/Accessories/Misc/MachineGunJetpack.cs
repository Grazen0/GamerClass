using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Accessories.Misc
{
    [AutoloadEquip(EquipType.Wings)]
    public class MachineGunJetpack : ModItem
    {
        public override bool CloneNewInstances => true;

        public SoundEffectInstance soundInstance = null;

        public override ModItem Clone(Item item)
        {
            var clone = (MachineGunJetpack)base.Clone(item);
            clone.soundInstance = soundInstance;
            return clone;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strapped Machine Gun");
            Tooltip.SetDefault("'The original and the best.'");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 120;

            var modPlayer = player.GamerPlayer();

            if (Main.myPlayer == player.whoAmI)
            {
                if (player.controlJump && !player.jumpAgainCloud && player.jump == 0 && player.velocity.Y != 0f && !player.mount.Active && !player.mount.Cart)
                {
                    if (soundInstance?.State != SoundState.Playing)
                    {
                        soundInstance = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/MachineGunJetpack").WithVolume(0.6f));
                    }

                    if (modPlayer.jetpackBulletCooldown <= 0)
                    {
                        modPlayer.jetpackBulletCooldown = 3;

                        int damage = (int)(20 * player.allDamage * player.allDamageMult * modPlayer.gamerDamageMult);

                        Projectile.NewProjectile(
                            player.Center + new Vector2(-16f * player.direction, 16f),
                            Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(20)) * 36f,
                            ModContent.ProjectileType<Projectiles.JetpackBullet>(),
                            (int)MathHelper.Max(1, damage),
                            1.5f,
                            player.whoAmI);
                    }
                }
                else if (soundInstance?.State == SoundState.Playing)
                {
                    soundInstance.Stop();
                }
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 9f;
            acceleration *= 2.5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Jetpack);
            recipe.AddIngredient(ItemID.EndlessMusketPouch);
            recipe.AddIngredient(ItemID.HallowedBar, 10);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
