using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles.Mage
{
    public class CursedBurst : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
            DisplayName.SetDefault("Cursed Burst");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 66;
            projectile.scale = 1f;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.magic = true;
            projectile.light = 0.45f;
            projectile.timeLeft = 30;
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

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.0f, 0.25f, 0.0525f);
            int dust = Dust.NewDust(projectile.position, projectile.width + 20, projectile.height + 20, 75);
            Main.dust[dust].velocity /= 20f;
            Main.dust[dust].scale = 0.8f;

            if (projectile.ai[0] == 0)
            {
                Main.PlaySound(SoundID.Item14, (int)projectile.position.X, (int)projectile.position.Y);
                projectile.ai[0] = 1;
            }

            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 60 * 5);
		}
    }
}
