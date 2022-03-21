using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TenebraeMod.Items.Weapons.Ranger
{
    public class Sightseeker : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots homing mechanical arrows" + "\nSpazmarrows leave a trail of cursed flames" + "\nRetinarrows shoot lasers on contact");
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.ranged = true;
            item.width = 28;
            item.height = 44;
            item.useTime = 25;
            item.useAnimation = 25;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 10000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 12f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = Main.rand.NextBool() ? ProjectileType<Retinarrow>() : ProjectileType<Spazmarrow>();
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Eyebow>());
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.SoulofSight, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    internal class Retinarrow : ModProjectile
    {
        private NPC target;
        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.aiStyle = -1;
            projectile.width = 14;
            projectile.height = 14;
            drawOriginOffsetY = 20;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2;

            if (target == null || !target.active || !target.chaseable || target.dontTakeDamage)
            {
                float distance = 900f;
                projectile.friendly = false;
                int targetID = -1;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable)
                    {
                        Vector2 newMove = Main.npc[k].Center - Main.MouseWorld;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            targetID = k;
                            distance = distanceTo;
                            projectile.friendly = true;
                        }
                    }
                }
                if (!projectile.friendly)
                {
                    target = null;
                }
                else
                {
                    target = Main.npc[targetID];
                }
                projectile.friendly = true;
            }

            if (target != null)
            {
                float dTheta = (target.Center - projectile.Center).ToRotation() - projectile.velocity.ToRotation();
                if (dTheta > Math.PI)
                {
                    dTheta -= 2 * (float)Math.PI;
                }
                else if (dTheta < -Math.PI)
                {
                    dTheta += 2 * (float)Math.PI;
                }
                if (Math.Abs(dTheta) > 0.02f)
                {
                    dTheta = (dTheta > 0) ? 0.05f : -0.05f;
                }
                projectile.velocity = projectile.velocity.RotatedBy(dTheta);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            //release laser projectiles
            for (int i = 0; i < 4; i++)
            {
                int shot = Projectile.NewProjectile(projectile.Center, new Vector2(32, 0).RotatedByRandom(Math.PI * 2), ProjectileID.MiniRetinaLaser, projectile.damage, projectile.knockBack, projectile.owner);
                Main.projectile[shot].minion = false;
                Main.projectile[shot].ranged = true;
            }
        }
    }

    internal class Spazmarrow : ModProjectile
    {
        private NPC target;
        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.aiStyle = -1;
            projectile.width = 14;
            projectile.height = 14;
            drawOriginOffsetY = 20;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2;

            if (target == null || !target.active || !target.chaseable || target.dontTakeDamage)
            {
                float distance = 900f;
                projectile.friendly = false;
                int targetID = -1;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable)
                    {
                        Vector2 newMove = Main.npc[k].Center - Main.MouseWorld;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            targetID = k;
                            distance = distanceTo;
                            projectile.friendly = true;
                        }
                    }
                }
                if (!projectile.friendly)
                {
                    target = null;
                }
                else
                {
                    target = Main.npc[targetID];
                }
                projectile.friendly = true;
            }

            if (target != null)
            {
                float dTheta = (target.Center - projectile.Center).ToRotation() - projectile.velocity.ToRotation();
                if (dTheta > Math.PI)
                {
                    dTheta -= 2 * (float)Math.PI;
                }
                else if (dTheta < -Math.PI)
                {
                    dTheta += 2 * (float)Math.PI;
                }
                if (Math.Abs(dTheta) > 0.02f)
                {
                    dTheta = (dTheta > 0) ? 0.05f : -0.05f;
                }
                projectile.velocity = projectile.velocity.RotatedBy(dTheta);
            }

            //release cursed flames around six times a second
            if (Main.rand.NextBool(10) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ProjectileType<CursedTrail>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            return true;
        }
    }
    internal class CursedTrail : ModProjectile
    {
        public override string Texture => "TenebraeMod/Items/Weapons/Ranger/CursedTrail";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Trail");
        }

        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.aiStyle = -1;
            projectile.width = 14;
            projectile.height = 14;
            projectile.penetrate = 4;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 300;
            projectile.hide = true;
            projectile.light = 0.8f;
        }

        public override void AI()
        {
            Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, Scale: 1.5f)];
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
        }
    }
}