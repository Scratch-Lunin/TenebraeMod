using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Microsoft.Xna.Framework;


namespace TenebraeMod.Items.Weapons.Melee
{
	public class SlimySlicer : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("g"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Fires slimy spikes");
		}

		public override void SetDefaults()
		{
			item.damage = 8;
			item.melee = true;
			item.width = 40;
			item.height = 42;
			item.useTime = 60;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2;
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("SlimeSpike");
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)

		{
			for (int i = 0; i < 5; i++) //replace 3 with however many projectiles you like

			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40)); /*12 is the spread in degrees, 
				although like with Set Spread it's technically a 24 degree spread due to the fact that it's randomly between 12 degrees above and 12 degrees below your cursor.*/
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI); //create the projectile
			}
			Main.PlaySound(SoundID.Item17, player.Center);
			return false;
		}
	}

	class SlimySlicerDrops : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{
			if (npc.type == NPCID.KingSlime)
			{
				if (Main.rand.NextBool(3) && !Main.expertMode)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SlimySlicer"), 1);
				}
			}
		}
	}

	class SlimySlicerDropsBossBag : GlobalItem
	{
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag")
			{
				if (arg == ItemID.KingSlimeBossBag)
				{
					if (Main.rand.NextBool(2))
					{
						player.QuickSpawnItem(ItemType<SlimySlicer>());
					}
				}
			}
		}
	}
}