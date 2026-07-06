using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chihiro
{
    public class B_Chihiro_1 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.DMGTaken = 4f * base.StackNum;
        }
    }
    public class B_Chihiro_3 : Buff, IP_PlayerTurn
    {
        public int num = 0;
        public override void Init()
        {
            base.Init();
        }
        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", num.ToString());
        }

        public void Turn()
        {
            BattleSystem.instance.AllyTeam.Draw(num);
            this.SelfDestroy();
        }
    }
    public class B_Chihiro_5 : Buff, IP_Dodge
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.PerfectDodge = true;
            this.OnePassive = true;
        }
        public void Dodge(BattleChar Char, SkillParticle SP)
        {
            if (!SP.UseStatus.Info.Ally && Char == this.BChar)
            {
                Skill skill = Skill.TempSkill(SP.SkillData.MySkill.KeyID,this.BChar, this.BChar.MyTeam);
                skill.PlusHit = true;
                BattleSystem.DelayInput(BattleSystem.instance.ForceAction(skill, SP.UseStatus, false, false, true, null));
                this.SelfDestroy();
            }
        }
    }

    public class B_Chihiro_9 : Buff, IP_SkillUse_User_After
    {
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 60;
        }

        public void SkillUseAfter(Skill SkillD)
        {
            if (SkillD.Master == this.BChar && SkillD.MySkill.KeyID != "S_Chihiro_9")
            {
                this.SelfDestroy();
            }
        }
    }
    public class B_Chihiro_R2 : Buff, IP_SkillUse_Team_Target
    {
        public int turnEndButton = 0;
        public List<string> Magatsumi = new List<string>();
        public void SkillUseTeam_Target(Skill skill, List<BattleChar> Targets)
        {
            if (skill.MySkill.KeyID == "S_Chihiro_R2_0" || skill.MySkill.KeyID == "S_Chihiro_R2_1" || skill.MySkill.KeyID == "S_Chihiro_R2_2" ||
                skill.MySkill.KeyID == "S_Chihiro_R2_3" || skill.MySkill.KeyID == "S_Chihiro_R2_4")
            {
                if (Magatsumi.Contains(skill.MySkill.KeyID))
                {

                }
                else
                {
                    Magatsumi.Add(skill.MySkill.KeyID);
                }
            }
        }
        public override string DescExtended()
        {
            if (Magatsumi.Count == 0)
            {
                this.IsHide = true;
                return base.DescExtended();
            }
            else
            {
                this.IsHide = false;
                string result = "";
                foreach (string s in Magatsumi)
                {
                    Skill skill = Skill.TempSkill(s);
                    result += skill.MySkill.Name + "\n";
                }
                return base.DescExtended().Replace("&a", result);
            }
        }
    }
    public class B_Chihiro_R2_0 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.Stun = true;
            this.PlusStat.dod = -33f;
        }

        public override void SelfdestroyPlus()
        {
            base.SelfDestroy();
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            //this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            //this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            //this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            //this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
        }
    }
    public class B_Chihiro_R2_1 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.def = -66f;
        }
    }
}
