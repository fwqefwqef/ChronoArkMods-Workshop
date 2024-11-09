using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpertPlusMod
{
	public class B_Mist4Buff : Buff
	{
		// Token: 0x06000C32 RID: 3122 RVA: 0x000805A4 File Offset: 0x0007E7A4
		public override void Init()
		{
			this.PlusPerStat.Damage = 10;
			//this.PlusPerStat.Heal = 100;
			this.PlusPerStat.MaxHP = 10;
			this.IsHide = true;
			base.Init();
		}
	}
}
