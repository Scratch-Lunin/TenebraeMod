using System.IO;
using Microsoft.Xna.Framework;
using TenebraeMod.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TenebraeMod.NPCs
{
    internal class VileLeechHead : VileLeech
    {
        public override string Texture => "TenebraeMod/NPCs/VileLeech_Head";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vile Leech");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.width = 32;
            npc.height = 26;
            npc.defense = 0;
            npc.damage = 26;
            npc.knockBackResist = 0f;
            npc.lifeMax = 200;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.npcSlots = 1f;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[BuffID.Confused] = true;
        }

        public override void AI()
        {
            NPC boss = Main.npc[(int)npc.ai[0]];
            npc.velocity = Vector2.Normalize(boss.Center - npc.Center) * 5;
            npc.direction = npc.velocity.X > 0 ? 1 : -1;
        }

        public override void Init()
        {
            base.Init();
            head = true;
            directional = false;
        }

        private int attackType;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackType);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackType = reader.ReadInt32();
        }
    }

    internal class VileLeechBody : VileLeech
    {
        public override string Texture => "TenebraeMod/NPCs/VileLeech_Body";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vile Leech");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.width = 18;
            npc.height = 18;
            npc.defense = 5;
            npc.damage = 16;
            npc.knockBackResist = 0f;
            npc.lifeMax = 400;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.npcSlots = 0f;
            npc.behindTiles = true;
            npc.dontCountMe = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[BuffID.Confused] = true;
        }

        public override void Init()
        {
            base.Init();
            head = false;
            directional = false;
        }
    }

    internal class VileLeechTail : VileLeech
    {
        public override string Texture => "TenebraeMod/NPCs/VileLeech_Tail";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vile Leech");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.width = 18;
            npc.height = 14;
            npc.defense = 5;
            npc.damage = 16;
            npc.knockBackResist = 0f;
            npc.lifeMax = 400;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.npcSlots = 0f;
            npc.dontCountMe = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[BuffID.Confused] = true;
        }

        public override void Init()
        {
            base.Init();
            tail = true;
            directional = false;
        }
    }

    public abstract class VileLeech : Worm
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vile Leech");
        }

        public override void Init()
        {
            maxLength = 6;
            minLength = 4;
            tailType = NPCType<VileLeechTail>();
            bodyType = NPCType<VileLeechBody>();
            headType = NPCType<VileLeechHead>();
            flies = true;
            speed = 6f;
            turnSpeed = 0.3f;
        }
    }
}