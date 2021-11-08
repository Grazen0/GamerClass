using GamerClass.Items.Weapons;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.DetachedGlove
{
    public class ChargeShotChannel : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.None;

        private readonly int MaxCharge = 60;
        private readonly float FingertipDistance = 40f;

        private bool chargeSound = true;
        private int charge = 0;
        private int ringTimer = 0;
        private int explosionTimer = 0;
        private int ramTimer = 0;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 1;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.penetrate = -1;
            projectile.GamerProjectile().gamer = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position = player.Center + projectile.velocity * 10f;
            projectile.timeLeft = 2;

            UpdatePlayer(player);
            ChargeShot(player);
            SpawnDusts();
        }

        private void UpdatePlayer(Player player)
        {
            if (projectile.owner == Main.myPlayer)
            {
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
        }

        private void ChargeShot(Player player)
        {
            if (!player.channel)
            {
                projectile.Kill();
            }
            else
            {
                if (++ramTimer > 10)
                {
                    ramTimer = 0;

                    if (
                        player.HeldItem.modItem is GamerWeapon modItem
                        && !player.GamerPlayer().ConsumeRam(modItem.ramUsage / 2, player.HeldItem.useTime)
                        )
                    {
                        projectile.Kill();
                    }
                }

                if (charge < MaxCharge)
                {
                    charge++;
                }
                else
                {
                    if (chargeSound && projectile.owner == Main.myPlayer)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ChargeReady"));
                        chargeSound = false;
                    }
                }
            }
        }

        private void SpawnDusts()
        {
            Vector2 position = Main.player[projectile.owner].Center + projectile.velocity * FingertipDistance;

            Dust dust = Dust.NewDustPerfect(position, DustID.AmberBolt);
            dust.scale = ((float)charge / MaxCharge) * 1.2f;
            dust.noGravity = true;
            dust.velocity *= 0.2f;

            if (charge < MaxCharge)
            {
                Vector2 dustDirection = Main.rand.NextVector2CircularEdge(20f, 20f);
                dust = Dust.NewDustPerfect(position - dustDirection, DustID.AmberBolt, Scale: 0.8f);
                dust.noGravity = true;
                dust.velocity = dustDirection * 0.1f;
            }
            else
            {
                if (Main.rand.NextBool(2))
                {
                    dust = Dust.NewDustPerfect(position, DustID.AmberBolt);
                    dust.noGravity = true;
                    dust.velocity *= 1.2f;
                }

                if (--ringTimer <= 0)
                {
                    Vector2 direction = -Vector2.UnitY;
                    int dusts = 45;
                    float separation = MathHelper.TwoPi / dusts;

                    for (int d = 0; d < dusts; d++)
                    {
                        dust = Dust.NewDustPerfect(position, DustID.AmberBolt);
                        dust.velocity = direction * 1.5f;
                        dust.noGravity = true;

                        direction = direction.RotatedBy(separation);
                    }

                    ringTimer = 30;
                }

                if (--explosionTimer <= 0)
                {
                    GamerUtils.DustExplosion(4, position, DustID.AmberBolt, 1.5f, Main.rand.NextFloat(MathHelper.Pi));
                    explosionTimer = 12;
                }
            }
        }

        public override bool PreKill(int timeLeft)
        {
            Vector2 fingertip = Main.player[projectile.owner].Center + projectile.velocity * FingertipDistance;
            if (charge < MaxCharge)
            {
                Player player = Main.player[projectile.owner];
                player.itemTime = player.itemAnimation = 10;

                GamerUtils.DustExplosion(4, fingertip, DustID.AmberBolt, 1f, Main.rand.NextFloat(MathHelper.Pi));

                for (int d = 0; d < 6; d++)
                {
                    Dust dust = Dust.NewDustPerfect(fingertip, DustID.AmberBolt, Scale: 1.2f);
                    dust.noGravity = true;
                    dust.velocity *= 1.6f;
                }
            }
            else
            {
                float baseRotation = Main.rand.NextFloat(MathHelper.Pi);
                for (int e = 0; e < 2; e++)
                {
                    GamerUtils.DustExplosion(7, fingertip, DustID.AmberBolt, 2f - e * 0.6f, baseRotation, dustPerPoint: 4, dustScale: 1.2f);
                }

                for (int d = 0; d < 12; d++)
                {
                    Dust dust = Dust.NewDustPerfect(fingertip, DustID.AmberBolt, Scale: 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                }
            }

            if (projectile.owner == Main.myPlayer)
            {
                string soundName = charge < MaxCharge ? "PeaShot" : "ChargeRelease";
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/" + soundName));

                Projectile.NewProjectile(
                    projectile.position + projectile.velocity * FingertipDistance,
                    projectile.velocity * (charge < MaxCharge ? 15f : 18f),
                    charge < MaxCharge ? ModContent.ProjectileType<ChargeShotSmall>() : ModContent.ProjectileType<ChargeShotBig>(),
                    charge < MaxCharge ? projectile.damage : projectile.damage * 15,
                    charge < MaxCharge ? projectile.knockBack : projectile.knockBack * 3f,
                    projectile.owner);
            }

            return base.PreKill(timeLeft);
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool CanDamage() => false;
    }
}
