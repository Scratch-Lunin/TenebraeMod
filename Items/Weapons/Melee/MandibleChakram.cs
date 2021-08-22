using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using TenebraeMod.Projectiles;
using Terraria.ModLoader;
using TenebraeMod.Projectiles.Melee;

namespace TenebraeMod.Items.Weapons.Melee
{
    public class MandibleChakram : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mandible Chakram"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Digs through walls\nDamage and speed slightly decreased when inside a block\nSpits spiky sand upon hit");
        }

        public override void SetDefaults()
        {
            item.damage = 32;
            item.crit = 4;
            item.melee = true;
            item.width = 47;
            item.height = 45;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.useTime = 15;
            item.useAnimation = 15;
            item.knockBack = 10;
            item.value = 2000;
            item.rare = ItemRarityID.Orange;
            item.shoot = ModContent.ProjectileType<MandibleChakramProj>();
            item.shootSpeed = 10f;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;
            item.useTurn = true;
        }
		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[item.shoot] < 2;
		}
    }
}

