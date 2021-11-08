using GamerClass.Buffs;
using GamerClass.Items;
using GamerClass.Items.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace GamerClass
{
    public class GamerPlayer : ModPlayer
    {
        public int screenShake = 0;
        public int jetpackBulletCooldown = 0;

        public bool glasses3D;
        public bool linkArmorBonus;
        public bool friskSet;

        public bool gamerCooldown;
        public bool inked;

        public int maxRam;
        public int maxRam2;
        public int usedRam;
        public int ramRegenTimer;
        public float ramRegenRate;
        public float ramUsageMult;

        public float gamerDamageMult;
        public float gamerKnockback;
        public float gamerUseTimeMult;
        public int gamerCrit;

        public override void Initialize()
        {
            maxRam = 5;
        }

        public override void ResetEffects() => ResetVariables();

        public override void UpdateDead() => ResetVariables();

        private void ResetVariables()
        {
            if (screenShake > 0)
                screenShake--;

            if (jetpackBulletCooldown > 0)
                jetpackBulletCooldown--;

            glasses3D = false;
            linkArmorBonus = false;
            friskSet = false;

            gamerCooldown = false;
            inked = false;

            maxRam2 = maxRam;
            ramUsageMult = 1f;

            gamerDamageMult = 1f;
            gamerKnockback = 0f;
            gamerUseTimeMult = 1f;
            gamerCrit = 0;
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (gamerCooldown)
            {
                Vector2 position = player.position;
                position.Y -= player.height / 4;

                Vector2 direction = -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2 * 0.8f);

                Dust dust = Dust.NewDustDirect(position, player.width, player.height / 2, DustID.Smoke, Scale: 1.6f);
                dust.velocity = direction * 2f;

                if (Main.rand.NextBool(8))
                {
                    dust = Dust.NewDustDirect(position, player.width, (int)(player.height * 0.75f), DustID.Fire, Scale: 1.5f);
                    dust.velocity *= 2f;
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (!gamerCooldown)
            {
                if (usedRam > 0 && ++ramRegenTimer > 20f * ramRegenRate)
                {
                    usedRam = (int)MathHelper.Max(usedRam - 1, 0);
                    ramRegenTimer = 0;
                }
                else
                {
                    ramRegenRate *= 0.997f;
                }
            }
        }

        public override void PostUpdate()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                if (glasses3D)
                {
                    if (!Filters.Scene["GamerClass:Glasses3D"].IsActive())
                    {
                        Filters.Scene.Activate("GamerClass:Glasses3D").GetShader().UseOpacity(0.3f).UseProgress(18f);
                    }
                }
                else if (Filters.Scene["GamerClass:Glasses3D"].IsActive())
                {
                    Filters.Scene["GamerClass:Glasses3D"].GetShader().UseProgress(0f);
                    Filters.Scene.Deactivate("GamerClass:Glasses3D");
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (crit && friskSet && item.modItem is GamerWeapon)
                SpawnUndertaleSoul(target.Center);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (crit && friskSet && proj.GamerProjectile().gamer)
                SpawnUndertaleSoul(target.Center);
        }

        public override void ModifyScreenPosition()
        {
            if (screenShake > 0)
                Main.screenPosition += Main.rand.NextVector2Circular(screenShake, screenShake);
        }

        private void SpawnUndertaleSoul(Vector2 position)
        {
            var type = new WeightedRandom<int>();

            type.Add(ModContent.ItemType<RedSoul>());
            type.Add(ModContent.ItemType<BlueSoul>());
            type.Add(ModContent.ItemType<GreenSoul>());
            type.Add(ModContent.ItemType<PurpleSoul>());
            type.Add(ModContent.ItemType<YellowSoul>());
            type.Add(ModContent.ItemType<OrangeSoul>());
            type.Add(ModContent.ItemType<LightBlueSoul>());

            Item.NewItem(position, type, noBroadcast: true);
        }

        public bool ConsumeRam(int amount, int regenDelay)
        {
            if (gamerCooldown) return false;

            ramRegenTimer = -regenDelay;
            ramRegenRate = 1f;
            amount = (int)Math.Round(amount * ramUsageMult);

            if (amount <= 0) return true;

            usedRam += amount;

            if (usedRam >= maxRam2)
            {
                // RAM overheat
                usedRam = maxRam2;
                player.AddBuff(ModContent.BuffType<GamerCooldown>(), 300);

                for (int d = 0; d < 20; d++)
                {
                    bool fire = Main.rand.NextBool(3);

                    int size = 20;
                    Vector2 position = player.Center - new Vector2(1f, 1f) * (size / 2);

                    Dust dust = Dust.NewDustDirect(position, size, size, fire ? DustID.Fire : DustID.Smoke);
                    dust.noGravity = true;
                    dust.fadeIn = 2f;
                    dust.velocity *= fire ? 8f : 4f;
                    dust.scale = 1f;
                }

                for (int g = 0; g < 6; g++)
                {
                    Gore gore = Gore.NewGoreDirect(player.position, Vector2.Zero, 99, Scale: 1.25f);
                    gore.velocity *= 0.75f;
                }

                Main.PlaySound(SoundID.NPCHit53);

                return false;
            }

            return true;
        }
    }
}
