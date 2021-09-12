using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Tiles
{
	public class AsteriteChunk : ModItem
	{
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("'Glimmers with heavenly energy'");
        }
        public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
			item.createTile = mod.TileType("AsteriteChunk");
			item.width = 18;
			item.height = 16;
			item.value = 1000;
		}
	}
}