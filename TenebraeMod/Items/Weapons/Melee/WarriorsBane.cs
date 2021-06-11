﻿using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TenebraeMod.Items.Weapons.Melee
{
	public class WarriorsBane : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("g"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			DisplayName.SetDefault("Warrior's Bane");
			Tooltip.SetDefault("'A blade made of pure, refined electricity, forged by yours truly.'\n[ Unobtainable Item ]");
		}

		public override void SetDefaults()
		{
			item.damage = 50000;
			item.melee = true;
			item.width = 46;
			item.height = 50;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2;
			item.value = 66112214;
			item.rare = ItemRarityID.Expert;
			item.UseSound = SoundID.Item71;
			item.autoReuse = true;
			item.shootSpeed = 0.8f;
			item.shoot = mod.ProjectileType("WarriorsBaneSlash");
		}
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Buffs.WarriorsAnimosity>(), 216000, true);
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			damage = target.lifeMax;
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/WarriorsBane_glowmask");
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}