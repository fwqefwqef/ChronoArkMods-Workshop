using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;
using I2.Loc;
using System.Collections;

namespace Elementalist
{
    public class P_Elementalist : Passive_Char//, IP_PlayerTurn, IP_EnemyAwake //, IP_Kill
    {
        //public bool kill = false;
        //public override void Init()
        //{
        //    base.Init();
        //    this.OnePassive = true;
        //}
        //public void Turn()
        //{
        //    kill = false;
        //}
        //public bool getkill()
        //{
        //    return kill;
        //}

        //public void setkill()
        //{
        //    kill = true;
        //}
        //public void triggerPassive()
        //{
        //    if (kill == true)
        //    {
        //        return;
        //    }

        //    Random random = new Random();
        //    int number = random.Next(3);
        //    if (number == 0)
        //    {
        //        Skill skill = Skill.TempSkill("S_Fire", this.BChar, BattleSystem.instance.AllyTeam);
        //        BattleSystem.instance.AllyTeam.Add(skill, true);
        //        skill.isExcept = true;
        //    }
        //    else if (number == 1)
        //    {
        //        Skill skill = Skill.TempSkill("S_Elec", this.BChar, BattleSystem.instance.AllyTeam);
        //        BattleSystem.instance.AllyTeam.Add(skill, true);
        //        skill.isExcept = true;
        //    }
        //    else
        //    {
        //        Skill skill = Skill.TempSkill("S_Dark", this.BChar, BattleSystem.instance.AllyTeam);
        //        BattleSystem.instance.AllyTeam.Add(skill, true);
        //        skill.isExcept = true;
        //    }
        //}

        //public void EnemyAwake(BattleChar Enemy)
        //{
        //    if (!Enemy.BuffFind("B_CheckDead", false))
        //    {
        //        Enemy.BuffAdd("B_CheckDead", this.BChar, true, 0, false, -1, false);
        //    }
        //}

        //public void KillEffect(SkillParticle SP)
        //{
        //    if (kill)
        //    {
        //        return;
        //    }
        //    kill = true; //

        //    Random random = new Random();
        //    int number = random.Next(3);
        //    if (number == 0)
        //    {
        //        Skill skill = Skill.TempSkill("S_Fire", this.BChar, BattleSystem.instance.AllyTeam);
        //        BattleSystem.instance.AllyTeam.Add(skill, true);
        //        skill.isExcept = true;
        //    }
        //    else if (number == 1)
        //    {
        //        Skill skill = Skill.TempSkill("S_Elec", this.BChar, BattleSystem.instance.AllyTeam);
        //        BattleSystem.instance.AllyTeam.Add(skill, true);
        //        skill.isExcept = true;
        //    }
        //    else
        //    {
        //        Skill skill = Skill.TempSkill("S_Dark", this.BChar, BattleSystem.instance.AllyTeam);
        //        BattleSystem.instance.AllyTeam.Add(skill, true);
        //        skill.isExcept = true;
        //    }

        //}
    }
}
