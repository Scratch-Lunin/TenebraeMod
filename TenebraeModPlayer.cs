using TenebraeMod.Buffs;
using TenebraeMod.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace TenebraeMod
{
    public class TenebraeModPlayer : ModPlayer
    {
        public int InpuratusDeathShake;
        public int DashShakeTimer;

        static void Method(PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Vector2 Position = drawInfo.position;
            float shadow = drawInfo.shadow;
            float num44 = drawPlayer.stealth;
            SpriteEffects spriteEffects = drawInfo.spriteEffects;
            Color color12 = drawInfo.middleArmorColor;
            DrawData value;
            int shader12 = drawInfo.wingShader;
            if ((drawPlayer.velocity.Y != 0f || drawPlayer.grappling[0] != -1) && !drawPlayer.mount.Active)
            {
                //Main.LoadItemFlames(1866); //unaccessible
                Microsoft.Xna.Framework.Color color20 = color12;
                int num63 = 24;
                int num64 = 0;
                if (shadow == 0f && drawPlayer.grappling[0] == -1)
                {
                    for (int m = 0; m < 7; m++)
                    {
                        Microsoft.Xna.Framework.Color color21 = new Microsoft.Xna.Framework.Color(250 - m * 10, 250 - m * 10, 250 - m * 10, 150 - m * 10);
                        Vector2 value5 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
                        num44 *= num44;
                        num44 *= 1f - shadow;
                        color21 = new Microsoft.Xna.Framework.Color((int)((float)(int)color21.R * num44), (int)((float)(int)color21.G * num44), (int)((float)(int)color21.B * num44), (int)((float)(int)color21.A * num44));
                        value5.X = drawPlayer.itemFlamePos[m].X;
                        value5.Y = 0f - drawPlayer.itemFlamePos[m].Y;
                        value5 *= 0.5f;
                        value = new DrawData(Main.itemFlameTexture[1866], new Vector2((int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) - (float)(9 * drawPlayer.direction)) + num64 * drawPlayer.direction, (int)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) + 2f * drawPlayer.gravDir + (float)num63 * drawPlayer.gravDir)) + value5, new Microsoft.Xna.Framework.Rectangle(0, Main.wingsTexture[drawPlayer.wings].Height / 7 * drawPlayer.wingFrame, Main.wingsTexture[drawPlayer.wings].Width, Main.wingsTexture[drawPlayer.wings].Height / 7 - 2), color21, drawPlayer.bodyRotation, new Vector2(Main.wingsTexture[drawPlayer.wings].Width / 2, Main.wingsTexture[drawPlayer.wings].Height / 14), 1f, spriteEffects, 0);
                        value.shader = shader12;
                        Main.playerDrawData.Add(value);
                    }
                }

                value = new DrawData(Main.wingsTexture[drawPlayer.wings], new Vector2((int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) - (float)(9 * drawPlayer.direction)) + num64 * drawPlayer.direction, (int)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) + 2f * drawPlayer.gravDir + (float)num63 * drawPlayer.gravDir)), new Microsoft.Xna.Framework.Rectangle(0, Main.wingsTexture[drawPlayer.wings].Height / 7 * drawPlayer.wingFrame, Main.wingsTexture[drawPlayer.wings].Width, Main.wingsTexture[drawPlayer.wings].Height / 7), color20, drawPlayer.bodyRotation, new Vector2(Main.wingsTexture[drawPlayer.wings].Width / 2, Main.wingsTexture[drawPlayer.wings].Height / 14), 1f, spriteEffects, 0);
                value.shader = shader12;
                Main.playerDrawData.Add(value);
            }

        }

        public override void ModifyScreenPosition()
        {
            if (TenebraeModWorld.InpuratusDies == true)
            {
                InpuratusDeathShake++;
                float intensity = 10f;
                if (InpuratusDeathShake >= 1)
                {
                    Main.screenPosition += new Vector2(Main.rand.NextFloat(intensity), Main.rand.NextFloat(intensity));
                    Main.screenPosition -= new Vector2(Main.rand.NextFloat(intensity), Main.rand.NextFloat(intensity));
                    intensity *= 0.9f;
                    if (InpuratusDeathShake == 30)
                    {
                        TenebraeModWorld.InpuratusDies = false;
                        InpuratusDeathShake = 0;
                    }
                }
            }

            if (TenebraeModWorld.DashShake == true)
            {
                DashShakeTimer++;
                float intensity = 3f;
                if (DashShakeTimer >= 1)
                {
                    Main.screenPosition += new Vector2(Main.rand.NextFloat(intensity), Main.rand.NextFloat(intensity));
                    Main.screenPosition -= new Vector2(Main.rand.NextFloat(intensity), Main.rand.NextFloat(intensity));
                    intensity *= 0.9f;
                    if (DashShakeTimer == 15)
                    {
                        TenebraeModWorld.DashShake = false;
                        DashShakeTimer = 0;
                    }
                }
            }
        }

        public bool holyflames;
        public bool warriordebuff;
        int holydamage = 0;

        public override void ResetEffects() {
			holyflames = false;
            warriordebuff = false;
		}

        public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
        {
            if (player.HasBuff(mod.BuffType("HolyRetribution")) && (item.type == ItemID.Excalibur || item.type == ItemID.TrueExcalibur))
                add += 0.2f;
        }

        public override float UseTimeMultiplier(Item item)
        {
            if (player.HasBuff(mod.BuffType("HolyRetribution")) && (item.type == ItemID.Excalibur || item.type == ItemID.TrueExcalibur))
            {
                return 1.2f;
            }
            return base.UseTimeMultiplier(item);
        }

        public override void UpdateBadLifeRegen() {
			if (holyflames) {
				if (holydamage >= player.statLifeMax2) { 
					holydamage = player.statLifeMax2; // Line 30 divides this by 16 and the game will divide that by 2.
				}
            	if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
                player.lifeRegenTime = 0;
				player.lifeRegen -= (int)Math.Ceiling((float)holydamage / 16);
				holydamage++;
			}
			else {
				holydamage = 0;
			}
            if (warriordebuff)
            {
                if (player.statLife < 10)
                {
                    if (player.lifeRegen > 0)
                    {
                        player.lifeRegen = 0;
                    }
                    player.lifeRegen -= player.statLife * 10;
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8 && player.HasBuff(ModContent.BuffType<WarriorsAnimosity>()) )
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name +   "'s soul was claimed by the Warrior");
            }
            return true;
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			if (holyflames) {
				if (Main.rand.NextBool(3) && drawInfo.shadow == 0f) {
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, ModContent.DustType<HolyflameDust>(), player.velocity.Y * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 5f);
					Main.dust[dust].velocity.Y += 0.2f;
                    Main.playerDrawDust.Add(dust);
				}
                r *= 0.7f;
				g *= 0.65f;
				b *= 0.3f;
				fullBright = true;
			}
            if (warriordebuff)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 271, 0f, 0f, 100, new Color(255, 0, 0), 1f);
                    Main.dust[dust].noGravity = true;
                    dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 270, 0f, 0f, 100, new Color(255, 0, 0), .8f);
                    Main.dust[dust].noGravity = true;
                }
            }
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            // Drawing in front of items
            {
                void ItemGlowmaskLayer(PlayerDrawInfo drawInfo)
                {
                    if (drawInfo.drawPlayer.HeldItem.modItem is Interfaces.IDrawPlayerGlowmask item) item.DrawPlayerGlowmask(drawInfo);
                }
                var index = layers.IndexOf(PlayerLayer.HeldItem);
                if (index >= 0) layers.Insert(index + 1, new PlayerLayer(nameof(TenebraeMod), "HeldItemGlowmask", ItemGlowmaskLayer));
            }
        }
    }
}