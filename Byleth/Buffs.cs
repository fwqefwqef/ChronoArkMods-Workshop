using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Byleth
{
    public class B_Byleth_1 : Buff
    {
        public override void Init()
        {
            this.PlusStat.dod = 33f * base.StackNum;
        }
    }
    public class B_Byleth_1_1 : Buff
    {
        public override void Init()
        {
            this.PlusStat.DMGTaken = 33f * base.StackNum;
        }
    }
    public class B_Byleth_4 : Buff, IP_SkillUse_User_After
    {
        public override void Init()
        {
            this.PlusPerStat.Damage = -35 * base.StackNum;
            this.PlusStat.hit = -50f * base.StackNum;
        }
        public void SkillUseAfter(Skill SkillD)
        {
            if (SkillD.IsDamage)
            {
                base.SelfDestroy();
            }
        }
    }

    public class B_Byleth_5 : Buff, IP_SkillUseHand_Team
    {
        public override void Init()
        {
            this.isStackDestroy = true;
        }

        public void SKillUseHand_Team(Skill skill)
        {
            if (skill.IsDamage && skill.MySkill.KeyID != "S_Byleth_5_0" /*&& skill.Master != this.BChar*/)
            {
                base.SelfStackDestroy();
                Skill tempskill = Skill.TempSkill("S_Byleth_5_0", this.BChar, this.BChar.MyTeam);
                BattleSystem.instance.AllyTeam.Add(tempskill, true);
            }
        }
    }
    public class B_Byleth_7 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.PlusMPUse.PlusMP_Skills = 1;
        }
    }
    public class B_Byleth_8 : Buff
    {
        public override void Init()
        {
            this.PlusStat.HEALTaken = -25f * base.StackNum;
        }
    }
    public class B_Byleth_10 : Buff, IP_SkillUse_User // tracks class skills used this battle
    {
        public List<GDESkillData> UsedClassSkills = new List<GDESkillData>();
        public void SkillUse(Skill SkillD, List<BattleChar> Targets)
        {
            // Check if used skill is a class skill
            List<GDESkillData> characterSkillNoOverLap = PlayData.GetCharacterSkillNoOverLap(this.BChar.Info, false, null);
            bool flag = true;
            //Debug.Log("Looking for skill: " + SkillD.MySkill.KeyID);
            foreach (GDESkillData skill in characterSkillNoOverLap)
            {
                //Debug.Log("Comparing to: " + skill.KeyID);
                if (skill.KeyID == SkillD.MySkill.KeyID)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                return;
            }
            foreach (GDESkillData skill in UsedClassSkills)
            {
                if (skill.KeyID == SkillD.MySkill.KeyID)
                {
                    return;
                }
            }
            UsedClassSkills.Add(SkillD.MySkill);
        }
        public override string DescExtended()
        {
            string skillList = "";
            // append the names of the used class skills to skillList, separated by linebreak
            foreach (GDESkillData skill in UsedClassSkills)
            {
                skillList += skill.Name + "\n";
            }
            return base.DescExtended().Replace("&b", skillList);
        }
    }
}
