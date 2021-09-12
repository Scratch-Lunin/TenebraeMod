using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TenebraeMod.Projectiles.Melee;

namespace TenebraeMod.Projectiles.Melee
{
    public class MandibleChakramProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mandible Chakram");
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 44;
            projectile.aiStyle = ProjectileID.Shuriken;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 100000;
            projectile.tileCollide = false;
            aiType = 18;
        }
        #region funny stuff
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.LightPink) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        #endregion
        #region DigEffects
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Vector2 posi = new Vector2(projectile.position.X, projectile.position.Y + 4 * projectile.ai[0]);
            Point pos = posi.ToTileCoordinates();
            Tile tileSafely = Framing.GetTileSafely(pos.X, pos.Y);
            if (tileSafely.active())
            {
                Tile tileSafely2 = Framing.GetTileSafely(pos.X, pos.Y - (int)projectile.ai[0]);
                if (!tileSafely2.active() || !Main.tileSolid[(int)tileSafely2.type] || Main.tileSolidTop[(int)tileSafely2.type])
                {
                    Dust dust = Main.dust[WorldGen.KillTile_MakeTileDust(pos.X, pos.Y, tileSafely)];
                    dust.velocity.Y = (dust.velocity.Y - 5 * projectile.ai[0]) * Main.rand.NextFloat();
                    int offset = projectile.ai[0] == -1 ? 24 : 8;
                }
            }
            projectile.velocity *= -2.7f;
            Main.PlaySound(SoundID.Roar, projectile.Center, 1);
            return false;
        }
        #endregion
        public override void AI()
        {
            projectile.rotation += (float)projectile.direction * 1f;
        }
        #region epic stuff
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int amountofproj = 3;
            target.AddBuff(BuffID.Slow, 200);
            for (int i = 0; i < amountofproj; ++i)
            {
                float currentRotation = (MathHelper.TwoPi / amountofproj) * i;
                Vector2 projectileVelocity = currentRotation.ToRotationVector2() * 6;
                Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2));
                int type = ModContent.ProjectileType<MCSandSpit>();
                Projectile.NewProjectile(projectile.Center, projectileVelocity, type, damage, 0, Main.myPlayer);
            }
        }
        #endregion
    }
}