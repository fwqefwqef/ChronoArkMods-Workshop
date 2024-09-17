using System;
using GameDataEditor;
using UnityEngine;

namespace Gojo
{
	public class B_Limitless : Buff, IP_Dodge
	{
		public override void Init()
		{
			base.Init();
			this.PlusStat.PerfectDodge = true;
		}
		public void Dodge(BattleChar Char, SkillParticle SP)
		{

			if (Char == this.BChar)
			{
				// Apply Stun = 80% + CC Acc
				if (SP.UseStatus != null && !SP.UseStatus.Dummy && !SP.UseStatus.Info.Ally && (SP.UseStatus as BattleEnemy).istaunt)
				{
					Debug.Log("Limitless Stun");
					SP.UseStatus.BuffAdd(GDEItemKeys.Buff_B_Common_Rest, this.BChar, false, 70, false, -1, false);
				}
				// Remove Buff
				base.SelfDestroy(false);
			}

			// Peel Complete obedience stack
			if (Char == this.BChar && SP.UseStatus is BattleAlly)
            {
				Buff b = this.BChar.BuffReturn("B_S3_Pope_P_2");
				if (b != null)
                {
					b.SelfStackDestroy();
					for (int i = 0; i < BattleSystem.instance.CastSkills.Count; i++)
					{
						if (BattleSystem.instance.CastSkills[i].skill.ExtendedFind("Skill_Extended_B_S3_Boss_Pope_P_0_2", true) != null)
						{
							BattleSystem.instance.ActWindow.CastingWaste(BattleSystem.instance.CastSkills[i].skill);
							BattleSystem.instance.CastSkills.RemoveAt(i);
						}
					}
				}
				base.SelfDestroy(false);
			}
		}
	}

	public class B_Twister1 : Buff
	{
		public override void Init()
		{
			this.PlusStat.def = -25f * base.StackNum;
		}
	}

	public class B_Twister2 : Buff
	{
		public override void Init()
		{
			this.PlusPerStat.Damage = -25 * base.StackNum;
		}
	}

	public class B_Phase : Buff
	{
		public override void Init()
		{
			this.PlusStat.dod = -20f;
			this.PlusStat.RES_CC = -20f;
			this.PlusStat.RES_DEBUFF = -20f;
			this.PlusStat.RES_DOT = -20f;
		}
	}

	public class B_Phase2 : Buff
	{
		public override void Init()
		{
			this.PlusPerStat.Damage = 15;
			this.PlusStat.HIT_CC = 15f;
			this.PlusStat.HIT_DEBUFF = 15f;
			this.PlusStat.HIT_DOT = 15f;
		}
	}

	public class B_SimpleDomain : Buff
	{
		public override void Init()
		{
			this.PlusStat.Strength = true;
			this.PlusStat.RES_CC = 60f;
		}
	}
}
