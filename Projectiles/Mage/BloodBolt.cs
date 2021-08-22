using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Projectiles.Mage
{
    public class BloodBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            aiType = ProjectileID.WaterBolt;
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.alpha = 255;
            projectile.penetrate = 3;
        }
        public int aiStyle = 8;

        public override void AI()
        {
            //add lighting
            Lighting.AddLight(projectile.position, new Vector3(0.5f, 0f, 0f)); //the Vector3 will be the color in rgb values, the vector2 will be your projectile's position
            Lighting.maxX = 400; //height 
            Lighting.maxY = 400; //width
        }
        // Additional hooks/methods here.
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //If collide with tile, reduce the penetrate.
            //So the projectile can reflect at most 5 times
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }
            else
            {
                Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
                Main.PlaySound(SoundID.Item10, projectile.position);
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Item10, projectile.position);
        }

        public override void PostAI()
        {
            for (int num92 = 0; num92 < 5; num92++)
            {
                float num93 = projectile.velocity.X / 3f * (float)num92;
                float num94 = projectile.velocity.Y / 3f * (float)num92;
                int num95 = 4;
                int num96 = Dust.NewDust(new Vector2(projectile.position.X + (float)num95, projectile.position.Y + (float)num95), projectile.width - num95 * 2, projectile.height - num95 * 2, 105, 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[num96].noGravity = true;
                Dust dust15 = Main.dust[num96];
                Dust dust2 = dust15;
                dust2.velocity *= 0.1f;
                dust15 = Main.dust[num96];
                dust2 = dust15;
                dust2.velocity += projectile.velocity * 0.1f;
                Main.dust[num96].position.X -= num93;
                Main.dust[num96].position.Y -= num94;
            }
            if (Main.rand.Next(5) == 0)
            {
                int num97 = 4;
                int num98 = Dust.NewDust(new Vector2(projectile.position.X + (float)num97, projectile.position.Y + (float)num97), projectile.width - num97 * 2, projectile.height - num97 * 2, 218, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num98].noGravity = true;
                Dust dust16 = Main.dust[num98];
                Dust dust2 = dust16;
                dust2.velocity *= 0.25f;
                dust16 = Main.dust[num98];
                dust2 = dust16;
                dust2.velocity += projectile.velocity * 0.5f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float distanceFromTarget = 700f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy() && target.whoAmI != npc.whoAmI)
                {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);

                    if (((closest && inRange) || !foundTarget) && lineOfSight)
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                        Vector2 direction = targetCenter - projectile.Center;
                        direction.Normalize();
                        projectile.velocity = (direction * projectile.velocity.Length());
                    }
                }
            }
        }
    }
}
