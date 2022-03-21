using System;
using Microsoft.Xna.Framework;
using TenebraeMod.Projectiles.Summoner;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Summoner
{
    /*
	 * This file contains all the code necessary for a minion
	 * - ModItem
	 *     the weapon which you use to summon the minion with
	 * - ModBuff
	 *     the icon you can click on to despawn the minion
	 * - ModProjectile 
	 *     the minion itself
	 *     
	 * It is not recommended to put all these classes in the same file. For demonstrations sake they are all compacted together so you get a better overwiew.
	 * To get a better understanding of how everything works together, and how to code minion AI, read the guide: https://github.com/tModLoader/tModLoader/wiki/Basic-Minion-Guide
	 * This is NOT an in-depth guide to advanced minion AI
	 */

    public class LifeElementalBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Life Elemental");
            Description.SetDefault("A life elemental will heal you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<LifeElementalMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }

    public class LifeElementalStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Life Elemental Staff");
            Tooltip.SetDefault("Summons life elemental to heal you");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 5;
            item.knockBack = 3f;
            item.mana = 10;
            item.width = 24;
            item.height = 36;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 30, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item44;

            // These below are needed for a minion weapon
            item.noMelee = true;
            item.summon = true;
            item.buffType = ModContent.BuffType<LifeElementalBuff>();
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            item.shoot = ModContent.ProjectileType<LifeElementalMinion>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(item.buffType, 2);

            // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
            position = Main.MouseWorld;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LifeCrystal);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    /*
	 * This minion shows a few mandatory things that make it behave properly. 
	 * Its attack pattern is simple: If an enemy is in range of 43 tiles, it will fly to it and deal contact damage
	 * If the player targets a certain NPC with right-click, it will fly through tiles to it
	 * If it isn't attacking, it will float near the player with minimal movement
	 */

    public class LifeElementalHeal : ModProjectile
    {
        public sealed override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.hostile = true;
            projectile.damage = 1;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            #region Find target
            // Starting search distance
            float distanceFromTarget = 700f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (Main.player[i].active && Main.player[i].statLifeMax2 * 2f / 3f > Main.player[i].statLife && !Main.player[i].dead)
                    {
                        float between = Vector2.Distance(player.Center, projectile.Center);
                        bool inRange = between < distanceFromTarget;

                        if ((inRange) || !foundTarget)
                        {
                            distanceFromTarget = between;
                            targetCenter = player.Center;
                            foundTarget = true;
                        }
                    }
                }
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (Main.player[i].active && !Main.player[i].dead)
                    {
                        float between = Vector2.Distance(player.Center, projectile.Center);
                        bool inRange = between < distanceFromTarget;
                        bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;

                        if ((closest && inRange) || !foundTarget)
                        {
                            distanceFromTarget = between;
                            targetCenter = player.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
            #endregion

            #region Movement

            // Default movement parameters (here for attacking)
            float speed = 8f;
            float inertia = 20;

            if (foundTarget)
            {
                // Minion has a target: attack (here, fly towards the enemy)
                if (Vector2.Distance(projectile.Center, targetCenter) > 40f)
                {
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    Vector2 direction = targetCenter - projectile.Center;
                    direction.Normalize();
                    direction *= speed;
                    projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                if (Main.player[k].active && projectile.Hitbox.Intersects(Main.player[k].Hitbox) && !Main.player[k].dead)
                {
                    projectile.Kill();
                    Main.player[k].HealEffect(5);
                    Main.player[k].statLife += 5;
                }
            }
            Dust.NewDustPerfect(new Vector2(projectile.position.X, projectile.position.Y), 235, new Vector2(0f, 0f), 0, default, 1.5f);
            #endregion
        }
    }
}