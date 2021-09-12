using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TenebraeMod.Tiles
{
	public class TarredSoil : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMerge[Type][mod.TileType("HardenedTar")] = true;
			dustType = 22;
			drop = mod.ItemType("TarredSoil");
			AddMapEntry(new Color(68, 52, 40));
			// Set other values here
		}
	}
}