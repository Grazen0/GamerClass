using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Buffs
{
    public class HaloShield : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff_" + BuffID.Ironskin;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Energy Shield");
            Description.SetDefault("70% increased damage reduction and movement speed reduced");

            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GamerPlayer().haloShield = true;
            player.endurance += 0.7f;
            player.moveSpeed -= 0.5f;
        }
    }
}
