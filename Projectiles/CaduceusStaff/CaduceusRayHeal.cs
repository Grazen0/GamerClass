using GamerClass.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace GamerClass.Projectiles.CaduceusStaff
{
    public class CaduceusRayHeal : CaduceusRay
    {
        public override int PlayerBuff => Terraria.ID.BuffID.Regeneration;

        protected override void DrawLaser(SpriteBatch spriteBatch, Vector2[] points, Color color)
        {
            Vector2[] pointsOpposite = new Vector2[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                pointsOpposite[i] = points[i];

                if (i < points.Length - 1)
                {
                    Vector2 dir = Utils.SafeNormalize(points[i + 1] - points[i], Vector2.Zero);
                    Vector2 cross = new Vector2(-dir.Y, dir.X); // dir rotated by -90°

                    const float timeScale = 2f;
                    float ratio = (float)i / points.Length;

                    float diffScale = (float)Math.Sin(ratio * MathHelper.PiOver2 + MathHelper.Pi);

                    if (ratio > 0.9f)
                        diffScale *= (1f - ratio) / 0.1f;

                    Vector2 offset =
                        cross * (float)Math.Sin(i * 0.05f) * 24f * diffScale * (float)Math.Sin(Main.GlobalTime * timeScale)
                        + dir * (float)Math.Cos(Main.GlobalTime * timeScale) * 12f * diffScale;

                    points[i] += offset;
                    pointsOpposite[i] -= offset;
                }
            }

            TrailHelper trail = new TrailHelper(mod.GetTexture("Textures/Trail"), color, _ => 12f);
            trail.Draw(spriteBatch, points);
            trail.Draw(spriteBatch, pointsOpposite);
        }

        protected override bool ShouldKill(Player player) => !player.channel;

        public override Color? GetAlpha(Color lightColor) => Color.Orange;
    }
}
