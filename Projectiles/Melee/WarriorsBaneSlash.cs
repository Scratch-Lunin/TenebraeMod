using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TenebraeMod.Projectiles.Melee
{
	public class WarriorsBaneSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warrior's Bane Slash");
			Main.projFrames[projectile.type] = 5;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 50;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
			projectile.penetrate = 9999;
		}

		public override void AI()
        {
			projectile.rotation = projectile.velocity.ToRotation(); // projectile faces sprite right
			projectile.velocity *= 1.02f;
			// Loop through the 4 animation frames, spending 5 ticks on each.
			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}

			//add lighting
			Lighting.AddLight(projectile.position, new Vector3(1f, 0f, 0f)); //the Vector3 will be the color in rgb values, the vector2 will be your projectile's position
			Lighting.maxX = 400; //height 
			Lighting.maxY = 400; //width
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage = target.lifeMax;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{ // TODO: Add OnHitPlayer method for PVP?
			target.AddBuff(ModContent.BuffType<Buffs.WarriorsAnimosity>(), 600, true);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) //trail effect
		{
			Texture2D texture = Main.projectileTexture[projectile.type];

			Color mainColor = lightColor;

			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = mainColor * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float rotation;
				if (k + 1 >= projectile.oldPos.Length)
				{
					rotation = projectile.velocity.ToRotation();
				}
				else
				{
					rotation = (projectile.oldPos[k] - projectile.oldPos[k + 1]).ToRotation();
				}

				spriteBatch.Draw(texture, projectile.Center - projectile.position + projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), mainColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}