using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.Plugin;
using ChronoArkMod.Template;
using Debug = UnityEngine.Debug;
using ChronoArkMod.ModData;
using Spine;
using NLog.Targets;

namespace Argalia
{
    // Largo
    public class S_Argalia_1 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            // Increase Standby Count by 2
            BattleSystem.instance.AllyTeam.WaitCount+=2;

            // Delay Action Count by 1
            if (Targets[0] is BattleEnemy)
            {
                foreach (CastingSkill castingSkill in (Targets[0] as BattleEnemy).SkillQueue)
                {
                    castingSkill._CastSpeed += 1;
                }
            }
        }
    }

    // Allegro
    public class S_Argalia_2 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            // Resonance: Draw 2
            if (P_Argalia.IsResonance(SkillD, Targets[0]))
            {
                BattleSystem.instance.AllyTeam.Draw(2);
            }

            this.BChar.BuffAdd("B_Argalia_LucyD", this.BChar);

            // Delay all enemies' action count by 2 and gain 1 standby
            //BattleSystem.instance.AllyTeam.WaitCount += 1;
            //foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            //{
            //    foreach (CastingSkill castingSkill in (b as BattleEnemy).SkillQueue)
            //    {
            //        castingSkill._CastSpeed += 2;
            //        //Debug.Log("Cast Speed Update");
            //    }
            //}
        }
    }

    // Tempestuous Danza
    public class S_Argalia_3 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            List<BattleChar> extraTargets = new List<BattleChar>();
            foreach (BattleChar b in Targets)
            {
                if (P_Argalia.IsResonance(SkillD, b))
                {
                    extraTargets.Add(b);
                }
            }

            // Additional hits
            BattleSystem.DelayInput(this.Attack(extraTargets));

            // Add more barrier for each Resonance proc
            if (extraTargets.Count == 0) return;
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                b.BuffAdd("B_Argalia_Barrier", this.BChar);
                Buff buff = b.BuffReturn("B_Argalia_Barrier");
                if (buff != null)
                {
                    buff.BarrierHP += extraTargets.Count * (int)(this.BChar.GetStat.maxhp * 0.16);
                }
            }
        }

        public IEnumerator Attack(List<BattleChar> Targets)
        {
            Debug.Log("Attack");
            yield return new WaitForSeconds(0.2f);
            Skill skill = Skill.TempSkill("S_Argalia_3_0", this.BChar, this.BChar.MyTeam);
            skill.PlusHit = true;
            foreach (BattleChar Target in Targets)
            {
                if (Target.IsDead && BattleSystem.instance.EnemyTeam.AliveChars.Count != 0)
                {
                    this.BChar.ParticleOut(this.MySkill, skill, BattleSystem.instance.EnemyTeam.AliveChars.Random(this.BChar.GetRandomClass().Main));
                }
                else
                {
                    this.BChar.ParticleOut(this.MySkill, skill, Target);
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield break;
        }

        // On hit gain barrier
        public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
        {
            base.AttackEffectSingle(hit, SP, DMG, Heal);
            foreach(BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                b.BuffAdd("B_Argalia_Barrier", this.BChar);
                Buff buff = b.BuffReturn("B_Argalia_Barrier");
                if (buff != null)
                {
                    buff.BarrierHP += (int)(this.BChar.GetStat.maxhp * 0.16);
                }
            }
        }
        public override string DescExtended(string desc)
        {
            return base.DescExtended(desc).Replace("&a", ((int)(this.BChar.GetStat.maxhp * 0.16)).ToString())
                .Replace("&b", ((int)(this.BChar.GetStat.atk * 0.8)).ToString())
                .Replace("&c", ((int)(this.BChar.GetStat.maxhp * 0.16)).ToString());
        }
    }

    // Controlled Resonance
    public class S_Argalia_4 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }

    // Resonate
    public class S_Argalia_5 : Skill_Extended, IP_DamageChange
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            //if (P_Argalia.IsResonance(SkillD, Targets[0]))
            //{

            //    Targets[0].BuffAdd("B_Common_Rest", this.BChar, false, 110, false, -1, false);
            //    Buff b = Targets[0].BuffReturn("B_Argalia_P");
            //    if (b != null)
            //    {
            //        int stacks = b.StackNum;
            //        Targets[0].Damage(this.BChar, (int)(this.BChar.GetStat.maxhp * 0.5), false, true, false, 0, false, false, false);
            //        Targets[0].BuffRemove("B_Argalia_P");
            //    }
            //}
        }

        public override string DescExtended(string desc)
        {
            return base.DescExtended(desc).Replace("&a", (this.BChar.GetStat.maxhp*0.5).ToString());
        }

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (!View)
            {
                if (P_Argalia.IsResonance(SkillD, Target))
                {
                    // Pull action count to 0
                    int speed = -99;
                    if (Target is BattleEnemy)
                    {
                        foreach (CastingSkill castingSkill in (Target as BattleEnemy).SkillQueue)
                        {
                            if (speed == -99)
                            {
                                speed = castingSkill.CastSpeed;
                                //if (!this.MySkill.NotCount) // Offset for non swift cast
                                //{
                                //    speed -= 1;
                                //}
                            }
                            castingSkill._CastSpeed -= speed;
                        }
                    }
                    // Force action
                    if (this.MySkill.NotCount)
                    {
                        BattleSystem.instance.StartCoroutine(BattleSystem.instance.EnemyTurn(false));
                    }
                    return Damage * 2;
                }
            }
            return Damage;
        }
    }

    // Trails of Blue
    public class S_Argalia_6 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            this.BChar.BuffAdd("B_Argalia_Barrier", this.BChar);
            Buff buff = this.BChar.BuffReturn("B_Argalia_Barrier");
            if (buff != null)
            {
                buff.BarrierHP += (int)(this.BChar.GetStat.maxhp * 1);
            }
            if (P_Argalia.IsResonance(SkillD, Targets[0]))
            {
                BattleSystem.instance.AllyTeam.WaitCount += 2;
                Targets[0].BuffAdd("B_Argalia_6", this.BChar, false, 999, false, -1, false);
            }
        }
        public override string DescExtended(string desc)
        {
            return base.DescExtended(desc).Replace("&a", ((int)(this.BChar.GetStat.maxhp * 1)).ToString());
        }
    }

    // Dissonance
    public class S_Argalia_7 : Skill_Extended
    {
        public override void Init()
        {
            this.CanUseStun = true;
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            int count = 0;

            // Cleanse CC and Weak debuff
            foreach (BattleChar hit in Targets)
            {
                for (int i = 0; i < hit.Buffs.Count; i++)
                {
                    if (hit.Buffs[i].BuffData.BuffTag.Key == GDEItemKeys.BuffTag_Debuff && !hit.Buffs[i].CantDisable)
                    {
                        hit.Buffs[i].SelfDestroy(false);
                        count++;
                    }
                    else if (hit.Buffs[i].BuffData.BuffTag.Key == GDEItemKeys.BuffTag_CrowdControl && !hit.Buffs[i].CantDisable)
                    {
                        hit.Buffs[i].SelfDestroy(false);
                        count++;
                    }
                }
            }

            // Apply vibration for each debuff removed
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                for (int i=0; i < count; i++)
                {
                    b.BuffAdd("B_Argalia_P", this.BChar);
                }
            }
        }
    }

    // Preludio
    public class S_Argalia_8 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }

    // Assolo
    public class S_Argalia_9 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (P_Argalia.IsResonance(SkillD, Targets[0]))
            {
                Targets[0].BuffAdd("B_Taunt", this.BChar, false, 110, false, -1, false);

                // Delay Action Count by 3
                if (Targets[0] is BattleEnemy)
                {
                    foreach (CastingSkill castingSkill in (Targets[0] as BattleEnemy).SkillQueue)
                    {
                        castingSkill._CastSpeed += 3;
                    }
                }
            }
        }
    }

    // Crescendo
    public class S_Argalia_10 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            int avgCount = 0;
            int numActed = 0;
            foreach (BattleChar b in Targets)
            {
                if (b is BattleEnemy)
                {
                    foreach (CastingSkill castingSkill in (b as BattleEnemy).SkillQueue)
                    {
                        avgCount += castingSkill.CastSpeed;
                        numActed++;
                        break; // Check only first
                    }
                }
            }

            // Bump everyone's action counts
            avgCount /= numActed;
            int offset = -99;
            foreach (BattleChar b in Targets)
            {
                offset = -99;
                if (b is BattleEnemy)
                {
                    foreach (CastingSkill castingSkill in (b as BattleEnemy).SkillQueue)
                    {
                        if (offset == -99)
                        {
                            offset = avgCount - castingSkill.CastSpeed;
                        }
                        castingSkill._CastSpeed += offset;
                    }
                }
            }

            Skill skill = Skill.TempSkill("S_Argalia_10_0", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }

    // Grand Finale
    public class S_Argalia_10_0 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            List<BattleChar> destroyTargets = new List<BattleChar>();
            foreach (BattleChar b in Targets)
            {
                if (P_Argalia.IsResonance(SkillD, b))
                {
                    if (b is BattleEnemy)
                    {
                        destroyTargets.Add(b);
                    }
                }
            }

            // destroy 1 action count of resonated enemies
            //foreach (CastingSkill c in BattleSystem.instance.EnemyCastSkills)
            //{
            //    if (destroyTargets.Contains(c.Usestate)) {
            //        BattleSystem.instance.EnemyCastSkills.Remove(c);
            //        BattleSystem.instance.ActWindow.CastingWaste(c);
            //        destroyTargets.Remove(c.Usestate);
            //        Debug.Log("Removed");
            //    }
            //}

            foreach (BattleChar b in destroyTargets)
            {
                b.BuffAdd("B_Common_Rest", this.BChar, false, 140, false, -1, false);
            }
        }

        public override string DescExtended(string desc)
        {
            return base.DescExtended(desc).Replace("&a", (140 + this.BChar.GetStat.HIT_CC).ToString());
        }
    }

    // Impromptu
    public class S_Argalia_11 : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            // 2 Copies of Impromptu Dance
            Skill skill = Skill.TempSkill("S_Argalia_11_0", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
            Skill skill2 = Skill.TempSkill("S_Argalia_11_0", this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(skill2, true);
        }
    }

    // Impromptu Dance
    public class S_Argalia_11_0 : Skill_Extended
    {
        bool notcount = true;
        List<BattleChar> targets = new List<BattleChar>();
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            targets = Targets;
            List<Skill> list = new List<Skill>();
            notcount = this.MySkill.NotCount;
            list.Add(Skill.TempSkill("S_Argalia_11_1", this.MySkill.Master, this.MySkill.Master.MyTeam));
            list.Add(Skill.TempSkill("S_Argalia_11_2", this.MySkill.Master, this.MySkill.Master.MyTeam));
            BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, false));
        }
        public void Del(SkillButton Mybutton)
        {
            if (Mybutton.Myskill.MySkill.KeyID == "S_Argalia_11_1")
            {
                foreach (BattleChar b in targets)
                {
                    if (b is BattleEnemy)
                    {
                        foreach (CastingSkill castingSkill in (b as BattleEnemy).SkillQueue)
                        {
                            castingSkill._CastSpeed += 1;
                        }
                    }
                }
            }
            if (Mybutton.Myskill.MySkill.KeyID == "S_Argalia_11_2")
            {
                foreach (BattleChar b in targets)
                {
                    if (b is BattleEnemy)
                    {
                        foreach (CastingSkill castingSkill in (b as BattleEnemy).SkillQueue)
                        {
                            castingSkill._CastSpeed -= 1;
                        }
                    }
                }
                if (notcount)
                {
                    BattleSystem.instance.StartCoroutine(BattleSystem.instance.EnemyTurn(false));
                }
            }
        }
    }

    // Argalia Draw Allegretto
    public class S_Argalia_LucyD : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.Draw(2);

            BattleSystem.instance.AllyTeam.WaitCount += 1;
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                foreach (CastingSkill castingSkill in (b as BattleEnemy).SkillQueue)
                {
                    castingSkill._CastSpeed += 2;
                    //Debug.Log("Cast Speed Update");
                }
            }
        }
    }
    public class SkillEn_Argalia_0 : Skill_Momori
    {
        public override bool CanSkillEnforce(Skill MainSkill)
        {
            return this.IsDamage || this.IsHeal;
        }

        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 20;
            this.PlusPerStat.Heal = 20;
        }

        // Token: 0x06001036 RID: 4150 RVA: 0x0008F4E3 File Offset: 0x0008D6E3
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.WaitCount += 1;
        }
    }
}
