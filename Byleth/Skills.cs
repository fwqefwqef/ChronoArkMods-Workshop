using GameDataEditor;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Byleth
{
    // Weapons of the Elite (Fixed)
    public class S_Byleth_0 : Skill_Extended
    {
        public override void Init()
        {
            //this.APChange = -99;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            // store all learned skills of the character in list
            List<CharInfoSkillData> learnedSkills = this.BChar.Info.SkillDatas;
            List<GDESkillData> list = new List<GDESkillData>();
            foreach (CharInfoSkillData skill in learnedSkills)
            {
                list.Add(skill.SkillInfo);
                //Debug.Log("Learned Skill: " + skill.SkillInfo.Key);
            }

            // retrieve all class skills of character and remove the ones that are already learned
            List<GDESkillData> characterSkillNoOverLap = PlayData.GetCharacterSkillNoOverLap(this.BChar.Info, false, null);
            var learnedKeyIds = new HashSet<string>(list.Select(s => s.KeyID));
            characterSkillNoOverLap.RemoveAll(x => learnedKeyIds.Contains(x.KeyID));
            //foreach (GDESkillData skill in characterSkillNoOverLap)
            //{
            //    Debug.Log("Available Skill: " + skill.Key);
            //}

            // randomly select 2 skills from the trimmed list and store them in list2
            List<GDESkillData> list2 = new List<GDESkillData>();
            list2.AddRange(characterSkillNoOverLap.Random(this.BChar.GetRandomClass().Main, 2));
            //foreach (GDESkillData skill in list2)
            //{
            //    Debug.Log("Randomly Selected Skill: " + skill.Key);
            //}

            // create tempskills for all skills in list2 and store them in list3
            List<Skill> list3 = new List<Skill>();
            foreach (GDESkillData gdeskillData2 in list2)
            {
                Skill skill = Skill.TempSkill(gdeskillData2.Key, this.BChar, this.BChar.MyTeam);
                skill.isExcept = true;
                skill.AutoDelete = 2;
                list3.Add(skill);
            }
            //foreach (Skill skill in list3)
            //{
            //    Debug.Log("Temp Skill: " + skill.MySkill.Key);
            //}
            // if there are any skills in list3, prompt the player to select one. If not, give the player 3 AP and draw 3 cards
            if (list3.Count > 0)
            {
                BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list3, new SkillButton.SkillClickDel(this.Skillbutton), ScriptLocalization.System_SkillSelect.CreateSkill, false, true, true, false, true));
            }
            else
            {
                BattleSystem.instance.AllyTeam.AP += 3;
                BattleSystem.instance.AllyTeam.Draw(3);
            }
        }
        public void Skillbutton(SkillButton mybutton)
        {
            BattleSystem.instance.AllyTeam.Add(mybutton.Myskill, true);
        }
    }
    // Haze Slice
    public class S_Byleth_1 : Skill_Extended
    {
        public override void Init()
        {
            //this.APChange = 0;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill skill = Skill.TempSkill("S_Byleth_1_1", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }
    // Sunder
    public class S_Byleth_1_1 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill skill = Skill.TempSkill("S_Byleth_1_2", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }
    // Windsweep
    public class S_Byleth_1_2 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (Targets[0] is BattleEnemy)
            {
                // If already at 9+ Action Count, stun 
                if ((Targets[0] as BattleEnemy).SkillQueue[0]._CastSpeed >= 9)
                {
                    Targets[0].BuffAdd(GDEItemKeys.Buff_B_Common_Rest, this.BChar, false, (int)(120f + this.BChar.GetStat.HIT_CC), false, -1, false);
                }

                // Otherwise, delay all actions by 99 Action Count
                else
                {
                    foreach (CastingSkill castingSkill in (Targets[0] as BattleEnemy).SkillQueue)
                    {
                        castingSkill._CastSpeed += 99;
                    }
                }
            }
        }
    }
    // Atrocity
    public class S_Byleth_2 : Skill_Extended, IP_DamageChange
    {
        int mana = 0;
        public override void Init()
        {
            this.OnePassive = true;
            this.APChange = -99;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            mana = BattleSystem.instance.AllyTeam.AP;
            BattleSystem.instance.AllyTeam.AP = 0;
        }
        public override void FixedUpdate() // Displayed Damage
        {
            this.PlusSkillPerFinal.Damage = 100 * 2 * BattleSystem.instance.AllyTeam.AP - 100;
            if (this.PlusSkillPerFinal.Damage < 0)
            {
                this.PlusSkillPerFinal.Damage = 0;
            }
        }
        // Actual damage change
        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (View)
            {
                return Damage;
            }
            this.PlusSkillPerFinal.Damage = 0;

            if (mana == 0)
            {
                return Damage;
            }
            else
            {
                return Damage * 2 * mana;
            }
        }
    }
    // Raging Storm
    public class S_Byleth_3 : Skill_Extended
    {
        public override void Init()
        {
            this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_1_Ex).Particle_Path;
        }

        public override void FixedUpdate()
        {
            if (BattleSystem.instance.EnemyTeam.AliveChars.Count == 1)
            {
                base.SkillParticleOn();
                this.PlusSkillStat.cri = 100f;
            }
            base.SkillParticleOff();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            int recasts = this.MySkill.UsedApNum;
            for (int i = 0; i < recasts; i++)
            {
                Skill skill = Skill.TempSkill("S_Byleth_3_0", this.BChar, this.BChar.MyTeam);
                BattleSystem.DelayInputAfter(this.Attack(Targets));
            }
        }
        public IEnumerator Attack(List<BattleChar> Targets)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            Skill skill = Skill.TempSkill("S_Byleth_3_0", this.BChar, this.BChar.MyTeam);
            skill.PlusHit = true;
            skill.FreeUse = true;

            if (BattleSystem.instance.EnemyList.Count != 0)
            {
                this.BChar.ParticleOut(this.MySkill, skill, Targets);
            }
            yield break;
        }
    }
    public class S_Byleth_3_0 : Skill_Extended, IP_DamageChange
    {
        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (BattleSystem.instance.EnemyTeam.AliveChars.Count == 1)
            {
                Cri = true;
            }
            return Damage;
        }
    }

    // Fallen Star
    public class S_Byleth_4 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            // Target same taunt status
            //if (Targets[0] is BattleEnemy)
            //{
            //    if ((Targets[0] as BattleEnemy).istaunt)
            //    {
            //        using (List<BattleEnemy>.Enumerator enumerator = BattleSystem.instance.EnemyList.GetEnumerator())
            //        {
            //            while (enumerator.MoveNext())
            //            {
            //                BattleEnemy battleEnemy = enumerator.Current;
            //                if (battleEnemy != Targets[0] && battleEnemy.istaunt)
            //                {
            //                    Targets.Add(battleEnemy);
            //                }
            //            }
            //            return;
            //        }
            //    }
            //    foreach (BattleEnemy battleEnemy2 in BattleSystem.instance.EnemyList)
            //    {
            //        if (battleEnemy2 != Targets[0] && !battleEnemy2.istaunt)
            //        {
            //            Targets.Add(battleEnemy2);
            //        }
            //    }
            //}
        }
    }
    // Fierce Iron Fist
    public class S_Byleth_5 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            //Skill skill = Skill.TempSkill("S_Byleth_5", this.BChar, this.BChar.MyTeam);
            //Skill skill2 = Skill.TempSkill("S_Byleth_5", this.BChar, this.BChar.MyTeam);

            //RandomClass randomClass = this.BChar.GetRandomClass().Main;
            ////int num = randomClass.RandomInt(0, BattleSystem.instance.AllyTeam.Skills_Deck.Count);
            ////BattleSystem.instance.AllyTeam.Skills_Deck.Insert(num, SkillD);

            //int num = randomClass.RandomInt(0, BattleSystem.instance.AllyTeam.Skills_UsedDeck.Count);
            //BattleSystem.instance.AllyTeam.Skills_UsedDeck.Insert(num, skill);
            //num = randomClass.RandomInt(0, BattleSystem.instance.AllyTeam.Skills_UsedDeck.Count);
            //BattleSystem.instance.AllyTeam.Skills_UsedDeck.Insert(num, skill2);

            //BattleSystem.instance.AllyTeam.Draw(1);
        }
        //public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        //{
        //    base.SkillUseSingle(SkillD, Targets);
        //    BattleSystem.DelayInput(this.Del());
        //}
        //private IEnumerator Del()
        //{
        //    int num;
        //    for (int i = 0; i < BattleSystem.instance.AllyTeam.Skills_UsedDeck.Count; i = num + 1)
        //    {
        //        yield return BattleSystem.instance.StartCoroutine(BattleSystem.instance.AllyTeam._UsedDeckToDeck(BattleSystem.instance.AllyTeam.Skills_UsedDeck[i]));
        //        num = i;
        //        i = num - 1;
        //        num = i;
        //    }
        //    BattleSystem.instance.AllyTeam.ShuffleDeck();
        //    //BattleSystem.instance.AllyTeam.AP += 1;
        //    BattleSystem.instance.AllyTeam.Draw(2);
        //    yield break;
        //}
    }
    // Wrath Strike
    public class S_Byleth_6 : Skill_Extended
    {
        public override void Init()
        {
            this.OnePassive = true;
        }
        public override void SkillKill(SkillParticle SP)
        {
            base.SkillKill(SP);
            BattleSystem.instance.AllyTeam.AP += this.MySkill.UsedApNum;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            Skill skill = Skill.TempSkill("S_Byleth_6", this.BChar, this.BChar.MyTeam);
            skill.AutoDelete = 1;
            skill.isExcept = true;
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }
    // Canto
    public class S_Byleth_7 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.AP += 4;
            BattleSystem.instance.AllyTeam.LucyChar.BuffAdd("B_Byleth_7", this.BChar, false, 0, false, 1, false);
        }
    }
    // Bolganone
    public class S_Byleth_8 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            //Debug.Log("Overload 1 more");
            this.BChar.Overload += 1;
        }
    }
    // Nosferatu
    public class S_Byleth_9 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
        public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
        {
            base.AttackEffectSingle(hit, SP, DMG, Heal);
            foreach (BattleChar battleChar in this.BChar.MyTeam.AliveChars)
            {
                battleChar.Heal(this.BChar, (float)DMG / 2, false, false, null);
            }
        }
    }
    // Ruptured Heaven
    public class S_Byleth_10 : Skill_Extended, IP_ParticleOut_After
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Buff b = this.BChar.BuffReturn("B_Byleth_10");
            if (b != null)
            {
                int num = (b as B_Byleth_10).UsedClassSkills.Count;
                this.PlusSkillPerFinal.Damage = (int)(100f * (1f + 1f * num) - 100f);
            }
        }

        public override void Init()
        {

        }

        public IEnumerator ParticleOut_After(Skill SkillD, List<BattleChar> Targets)
        {
            if (SkillD == this.MySkill)
            {
                Buff b = this.BChar.BuffReturn("B_Byleth_10");
                if (b != null)
                {
                    (b as B_Byleth_10).UsedClassSkills = new List<GDESkillData>();
                }
            }
            yield return null;
        }
    }
    // Divine Pulse
    public class S_Byleth_11 : Skill_Extended
    {
        public override void Init()
        {

        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            foreach (BattleChar battleChar in BattleSystem.instance.AllyTeam.AliveChars)
            {
                List<Buff> buffs = battleChar.GetBuffs(BattleChar.GETBUFFTYPE.ALLDEBUFF, true, false);
                if (buffs != null && buffs.Count > 0)
                {
                    buffs.Random(BattleRandom.PassiveItem).SelfDestroy(false);
                }
            }
            BattleSystem.DelayInput(this.Del());
        }
        private IEnumerator Del()
        {
            int num;
            for (int i = 0; i < BattleSystem.instance.AllyTeam.Skills_UsedDeck.Count; i = num + 1)
            {
                yield return BattleSystem.instance.StartCoroutine(BattleSystem.instance.AllyTeam._UsedDeckToDeck(BattleSystem.instance.AllyTeam.Skills_UsedDeck[i]));
                num = i;
                i = num - 1;
                num = i;
            }

            BattleSystem.instance.AllyTeam.ShuffleDeck();
            //BattleSystem.instance.AllyTeam.AP += 1;
            BattleSystem.instance.AllyTeam.Draw(1);
            yield break;
        }
    }

    public class S_Byleth_LucyD : Skill_Extended
    {
        public override void Init()
        {
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.Draw(4);
        }
    }

    public class SkillEn_Byleth_0 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);

            List<GDESkillData> characterSkillNoOverLap = PlayData.GetCharacterSkillNoOverLap(this.BChar.Info, false, null);
            GDESkillData data = characterSkillNoOverLap.Random(this.BChar.GetRandomClass().Main);

            Skill skill = Skill.TempSkill(data.Key, this.BChar, this.BChar.MyTeam);
            skill.isExcept = true;
            skill.AutoDelete = 2;
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }
    //public class SkillEn_Byleth_0 : Skill_Extended
    //{
    //    public override bool CanSkillEnforce(Skill MainSkill)
    //    {
    //        return !MainSkill.Disposable || MainSkill.isExcept;
    //    }

    //    // Token: 0x06001B51 RID: 6993 RVA: 0x00079741 File Offset: 0x00077941
    //    public override void Init()
    //    {
    //        base.Init();
    //        this.Disposable = true;
    //        this.PlusPerStat.Damage = 33;
    //        this.PlusPerStat.Heal = 33;
    //    }
    //}
}