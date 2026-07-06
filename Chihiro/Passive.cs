using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GameDataEditor;
using I2.Loc;
using System.Collections;

namespace Chihiro
{
    public class P_Chihiro : Passive_Char, IP_TurnEnd, IP_PlayerTurn, IP_SkillUse_User_After
    {
        public int count = 0;
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public void SkillUseAfter(Skill SkillD)
        {
            if (count == 0 && SkillD.IsDamage)
            {
                BattleSystem.instance.AllyTeam.DiscardCount += 1;
                count++;
            }
        }

        public void Turn()
        {
            if (this.BChar.BuffReturn("B_Chihiro_Tracker") == null)
            {
                this.BChar.BuffAdd("B_Chihiro_Tracker", this.BChar);
            }
        }

        public void TurnEnd()
        {
            count = 0;
        }
    }

    public class B_Chihiro_Tracker : Buff, IP_SkillUse_Team_Target
    {
        public BattleChar recent;
        public void SkillUseTeam_Target(Skill skill, List<BattleChar> Targets)
        {
            if (skill.IsDamage && (Targets[0] is BattleEnemy))
            {
                recent = Targets[0];
            }
        }
    }
}
