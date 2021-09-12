using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TenebraeMod.Dusts
{
	public class HolyflameDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
            dust.scale = 2f;
		}

		public override bool Update(Dust dust) {
            dust.position += dust.velocity / 2;
			dust.rotation += dust.velocity.X / 2;
			dust.scale *= 0.95f;
			if (dust.scale < 0.1f) {
				dust.active = false;
			}
            else {
                float strength = dust.scale * 1.4f;
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), strength * 0.7f, strength * 0.65f, strength * 0.3f);
			}
            return false;
		}

        public override Color? GetAlpha(Dust dust, Color lightColor)
			=> new Color(lightColor.R, lightColor.G, lightColor.B, 25);
	}
	
	public class PinkHolyflameDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.scale = 2f;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity / 2;
			dust.rotation += dust.velocity.X / 2;
			dust.scale *= 0.95f;
			if (dust.scale < 0.1f)
			{
				dust.active = false;
			}
			else
			{
				float strength = dust.scale * 1.4f;
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), strength * 0.7f, strength * 0.65f, strength * 0.3f);
			}
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
			=> new Color(lightColor.R, lightColor.G, lightColor.B, 25);
	}
}