using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TenebraeMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TenebraeMod.Items.Weapons.Mage
{
    public class TrueDarklight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Darklight");
            Tooltip.SetDefault("Summons a stream of blades from below the cursor");
        }

        public override void SetDefaults()
        {
            item.damage = 70;
            item.magic = true;
            item.mana = 15;
            item.width = 34;
            item.height = 36;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.sellPrice(gold: 10);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ProjectileType<TrueDarklightSword>();
            item.shootSpeed = 20f;
        }

        public override bool UseItem(Player player)
        {
            player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
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
            recipe.AddIngredient(ItemType<Abaddon>());
            recipe.AddIngredient(ItemType<MoldyHerosTome>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public class TrueDarklightSword : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("True Darklight Blade");
                Main.projFrames[projectile.type] = 4;
            }

            public override void SetDefaults()
            {
                projectile.aiStyle = -1;
                projectile.width = 10;
                projectile.height = 10;
                drawOffsetX = -66;
                drawOriginOffsetY = -14;
                drawOriginOffsetX = 33;

                projectile.alpha = 0;
                projectile.timeLeft = 360;
                projectile.penetrate = -1;
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

                Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, Scale: 1f)].noGravity = true;
                Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, Scale: 1f)].noGravity = true;

                projectile.rotation = projectile.velocity.ToRotation();
            }

            public override void Kill(int timeLeft)
            {
                for (int i = 0; i < 19; i++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, projectile.velocity.X, projectile.velocity.Y, Scale: 1f);
                }
            }

            public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
            {
                float trailLength = 10f;
                for (int i = (int)trailLength - 1; i >= 0; i--)
                {
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - 10 * projectile.velocity * (i / trailLength) - Main.screenPosition, new Rectangle(0, projectile.frame * Main.projectileTexture[projectile.type].Height / 4, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4), Color.White * (1 - i / trailLength), projectile.rotation, new Vector2(71, 19), projectile.scale * (1 - i / trailLength), SpriteEffects.None, 0f);
                }
                return false;
            }

            public override bool? CanCutTiles()
            {
                return false;
            }

            public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            {
                target.AddBuff(BuffID.CursedInferno, 2 * 60);
                target.AddBuff(BuffID.ShadowFlame, 2 * 60);
            }

            public override void OnHitPvp(Player target, int damage, bool crit)
            {
                target.AddBuff(BuffID.CursedInferno, 2 * 60);
                target.AddBuff(BuffID.ShadowFlame, 2 * 60);
            }
        }
    }
}