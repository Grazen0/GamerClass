using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace GamerClass
{
    public class GamerUtils
    {
        public static void DustExplosion(Vector2 position, int dustType, float size, float dustScale = 1f, Color? color = null)
        {
            Vector2 baseDirection = -Vector2.UnitY;
            int points = 7;
            int dustPerPoint = 4;
            float separation = MathHelper.TwoPi / points;

            float minVelocity = size / 2;
            float maxVelocity = size;

            for (int p = 0; p < points; p++)
            {
                for (int side = -1; side <= 2; side += 2)
                {
                    Vector2 sideDirection = baseDirection.RotatedBy(side * (separation / 2f));

                    for (int d = 0; d < dustPerPoint; d++)
                    {
                        float dustRadius = (float)d / dustPerPoint;

                        Vector2 direction = Vector2.Lerp(baseDirection, sideDirection, dustRadius);
                        float dustVelocity = maxVelocity - ((maxVelocity - minVelocity) * (float)Math.Sin(dustRadius * MathHelper.PiOver2));

                        Dust dust = Dust.NewDustPerfect(position, dustType, Scale: dustScale, newColor: color ?? Color.White);
                        dust.scale *= 0.8f;
                        dust.velocity = direction * dustVelocity;
                        dust.noGravity = true;
                    }
                }

                baseDirection = baseDirection.RotatedBy(separation);
            }
        }
    }
}
