using System;
using GamerClass.Projectiles.Weapons.GasterBlaster;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class GasterBlaster : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Funny Looking Bone");
            Tooltip.SetDefault("'It wants to tell you a pun about skeletons'");
        }

        public override void SafeSetDefaults()
        {
            item.width = item.height = 42;
            item.noMelee = true;
            item.damage = 20;
            item.crit = 0;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(gold: 10);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 45;
            item.UseSound = SoundID.Item78;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<GasterSkull>();

            ramUsage = 4;
        }

        public override void GetWeaponCrit(Player player, ref int crit) => crit = 0;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float spawnRange = 400f;

            Vector2 spawnAt = new Vector2(Main.MouseWorld.X + Main.rand.NextFloat(-spawnRange, spawnRange), 580f);
            if (Main.rand.NextBool()) spawnAt.Y *= -1f;

            spawnAt.Y += player.Center.Y;

            Vector2 toMouse = Main.MouseWorld - spawnAt;
            Vector2 velocity = toMouse * Main.rand.NextFloat(0.09f, 0.095f);

            float rotation = Main.rand.NextFloat(MathHelper.TwoPi);

            Projectile.NewProjectile(spawnAt, velocity, type, damage, knockBack, player.whoAmI, 0f, rotation);

            return false;
        }

        public override void SafeHoldItem(Player player)
        {
            // Totally not adapted from Yoraiz0r's Spell
            int headgearOffset = player.bodyFrame.Y / 56;
            if (headgearOffset >= Main.OffsetsPlayerHeadgear.Length)
                headgearOffset = 0;

            Vector2 eyeOffset = new Vector2(3f * player.direction - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir + player.gfxOffY)
                + player.Size / 2 + Main.OffsetsPlayerHeadgear[headgearOffset];

            Vector2 shadowEyeOffset = new Vector2(3f * player.shadowDirection[1] - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir)
                + player.Size / 2 + Main.OffsetsPlayerHeadgear[headgearOffset];

            if (player.fullRotation != 0f)
            {
                eyeOffset = eyeOffset.RotatedBy(player.fullRotation, player.fullRotationOrigin);
                shadowEyeOffset = shadowEyeOffset.RotatedBy(player.fullRotation, player.fullRotationOrigin);
            }

            Vector2 end = player.position + eyeOffset;
            Vector2 start = player.oldPosition + shadowEyeOffset;

            if (player.mount.Active)
            {
                if (player.mount.Cart)
                {
                    int dir = Math.Sign(player.velocity.X);
                    if (dir == 0)
                        dir = player.direction;

                    Vector2 offset = new Vector2(
                        MathHelper.Lerp(0f, -8f, player.fullRotation / MathHelper.PiOver4),
                        MathHelper.Lerp(0f, 2f, Math.Abs(player.fullRotation / MathHelper.PiOver4))).RotatedBy(player.fullRotation);

                    if (dir == Math.Sign(player.fullRotation))
                    {
                        offset *= MathHelper.Lerp(1f, 0.6f, Math.Abs(player.fullRotation / MathHelper.PiOver4));
                    }

                    start += offset;
                    end += offset;
                }

                start.Y -= player.mount.PlayerOffset / 2;
                end.Y -= player.mount.PlayerOffset / 2;
            }

            int distance = (int)Vector2.Distance(end, start);
            int dusts = distance + 1;

            for (float i = 1f; i <= dusts; i++)
            {
                Dust dust = Dust.NewDustPerfect(player.Center, DustID.Clentaminator_Cyan);
                dust.position = Vector2.Lerp(start, end, i / dusts);
                dust.noGravity = true;
                dust.velocity *= 0f;
                dust.scale = 0.6f;
            }

            // Cast lights
            DelegateMethods.v3_1 = Color.Cyan.ToVector3() * 0.4f;
            if (player.velocity != Vector2.Zero)
                Utils.PlotTileLine(player.Center, player.Center + player.velocity * 2f, 4f, DelegateMethods.CastLightOpen);
            else
                Utils.PlotTileLine(player.Left, player.Right, 4f, DelegateMethods.CastLightOpen);
        }
    }
}
