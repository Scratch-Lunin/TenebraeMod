
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TenebraeMod.Items.Weapons.Melee
{
    public class SpectreObliterator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Obliterator"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Creates a distructive array of homing souls to disintegrate your enemies to ash" + "\nHowever, you can't contain the weapons power causing you to take damage whenever you use it");
        }

        public override void SetDefaults()
        {
            item.damage = 90;
            item.melee = true;
            item.width = 74;
            item.height = 74;
            item.useTime = 50;
            item.useAnimation = 50;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4;
            item.value = 300000;
            item.rare = ItemRarityID.Lime;
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 85f;
            item.shoot = mod.ProjectileType("SpectreHammerWave");
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool())
            {
                player.AddBuff(mod.BuffType("Shadowflame"), 60);
            }
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)

        {
            float numberProjectiles = 10;
            float rotation = MathHelper.ToRadians(45);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("SkullSmasher"));
            recipe.AddIngredient(ItemID.Ectoplasm, 25);
            recipe.AddIngredient(ItemID.SpectreStaff, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}