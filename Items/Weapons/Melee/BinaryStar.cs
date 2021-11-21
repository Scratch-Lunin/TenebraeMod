using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using TenebraeMod.Projectiles.Melee;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace TenebraeMod.Items.Weapons.Melee
{
	public class BinaryStar : ModItem
	{
        private int channelTimer;
        private bool consume;

		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Binary Star");
			Tooltip.SetDefault("'Melt your foes with double the firepower!'");
		}

		public override void SetDefaults() {
			item.damage = 80;
			item.melee = true;
			item.width = 74;
			item.height = 66;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
            item.noUseGraphic = true;
			item.knockBack = 6.5f;
			item.value = 100000;
			item.rare = ItemRarityID.Red;
			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileType<BinaryStarProjectile>();
			item.shootSpeed = 15.9f;
            item.channel = true;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentSolar, 18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/Melee/BinaryStar_glowmask");
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
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int shot = Projectile.NewProjectile(player.Center,new Vector2(speedX,speedY),type,damage,knockBack,player.whoAmI,0,0);
			(Main.projectile[shot].modProjectile as BinaryStarProjectile).theta = Math.PI;
            return true;
		}
	}
}
