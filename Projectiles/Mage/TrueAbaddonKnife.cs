using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles.Mage
{
    public class TrueAbaddonKnife : ModProjectile
    {
        private NPC target;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            Main.projFrames[projectile.type] = 4;
            DisplayName.SetDefault("Devil's Knife");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.melee = true;
            aiType = 18;
            projectile.frame = Main.rand.Next(0, Main.projFrames[projectile.type]);
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, /*R*/ 206f / 255, /*G*/ 93f / 255, /*B*/ 207f / 255);
            projectile.velocity.Y += 0.5f;
            projectile.rotation = projectile.velocity.ToRotation();

            if (target == null || !target.active || !target.chaseable || target.dontTakeDamage)
            {
                float distance = 400f;
                projectile.friendly = false;
                int targetID = -1;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable)
                    {
                        Vector2 newMove = Main.npc[k].Center - Main.MouseWorld;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            targetID = k;
                            distance = distanceTo;
                            projectile.friendly = true;
                        }
                    }
                }
                if (!projectile.friendly)
                {
                    target = null;
                }
                else
                {
                    target = Main.npc[targetID];
                }
                projectile.friendly = true;
            }

            if (target != null)
            {
                float dTheta = (target.Center - projectile.Center).ToRotation() - projectile.velocity.ToRotation();
                if (dTheta > Math.PI)
                {
                    dTheta -= 2 * (float)Math.PI;
                }
                else if (dTheta < -Math.PI)
                {
                    dTheta += 2 * (float)Math.PI;
                }
                if (Math.Abs(dTheta) > 0.01f)
                {
                    dTheta = (dTheta > 0) ? 0.03f : -0.03f;
                }
                projectile.velocity = projectile.velocity.RotatedBy(dTheta);
            }

            //scratch how do i make slight homing
            //add fx
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

        }
    }
}
