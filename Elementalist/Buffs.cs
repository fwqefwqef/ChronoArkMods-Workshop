using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;
using I2.Loc;
using System.Collections;
using UnityEngine;
using DarkTonic.MasterAudio;
using HarmonyLib;
using Random = System.Random;

namespace Elementalist
{
    public class B_Channel : Buff
    {
        public override void Init()
        {
            base.Init();
        }
    }
    public class B_EnchantFire : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 25;
            this.PlusPerStat.Heal = 25;
        }
    }

    public class B_EnchantElec : Buff, IP_SkillUseHand_Team
    {
        public override void Init()
        {
            base.Init();
            //if (BattleSystem.instance != null)
            //{
            //    this.BChar.Overload = 0;
            //}
            this.PlusStat.PlusMPUse = -2;
        }
        
        public void SKillUseHand_Team(Skill skill)
        {
            if (skill.Master == this.BChar)
            {
                base.SelfDestroy(false);
            }
        }
    }

    public class B_EnchantDark : Buff, IP_BattleEnd //IP_TurnEnd,
    {
        public override void Init()
        {
            base.Init();
            this.BarrierHP += (int)Misc.PerToNum(base.Usestate_L.GetStat.atk, 100f);
            this.PlusStat.Strength = true; // HGP
        }
        public override void SelfdestroyPlus()
        {
            if (this.BarrierHP > 0)
            {
                this.BChar.Heal(base.Usestate_L, (float)this.BarrierHP, base.Usestate_L.GetCri(), false, null);
            }
        }
        public void BattleEnd()
        {
            if (this.BarrierHP > 0)
            {
                this.BChar.Heal(base.Usestate_L, (float)this.BarrierHP, base.Usestate_L.GetCri(), false, null);
            }
        } //
    }

    public class B_ElecCrit : Buff, IP_Hit
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.crihit = 100;
        }
        public void Hit(SkillParticle SP, int Dmg, bool Cri)
        {
            if (Dmg > 0)
            {
                base.SelfDestroy(false);
            }
        }
    }

    public class B_FireBurn : Buff
    {
        public override void Init()
        {
            base.Init();
        }

    }

    public class B_DarkElec : Buff
    {
        public override void Init()
        {
            base.Init();
            //this.PlusStat.def = -25f;
            this.PlusStat.def = -20f;
            this.PlusPerStat.Damage = -20;
        }
    }

    public class B_DarkBase : Buff, IP_SkillUse_User_After
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.Weak = true;
        }

        // Token: 0x06001000 RID: 4096 RVA: 0x0008AB67 File Offset: 0x00088D67
        public void SkillUseAfter(Skill SkillD)
        {
            BattleSystem.DelayInput(this.SkillUseAfter());
        }

        // Token: 0x06001001 RID: 4097 RVA: 0x0008AB74 File Offset: 0x00088D74
        public IEnumerator SkillUseAfter()
        {
            base.SelfStackDestroy();
            yield return null;
            yield break;
        }
    }

    //public class B_DarkBase_0 : Buff
    //{
    //    public override void Init()
    //    {
    //        base.Init();
    //        this.PlusStat.hit = -3f;
    //    }
    //}

    public class B_DarkDark : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.dod = -10f;
            this.PlusStat.RES_CC = -25f;
            this.PlusStat.RES_DEBUFF = -25f;
            this.PlusStat.RES_DOT = -25f;
        }
    }

    public class B_DarkDark_0 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.hit = -10f;
            this.PlusStat.HIT_CC = -25f;
            this.PlusStat.HIT_DEBUFF = -25f;
            this.PlusStat.HIT_DOT = -25f;
        }
    }

    public class B_MindControl : B_Taunt, IP_Awake, IP_SkillUse_User, IP_DamageChange
    {
        public int usecount = 0;
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 50;
            this.PlusStat.RES_CC = (float)(100); //
        }

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            // Your party target: deal 0 damage
            foreach(BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (Target.Equals(b))
                {
                    return 0;
                }
            }
            return Damage;
        }

        // Token: 0x06000A70 RID: 2672 RVA: 0x000788E1 File Offset: 0x00076AE1
        public override void SkillUse(Skill SkillD, List<BattleChar> Targets)
        {
            string target = SkillD.MySkill.Target.Key;
            Debug.Log("target type: "+target);
            // normal attack
            if (target == "enemy" || target == "enemy_PlusRandom" || target == "random_enemy")
            {
                Targets.Clear();
                Targets.Add(base.BChar);
            }
            // AoE
            else if (target == "all_enemy")
            {
                Targets.Clear();
                foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
                {
                    Targets.Add(b);
                }
            }
            // ally target skill
            else if (target == "ally")
            {
                Targets.Clear();
                Targets.Add(base.Usestate_L);
            }
            // all allies
            else if (target == "all_ally"){
                Targets.Clear();
                foreach (BattleChar b in BattleSystem.instance.AllyTeam.Chars)
                {
                    Targets.Add(b);
                }
            }
            else
            {
                Debug.Log("misc target type");
            }
            base.SkillUse(SkillD, Targets);

            usecount++;
            if (usecount == 2)
            {
                base.SelfDestroy();
            }
        }

        //public override void Init()
        //{
        //    base.Init();
        //    this.PlusStat.Stun = true;
        //}
    }

    public class B_Echo : Buff, IP_SkillUseHand_Team
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public void SKillUseHand_Team(Skill skill)
        {
            Skill copy = skill.CloneSkill(true, this.BChar, null, false);
            copy.isExcept = true;
            copy.FreeUse = true;
            copy.PlusHit = true;
            this.BChar.ParticleOut(skill, copy, this.BChar.BattleInfo.EnemyList.Random(this.BChar.GetRandomClass().Main));
            base.SelfDestroy();
        }
    }

    public class B_Dark : Buff
    {
        public override void Init()
        {
            base.Init();
            this.BarrierHP += (int)Misc.PerToNum(base.Usestate_L.GetStat.reg, 100f);
        }
    }

    public class B_FireDark : Buff, IP_SkillUse_Team_Target
    {
        public override void Init()
        {
            base.Init();
        }

        public void SkillUseTeam_Target(Skill skill, List<BattleChar> Targets)
        {
            if (skill.MySkill.KeyID == "S_FireDark" || Targets == null || Targets[0].Info.Ally == this.BChar.Info.Ally)
            {
                return;
            }
            foreach (Skill s in this.BChar.MyTeam.Skills)
            {
                if (s.MySkill.KeyID == "S_FireDark")
                {
                    s.Delete(false);
                    BattleSystem.DelayInputAfter(this.Attack(skill, Targets[0]));
                    break;
                }
            }
        }
        public IEnumerator Attack(Skill skill, BattleChar target)
        {
            yield return new WaitForSeconds(0.1f);
            Skill skill2 = Skill.TempSkill("S_FireDark", this.BChar, this.BChar.MyTeam);
            skill2.isExcept = true;
            skill2.FreeUse = true;
            skill2.PlusHit = true;

            if (target.IsDead)
            {
                this.BChar.ParticleOut(skill, skill2, this.BChar.BattleInfo.EnemyList.Random<BattleEnemy>());
            }
            else
            {
                this.BChar.ParticleOut(skill, skill2, target);
            }
            yield break;
        }
    }
    //public class B_CheckDead : Buff, IP_Dead //
    //{
    //    public void Dead()
    //    {
    //        BattleChar user = base.Usestate_L;
    //        if (user.Info.Name == "Elementalist")
    //        {
    //            if ((user.Info.Passive as P_Elementalist).getkill() == false)
    //            {
    //                (user.Info.Passive as P_Elementalist).triggerPassive();
    //                (user.Info.Passive as P_Elementalist).setkill();
    //            }
    //        }
    //    }
    //}

    public class B_Enchant : Buff, IP_SkillUseHand_Team
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.IgnoreTaunt = true;
            this.PlusStat.hit = 3f;
        }

        public void SKillUseHand_Team(Skill skill)
        {
            if (skill.Master == this.BChar)
            {
                base.SelfDestroy(false);
            }
        }
    }

    public class B_TransferRune : Buff, IP_BuffAdd
    {
        public override void Init()
        {
            base.Init();
        }

        public void Buffadded(BattleChar BuffUser, BattleChar BuffTaker, Buff addedbuff)
        {
            if (BuffTaker == this.BChar && !addedbuff.BuffData.Debuff) // A buff applied onto this character
            {
                List<BattleChar> list = BattleSystem.instance.AllyTeam.AliveChars.DeepCopy();
                list.Remove(this.BChar);
                BattleChar b = list.Random();
                Buff buff = DataToBuff(new GDEBuffData(addedbuff.BuffData.Key), b, BuffUser, addedbuff.LifeTime, addedbuff.View, addedbuff.BarrierHP);
                buff.BarrierHP = addedbuff.BarrierHP;
                //BattleChar b = list.Random();
                //Buff buff = addedbuff.DeepCopyCollection();
                //buff.BChar = b;
                //AccessTools.FieldRef<Buff, BattleChar> Usestate_F = AccessTools.FieldRefAccess<BattleChar>(typeof(Buff), "_SaveUseState_F");
                //Usestate_F(buff) = BuffUser;
                //AccessTools.FieldRef<Buff, BattleChar> Usestate_L = AccessTools.FieldRefAccess<BattleChar>(typeof(Buff), "_SaveUseState_L");
                //Usestate_L(buff) = b;

                b.BuffAddDelay(buff,false);
            }
        }

        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }

}
