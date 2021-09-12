using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;

namespace ShaderRarity
{
    public class CustomRarityGlobalItem : GlobalItem
    {
        public static int RarityID => -1337;

        public override void SetDefaults(Item item)
        {
            // For testing... 
            if (item.type == ItemID.TerraBlade) item.rare = CustomRarityGlobalItem.RarityID;
        }

        public override bool Autoload(ref string name)
        {
            // So, we are replacing vanilla text drawing. Therefore, if someone wants to change the drawing method in some way, they can break the current rarity.
            // But I doubt that anyone needs it.
            On.Terraria.Main.MouseText += (orig, self, cursorText, rare, diff, hackedMouseX, hackedMouseY, hackedScreenWidth, hackedScreenHeight) =>
            {
                if (rare != CustomRarityGlobalItem.RarityID) orig(self, cursorText, rare, diff, hackedMouseX, hackedMouseY, hackedScreenWidth, hackedScreenHeight);
                else
                {
                    int num = Main.mouseX + 10;
                    int num2 = Main.mouseY + 10;
                    if (hackedMouseX != -1 && hackedMouseY != -1)
                    {
                        num = hackedMouseX + 10;
                        num2 = hackedMouseY + 10;
                    }
                    if (Main.ThickMouse)
                    {
                        num += 6;
                        num2 += 6;
                    }
                    Vector2 vector = Main.fontMouseText.MeasureString(cursorText);
                    if (hackedScreenHeight != -1 && hackedScreenWidth != -1)
                    {
                        if ((float)num + vector.X + 4f > (float)hackedScreenWidth) num = (int)((float)hackedScreenWidth - vector.X - 4f);
                        if ((float)num2 + vector.Y + 4f > (float)hackedScreenHeight) num2 = (int)((float)hackedScreenHeight - vector.Y - 4f);
                    }
                    else
                    {
                        if ((float)num + vector.X + 4f > (float)Main.screenWidth) num = (int)((float)Main.screenWidth - vector.X - 4f);
                        if ((float)num2 + vector.Y + 4f > (float)Main.screenHeight) num2 = (int)((float)Main.screenHeight - vector.Y - 4f);
                    }

                    Main.spriteBatch.End();
                    Effect effect = mod.GetEffect("Effects/CustomRarity");
                    effect.Parameters["time"].SetValue(Main.GlobalTime * 15);
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, effect, Main.UIScaleMatrix);

                    Vector2 position = new Vector2((float)num, (float)num2);
                    ChatManager.DrawColorCodedString(Main.spriteBatch, Main.fontMouseText, cursorText, position + new Vector2(3.5f, 0).RotatedBy(Main.GlobalTime), Colors.AlphaDarken(new Color(250, 0, 60)) * 0.9f, 0f, Vector2.Zero, Vector2.One, -1f);
                    ChatManager.DrawColorCodedString(Main.spriteBatch, Main.fontMouseText, cursorText, position + new Vector2(3.5f, 0).RotatedBy(Main.GlobalTime + MathHelper.Pi), Colors.AlphaDarken(new Color(0, 120, 255)) * 0.9f, 0f, Vector2.Zero, Vector2.One, -1f);
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, cursorText, position, Colors.AlphaDarken(Color.White), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                    ChatManager.DrawColorCodedString(Main.spriteBatch, Main.fontMouseText, cursorText, position + new Vector2(1, 0).RotatedBy(Main.GlobalTime + MathHelper.PiOver2), Colors.AlphaDarken(new Color(255, 240, 0)) * 0.5f, 0f, Vector2.Zero, Vector2.One, -1f);
                    ChatManager.DrawColorCodedString(Main.spriteBatch, Main.fontMouseText, cursorText, position + new Vector2(1, 0).RotatedBy(Main.GlobalTime - MathHelper.PiOver2), Colors.AlphaDarken(Color.White), 0f, Vector2.Zero, Vector2.One, -1f);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                }
            };

            // I think it's safe.
            On.Terraria.Item.Prefix += (orig, self, prefixType) =>
            {
                bool update = self.rare == CustomRarityGlobalItem.RarityID;
                bool flag = orig(self, prefixType);

                if (update) self.rare = CustomRarityGlobalItem.RarityID;

                return flag;
            };

            return base.Autoload(ref name);
        }

        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.mod == "Terraria" && line.Name == "ItemName" && item.rare == CustomRarityGlobalItem.RarityID)
            {
                Main.spriteBatch.End();
                Effect effect = mod.GetEffect("Effects/CustomRarity");
                effect.Parameters["time"].SetValue(Main.GlobalTime * 15);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, effect, Main.UIScaleMatrix);

                Vector2 position = new Vector2((float)line.X, (float)line.Y);
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.font, line.text, position + new Vector2(3.5f, 0).RotatedBy(Main.GlobalTime), Colors.AlphaDarken(new Color(250, 0, 60)) * 0.9f, line.rotation, line.origin, line.baseScale);
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.font, line.text, position + new Vector2(3.5f, 0).RotatedBy(Main.GlobalTime + MathHelper.Pi), Colors.AlphaDarken(new Color(0, 120, 255)) * 0.9f, line.rotation, line.origin, line.baseScale);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, position, Colors.AlphaDarken(Color.White), line.rotation, line.origin, line.baseScale, line.maxWidth, line.spread);
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.font, line.text, position + new Vector2(1, 0).RotatedBy(Main.GlobalTime + MathHelper.PiOver2), Colors.AlphaDarken(new Color(255, 240, 0)) * 0.5f, line.rotation, line.origin, line.baseScale);
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.font, line.text, position + new Vector2(1, 0).RotatedBy(Main.GlobalTime - MathHelper.PiOver2), Colors.AlphaDarken(Color.White), line.rotation, line.origin, line.baseScale);

                return false;
            }
            return base.PreDrawTooltipLine(item, line, ref yOffset);
        }

        public override void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
        {
            if (line.mod == "Terraria" && line.Name == "ItemName" && item.rare == CustomRarityGlobalItem.RarityID)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
            }
        }
    }
}
