using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class SpaceInvadersCannon : GamerWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Joystick");
            Tooltip.SetDefault("'There's some sort of martian scribble on it'");
        }

        public override void SafeSetDefaults()
        {
            item.noMelee = true;
            item.damage = 22;
            item.useTime = item.useAnimation = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/SpaceInvadersCannon");
            item.shoot = ModContent.ProjectileType<Projectiles.DustyLaserCannon.ShoddyBeam>();
            item.shootSpeed = 30;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 front = Vector2.Normalize(new Vector2(speedX, speedY));
            position += front * 12f;

            return true;
        }
    }
}
