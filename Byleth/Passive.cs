using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GameDataEditor;
using I2.Loc;
using System.Security.Cryptography;
using Spine;

namespace Byleth
{
    public class P_Byleth : Passive_Char, IP_BattleStart_Ones
    {
        public void BattleStart(BattleSystem Ins)
        {
            this.BChar.BuffRemove("B_Byleth_P");
            this.BChar.BuffAdd("B_Byleth_P", this.BChar);

            // Check if this character has leanrned S_Byleth_10 (Ruptured Heaven)
            List<CharInfoSkillData> learnedSkills = this.BChar.Info.SkillDatas;
            foreach (CharInfoSkillData skill in learnedSkills)
            {
                if (skill.SkillInfo.KeyID == "S_Byleth_10")
                {
                    this.BChar.BuffAdd("B_Byleth_10", this.BChar);
                    break;
                }
            }
        }
    }

    // Passive buff that increases attack power every 3rd attack
    public class B_Byleth_P : Buff, IP_ParticleOut_After
    {
        public List<GDESkillData> UsedClassSkills = new List<GDESkillData>();
        public int count = 0;
     
        public override void Init()
        {
            
        }
        public override void FixedUpdate()
        {
            if (count == 2)
            {
                this.PlusPerStat.Damage = 20;
            }
            else
            {
                this.PlusPerStat.Damage = 0;
            }
        }

        public IEnumerator ParticleOut_After(Skill SkillD, List<BattleChar> Targets)
        {
            if (SkillD.Master != this.BChar || !SkillD.IsDamage || SkillD.PlusHit)
            {
                yield return null;
            }

            else
            {
                // Flag = already at 2 stacks
                if (count == 2)
                {
                    count = 0;
                }
                else
                {
                    count += 1;
                    if (count == 2)
                    {
                        this.BChar.BuffAdd("B_Crest", this.BChar);
                    }
                }
            }
        }
        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", count.ToString());
        }
    }

    public class Ex_Crest : BuffSkillExHand
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            //this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_1_Ex).Particle_Path;
            //base.SkillParticleOn();
        }
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingle(SkillD, Targets);
            BattleSystem.DelayInputAfter(this.Del());
        }

        private IEnumerator Del()
        {
            yield return new WaitForFixedUpdate();
            this.MainBuff.SelfDestroy(false);
            yield break;
        }
    }

    public class B_Crest : Buff
    {
        public override void Init()
        {
            base.Init();
            this.LucySkillExBuff = (BuffSkillExHand)Skill_Extended.DataToExtended("Ex_Crest");
        }

        // Token: 0x060029BB RID: 10683 RVA: 0x0010F168 File Offset: 0x0010D368
        public override bool CanSkillBuffAdd(Skill AddedSkill, int Index)
        {
            return AddedSkill.Master == this.BChar && AddedSkill.IsDamage && (AddedSkill.ExtendedFind_DataName("Ex_Crest")==null);
        }
    }
}
