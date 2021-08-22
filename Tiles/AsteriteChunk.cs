using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Tiles
{
	public class AsteriteChunk : ModTile
	{
		public override void SetDefaults()
		{
			minPick = 1;
			dustType = 145;
			soundType = SoundID.Tink;
			soundStyle = 1;
			drop = mod.ItemType("AsteriteChunk");
			AddMapEntry(new Color(253, 93, 141));
			minPick = 55;

			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileValue[Type] = 270; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
		}
	}
}