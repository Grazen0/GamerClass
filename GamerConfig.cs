using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace GamerClass
{
    public class GamerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Play Raccoon Flap Sound")]
        [Tooltip("Replace the normal flap sound when using the Strange Leaf")]
        [DefaultValue(true)]
        public bool RaccoonFlySound;

        [Label("Scar rounds and magazines")]
        [Tooltip("Spawn bullet round and magazine gores when using the Scar")]
        [DefaultValue(true)]
        public bool ScarGores;
    }
}
