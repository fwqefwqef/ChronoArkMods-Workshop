using GameDataEditor;
using I2.Loc;
using NLog.Targets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleTear
{
    public class S_PurpleTear_1 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
        public override void SkillKill(SkillParticle SP)
        {
            base.SkillKill(SP);
            BattleSystem.instance.AllyTeam.AP += 2;
        }
    }
    public class S_PurpleTear_2 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill temp = Skill.TempSkill("S_PurpleTear_2_0", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(temp, true);
        }
    }
    public class S_PurpleTear_3 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
    public class S_PurpleTear_4 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
    public class S_PurpleTear_5 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
    public class S_PurpleTear_6 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
    public class S_PurpleTear_7 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (b != this.BChar)
                {
                    b.BuffAdd("B_PurpleTear_7", this.BChar);
                }
            }
        }
    }
    public class S_PurpleTear_8 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            int counter = 0;

            using (List<Buff>.Enumerator enumerator = Targets[0].Buffs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_DOT)
                    {
                        counter++;
                        break;
                    }
                }
            }

            using (List<Buff>.Enumerator enumerator = Targets[0].Buffs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_Debuff)
                    {
                        counter++;
                        break;
                    }
                }
            }

            using (List<Buff>.Enumerator enumerator = Targets[0].Buffs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_CrowdControl)
                    {
                        counter++;
                        break;
                    }
                }
            }

            for (int i = 0; i < counter; i++)
            {
                BattleSystem.DelayInput(this.Attacks(Targets[0]));
            }

        }
        public IEnumerator Attacks(BattleChar Target)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            Skill skill = Skill.TempSkill("S_PurpleTear_8_0", this.BChar, this.BChar.MyTeam);
            skill.PlusHit = true;

            if (Target != null && !Target.IsDead)
            {
                this.BChar.ParticleOut(skill, Target);
            }
            else if (BattleSystem.instance.EnemyTeam.AliveChars.Count != 0)
            {
                this.BChar.ParticleOut(skill, BattleSystem.instance.EnemyTeam.AliveChars.Random(this.BChar.GetRandomClass().Main));
            }
            yield break;
        }
    }
    public class S_PurpleTear_9 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (Targets[0] is BattleEnemy)
            {
                BattleEnemy battleEnemy = Targets[0] as BattleEnemy;
                if (battleEnemy.SkillQueue.Count != 0 && (battleEnemy.SkillQueue[0].CastSpeed == 0 || battleEnemy.SkillQueue[0].CastSpeed == 1 || battleEnemy.SkillQueue[0].CastSpeed >= 9))
                {
                    this.PlusSkillStat.cri = 100f;
                }
            }
        }
    }
    public class S_PurpleTear_10 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            for (int i = 0; i < Targets[0].Buffs.Count; i++)
            {
                if (Targets[0].Buffs[i].BuffData.Debuff && !Targets[0].Buffs[i].CantDisable)
                {
                    Targets[0].Buffs[i].SelfDestroy(false);
                }
            }
        }
    }
    public class S_PurpleTear_11 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
    public class S_PurpleTear_12 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill temp = Skill.TempSkill("S_PurpleTear_12_0",this.BChar, this.BChar.MyTeam);
            temp.PlusHit = true;
            BattleSystem.DelayInput(Wait());
            BattleSystem.DelayInput(BattleSystem.instance.ForceAction(temp, Targets[0], false, false, true));
            BattleSystem.DelayInput(BattleSystem.instance.ForceAction(temp, Targets[0], false, false, true));
        }

        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.3f);
        }
    }
    public class S_PurpleTear_13 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            bool SlashStance = this.BChar.BuffFind("B_PurpleTear_P_Slash");
            bool PierceStance = this.BChar.BuffFind("B_PurpleTear_P_Pierce");
            bool BluntStance = this.BChar.BuffFind("B_PurpleTear_P_Blunt");
            bool GuardStance = this.BChar.BuffFind("B_PurpleTear_P_Guard");

            string stance = "";

            if (SlashStance)
            {
                stance = "B_PurpleTear_P_Slash_Temp";
            }
            else if (PierceStance)
            {
                stance = "B_PurpleTear_P_Pierce_Temp";
            }
            else if (BluntStance)
            {
                stance = "B_PurpleTear_P_Blunt_Temp";
            }
            else if (GuardStance)
            {
                stance = "B_PurpleTear_P_Guard_Temp";
            }
            else
            {
                return;
            }

            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (b != this.BChar)
                {
                    b.BuffAdd(stance, this.BChar);
                }
            }
        }
    }
    public class S_PurpleTear_LucyD : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(BattleSystem.instance.AllyTeam.Skills_Deck, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.DrawSkill, false, true, true, false, true));
        }
        public void Del(SkillButton Mybutton)
        {
            BattleTeam team = Mybutton.Myskill.Master.MyTeam;
            List<Skill> drawPile = team.Skills_Deck;
            List<Skill> discardPile = team.Skills_UsedDeck;
            int selectedIndex = drawPile.IndexOf(Mybutton.Myskill);

            if (selectedIndex < 0)
            {
                return;
            }

            for (int i = selectedIndex - 1; i >= 0; i--)
            {
                Skill skill = drawPile[i];
                drawPile.RemoveAt(i);
                discardPile.Insert(0, skill);
            }

            team.Draw(1);
        }
    }
    public class SkillEn_PurpleTear_0 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.NotCount = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
}
