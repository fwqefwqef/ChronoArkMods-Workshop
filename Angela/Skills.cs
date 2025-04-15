using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDataEditor;
using I2.Loc;
using UnityEngine;

namespace Angela
{
    // Urging
    public class S_Angela_0 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            //foreach (BattleChar b in Targets)
            //{
            //    b.Overload = 0;
            //}
            BattleSystem.instance.AllyTeam.AP += 1; 
        }
    }

    // Happy Memories
    public class S_Angela_1 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }


    // Coffin
    public class S_Angela_2 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            foreach (BattleChar b in Targets)
            {
                if (b is BattleEnemy)
                {
                    // Stun
                    b.BuffAdd(GDEItemKeys.Buff_B_Common_Rest, this.BChar, false, (int)(100f + this.BChar.GetStat.HIT_CC), false, -1, false);

                    // Heal 140%
                    b.Heal(this.BChar, this.BChar.GetStat.reg, 140, 0, (int)this.BChar.GetStat.cri);
                }
            }
        }
    }

    // Meal
    public class S_Angela_3 : Skill_Extended, IP_DamageChange, IP_SkillUse_Target
    {

        public override void Init()
        {
            base.Init();
            this.SkillBasePlus.Target_BaseDMG = (int)(this.BChar.GetStat.reg * 1.1f);
            this.OnePassive = true;
        }

        public override void FixedUpdate()
        {
            this.SkillBasePlus.Target_BaseDMG = (int)(this.BChar.GetStat.reg * 1.1f);
            base.FixedUpdate();
        }

        // AoE Heal 50% of damage dealt
        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            if (DMG != 0 && Misc.PerToNum((float)DMG, 50f) >= 1f)
            {
                foreach (BattleChar b in this.BChar.MyTeam.AliveChars)
                {
                    b.Heal(this.BChar, Misc.PerToNum((float)DMG, 50f), false, false, null);
                }
            }
        }

        // Damage Increase baed on enemy debuffs
        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            int finalDamage = Damage;

            if (Target.GetBuffs(BattleChar.GETBUFFTYPE.CC, false, false).Count != 0)
            {
                finalDamage += (int)(Damage * 0.33f);
            }

            if (Target.GetBuffs(BattleChar.GETBUFFTYPE.DEBUFF, false, false).Count != 0)
            {
                finalDamage += (int)(Damage * 0.33f);
            }

            if (Target.GetBuffs(BattleChar.GETBUFFTYPE.DOT, false, false).Count != 0)
            {
                finalDamage += (int)(Damage * 0.33f);
            }

            return finalDamage;
        }
    }

    // A Token of Friendship
    public class S_Angela_4 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }

    // Void
    public class S_Angela_5 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);

            List<Skill> list = new List<Skill>();
            list.Add(Skill.TempSkill("S_Angela_5_0", this.MySkill.Master, this.MySkill.Master.MyTeam));
            list.Add(Skill.TempSkill("S_Angela_5_1", this.MySkill.Master, this.MySkill.Master.MyTeam));
            BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, false));
        }
        public void Del(SkillButton Mybutton)
        {
            // Apply Once to all skills in hand and draw 1.
            if (Mybutton.Myskill.MySkill.KeyID == "S_Angela_5_0")
            {
                foreach (Skill skill in this.BChar.MyTeam.Skills)
                {
                    skill.Disposable = true;
                }
                BattleSystem.instance.AllyTeam.Draw(1);
            }

            // Exclude this skill
            if (Mybutton.Myskill.MySkill.KeyID == "S_Angela_5_1" && (this.MySkill.Disposable == false && this.MySkill.isExcept == false))
            {
                // delete from used deck
                foreach (Skill skill in this.BChar.MyTeam.Skills_UsedDeck)
                {
                    if (skill.MySkill.KeyID == "S_Angela_5")
                    {
                        this.BChar.MyTeam.Skills_UsedDeck.Remove(skill);
                    }
                }
            }
        }
    }

    // Vampirism
    public class S_Angela_6 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }
    
    // Oblivion
    public class S_Angela_7 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }

    // Eternally Lit Lamp
    public class S_Angela_8 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }

    // Baptism
    public class S_Angela_9 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            foreach (BattleChar b in this.BChar.MyTeam.AliveChars)
            {
                b.BuffAdd("B_Angela_9", this.BChar);
                //b.BuffAdd("B_Angela_9", this.BChar);
                //b.BuffAdd("B_Angela_9", this.BChar);
                //b.BuffAdd("B_Angela_9", this.BChar);
                //b.BuffAdd("B_Angela_9", this.BChar);
                //b.BuffAdd("B_Angela_9", this.BChar);
            }
        }
    }

    // Our Galaxy
    public class S_Angela_10 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            
            int additionalHits = SkillD.UsedApNum * 5;
            
            for (int i = 0; i < additionalHits; i++)
            {
                if (RandomManager.RandomPer(this.BChar.GetRandomClass().Main, 100, 80))
                {
                    BattleSystem.DelayInput(EnemyStar());
                }
                else
                {
                    BattleSystem.DelayInput(AllyStar());
                }
            }
        }

        public IEnumerator EnemyStar()
        {
            Skill skill = Skill.TempSkill("S_Angela_10_0", this.BChar, this.BChar.MyTeam);
            this.BChar.ParticleOut(this.MySkill, skill, BattleSystem.instance.EnemyTeam.AliveChars.Random());
            yield return null;
            yield break;
        }

        public IEnumerator AllyStar()
        {
            Skill skill = Skill.TempSkill("S_Angela_10_1", this.BChar, this.BChar.MyTeam);
            this.BChar.ParticleOut(this.MySkill, skill, BattleSystem.instance.AllyTeam.AliveChars.Random());
            BattleSystem.instance.AllyTeam.AP += 1;
            yield return null;
            yield break;
        }


    }

    // Additional hit enemy
    public class S_Angela_10_0 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.MySkill.PlusHit = true;
            this.SkillBasePlus.Target_BaseDMG = (int)(this.BChar.GetStat.reg * 0.35f);
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }

    // Powder of Life
    public class S_Angela_11 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
        }
    }


    /// Abnormality Pages
    /// 
    
    public class S_Angela_Lies : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Lies", this.BChar);
        }
    }

    public class S_Angela_Courage : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Courage", this.BChar);
        }
    }

    public class S_Angela_Vengeance : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Vengeance", this.BChar);
        }
    }

    public class S_Angela_Flutters : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Flutters", this.BChar);
        }
    }

    public class S_Angela_Ashes : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Ashes", this.BChar);
        }
    }

    public class S_Angela_Love : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Love", this.BChar);
        }
    }

    public class S_Angela_FerventBeats : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_FerventBeats", this.BChar);
        }
    }

    public class S_Angela_ChainedWrath : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_ChainedWrath", this.BChar);
        }
    }

    public class S_Angela_Obsession : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.LucyChar.BuffAdd("B_Angela_Obsession", this.BChar);
        }
    }

    public class S_Angela_Confession : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill temp = Skill.TempSkill("S_Angela_Confession_0", BattleSystem.instance.AllyTeam.LucyChar, this.BChar.MyTeam);
            this.BChar.MyTeam.Add(temp, true);
        }
    }

    public class S_Angela_Confession_0 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (BattleChar battleChar in Targets)
            {
                if (battleChar.HP < battleChar.Recovery)
                {
                    int num = battleChar.Recovery - battleChar.HP;
                    battleChar.Heal(this.BChar, (float)num, false, false, null);
                }
            }
        }
    }

    public class S_Angela_Incomprehensible : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Incomprehensible", this.BChar);
        }
    }

    public class S_Angela_Absorption : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Absorption", this.BChar);
        }
    }

    public class S_Angela_DarkFlame : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.LucyChar.BuffAdd("B_Angela_DarkFlame", this.BChar);
        }
    }

    public class S_Angela_Blizzard : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (b != Targets[0])
                {
                    b.BuffAdd("B_Angela_Blizzard", this.BChar);
                    b.BuffAdd("B_Angela_Blizzard", this.BChar);
                    b.BuffAdd("B_Angela_Blizzard", this.BChar);
                    b.BuffAdd("B_Angela_Blizzard", this.BChar);
                    b.BuffAdd("B_Angela_Blizzard", this.BChar);
                }
            }

            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                b.BuffAdd("B_Angela_Blizzard", this.BChar);
                b.BuffAdd("B_Angela_Blizzard", this.BChar);
                b.BuffAdd("B_Angela_Blizzard", this.BChar);
                b.BuffAdd("B_Angela_Blizzard", this.BChar);
                b.BuffAdd("B_Angela_Blizzard", this.BChar);
            }
        }
    }

    public class S_Angela_Loyalty : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (b != Targets[0])
                {
                    b.BuffAdd("B_Angela_Loyalty_Ally", this.BChar);
                }
                else
                {
                    b.BuffAdd("B_Angela_Loyalty", this.BChar);
                }
            }
        }
    }

    public class S_Angela_Shell : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_Shell", this.BChar);
        }
    }

    public class S_Angela_GooeyWaste : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd("B_Angela_GooeyWaste", this.BChar);
        }
    }

    public class S_Angela_MagicTrick : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill temp = Skill.TempSkill("S_Angela_MagicTrick_0", BattleSystem.instance.AllyTeam.LucyChar, this.BChar.MyTeam);
            this.BChar.MyTeam.Add(temp, true);
        }
    }

    public class S_Angela_MagicTrick_0 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);

            List<Skill> list = new List<Skill>();
            list.Add(Skill.TempSkill("S_Angela_MagicTrick_0_0", this.MySkill.Master, this.MySkill.Master.MyTeam));
            list.Add(Skill.TempSkill("S_Angela_MagicTrick_0_1", this.MySkill.Master, this.MySkill.Master.MyTeam));
            BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, false));
        }
        public void Del(SkillButton Mybutton)
        {
            // Reduce the cost of all skills in hand to 0
            if (Mybutton.Myskill.MySkill.KeyID == "S_Angela_MagicTrick_0_0")
            {
                foreach (Skill skill in BattleSystem.instance.AllyTeam.Skills)
                {
                    if (skill != this.MySkill)
                    {
                        Extended_Angela_MagicTrick extended = new Extended_Angela_MagicTrick();
                        skill.ExtendedAdd(extended);
                    }
                }
            }

            // Restore 5 mana
            if (Mybutton.Myskill.MySkill.KeyID == "S_Angela_MagicTrick_0_1")
            {
                BattleSystem.instance.AllyTeam.AP += 5;
            }
        }
    }

    public class Extended_Angela_MagicTrick : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.APChange = -99;
        }
    }

}
