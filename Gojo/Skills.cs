using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;
using I2.Loc;
using System.Collections;

namespace Gojo
{
	public class S_Twister : Skill_Extended
	{
		List<BattleChar> targets = new List<BattleChar>();
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			targets = Targets;
			List<Skill> list = new List<Skill>();
			list.Add(Skill.TempSkill("S_Twister1", this.MySkill.Master, this.MySkill.Master.MyTeam));
			list.Add(Skill.TempSkill("S_Twister2", this.MySkill.Master, this.MySkill.Master.MyTeam));
			BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, false));
		}
		public void Del(SkillButton Mybutton)
		{
			if (Mybutton.Myskill.MySkill.KeyID == "S_Twister1")
			{
				foreach(BattleChar b in targets)
                {
					b.BuffAdd("B_Twister1", this.BChar, false, 0, false, -1, false);
				}
			}
			if (Mybutton.Myskill.MySkill.KeyID == "S_Twister2")
			{
				foreach (BattleChar b in targets)
				{
					b.BuffAdd("B_Twister2", this.BChar, false, 0, false, -1, false);
				}
			}
		}
	}
	public class S_BlackHole : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			foreach (BattleChar b in Targets)
            {
				if (b is BattleEnemy && !(b as BattleEnemy).istaunt)
                {
					Buff buff = b.BuffAdd(GDEItemKeys.Buff_B_EnemyTaunt, this.BChar, false, 0, false, 3, false);
					buff.TimeUseless = false;
					buff.LifeTime = 5;
					buff.StackInfo[0].RemainTime = 5;
				}
				//else if (b is BattleEnemy && (b as BattleEnemy).istaunt)
    //            {
				//	b.BuffAdd("B_Phase", this.BChar, false, 0, false, -1, false);
				//}
            }
		}
	}
	public class S_Fist : Skill_Extended
	{
		// Enraged: "B_Momori_7_T"
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			foreach (BattleChar b in Targets)
			{
				if (b is BattleEnemy && !(b as BattleEnemy).istaunt)
				{
					Buff buff = b.BuffAdd(GDEItemKeys.Buff_B_EnemyTaunt, this.BChar, false, 0, false, 3, false);
					buff.TimeUseless = false;
					buff.LifeTime = 5;
					buff.StackInfo[0].RemainTime = 5;
				}
			}
		}
	}

	public class S_Travel : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			foreach (BattleChar battleChar in BattleSystem.instance.AllyTeam.AliveChars)
            {
				battleChar.Overload = 0;
			}
			this.MySkill.Master.MyTeam.Draw(1);
		}
	}

	public class S_Phase : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);

			// If Limitless Active
			if (this.BChar.BuffFind("B_Limitless"))
            {
				this.BChar.BuffAdd("B_Phase2", this.BChar, false, 0, false, -1, false);

				//foreach (BattleChar battleChar in BattleSystem.instance.EnemyTeam.AliveChars)
				//{
				//	battleChar.BuffAdd("B_Phase", this.BChar, false, 0, false, -1, false);
				//                if (battleChar is BattleEnemy && (battleChar as BattleEnemy).istaunt)
				//                {
				//                    battleChar.BuffAdd("B_Phase", this.BChar, false, 0, false, -1, false);
				//                }
				//            }
			}

			// Otherwise
			else
            {
				this.BChar.BuffAdd("B_Limitless", this.BChar, false, 0, false, -1, false);
			}
		}
	}

	public class S_Red : Skill_Extended, IP_SkillUse_User_After, IP_DamageChange
	{
		bool removedTaunt = false;
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);

			if (Targets[0] is BattleEnemy && (Targets[0] as BattleEnemy).istaunt)
			{
				Targets[0].BuffRemove(GDEItemKeys.Buff_B_EnemyTaunt);
				Targets[0].BuffRemove("B_Priest_3_T_0");
				Targets[0].BuffRemove("B_Lucy_30_2");
				removedTaunt = true;
			}
		}

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (removedTaunt)
            {
                return Damage * 2;
            }
            return Damage;
        }

		public void SkillUseAfter(Skill SkillD)
		{
			removedTaunt = false;
		}
	}

	public class S_RCT : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);

			BattleChar hit = Targets[0];
			List<Buff> list = new List<Buff>();
			foreach (Buff buff in hit.Buffs)
			{
				if (buff.BuffData.Debuff && !buff.BuffData.Cantdisable && !buff.BuffData.Hide && !buff.DestroyBuff)
				{
					list.Add(buff);
				}
			}
			if (list.Count != 0)
			{
				hit.BuffRemove(list.Random(hit.GetRandomClass().Main).BuffData.Key, false);
			}
		}
	}

	public class S_SimpleDomain : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
		}
	}

	public class S_HollowPurple : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.PlusSkillStat.Penetration = 100f;
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{

			bool isTaunt = false;
			if (Targets[0] is BattleEnemy)
			{
				if ((Targets[0] as BattleEnemy).istaunt)
				{
					isTaunt = true;
				}
				using (List<BattleEnemy>.Enumerator enumerator = BattleSystem.instance.EnemyList.GetEnumerator())
				{
					if (isTaunt)
                    {
						while (enumerator.MoveNext())
						{
							BattleEnemy battleEnemy = enumerator.Current;
							if (battleEnemy != Targets[0] && battleEnemy.istaunt)
							{
								Targets.Add(battleEnemy);
							}
						}
					}
					else
                    {
						while (enumerator.MoveNext())
						{
							BattleEnemy battleEnemy = enumerator.Current;
							if (battleEnemy != Targets[0] && !battleEnemy.istaunt)
							{
								Targets.Add(battleEnemy);
							}
						}
					}

				}
			}

			foreach (BattleChar b in Targets)
			{
				if (b is BattleEnemy && !(b as BattleEnemy).Boss)
				{
					List<Buff> list = new List<Buff>();
					foreach (Buff buff in b.Buffs)
					{
						if (!buff.BuffData.Debuff && !buff.BuffData.Hide && !(buff.BuffData.Key == "B_EnemyTaunt"))
						{
							list.Add(buff);
						}
					}
					for (int i = 0; i < list.Count; i++)
					{
						b.BuffRemove(list[i].BuffData.Key);
					}
				}
			}

			base.SkillUseSingle(SkillD, Targets);
		}
	}

	public class S_UnlimitedVoid : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);

			// Remove all enemy actions
			for (int i = BattleSystem.instance.EnemyCastSkills.Count - 1; i >= 0; i--)
			{
				BattleSystem.instance.ActWindow.CastingWaste(BattleSystem.instance.EnemyCastSkills[i].skill);
				BattleSystem.instance.EnemyCastSkills.RemoveAt(i);
			}
		}
	}

}
