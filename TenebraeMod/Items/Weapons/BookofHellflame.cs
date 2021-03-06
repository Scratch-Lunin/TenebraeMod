using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons
{
    public class BookofHellflame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Book of Hellflame");
            Tooltip.SetDefault("Casts a barrage of flamebursts");
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.magic = true;
            item.width = 20;
            item.height = 20;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 1;
            item.value = 10000;
            item.rare = ItemRarityID.LightRed;
            item.mana = 15;
            item.shoot = mod.ProjectileType("HellflameBurst");
            item.shootSpeed = 5f;
            item.UseSound = SoundID.Item20;
            item.noMelee = true;
            item.autoReuse = true;



        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 1 + Main.rand.Next(3);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(80));
                position = player.Center + perturbedSpeed.SafeNormalize(Vector2.Zero) * 158;

                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 60);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}