using DarkTonic.MasterAudio;
using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static System.Random;
using Random = System.Random;

namespace Angela
{
    // Happy Memories
    public class B_Angela_1 : Buff
    {
        public override void Init()
        {
            base.Init();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            foreach (Skill skill in this.BChar.MyTeam.Skills)
            {
                if (skill.Master == this.BChar && skill.ExtendedFind_DataName("Angela_1_Ex") == null)
                {
                    skill.ExtendedAdd(Skill_Extended.DataToExtended("Angela_1_Ex"));
                }
            }
        }
    }

    public class Angela_1_Ex : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.APChange = -1;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!this.Use && !this.MySkill.IsNowCasting)
            {
                if (this.BChar.BuffFind("B_Angela_1"))
                {
                    return;
                }
                this.SelfDestroy();
            }
        }
        public override void SkillUseHand(BattleChar Target)
        {
            this.Use = true;
        }

        private bool Use;
    }

    // A Token of Friendship
    public class B_Angela_4 : Buff
    {
        public override void Init()
        {
            base.Init();
        }
    }

    // Vampirism
    public class B_Angela_6 : Buff, IP_SkillUse_Target
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.def = -20f;
        }

        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            if (DMG != 0 && Misc.PerToNum((float)DMG, 20f) >= 1f)
            {
                this.BChar.Heal(this.BChar, Misc.PerToNum((float)DMG, 20f), false, false, null);
            }
        }
    }

    // Oblivion
    public class B_Angela_7 : Buff, IP_PlayerTurn
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.def = 40f * base.StackNum;
            this.PlusPerStat.Damage = -40 * base.StackNum;
        }

        public void Turn() // give mana then destroy
        {
            int stacks = base.StackNum;
            BattleSystem.instance.AllyTeam.AP += stacks;
            base.SelfDestroy();
        }
    }

    // Eternally Lit Lamp
    public class B_Angela_8 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.AggroPer = 80; // aggro greatly increased
        }
    }

    // Baptism
    public class B_Angela_9 : Buff, IP_FogDamageChange
    {
        public override void Init()
        {
            base.Init();

            if (base.StackNum == 12)
            {
                this.PlusStat.DMGTaken = (float)(-33);
            }
        }
        public void FogDamageChange(BattleChar Char, ref int FogDamage)
        {
            if (base.StackNum == 12)
            {
                FogDamage = 0;
            }
        }
    }

    // Powder of Life
    public class B_Angela_11 : Buff, IP_DamageTake
    {
        public override void Init()
        {
            base.Init();
        }

        public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
        {
            if (!resist && this.BChar.HP <= 0 && Dmg >= 1)
            {
                resist = true;
                this.BChar.HP = Mathf.RoundToInt(this.BChar.GetStat.maxhp * 0.2f);
                base.SelfDestroy();
            }
        }
    }

    // Abnormality Pages!

    public class B_Angela_Lies : Buff, IP_PlayerTurn
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.hit = 5f;

            Random r = new Random();

            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills)
            {
                if (s.Master == this.Usestate_L)
                {

                    s.AP = r.Next(0, 4);
                }
            }

            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills_Deck)
            {
                if (s.Master == this.Usestate_L)
                {
                    s.AP = r.Next(0, 4);
                }
            }

            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills_UsedDeck)
            {
                if (s.Master == this.Usestate_L)
                {
                    s.AP = r.Next(0, 4);
                }
            }
        }

        public void Turn()
        {
            Random r = new Random();

            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills)
            {
                if (s.Master == this.BChar)
                {

                    s.AP = r.Next(0, 4);
                }
            }

            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills_Deck)
            {
                if (s.Master == this.BChar)
                {
                    s.AP = r.Next(0, 4);
                }
            }

            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills_UsedDeck)
            {
                if (s.Master == this.BChar)
                {
                    s.AP = r.Next(0, 4);
                }
            }
        }
    }

    public class B_Angela_Courage : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 10;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.BChar.MyTeam.Chars.Count == this.BChar.MyTeam.AliveChars.Count)
            {
                this.PlusPerStat.Damage = 10;
            }
            else {
                this.PlusPerStat.Damage = -20;
            }
        }
    }

    public class B_Angela_Vengeance : Buff, IP_NearDeath, IP_SkillUseHand_Team
    {
        public bool flag = false;
        public override void Init()
        {
            base.Init();
            this.PlusStat.crihit = 100;
            this.PlusStat.Penetration = 50f;
        }

        public void NearDeath(BattleAlly Ally)
        {
            if (Ally == this.BChar)
            {
                flag = true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (flag)
            {
                Debug.Log("Flag");
                foreach (Skill skill in this.BChar.MyTeam.Skills)
                {
                    if (skill.Master == this.BChar && skill.ExtendedFind_DataName("Angela_Vengeance_Ex") == null)
                    {
                        skill.ExtendedAdd(Skill_Extended.DataToExtended("Angela_Vengeance_Ex"));
                    }
                }
            }
            else
            {

            }
        }

        public void SKillUseHand_Team(Skill skill)
        {
            if (skill.Master == this.BChar && skill.ExtendedFind_DataName("Angela_Vengeance_Ex") != null)
            {
                flag = false;
                skill.ExtendedDelete("Angela_Vengeance_Ex");
                
                // Remove extends from hand
                foreach (Skill s in this.BChar.MyTeam.Skills)
                {
                    if (s.Master == this.BChar && s.ExtendedFind_DataName("Angela_Vengeance_Ex") != null)
                    {
                        s.ExtendedDelete("Angela_Vengeance_Ex");
                    }
                }
            }
        }
    }

    public class Angela_Vengeance_Ex : Skill_Extended
    {
        public override void Init()
        {
            base.Init();
            this.PlusSkillPerFinal.Damage = 33;
            this.PlusSkillPerFinal.Heal = 33;
        }
    }

    public class B_Angela_Flutters : Buff, IP_Dodge, IP_PlayerTurn
    {
        public bool flag = true;
        public override void Init()
        {
            base.Init();
            this.PlusStat.dod = 8;
        }

        public void Dodge(BattleChar Char, SkillParticle SP)
        {
            if (flag)
            {
                BattleSystem.instance.AllyTeam.AP += 1;
                flag = false;
            }
        }

        public void Turn()
        {
            flag = true;
        }
    }

    public class B_Angela_Ashes : Buff, IP_Hit
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.def = 5f;
        }
        public void Hit(SkillParticle SP, int Dmg, bool Cri)
        {
            if (SP.UseStatus.Info.Ally != this.BChar.Info.Ally)
            {
                SP.UseStatus.BuffAdd("B_Angela_Ashes_0", this.BChar, false, (int)(this.BChar.GetStat.HIT_DOT));
            }
        }
    }

    public class B_Angela_Ashes_0 : Buff // Burn
    {
        public override void Init()
        {
            base.Init();
        }
    }

    public class B_Angela_Love : Buff, IP_SkillUse_Target
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.cri = 5;
        }
        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            if ((SP.SkillData.IsDamage || SP.SkillData.IsHeal) && Cri)
            {
                // Full party 1 heal
                foreach(BattleChar b in BattleSystem.instance.AllyTeam.Chars)
                {
                    b.Heal(b, 1, false, false, null);
                }
            }
        }
    }

    public class B_Angela_FerventBeats : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 50;
            this.PlusPerStat.Heal = 50;
            this.PlusStat.def = 50f;
        }

        public override void SelfdestroyPlus()
        {
            base.SelfdestroyPlus();
            this.BChar.Dead();
        }
    }
    public class B_Angela_ChainedWrath : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.cri = 70f;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Hand skills no swiftness
            foreach (Skill skill in this.BChar.MyTeam.Skills)
            {
                if (skill.Master == this.BChar)
                {
                    skill.NotCount = false;
                }
            }

            // Fixed no swiftness
            (this.BChar as BattleAlly).MyBasicSkill.buttonData.NotCount = false;
        }
    }

    public class B_Angela_Obsession : Buff, IP_EnemyAwake, IP_BuffAddAfter
    {
        public override void Init()
        {
            base.Init();

            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (b.IsLucyC)
                {

                }
                else
                {
                    b.BuffAdd("B_Angela_Obsession_0", this.BChar);
                }
            }
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                b.BuffAdd("B_Angela_Obsession_0", this.BChar);
            }
        }

        public void EnemyAwake(BattleChar Enemy)
        {
            if (!Enemy.BuffFind("B_Angela_Obsession_0", false))
            {
                Enemy.BuffAdd("B_Angela_Obsession_0", this.BChar, true, 0, false, -1, false);
            }
        }

        public void BuffaddedAfter(BattleChar BuffUser, BattleChar BuffTaker, Buff addedbuff, StackBuff stackBuff)
        {
            if (BuffUser == this.BChar && addedbuff.BuffData.Debuff && stackBuff.RemainTime > 0 && (addedbuff.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_Debuff || addedbuff.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_DOT))
            {
                stackBuff.RemainTime += 2;
            }
        }
    }

    public class B_Angela_Obsession_0 : Buff, IP_BuffAddAfter
    {
        public override void Init()
        {
            base.Init();
        }
        public void BuffaddedAfter(BattleChar BuffUser, BattleChar BuffTaker, Buff addedbuff, StackBuff stackBuff)
        {
            if (BuffUser == this.BChar && addedbuff.BuffData.Debuff && stackBuff.RemainTime > 0 && (addedbuff.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_Debuff || addedbuff.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_DOT))
            {
                stackBuff.RemainTime+=2;
            }
        }
    }

    public class B_Angela_Incomprehensible : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.HIT_DEBUFF = 20f;
            this.PlusStat.HIT_CC = 20f;
            this.PlusStat.HIT_DOT = 20f;
        }
    }

    public class B_Angela_Absorption : Buff, IP_Kill
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.HEALTaken = 20f;
        }

        public void KillEffect(SkillParticle SP)
        {
            this.PlusPerStat.MaxHP += 10;
        }
    }

    public class B_Angela_DarkFlame : Buff, IP_EnemyAwake
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.DMGTaken = 100f;

            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                if (b.IsLucyC)
                {

                }
                else
                {
                    b.BuffAdd("B_Angela_DarkFlame_0", this.BChar);
                }
            }
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                b.BuffAdd("B_Angela_DarkFlame_0", this.BChar);
            }

            MasterAudio.StopBus("BGM");
            MasterAudio.StopBus("BattleBGM");
            MasterAudio.FadeBusToVolume("BGM", 1f, 1f, null, false, false);
            MasterAudio.FadeBusToVolume("BattleBGM", 0f, 0.5f, null, false, false);
            MasterAudio.PlaySound("DarkFlame",1f);
        }
        public void EnemyAwake(BattleChar Enemy)
        {
            if (!Enemy.BuffFind("B_Angela_DarkFlame_0", false))
            {
                Enemy.BuffAdd("B_Angela_DarkFlame_0", this.BChar, true, 0, false, -1, false);
            }
        }
    }

    public class B_Angela_DarkFlame_0 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.DMGTaken = 100f;
        }
    }

    public class B_Angela_Blizzard : Buff, IP_PlayerTurn
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.Stun = true;
        }

        public void Turn()
        {
            if (this.BChar is BattleEnemy)
            {
                this.BChar.BuffAdd("B_Angela_Blizzard_0", this.Usestate_F);
            }
            this.SelfDestroy();
        }
    }

    public class B_Angela_Blizzard_0 : Buff // Slowed
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.spd = -5;
        }
    }

    public class B_Angela_Loyalty : Buff, IP_Healed
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;

            if (this.BChar.HP > (this.BChar.GetStat.maxhp * 0.2))
            {
                this.BChar.HP = (int)(this.BChar.GetStat.maxhp * 0.2);
            }
        }
        public void Healed(BattleChar Healer, BattleChar HealedChar, int HealNum, bool Cri, int OverHeal)
        {
            if (HealedChar == this.BChar)
            {
                if (this.BChar.HP > (this.BChar.GetStat.maxhp * 0.2))
                {
                    this.BChar.HP = (int)(this.BChar.GetStat.maxhp * 0.2);
                }
            }
        }
    }

    public class B_Angela_Loyalty_Ally : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 15;
            this.PlusPerStat.Heal = 15;
            this.PlusStat.def = 15f;
        }
    }

    public class B_Angela_Shell : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.RES_CC = 100f;
            this.PlusStat.RES_DOT = 100f;
            this.PlusStat.RES_DEBUFF = 100f;

            for (int i = 0; i < this.BChar.Buffs.Count; i++)
            {
                if (this.BChar.Buffs[i].BuffData.Debuff && !this.BChar.Buffs[i].CantDisable)
                {
                    this.BChar.Buffs[i].SelfDestroy(false);
                }
            }
        }
    }

    public class B_Angela_GooeyWaste : Buff, IP_SkillUse_Target
    {
        public override void Init()
        {
            base.Init();
        }

        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            if (SP.SkillData.IsDamage)
            {
                foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
                {
                    b.BuffAdd("B_Angela_GooeyWaste_0", this.BChar, false, (int)(this.BChar.GetStat.HIT_DOT));
                }
            }
        }
    }

    public class B_Angela_GooeyWaste_0 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.cri = -3 * base.StackNum;
            this.PlusStat.def = -3 * base.StackNum;
            this.PlusStat.dod = -3 * base.StackNum;
        }
    }
}
