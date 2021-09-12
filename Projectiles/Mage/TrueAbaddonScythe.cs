using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace TenebraeMod.Projectiles.Mage
{
    public class TrueAbaddonScythe : ModProjectile
    {
        public float scytheRotation = 0.25f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            DisplayName.SetDefault("Devil's Scythe");
        }

        public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 54;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 120;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.melee = true;
        }
        public override void AI()
        {
            scytheRotation *= 0.98f;
            projectile.rotation += scytheRotation * (float)projectile.direction;
            projectile.velocity *= 0.98f; // 0.99f for rolling grenade speed reduction. Try values between 0.9f and 0.99f

            Lighting.AddLight(projectile.Center, /*R*/ 206f / 255, /*G*/ 93f / 255, /*B*/ 207f / 255);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color mainColor = lightColor;

            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = mainColor * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                spriteBatch.Draw(texture, projectile.Center - projectile.position + projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), color, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), mainColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item104, projectile.Center);
            Main.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < 40; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 4f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, 0f, 0f, 100, default(Color), 1f);
                Main.dust[dustIndex].velocity *= 2f;
            }
            int amount3 = 6;
            for (int i = 0; i < amount3; ++i)
            {
                float currentRotation = (MathHelper.TwoPi / amount3) * i;
                Vector2 projectileVelocity = currentRotation.ToRotationVector2() * 6;
                float speed = 40f;
                Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2));
                int damage = (int)(projectile.damage * 0.8);
                int type = mod.ProjectileType("TrueAbaddonKnife");
                Projectile.NewProjectile(projectile.Center, projectileVelocity * 2, type, damage, 0, Main.myPlayer);
                //add effects
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //scrathcput anythign her
            //maybe put shadowflame debuff?
            projectile.Kill();
        }

    }
}
