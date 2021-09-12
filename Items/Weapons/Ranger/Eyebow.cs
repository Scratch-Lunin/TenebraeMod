using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TenebraeMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TenebraeMod.Items.Weapons.Ranger
{
    public class Eyebow : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots eye arrows" +
                "\n<right> to shoot fang arrows that pierce" +
                "\nFang arrows inflict enemies, causing eye arrows to chase them");
        }

        public override void SetDefaults()
        {
            item.damage = 16;
            item.ranged = true;
            item.width = 24;
            item.height = 36;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 2000;
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 5f;
            item.useAmmo = AmmoID.Arrow;
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
                type = ProjectileType<EyeFangArrow>();
                speedY *= 0.65f;
            }
            else
            {
                type = ProjectileType<EyeArrow>(); damage = (int)(item.damage * .5);
            }
            return true;
        }
    }

    internal class EyeArrow : ModProjectile
    {
        private NPC target;
        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.aiStyle = 1;
            projectile.width = 14;
            projectile.height = 14;
            drawOriginOffsetY = 20;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.tileCollide = true;
        }
        public override void AI()
        {
            if (Main.rand.Next(4) == 0)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, 0f, 0f, 0, default(Color), 1f);
            }
            projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2;

            if (target == null || !target.active || !target.chaseable || target.dontTakeDamage)
            {
                float distance = 400f;
                projectile.friendly = false;
                int targetID = -1;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable && Main.npc[k].HasBuff(BuffType<Sighted>()))
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
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            return true;
        }
    }

    internal class EyeFangArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fanged Eye Arrow");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.aiStyle = 1;
            projectile.width = 14;
            projectile.height = 14;
            drawOriginOffsetY = 20;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            if (Main.rand.Next(4) == 0)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, 0f, 0f, 0, default(Color), 1f);
            }
            projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Sighted>(), 60*5, true);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage + (10 / 2));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color mainColor = lightColor;

            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = mainColor * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float rotation;
                if (k + 1 >= projectile.oldPos.Length)
                {
                    rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
                }
                else
                {
                    rotation = (projectile.oldPos[k] - projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
                }

                spriteBatch.Draw(texture, projectile.Center - projectile.position + projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.frame * texture.Height / Main.projFrames[projectile.type], texture.Width, texture.Height / Main.projFrames[projectile.type]), mainColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }

    class EyebowDrops : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (Main.rand.NextBool(3) && !Main.expertMode)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Eyebow"), 1);
                }
            }
        }
    }

    class EyebowDropsBossBag : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag")
            {
                if (arg == ItemID.EyeOfCthulhuBossBag)
                {
                    if (Main.rand.NextBool(2))
                    {
                        player.QuickSpawnItem(ItemType<Eyebow>());
                    }
                }
            }
        }
    }
}
