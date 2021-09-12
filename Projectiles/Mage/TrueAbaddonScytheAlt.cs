using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles.Mage
{
    public class TrueAbaddonScytheAlt : ModProjectile
    {
        public float scytheRotation = 0.25f;
        public int fireballTimer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            DisplayName.SetDefault("True Devil's Scythe");
        }

        public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 66;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.timeLeft = 120; //dunno if tis' balanced
            projectile.melee = true;
            projectile.frame = Main.rand.Next(0, Main.projFrames[projectile.type]);
        }
        public override void AI()
        {
            //scract put what you want
            scytheRotation *= 0.98f;
            projectile.rotation += scytheRotation * (float)projectile.direction;
            projectile.velocity *= 0.98f; // 0.99f for rolling grenade speed reduction. Try values between 0.9f and 0.99f

            if (projectile.frame == 0)
            {
                Lighting.AddLight(projectile.Center, /*R*/ 255f / 255, /*G*/ 248f / 255, /*B*/ 52f / 255);
            }

            if (projectile.frame == 1)
            {
                Lighting.AddLight(projectile.Center, /*R*/ 181f / 255, /*G*/ 247f / 255, /*B*/ 12f / 255);
                if (fireballTimer < 70)
                {
                    fireballTimer += 1;
                    if (fireballTimer % 8 == 0)
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.Zero, ProjectileID.CursedFlameFriendly, 35, 0f, Main.myPlayer);
                    }
                }
            }
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
            Main.PlaySound(SoundID.Item, projectile.Center, 104);
            Main.PlaySound(SoundID.Item14, projectile.Center);
            if (projectile.frame == 0)
            {
                int amount3 = 6;
                for (int i = 0; i < amount3; ++i)
                {
                    float currentRotation = (MathHelper.TwoPi / amount3) * i;
                    Vector2 projectileVelocity = currentRotation.ToRotationVector2() * 6;
                    float speed = 20f;
                    Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2));
                    int damage = 69;
                    int type = ProjectileID.GoldenShowerFriendly; //dunno what you mean by ichor tentacles
                    Projectile.NewProjectile(projectile.Center, projectileVelocity, type, damage, 0, Main.myPlayer);
                    //add effects
                }
            }
            if (projectile.frame == 1)
            {
                for (int i = 0; i < 70; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 4f;
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[dustIndex].velocity *= 2f;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(SoundID.Item, projectile.Center, 104);
            Main.PlaySound(SoundID.Item14, projectile.Center);
            if (projectile.frame == 0)
            {
                target.AddBuff(BuffID.Ichor, 120);
                //add fx
            }
            if (projectile.frame == 1)
            {
                target.AddBuff(BuffID.CursedInferno, 120);
                //add fx
            }
        }

    }
}
