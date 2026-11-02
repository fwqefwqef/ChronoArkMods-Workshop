using ChronoArkMod;
using Curse;
using GameDataEditor;
using I2.Loc;
using NLog.Targets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Eria
{
    // Rose
    public class B_Eria_1 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.DMGTaken = base.Usestate_F.GetStat.reg * 2f;
            this.PlusStat.RES_CC = -base.Usestate_F.GetStat.reg * 2f;
            this.PlusStat.RES_DEBUFF = -base.Usestate_F.GetStat.reg * 2f;
            this.PlusStat.RES_DOT = -base.Usestate_F.GetStat.reg * 2f;
        }
        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", ((int)(base.Usestate_F.GetStat.reg * 2f)).ToString());
        }
    }
    // Vin de Sanguine
    public class B_Eria_2 : Buff, IP_DamageTake
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
        {
            if (NODEF)
            {
                BattleChar battlechar = this.Usestate_F;
                int lowest = 9999;
                foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
                {
                    if (b.HP < lowest)
                    {
                        lowest = b.HP;
                        battlechar = b;
                    }
                }

                battlechar.Heal(this.Usestate_F, 0, Dmg / 2, 0);

                //if (base.StackNum == 1)
                //{
                //    battlechar.Heal(this.Usestate_F, 0, Dmg / 2, 0);
                //}
                //else if (base.StackNum == 2)
                //{
                //    battlechar.Heal(this.Usestate_F, 0, Dmg, 0);
                //}
            }
        }
    }
    // Putréfaction: on death, every debuff on this enemy (including Putréfaction itself) spreads to the other enemies
    public class B_Eria_RedBlue : Buff, IP_Dead
    {
        public void Dead()
        {
            List<Buff> debuffs = this.BChar.GetBuffs(BattleChar.GETBUFFTYPE.ALLDEBUFF, false, false);
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                if (b == this.BChar)
                {
                    continue;
                }
                foreach (Buff buff in debuffs)
                {
                    foreach (StackBuff stack in buff.StackInfo)
                    {
                        b.BuffAdd(buff.BuffData.Key, stack.UseState, false, 0, false, stack.RemainTime, false);
                    }
                }
            }
        }

        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusPerStat.Damage = -25;
        }
    }
    // Le Soleil: no draw at the start of next turn
    public class B_Eria_RedYellow : Buff, IP_DrawNumChange
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
        public void DrawNumChange(int DrawNum, out int OutNum)
        {
            OutNum = 0;
        }
    }
    // Lily
    public class B_Eria_4 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.BarrierHP = (int)(this.Usestate_F.GetStat.reg * 1);
        }
    }
    public class B_Eria_5 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.Strength = true;
        }
    }
    public class B_Eria_BlueBlue : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.BarrierHP = (int)(this.Usestate_F.GetStat.reg * 2);
        }
    }
    public class B_Eria_BlueYellow : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
    }

    // Sacred Swallow Hirondelle Sacree
    public class B_Eria_9 : Buff, IP_TurnEndButtonEnemy
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.DMGTaken = -70;
        }

        // Removed when the player presses the end turn button
        public void TurnEndButtonEnemy()
        {
            this.SelfDestroy();
        }
    }

    public class B_Eria_YellowYellow : Buff, IP_HPChange
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusPerStat.Damage = 40;
            this.PlusPerStat.Heal = 40;
            this.PlusStat.def = 40;
        }
        public void HPChange(BattleChar Char, bool Healed)
        {
            if (Char == this.BChar && Char.HP <= 0)
            {
                BattleSystem.DelayInputAfter(Dead(Char));
            }
        }

        public static IEnumerator Dead(BattleChar Char)
        {
            if (Char.HP <= 0)
            {
                Char.SimpleTextOut(ScriptLocalization.UI_Battle.instantdeath);
                Char.Dead(false, false);
            }
            yield return null;
            yield break;
        }
    }
    public class B_Eria_RedBlueYellow : Buff, IP_SkillUse_User, IP_DamageChange
    {
        public int usecount = 0;
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.def = -50;
        }

        public override void SelfdestroyPlus()
        {
            base.SelfdestroyPlus();
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
            this.BChar.BuffAdd(GDEItemKeys.Buff_B_Common_CCRsis, this.BChar, false, 0, false, -1, false);
        }

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            // Your party target: deal 0 damage
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (Target.Equals(b))
                {
                    return 0;
                }
            }
            return Damage;
        }

        // Token: 0x06000A70 RID: 2672 RVA: 0x000788E1 File Offset: 0x00076AE1
        public void SkillUse(Skill SkillD, List<BattleChar> Targets)
        {
            string target = SkillD.MySkill.Target.Key;
            Debug.Log("target type: " + target);
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
            else if (target == "all_ally")
            {
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

            usecount++;
            if (usecount == 1)
            {
                base.SelfDestroy();
            }
        }
    }

    // Le Jardin Fleuri: red flower. Same setup as Caramel Popcorn: the buff puts Ex_Eria_10 on this character's attacks in hand.
    // Attacks that don't come from hand (fixed abilities, etc.) get the +50% through DamageChange instead.
    // The buff is used up after this character's next attack, wherever it came from.
    public class B_Eria_10 : Buff, IP_DamageChange, IP_SkillUse_User_After
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.LucySkillExBuff = (BuffSkillExHand)Skill_Extended.DataToExtended("Ex_Eria_10");
        }

        public override bool CanSkillBuffAdd(Skill AddedSkill, int Index)
        {
            return AddedSkill.Master == this.BChar && AddedSkill.IsDamage && AddedSkill.ExtendedFind_DataName("Ex_Eria_10") == null;
        }

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (SkillD.Master == this.BChar && !SkillD.PlusHit && SkillD.ExtendedFind_DataName("Ex_Eria_10") == null)
            {
                return (int)(Damage * 1.5f);
            }
            return Damage;
        }

        public void SkillUseAfter(Skill SkillD)
        {
            if (SkillD.Master == this.BChar && SkillD.IsDamage && !SkillD.PlusHit && !this.DestroyBuff)
            {
                this.SelfDestroy(false);
            }
        }
    }
    // +50% damage on the next attack played from hand, then the red flower is used up
    public class Ex_Eria_10 : BuffSkillExHand
    {
        private bool flag;

        public override void Init()
        {
            base.Init();
            this.PlusSkillPerFinal.Damage = 50;
        }

        public override void SkillUseHand(BattleChar Target)
        {
            base.SkillUseHand(Target);
            flag = true;
            BattleSystem.DelayInput(this.Del());
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            if (!flag)
            {
                this.PlusSkillPerFinal.Damage = 0;
            }
        }

        public IEnumerator Del()
        {
            if (this.MainBuff != null && !this.MainBuff.DestroyBuff)
            {
                this.MainBuff.SelfDestroy(false);
            }
            yield break;
        }
    }
    // Le Jardin Fleuri: blue flower. The barrier amount is added by S_Eria_10_1 when it's applied.
    public class B_Eria_10_1 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
    }

    // Couleurs Vivantes
    public class B_Eria_11 : Buff, IP_SkillUse_User
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.isStackDestroy = true;
        }

        public void SkillUse(Skill SkillD, List<BattleChar> Targets)
        {
            // Only skills played from hand: no fixed abilities or extra hits
            // Couleurs Vivantes itself (this buff is added mid-cast) doesn't count
            if (SkillD.Master != this.BChar || SkillD.BasicSkill || SkillD.PlusHit || SkillD.MySkill.KeyID == "S_Eria_11")
            {
                return;
            }
            BattleChar eria = P_Eria.FindEria();
            if (eria == null)
            {
                return;
            }
            string paint = P_Eria.Colors.Random(this.BChar.GetRandomClass().Main);
            eria.BuffAdd(paint, eria);
            // Fully refresh the Paint Fixed Ability: clear the cooldown, then refill it
            BattleAlly ally = eria as BattleAlly;
            if (ally != null && ally.MyBasicSkill != null)
            {
                ally.MyBasicSkill.CoolDownNum = 0;
            }
            eria.MyTeam.BasicSkillRefill(eria, eria.BattleBasicskillRefill);
            this.SelfStackDestroy();
        }

        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", base.StackNum.ToString());
        }
    }


    public class B_Eria_X : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
    }
}