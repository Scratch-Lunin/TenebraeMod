using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TenebraeMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class BlindGlasses : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blind Man's Glasses");
            Tooltip.SetDefault("Provides immunity to Stoned and Blindness");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 12;
            item.value = 30000;
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[156] = true;
            player.buffImmune[22] = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PocketMirror);
            recipe.AddIngredient(ItemID.Blindfold);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class AnkhChange : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == (1612 | 1613))
            {
                player.buffImmune[156] = true;
                player.buffImmune[47] = true;
            }
        }
    }
}