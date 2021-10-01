using Terraria.ModLoader;

namespace GamerClass.Projectiles.TF2RocketLauncher
{
    public class RocketExplosion : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_0";
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 150;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 3;
        }
    }
}
