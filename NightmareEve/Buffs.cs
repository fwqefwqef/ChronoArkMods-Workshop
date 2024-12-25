using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using GameDataEditor;
using I2.Loc;
using UnityEngine;

namespace NightmareEve
{
	public class B_CustomBGM : Buff, IP_Dead
	{
		public override void Init()
		{
			base.Init();
			BattleSystem.instance.StartCoroutine(this.BGMStart());
		}

		public IEnumerator BGMStart()
		{
			if (this.BChar.Info.KeyData == "Abbadon")
            {
				MasterAudio.PlaySound("Boss0", 1f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Minotaur")
			{
				MasterAudio.PlaySound("Boss1", 1f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Matador")
			{
				MasterAudio.PlaySound("Boss2", 1f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "WhiteRider")
			{
				MasterAudio.PlaySound("Boss3", 1f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Belial")
			{
				MasterAudio.PlaySound("Boss4", 1f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Beelzebub")
			{
				MasterAudio.PlaySound("Boss5", 1f, null, 0f, null, null, false, false);
			}
			else if (this.BChar.Info.KeyData == "Michael")
			{
				if (BattleSystem.instance.TurnNum <= 1)
                {
					MasterAudio.PlaySound("Boss6", 1f, null, 0f, null, null, false, false);
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
			if (this.BChar.Info.KeyData == "Belial" && this.BChar.Info.KeyData == "Beelzebub")
			{
				foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
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
			this.PlusPerStat.Damage = 20;
			this.PlusStat.crihit = 50;
		}
	}

	public class B_DarkMight : Buff
	{
		public override void Init()
		{
			base.Init();
			this.PlusPerStat.Damage = 10 * base.StackNum;
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
			BattleSystem.instance.StartCoroutine(BattleSystem.instance.NewEnemyAutoPos("Fly", null));
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
			BattleSystem.instance.StartCoroutine(BattleSystem.instance.NewEnemyAutoPos("Fly", null));
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
			BattleSystem.instance.StartCoroutine(BattleSystem.instance.NewEnemyAutoPos("Fly", null));
		}
		public override string DescExtended(string desc)
		{
			return base.DescExtended(desc).Replace("&a", (this.BChar.GetStat.atk * 0.85).ToString());
		}
	}

	public class B_Recarm : Buff
	{
		public override void Init()
		{
			base.Init();
			foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
            {
				Debug.Log(b.Info.KeyData);
				if (b.Info.KeyData == "Metatron")
                {
					this.PlusPerStat.MaxHP = -50;
				}
            }
		}
	}

	public class B_Gathering : Buff, IP_HPChange
	{
		public bool gaveBlessing = false;
		public override void Init()
		{
			base.Init();
			this.PlusStat.atk = (float)base.StackNum;
		}

		public void HPChange(BattleChar Char, bool Healed)
		{
			if (!gaveBlessing && this.BChar.HP <= 1000)
			{
				BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Blessing", BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam), true);
				gaveBlessing = true;
			}
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
}
