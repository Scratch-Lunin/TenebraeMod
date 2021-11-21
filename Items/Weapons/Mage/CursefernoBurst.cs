using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Mage
{
    public class CursefernoBurst : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Curseferno Burst");
            Tooltip.SetDefault("Casts a cursed explosion which releases shards at the cursor");
            Item.staff[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.damage = 54;
            item.magic = true;
            item.mana = 10;
            item.width = 52;
            item.height = 54;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 6;
            item.value = Item.sellPrice(silver: 25);
            item.rare = ItemRarityID.Pink;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 0.01f; //Literally almost no visible effect ingame; solely here so the staff points to where you click.
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.whoAmI == Main.myPlayer) //This *should* always be true, but a good habit to check anyway
            {
                Vector2 vectorCursor = Main.MouseWorld;
                Projectile.NewProjectile(vectorCursor.X, vectorCursor.Y, speedX, speedY, mod.ProjectileType("CursedBurst"), damage, knockBack, player.whoAmI); //Outdated method ([Obsolete]), see ModContent.ProjectileType<T>()
                for (int i = 0; i < Main.rand.Next(3, 6); i++) //2-3 times
                {
                    Vector2 vel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 2.5f; //Modify 11.4f to change speed
                    Main.projectile[Projectile.NewProjectile(Main.MouseWorld, vel, ModContent.ProjectileType<CursefernoShard>(), damage / 2, 0f, player.whoAmI)].frame = Main.rand.Next(3);
                    //Create projectile, find projectile at returned index, and set its frame to between 0 and 3 (inclusive, exclusive)
                }
            }
            return false;
        }
    }
    public class CursefernoShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 100;
        }
        public override void AI()
        {
            projectile.velocity.Y += .25f; //Change .25f to modify gravity strength
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2; // projectile sprite faces up
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 75);
                dust.noGravity = true;
                dust.scale = 1.1f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 60);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            projectile.Kill();
            return false;
        }
    }
}