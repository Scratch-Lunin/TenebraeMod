using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenebraeMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Items.Weapons.Mage
{
    public class StarburstScepter : ModItem, IDrawPlayerGlowmask
    {
        private static readonly Color _lightColor = new Color(244, 179, 12);

        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;

            DisplayName.SetDefault("Starburst Scepter");
            Tooltip.SetDefault("...");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 34;

            item.magic = true;
            item.damage = 666;
            item.crit = 0;
            item.knockBack = 4f;
            item.mana = 1;
            item.rare = -1;
            item.value = Item.sellPrice(99, 99, 99, 99);

            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = SoundID.Item45;

            item.noMelee = true;
            item.autoReuse = true;

            item.shoot = ModContent.ProjectileType<Projectiles.Mage.StarburstStar>();
            item.shootSpeed = 15f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            var projs = Main.projectile.ToList().FindAll(proj => proj.active && proj.type == item.shoot && proj.owner == player.whoAmI && (proj.modProjectile as Projectiles.Mage.StarburstStar).IsReadyToAttack);
            if (projs.Count() > 0)
            {
                SpawnDustCircle(Main.MouseWorld, 16, 25, 62, (dust) =>
                {
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                    dust.noLight = true;
                });

                (projs[Main.rand.Next(projs.Count())].modProjectile as Projectiles.Mage.StarburstStar).SetMousePosition(Main.MouseWorld);
            }
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return Main.projectile.Count(proj => proj.active && proj.type == item.shoot && proj.owner == player.whoAmI && (proj.modProjectile as Projectiles.Mage.StarburstStar).IsReadyToAttack) > 0;
        }

        public override void HoldStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[item.shoot] < 3)
            {
                for (int i = player.ownedProjectileCounts[item.shoot]; i < 3; i++)
                {
                    Projectile.NewProjectile(player.Center + Vector2.One * 30 * i, Vector2.Zero, item.shoot, item.damage, item.knockBack, player.whoAmI, ai0: i);
                }
            }
        }

        public override void UseStyle(Player player)
        {
            Lighting.AddLight(player.itemLocation, _lightColor.ToVector3() * 0.3f * Main.essScale);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, _lightColor.ToVector3() * 0.3f * Main.essScale);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetTexture(this.Texture + "_Glowmask");
            Vector2 offset = new Vector2(0, 2);
            var drawPosition = item.position - Main.screenPosition + item.Size * 0.5f + offset;
            var origin = item.Size * 0.5f;

            void DrawGlowmask(Vector2 pos, Color color)
            {
                spriteBatch.Draw(texture, drawPosition + pos, null, color * Main.essScale, rotation, origin, scale, SpriteEffects.None, 0f);
            }

            float value = (float)Math.Cos(Main.GlobalTime);
            DrawGlowmask(Vector2.One.RotatedBy(Main.GlobalTime) * value, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            DrawGlowmask(Vector2.One.RotatedBy(Main.GlobalTime + MathHelper.Pi) * value, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            DrawGlowmask(Vector2.Zero, new Color(1f, 1f, 1f, 1f));
        }

        public void DrawPlayerGlowmask(PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Item heldItem = drawPlayer.inventory[drawPlayer.selectedItem];

            if (drawInfo.shadow != 0f || drawPlayer.frozen || ((drawPlayer.itemAnimation <= 0 || heldItem.useStyle == 0) &&
               (heldItem.holdStyle <= 0 || drawPlayer.pulley)) || heldItem.type <= ItemID.None || drawPlayer.dead || heldItem.noUseGraphic || (drawPlayer.wet && heldItem.noWet)) return;

            Texture2D texture = ModContent.GetTexture(this.Texture + "_Glowmask");
            Vector2 vector = drawInfo.itemLocation;
            SpriteEffects effect;

            if (drawPlayer.gravDir == 1f)
            {
                if (drawPlayer.direction == 1) effect = SpriteEffects.None;
                else effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                if (drawPlayer.direction == 1) effect = SpriteEffects.FlipVertically;
                else effect = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
            }

            var drawPosition = new Vector2((float)((int)(vector.X - Main.screenPosition.X)), (float)((int)(vector.Y - Main.screenPosition.Y)));
            var rectangle = new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height));
            var origin = new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * drawPlayer.direction, (drawPlayer.gravDir == -1f) ? 0 : texture.Height);

            void DrawGlowmask(Vector2 pos, Color color)
            {
                var drawData = new DrawData(texture, drawPosition + pos, rectangle, drawPlayer.GetImmuneAlpha(heldItem.GetAlpha(new Color(color.R, color.G, color.B, heldItem.alpha * color.A)), 0), drawPlayer.itemRotation, origin, heldItem.scale, effect, 0);
                Main.playerDrawData.Add(drawData);
            }

            float value = (float)Math.Cos(Main.GlobalTime);
            DrawGlowmask(Vector2.One.RotatedBy(Main.GlobalTime) * value, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            DrawGlowmask(Vector2.One.RotatedBy(Main.GlobalTime + MathHelper.Pi) * value, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            DrawGlowmask(Vector2.Zero, new Color(1f, 1f, 1f, 1f));
        }

        public static void SpawnDustCircle(Vector2 center, float radius, int count, int type, Action<Dust> onSpawn = null)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 position = center + new Vector2(radius, 0).RotatedBy(i / (float)count * MathHelper.TwoPi);
                var dust = Dust.NewDustPerfect(position, type);
                onSpawn?.Invoke(dust);
            }
        }
    }
}
