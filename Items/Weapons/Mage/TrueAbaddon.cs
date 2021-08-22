using Microsoft.Xna.Framework;
using TenebraeMod.Projectiles.Mage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Mage
{
    public class TrueAbaddon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Devil's Resuscitation");
            Tooltip.SetDefault("Fires a devil sycthe that explodes into knives" +
                "\n<right> to fire either an ichor or cursed scythe with special abilities" +
                "\nCursed scythes leave a path of bouncing fireballs" +
                "\nIchor scythes explode into ichor tentacles");
        }

        public override void SetDefaults()
        {
            item.damage = 69; //im not the type of guy to balance shit
            item.magic = true;
            item.noMelee = true;
            item.mana = 5;
            item.width = 40;
            item.height = 40;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 6;
            item.value = 10000;
            item.rare = ItemRarityID.Yellow; //idk
            item.UseSound = SoundID.Item73;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TrueAbaddonScythe");
            item.shootSpeed = 9f;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<TrueAbaddonScytheAlt>(); damage = (int)(item.damage * .8);
            }
            else
            {
                type = ModContent.ProjectileType<TrueAbaddonScythe>();
                speedX *= 1.5f;
                speedY *= 1.5f;
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Abaddon>());
            recipe.AddIngredient(ItemID.CursedFlames);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Abaddon>());
            recipe.AddIngredient(ItemID.GoldenShower);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
