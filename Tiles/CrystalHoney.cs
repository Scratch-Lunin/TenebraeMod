using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Tiles
{
	public class CrystalHoney : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
			minPick = 1;
			dustType = 153;
			drop = mod.ItemType("CrystalHoney");
			AddMapEntry(new Color(255, 214, 59));
			// Set other values here
		}
	}
}