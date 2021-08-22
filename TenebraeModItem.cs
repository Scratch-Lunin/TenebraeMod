using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TenebraeMod
{
    public class TenebraeModItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Wood", 5);
            recipe.AddRecipeGroup("TenebraeMod:GoldBar", 3);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.GoldChest);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TenebraeMod:DemoniteBrick", 8);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(ItemID.ShadowChest);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Wood", 5);
            recipe.AddIngredient(ItemID.JungleSpores, 2);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.IvyChest);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 8);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.WaterChest);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chest);
            recipe.AddIngredient(ItemID.Cobweb, 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.WebCoveredChest);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chest, 5);
            recipe.AddIngredient(ItemID.SnowBlock, 25);
            recipe.AddIngredient(ItemID.IceBlock, 25);
            recipe.AddIngredient(ItemID.TempleKey);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.FrozenChest, 5);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chest, 5);
            recipe.AddIngredient(ItemID.MudBlock, 25);
            recipe.AddIngredient(ItemID.JungleGrassSeeds, 25);
            recipe.AddIngredient(ItemID.TempleKey);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.JungleChest, 5);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chest, 5);
            recipe.AddIngredient(ItemID.PearlstoneBlock, 50);
            recipe.AddIngredient(ItemID.TempleKey);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.HallowedChest, 5);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chest, 5);
            recipe.AddIngredient(ItemID.EbonstoneBlock, 50);
            recipe.AddIngredient(ItemID.TempleKey);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.CorruptionChest, 5);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chest, 5);
            recipe.AddIngredient(ItemID.CrimstoneBlock, 50);
            recipe.AddIngredient(ItemID.TempleKey);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.CrimsonChest, 5);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Frog, 5);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(ItemID.FrogLeg);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 50);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.CloudinaBottle);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 25);
            recipe.AddIngredient(ItemID.SnowBlock, 25);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.BlizzardinaBottle);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 25);
            recipe.AddIngredient(ItemID.SandBlock, 25);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.SandstorminaBottle);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 20);
            recipe.AddIngredient(ItemID.WhiteString, 2);
            recipe.AddIngredient(ItemID.Feather, 10);
            recipe.AddTile(TileID.SkyMill);
            recipe.SetResult(ItemID.ShinyRedBalloon);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OldShoe);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient(ItemID.Feather, 10);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(ItemID.HermesBoots);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OldShoe);
            recipe.AddIngredient(ItemID.Coral, 5);
            recipe.AddIngredient(ItemID.WaterWalkingPotion, 5);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(ItemID.WaterWalkingBoots);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OldShoe);
            recipe.AddIngredient(ItemID.IceBlock, 25);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(ItemID.IceSkates);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.Aglet);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TatteredBand");
            recipe.AddIngredient(ItemID.JungleSpores, 10);
            recipe.AddIngredient(ItemID.Cloud, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.AnkletoftheWind);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.OldShoe);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TatteredBand");
            recipe.AddIngredient(ItemID.RegenerationPotion, 3);
            recipe.AddIngredient(ItemID.LifeCrystal, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.BandofRegeneration);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TatteredBand");
            recipe.AddIngredient(ItemID.ManaRegenerationPotion, 3);
            recipe.AddIngredient(ItemID.ManaCrystal, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.BandofStarpower);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Vine, 10);
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(ItemID.FeralClaws);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FeralClaws);
            recipe.AddIngredient(ItemID.Spike, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(ItemID.ClimbingClaws);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OldShoe);
            recipe.AddIngredient(ItemID.Spike, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(ItemID.ShoeSpikes);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OldShoe);
            recipe.AddIngredient(ItemID.DayBloomPlanterBox, 25);
            recipe.AddIngredient(ItemID.JungleGrassSeeds, 10);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(ItemID.FlowerBoots);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TenebraeMod:GoldBar", 3);
            recipe.AddIngredient(ItemID.Cloud, 50);
            recipe.AddIngredient(ItemID.Feather, 5);
            recipe.AddTile(TileID.SkyMill);
            recipe.SetResult(ItemID.LuckyHorseshoe);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Paper", 3);
            recipe.AddIngredient(ItemID.Leather);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(ItemID.Book, 3);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(ItemID.WaterCandle, 3);
            recipe.AddIngredient(ItemID.Bone, 20);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(ItemID.WaterBolt);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(ItemID.LivingDemonFireBlock, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(ItemID.DemonScythe);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(ItemID.CelestialMagnet);
            recipe.AddIngredient(ItemID.Ectoplasm, 15);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(ItemID.MagnetSphere);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "WornCloth", 15);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddIngredient(ItemID.DarkShard, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.Blindfold);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "WornCloth", 10);
            recipe.AddIngredient(ItemID.SandBlock, 50);
            recipe.AddIngredient(ItemID.Sandstone, 25);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(ItemID.FlyingCarpet);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TatteredBand", 1);
            recipe.AddIngredient(ItemID.LavaBucket, 3);
            recipe.AddIngredient(ItemID.AshBlock, 25);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(ItemID.LavaCharm);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod); // New Ankh Shield recipe with Blind Man's Glasses and Hand Warmer
            recipe.AddIngredient(ItemID.ArmorBracing);
            recipe.AddIngredient(ItemID.MedicatedBandage);
            recipe.AddIngredient(ItemID.ThePlan);
            recipe.AddIngredient(ItemID.CountercurseMantra);
            recipe.AddIngredient(ItemID.HandWarmer);
            recipe.AddIngredient(null, "BlindGlasses");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(ItemID.AnkhCharm);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoldyHerosTome");
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(ItemID.BrokenHeroSword);
            recipe.AddRecipe();

            RecipeFinder finder = new RecipeFinder(); // Removing vanilla Ankh Shield recipe
            finder.AddIngredient(ItemID.Blindfold);
            finder.AddIngredient(ItemID.ArmorBracing);
            finder.AddIngredient(ItemID.MedicatedBandage);
            finder.AddIngredient(ItemID.ThePlan);
            finder.AddIngredient(ItemID.CountercurseMantra);
            finder.AddTile(TileID.TinkerersWorkbench);
            finder.SetResult(ItemID.AnkhCharm);
            Recipe recipe2 = finder.FindExactRecipe();
            if (recipe2 != null)
            {
                RecipeEditor editor = new RecipeEditor(recipe2);
                editor.DeleteRecipe();
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.rare)
            {
                case 12:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "ItemName")
                        {
                            line.overrideColor = new Color(70, 48, 167);
                        }
                    }
                    break;
                case -20:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "ItemName")
                        {
                            TenebraeModWorld.timer++;
                            if (TenebraeModWorld.timer == 380)
                            {
                                TenebraeModWorld.timer = 0;
                            }
                            int r = 255;
                            int g = (int)(100 * (1 + Math.Sin(TenebraeModWorld.timer / 60f)));
                            int b = 0;
                            line.overrideColor = new Color(r, g, b);
                        }
                    }
                    break;
            }



            if (item.type == ItemID.MedicatedBandage)
            {
                foreach (TooltipLine x1 in tooltips)
                {
                    if (x1.mod == "Terraria" && x1.Name == "Tooltip0")
                    {
                        x1.text = "Immunity to Poison, Bleeding, and Feral Bite";
                    }
                }
            }

            if (item.type == ItemID.Excalibur)
            {
                TooltipLine tip = new TooltipLine(mod, "Placeable", "Critical hits buff the Exalibur's abilities");
                tooltips.Add(tip);
            }

            if (item.type == ItemID.TrueExcalibur)
            {
                TooltipLine tip = new TooltipLine(mod, "Placeable", "Critical hits buff the True Exalibur's abilities");
                tooltips.Add(tip);
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.MedicatedBandage)
            {
                player.buffImmune[BuffID.Rabies] = true;
                player.buffImmune[BuffID.Venom] = true;
            }

            if (item.type == ItemID.ArmorBracing)
            {
                player.buffImmune[BuffID.Webbed] = true;
            }

            if (item.type == ItemID.AnkhCharm)
            {
                player.buffImmune[BuffID.Rabies] = true;
                player.buffImmune[BuffID.Webbed] = true;
                player.buffImmune[BuffID.Venom] = true;
            }

            if (item.type == ItemID.AnkhShield)
            {
                player.buffImmune[BuffID.Rabies] = true;
                player.buffImmune[BuffID.Webbed] = true;
                player.buffImmune[BuffID.Venom] = true;
            }
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.NightsEdge)
            {
                item.damage = 45;
                item.autoReuse = true;
                item.useAnimation = 32;
                item.useTime = 32;
            }

            if (item.type == ItemID.TrueNightsEdge)
            {
                item.autoReuse = true;
                item.useAnimation = 28;
                item.useTime = 28;
            }

            if (item.type == ItemID.TrueExcalibur)
            {
                item.autoReuse = true;
                item.useAnimation = 20;
                item.useTime = 20;
            }
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (item.type == ItemID.NightsEdge)
            {
                target.AddBuff(BuffID.ShadowFlame, 60);
            }

            if (item.type == ItemID.TrueNightsEdge)
            {
                target.AddBuff(BuffID.ShadowFlame, 240);
            }

            if (player.HeldItem.type == ItemID.Excalibur && crit || player.HeldItem.type == ItemID.TrueExcalibur && crit)
            {
                player.AddBuff(BuffType<Buffs.HolyRetribution>(), 240, false);
            }
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (player.HasBuff(mod.BuffType("HolyRetribution")) && item.type == ItemID.Excalibur || item.type == ItemID.TrueExcalibur)
            {
                damage = (int)(damage * 1.2f);
            }
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (Main.rand.NextFloat() < 0.5f && (player.HasBuff(mod.BuffType("HolyRetribution")) && item.type == ItemID.Excalibur || (player.HasBuff(mod.BuffType("HolyRetribution")) && item.type == ItemID.TrueExcalibur)))
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                dust = Main.dust[Terraria.Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 130, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }
        }
    }
}