using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace ExpertPlusMod
{
	public class B_Mist4Buff : Buff
	{
		// Token: 0x06000C32 RID: 3122 RVA: 0x000805A4 File Offset: 0x0007E7A4
		public override void Init()
		{
			if ((this.BChar as BattleEnemy).Boss)
			{
				this.PlusStat.spd = 1;
            }
			else
			{
				this.PlusPerStat.Damage = 15;
			}

			this.IsHide = true;
            base.Init();
		}
	}
}
