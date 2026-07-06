using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.Plugin;
using ChronoArkMod.Template;
using Debug = UnityEngine.Debug;
using ChronoArkMod.ModData;

namespace Argalia
{
    public class B_Argalia_Immune : Buff, IP_DamageTakeChange, IP_PlayerTurn
    {
        public override void Init()
        {
            base.Init();
        }
        public int DamageTakeChange(BattleChar Hit, BattleChar User, int Dmg, bool Cri, bool NODEF = false, bool NOEFFECT = false, bool Preview = false)
        {
            if (!NODEF)
            {
                int newDMG = (int)(Dmg * 0.2);
                if (newDMG == 0)
                {
                    newDMG = 1;
                }
                return newDMG;
            }
            return Dmg;
        }
        public void Turn() // Remove on turn start
        {
            this.SelfDestroy();
        }
    }

    // Vibration
    public class B_Argalia_P : Buff //, IP_SkillUse_User_After
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            base.isStackDestroy = true;
            this.PlusStat.hit = -1f * base.StackNum;
        }

        public override void TurnUpdate()
        {
            base.SelfStackDestroy();
        }
        //public void SkillUseAfter(Skill SkillD)
        //{
        //    base.SelfStackDestroy();
        //}
    }

    // Tempestuous Danza and Trails of Blue
    public class B_Argalia_Barrier : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
    }

    // Controlled Resonance
    public class B_Argalia_4 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
    }

    // Trails of Blue
    public class B_Argalia_6 : Buff, IP_DamageChange
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            int vibration = 0;
            Buff b = this.BChar.BuffReturn("B_Argalia_P");
            if (b != null)
            {
                vibration = b.StackNum;
            }
            return Damage - 1 - vibration;
        }
    }

    // Dissonance
    public class B_Argalia_7 : Buff //, IP_SkillUse_User
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.RES_CC = 80f;
            this.PlusStat.RES_DEBUFF = 80f;
        }

        //public void SkillUse(Skill SkillD, List<BattleChar> Targets)
        //{
        //    foreach (BattleChar b in Targets)
        //    {
        //        if (b is BattleEnemy)
        //        {
        //            //b.BuffRemove("B_Argalia_P");
        //            b.BuffAdd("B_Argalia_P", this.BChar);
        //        }
        //    }
        //}
    }

    // Preludio
    public class B_Argalia_8 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.Strength = true;
            this.PlusStat.def = 15f;
        }
    }

    // Second Wind
    public class B_Argalia_8_0 : Buff, IP_PlayerTurn
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public void Turn()
        {
            BattleSystem.instance.AllyTeam.AP += 1;
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                b.BuffRemove("B_Argalia_P");
                b.BuffAdd("B_Argalia_P", this.BChar);
                b.BuffAdd("B_Argalia_P", this.BChar);
            }
            this.SelfDestroy();
        }
    }

    // Impromptu
    public class B_Argalia_11 : Buff, IP_PlayerTurn
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusPerStat.Damage = 15;
            this.PlusPerStat.MaxHP = 15;
        }
        public void Turn()
        {
            Skill skill = Skill.TempSkill("S_Argalia_11_0", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }

    public class B_Argalia_LucyD : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.spd = 1 * base.StackNum;
        }
    }
}
