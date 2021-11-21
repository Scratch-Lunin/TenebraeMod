using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles.Ranger
{
	public class CursedCarbineBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Carbine Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.noDropItem = true;
			aiType = ProjectileID.WoodenArrowFriendly;
		}
        public override void AI()
        {
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 / 2;
			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 75);
				dust.noGravity = true;
				dust.scale = 1.2f;
			}
		}
		public void Explode()
		{
			Main.PlaySound(SoundID.Item14, projectile.position);
			for (int i = 0; i < 80; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75, 0f, 0f, 100, default(Color), 3f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 4f;
				dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75, 0f, 0f, 100, default(Color), 2f);
				Main.dust[dustIndex].velocity *= 2f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.CursedInferno, 240);
			Explode();
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.CursedInferno, 240);
			Explode();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Explode();
			return true;
		}
	}
}