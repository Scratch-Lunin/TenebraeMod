using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace TenebraeMod.Items.Weapons.Mage
{
    public class Abaddon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abaddon");
            Tooltip.SetDefault("Summons blades from below the cursor");
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.magic = true;
            item.mana = 15;
            item.width = 30;
            item.height = 42;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 10000;
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ProjectileType<AbaddonSword>();
            item.shootSpeed = 16f;
        }

        public override bool UseItem(Player player)
        {
            player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(3,6); i++)
            {
                position = Main.MouseWorld + new Vector2(0, Main.rand.NextFloat(500, 700)).RotatedByRandom(0.2f);
                Vector2 speed = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * item.shootSpeed * Main.rand.NextFloat(0.9f, 1.1f);
                Projectile.NewProjectile(position, speed, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ShadeSphere>());
            recipe.AddIngredient(ItemID.WaterBolt);
            recipe.AddIngredient(ItemID.DemonScythe);
            recipe.AddIngredient(ItemID.JungleSpores, 15);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Bloodbane>());
            recipe.AddIngredient(ItemID.WaterBolt);
            recipe.AddIngredient(ItemID.DemonScythe);
            recipe.AddIngredient(ItemID.JungleSpores, 15);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class AbaddonSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abaddon Blade");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            projectile.width = 10;
            projectile.height = 10;
            drawOffsetX = -44;
            drawOriginOffsetY = -8;
            drawOriginOffsetX = 22;

            projectile.alpha = 0;
            projectile.timeLeft = 360;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;
                projectile.frame = Main.rand.Next(4);
                projectile.netUpdate = true;
            }

            Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, Scale: 0.75f)].noGravity = true;

            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, projectile.velocity.X, projectile.velocity.Y, Scale: 1f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float trailLength = 10f;
            for (int i = (int)trailLength - 1; i >= 0; i--)
            {
                spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - 5 * projectile.velocity * (i / trailLength) - Main.screenPosition, new Rectangle(0, projectile.frame* Main.projectileTexture[projectile.type].Height/4, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height/4), Color.White * (1 - i / trailLength), projectile.rotation, new Vector2(49, 13), projectile.scale * (1 - i / trailLength), SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}