using I2.Loc;
using UnityEngine;
using GameDataEditor;

namespace NightmareEve
{
    public class Mistletoe : EquipBase, IP_BattleEnd
	{
		public override void Init()
		{
			this.PlusStat.atk = 1f;
			this.PlusStat.reg = 1f;
			base.Init();
		}

		public void BattleEnd()
		{
			Debug.Log("Battle End");
			Debug.Log(BattleSystem.instance.MainQueueData.Key);
			if (BattleSystem.instance.MainQueueData.Key == "Abbadon_Queue")
			{
				Debug.Log("Abbadon Queue");
				int i = 0;
				while (i < this.MyChar.Equip.Count)
				{
					if (this.MyChar.Equip[i] == this.MyItem)
					{
						this.MyChar.Equip[i] = ItemBase.GetItem("Mistletoe_1");
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}
	}

	public class Mistletoe_1 : EquipBase, IP_BattleEnd, IP_DamageTake, IP_CampFire
	{
		public override void Init()
		{
			this.PlusStat.atk = 1f;
			this.PlusStat.reg = 1f;
			this.PlusStat.def = 10f;
			base.Init();
		}

			public void BattleEnd()
			{
				Debug.Log("Battle End");
				if (BattleSystem.instance.MainQueueData.Key == "Minotaur_Queue")
				{
					int i = 0;
					while (i < this.MyChar.Equip.Count)
					{
						if (this.MyChar.Equip[i] == this.MyItem)
						{
							this.MyChar.Equip[i] = ItemBase.GetItem("Mistletoe_2");
							break;
						}
						else
						{
							i++;
						}
					}
				}
			}

		public override string DescExtended(string desc)
		{
			if (this.Effect)
			{
				return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Disable);
			}
			return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Enable);
		}
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (!this.Effect && this.BChar.Info.Hp <= 0)
			{
				resist = true;
				this.BChar.Heal(this.BChar, 0, 1 - this.BChar.Info.Hp , 0);
			}
		}
		public void Camp()
		{
			this.Effect = false;
		}

		public bool Effect = false;
	}
	public class Mistletoe_22 : EquipBase, IP_BattleEnd, IP_DamageTake, IP_CampFire
	{
		public override void Init()
		{
			this.PlusStat.atk = 1f;
			this.PlusStat.reg = 1f;
			this.PlusStat.def = 10f;
			this.PlusStat.hit = 20f;
			this.PlusStat.HIT_CC = 20f;
			this.PlusStat.HIT_DEBUFF = 20f;
			this.PlusStat.HIT_DOT = 20f;
			base.Init();
		}

		public void BattleEnd()
		{
			Debug.Log("Battle End");
			if (BattleSystem.instance.MainQueueData.Key == "Matador_Queue")
			{
				int i = 0;
				while (i < this.MyChar.Equip.Count)
				{
					if (this.MyChar.Equip[i] == this.MyItem)
					{
						this.MyChar.Equip[i] = ItemBase.GetItem("Mistletoe_3");
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		public override string DescExtended(string desc)
		{
			if (this.Effect)
			{
				return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Disable);
			}
			return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Enable);
		}
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (!this.Effect && this.BChar.Info.Hp <= 0)
			{
				resist = true;
				this.BChar.Heal(this.BChar, 0, 1 - this.BChar.Info.Hp, 0);
			}
		}
		public void Camp()
		{
			this.Effect = false;
		}

		public bool Effect = false;
	}

	public class Mistletoe_3 : EquipBase, IP_BattleEnd, IP_DamageTake, IP_CampFire
	{
		public override void Init()
		{
			this.PlusStat.atk = 3f;
			this.PlusStat.reg = 3f;
			this.PlusStat.def = 10f;
			this.PlusStat.hit = 20f;
			this.PlusStat.HIT_CC = 20f;
			this.PlusStat.HIT_DEBUFF = 20f;
			this.PlusStat.HIT_DOT = 20f;
			base.Init();
		}

		public void BattleEnd()
		{
			Debug.Log("Battle End");
			if (BattleSystem.instance.MainQueueData.Key == "WhiteRider_Queue")
			{
				int i = 0;
				while (i < this.MyChar.Equip.Count)
				{
					if (this.MyChar.Equip[i] == this.MyItem)
					{
						this.MyChar.Equip[i] = ItemBase.GetItem("Mistletoe_4");
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		public override string DescExtended(string desc)
		{
			if (this.Effect)
			{
				return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Disable);
			}
			return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Enable);
		}
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (!this.Effect && this.BChar.Info.Hp <= 0)
			{
				resist = true;
				this.BChar.Heal(this.BChar, 0, 1 - this.BChar.Info.Hp, 0);
			}
		}
		public void Camp()
		{
			this.Effect = false;
		}

		public bool Effect = false;
	}

