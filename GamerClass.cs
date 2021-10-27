using GamerClass.Items.Accessories.Misc;
using GamerClass.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace GamerClass
{
    public class GamerClass : Mod
    {
        public static GamerClass Instance { get; private set; }

        internal RamUsageBar RamUsageBar;
        internal UserInterface RamInterface;

        public override void Load()
        {
            Instance = this;

            GamerPatches.Apply();

            if (!Main.dedServ)
            {
                RamUsageBar = new RamUsageBar();
                RamInterface = new UserInterface();
                RamInterface.SetState(RamUsageBar);

                AddMusicBox(
                    GetSoundSlot(SoundType.Music, "Sounds/Music/Megalovania"),
                    ItemType("SansMusicBox"),
                    TileType("SansMusicBox"));
            }
        }

        public override void Unload()
        {
            if (RamUsageBar != null)
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