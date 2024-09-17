using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;
using UnityEngine;

namespace Elementalist
{
	public class Ex_Fire : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
            foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                b.BuffAdd("B_FireBurn", this.BChar, false, 0, false, -1, false);
            }

            //Targets[0].BuffAdd("B_FireBurn", this.BChar, false, 0, false, -1, false);
		}
	}

	public class Ex_Elec : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.PlusSkillStat.cri = 33f;
		}
	}

	public class Ex_Dark : Skill_Extended, IP_SkillUse_Target
	{
		public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
		{
			if (DMG != 0 && Misc.PerToNum((float)DMG, 50f) >= 1f)
			{
				this.BChar.Heal(this.BChar, Misc.PerToNum((float)DMG, 33f), false, false, null);
			}
		}
	}
}
