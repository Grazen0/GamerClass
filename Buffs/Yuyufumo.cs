using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Buffs
{
    public class Yuyufumo : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Yuyufumo");
            Description.SetDefault("She's doing her best");

            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GamerPlayer().yuyufumo = true;

            int yuyufumo = ModContent.ProjectileType<Projectiles.Yuyufumo>();
            if (player.ownedProjectileCounts[yuyufumo] <= 0)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, yuyufumo, 0, 0f, player.whoAmI);
            }
        }
    }
}
