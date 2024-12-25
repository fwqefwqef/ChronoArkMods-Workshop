using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GameDataEditor;
using UnityEngine;

namespace NightmareEve
{
	using System;

	public class S_Abbadon_0 : Skill_Extended, IP_DamageChange
	{
		public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
		{
			if (Target.HP > 0)
			{
				Target.HPToZero();
				return 0;
			}
			return Damage;
		}
	}

	public class NoTargetView : Skill_Extended
    {
		public override void Init()
		{
			base.Init();
			this.EnemyPreviewNoArrow = true;
		}
	}
	public class S_Medusa_0 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.EnemyPreviewNoArrow = true;
		}

		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			BattleSystem.DelayInput(this.AfterAttack(Targets[0]));
		}

		public IEnumerator AfterAttack(BattleChar Target)
		{
			yield return new WaitForSecondsRealtime(0.1f);
			if (Target.HP >= 1)
			{
				Skill skill = Skill.TempSkill("S_Medusa_0_1", this.BChar, this.BChar.MyTeam);
				this.BChar.ParticleOut(skill, Target);
			}
			yield break;
		}
	}

	// Zandyne
	public class S_Matador_2 : Skill_Extended, IP_SkillUse_Target
	{
		public override void Init()
		{
			base.Init();
			//this.EnemyPreviewNoArrow = true;
		}

		public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
		{
			if (DMG >= 1)
			{
				List<Skill> skills = BattleSystem.instance.AllyTeam.Skills;
				for (int i = 0; i < skills.Count; i++)
                {
					if (skills[i].Master == hit)
                    {
						skills[i].Delete(false);
						i--;
					}
				}
			}
		}
	}

	public class S_Matador_4 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.EnemyPreviewNoArrow = true;
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill = Skill.TempSkill("S_Matador_4_0", this.BChar, this.BChar.MyTeam);

			List<BattleChar> targets = BattleSystem.instance.AllyTeam.AliveChars;
			targets.AddRange(targets);
			targets.Remove(Targets[0]);
			
			foreach(BattleChar target in targets)
            {
				Debug.Log(target);
            }

			for (int i = 0; i < 3; i++)
			{
				if (targets.Count == 0)
                {
					break;
                }
				BattleChar pop = targets.Random();
				targets.Remove(pop);
				BattleSystem.DelayInput(this.Attack(skill, pop));
			}
		}

		public IEnumerator Attack(Skill skill, BattleChar target)
		{
			yield return new WaitForSeconds(0.05f);
			this.BChar.ParticleOut(skill, target);
			yield break;
		}
	}

	public class S_WhiteRider_0 : Skill_Extended, IP_TargetAI
	{
		public List<BattleChar> TargetAI(BattleEnemy MyBchar)
		{
			return new List<BattleChar> { BattleSystem.instance.AllyTeam.FindChar_LowHP() };
		}
	}

	public class S_BlackRider_0 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.EnemyPreviewNoArrow = true;
		}
		public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
		{
			base.AttackEffectSingle(hit, SP, DMG, Heal);
			foreach (BattleChar battleChar in this.BChar.MyTeam.AliveChars)
			{
				battleChar.Heal(this.BChar, (float)DMG, false, false, null);
			}
		}
	}

	public class S_PaleRider_1 : Skill_Extended, IP_DamageChange
	{
		public override void Init()
		{
			base.Init();
		}

		public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
		{
			if (Target.BuffReturn("B_Common_Rest") != null)
			{
				return Damage * 2;
			}
			return Damage;
		}
	}

	// Hell Thrust
	public class S_Belial_2 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.EnemyPreviewNoArrow = true;
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill = Skill.TempSkill("S_Belial_2_0", this.BChar, this.BChar.MyTeam);
			for (int i = 0; i < 2; i++)
			{
				BattleSystem.DelayInput(this.Attack(skill, Targets[0]));
			}
		}

		public IEnumerator Attack(Skill skill, BattleChar target)
		{
			yield return new WaitForSeconds(0.1f);
			this.BChar.ParticleOut(skill, target);
			yield break;
		}
	}

	// Summon
	public class S_Belial_4 : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos("S4_MagicDochi", null));
		}
	}

	// Flames of Gomorah
	public class S_Belial_5 : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			this.BChar.BuffRemove("B_Invincible");

			Skill skill = Skill.TempSkill("S_Belial_6", this.BChar, this.BChar.MyTeam);
			for (int i = 0; i < Targets.Count; i++)
			{
				BattleSystem.DelayInput(this.Attack(skill, Targets));
			}
		}
		public IEnumerator Attack(Skill skill, List<BattleChar> Targets)
		{
			yield return new WaitForSeconds(0.1f);
			this.BChar.ParticleOut(skill, Targets);
			yield break;
		}
	}

	public class S_Fervor : Skill_Extended
	{

	}
	public class S_Sacrifice : Skill_Extended
    {
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			Targets[0].Dead(false);
			BattleSystem.instance.AllyTeam.AP += 1;
		}
	}

	// Pestilence (Beelzebub)
	public class S_Beel_1 : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);

			foreach (BattleChar b in Targets)
            {
				b.BuffRemove("B_Egg_0");
				b.BuffRemove("B_Egg_1");
				b.BuffRemove("B_Egg_2");
			}

			List<Skill> skills = BattleSystem.instance.AllyTeam.Skills;
			for (int i = 0; i < skills.Count; i++)
			{
				if (skills[i].MySkill.KeyID == "S_Hatch")
				{
					skills[i].Delete();
					i--;
				}
			}
		}
    }

	// Deathbound
    public class S_Beel_2 : Skill_Extended 
	{
		public override void Init()
		{
			base.Init();
			this.EnemyPreviewNoArrow = true;
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill = Skill.TempSkill("S_Beel_2_0", this.BChar, this.BChar.MyTeam);

			List<BattleChar> targets = BattleSystem.instance.AllyTeam.AliveChars;
			targets.AddRange(targets);
			targets.Remove(Targets[0]);

			foreach (BattleChar target in targets)
			{
				Debug.Log(target);
			}

			for (int i = 0; i < 5; i++)
			{
				if (targets.Count == 0)
				{
					break;
				}
				BattleChar pop = targets.Random();
				targets.Remove(pop);
				BattleSystem.DelayInput(this.Attack(skill, pop));
			}
		}

		public IEnumerator Attack(Skill skill, BattleChar target)
		{
			yield return new WaitForSeconds(0.05f);
			this.BChar.ParticleOut(skill, target);
			yield break;
		}
	}

	// Thunder Reign
	public class S_Beel_4 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.EnemyPreviewNoArrow = true;
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			List<Skill> list = new List<Skill>();
			foreach (BattleChar battleChar in BattleSystem.instance.AllyTeam.AliveChars)
			{
				if (!(battleChar.Info.KeyData == GDEItemKeys.Character_Phoenix) || battleChar.HP >= 0)
				{
					Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_LBossFirst_2_Plus, battleChar, battleChar.MyTeam);
					list.Add(skill);
				}
			}
			BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), "", false, false, true, false, true));
		}
		public void Del(SkillButton Mybutton)
		{
			Skill skill = Skill.TempSkill("S_Beel_4_0", this.BChar, this.BChar.MyTeam);
			this.BChar.ParticleOut(skill, Mybutton.Myskill.Master);
		}
	}

	// Death Flies
	public class S_Beel_5 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			this.BChar.BuffRemove("B_Invincible");
			List<string> list = new List<string>() { "B_Egg_0", "B_Egg_1", "B_Egg_2" };
			Debug.Log(list);
			foreach (BattleChar b in Targets)
            {
				b.BuffAdd(list.Random(), this.BChar);
            }
		}
	}
	public class S_Beel_4_0 : Skill_Extended
	{
		public override string DescExtended(string desc)
		{
			return base.DescExtended(desc).Replace("&a", (this.BChar.GetStat.atk * 1.15).ToString());
		}
	}

	// Life Drain
	public class S_Fly_0 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			//this.EnemyPreviewNoArrow = true;
		}
		public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
		{
			base.AttackEffectSingle(hit, SP, DMG, Heal);
			foreach (BattleChar battleChar in this.BChar.MyTeam.AliveChars)
			{
				if (battleChar == this.BChar || battleChar.Info.KeyData == "Beelzebub")
                {
					battleChar.Heal(this.BChar, (float)DMG, false, false, null);
				}
			}
		}
	}

	public class S_Fly_1 : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			//this.EnemyPreviewNoArrow = true;
		}
		public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
		{
			if (DMG >= 10)
            {
				BattleSystem.instance.AllyTeam.AP -= 1;
			}
		}
	}

	public class S_Hatch : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
			this.CanUseStun = true;
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);

			foreach (BattleChar b in Targets)
			{
				b.BuffRemove("B_Egg_0");
				b.BuffRemove("B_Egg_1");
				b.BuffRemove("B_Egg_2");
			}
		}
	}

	// Fatal Sword
	public class S_Michael_0 : Skill_Extended, IP_TargetAI
	{
		public List<BattleChar> TargetAI(BattleEnemy MyBchar)
		{
			return new List<BattleChar> { BattleSystem.instance.AllyTeam.FindChar_LowHP() };
		}
	}

	// Prayer
	public class S_Gabriel_1 : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			foreach (BattleChar battleChar in Targets)
			{
				List<Buff> buffs = battleChar.GetBuffs(BattleChar.GETBUFFTYPE.ALLDEBUFF, true, false);
				if (buffs.Count >= 1)
				{
					buffs.Random(this.BChar.GetRandomClass().Main).SelfDestroy(false);
				}
			}
		}
	}

	// Megidoladyne
	public class S_Metatron_0 : Skill_Extended, IP_TargetAI
	{
		public int NowDamage
		{
			get
			{
				Buff buff = this.BChar.BuffReturn("B_Gathering", false);
				if (buff == null)
				{
					return 80;
				}
				return 80 + buff.StackNum * 10;
			}
		}
		public override string DescExtended(string desc)
		{
			return base.DescExtended(desc).Replace("&a", this.NowDamage.ToString());
		}

		public override void Init()
		{
			base.Init();
			this.EnemyTargetAIOnly = true;
			this.IsDamage = true;
		}

		public List<BattleChar> TargetAI(BattleEnemy MyBchar)
		{
			List<BattleChar> list = new List<BattleChar>();
			foreach (BattleChar battleChar in BattleSystem.instance.AllyTeam.AliveChars)
			{
				if (!battleChar.BuffFind(GDEItemKeys.Buff_B_Neardeath, false))
				{
					list.Add(battleChar);
				}
			}
			if (list.Count == 0)
			{
				list.AddRange(BattleSystem.instance.AllyTeam.AliveChars);
				return list;
			}
			int num = this.NowDamage;
			num /= list.Count;
			this.SkillBasePlus.Target_BaseDMG = num;
			return list;
		}
	}

	// Raging Whirlwind
	public class S_Metatron_2 : Skill_Extended, IP_SkillUse_Target
	{
		public override void Init()
		{
			base.Init();
			//this.EnemyPreviewNoArrow = true;
		}

		public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
		{
			if (DMG >= 1)
			{
				List<Skill> skills = BattleSystem.instance.AllyTeam.Skills;
				for (int i = 0; i < skills.Count; i++)
				{
					if (skills[i].Master == hit)
					{
						skills[i].Delete(false);
						i--;
					}
				}
			}
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000AA9B8 File Offset: 0x000A8BB8
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			List<BattleChar> list = new List<BattleChar>();
			if (BattleSystem.instance.AllyList.Count == 2 || BattleSystem.instance.AllyList.Count == 3)
			{
				BattleSystem.DelayInput(this.Damage(Targets[0].MyLeftCharReturn()));
				BattleSystem.DelayInput(this.Damage(Targets[0].MyLeftCharReturn().MyLeftCharReturn()));
				return;
			}
			if (BattleSystem.instance.AllyList.Count >= 4)
			{
				list.Add(Targets[0].MyLeftCharReturn());
				list.Add(Targets[0].MyRightCharReturn());
			}
			List<Skill> list2 = new List<Skill>();
			Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_ProgramMaster_0_1, list[0], list[0].MyTeam);
			(skill.ExtendedFind("S_ProgramMaster_0_1", true) as S_ProgramMaster_0_1).Damage = (int)(this.BChar.GetStat.atk * 0.6f);
			list2.Add(skill);
			Skill skill2 = Skill.TempSkill(GDEItemKeys.Skill_S_ProgramMaster_0_2, list[1], list[1].MyTeam);
			(skill2.ExtendedFind("S_ProgramMaster_0_1", true) as S_ProgramMaster_0_1).Damage = (int)(this.BChar.GetStat.atk * 0.6f);
			list2.Add(skill2);
			BattleSystem.DelayInputAfter(BattleSystem.I_OtherSkillSelect(list2, new SkillButton.SkillClickDel(this.Del), "", false, false, true, false, true));
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000AAB2A File Offset: 0x000A8D2A
		public IEnumerator Damage(BattleChar Target)
		{
			Skill skill = Skill.TempSkill("S_Metatron_2_0", this.BChar, this.BChar.MyTeam);
			this.BChar.ParticleOut(skill, Target);
			yield return new WaitForSecondsRealtime(0.2f);
			yield break;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000AAB40 File Offset: 0x000A8D40
		public void Del(SkillButton Mybutton)
		{
			BattleSystem.DelayInput(this.Damage(Mybutton.Myskill.Master));
			if (Mybutton.Myskill.MySkill.KeyID == GDEItemKeys.Skill_S_ProgramMaster_0_1)
			{
				BattleSystem.DelayInput(this.Damage(Mybutton.Myskill.Master.MyLeftCharReturn()));
				return;
			}
			BattleSystem.DelayInput(this.Damage(Mybutton.Myskill.Master.MyRightCharReturn()));
		}
	}

	// Fire of Sinai
	public class S_Metatron_4 : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
		}

		public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
		{
				List<Buff> buffs = hit.GetBuffs(BattleChar.GETBUFFTYPE.BUFF, true, false);
				foreach (Buff b in buffs)
				{
					b.SelfDestroy();
				}
			
		}
	}

	// Recarm
	public class S_Metatron_6 : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);

			List<string> enemies = new List<string>() {"Michael", "Uriel", "Gabriel", "Raphael"};
			foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
				if (b.Info.KeyData == "Michael" || b.Info.KeyData == "Uriel" || b.Info.KeyData == "Gabriel" || b.Info.KeyData == "Raphael")
                {
					enemies.Remove(b.Info.KeyData);	
                }
            }

			BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos(enemies.Random(), null));
		}
	}

	// Blessing
	public class S_Blessing : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			foreach (BattleChar battleChar in Targets)
			{
				List<Buff> buffs = battleChar.GetBuffs(BattleChar.GETBUFFTYPE.ALLDEBUFF, true, false);
				foreach (Buff b in buffs)
                {
					b.SelfDestroy();
                }
			}
		}
	}
}
