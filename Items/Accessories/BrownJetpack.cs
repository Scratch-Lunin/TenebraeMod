using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TenebraeMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class BrownJetpack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brown's Space Booster");
			Tooltip.SetDefault("'Great for impersonating modded devs!'" + "\n'A blast from the past... and the future!'");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 42;
			Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
		}
		//these wings use the same values as the solar wings
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 70;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 9f;
			acceleration *= 2.5f;
			if (player.controlDown && player.controlJump && player.wingTime > 0f)
			{
				player.accRunSpeed = 10f;
				player.runAcceleration *= 10f;
			}
			else
			{
				player.accRunSpeed = 6.25f;
			}
		}
        public override bool WingUpdate(Player player, bool inUse)
        {
			player.wingFrameCounter++;
			if (inUse && player.wingFrameCounter > 4)
			{
				player.wingFrame++;
				player.wingFrameCounter = 0;
				if (player.wingFrame >= 3)
                {
					player.wingFrame = 0;
				}
			}
			else if (!player.controlJump || player.velocity.Y == 0f)
			{
				player.wingFrame = 0;
			}
			return true;
		}
    }
}