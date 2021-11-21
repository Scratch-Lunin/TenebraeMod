using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TenebraeMod.Items.Weapons.Ranger;
using TenebraeMod.Items.Weapons.Melee;
using TenebraeMod.Items.Weapons.Mage;
using TenebraeMod.Items.Armor;
using TenebraeMod.Items.Accessories;
using TenebraeMod.Items.Tiles;

namespace TenebraeMod
{
    public partial class TenebraeMod : Mod
    {
        public static TenebraeMod Instance { get; private set; }

        // As an example check Starburst Star projectile
        public static List<int> DrawCacheProjsFrontPlayers;

        public TenebraeMod() => Instance = this;

        public override void Load()
        {
            DrawCacheProjsFrontPlayers = new List<int>(200);

            On.Terraria.Main.DrawPlayers += (orig, self) =>
            {
                orig(self);

                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                for (int i = 0; i < TenebraeMod.DrawCacheProjsFrontPlayers.Count; i++)
                {
                    try
                    {
                        Main.instance.DrawProj(TenebraeMod.DrawCacheProjsFrontPlayers[i]);
                    }
                    catch (Exception e)
                    {
                        TimeLogger.DrawException(e);
                        Main.projectile[TenebraeMod.DrawCacheProjsFrontPlayers[i]].active = false;
                    }
                }
                Main.spriteBatch.End();

                TenebraeMod.DrawCacheProjsFrontPlayers.Clear();
            };
        }

        public override void Unload()
        {
            DrawCacheProjsFrontPlayers.Clear();
            DrawCacheProjsFrontPlayers = null;
        }

        public override void PostSetupContent()
        {
            Mod fargos = ModLoader.GetMod("Fargowiltas");
            if (fargos != null)
            {
                /* AddSummon, order or value in terms of vanilla bosses, your mod internal name, summon  
                item internal name, inline method for retrieving downed value, price to sell for in copper */
                fargos.Call("AddSummon", 10.5f, "TenebraeMod", "InpuratusSummonFargo", (Func<bool>)(() => TenebraeModWorld.downedInpuratus), 480000);

            }

            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call(
                    "AddBoss",
                    10.5f,
                    ModContent.NPCType<NPCs.Inpuratus.Inpuratus>(),
                    this, // Mod
                    "Inpuratus",
                    (Func<bool>)(() => TenebraeModWorld.downedInpuratus),
                    ModContent.ItemType<Items.Summoning.InpuratusSummon>(),
                    new List<int> { ModContent.ItemType<InpuratusTrophy>(), ModContent.ItemType<InpuratusMask>() },
                    new List<int> { ModContent.ItemType<VileGlaive>(), ModContent.ItemType<CursedCarbine>(), ModContent.ItemType<CursefernoBurst>(), ModContent.ItemType<VileAmulet>(),
                    ItemID.CursedFlame, ItemID.RottenChunk},
                    "Use a [i:" + ItemType("InpuratusSummon") + "] in the Underground Corruption.",
                    "Inpuratus retreats to the depths of the Corruption...",
                    "TenebraeMod/BossChecklistTextures/BossInpuratus",
                    //"TenebraeMod/NPCs/Inpuratus/Inpuratus_Head_Boss",
                    (Func<bool>)(() => !WorldGen.crimson));
                // Additional bosses here
            }
        }

        #region Recipe Groups
        public override void AddRecipeGroups()
        {
            RecipeGroup goldbar = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("TenebraeMod:GoldBar", goldbar);

            RecipeGroup demonbrick = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Demonite Brick", new int[]
            {
                ItemID.DemoniteBrick,
                ItemID.CrimtaneBrick
            });
            RecipeGroup.RegisterGroup("TenebraeMod:DemoniteBrick", demonbrick);

            RecipeGroup cobaltbar = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cobalt Bar", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            });
            RecipeGroup.RegisterGroup("TenebraeMod:CobaltBar", cobaltbar);


            RecipeGroup bluebrickwall = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Blue Brick Wall", new int[]
           {
                ItemID.BlueBrickWall,
                ItemID.BlueSlabWall,
                ItemID.BlueTiledWall
           });
            RecipeGroup.RegisterGroup("TenebraeMod:BlueBrickWall", bluebrickwall);

            RecipeGroup greenbrickwall = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Green Brick Wall", new int[]
           {
                ItemID.GreenBrickWall,
                ItemID.GreenSlabWall,
                ItemID.GreenTiledWall
           });
            RecipeGroup.RegisterGroup("TenebraeMod:GreenBrickWall", greenbrickwall);

            RecipeGroup pinkbrickwall = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Pink Brick Wall", new int[]
           {
                ItemID.PinkBrickWall,
                ItemID.PinkSlabWall,
                ItemID.PinkTiledWall
           });
            RecipeGroup.RegisterGroup("TenebraeMod:PinkBrickWall", pinkbrickwall);


            RecipeGroup unsafebluebrickwall = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Unsafe Blue Brick Wall", new int[]
           {
                ModContent.ItemType<Items.Tiles.Walls.BlueDungeonUnsafe>(),
                ModContent.ItemType<Items.Tiles.Walls.BlueDungeonSlabUnsafe>(),
                ModContent.ItemType<Items.Tiles.Walls.BlueDungeonTileUnsafe>()
           });
            RecipeGroup.RegisterGroup("TenebraeMod:UnsafeBlueBrickWall", unsafebluebrickwall);

            RecipeGroup unsafegreenbrickwall = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Unsafe Green Brick Wall", new int[]
           {
                ModContent.ItemType<Items.Tiles.Walls.GreenDungeonUnsafe>(),
                ModContent.ItemType<Items.Tiles.Walls.GreenDungeonSlabUnsafe>(),
                ModContent.ItemType<Items.Tiles.Walls.GreenDungeonTileUnsafe>()
           });
            RecipeGroup.RegisterGroup("TenebraeMod:UnsafeGreenBrickWall", unsafegreenbrickwall);

            RecipeGroup unsafepinkbrickwall = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Unsafe Pink Brick Wall", new int[]
           {
                ModContent.ItemType<Items.Tiles.Walls.PinkDungeonUnsafe>(),
                ModContent.ItemType<Items.Tiles.Walls.PinkDungeonSlabUnsafe>(),
                ModContent.ItemType<Items.Tiles.Walls.PinkDungeonTileUnsafe>()
           });
            RecipeGroup.RegisterGroup("TenebraeMod:UnsafePinkBrickWall", unsafepinkbrickwall);
        }
        #endregion
    }
}