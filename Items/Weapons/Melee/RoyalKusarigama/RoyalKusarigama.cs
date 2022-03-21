using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Melee.RoyalKusarigama
{
	public class RoyalKusarigama : ModItem
	{
		bool alt = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Kusarigama");
			Tooltip.SetDefault("'The trusted companion of an ancient ninja, it's blade is still sharp after all these years'\nRight-click to lash out at enemies\nHold down Right-click to reel enemies hit by it's tip\nStunned enemies take 3x damage from the left-click attack");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 42;
			item.useTime = 2;
			item.useAnimation = 2;
			item.noUseGraphic = true;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.damage = 10;
			item.melee = true;
			item.rare = ItemRarityID.Green;
			item.value = 35000;
			item.shoot = ProjectileID.PurificationPowder;
			item.autoReuse = true;
		}
		public override bool CanUseItem(Player player)
		{
			if (BoolRightClick(player))
			{
				item.damage = 28;
				return player.ownedProjectileCounts[ModContent.ProjectileType<SlasherAlt>()] < 1;
			}
			else
				item.damage = 16;
			return base.CanUseItem(player);
		}
		public static void UpdatePlayerArm(Player player, Vector2 pos)
        {
			int frame = player.bodyFrame.Height;
			if (pos.Y < player.MountedCenter.Y - 75)
				player.bodyFrame.Y = frame * 2;
			if (pos.Y > player.MountedCenter.Y + 75)
				player.bodyFrame.Y = frame * 4;
			if (pos.Y > player.MountedCenter.Y + 75 && pos.Y < player.MountedCenter.Y - 75)
				player.bodyFrame.Y = frame * 3;
		}
		public override bool AltFunctionUse(Player player) => true;
        bool BoolRightClick(Player player) => player.altFunctionUse == 2;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (BoolRightClick(player))
			{
				SlasherAlt slasherAlt = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<SlasherAlt>(), item.damage, item.knockBack, player.whoAmI).modProjectile as SlasherAlt;
				slasherAlt.alt = alt = !alt;
			}
			else if (player.ownedProjectileCounts[ModContent.ProjectileType<SlasherMain>()] < 1)
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<SlasherMain>(), item.damage, item.knockBack, player.whoAmI);
			return false;
        }
    }
	public class SlasherMain : ModProjectile
	{
		Vector2 slashPos;
		int[] timer = new int[2];
		float rotation;
		const int Cloud = 16;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Kusarigama");
		}
		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.aiStyle = 0;
			projectile.scale = 1f;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.ownerHitCheck = true;
			projectile.ignoreWater = true;
			projectile.timeLeft = 2;
		}
		public override void AI()
		{
			Player p = Main.player[projectile.owner];
			if (p.channel)
				projectile.timeLeft = 2;
			RoyalKusarigama.UpdatePlayerArm(p, Main.MouseWorld);
			Vector2 targPos = new Vector2(Main.MouseWorld.X - p.MountedCenter.X, -1 * (Main.MouseWorld.Y - p.MountedCenter.Y));
			rotation = (float)Math.Atan(targPos.Y / targPos.X) + 7.825f;
			float rot = targPos.X < 0 ? -16f : 16f;
			slashPos = new Vector2((float)Math.Sin(rotation) * rot, (float)Math.Cos(rotation) * rot);
			projectile.Center = p.MountedCenter + slashPos;
			if (timer[0]++ > 3)
			{
				Vector2 dustPos = new Vector2((float)Math.Sin(rotation) * rot, (float)Math.Cos(rotation) * rot);
				Dust dust = Dust.NewDustDirect(projectile.Center + dustPos, 32, 32, Cloud);
				dust.noGravity = true;
				Vector2 motion = new Vector2(projectile.Center.X - p.MountedCenter.X, projectile.Center.Y - p.MountedCenter.Y);
				dust.velocity = motion * 0.2f;
				timer[0] = 0;
				if (timer[1]++ > 2)
				{
					Main.PlaySound(SoundID.Item1, projectile.Center);
					timer[1] = 0;
				}
			}
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (target.GetGlobalNPC<KusarigamaNPC>().stunned)
				damage *= 3;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Player p = Main.player[projectile.owner];
			Texture2D tex = ModContent.GetTexture("TenebraeMod/Items/Weapons/Melee/RoyalKusarigama/RoyalKusarigamaSlash");
			Vector2 basePos = (p.MountedCenter + (slashPos * 3f)) - Main.screenPosition;
			float rot = (float)Math.Atan2((double)((float)Main.mouseY + Main.screenPosition.Y - projectile.Center.Y), (double)((float)Main.mouseX + Main.screenPosition.X - projectile.Center.X));
			SpriteEffects effects = projectile.Center.X - p.MountedCenter.X < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
			spriteBatch.Draw(tex, basePos, new Rectangle(0, timer[1] * (tex.Height / 4), tex.Width, tex.Height / 4), lightColor, rot, new Vector2(tex.Width / 1f, tex.Height / 8), 1.5f, effects, 0);
		}
    }
	public class SlasherAlt : ModProjectile
	{
		Vector2[] targPos = new Vector2[2];
		Vector2[] slashPos = new Vector2[2];
		Vector2[] gripPos = new Vector2[2];
		float[] x = new float[3];
		float[] rotation = new float[2];
		float[] shine = new float[3];
		bool setup;
		public bool alt;
		int channel;
		int[] grapple = new int[4];
		float reel;
		//bool reeling;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Kusarigama");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 28;
			projectile.aiStyle = 0;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.ownerHitCheck = true;
			projectile.ignoreWater = true;
			projectile.timeLeft = 2;
		}
		public override void AI()
		{
			projectile.timeLeft = 2;
			Player p = Main.player[projectile.owner];
			RoyalKusarigama.UpdatePlayerArm(p, setup ? targPos[1] : Main.MouseWorld);
			p.itemTime = 2;
			p.itemAnimation = 2;
			float rot = 0;
			if (targPos[0] != Vector2.Zero)
				rot = targPos[0].X < 0 ? -0.1f : 0.1f;
			if (Main.myPlayer == projectile.owner && Main.mouseRight && !setup)
			{
				if (grapple[0] < 60 && grapple[0] != -1)
					grapple[0]++;
				if(grapple[0] >= 60)
                {
					shine[2] = x[0] / ((float)Math.PI / 40);
					shine[1] = (float)(Math.PI / (40 - shine[2]));
					shine[0] = (float)Math.PI;
					Main.PlaySound(SoundID.Item105, projectile.Center);
					grapple[0] = -1;
				}
				p.direction = Main.MouseWorld.X < p.MountedCenter.X ? -1 : 1;
				if (channel == 0)
					channel = projectile.damage;
				projectile.damage = 0;
				targPos[1] = Main.MouseWorld;
				targPos[0] = new Vector2(Main.MouseWorld.X - p.MountedCenter.X, -1 * (Main.MouseWorld.Y - p.MountedCenter.Y));
				rot = targPos[0].X < 0 ? -0.01f : 0.01f;
				rotation[0] = (float)Math.Atan(targPos[0].Y / targPos[0].X) + 7.825f;
				slashPos[0] = new Vector2((float)Math.Sin(rotation[0]) * rot, (float)Math.Cos(rotation[0]) * rot);
				gripPos[0] = new Vector2((float)Math.Sin(rotation[0]) * (rot * 1200), (float)Math.Cos(rotation[0]) * (rot * 1200));
			}
			else
            {
				setup = true;
				x[0] += (float)Math.PI / 40;
				if (grapple[2] < 1)
					projectile.damage = channel;
			}
			if(x[0] >= Math.PI / 20 && grapple[1] < 1)
            {
				Main.PlaySound(SoundID.Item1, projectile.Center);
				grapple[1] = 1;
            }
			slashPos[0] = new Vector2((float)Math.Sin(rotation[0] + rotation[1]) * rot, (float)Math.Cos(rotation[0] + rotation[1]) * rot);
			slashPos[1] = slashPos[0] * x[1];
			gripPos[1] = gripPos[0] + p.MountedCenter;
			shine[0] -= shine[1];
			if (grapple[2] < 1)
            {
				x[1] = (float)Math.Pow(Math.Abs(Math.Sin(x[0])), 2) * 3140;
				x[2] = (float)(-1 * Math.Cos(x[0]));
				rotation[1] = (alt ? -1.5f : 1.5f) * x[2];
				projectile.Center = p.MountedCenter + slashPos[1];
				#region Chain damage
				Vector2 origin = new Vector2(projectile.Center.X, projectile.Center.Y);
				Vector2 center = gripPos[1];
				Vector2 distToOrig = origin - center;
				float distance = distToOrig.Length();
				while (distance > 30f && !float.IsNaN(distance))
				{
					distToOrig.Normalize();
					distToOrig *= 30f;
					center += distToOrig;
					distToOrig = origin - center;
					distance = distToOrig.Length();
					ChainCollision collision = Projectile.NewProjectileDirect(center, Vector2.Zero, ModContent.ProjectileType<ChainCollision>(), projectile.damage, projectile.knockBack / 2, projectile.owner).modProjectile as ChainCollision;
					collision.parent = projectile;
				}
				#endregion
				if (x[0] >= Math.PI)
					projectile.Kill();
			}
			else
            {
				NPC targ = Main.npc[TargetWhoAmI];
				bool canReel = targ.active && targ != null && targ.knockBackResist > 0.25f && !targ.boss && targ.type != NPCID.TargetDummy  && grapple[3] < 1;
				Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width * 0.5f), projectile.position.Y + (projectile.height * 0.5f));
				{
					projectile.damage = 0;
					reel += (float)Math.PI / 120;
					if (reel > Math.PI / 2)
						reel = (float)Math.PI / 2;
					float speed = (-7.5f * (float)Math.Sin(reel)) * (!canReel ? 3 : targ.knockBackResist);
					float rotation = (float)Math.Atan2((vector8.Y) - (p.MountedCenter.Y + (p.height * 0.5f)), (vector8.X) - (p.MountedCenter.X + (p.width * 0.5f)));
					projectile.velocity.X = (float)(Math.Cos(rotation) * speed);
					projectile.velocity.Y = (float)(Math.Sin(rotation) * speed);
				}
				if (canReel)
					targ.Center = projectile.Center;
				if (Main.myPlayer == projectile.owner && Main.mouseRight)
					grapple[3] = 1;
				if (projectile.Hitbox.Intersects(new Rectangle((int)p.MountedCenter.X, (int)p.MountedCenter.Y, 75, 50)))
                {
					if (canReel)
                    {
						targ.StrikeNPC((int)(channel * 2.5f), projectile.knockBack, projectile.direction);
						targ.HitEffect(targ.direction);
						targ.GetGlobalNPC<KusarigamaNPC>().stunned = true;
						Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);
					}
					projectile.Kill();
				}
				/*void PerformReel()
                {
					projectile.timeLeft = 3600;
					if (p == null || !p.active)
						projectile.Kill();
					if (grapple[3] < 1)
					{
						if (LineStrength())
							Reel();
						bool LineStrength()
						{
							if (IsStickingToTarget)
								return (projectile.Center - p.MountedCenter).Length() > 750;
							return false;
						}
						grapple[3] = 0;
					}
					projectile.rotation = NewToRotation(p.MountedCenter, projectile.position) + MathHelper.ToRadians(270f);
				}
				void Reel()
				{
					grapple[3] = 1;
					aiType = 0;
					IsStickingToTarget = false;
					projectile.tileCollide = false;
					Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width * 0.5f), projectile.position.Y + (projectile.height * 0.5f));
					{
						float rotation = (float)Math.Atan2((vector8.Y) - (p.MountedCenter.Y + (p.height * 0.5f)), (vector8.X) - (p.MountedCenter.X + (p.width * 0.5f)));
						projectile.velocity.X = (float)(Math.Cos(rotation) * (-1 * targ.knockBackResist));
						projectile.velocity.Y = (float)(Math.Sin(rotation) * (-1 * targ.knockBackResist));
					}
				}
				float NewToRotation(Vector2 targ, Vector2 orig) => (float)Math.Atan2(targ.Y - orig.Y, targ.X - orig.X);*/
			}
		}
		public int TargetWhoAmI
		{
			get => (int)projectile.ai[1];
			set => projectile.ai[1] = value;
		}
		public bool IsStickingToTarget
		{
			get => projectile.ai[0] == 1f;
			set => projectile.ai[0] = value ? 1f : 0f;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = true;
			damage = (int)(damage * 1.25f);
			knockback = (int)(knockback * 1.5f);
			if (grapple[0] == -1)
            {
				projectile.ignoreWater = true;
				projectile.tileCollide = false;
				IsStickingToTarget = true;
				TargetWhoAmI = target.whoAmI;
				projectile.velocity = (target.Center - projectile.Center) * 0.75f;
				projectile.netUpdate = true;
				projectile.damage = 0;
				grapple[2] = 1;
			}
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			if(setup)
            {
				Vector2 orig = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] + orig + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					Draw(drawPos, color, true);
				}
			}
			Draw(projectile.Center, Lighting.GetColor((int)gripPos[1].X / 16, (int)gripPos[1].Y / 16), false);
			void Draw(Vector2 tipPos, Color col, bool sh)
            {
				Texture2D chainTex = ModContent.GetTexture("TenebraeMod/Items/Weapons/Melee/RoyalKusarigama/Special/RoyalKusarigamaChain");
				Texture2D gripTex = ModContent.GetTexture("TenebraeMod/Items/Weapons/Melee/RoyalKusarigama/Special/RoyalKusarigamaGrip");
				Texture2D tipTex = ModContent.GetTexture("TenebraeMod/Items/Weapons/Melee/RoyalKusarigama/Special/RoyalKusarigamaTip");
				float shineCol = (float)(shine[0] / Math.PI);
				Color shade = col;
				SpriteEffects effects = alt ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 origin = tipPos;
				Vector2 center = gripPos[1];
				Vector2 distToOrig = origin - center;
				float projRotation = distToOrig.ToRotation() - 1.57f;
				float distance = distToOrig.Length();
				spriteBatch.Draw(ModContent.GetTexture("TenebraeMod/Items/Weapons/Melee/RoyalKusarigama/Special/RoyalKusarigamaGrip"), gripPos[1] - Main.screenPosition, new Rectangle(0, 0, gripTex.Width, gripTex.Height), shade, projRotation, new Vector2(gripTex.Width / 2, gripTex.Height / 2), 1f, effects, 0f);
				while (distance > 30f && !float.IsNaN(distance))
				{
					distToOrig.Normalize();
					distToOrig *= 30f;
					center += distToOrig;
					distToOrig = origin - center;
					distance = distToOrig.Length();

					spriteBatch.Draw(ModContent.GetTexture("TenebraeMod/Items/Weapons/Melee/RoyalKusarigama/Special/RoyalKusarigamaChain"), center - Main.screenPosition,
						new Rectangle(0, 0, chainTex.Width, chainTex.Height), shade, projRotation,
						new Vector2(chainTex.Width / 2, chainTex.Height / 2), 1f, effects, 0f);

				}
				spriteBatch.Draw(tipTex, tipPos - Main.screenPosition, new Rectangle(0, 0, tipTex.Width, tipTex.Height), col, projRotation, new Vector2(tipTex.Width / 2, tipTex.Height / 2), 1, effects, 0);
				if(!sh)
					spriteBatch.Draw(ModContent.GetTexture("TenebraeMod/Items/Weapons/Melee/RoyalKusarigama/Special/RoyalKusarigamaTipShine"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, tipTex.Width, tipTex.Height), lightColor * shineCol, projRotation, new Vector2(tipTex.Width / 2, tipTex.Height / 2), 1, effects, 0);
			}
		}
	}
	public class KusarigamaNPC : GlobalNPC
    {
		public int immunity;
		public bool stunned;
		float[] starCircle = new float[2];
		int stunTimer;
		public override bool InstancePerEntity => true;
        public override bool PreAI(NPC npc)
        {
			if (immunity > 0)
				immunity--;
			if (stunned)
            {
				npc.velocity.X = 0;
				if (stunned)
				{
					if (stunTimer++ > (int)300 * npc.knockBackResist)
					{
						stunTimer = 0;
						stunned = false;
					}
				}
				return false;
			}
            return base.PreAI(npc);
        }
        public override void NPCLoot(NPC npc)
        {
			if (npc.type == NPCID.KingSlime && !Main.expertMode && Main.rand.NextBool(4))
				Item.NewItem(npc.getRect(), ModContent.ItemType<RoyalKusarigama>());
            base.NPCLoot(npc);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
			if (stunned)
			{
				starCircle[0] += starCircle[1] += (float)Math.PI / 30;
				if (starCircle[0] >= Math.PI)
					starCircle[0] = 0;
				if (starCircle[1] >= 2 * Math.PI)
					starCircle[1] = 0;
				Vector2 origin = new Vector2(npc.Center.X, npc.Center.Y - (npc.height / 2));
				Vector2[] pos = new Vector2[2] { new Vector2((origin.X - (npc.width / 2)) + (float)Math.Sin(0.5f * starCircle[1]) * npc.width, origin.Y - (2.5f * (float)Math.Sin(starCircle[0]))), new Vector2((origin.X + (npc.width / 2)) - (float)Math.Sin(0.5f * starCircle[1]) * npc.width, origin.Y + (2.5f * (float)Math.Sin(starCircle[0]))) };
				DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, Main.fontMouseText, "*", pos[0] - Main.screenPosition, Color.Yellow, 0f, Main.fontMouseText.MeasureString("*") / 2, 1f, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, Main.fontMouseText, "*", pos[1] - Main.screenPosition, Color.Yellow, 0f, Main.fontMouseText.MeasureString("*") / 2, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(npc, spriteBatch, drawColor);
        }
    }
	public class ChainCollision : ModProjectile
	{
		public Projectile parent;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Kusarigama");
		}
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.extraUpdates = 100;
			projectile.timeLeft = 60;
		}
		public override void AI()
		{
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] > 9f)
			{
				if (projectile.localAI[0] > 9f)
				{
					for (int i = 0; i < 4; i++)
					{
						projectile.position -= projectile.velocity * ((float)i * 0.25f);
						projectile.alpha = 255;
						projectile.Kill();
					}
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.GetGlobalNPC<KusarigamaNPC>().immunity = 30;
		}
        public override bool? CanHitNPC(NPC target)
        {
			if (target.GetGlobalNPC<KusarigamaNPC>().immunity > 0)
				return false;
            return base.CanHitNPC(target);
        }
        public override bool? CanCutTiles() => false;
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = oldVelocity.X * 1f;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = oldVelocity.Y * 1f;
			}
			return false;
		}
	}
	public class KusarigamaDrop : GlobalItem
    {
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			// This method shows adding items to Fishrons boss bag. 
			// Typically you'll also want to also add an item to the non-expert boss drops, that code can be found in ExampleGlobalNPC.NPCLoot. Use this and that to add drops to bosses.
			if (context == "bossBag" && arg == ItemID.KingSlimeBossBag && Main.rand.NextBool(4))
				player.QuickSpawnItem(ModContent.ItemType<RoyalKusarigama>());
		}
	}
}