using TenebraeMod.Projectiles.Mage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Mage
{
    public class ShadeSphere : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shade Sphere");
            Tooltip.SetDefault("Casts a floating shade ball that explodes");
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.width = 32;
            item.width = 32;
            item.useTime = 80;
            item.useAnimation = 80;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.UseSound = SoundID.Item104;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Green;
            item.mana = 16;
            item.magic = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<ShadeBall>();
            item.shootSpeed = 8f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}