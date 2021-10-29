using GamerClass.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace GamerClass
{
    public static class GamerUtils
    {
        public static List<Dust> DustExplosion(
            int points,
            Vector2 position,
            int dustType,
            float size,
            float baseRotation = -MathHelper.PiOver2,
            float dustScale = 1f,
            Color? color = null,
            int dustPerPoint = 4)
        {
            List<Dust> dusts = new List<Dust>();

            Vector2 baseDirection = baseRotation.ToRotationVector2();
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

                        Dust dust = Dust.NewDustPerfect(position, dustType, newColor: color ?? Color.White);
                        dust.scale = dustScale;
                        dust.velocity = direction * dustVelocity;
                        dust.noGravity = true;

                        dusts.Add(dust);
                    }
                }

                baseDirection = baseDirection.RotatedBy(separation);
            }

            return dusts;
        }

        public static GamerGlobalProjectile GamerProjectile(this Projectile projectile) => 
            projectile.GetGlobalProjectile<GamerGlobalProjectile>();
    }
}
