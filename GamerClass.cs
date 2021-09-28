using System;
using System.Runtime.CompilerServices;
using MonoMod.Cil;
using Terraria;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace GamerClass
{
    public class GamerClass : Mod
    {
        public override void Load()
        {
            IL.Terraria.NPC.NPCLoot += NPC_NPCLoot;
            IL.Terraria.Player.OpenBossBag += Player_OpenBossBag;
        }

        private void NPC_NPCLoot(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdtoken(out _),
                i => i.MatchCall(typeof(RuntimeHelpers).GetMethod("InitializeArray", new Type[] { typeof(Array), typeof(RuntimeFieldHandle) })),
                i => i.MatchCall(out _),
                i => i.MatchStloc(56)))
            {
                Logger.Error("NPC_NPCLoot IL patch could not apply");
                return;
            } else
            {
                c.Index--;
                c.EmitDelegate<Func<int, int>>(itemId =>
                {
                    return 1;
                });
            }
        }

        private void Player_OpenBossBag(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdtoken(out _),
                i => i.MatchCall(typeof(RuntimeHelpers).GetMethod("InitializeArray", new Type[] { typeof(Array), typeof(RuntimeFieldHandle) })),
                i => i.MatchCall(out _),
                i => i.MatchStloc(11)))
            {
                Logger.Error("Player_OpenBossBag IL patch could not apply");
                return;
            }
            else
            {
                c.Index--;
                c.EmitDelegate<Func<int, int>>(itemId =>
                {
                    return 1;
                });
            }
        }
    }
}