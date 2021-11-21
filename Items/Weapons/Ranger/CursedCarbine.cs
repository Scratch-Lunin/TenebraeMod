using Microsoft.Xna.Framework;
using TenebraeMod.Projectiles.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Ranger
{
    public class CursedCarbine : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Converts musket balls into cursed bullets" + "\nHas a chance to fire an exploding corrupt crystal" + "\n20% not to consume ammo");
        }

        public override void SetDefaults()
        {
            item.damage = 40; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            item.ranged = true; // sets the damage type to ranged
            item.width = 64; // hitbox width of the item
            item.height = 30; // hitbox height of the item
            item.scale = 0.8f;
            item.useTime = 10; // The item's use time in ticks (60 ticks == 1 second.)
            item.useAnimation = 10; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            item.useStyle = ItemUseStyleID.HoldingOut; // how you use the item (swinging, holding out, etc)
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 4; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            Item.sellPrice(0, 5, 50, 0);
            item.rare = ItemRarityID.Pink; // the color that the item's name will be in-game
            item.UseSound = SoundID.Item11; // The sound that this item plays when used.
            item.autoReuse = true; // if you can hold click to automatically use it again
            item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 4f; // the speed of the projectile (measured in pixels per frame)
            item.useAmmo = AmmoID.Bullet; // The "ammo Id" of the ammo item that this weapon uses. Note that this is not an item Id, but just a magic value.
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 1);
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .20f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.Bullet) // or ProjectileID.WoodenArrowFriendly
            {
                type = ProjectileID.CursedBullet;
            }
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (Main.rand.NextBool(5))
            {
                // Here we manually spawn the 2nd projectile, manually specifying the projectile type that we wish to shoot.
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<CursedCarbineBullet>(), damage, knockBack, player.whoAmI);
                // By returning true, the vanilla behavior will take place, which will shoot the 1st projectile, the one determined by the ammo. 
            }
            return true;
        }
    }
}