using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace GamerClass.Items
{
    public class GamerPlayer : ModPlayer
    {
		public int availableRam;
        public float gamerDamageMult;
        public float gamerKnockback;
		public float gamerUseTimeMult;
		public int gamerCrit;

		public override void ResetEffects()
		{
			ResetVariables();
		}

		public override void UpdateDead()
		{
			ResetVariables();
		}

		private void ResetVariables()
		{
			availableRam = 0;
			gamerDamageMult = 1f;
			gamerKnockback = 0f;
			gamerUseTimeMult = 1f;
			gamerCrit = 0;
		}
    }
}
