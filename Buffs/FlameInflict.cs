using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TenebraeMod.Buffs
{
    public class FlameInflict : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Flaming Infliction");
            Description.SetDefault("Fireballs of the Vile Amulet will target you");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
        }
    }
}