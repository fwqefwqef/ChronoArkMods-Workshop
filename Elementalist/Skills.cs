using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;
using I2.Loc;
using System.Collections;
using UnityEngine;
using DarkTonic.MasterAudio;
using Random = System.Random;

namespace Elementalist
{
	public class S_Transfer : Skill_Extended
	{
		BattleChar target = null;
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			target = Targets[0];
			Skill skill1 = Skill.TempSkill("S_Transfer_0", this.BChar, this.BChar.MyTeam);
			Skill skill2 = Skill.TempSkill("S_Transfer_1", this.BChar, this.BChar.MyTeam);
			List<Skill> list = new List<Skill>();
			list.Add(skill1);
			list.Add(skill2);

			BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, true, true, false, false));
		}
		public void Del(SkillButton Mybutton)
		{
			if (Mybutton.Myskill.MySkill.KeyID == "S_Transfer_0")
			{
				this.MySkill.Master.MyTeam.DiscardCount += 3;
			}
			if (Mybutton.Myskill.MySkill.KeyID == "S_Transfer_1")
			{
				target.BuffAdd("B_TransferRune", this.BChar, false, 0, false, -1, false);
			}
		}
	}

	public class S_Channel : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Random random = new Random();
			int number = random.Next(3);
			if (number == 0)
			{
				Skill skill = Skill.TempSkill("S_Fire", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				skill.isExcept = true;
			}
			else if (number == 1)
			{
				Skill skill = Skill.TempSkill("S_Elec", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				skill.isExcept = true;
			}
			else
			{
				Skill skill = Skill.TempSkill("S_Dark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				skill.isExcept = true;
			}

			number = random.Next(3);
			if (number == 0)
			{
				Skill skill2 = Skill.TempSkill("S_Fire", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill2, true);
				skill2.isExcept = true;
			}
			else if (number == 1)
			{
				Skill skill2 = Skill.TempSkill("S_Elec", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill2, true);
				skill2.isExcept = true;
			}
			else
			{
				Skill skill2 = Skill.TempSkill("S_Dark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill2, true);
				skill2.isExcept = true;
			}
		}
	}

	public class S_Anima_2 : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill = Skill.TempSkill("S_Anima_2_0", this.BChar, this.BChar.MyTeam);
			//skill.isExcept = true;
			skill.FreeUse = true;
			skill.PlusHit = true;

			int count = 0;
			List<Skill> hand = this.BChar.MyTeam.Skills;
			for (int i = 0; i < hand.Count; i++)
            {
				if (hand[i].MySkill.KeyID == "S_Fire" || hand[i].MySkill.KeyID == "S_Elec" || hand[i].MySkill.KeyID == "S_Dark")
				{
					hand[i].Delete(false);
					count++;
					i--;
				}
			}

			for(int i = 0; i < count; i++)
            {
				BattleSystem.DelayInputAfter(this.Attack(skill, Targets[0]));
			}
		}
		public IEnumerator Attack(Skill skill, BattleChar Target)
		{
			yield return new WaitForSeconds(0.1f);
			this.BChar.ParticleOut(this.MySkill, skill, this.BChar.BattleInfo.EnemyList.Random(this.BChar.GetRandomClass().Main));
			yield break;
		}
	}

	public class S_ManaStorm : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill = Skill.TempSkill("S_ManaStorm_0", this.BChar, this.BChar.MyTeam);
			//skill.isExcept = true;
			skill.FreeUse = true;
			skill.PlusHit = true;

			int count = 0;
			List<Skill> hand = this.BChar.MyTeam.Skills;
			for (int i = 0; i < hand.Count; i++)
			{
				if (hand[i].MySkill.KeyID == "S_Fire" || hand[i].MySkill.KeyID == "S_Elec" || hand[i].MySkill.KeyID == "S_Dark")
				{
					hand[i].Delete(false);
					count++;
					i--;
					//break; // only discards one now
				}
			}
			for (int i = 0; i < count; i++)
			{
				BattleSystem.DelayInputAfter(this.Attack(skill, Targets[0]));
			}
		}
		public IEnumerator Attack(Skill skill, BattleChar Target)
		{
			yield return new WaitForSeconds(0.1f);
			if (Target.IsDead)
            {
				this.BChar.ParticleOut(this.MySkill, skill, this.BChar.BattleInfo.EnemyList.Random(this.BChar.GetRandomClass().Main));
			}
            else
            {
				this.BChar.ParticleOut(this.MySkill, skill, Target);
			}
			yield break;
		}
	}

	public class S_Enchant : Skill_Extended
	{
		BattleChar target = null;
		Skill tempfire = null;
		Skill tempelec = null;
		Skill tempdark = null;
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill fire = null;
			Skill elec = null;
			Skill dark = null;
			foreach (Skill s in this.BChar.MyTeam.Skills) 
            {
				if (s.MySkill.KeyID == "S_Fire" && fire == null)
                {
					fire = s;
                }
				else if (s.MySkill.KeyID == "S_Elec" && elec == null)
				{
					elec = s;
				}
				else if (s.MySkill.KeyID == "S_Dark" && dark == null)
				{
					dark = s;
				}
			}
			// No elements found
			if (fire == null && elec == null && dark == null)
			{
				Debug.Log("No elements");
				return;
			}

			// Only Fire
			if (fire != null && elec == null && dark == null)
            {
				Debug.Log("Only Fire");
				Targets[0].BuffAdd("B_EnchantFire", this.BChar, false, 0, false, -1, false);
				fire.Delete(false);
				//BattleSystem.instance.AllyTeam.Draw(1);
			}
			// Only Elec
			else if (fire == null && elec != null && dark == null)
			{
				Debug.Log("Only Elec");
				Targets[0].BuffAdd("B_EnchantElec", this.BChar, false, 0, false, -1, false);
				elec.Delete(false);
				//BattleSystem.instance.AllyTeam.Draw(1);
			}
			// Only Dark
			else if (fire == null && elec == null && dark != null)
			{
				Debug.Log("Only Dark");
				Targets[0].BuffAdd("B_EnchantDark", this.BChar, false, 0, false, -1, false);
				dark.Delete(false);
				//BattleSystem.instance.AllyTeam.Draw(1);
			}
			// More than 1
			else
			{
				Debug.Log("More than 1 element");
				Skill enchantFire = Skill.TempSkill("S_EnchantFire", this.BChar, BattleSystem.instance.AllyTeam);
				Skill enchantElec = Skill.TempSkill("S_EnchantElec", this.BChar, BattleSystem.instance.AllyTeam);
				Skill enchantDark = Skill.TempSkill("S_EnchantDark", this.BChar, BattleSystem.instance.AllyTeam);
				List<Skill> menu = new List<Skill>();
				if (fire!=null)
                {
					menu.Add(enchantFire);
                }
				if (elec != null)
				{
					menu.Add(enchantElec);
				}
				if (dark != null)
				{
					menu.Add(enchantDark);
				}
				target = Targets[0];
				tempfire = fire;
				tempelec = elec;
				tempdark = dark;
				BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(menu, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, true, true, false, false));
			}
		}
		public void Del(SkillButton Mybutton)
		{
			if (Mybutton.Myskill.MySkill.KeyID == "S_EnchantFire")
			{
				target.BuffAdd("B_EnchantFire", this.BChar, false, 0, false, -1, false);
				tempfire.Delete(false);
				BattleSystem.instance.AllyTeam.Draw(1);
			}
			if (Mybutton.Myskill.MySkill.KeyID == "S_EnchantElec")
			{
				target.BuffAdd("B_EnchantElec", this.BChar, false, 0, false, -1, false);
				tempelec.Delete(false);
				BattleSystem.instance.AllyTeam.Draw(1);
			}
			if (Mybutton.Myskill.MySkill.KeyID == "S_EnchantDark")
			{
				target.BuffAdd("B_EnchantDark", this.BChar, false, 0, false, -1, false);
				tempdark.Delete(false);
				BattleSystem.instance.AllyTeam.Draw(1);
			}
		}
	}

	public class S_Fire : Skill_Extended
	{
		public override void SkillTargetSingle(List<Skill> Targets)
		{
			if (Targets[0].MySkill.KeyID == "S_Fire")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_FireFire", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_Elec")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_ElecFire", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_Dark")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_DarkFire", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_FireFire")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_FireFireFire", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_ElecDark" || Targets[0].MySkill.KeyID == "S_DarkElec")
			{
				Targets[0].Delete(true);
				Skill skill = Skill.TempSkill("S_Spectrum", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else
            {
				Targets[0].ExtendedAdd("Ex_Fire");
            }
		}
	}

	public class S_Dark : Skill_Extended
	{
		public override void SkillTargetSingle(List<Skill> Targets)
		{
			if (Targets[0].MySkill.KeyID == "S_Fire")
            {
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_FireDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				Skill skill2 = Skill.TempSkill("S_FireDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill2, true);
				Skill skill3 = Skill.TempSkill("S_FireDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill3, true);

				this.BChar.BuffAdd("B_FireDark", this.BChar, false, 0, false, -1, false);

				//foreach(BattleChar b in BattleSystem.instance.AllyTeam.Chars)
				//            {
				//	b.BuffAdd("B_FireDark", this.BChar, false, 0, false, -1, false);
				//}
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_Elec")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_ElecDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				Skill skill2 = Skill.TempSkill("S_ElecDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill2, true);
				Skill skill3 = Skill.TempSkill("S_ElecDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill3, true);
				//this.BChar.Overload = 0;
			}
			else if(Targets[0].MySkill.KeyID == "S_Dark")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_DarkDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if(Targets[0].MySkill.KeyID == "S_DarkDark" || Targets[0].MySkill.KeyID == "S_DarkDark_0")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_DarkDarkDark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_FireElec" || Targets[0].MySkill.KeyID == "S_ElecFire")
			{
				Targets[0].Delete(true);
				Skill skill = Skill.TempSkill("S_Spectrum", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else
            {
				Targets[0].ExtendedAdd("Ex_Dark");
			}
		}
	}

	public class S_Elec : Skill_Extended
	{
		public override void SkillTargetSingle(List<Skill> Targets)
		{
			if (Targets[0].MySkill.KeyID == "S_Fire")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_FireElec", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_Elec")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_ElecElec", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_Dark")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_DarkElec", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_ElecElec")
			{
				Targets[0].Delete(false);
				Skill skill = Skill.TempSkill("S_ElecElecElec", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else if (Targets[0].MySkill.KeyID == "S_FireDark" || Targets[0].MySkill.KeyID == "S_DarkFire")
			{
				Targets[0].Delete(true);
				Skill skill = Skill.TempSkill("S_Spectrum", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
				//this.BChar.Overload = 0;
			}
			else
			{
				Targets[0].ExtendedAdd("Ex_Elec");
			}
		}
	}

	public class S_DarkFire : Skill_Extended, IP_SkillUse_Target
	{
		public float healnum;

		public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
		{
			if (SP.SkillData.MySkill.KeyID != "S_DarkFire")
            {
				return;
            }
			healnum = 0;
			if (DMG != 0)
			{
				healnum = Misc.PerToNum((float)DMG, 50f);
				Debug.Log("Added " + healnum);
			}
			List<Skill> list = new List<Skill>();
			foreach (Skill s in BattleSystem.instance.AllyTeam.Skills)
			{
				if (s.MySkill.KeyID == "S_DarkFire_0")
				{
					list.Add(s);
					break;
				}
			}
			if (list.Count == 0)
			{
				list.Add(Skill.TempSkill("S_DarkFire_0", BChar, BChar.MyTeam));
				Skill_Extended heal = new Skill_Extended();
				heal.SkillBasePlus.Target_BaseHeal = (int)healnum;
				list[0].ExtendedAdd(heal);
				BattleSystem.instance.StartCoroutine(BattleSystem.instance.AllyTeam._Add(list[0], true, -1));
			}
			else
            {
				Skill_Extended heal = new Skill_Extended();
				heal.SkillBasePlus.Target_BaseHeal = (int)healnum;
				list[0].ExtendedAdd(heal); 
			}
		}
	}

	public class S_DarkElec : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			foreach (BattleChar b in Targets)
            {
				if ((b as BattleEnemy) != null && (b as BattleEnemy).istaunt)
				{
					(b as BattleEnemy).BuffScriptReturn("Common_Buff_EnemyTaunt").SelfDestroy(false);
				}
				else
				{
					(b as BattleEnemy).BuffAdd(GDEItemKeys.Buff_B_EnemyTaunt, (b as BattleEnemy), false, 0, false, -1, false);
				}
                //foreach (CastingSkill castingSkill in (b as BattleEnemy).SkillQueue)
                //{
                //    castingSkill._CastSpeed += 3;
                //}
            }
		}
	}

	public class S_DarkBase : Skill_Extended
	{
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill = Skill.TempSkill("S_Dark", this.BChar, BattleSystem.instance.AllyTeam);
			BattleSystem.instance.AllyTeam.Add(skill, true);

			//Skill skill2 = Skill.TempSkill("S_DarkBase_0", this.BChar, BattleSystem.instance.AllyTeam);
			//BattleSystem.instance.AllyTeam.Add(skill2, true);
		}
	}

	//public class S_DarkBase_0 : Skill_Extended
	//{
	//	public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
	//	{

	//	}
	//}

	public class S_DarkDark : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill2 = Skill.TempSkill("S_DarkDark_0", this.BChar, BattleSystem.instance.AllyTeam);
			BattleSystem.instance.AllyTeam.Add(skill2, true);
		}
	}
	public class S_DarkDarkDark : Skill_Extended
    {

    }

	public class S_ElecFire : Skill_Extended
	{
		public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
		{
			int num = 0;
			List<BattleEnemy> list = (hit as BattleEnemy).EnemyPosNum(out num);
			List<BattleChar> list2 = new List<BattleChar>();
			if (num - 1 >= 0)
			{
				list2.Add(list[num - 1]);
				
				if (num - 2 >= 0)
				{
					list2.Add(list[num - 2]);
				}
				else
				{
					list2.Add(list[num]);
				}
			}
			else if (num == 0 && list.Count > 1) // bounce right
            {
				list2.Add(list[num+1]);
				if (num + 2 < list.Count)
				{
					list2.Add(list[num + 2]);
				}
				else {
					list2.Add(list[num]);
				}
			}
			BattleSystem.instance.StartCoroutine(this.PlusAttack(list2));
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0008B1F2 File Offset: 0x000893F2
		public IEnumerator PlusAttack(List<BattleChar> CharList)
		{
			if (CharList.Count != 0)
			{
				Skill skill = Skill.TempSkill("S_ElecFire_0", this.BChar, this.BChar.MyTeam);
				skill.isExcept = true;
				skill.FreeUse = true;
				skill.PlusHit = true;
				foreach (BattleChar b in CharList)
				{
					yield return new WaitForSecondsRealtime(0.2f);
					this.BChar.ParticleOut(this.MySkill, skill, b);
				}
			}
			yield break;
		}
	}

	public class S_ElecBase : Skill_Extended //, IP_Kill
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			if (Misc.NumToPer((float)Targets[0].GetStat.maxhp, (float)Targets[0].HP) <= 50f)
			{
				this.PlusSkillStat.cri = 100f;
			}

			Skill skill = Skill.TempSkill("S_Elec", this.BChar, BattleSystem.instance.AllyTeam);
			BattleSystem.instance.AllyTeam.Add(skill, true);
		}
    }

	public class S_ElecElec : Skill_Extended //, IP_Kill
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			if (Misc.NumToPer((float)Targets[0].GetStat.maxhp, (float)Targets[0].HP) <= 90f)
			{
				this.PlusSkillStat.cri = 100f;
			}
		}
	}

	public class S_ElecDark : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			// 1. find location of this skill in hand
			int index = this.BChar.MyTeam.Skills.IndexOf(this.MySkill);
			Debug.Log(index);

			// 2. Find skill to cast from below
			Skill next = this.BChar.MyTeam.Skills[index + 1];

			List<Skill> hand = this.BChar.MyTeam.Skills;
			for (int i = index+1; i < hand.Count; i++)
            {
				if (hand[i].MySkill.KeyID == "S_ElecDark")
				{
					hand[i].Delete(false);
					BattleSystem.DelayInputAfter(this.Attack(SkillD, Targets[0]));
					i--;
				}
				else
                {
					break;
                }
			}
		}
		public IEnumerator Attack(Skill skill, BattleChar target)
		{
			//yield return new WaitForSeconds(0.1f);
			Skill skill2 = Skill.TempSkill("S_ElecDark", this.BChar, this.BChar.MyTeam);
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

	public class S_ElecElecElec : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			this.PlusSkillStat.PlusCriDmg = 100;
			float num = (float)this.MySkill.GetCriPer(Targets[0], 0);
			if (num >= 100f)
			{
				this.SkillBasePlus.Target_BaseDMG = (int)Misc.PerToNum((float)this.MySkill.TargetDamage, (float)((int)(num - 100f)));
			}
		}
	}

	public class S_FireBase : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			Skill skill = Skill.TempSkill("S_Fire", this.BChar, BattleSystem.instance.AllyTeam);
			BattleSystem.instance.AllyTeam.Add(skill, true);
		}
	}
	public class S_FireFire : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{

		}
	}

	public class S_FireElec : Skill_Extended
	{
		public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
		{
			if (!hit.IsDead)
            {
				return;
            }
			int num = 0;
			List<BattleEnemy> list = (hit as BattleEnemy).EnemyPosNum(out num);
			List<BattleChar> list2 = new List<BattleChar>();
			if (num - 1 >= 0)
			{
				list2.Add(list[num - 1]);
			}
			if (num + 1 < list.Count)
			{
				list2.Add(list[num + 1]);
			}
			BattleSystem.instance.StartCoroutine(this.Delay());
			BattleSystem.instance.StartCoroutine(this.PlusAttack(list2));
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0008B1F2 File Offset: 0x000893F2
		public IEnumerator PlusAttack(List<BattleChar> CharList)
		{
			if (CharList.Count != 0)
			{
				Skill skill = Skill.TempSkill("S_FireElec_0", this.BChar, this.BChar.MyTeam);
				skill.isExcept = true;
				skill.FreeUse = true;	
				skill.PlusHit = true;
				foreach (BattleChar b in CharList)
				{
					this.BChar.ParticleOut(this.MySkill, skill, b);
				}
			}
			yield break;
		}
		public IEnumerator Delay()
		{
			yield return new WaitForSecondsRealtime(0.4f);
			yield break;
		}
	}

	public class S_FireDark : Skill_Extended
	{

	}

	public class S_FireFireFire : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			base.SkillUseSingle(SkillD, Targets);
			int recast = Targets.Count;

			BattleSystem.DelayInput(this.Delay());
			Skill skill = Skill.TempSkill("S_FireFireFire_0", this.BChar, this.BChar.MyTeam);
			skill.isExcept = true;
			skill.FreeUse = true;
			skill.PlusHit = true;

			for (int i = 0; i < recast; i++)
			{
				BattleSystem.DelayInput(this.Damage(skill));
			}
		}
		public IEnumerator Damage(Skill skill)
		{
			this.BChar.ParticleOut(this.MySkill, skill, this.BChar.BattleInfo.EnemyList.Random<BattleEnemy>());
			yield return new WaitForSecondsRealtime(0.1f);
			yield break;
		}

		public IEnumerator Delay()
		{
			yield return new WaitForSecondsRealtime(0.6f);
			yield break;
		}
	}

	public class S_Echo : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{

		}
	}

	public class S_Omni : Skill_Extended
	{
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)////
		{
			MasterAudio.PlaySound("Friendly2", 1f, null, 0f, null, null, false, false);

			if (this.MySkill.BasicSkill)
            {
				Random random = new Random();
				int number = random.Next(3);
				if (number == 0)
				{
					Skill skill = Skill.TempSkill("S_Fire", this.BChar, BattleSystem.instance.AllyTeam);
					BattleSystem.instance.AllyTeam.Add(skill, true);
					skill.isExcept = true;
				}
				else if (number == 1)
				{
					Skill skill = Skill.TempSkill("S_Elec", this.BChar, BattleSystem.instance.AllyTeam);
					BattleSystem.instance.AllyTeam.Add(skill, true);
					skill.isExcept = true;
				}
				else
				{
					Skill skill = Skill.TempSkill("S_Dark", this.BChar, BattleSystem.instance.AllyTeam);
					BattleSystem.instance.AllyTeam.Add(skill, true);
					skill.isExcept = true;
				}
			}
			else // non fixed
            {
				List<Skill> list = new List<Skill>();
				list.Add(Skill.TempSkill("S_Fire", this.MySkill.Master, this.MySkill.Master.MyTeam));
				list.Add(Skill.TempSkill("S_Elec", this.MySkill.Master, this.MySkill.Master.MyTeam));
				list.Add(Skill.TempSkill("S_Dark", this.MySkill.Master, this.MySkill.Master.MyTeam));
				BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, true, true, false, false));
			}
		}
		public void Del(SkillButton Mybutton)
		{
			if (Mybutton.Myskill.MySkill.KeyID == "S_Fire")
			{
				Skill skill = Skill.TempSkill("S_Fire", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
			}
			if (Mybutton.Myskill.MySkill.KeyID == "S_Elec")
			{
				Skill skill = Skill.TempSkill("S_Elec", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
			}
			if (Mybutton.Myskill.MySkill.KeyID == "S_Dark")
			{
				Skill skill = Skill.TempSkill("S_Dark", this.BChar, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
			}
		}
    }

	public class CostInc : Skill_Extended
	{
		// Token: 0x0600277E RID: 10110 RVA: 0x0007BAB6 File Offset: 0x00079CB6
		public override void Init()
		{
			base.Init();
			this.APChange = +1;
		}
	}

	public class S_Transmute : Skill_Extended
	{
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			BattleSystem.instance.AllyTeam.Draw(2);

			// find elementalist //
			BattleChar elementalist = null;
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.Chars)
            {
                if (b.Info.KeyData == "Elementalist")
                {
                    elementalist = b;
                }
            }

            if (elementalist == null)
            {
                return;
            }

            Random random = new Random();
			int number = random.Next(3);
			if (number == 0)
			{
				Skill skill = Skill.TempSkill("S_Fire", elementalist, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
			}
			else if (number == 1)
			{
				Skill skill = Skill.TempSkill("S_Elec", elementalist, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
			}
			else
			{
				Skill skill = Skill.TempSkill("S_Dark", elementalist, BattleSystem.instance.AllyTeam);
				BattleSystem.instance.AllyTeam.Add(skill, true);
			}
		}
		//public override void SkillTargetSingle(List<Skill> Targets)
		//{
		//	BattleSystem.instance.AllyTeam.Draw(2);

		//	// find elementalist //
		//	BattleChar elementalist = null;
		//	foreach (BattleChar b in BattleSystem.instance.AllyTeam.Chars)
  //          {
		//		if (b.Info.Name == "Elementalist")
  //              {
		//			elementalist = b;
  //              }
  //          }

		//	if (elementalist == null)
  //          {
		//		return;
  //          }

		//	// give element
		//	Targets[0].Delete(false);

		//	Random random = new Random();
		//	int number = random.Next(3);
		//	if (number == 0)
		//	{
		//		Skill skill = Skill.TempSkill("S_Fire", elementalist, BattleSystem.instance.AllyTeam);
		//		BattleSystem.instance.AllyTeam.Add(skill, true);
		//		skill.isExcept = true;
		//	}
		//	else if (number == 1)
		//	{
		//		Skill skill = Skill.TempSkill("S_Elec", elementalist, BattleSystem.instance.AllyTeam);
		//		BattleSystem.instance.AllyTeam.Add(skill, true);
		//		skill.isExcept = true;
		//	}
		//	else
		//	{
		//		Skill skill = Skill.TempSkill("S_Dark", elementalist, BattleSystem.instance.AllyTeam);
		//		BattleSystem.instance.AllyTeam.Add(skill, true);
		//		skill.isExcept = true;
		//	}
		//}
	}

	public class S_Spectrum : Skill_Extended
	{
		public override void Init()
		{
			base.Init();
		}
		public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
		{
			BattleChar hit = new BattleChar();
			foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
				if (!b.IsDead)
                {
					hit = b;
                }
            }

			int num;
			List<BattleEnemy> list = (hit as BattleEnemy).EnemyPosNum(out num);
			//List<BattleChar> enemies = BattleSystem.instance.EnemyTeam.AliveChars;
			List<int> target = new List<int>();
			List<BattleChar> targetChar = new List<BattleChar>();
			Random random = new Random();
			for (int i = 0; i < 5; i++)
            {
				if (list.Count == 1 && target.Count == 3)
				{
					break;
                }
				int number = random.Next(list.Count);
				if (target.Count(item => item == number) < 3) // 3 instances
                {
					target.Add(number);
					targetChar.Add(list[number]);
				}
				else
                {
					i--;
                }
			}
			BattleSystem.DelayInput(this.Attack(target, targetChar));
		}
		public IEnumerator Attack(List<int> target, List<BattleChar> targetChar)
		{
			yield return new WaitForSeconds(0.5f);
			//List<BattleChar> enemies = BattleSystem.instance.EnemyTeam.AliveChars;
			Skill skill2 = Skill.TempSkill("S_Spectrum_0", this.BChar, this.BChar.MyTeam);
			Skill_Extended s = new Skill_Extended();
			s.PlusSkillStat.Penetration = 100f;
			skill2.ExtendedAdd(s);
			skill2.FreeUse = true;
			skill2.PlusHit = true;
			for (int i = 0; i < target.Count; i++)
            {
				if (!targetChar[i].IsDead)
                {
					this.BChar.ParticleOut(this.MySkill, skill2, targetChar[i]);
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}
	}

	public class SkillEn_Elementalist : Skill_Extended
	{
		public override bool CanSkillEnforce(Skill MainSkill)
		{
			return MainSkill.AP >= 2;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.MySkill.ExtendedFind_DataName("Ex_Fire") != null || this.MySkill.ExtendedFind_DataName("Ex_Elec") != null || this.MySkill.ExtendedFind_DataName("Ex_Dark") != null)
			{
				this.APChange = -2;
			}
			else
            {
				this.APChange = 0;
            }
		}
	}

}
