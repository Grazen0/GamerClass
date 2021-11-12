using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class StarKeeper : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Causes bees to rain from the sky");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.StarWrath);
            item.height = 58;
            item.shootSpeed *= 3f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 offset = Main.screenPosition + Main.MouseWorld;

			for (int i = 0; i < 20; i++)
			{
				position = player.Center + new Vector2(-Main.rand.NextFloat(401f) * player.direction, -650f - (30f * i));

				Vector2 velocity = offset - position;
				if (velocity.Y < 0f)
					velocity.Y *= -1f;
				
				if (velocity.Y < 20f)
					velocity.Y = 20f;
				
				velocity = Vector2.Normalize(velocity) * item.shootSpeed;

                type = player.beeType();
                damage = player.beeDamage(damage);
                knockBack = player.beeKB(knockBack);

                Projectile.NewProjectile(position, velocity, type, damage * 2, knockBack, player.whoAmI);
			}

			return false;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, lightColor.A - item.alpha);
    }
}
