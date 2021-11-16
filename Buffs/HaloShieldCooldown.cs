using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass.Buffs
{
    public class HaloShieldCooldown : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff_" + BuffID.Slow;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Energy Shield Cooldown");
            Description.SetDefault("You can't activate the energy shield");

            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GamerPlayer().haloShieldCooldown = true;
        }
    }
}
