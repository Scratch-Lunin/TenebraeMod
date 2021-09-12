using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]

    public class VileAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vile Amulet");
            Tooltip.SetDefault("Summons a ring of cursed fireballs when hit" +
                "\nThese fireballs auto-aim to enemies you attack" +
                "\nProvides immunity to Cursed Inferno");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 42;
            item.value = 10000;
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[39] = true;
            var tenebraePlayer = player.GetModPlayer<TenebraeModPlayer>();
            tenebraePlayer.VileAmulet = true;
        }
    }
    public class VileAmuletFireball : ModProjectile
    {
        public double angleTimer = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 60*5;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.position, new Vector3(0f, 1f, 0f)); //the Vector3 will be the color in rgb values, the vector2 will be your projectile's position
            Lighting.maxX = 100; //height 
            Lighting.maxY = 100; //width
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
            projectile.rotation = (float)(angleTimer + MathHelper.PiOver2 / 2); // projectile sprite faces up

            angleTimer += 0.08;
            if (angleTimer > 360)
            {
                angleTimer = 0;
            }

            int radius = 50;
            projectile.Center = Main.player[projectile.owner].Center + Vector2.One.RotatedBy(angleTimer) * radius;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 60);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color mainColor = Color.White;

            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = mainColor * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                float scale = projectile.scale * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                spriteBatch.Draw(texture, projectile.Center - projectile.position + projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), color, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), mainColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}