	public class Mistletoe_4 : EquipBase, IP_BattleEnd, IP_DamageTake, IP_CampFire
	{
		public override void Init()
		{
			this.PlusStat.atk = 3f;
			this.PlusStat.reg = 3f;
			this.PlusStat.def = 10f;
			this.PlusStat.hit = 20f;
			this.PlusStat.HIT_CC = 20f;
			this.PlusStat.HIT_DEBUFF = 20f;
			this.PlusStat.HIT_DOT = 20f;
			this.PlusStat.RES_DOT = 80f;
			this.PlusStat.RES_CC = 80f;
			this.PlusStat.RES_DEBUFF = 80f;
			base.Init();
		}

		public void BattleEnd()
		{
			Debug.Log("Battle End");
			if (BattleSystem.instance.MainQueueData.Key == "Belial_Queue")
			{
				int i = 0;
				while (i < this.MyChar.Equip.Count)
				{
					if (this.MyChar.Equip[i] == this.MyItem)
					{
						this.MyChar.Equip[i] = ItemBase.GetItem("Mistletoe_5");
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		public override string DescExtended(string desc)
		{
			if (this.Effect)
			{
				return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Disable);
			}
			return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Enable);
		}
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (!this.Effect && this.BChar.Info.Hp <= 0)
			{
				resist = true;
				this.BChar.Heal(this.BChar, 0, 1 - this.BChar.Info.Hp, 0);
			}
		}
		public void Camp()
		{
			this.Effect = false;
		}

		public bool Effect = false;
	}

	public class Mistletoe_5 : EquipBase, IP_BattleEnd, IP_DamageTake, IP_CampFire, IP_BuffAddAfter
	{
		public override void Init()
		{
			this.PlusStat.atk = 3f;
			this.PlusStat.reg = 3f;
			this.PlusStat.def = 20f;
			this.PlusStat.hit = 20f;
			this.PlusStat.HIT_CC = 20f;
			this.PlusStat.HIT_DEBUFF = 20f;
			this.PlusStat.HIT_DOT = 20f;
			this.PlusStat.RES_DOT = 80f;
			this.PlusStat.RES_CC = 80f;
			this.PlusStat.RES_DEBUFF = 80f;
			base.Init();
		}

		public void BattleEnd()
		{
			Debug.Log("Battle End");
			if (BattleSystem.instance.MainQueueData.Key == "LBossFirst_Queue" || BattleSystem.instance.MainQueueData.Key == "Beelzebub_Queue")
			{
				Debug.Log("Battle End2");
				int i = 0;
				while (i < this.MyChar.Equip.Count)
				{
					Debug.Log("Battle End3");
					if (this.MyChar.Equip[i] == this.MyItem)
					{
						Debug.Log("Battle End4");
						this.MyChar.Equip[i] = ItemBase.GetItem("Mistletoe_6");
						break;
					}
					else
					{
						i++;
					}
				} // 
			}
		}

		public override string DescExtended(string desc)
		{
			if (this.Effect)
			{
				return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Disable);
			}
			return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Enable);
		}
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (!this.Effect && this.BChar.Info.Hp <= 0)
			{
				resist = true;
				this.BChar.Heal(this.BChar, 0, 1 - this.BChar.Info.Hp, 0);
			}
		}
		public void Camp()
		{
			this.Effect = false;
		}

		public bool Effect = false;

		public void BuffaddedAfter(BattleChar BuffUser, BattleChar BuffTaker, Buff addedbuff, StackBuff stackBuff)
		{
			if (BuffUser == this.BChar && !(addedbuff.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_CrowdControl) && stackBuff.RemainTime != 0)
			{
				stackBuff.RemainTime++;
			}
		}
	}

	public class Mistletoe_6 : EquipBase, IP_DamageTake, IP_CampFire, IP_BuffAddAfter, IP_PlayerTurn
	{
		public override void Init()
		{
			this.PlusStat.atk = 10f;
			this.PlusStat.reg = 10f;
			this.PlusStat.def = 20f;
			this.PlusStat.hit = 20f;
			this.PlusStat.HIT_CC = 20f;
			this.PlusStat.HIT_DEBUFF = 20f;
			this.PlusStat.HIT_DOT = 20f;
			this.PlusStat.RES_DOT = 80f;
			this.PlusStat.RES_CC = 80f;
			this.PlusStat.RES_DEBUFF = 80f;
			base.Init();
		}

		public override string DescExtended(string desc)
		{
			if (this.Effect)
			{
				return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Disable);
			}
			return base.DescExtended(desc).Replace("&a", ScriptLocalization.UI_Battle_Item_ForestSword.Enable);
		}
		public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
		{
			if (!this.Effect && this.BChar.Info.Hp <= 0)
			{
				resist = true;
				this.BChar.Heal(this.BChar, 0, 1 - this.BChar.Info.Hp, 0);
			}
		}
		public void Camp()
		{
			this.Effect = false;
		}

		public bool Effect = false;

		public void BuffaddedAfter(BattleChar BuffUser, BattleChar BuffTaker, Buff addedbuff, StackBuff stackBuff)
		{
			if (BuffUser == this.BChar && !(addedbuff.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_CrowdControl) && stackBuff.RemainTime != 0)
			{
				stackBuff.RemainTime++;
			}
		}

		public void Turn()
        {
			BattleSystem.instance.AllyTeam.Draw(2);
			BattleSystem.instance.AllyTeam.AP += 2;
		}
	}
}
