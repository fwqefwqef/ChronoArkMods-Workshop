using System;
using GameDataEditor;

namespace Gojo
{
	public class P_Gojo : Passive_Char, IP_PlayerTurn
	{
		public override void Init()
		{
			base.Init();
			this.OnePassive = true;
		}
		public void Turn()
		{
			this.BChar.BuffAdd("B_Limitless", this.BChar, false, 0, false, -1, false);
		}
	}
}
