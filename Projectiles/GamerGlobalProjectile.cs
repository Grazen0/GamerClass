using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Projectiles
{
    public class GamerGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool gamer;
        public bool firstTick { get; private set; } = true;

        public override void PostAI(Projectile projectile)
        {
            if (firstTick) firstTick = false;
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (gamer)
                ModifyGamerCrit(projectile, ref crit);
        }

        public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            if (gamer)
                ModifyGamerCrit(projectile, ref crit);
        }

        private void ModifyGamerCrit(Projectile projectile, ref bool crit)
        {
            Player player = Main.player[projectile.owner];
            int critChance = player.HeldItem.crit;

            ItemLoader.GetWeaponCrit(player.HeldItem, player, ref critChance);
            PlayerHooks.GetWeaponCrit(player, player.HeldItem, ref critChance);

            crit = critChance >= 100 || Main.rand.Next(0, 100) < critChance;
        }
    }
}
