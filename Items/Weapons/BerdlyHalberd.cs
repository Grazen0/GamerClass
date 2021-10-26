using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class BerdlyHalberd : GamerWeapon
    {
        public new int ramUsage = 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Halbird");
            Tooltip.SetDefault("The weapon of a true gamer\n'It smells like math'");
        }

        public override void SafeSetDefaults()
        {
            item.width = 42;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.height = 34;
            item.noMelee = true;
            item.damage = 110;
            item.useAnimation = item.useTime = 45;
            item.shoot = ModContent.ProjectileType<Projectiles.BerdlyHalberd.HalberdSpear>();
            item.shootSpeed = 18f;
            item.knockBack = 6f;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float MaxCursorLength = new Vector2(1920 / 2, 1080 / 2).Length();
            float cursorLength = MathHelper.Min((Main.MouseWorld - position).Length(), MaxCursorLength);
            float SineFactor = 0.5f + cursorLength * 0.0008f;

            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0f, SineFactor);

            return false;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] < 1;
    }
}
