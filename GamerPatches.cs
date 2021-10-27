using GamerClass.Items.Accessories.Misc;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GamerClass
{
    public class GamerPatches
    {
        public static void Apply()
        {
            IL.Terraria.NPC.NPCLoot += NPC_NPCLoot;
            IL.Terraria.Player.OpenBossBag += Player_OpenBossBag;
            IL.Terraria.Player.Update += Player_Update;
        }

        private static void NPC_NPCLoot(ILContext il)
        {
            var c = new ILCursor(il);

            // WoF gamer emblem drop (Normal mode)
            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdcI4(489),
                i => i.MatchLdloc(50),
                i => i.MatchAdd(),
                i => i.MatchStloc(50)))
            {
                GamerClass.Instance.Logger.Error("NPC_NPCLoot IL patch could not apply (2)");
            }
            else
            {
                c.Index--;
                c.EmitDelegate<Func<int, int>>(ChooseGamerEmblem);
            }

            // Moon Lord iron pickaxe drop (will be useful later)
            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdtoken(out _),
                i => i.MatchCall(typeof(RuntimeHelpers).GetMethod("InitializeArray", new Type[] { typeof(Array), typeof(RuntimeFieldHandle) })),
                i => i.MatchCall(out _),
                i => i.MatchStloc(56)))
            {
                GamerClass.Instance.Logger.Error("NPC_NPCLoot IL patch could not apply (3)");
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

        private static void Player_OpenBossBag(ILContext il)
        {
            var c = new ILCursor(il);

            // WoF gamer emblem drop (boss bag)
            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdcI4(489),
                i => i.MatchLdloc(10),
                i => i.MatchAdd(),
                i => i.MatchStloc(10)))
            {
                GamerClass.Instance.Logger.Error("Player_OpenBossBag IL patch could not apply (1)");
            }
            else
            {
                c.Index--;
                c.EmitDelegate<Func<int, int>>(ChooseGamerEmblem);
            }


            // Moon Lord iron pickaxe drop (boss bag) (real)
            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdtoken(out _),
                i => i.MatchCall(typeof(RuntimeHelpers).GetMethod("InitializeArray", new Type[] { typeof(Array), typeof(RuntimeFieldHandle) })),
                i => i.MatchCall(out _),
                i => i.MatchStloc(11)))
            {
                GamerClass.Instance.Logger.Error("Player_OpenBossBag IL patch could not apply (2)");
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

        private static void Player_Update(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.Before,
                i => i.MatchLdsfld(typeof(SoundID).GetField("Item32")),
                i => i.MatchLdarg(0),
                i => i.MatchLdfld(typeof(Entity).GetField("position")),
                i => i.MatchCall(typeof(Main).GetMethod("PlaySound", new Type[] { typeof(LegacySoundStyle), typeof(Vector2) })),
                i => i.MatchPop()))
            {
                GamerClass.Instance.Logger.Error("Player_Update IL patch could not apply");
                return;
            }

            c.Index++;

            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<LegacySoundStyle, Player, LegacySoundStyle>>((sound, player) =>
            {
                ModItem raccoonLeaf = ModContent.GetModItem(ModContent.ItemType<RaccoonLeaf>());
                if (player.wings == raccoonLeaf.item.wingSlot && ModContent.GetInstance<GamerConfig>().RaccoonFlySound)
                {
                    sound = GamerClass.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Item, "Sounds/Item/RaccoonFly");
                }

                return sound;
            });
        }

        private static int ChooseGamerEmblem(int itemId)
        {
            if (Main.rand.NextBool(5))
                itemId = ModContent.ItemType<GamerEmblem>();

            return itemId;
        }
    }
}
