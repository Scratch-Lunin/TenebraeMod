using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles.Mage
{
    public class StarburstStarExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starburst Star");
        }

        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;

            projectile.magic = true;
            projectile.friendly = true;

            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;

            projectile.timeLeft = 10;
        }

        public override void AI()
        {
            projectile.rotation += 0.1f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var drawPosition = projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture = Main.projectileTexture[projectile.type];
            var origin = texture.Size() * 0.5f + new Vector2(0, 8);
            Color color = new Color(0.35f, 0.05f, 0.95f);

            float progress = 1 - Math.Abs(1 - projectile.timeLeft / 10f);

            spriteBatch.Draw(texture, drawPosition, null, color * 0.7f, projectile.rotation, origin, projectile.scale * 0.5f * progress, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, drawPosition, null, color * 0.5f, -projectile.rotation, origin, projectile.scale * 0.4f * progress, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
