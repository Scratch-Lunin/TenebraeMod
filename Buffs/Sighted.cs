using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TenebraeMod.Buffs
{
    public class Sighted : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sighted");
            Description.SetDefault("You have been spotted, Eye Arrows will home towards you!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.Next(4) < 3)
            {
                int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 2, npc.height + 2, 43, 0f, 0f, 100, new Color(255, 0, 0), 1f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}