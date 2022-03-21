using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TenebraeMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
            projectile.timeLeft = 60 * 20;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                projectile.ai[0] = -1;
                projectile.localAI[0] = 1;
            }

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

            if (projectile.ai[0] >= 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2; // projectile sprite faces up

                NPC target = Main.npc[(int)projectile.ai[0]];

                projectile.friendly = true;
                if (target == null || !target.active || !target.chaseable || target.dontTakeDamage) // homing code :spiderman:
                {
                    for (int k = 0; k < 200; k++)
                    {
                        float distance = 400f;
                        projectile.ai[0] = -2;
                        if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable && k == target.whoAmI && Main.npc[k].HasBuff(BuffType<FlameInflict>()))
                        {
                            Vector2 newMove = Main.npc[k].Center - projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                projectile.ai[0] = k;
                                distance = distanceTo;
                                projectile.friendly = true;
                            }
                        }
                    }
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
                    if (Math.Abs(dTheta) > 0.1f)
                    {
                        dTheta = (dTheta > 0) ? 0.1f : -0.1f;
                    }
                    projectile.velocity = Vector2.Normalize(projectile.velocity).RotatedBy(dTheta) * 5f;
                }
            }
            else if (projectile.ai[0] == -1)
            {
                projectile.friendly = false;
                projectile.ai[1] += 0.05f;
                int count = 0;
                int index = 0;
                float timer = projectile.ai[1];
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (i == projectile.whoAmI)
                    {
                        if (count == 0)
                        {
                            timer = Main.projectile[i].ai[1];
                        }
                        count++;
                        index = count;
                    }
                    else if (Main.projectile[i].active && Main.projectile[i].type == projectile.type && Main.projectile[i].owner == projectile.owner)
                    {
                        if (count == 0)
                        {
                            timer = Main.projectile[i].ai[1];
                        }
                        count++;
                    }
                }
                float rotation; // spaced out boys
                if (count == 1)
                {
                    rotation = 0f;
                }
                else
                {
                    rotation = MathHelper.TwoPi / (count - 1) * (index - 1) - MathHelper.PiOver2;
                }

                int radius = 50;
                if (timer > 360)
                {
                    timer = 0;
                }

                
                projectile.Center = Main.player[projectile.owner].Center + Vector2.One.RotatedBy(rotation + timer) * radius;
            }
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