using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Tiles
{
	public class AsteriteIngot : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Forged with the essence of starlight'");
			ItemID.Sets.SortingPriorityMaterials[item.type] = 65; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 99;
			item.value = 4000;
			item.rare = ItemRarityID.Blue;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("AsteriteIngot");
			item.placeStyle = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AsteriteChunk>(), 3);
			recipe.AddIngredient(ItemID.FallenStar);
			recipe.AddTile(TileID.SkyMill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetTexture(this.Texture + "_Glowmask");
            var drawPosition = item.position - Main.screenPosition + item.Size * 0.2f;
            var origin = item.Size * 0.5f;

            void DrawGlowmask(Vector2 pos, Color color)
            {
                spriteBatch.Draw(texture, drawPosition + pos, null, color * Main.essScale, rotation, origin, scale, SpriteEffects.None, 0f);
            }

            float value = (float)Math.Cos(Main.GlobalTime);
            DrawGlowmask(Vector2.One.RotatedBy(Main.GlobalTime) * value, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            DrawGlowmask(Vector2.One.RotatedBy(Main.GlobalTime + MathHelper.Pi) * value, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            DrawGlowmask(Vector2.Zero, new Color(1f, 1f, 1f, 1f));
        }
    }
}