using DarkTonic.MasterAudio;
using GameDataEditor;
using NLog.Targets;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RedMist
{
    public class S_RedMist_0 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            //MasterAudio.StopBus("BGM");
            //MasterAudio.StopBus("BattleBGM");
            //MasterAudio.FadeBusToVolume("BGM", 1f, 1f, null, false, false);
            //MasterAudio.FadeBusToVolume("BattleBGM", 0f, 0.5f, null, false, false);
            //MasterAudio.PlaySound("RedMist", 1f);
        }
    }
    public class S_RedMist_1 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public int ShotNum
        {
            get
            {
                if (BattleSystem.instance != null && BattleSystem.instance.BattleLogs != null && BattleSystem.instance.TurnNum >= 1)
                {
                    return BattleSystem.instance.BattleLogs.getSkills(null, (Skill skill) => !skill.FreeUse && skill.Master == this.BChar, BattleSystem.instance.TurnNum).Count;
                }
                return 0;
            }
        }
        public override string DescExtended(string desc)
        {
            return base.DescExtended(desc).Replace("&a", this.ShotNum.ToString());
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);

            for (int i=0;i<ShotNum+1; i++)
            {
                BattleSystem.DelayInput(Attack(Targets));
            }
        }
        public IEnumerator Attack(List<BattleChar> Targets)
        {
            yield return new WaitForSeconds(0.15f);
            Skill s = Skill.TempSkill("S_RedMist_1_0", this.BChar, this.BChar.MyTeam);
            s.PlusHit = true;
            if (Targets[0].IsDead)
            {
                this.BChar.ParticleOut(s, BattleSystem.instance.EnemyList.Random(this.BChar.GetRandomClass().Main));
            }
            else
            {
                this.BChar.ParticleOut(s, Targets[0]);
            }
            yield break;
        }
    }
    public class S_RedMist_2 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public override void SkillKill(SkillParticle SP)
        {
            
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            BattleSystem.instance.AllyTeam.CharacterDraw(this.BChar, null);
        }
    }
    public class S_RedMist_3 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public override void SkillKill(SkillParticle SP)
        {
            BattleSystem.instance.AllyTeam.AP += 3;
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }
    public class S_RedMist_4 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);

            // Shuffle own skills back into deck
            for (int i = 0; i < BattleSystem.instance.AllyTeam.Skills_UsedDeck.Count; i++)
            {
                if (BattleSystem.instance.AllyTeam.Skills_UsedDeck[i].Master == this.BChar)
                {
                    int index = RandomManager.RandomInt(this.BChar.GetRandomClass().Main, 0, BattleSystem.instance.AllyTeam.Skills_Deck.Count + 1);
                    BattleSystem.instance.AllyTeam.Skills_Deck.Insert(index, BattleSystem.instance.AllyTeam.Skills_UsedDeck[i]);
                    BattleSystem.instance.AllyTeam.Skills_UsedDeck.RemoveAt(i);
                    i--;
                }
            }

            BattleSystem.instance.AllyTeam.CharacterDraw(this.BChar, null);
            BattleSystem.instance.AllyTeam.CharacterDraw(this.BChar, null);
        }
    }
    public class S_RedMist_5 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public override void SkillKill(SkillParticle SP)
        {
            Debug.Log("I'm here");
            Skill s = Skill.TempSkill("S_RedMist_5", this.BChar, this.BChar.MyTeam);
            s.PlusHit = true;
            BattleSystem.DelayInput(BattleSystem.instance.ForceAction(s, BattleSystem.instance.EnemyList.Random(this.BChar.GetRandomClass().Main), false, false, true, null));
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }
    public class S_RedMist_6 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);

            Buff buff = this.BChar.BuffAdd(GDEItemKeys.Buff_B_Momori_P_NoDead, this.BChar, false, 0, false, -1, false);
            this.BChar.Damage(this.BChar, this.BChar.GetStat.maxhp / 3, false, true);
            buff.SelfDestroy(false);
        }
    }
    public class S_RedMist_7 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos("SmilingBody", null));
            //BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos("SmilingBody", null));
        }
    }
    public class S_RedMist_8 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            int num = Targets.Count;
            int restoreHP = this.BChar.GetStat.maxhp * num / 5;
            this.BChar.Heal(this.BChar, restoreHP, false, false, new BattleChar.ChineHeal());
        }
    }
    public class S_RedMist_9 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public override void SkillKill(SkillParticle SP)
        {
            Skill s = Skill.TempSkill("S_RedMist_9", this.BChar, this.BChar.MyTeam);
            s.isExcept = true;
            s.AutoDelete = 1;
            BattleSystem.instance.AllyTeam.Add(s, true);
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }
    public class S_RedMist_10 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }
    public class S_RedMist_11 : Skill_Extended, IP_Kill
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public void KillEffect(SkillParticle SP)
        {
            this.APChange -= 1;
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            MasterAudio.PlaySound("SqueakMist", 10f);
        }
    }

    public class S_RedMist_LucyD : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            BattleSystem.instance.AllyTeam.Draw(2);
        }
    }
}
