using DarkTonic.MasterAudio;
using GameDataEditor;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace RedMist
{
    public class B_RedMist_0 : Buff, IP_SkillUse_Target, IP_Kill, IP_TurnEnd, IP_BattleEnd
    {
        public int damage = 0;
        public bool trigger = false;

        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", damage.ToString()).Replace("&b", trigger.ToString());
        }
        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            damage += DMG;
            if (damage >= 50)
            {
                TriggerEffect();
            }
        }
        public void KillEffect(SkillParticle SP)
        {
            if (SP.SkillData.Master == this.BChar)
            {
                TriggerEffect();
            }
        }
        public void TriggerEffect()
        {
            Debug.Log("Triggered Passive Effect");
            MasterAudio.PlaySound("Ding!", 10f, null, 0f, null, null, false, false);
            this.BChar.Overload = 0;
            if (this.PlusStat.atk >= 10)
            {

            }
            else
            {
                this.PlusStat.atk += 2;
            }
            damage = 0;
            trigger = true;
        }
        public void TurnEnd()
        {
            if (trigger == false)
            {
                this.BChar.BuffAdd("B_Common_Rest", this.BChar, false, 200, false, -1, false);
                this.SelfDestroy();
            }
            else
            {
                trigger = false;
                damage = 0;
            }
        }

        public void BattleEnd()
        {
            //MasterAudio.StopBus("BGM");
            //MasterAudio.StopBus("BattleBGM");
            //MasterAudio.FadeBusToVolume("BGM", 1f, 1f, null, false, false);
            //MasterAudio.FadeBusToVolume("BattleBGM", 0f, 0.5f, null, false, false);
        }

        public override void SelfdestroyPlus()
        {
            //MasterAudio.StopBus("BGM");
            //MasterAudio.StopBus("BattleBGM");
            //MasterAudio.FadeBusToVolume("BGM", 1f, 1f, null, false, false);
            //MasterAudio.FadeBusToVolume("BattleBGM", 0f, 0.5f, null, false, false);
        }
    }
    public class B_RedMist_Bleed : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }
    }
    public class B_RedMist_4 : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusPerStat.Damage = 25 * this.StackNum;
            this.PlusStat.hit = 10 * this.StackNum;
        }
    }
    public class B_RedMist_6 : Buff, IP_SkillUse_User_After
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public override void FixedUpdate()
        {
            int missingHealth = this.BChar.GetStat.maxhp - this.BChar.HP;
            this.PlusStat.atk = missingHealth / 2;
        }

        public void SkillUseAfter(Skill SkillD)
        {
            if (SkillD.IsDamage)
            {
                Buff buff = this.BChar.BuffAdd(GDEItemKeys.Buff_B_Momori_P_NoDead, this.BChar, false, 0, false, -1, false);
                this.BChar.Damage(this.BChar, this.BChar.GetStat.maxhp / 3, false, true);
                buff.SelfDestroy(false);
            }
        }
    }
    public class B_RedMist_ArmorDown : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.def = -15 * this.StackNum;
        }
    }

    public class B_RedMist_SmilingBody : Buff
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.PlusStat.def = -100 * this.StackNum;
        }
    }
}
