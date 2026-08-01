using ChronoArkMod.ModEditor;
using GameDataEditor;
using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace PurpleTear
{
    public class B_PurpleTear_P_Slash : Buff//, IP_Kill
    {
        private const int StatRefreshFrames = 10;
        private int statRefreshTimer;

        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.statRefreshTimer = StatRefreshFrames;
            this.ApplyArmorConversion();
        }

        public override void FixedUpdate()
        {
            this.statRefreshTimer++;
            if (this.statRefreshTimer < StatRefreshFrames)
            {
                return;
            }

            this.statRefreshTimer = 0;
            this.ApplyArmorConversion();
        }

        private void ApplyArmorConversion()
        {
            float sourceArmor = Math.Max(0f, this.BChar.GetStat.def - this.PlusStat.def);

            this.PlusPerStat.Damage = 20 + (int)sourceArmor;
            this.PlusStat.def = -sourceArmor;
        }

        //public void KillEffect(SkillParticle SP)
        //{
        //    Skill skill = Skill.TempSkill(SP.SkillData.MySkill.Key, this.BChar, this.BChar.MyTeam);
        //    skill.isExcept = true;
        //    skill.AutoDelete = 1;
        //    BattleSystem.instance.AllyTeam.Add(skill, true);
        //}

        public override string DescExtended()
        {
            Passive_Char passive = this.BChar.Info.Passive;
            if (passive is P_PurpleTear)
            {
                return base.DescExtended().Replace("&a", (passive as P_PurpleTear).skillsPlayedInStance.ToString());
            }
            return base.DescExtended();
        }
    }

    public class B_PurpleTear_P_Pierce : Buff
    {
        int guard = 0;

        public override void FixedUpdate()
        {
            if (guard < 10)
            {
                guard++;
                return;
            }
            guard = 0;
            Debug.Log("Hello World");

            foreach (BattleChar Enemy in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                Buff b = Enemy.BuffReturn("B_PurpleTear_DoubleDebuff");
                if (b == null)
                {
                    Enemy.BuffAdd("B_PurpleTear_DoubleDebuff", this.BChar, false, 0, false, -1, false);
                }
            }
        }

        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.HIT_CC = 20;
            this.PlusStat.HIT_DEBUFF = 20;
            this.PlusStat.HIT_DOT = 20;
        }
        public override string DescExtended()
        {
            Passive_Char passive = this.BChar.Info.Passive;
            if (passive is P_PurpleTear)
            {
                return base.DescExtended().Replace("&a", (passive as P_PurpleTear).skillsPlayedInStance.ToString());
            }
            return base.DescExtended();
        }
    }

    public class B_PurpleTear_DoubleDebuff : Buff, IP_BuffAdd
    {
        private bool addingDuplicate;

        public override void Init()
        {
            base.Init();
        }

        public void Buffadded(BattleChar BuffUser, BattleChar BuffTaker, Buff addedbuff)
        {
            if (BuffTaker == this.BChar && addedbuff.BuffData.Debuff && (BuffUser.BuffFind("B_PurpleTear_P_Pierce") || BuffUser.BuffFind("B_PurpleTear_P_Pierce_Temp")))
            {
                if (this.addingDuplicate)
                {
                    return;
                }

                try
                {
                    this.addingDuplicate = true;
                    this.BChar.BuffAdd(addedbuff.BuffData.Key, BuffUser);
                }
                finally
                {
                    this.addingDuplicate = false;
                }
            }
        }
    }

    public class B_PurpleTear_P_Blunt : Buff, IP_SkillUse_Target
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.cri = 25;
        }
        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            if (DMG > 0)
            {
                hit.BuffAdd("B_PurpleTear_Stagger", this.BChar);
                if (Cri)
                {
                    hit.BuffAdd("B_PurpleTear_Stagger", this.BChar);
                }
            }
        }
        public override string DescExtended()
        {
            Passive_Char passive = this.BChar.Info.Passive;
            if (passive is P_PurpleTear)
            {
                return base.DescExtended().Replace("&a", (passive as P_PurpleTear).skillsPlayedInStance.ToString());
            }
            return base.DescExtended();
        }
    }

    public class B_PurpleTear_Stagger : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.RES_CC = -8 * base.StackNum;

            if (base.StackNum >= 4)
            {
                this.BChar.BuffAdd("B_Common_Rest", this.Usestate_F, false, 100);
                this.SelfDestroy();
            }
        }
    }

    public class B_PurpleTear_P_Guard : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.RES_CC = 300;
            this.PlusStat.RES_DOT = 300;
            this.PlusStat.RES_DEBUFF = 300;

            for (int i = 0; i < this.BChar.Buffs.Count; i++)
            {
                if (this.BChar.Buffs[i].BuffData.Debuff && !this.BChar.Buffs[i].CantDisable)
                {
                    this.BChar.Buffs[i].SelfDestroy(false);
                }
            }
        }
        public override string DescExtended()
        {
            Passive_Char passive = this.BChar.Info.Passive;
            if (passive is P_PurpleTear)
            {
                return base.DescExtended().Replace("&a", (passive as P_PurpleTear).skillsPlayedInStance.ToString());
            }
            return base.DescExtended();
        }
    }
    public class B_PurpleTear_1 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.crihit = 50 * base.StackNum;
        }
    }
    public class B_PurpleTear_2 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = 25 * base.StackNum;
            this.PlusStat.cri = 25 * base.StackNum;
        }
    }
    public class B_PurpleTear_Bleed : Buff
    {
        public override void Init()
        {
            base.Init();
        }
    }
    public class B_PurpleTear_4 : Buff, IP_SkillUse_Target
    {
        public override void Init()
        {
            base.Init();
        }
        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            if (DMG > 0)
            {
                hit.BuffAdd("B_PUrpleTear_Bleed",this.BChar);
            }
        }
    }
    public class B_PurpleTear_5 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.def = -25 * base.StackNum;
        }
    }
    public class B_PurpleTear_6 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusPerStat.Damage = -20 * base.StackNum;
        }
    }
    public class B_PurpleTear_7 : Buff, IP_Hit
    {
        public BattleChar user;
        public override void Init()
        {
            base.Init();
            this.PlusStat.Strength = true;
            this.PlusStat.def = 25;
            user = this.Usestate_F;
        }
        public void Hit(SkillParticle SP, int Dmg, bool Cri)
        {
            Debug.Log("Hit");
            Skill temp = Skill.TempSkill("S_PurpleTear_7_0",user,user.MyTeam);
            temp.PlusHit = true;
            BattleSystem.DelayInput(BattleSystem.instance.ForceAction(temp, SP.SkillData.Master, false, false, true, null));
        }

        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", ((int)(user.GetStat.atk * 0.8)).ToString());
        }
    }

    public class B_PurpleTear_10 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.BarrierHP = (int)this.Usestate_F.GetStat.atk * 2;
        }
    }
    public class B_PurpleTear_11 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.def = 30;
        }
    }
    public class B_PurpleTear_12 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.PlusStat.def = -12 * base.StackNum;
            this.PlusPerStat.Damage = -12 * base.StackNum;
        }
    }
    public class B_PurpleTear_13 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.BarrierHP = (int)this.Usestate_F.GetStat.atk;
        }
    }
}
