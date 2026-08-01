using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using GameDataEditor;
using I2.Loc;
using UnityEngine;

namespace NightmareEve
{
	internal static class LunaticModeHelpers
	{
		public static readonly Vector3 SusanoSummonPosition = new Vector3(6f, 1f, 1f);
		public static readonly Vector3 RangdaSummonPosition = new Vector3(-6f, 0f, 0f);

		public static bool IsHorseman(string keyData)
		{
			return keyData == "BlackRider"
				|| keyData == "RedRider"
				|| keyData == "WhiteRider"
				|| keyData == "PaleRider";
		}

		public static BattleChar FindMinoMeduPartner(BattleChar source)
		{
			if (source?.MyTeam?.Chars == null || source.Info == null)
			{
				return null;
			}

			string partnerKey = source.Info.KeyData == "Minotaur" ? "Medusa" : source.Info.KeyData == "Medusa" ? "Minotaur" : string.Empty;
			if (string.IsNullOrEmpty(partnerKey))
			{
				return null;
			}

			foreach (BattleChar battleChar in source.MyTeam.Chars)
			{
				if (battleChar?.Info == null || battleChar == source)
				{
					continue;
				}

				if (battleChar.Info.KeyData == partnerKey)
				{
					return battleChar;
				}
			}

			return null;
		}

		public static IEnumerable<BattleChar> GetLivingTeamMembers(BattleTeam team)
		{
			if (team?.Chars == null)
			{
				yield break;
			}

			foreach (BattleChar battleChar in team.Chars)
			{
				if (battleChar == null || battleChar.IsDead)
				{
					continue;
				}

				yield return battleChar;
			}
		}

		public static int GetBeelzebubFlySpawnCount(BattleChar source)
		{
			if (source != null && source.BuffFind("B_Beelzebub_LunaticMode", false))
			{
				return 2;
			}

			return 1;
		}

		public static IEnumerator SpawnEnemiesAutoPosSequential(IEnumerable<string> enemyKeys, float delaySeconds = 0.05f)
		{
			if (enemyKeys == null)
			{
				yield break;
			}

			foreach (string enemyKey in enemyKeys)
			{
				if (string.IsNullOrEmpty(enemyKey))
				{
					continue;
				}

				yield return BattleSystem.instance.NewEnemyAutoPos(enemyKey, null);
				if (delaySeconds > 0f)
				{
					yield return new WaitForSeconds(delaySeconds);
				}
				else
				{
					yield return null;
				}
			}
		}

		public static IEnumerator SpawnBeelzebubFlies(int count)
		{
			List<string> flies = new List<string>();
			for (int i = 0; i < count; i++)
			{
				flies.Add("Fly");
			}

			yield return SpawnEnemiesAutoPosSequential(flies);
		}

		public static IEnumerator SummonBelialSupports()
		{
			if (BattleSystem.instance?.EnemyTeam == null)
			{
				yield break;
			}

			List<string> summons = new List<string> { "Susano", "Rangda" };
			foreach (BattleChar ally in BattleSystem.instance.EnemyTeam.AliveChars)
			{
				if (ally?.Info == null)
				{
					continue;
				}

				summons.Remove(ally.Info.KeyData);
			}

			if (summons.Contains("Susano"))
			{
				yield return SummonAtPosition("Susano", SusanoSummonPosition);
			}

			if (summons.Contains("Rangda"))
			{
				yield return SummonAtPosition("Rangda", RangdaSummonPosition);
			}
		}

		private static IEnumerator SummonAtPosition(string enemyKey, Vector3 position)
		{
			yield return BattleSystem.instance.NewEnemy(enemyKey, position, summonedChar =>
			{
				if (summonedChar == null)
				{
					return;
				}

				summonedChar.transform.position = position;
				summonedChar.transform.localPosition = position;
			});
		}

		private static BattleChar FindEnemyByKey(string enemyKey)
		{
			for (int i = BattleSystem.instance.EnemyTeam.AliveChars.Count - 1; i >= 0; i--)
			{
				BattleChar battleChar = BattleSystem.instance.EnemyTeam.AliveChars[i];
				if (battleChar?.Info != null && battleChar.Info.KeyData == enemyKey)
				{
					return battleChar;
				}
			}

			return null;
		}
	}

	public class B_CustomBGM : Buff, IP_Dead
	{
		public override void Init()
		{
			base.Init();
			BattleSystem.instance.StartCoroutine(this.BGMStart());
		}

