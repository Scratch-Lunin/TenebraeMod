using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Mage
{
    public class Bloodbane : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts a fast moving bolt of blood that ricochets off enemies");
        }
        public override void SetDefaults()
        {
            item.damage = 12;
            item.magic = true;
            item.width = 32;
            item.height = 36;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 1;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.mana = 10;
            item.shoot = mod.ProjectileType("BloodBolt");
            item.shootSpeed = 6f;
            item.UseSound = SoundID.Item21;
            item.noMelee = true;
            item.autoReuse = true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Bleeding, 60);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}