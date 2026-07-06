using EItem;
using GameDataEditor;
using I2.Loc;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = System.Random;
using BasicMethods;

namespace Chihiro
{
    // Kuro : Shitten
    public class S_Chihiro_1 : Skill_Extended
    {
        public override void SkillTargetSingle(List<Skill> Targets)
        {
            Targets[0].isExcept = true;
            Targets[0].Delete(false);
            List<Skill> list = new List<Skill>();
            Skill skill = Skill.TempSkill("S_Chihiro_1_0", this.BChar, BattleSystem.instance.AllyTeam);
            Skill skill2 = Skill.TempSkill("S_Chihiro_1_1", this.BChar, BattleSystem.instance.AllyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
            BattleSystem.instance.AllyTeam.Add(skill2, true);
        }
    }
    // Kuro : Shissen
    public class S_Chihiro_1_0 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.Draw(1);
        }
    }
    // Kuro : Shitten (recast)
    public class S_Chihiro_1_1 : Skill_Extended
    {
        public override void SkillTargetSingle(List<Skill> Targets)
        {
            Targets[0].isExcept = true;
            Targets[0].Delete(false);
            List<Skill> list = new List<Skill>();
            Skill skill = Skill.TempSkill("S_Chihiro_1_0", this.BChar, BattleSystem.instance.AllyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }

    // Kuro : Chigiri
    public class S_Chihiro_2 : Skill_Extended
    {
        public override void SkillUseSingleAfter(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingleAfter(SkillD, Targets);
            this.flag = true;
            BattleSystem.DelayInput(this.Delay());
        }

        public List<Skill> SkillList = new List<Skill>();
        public int count;
        private bool flag;
        private void Del(SkillButton Myskill)
        {
            //Debug.Log("Myskill key: " + Myskill.Myskill.MySkill.KeyID);
            //Debug.Log("Count : " + count);

            if (Myskill.Myskill.MySkill.KeyID == "S_Chihiro_Skip")
            {
                this.flag = true;
                return;
            }

            SkillList.Remove(Myskill.Myskill);
            BattleSystem.instance.AllyTeam.Skills_Deck.Remove(Myskill.Myskill);
            count--;

            bool hasMoreSelections = count > 0 && SkillList.Exists(skill => skill.MySkill.KeyID != "S_Chihiro_Skip");
            if (hasMoreSelections)
            {
                this.flag = false;
                BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(SkillList, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.Next1SKillView, false, true, true, false, true));
            }
            else
            {
                this.flag = true;
            }
        }
        public IEnumerator Delay()
        {
            yield return new WaitForFixedUpdate();
            List<Skill> list = new List<Skill>();
            list.AddRange(BattleSystem.instance.AllyTeam.Skills_Deck);
            List<Skill> list2 = new List<Skill>();
            int num = 0;
            while (num < 3 && list.Count > num)
            {
                list2.Add(list[num]);
                num++;
            }
            if (list2.Count == 0)
            {
                yield break;
            }
            Skill skill = Skill.TempSkill("S_Chihiro_Skip", this.BChar, this.BChar.MyTeam);
            list2.Add(skill);

            this.flag = false;
            SkillList = list2;
            count = 3;
            BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list2, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.Next1SKillView, false, true, true, false, true));
            BattleSystem.DelayInputAfter(this.Cast());
            yield break;
        }

        // Token: 0x0600138D RID: 5005 RVA: 0x0009BBFA File Offset: 0x00099DFA
        public IEnumerator Cast()
        {
            yield return new WaitUntil(() => this.flag);
            yield return new WaitForFixedUpdate();
            int recastCount = 3 - count;
            //Debug.Log("Recast Count : " + recastCount);
            for (int i = 0; i < recastCount; i++)
            {
                BattleSystem.DelayInput(this.Ienum());
            }
            yield break;
        }

        // Token: 0x0600138E RID: 5006 RVA: 0x0009BC10 File Offset: 0x00099E10
        public IEnumerator Ienum()
        {
            //Debug.Log("Ienum");
            Skill skill = Skill.TempSkill("S_Chihiro_2_0", this.BChar, this.BChar.MyTeam);
            skill.PlusHit = true;
            skill.FreeUse = true;

            this.BChar.ParticleOut(this.MySkill, skill, BattleSystem.instance.EnemyTeam.AliveChars);
            yield return new WaitForSecondsRealtime(0.2f);
            yield break;
        }
    }
    // Recast
    public class S_Chihiro_2_0 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
    // Kuro : Kokusai
    public class S_Chihiro_3 : Skill_Extended, IP_DamageChange
    {
        public int count = 0;
        public double scaling = 0.5;

        public override void Init()
        {
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            count = 0;
            for (int i = BattleSystem.instance.AllyTeam.Skills.Count - 1; i >= 0; i--)
            {
                if (BattleSystem.instance.AllyTeam.Skills[i] == this.MySkill)
                {

                }
                else
                {
                    BattleSystem.instance.AllyTeam.Skills[i].isExcept = true;
                    BattleSystem.instance.AllyTeam.Skills[i].Delete(true);
                    count++;
                    //BattleSystem.instance.AllyTeam.Skills.Remove(BattleSystem.instance.AllyTeam.Skills[i]);
                }
            }
            //this.SkillBasePlus.Target_BaseDMG = count * (int)(this.BChar.GetStat.atk * scaling);
            this.BChar.BuffAdd("B_Chihiro_3", this.BChar);
            (this.BChar.BuffReturn("B_Chihiro_3") as B_Chihiro_3).num = count;
        }

        public override void FixedUpdate()  
        {
            if (!this.MySkill.IsNowCasting)
            {
                if (this.MySkill.BasicSkill)
                {
                    this.SkillBasePlus.Target_BaseDMG = BattleSystem.instance.AllyTeam.Skills.Count * (int)(this.BChar.GetStat.atk * scaling);
                }
                else
                {
                    this.SkillBasePlus.Target_BaseDMG = (BattleSystem.instance.AllyTeam.Skills.Count - 1) * (int)(this.BChar.GetStat.atk * scaling);
                }

                if (this.SkillBasePlus.Target_BaseDMG < 0)
                {
                    this.SkillBasePlus.Target_BaseDMG = 0;
                }
            }
        }
        public override string DescExtended(string desc)
        {
            if (this.MySkill.BasicSkill)
            {
                return base.DescExtended(desc).Replace("&a", ((int)(this.BChar.GetStat.atk * scaling)).ToString());
            }
            return base.DescExtended(desc).Replace("&a", ((int)(this.BChar.GetStat.atk * scaling)).ToString());
        }

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (!View)
            {
                this.SkillBasePlus.Target_BaseDMG = 0;
                //Debug.Log("additional damage: " + (count) * (int)(this.BChar.GetStat.atk * scaling));
                return Damage + (count) * (int)(this.BChar.GetStat.atk * scaling);
            }
            return Damage;
        }

        //public void DamageChange_sumoperation(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View, ref int PlusDamage)
        //{
        //    PlusDamage = count * (int)(this.BChar.GetStat.atk * scaling);
        //}
    }
    // Tobimune : Fukuro
    public class S_Chihiro_4 : Skill_Extended
    {
        int charges = 3;
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            for (int i = 0; i < charges; i++)
            {
                BattleSystem.DelayInputAfter(this.Effect(charges - i));
            }
        }
        public IEnumerator Effect(int Num)
        {
            List<Skill> list = new List<Skill>();
            for (int i = 0; i < BattleSystem.instance.AllyTeam.Skills_Deck.Count; i++)
            {
                list.Add(BattleSystem.instance.AllyTeam.Skills_Deck[i]);
            }
            if (list.Count == 0)
            {
                
            }
            else
            {
                BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.TW_Blue_R1.Replace("&a", Num.ToString()), true, true, true, false, true));
            }
            yield return null;
            yield break;
        }
        public void Del(SkillButton skillButton)
        {
            BattleSystem.instance.AllyTeam.Skills_Deck.Remove(skillButton.Myskill);
            BattleSystem.instance.AllyTeam.Skills_Deck.Insert(0, skillButton.Myskill);
        }
    }
    // Aka : Shinku no Kyō
    public class S_Chihiro_5 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
    // Nishiki : Ryūsai
    public class S_Chihiro_6 : Skill_Extended
    {
        public int charges = 3;
        public bool skipped = false;
        private List<Skill> shuffledSelectionOrder = new List<Skill>();
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            shuffledSelectionOrder = new List<Skill>();
            for (int i = 0; i < BattleSystem.instance.AllyTeam.Skills_Deck.Count; i++)
            {
                Skill deckSkill = BattleSystem.instance.AllyTeam.Skills_Deck[i];
                if (deckSkill.MySkill.KeyID != "S_Chihiro_6_0")
                {
                    shuffledSelectionOrder.Add(deckSkill);
                }
            }
            shuffledSelectionOrder = Shuffle(shuffledSelectionOrder);
            for (int i = 0; i < charges; i++)
            {
                BattleSystem.DelayInputAfter(this.Effect(charges - i));
            }
            charges = 3;
            skipped = false;
        }
        public static List<Skill> Shuffle(List<Skill> values)
        {
            Random rand = new Random();
            var shuffled = values.OrderBy(_ => rand.Next()).ToList();
            return shuffled;
        }
        public IEnumerator Effect(int Num)
        {
            if (skipped)
            {
                yield return null;
                yield break;
            }
            List<Skill> list = shuffledSelectionOrder.FindAll(skill => BattleSystem.instance.AllyTeam.Skills_Deck.Contains(skill));
            Skill skip = Skill.TempSkill("S_Chihiro_Skip", this.BChar, this.BChar.MyTeam);
            list.Insert(list.Count, skip);

            if (list.Count == 0)
            {
                
            }
            else
            {
                BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), "Choose a skill to move to the discard pile (&a remaining)".Replace("&a", Num.ToString()), true, true, true, false, true));
            }
            yield return null;
            yield break;
        }
        public void Del(SkillButton skillButton)
        {
            if (skillButton.Myskill.MySkill.KeyID == "S_Chihiro_Skip")
            {
                skipped = true;
                return;
            }

            int index = 0;
            for (int i = 0; i < BattleSystem.instance.AllyTeam.Skills_Deck.Count; i++)
            {
                Skill s = BattleSystem.instance.AllyTeam.Skills_Deck[i];
                if (s.MySkill.KeyID == skillButton.Myskill.MySkill.KeyID)
                {
                    index = i;
                    break;
                }
            }
            Skill skill = Skill.TempSkill("S_Chihiro_6_0", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Skills_Deck.Insert(index, skill);
            BattleSystem.instance.AllyTeam.Skills_Deck.Remove(skillButton.Myskill);
            BattleSystem.instance.AllyTeam.Skills_UsedDeck.Insert(0, skillButton.Myskill);
        }
    }

    // Nishiki : Ryusen
    public class S_Chihiro_6_0 : Skill_Extended
    {
        public override IEnumerator DrawAction()
        {
            BattleChar target = BattleSystem.instance.EnemyTeam.AliveChars[0];
            Buff b = this.BChar.BuffReturn("B_Chihiro_Tracker");
            if (b != null)
            {
                target = (b as B_Chihiro_Tracker).recent;
            }

            BattleSystem.DelayInput(BattleSystem.instance.ForceAction(this.MySkill, target, false, false, true));
            return base.DrawAction();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.Draw(1);
        }
    }
    // Nishiki : Kagura 
    public class S_Chihiro_7 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            List<Skill> drawPile = BattleSystem.instance.AllyTeam.Skills_Deck;
            List<Skill> discardPile = BattleSystem.instance.AllyTeam.Skills_UsedDeck;
            Skill skill;
            int count = 0;
            for (int i = drawPile.Count-1; i >= 0; i--)
            {
                skill = drawPile[i];
                drawPile.Remove(skill);
                discardPile.Insert(0, skill);
                count++;
            }
            count = count / 3;
            if (count > 7)
            {
                count = 7;
            }
            for (int i = 0; i < count; i++)
            {
                skill = Skill.TempSkill("S_Chihiro_6_0", this.BChar, this.BChar.MyTeam);
                BattleSystem.instance.AllyTeam.Skills_UsedDeck.Insert(0, skill);
            }
        }
    }
    // Iai : Byakkei-ryu
    public class S_Chihiro_8 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills_Deck)
            {
                if (s.MySkill.KeyID == "S_Chihiro_8_0")
                {
                    return;
                }
            }
            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills_UsedDeck)
            {
                if (s.MySkill.KeyID == "S_Chihiro_8_0")
                {
                    return;
                }
            }

            Skill skill = Skill.TempSkill("S_Chihiro_8_0", this.BChar, this.BChar.MyTeam);
            int index = this.BChar.GetRandomClass().Main.RandomInt(0,BattleSystem.instance.AllyTeam.Skills_Deck.Count);
            BattleSystem.instance.AllyTeam.Skills_Deck.Insert(index, skill);

            if (!this.BChar.BuffFind("B_Bakusen"))
            {
                this.BChar.BuffAdd("B_Bakusen", this.BChar);
            }
        }
    }
    // Iai : Bakusen
    public class S_Chihiro_8_0 : Skill_Extended
    {
        //int turnCreated = 0;
        public override void Init()
        {
            base.Init();
            this.PlusSkillStat.Penetration = 100f;
        }
        public override IEnumerator DrawAction()
        {
            BattleChar target = BattleSystem.instance.EnemyTeam.AliveChars[0];
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                if (b.HP > target.HP)
                {
                    target = b;
                }
            }
            BattleSystem.DelayInput(BattleSystem.instance.ForceAction(this.MySkill, target, false, false, true));
            return base.DrawAction();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
        }
    }

    public class B_Bakusen : Buff, IP_TurnEndButtonEnemy, IP_SkillUse_User_After, IP_DamageChange
    {
        int count = 1;

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (SkillD.MySkill.KeyID == "S_Chihiro_8_0")
            {
                return Damage * count;
            }
            return Damage;
        }

        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", count.ToString());
        }
        public override void Init()
        {
            this.OnePassive = true;
        }
        public void SkillUseAfter(Skill SkillD)
        {
            if (SkillD.MySkill.KeyID == "S_Chihiro_8_0")
            {
                this.SelfDestroy();
            }
        }
        public void TurnEndButtonEnemy()
        {
            count++;
        }
    }

    // Kuregumo : Mei
    public class S_Chihiro_9 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill prioritizedSkill = this.BChar.MyTeam.Skills_Deck.Find((Skill a) => a.Master == this.BChar);
            if (prioritizedSkill != null)
            {
                this.BChar.MyTeam.Draw(prioritizedSkill, null);
            }
            else
            {
                this.BChar.MyTeam.Draw(1);
            }
            this.BChar.Overload = 0;
        }
    }
    // Kuro : Shokkai
    public class S_Chihiro_R1 : Skill_Extended, IP_DamageChange
    {
        double scaling = 0.25;
        int limit = 0;

        public override void Init()
        {
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (limit < 10)
            {
                limit++;
                return;
            }

            //Debug.Log("Update");
            limit = 0;

            int numCostZero = 0;
            int numCostNonZero = 0;

            List<Skill> excludeDeck = BV_ExceptDeck.TryGetExcptedSkills();
            for (int i = 0; i < excludeDeck.Count; i++)
            {
                if (excludeDeck[i].AP == 0)
                {
                    numCostZero++;
                }
                else
                {
                    numCostNonZero++;
                }
            }
            this.SkillBasePlus.Target_BaseDMG = numCostZero * 1 + numCostNonZero * (int)(this.BChar.GetStat.atk * scaling);
        }

        public override string DescExtended(string desc)
        {
            return base.DescExtended(desc).Replace("&a", ((int)(this.BChar.GetStat.atk * scaling)).ToString());
        }

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (BattleSystem.instance.EnemyTeam.AliveChars.Count == 1)
            {
                Cri = true;
            }
            return Damage;
        }
    }
    // Magatsumi
    public class S_Chihiro_R2 : Skill_Extended //, IP_ParticleOut_After
    {
        public bool failed = false;
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);

            int overload = this.BChar.Overload;
            if (this.MySkill.NotCount)
            {
                
            }
            else
            {
                overload -= 1;
            }
            int success = 33 * (1 + overload);
            int roll = this.BChar.GetRandomClass().Main.RandomInt(0, 101);
            //Debug.Log("Success Chance: " + success);
            //Debug.Log("Rolled: " + roll);
            //roll = 0; // Debug
            if (roll < success) // Success
            {
                List<Skill> list = new List<Skill>();
                list.Add(Skill.TempSkill("S_Chihiro_R2_0", this.MySkill.Master, this.MySkill.Master.MyTeam));
                list.Add(Skill.TempSkill("S_Chihiro_R2_1", this.MySkill.Master, this.MySkill.Master.MyTeam));
                list.Add(Skill.TempSkill("S_Chihiro_R2_2", this.MySkill.Master, this.MySkill.Master.MyTeam));
                list.Add(Skill.TempSkill("S_Chihiro_R2_3", this.MySkill.Master, this.MySkill.Master.MyTeam));
                list.Add(Skill.TempSkill("S_Chihiro_R2_4", this.MySkill.Master, this.MySkill.Master.MyTeam));
                list.Add(Skill.TempSkill("S_Chihiro_R2_5", this.MySkill.Master, this.MySkill.Master.MyTeam));
                BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, false));
            }
            else // Fail
            {
                // Create copy of skill
                Skill skill = Skill.TempSkill("S_Chihiro_R2", this.MySkill.Master, this.MySkill.Master.MyTeam);
                skill.isExcept = true;
                skill.AutoDelete = 1;
                BattleSystem.instance.AllyTeam.Add(skill, true);
                this.BChar.Damage(this.BChar, 6, false, true, false, 0, false, false, false);
                //failed = true;
            }
        }
        public void Del(SkillButton Mybutton)
        {
            if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_R2_0")
            {
                BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Chihiro_R2_0", this.MySkill.Master, this.MySkill.Master.MyTeam), true);
            }
            else if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_R2_1")
            {
                BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Chihiro_R2_1", this.MySkill.Master, this.MySkill.Master.MyTeam), true);
            }
            else if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_R2_2")
            {
                BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Chihiro_R2_2", this.MySkill.Master, this.MySkill.Master.MyTeam), true);
            }
            else if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_R2_3")
            {
                BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Chihiro_R2_3", this.MySkill.Master, this.MySkill.Master.MyTeam), true);
            }
            else if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_R2_4")
            {
                BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Chihiro_R2_4", this.MySkill.Master, this.MySkill.Master.MyTeam), true);
            }
            else if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_R2_5")
            {
                BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Chihiro_R2_5", this.MySkill.Master, this.MySkill.Master.MyTeam), true);
            }
        }

        //public IEnumerator ParticleOut_After(Skill SkillD, List<BattleChar> Targets)
        //{
        //    if (failed)
        //    {
        //        // Take pain damage
        //        this.BChar.Damage(this.BChar, 6, false, true, false, 0, false, false, false);
        //        failed = false;
        //    }
        //    yield return null;
        //    yield break;
        //}
    }
    // Kumo Spider
    public class S_Chihiro_R2_0 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (this.BChar.BuffReturn("B_Chihiro_R2") == null)
            {
                this.BChar.BuffAdd("B_Chihiro_R2", this.BChar);
            }
        }

    }
    // Tonbo Dragonfly
    public class S_Chihiro_R2_1 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (this.BChar.BuffReturn("B_Chihiro_R2") == null)
            {
                this.BChar.BuffAdd("B_Chihiro_R2", this.BChar);
            }
        }
    }
    // Hachi Bee
    public class S_Chihiro_R2_2 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (this.BChar.BuffReturn("B_Chihiro_R2") == null)
            {
                this.BChar.BuffAdd("B_Chihiro_R2", this.BChar);
            }
        }
    }
    // Mukade Centipede
    public class S_Chihiro_R2_3 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (this.BChar.BuffReturn("B_Chihiro_R2") == null)
            {
                this.BChar.BuffAdd("B_Chihiro_R2", this.BChar);
            }

            this.SkillBasePlus.Target_BaseDMG = -9999;

            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                if (b is BattleEnemy && !(b as BattleEnemy).Boss)
                {
                    List<Buff> list = new List<Buff>();
                    foreach (Buff buff in b.Buffs)
                    {
                        if (!buff.BuffData.Debuff && !buff.BuffData.Hide && !(buff.BuffData.Key == "B_EnemyTaunt"))
                        {
                            list.Add(buff);
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        b.BuffRemove(list[i].BuffData.Key);
                    }
                }
            }
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                BattleSystem.DelayInput(this.Attack(b));
            }
        }
        public IEnumerator Attack(BattleChar b)
        {
            Skill skill = Skill.TempSkill("S_Chihiro_R2_3_0", this.MySkill.Master, this.MySkill.Master.MyTeam);
            skill.FreeUse = true;
            skill.PlusHit = true;

            this.BChar.ParticleOut(this.MySkill, skill, b);
            yield return new WaitForSecondsRealtime(0.1f);
            yield break;
        }
    }
    // Chō Butterfly
    public class S_Chihiro_R2_4 : Skill_Extended, IP_DamageChange
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusSkillStat.Penetration = 100f;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (this.BChar.BuffReturn("B_Chihiro_R2") == null)
            {
                this.BChar.BuffAdd("B_Chihiro_R2", this.BChar);
            }
        }
        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (BattleSystem.instance.EnemyTeam.AliveChars.Count == 1)
            {
                Cri = true;
            }
            return Damage;
        }
    }
    // Kodoku Malediction
    public class S_Chihiro_R2_5 : Skill_Extended
    {
        public override bool Terms()
        {
            bool condition = false;
            Buff b = this.BChar.BuffReturn("B_Chihiro_R2");
            if (b!= null)
            {
                //Debug.Log((b as B_Chihiro_R2).Magatsumi.Count);
                condition = (b as B_Chihiro_R2).Magatsumi.Count == 5;
            }
            return condition;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                b.Damage(this.BChar, 9999, false, true, false, 0, false, false, false);
            }
        }
    }
    // Master Smith
    public class S_Chihiro_LucyD : Skill_Extended
    {

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            List<Skill> list = new List<Skill>();
            list.Add(Skill.TempSkill("S_Chihiro_LucyD_0", this.MySkill.Master, this.MySkill.Master.MyTeam));
            list.Add(Skill.TempSkill("S_Chihiro_LucyD_1", this.MySkill.Master, this.MySkill.Master.MyTeam));
            BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, false));
        }

        // Token: 0x06000B08 RID: 2824 RVA: 0x0007F548 File Offset: 0x0007D748
        public void Del(SkillButton Mybutton)
        {
            if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_LucyD_0")
            {
                BattleSystem.instance.AllyTeam.Draw(2);
            }
            if (Mybutton.Myskill.MySkill.KeyID == "S_Chihiro_LucyD_1")
            {
                // Bring skill from exclude pile back into hand, thanks Alezy
                List<Skill> excDeck = Enumerable.ToList<Skill>(Enumerable.Where<Skill>(BV_ExceptDeck.TryGetExcptedSkills(), (Skill sk) => !sk.Master.IsDead && !sk.MySkill.Rare));
                if (excDeck.Count > 0)
                {
                    BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(excDeck, delegate (SkillButton skillbutton)
                    {
                        BV_ExceptDeck.RemoveSkill(skillbutton.Myskill);
                        BattleSystem.instance.AllyTeam.Add(skillbutton.Myskill, true);
                    }, "Choose a skill to bring back to hand.", false, true, true, false, true));
                }

                //List<Skill> discardPile = BattleSystem.instance.AllyTeam.Skills_UsedDeck;
                //for (int i=0;i< discardPile.Count; i++)
                //{
                //    if (discardPile[i].MySkill.KeyID == "S_Chihiro_LucyD")
                //    {
                //        discardPile.RemoveAt(i);
                //        //Debug.Log("Removed!");
                //        break;
                //    }
                //}
            }
        }
    }
    public class SkillEn_Chihiro_0 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.Disposable = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            BattleSystem.instance.AllyTeam.DiscardCount += 2;
        }
    }
}

