using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace GamerClass.Sounds.Item
{
    public class TouhouStickHit : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            if (soundInstance.State == SoundState.Playing)
                soundInstance.Stop();

            soundInstance = sound.CreateInstance();
            soundInstance.Volume = 0.2f;
            soundInstance.Pan = pan;
            
            return soundInstance;
        }
    }
}
