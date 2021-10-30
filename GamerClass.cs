using GamerClass.Items.Accessories.Misc;
using GamerClass.Items.Placeable;
using GamerClass.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace GamerClass
{
    public partial class GamerClass : Mod
    {
        internal RamUsageBar RamUsageBar;
        internal UserInterface RamInterface;

        public override void Load()
        {
            ApplyPatches();

            if (!Main.dedServ)
            {
                RamUsageBar = new RamUsageBar();
                RamInterface = new UserInterface();
                RamInterface.SetState(RamUsageBar);

                AddMusicBox(
                    GetSoundSlot(SoundType.Music, "Sounds/Music/Megalovania"),
                    ModContent.ItemType<SansMusicBox>(),
                    ModContent.TileType<Tiles.SansMusicBox>());
                AddMusicBox(
                    GetSoundSlot(SoundType.Music, "Sounds/Music/GerudoValley"),
                    ModContent.ItemType<ZeldaMusicBox>(),
                    ModContent.TileType<Tiles.ZeldaMusicBox>());
                AddMusicBox(
                    GetSoundSlot(SoundType.Music, "Sounds/Music/MaidensCapriccio"),
                    ModContent.ItemType<TouhouMusicBox>(),
                    ModContent.TileType<Tiles.TouhouMusicBox>());
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ && RamUsageBar != null)
            {
                RamUsageBar.Unload();
                RamUsageBar = null;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);

            recipe.AddIngredient(ModContent.ItemType<GamerEmblem>());
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.SoulofFright, 5);

            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(ItemID.AvengerEmblem);

            recipe.AddRecipe();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            RamInterface?.Update(gameTime);
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
                        RamInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}