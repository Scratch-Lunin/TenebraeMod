using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TenebraeMod.Items.Accessories
{
    public class ClingersEye : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Clinger's Eye");
            Tooltip.SetDefault("Provides immunity to Cursed Inferno");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 34;
            item.value = 10000;
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[39] = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CursedFlame, 15);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.Nazar);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}