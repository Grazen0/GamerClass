using GamerClass.Items.Accessories.Misc;
using GamerClass.Items.Dyes;
using GamerClass.Items.Placeable;
using GamerClass.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace GamerClass
{
    public partial class GamerClass : Mod
    {
        public static Effect TrailEffect;
        public static ModHotKey HaloShieldHotKey;

        internal RamUsageBar RamUsageBar;
        internal UserInterface RamInterface;

        public override void Load()
        {
            ApplyPatches();

            HaloShieldHotKey = RegisterHotKey("Military Space Armor Shield", "F");

            if (!Main.dedServ)
            {
                RamUsageBar = new RamUsageBar();
                RamInterface = new UserInterface();
                RamInterface.SetState(RamUsageBar);

                Ref<Effect> glasses3dRef = new Ref<Effect>(GetEffect("Effects/Glasses3D"));
                Ref<Effect> oldMovieRef = new Ref<Effect>(GetEffect("Effects/OldMovie"));
                Ref<Effect> haloShieldRef = new Ref<Effect>(GetEffect("Effects/HaloShield"));
                Ref<Effect> inkDyeRef = new Ref<Effect>(GetEffect("Effects/InkDye"));

                Filters.Scene["GamerClass:Glasses3D"] = new Filter(new ScreenShaderData(glasses3dRef, "Main"), EffectPriority.Low);
                Filters.Scene["GamerClass:OldMovie"] = new Filter(new ScreenShaderData(oldMovieRef, "Main"), EffectPriority.Medium);

                GameShaders.Armor.BindShader(ModContent.ItemType<HaloShieldDye>(), new ArmorShaderData(haloShieldRef, "Main")).UseColor(new Color(255, 200, 48));
                GameShaders.Armor.BindShader(ModContent.ItemType<BendyDye>(), new ArmorShaderData(inkDyeRef, "Main")).UseColor(new Color(240, 160, 100)).UseImage("Images/Misc/Noise");

                TrailEffect = GetEffect("Effects/TrailShader");

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
            if (!Main.dedServ)
            {
                TrailEffect = null;

                RamUsageBar?.Unload();
                RamUsageBar = null;
            }

            HaloShieldHotKey = null;
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

        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => "Any Evil Scale", new int[] { ItemID.ShadowScale, ItemID.TissueSample });
            RecipeGroup.RegisterGroup("GamerClass:EvilScale", group);
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
