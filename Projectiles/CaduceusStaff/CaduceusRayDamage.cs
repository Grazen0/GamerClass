using System;
using GamerClass.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles.CaduceusStaff
{
    public class CaduceusRayDamage : CaduceusRay
    {
        public override int PlayerBuff => Terraria.ID.BuffID.Rage;

        protected override void DrawLaser(SpriteBatch spriteBatch, Vector2[] points, Color color)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < points.Length - 1)
                {
                    Vector2 dir = Utils.SafeNormalize(points[i + 1] - points[i], Vector2.Zero);
                    Vector2 cross = new Vector2(-dir.Y, dir.X); // dir rotated by -90°

                    float ratio = (float)i / points.Length;

                    float diffScale = (float)Math.Sin(ratio * MathHelper.PiOver2 + MathHelper.Pi);

                    if (ratio > 0.8f)
                        diffScale *= (1f - ratio) / 0.2f;

                    points[i] +=
                        cross * 20f * (float)Math.Sin(Main.GlobalTime * 6f - i * 0.06f) * diffScale;
                }
            }

            new TrailHelper(mod.GetTexture("Textures/Trail"), color, _ => 16f).Draw(spriteBatch, points);
        }

        protected override bool ShouldKill(Player player) => !player.controlUseTile || player.altFunctionUse != 2 || player.HeldItem.type != ModContent.ItemType<Items.Weapons.CaduceusStaff>();

        public override Color? GetAlpha(Color lightColor) => Color.Blue;
    }
}
