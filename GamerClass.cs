using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MonoMod.Cil;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.ModLoader;
using GamerClass.UI;

namespace GamerClass
{
    public class GamerClass : Mod
    {
        internal RamUsageBar RamUsageBar;
        private UserInterface _ramUsageBar;

        public override void Load()
        {
            IL.Terraria.NPC.NPCLoot += NPC_NPCLoot;
            IL.Terraria.Player.OpenBossBag += Player_OpenBossBag;
            IL.Terraria.Player.Update += Player_Update;

            RamUsageBar = new RamUsageBar();
            RamUsageBar.Activate();

            _ramUsageBar = new UserInterface();
            _ramUsageBar.SetState(RamUsageBar);
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
            }

            c.Index--;
            c.EmitDelegate<Func<int, int>>(itemId =>
            {
                return 1;
            });
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

            c.Index--;
            c.EmitDelegate<Func<int, int>>(itemId =>
            {
                return 1;
            });
        }

        private void Player_Update(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.Before,
                i => i.MatchLdsfld(typeof(SoundID).GetField("Item32")),
                i => i.MatchLdarg(0),
                i => i.MatchLdfld(typeof(Entity).GetField("position")),
                i => i.MatchCall(typeof(Main).GetMethod("PlaySound", new Type[] { typeof(LegacySoundStyle), typeof(Vector2) })),
                i => i.MatchPop()))
            {
                Logger.Error("Player_Update IL patch could not apply");
                return;
            }

            c.Index++;

            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<LegacySoundStyle, Player, LegacySoundStyle>>((sound, player) =>
            {
                ModItem raccoonLeaf = ModContent.GetModItem(ModContent.ItemType<Items.Accessories.Misc.RaccoonLeaf>());
                if (player.wings == raccoonLeaf.item.wingSlot && ModContent.GetInstance<GamerConfig>().RaccoonFlySound)
                {
                    sound = GetLegacySoundSlot(Terraria.ModLoader.SoundType.Item, "Sounds/Item/RaccoonFly");
                }

                return sound;
            });
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _ramUsageBar.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarsIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarsIndex != -1)
            {
                layers.Insert(resourceBarsIndex, new LegacyGameInterfaceLayer(
                    "GamerClass: RAM Usage",
                    delegate
                    {
                        _ramUsageBar.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}