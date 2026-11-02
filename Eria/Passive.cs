using ChronoArkMod;
using System;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
namespace Eria
{
    // Magic Palette: class skills leave behind colored paint
    public class P_Eria : Passive_Char, IP_SkillUse_User
    {
        public const string Red = "B_Eria_P_Red";
        public const string Blue = "B_Eria_P_Blue";
        public const string Yellow = "B_Eria_P_Yellow";
        public static readonly List<string> Colors = new List<string> { Red, Blue, Yellow };

        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public void SkillUse(Skill SkillD, List<BattleChar> Targets)
        {
            if (SkillD.Master != this.BChar)
            {
                return;
            }
            string paint = PaintOf(SkillD.MySkill.KeyID);
            if (paint != null)
            {
                this.BChar.BuffAdd(paint, this.BChar);
            }
        }

        // Which paint a class skill leaves behind
        public static string PaintOf(string key)
        {
            switch (key)
            {
                case "S_Eria_1":
                case "S_Eria_2":
                case "S_Eria_3":
                    return Red;
                case "S_Eria_4":
                case "S_Eria_5":
                case "S_Eria_6":
                    return Blue;
                case "S_Eria_7":
                case "S_Eria_8":
                case "S_Eria_9":
                    return Yellow;
            }
            return null;
        }

        public static int PaintCount(BattleChar eria, string paint)
        {
            Buff b = eria.BuffReturn(paint);
            return b == null ? 0 : b.StackNum;
        }

        public static void UsePaint(BattleChar eria, string paint)
        {
            Buff b = eria.BuffReturn(paint);
            if (b == null)
            {
                return;
            }
            b.SelfStackDestroy();
            if (b.StackNum > 0)
            {
                b.Init(); // Init only reruns when stacks are added, so refresh the per-stack stats
            }
        }

        public static BattleChar FindEria()
        {
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (b.Info.KeyData == "Eria")
                {
                    return b;
                }
            }
            return null;
        }
    }
    public class B_Eria_P_Red : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.HIT_CC = 1f * base.StackNum;
            this.PlusStat.HIT_DEBUFF = 1f * base.StackNum;
            this.PlusStat.HIT_DOT = 1f * base.StackNum;
        }
    }
    public class B_Eria_P_Blue : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.RES_CC = 1f * base.StackNum;
            this.PlusStat.RES_DEBUFF = 1f * base.StackNum;
            this.PlusStat.RES_DOT = 1f * base.StackNum;
        }
    }
    public class B_Eria_P_Yellow : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.DeadImmune = 1 * base.StackNum;
        }
    }
}
