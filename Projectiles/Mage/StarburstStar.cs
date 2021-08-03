using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles.Mage
{
    // I do not know if this works correctly in multiplayer, but as WAV42 said, many things in this mod don't work well in multiplayer

    public class StarburstStar : ModProjectile
    {
        public Vector3 Center
        {
            get => new Vector3(projectile.Center.X, projectile.Center.Y, ZCenter);
            set
            {
                projectile.position.X = value.X - projectile.width * 0.5f;
                projectile.position.Y = value.Y - projectile.height * 0.5f;
                ZCenter = value.Z;
            }
        }

        // ...

        private AIState State { get; set; } = AIState.Emergence;
        private int Index => (int)projectile.ai[0];
        private ref float ZCenter => ref projectile.ai[1];

        private Vector2? _attackDot = null;

        // ...

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starburst Star");

            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;

            projectile.magic = true;
            projectile.friendly = true;

            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;

            projectile.hide = true;
            projectile.timeLeft = 720;
        }

        public override void AI()
        {
            var owner = Main.player[projectile.owner];

            switch (State)
            {
                case AIState.Emergence:
                    {
                        var (timeDifference, time) = this.SetPositionAroundPlayer(owner);
                        projectile.rotation = (float)Math.Sin(time + timeDifference) * 0.5f;

                        float progress = (projectile.timeLeft - 700) / 10f;
                        projectile.scale = MathHelper.SmoothStep(1.5f, 1, Math.Abs(1 - progress));

                        if (progress <= 0.025f)
                        {
                            projectile.scale = 1f;
                            this.ChangeState(AIState.RotationAroundPlayer);
                        }
                    }
                    break;
                case AIState.Attack:
                    {
                        if (_attackDot == null || Vector2.Distance(projectile.Center, _attackDot.Value) < 8)
                        {
                            projectile.tileCollide = false;
                            this.ChangeState(AIState.Return);
                            _attackDot = null;

                            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<StarburstStarExplosion>(), projectile.damage, projectile.knockBack, projectile.owner);
                            return;
                        }

                        projectile.rotation += 0.2f;
                        projectile.velocity = Vector2.Normalize(_attackDot.Value - projectile.Center) * (owner?.HeldItem.shootSpeed ?? 15f);
                    }
                    break;
                case AIState.Return:
                    {
                        float value = MathHelper.TwoPi / 3f * Index;
                        float value2 = MathHelper.ToRadians(-projectile.timeLeft);
                        Vector3 target3 = GetPositionAroundPlayer(owner, value, value2);
                        Vector2 target = V3ToV2(target3);

                        projectile.rotation += 0.2f;
                        projectile.velocity = Vector2.Normalize(target - projectile.Center) * 12f;

                        if (Vector2.Distance(projectile.Center, target) < 12f)
                        {
                            this.ChangeState(AIState.Emergence);
                        }
                    }
                    break;
                case AIState.RotationAroundPlayer:
                    {
                        var (timeDifference, time) = this.SetPositionAroundPlayer(owner);
                        projectile.rotation = (float)Math.Sin(time + timeDifference) * 0.5f;
                    }
                    break;
                default:
                    projectile.scale = projectile.timeLeft / 8f;
                    break;
            }
        }

        public override bool PreAI()
        {
            var owner = Main.player[projectile.owner];

            if (State != AIState.Disappearance)
            {
                if (projectile.timeLeft <= 360) projectile.timeLeft = 721;

                if (owner?.HeldItem.type != ModContent.ItemType<Items.Weapons.Mage.StarburstScepter>())
                {
                    projectile.velocity = Vector2.Zero;
                    projectile.timeLeft = 8;
                    projectile.damage = 0;

                    this.ChangeState(AIState.Disappearance);
                }

                if (State == AIState.RotationAroundPlayer && _attackDot != null)
                {
                    ZCenter = 60f;
                    projectile.tileCollide = true;
                    this.ChangeState(AIState.Attack);
                }
            }

            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (State == AIState.Attack)
            {
                projectile.tileCollide = false;
                projectile.velocity = Vector2.Zero;
                _attackDot = null;

                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<StarburstStarExplosion>(), projectile.damage, projectile.knockBack, projectile.owner);

                this.ChangeState(AIState.Return);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            _attackDot = null;

            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<StarburstStarExplosion>(), projectile.damage, projectile.knockBack, projectile.owner);

            this.ChangeState(AIState.Return);
        }

        public override bool? CanHitNPC(NPC target) => State == AIState.Attack;
        public override void SendExtraAI(BinaryWriter writer) => writer.Write((int)this.State);
        public override void ReceiveExtraAI(BinaryReader reader) => this.State = (AIState)reader.ReadInt32();

        private enum AIState
        {
            Emergence,
            RotationAroundPlayer,
            Attack,
            Return,
            Disappearance
        }

        #region | Drawing
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            // Set projectile.hide = true;

            if (ZCenter >= 0) TenebraeMod.DrawCacheProjsFrontPlayers.Add(index);
            else drawCacheProjsBehindProjectiles.Add(index);
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            Lighting.AddLight(projectile.Center, new Color(255, 239, 38).ToVector3() * 0.2f);
            return base.PreDrawExtras(spriteBatch);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture;

            var drawPosition = projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            float zScale = 0.75f + ZCenter / 240f;
            float scale = projectile.scale * zScale;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            if (State == AIState.Attack || State == AIState.Return)
            {
                texture = ModContent.GetTexture(this.Texture + "_Extra1");

                for (int k = 1; k < projectile.oldPos.Length; k++)
                {
                    var position = projectile.oldPos[k] + projectile.Size * 0.5f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                    float num = (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length;
                    Color trailColor = Color.Lerp(new Color(0.95f, 0.65f, 0.03f, 0.5f), new Color(0.35f, 0.05f, 0.95f, 0.5f), num) * num * zScale;
                    spriteBatch.Draw(texture, position, null, trailColor, projectile.oldRot[k], texture.Size() * 0.5f, scale * num, SpriteEffects.None, 0f);
                }
            }

            texture = ModContent.GetTexture(this.Texture + "_Extra0");
            spriteBatch.Draw(texture, drawPosition, null, new Color(0.95f, 0.65f, 0.03f, 0.5f), 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, drawPosition, null, new Color(zScale, zScale, zScale, 1f), projectile.rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            return false;
        }
        #endregion

        #region | New Methods
        public bool IsReadyToAttack => State == AIState.RotationAroundPlayer;

        public void SetMousePosition(Vector2 value)
        {
            _attackDot = value;
            projectile.netUpdate = true;
        }

        private void ChangeState(AIState value)
        {
            State = value;
            projectile.netUpdate = true;
        }

        private Vector3 GetPositionAroundPlayer(Player owner, float timeDifference, float time)
        {
            Vector2 center = new Vector2((int)(owner.position.X + (owner.width / 2)), (int)(owner.position.Y + (owner.height / 2) + owner.gfxOffY));
            float rotX = time + timeDifference;
            float rotY = (float)Math.Cos(rotX) * 0.15f * (float)Math.Sin(time);

            return V2ToV3(center) + Vector3.Transform(Vector3.UnitZ * 60, Matrix.CreateFromYawPitchRoll(rotX, rotY, 0f));
        }

        private (float timeDifference, float time) SetPositionAroundPlayer(Player owner)
        {
            float value = MathHelper.TwoPi / 3f * Index;
            float value2 = MathHelper.ToRadians(-projectile.timeLeft);
            Center = GetPositionAroundPlayer(owner, value, value2);
            return (value, value2);
        }

        private static Vector3 V2ToV3(Vector2 vector) => new Vector3(vector.X, vector.Y, 0);
        private static Vector2 V3ToV2(Vector3 vector) => new Vector2(vector.X, vector.Y);

        // How disgusting it is for me to look at this...
        #endregion
    }
}