		public IEnumerator BGMStart()
		{
			if (BattleSystem.instance.TurnNum <= 1)
			{
                MasterAudio.StopBus("BGM");
                MasterAudio.StopBus("BattleBGM");
                MasterAudio.FadeBusToVolume("BGM", 1f, 1f, null, false, false);
                MasterAudio.FadeBusToVolume("BattleBGM", 0f, 0.5f, null, false, false);
            }

			if (this.BChar.Info.KeyData == "Abbadon")
            {
                MasterAudio.PlaySound("Boss0", 0.7f, null, 0f, null, null, false, false);
                //MasterAudio.PlaySound("Boss0", 1f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Minotaur")
			{
				MasterAudio.PlaySound("Boss1", 0.7f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Matador")
			{
				MasterAudio.PlaySound("Boss2", 0.7f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "WhiteRider")
			{
				MasterAudio.PlaySound("Boss3", 0.7f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Belial")
			{
				MasterAudio.PlaySound("Boss4", 0.7f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Beelzebub")
			{
				MasterAudio.PlaySound("Boss5", 0.7f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Michael")
			{
				if (BattleSystem.instance.TurnNum <= 1)
                {
					MasterAudio.PlaySound("Boss6", 0.7f, null, 0f, null, null, false, false);
				}
			}
			//yield return new WaitForSecondsRealtime(22.675f);
			//MasterAudio.PlaySound("RozeP1", 1f, null, 0f, null, null, false, false);

			yield return null;
			yield break;
		}

		public virtual void Dead()
		{
			// Revive and heal
			if (this.BChar.Info.KeyData == "Belial" || this.BChar.Info.KeyData == "Beelzebub")
			{
				foreach (BattleChar b in BattleSystem.instance.AllyTeam.Chars)
				{
					if (b.Info.Incapacitated)
					{
						b.Info.Incapacitated = false;
						b.HP = 1;
						Debug.Log("healed");
					}
					int num = (int)Misc.PerToNum((float)b.GetStat.maxhp, 400f);
					b.Heal(b, (float)num, false);
				}
			}
		}
	}

	public class B_RedCapote : Buff
	{
		public override void Init()
		{
			base.Init();
			this.PlusStat.hit = 80f;
			this.PlusStat.dod = 80f;
		}
	}

	public class B_MatadorTaunt : Buff
	{
		public override void Init()
		{
			base.Init();
			this.PlusPerStat.Damage = 10;
			this.PlusStat.crihit = 50;
		}
	}

	public class B_DarkMight : Buff
	{
		public override void Init()
		{
			base.Init();
			this.PlusPerStat.Damage = 17 * base.StackNum;
		}
	}

	public class B_Inferno : Buff
	{
		public override void Init()
		{
			base.Init();
		}
	}
	
	public class B_Fervor : Buff
    {
		public override void Init()
		{
			base.Init();
			this.PlusPerStat.Damage = 25;
			this.PlusStat.def = 25;
		}
	}

	public class B_FiendSoul : Buff, IP_Dead
	{
		public override void Init()
		{
			base.Init();

			ItemBase fiendSoul = ItemBase.GetItem(ModItemKeys.Item_Consume_FiendSoul);
			if (fiendSoul != null)
			{
				this.Itemviews.Add(fiendSoul);
			}
		}

		public void Dead()
		{
			if (BattleSystem.instance == null)
			{
				return;
			}

			if (this.BChar?.Info != null && this.BChar.Info.KeyData == "Beelzebub")
			{
				BattleSystem.instance.Reward.Add(ItemBase.GetItem("EquipPouch"));
				BattleSystem.instance.Reward.Add(ItemBase.GetItem("EquipPouch"));
				BattleSystem.instance.Reward.Add(ItemBase.GetItem("SkillBookLucy_Rare"));
				BattleSystem.instance.Reward.Add(ItemBase.GetItem("Soul", 40));
			}

			if (this.Itemviews == null || this.Itemviews.Count == 0)
			{
				return;
			}

			BattleSystem.instance.Reward.AddRange(this.Itemviews);
		}
	}
	public class B_Invincible : Buff
	{
		public override void Init()
		{
			base.Init();
			this.PlusStat.invincibility = true;
			this.PlusStat.RES_CC = 999f;
			if (this.BChar != null)
            {
				this.BChar.BuffRemove("B_Common_Rest");
            }
		}
	}

	public class P_Belial : Buff, IP_HPChange, IP_TurnEnd, IP_PlayerTurn
	{
		public bool flag = true;
		public override void Init()
		{
			base.Init();
		}

		public void HPChange(BattleChar Char, bool Healed)
		{
			if (flag && Misc.NumToPer((float)this.BChar.GetStat.maxhp, (float)this.BChar.HP) <= 50f)
			{
				this.BChar.Info.Hp = this.BChar.GetStat.maxhp / 2;
				//this.PlusStat.invincibility = true;
				this.BChar.BuffAdd("B_Invincible", this.BChar);
				flag = false;
			}
		}
		public void TurnEnd()
        {
			//if (this.PlusStat.invincibility == true)
   //         {
			//	this.PlusStat.invincibility = false;
   //         }
        }

		public void Turn()
        {
			if (BattleSystem.instance.TurnNum > 1)
            {
				BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Fervor", BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam), true);
			}
        }
	}

	public class P_Beelzebub : Buff, IP_HPChange, IP_TurnEnd, IP_PlayerTurn
	{
		public bool flag = true;
		public override void Init()
		{
			base.Init();
		}

		public void HPChange(BattleChar Char, bool Healed)
		{
			if (flag && Misc.NumToPer((float)this.BChar.GetStat.maxhp, (float)this.BChar.HP) <= 50f)
			{
				this.BChar.Info.Hp = this.BChar.GetStat.maxhp / 2;
				//this.PlusStat.invincibility = true;
				this.BChar.BuffAdd("B_Invincible",this.BChar);
				flag = false;
			}
		}
		public void TurnEnd()
		{
			//if (this.PlusStat.invincibility == true)
			//{
			//	this.PlusStat.invincibility = false;
			//}
		}

		public void Turn()
		{

		}
	}

	public class B_Egg_0 : Buff
	{
		public override void Init()
		{
			base.Init();

			if (BattleSystem.instance != null && this.BChar != null && !(this.BChar is BattleEnemy))
			{
				bool flag = true;
				foreach (Skill s in BattleSystem.instance.AllyTeam.Skills)
				{
					// Only add hatch if there is no owner hatch in hand
					if (s.Master == this.BChar && s.MySkill.KeyID == "S_Hatch")
					{
						flag = false;
					}
				}
				if (flag)
				{
					BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Hatch", this.BChar, BattleSystem.instance.AllyTeam), true);
				}
			}
		}
		public override void SelfdestroyPlus()
		{
			base.SelfdestroyPlus();
			int num = (int)(base.Usestate_F.GetStat.atk * 0.85);
			this.BChar.Damage(base.Usestate_F, num, false, true, false, 0, false, false, false);
			BattleSystem.instance.StartCoroutine(LunaticModeHelpers.SpawnBeelzebubFlies(LunaticModeHelpers.GetBeelzebubFlySpawnCount(base.Usestate_F)));
		}
		public override string DescExtended(string desc)
		{
			return base.DescExtended(desc).Replace("&a", (this.BChar.GetStat.atk * 0.85).ToString());
		}
	}
	public class B_Egg_1 : Buff
	{
		public override void Init()
		{
			base.Init();
			this.PlusStat.Stun = true;

			if (BattleSystem.instance != null && this.BChar != null && !(this.BChar is BattleEnemy))
			{
				bool flag = true;
				foreach (Skill s in BattleSystem.instance.AllyTeam.Skills)
				{
					// Only add hatch if there is no owner hatch in hand
					if (s.Master == this.BChar && s.MySkill.KeyID == "S_Hatch")
					{
						flag = false;
					}
				}
				if (flag)
				{
					BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Hatch", this.BChar, BattleSystem.instance.AllyTeam), true);
				}
			}
		}
		public override void SelfdestroyPlus()
		{
			base.SelfdestroyPlus();
			int num = (int)(base.Usestate_F.GetStat.atk * 0.85);
			this.BChar.Damage(base.Usestate_F, num, false, true, false, 0, false, false, false);
			BattleSystem.instance.StartCoroutine(LunaticModeHelpers.SpawnBeelzebubFlies(LunaticModeHelpers.GetBeelzebubFlySpawnCount(base.Usestate_F)));
		}

		public override string DescExtended(string desc)
		{
			return base.DescExtended(desc).Replace("&a", (this.BChar.GetStat.atk * 0.85).ToString());
		}
	}
	public class B_Egg_2 : Buff
	{
		public override void Init()
		{
			base.Init();
			this.PlusPerStat.Damage = -50;
			this.PlusPerStat.Heal = -50;

			if (BattleSystem.instance != null && this.BChar != null && !(this.BChar is BattleEnemy))
            {
				bool flag = true;
				foreach (Skill s in BattleSystem.instance.AllyTeam.Skills)
                {
					// Only add hatch if there is no owner hatch in hand
					if (s.Master == this.BChar && s.MySkill.KeyID == "S_Hatch")
                    {
						flag = false;
                    }
                }
				if (flag)
                {
					BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Hatch", this.BChar, BattleSystem.instance.AllyTeam), true);
				}
            }
		}
		public override void SelfdestroyPlus()
		{
			base.SelfdestroyPlus();
			int num = (int)(base.Usestate_F.GetStat.atk * 0.85);
			this.BChar.Damage(base.Usestate_F, num, false, true, false, 0, false, false, false);
			BattleSystem.instance.StartCoroutine(LunaticModeHelpers.SpawnBeelzebubFlies(LunaticModeHelpers.GetBeelzebubFlySpawnCount(base.Usestate_F)));
		}
		public override string DescExtended(string desc)
		{
			return base.DescExtended(desc).Replace("&a", (this.BChar.GetStat.atk * 0.85).ToString());
		}
	}

	public class B_Gathering : Buff, IP_HPChange
	{
		public bool gaveBlessing = false;
		public override void Init()
		{
			base.Init();
			this.PlusStat.atk = (float)base.StackNum * 2;
		}

		public void HPChange(BattleChar Char, bool Healed)
		{
			//if (!gaveBlessing && this.BChar.HP <= 1500)
			//{
			//	BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Blessing", BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam), true);
			//	gaveBlessing = true;
			//}
		}
	}

	public class B_Blessing : Buff
	{
		public override void Init()
		{
			base.Init();
            this.PlusPerStat.Damage = 100;
            this.PlusPerStat.Heal = 100;
		}
	}

	// Does nothing for now
	public class B_Abbadon_LunaticMode : Buff
    {
        public override void Init()
        {
            base.Init();
        }
    }

	public class B_Tarukaja : Buff, IP_TurnEnd
	{
		private int critBonus = 50;

		public override void Init()
		{
			base.Init();
			this.OnePassive = true;
			this.ApplyBonus();
		}

		public void TurnEnd()
		{
			if (this.BChar == null || this.BChar.IsDead || this.BChar.HP <= 0)
			{
				return;
			}

			critBonus += 50;
			this.ApplyBonus();
			this.BChar.Info?.ForceGetStat();
		}

		private void ApplyBonus()
		{
			float baseCrit = 0f;
			if (this.BChar != null)
			{
				baseCrit = Math.Max(0f, this.BChar.GetStat.cri - this.PlusStat.cri);
			}

			float critToCap = Math.Max(0f, 100f - baseCrit);
			float appliedCrit = Math.Min(critBonus, critToCap);
			this.PlusStat.cri = appliedCrit;
			this.PlusStat.PlusCriDmg = Math.Max(0f, baseCrit + critBonus - 100f);
		}
	}

	public class B_Rakukaja : Buff, IP_HPChange
	{
		private bool endured;

		public override void Init()
		{
			base.Init();
			this.OnePassive = true;
			this.PlusStat.DMGTaken = -10f;
		}

		public void HPChange(BattleChar Char, bool Healed)
		{
			if (endured || Healed || this.BChar == null || this.BChar.HP > 0)
			{
				return;
			}

			this.BChar.HP = 1;
			this.BChar.IsDead = false;
			EffectView.SimpleTextout(this.BChar.GetPos(), ScriptLocalization.UI_Battle.Endure, true, 1f, false, 1f);
			endured = true;
		}
	}

	public class B_Sukukaja : Buff
	{
		public override void Init()
		{
			base.Init();
			this.OnePassive = true;
			this.PlusPerStat.Damage = -30;
			if (this.BChar?.Info != null)
			{
				this.BChar.Info.PlusActCount.Add(1);
				this.BChar.Info.ForceGetStat();
			}
		}
	}

	public class B_MinoMedu_LunaticMode : Buff, IP_Dead, IP_BattleStart_Ones, IP_PlayerTurn
	{
		private bool enraged;
		private BattleChar partner;

		public override void Init()
		{
			base.Init();
		}

		public void BattleStart(BattleSystem Ins)
		{
			this.CachePartner();
		}

		public void Turn()
		{
			if (this.partner == null || this.partner.IsDead)
			{
				this.CachePartner();
			}
		}

		public void Dead()
		{
			if (this.partner == null || this.partner.IsDead)
			{
				this.CachePartner();
			}

			if (this.partner == null || this.partner.IsDead)
			{
				return;
			}

			B_MinoMedu_LunaticMode lunaticBuff = this.partner.BuffReturn("B_MinoMedu_LunaticMode", false) as B_MinoMedu_LunaticMode;
			lunaticBuff?.Enrage();
		}

		private void Enrage()
		{
			if (enraged || this.BChar?.Info == null)
			{
				return;
			}

			int previousMaxHp = this.BChar.GetStat.maxhp;
			this.PlusPerStat.MaxHP += 50;
			this.BChar.Info.PlusActCount.Add(1);
			this.BChar.Info.ForceGetStat();
			int maxHpGain = this.BChar.GetStat.maxhp - previousMaxHp;
			if (maxHpGain > 0)
			{
				this.BChar.Heal(this.BChar, (float)maxHpGain, false, false, null);
			}
			enraged = true;
		}

		private void CachePartner()
		{
			this.partner = LunaticModeHelpers.FindMinoMeduPartner(this.BChar);
		}
	}

	public class B_Matador_LunaticMode : Buff, IP_DamageChange
	{
		public int andaluciaCastCount = 0;

        public int DamageChange(Skill SkillD, BattleChar Target, int Damage, ref bool Cri, bool View)
        {
            if (SkillD.MySkill.KeyID == "S_Matador_4_0")
			{
				return Damage + andaluciaCastCount * 2;
			}
			return Damage;
        }

        public override void Init()
		{
			base.Init();
			this.PlusStat.maxhp = 100;
			this.OnePassive = true;
		}
    }

	public class B_Horsemen_LunaticMode : Buff, IP_Dead
	{
		public override void Init()
		{
			base.Init();
		}

		public void Dead()
		{
			if (this.BChar?.MyTeam == null)
			{
				return;
			}

			foreach (BattleChar ally in LunaticModeHelpers.GetLivingTeamMembers(this.BChar.MyTeam))
			{
				if (ally == null || ally == this.BChar || ally.Info == null || !LunaticModeHelpers.IsHorseman(ally.Info.KeyData))
				{
					continue;
				}

				B_Horsemen_LunaticMode lunaticBuff = ally.BuffReturn("B_Horsemen_LunaticMode", false) as B_Horsemen_LunaticMode;
				lunaticBuff?.Empower();
			}
		}

		private void Empower()
		{
			if (this.BChar?.Info == null)
			{
				return;
			}

			int previousMaxHp = this.BChar.GetStat.maxhp;
			this.PlusPerStat.MaxHP += 15;
			this.PlusPerStat.Damage += 25;
			this.BChar.Info.ForceGetStat();
			int maxHpGain = this.BChar.GetStat.maxhp - previousMaxHp;
			if (maxHpGain > 0)
			{
				this.BChar.Heal(this.BChar, (float)maxHpGain, false, false, null);
			}
		}
	}

	public class B_Belial_LunaticMode : Buff, IP_BattleStart_Ones
	{
		public override void Init()
		{
			base.Init();
		}

		public void BattleStart(BattleSystem Ins)
		{
			BattleSystem.DelayInput(LunaticModeHelpers.SummonBelialSupports());
		}
	}

	public class B_Rangda : Buff, IP_DamageTake
	{
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (User == null || User == this.BChar || Dmg < 20)
			{
				return;
			}

			resist = true;
			User.Damage(this.BChar, Dmg, false, true, false, 0, false, false, false);
		}
	}

	public class B_Susano : Buff, IP_DamageTake
	{
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (Dmg >= 20)
			{
				return;
			}

			resist = true;
			this.BChar.Heal(this.BChar, (float)Dmg, false, false, null);
		}
	}

	public class B_Beelzebub_LunaticMode : Buff
	{
		public override void Init()
		{
			base.Init();
		}
	}

	public class B_Metatron_LunaticMode : Buff
	{
		public override void Init()
		{
			base.Init();
		}
	}
}
