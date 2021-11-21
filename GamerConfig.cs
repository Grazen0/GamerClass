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

        [Label("Screen Shake Intensity")]
        [Tooltip("Set the intensity for screen shake effects")]
        [DefaultValue(100)]
        [Slider]
        [Increment(4)]
        [Range(0, 400)]
        public int ScreenShakeIntensity;

        [Label("Ink-Black Sepia Dye Screen Effect")]
        [Tooltip("Use an old movie screen effect when equipping Ink-Black Sepia Dye")]
        [DefaultValue(true)]
        public bool OldMovieEffect;
    }
}
