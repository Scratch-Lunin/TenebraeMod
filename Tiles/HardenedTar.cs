using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Tiles
{
	public class HardenedTar : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMerge[Type][mod.TileType("TarredSoil")] = true;
			minPick = 1;
			dustType = 1;
			soundType = SoundID.Tink;
			drop = mod.ItemType("HardenedTar");
			AddMapEntry(new Color(79, 75, 74));
			// Set other values here
		}
	}
}