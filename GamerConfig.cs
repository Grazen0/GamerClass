using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace GamerClass
{
    public class GamerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Strange Leaf Raccoon Flap Sound")]
        [Tooltip("Replace the normal flap sound when using the Strange Leaf")]
        [DefaultValue(true)]
        public bool RaccoonFlySound;

        [Label("Scar Rounds and Magazines")]
        [Tooltip("Spawn bullet round and magazine gores when using the Scar")]
        [DefaultValue(true)]
        public bool ScarGores;

        [Increment(2)]
        [Label("Gaster Blaster Screen Shake")]
        [Tooltip("(Set to 0 to disable)")]
        [DefaultValue(16)]
        [Slider]
        [Range(0, 32)]
        public int GasterBlasterScreenShake;

        [Label("Ink-Black Sepia Dye Screen Effect")]
        [Tooltip("Use an old movie screen effect when equipping Ink-Black Sepia Dye")]
        [DefaultValue(true)]
        public bool OldMovieEffect;
    }
}
