using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMist
{
    public class P_RedMist_0 : Passive_Char, IP_PlayerTurn
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public void Turn()
        {
            if (this.BChar.BuffFind("B_RedMist_0") || BattleSystem.instance.TurnNum < 2)
            {
                return;
            }
            Skill s = Skill.TempSkill("S_RedMist_0", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(s, true);
        }
    }
}
