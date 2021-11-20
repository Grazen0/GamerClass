using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Items.Weapons
{
    public class Splattershot : GamerWeapon
    {
        public override string Texture => "Terraria/Item_" + ItemID.SlimeGun;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overloaded Ink Gun");
            Tooltip.SetDefault("Shoots a splash of blue ink");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 20;
            item.knockBack = 2f;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Splattershot");
            item.useTime = 6;
            item.useAnimation = 18;
            item.reuseDelay = 16;
            item.shoot = ModContent.ProjectileType<Projectiles.Weapons.InkShot>();
            item.shootSpeed = 5f;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 60);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;

            return true;
        }

        public override bool ConsumeRAM(Player player) => !(player.itemAnimation < item.useAnimation - 2);

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.SlimeGun);
            recipe.AddIngredient(ItemID.BluePaint, 60);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
