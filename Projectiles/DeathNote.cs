using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles
{
    public class DeathNote : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.melee = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 1;
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (target.lifeMax / 1 + (target.defense / 2));
            crit = true;
        }
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), (int)(projectile.width * .75), (int)(projectile.height * .75), 66, 0f, -3f, 100, default(Color), .9f);
            Main.dust[dustIndex].noGravity = true;

            dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), (int)(projectile.width * 1.5), (int)(projectile.height * 1.5), 240, 0f, 0f, 100, default(Color), 1f);
            Main.dust[dustIndex].noGravity = true;
        }
    }
}