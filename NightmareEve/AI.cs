using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NightmareEve
{
	using System;

	public class AI_Abbadon : AI
	{

		public override int SpeedChange(Skill skill, int ActionCount, int OriginSpeed)
		{
			if (skill.MySkill.KeyID == "S_Abbadon_1")
			{
				return 1;
			}
			else
            {
				return base.SpeedChange(skill, ActionCount, OriginSpeed);
            }
		}

		public override Skill SkillSelect(int ActionCount)
		{
			if (ActionCount == 0)
			{
				return this.BChar.Skills[0];
			}
			return this.BChar.Skills[1];
		}
	}

	public class AI_Matador : AI
	{
		public override Skill SkillSelect(int ActionCount)
		{
			// Odd
			if (BattleSystem.instance.TurnNum % 2 == 1)
			{
				if (ActionCount == 0)
				{
					return this.BChar.Skills[0];
				}
				if (ActionCount == 1)
				{
					return this.BChar.Skills[1];
				}
				if (ActionCount == 2)
				{
					return this.BChar.Skills[2];
				}
			}
			// Even
			if (BattleSystem.instance.TurnNum % 2 == 0)
			{
				if (ActionCount == 0)
				{
					return this.BChar.Skills[3];
				}
				if (ActionCount == 1)
				{
					return this.BChar.Skills[4];
				}
				if (ActionCount == 2)
				{
					return this.BChar.Skills[4];
				}
			}
			return this.BChar.Skills[0];
		}
	}

	public class AI_WhiteRider : AI
	{
		public override int SpeedChange(Skill skill, int ActionCount, int OriginSpeed)
		{
			if (skill.MySkill.KeyID == "S_WhiteRider_1")
			{
				return 0;
			}
			else
			{
				return base.SpeedChange(skill, ActionCount, OriginSpeed);
			}
		}

		public override Skill SkillSelect(int ActionCount)
		{
			// Odd
			if (BattleSystem.instance.TurnNum % 4 == 1)
			{
				return this.BChar.Skills[1];
			}
			return this.BChar.Skills[0];
		}
	}
	public class AI_RedRider : AI
	{
		public override int SpeedChange(Skill skill, int ActionCount, int OriginSpeed)
		{
			if (skill.MySkill.KeyID == "S_RedRider_1")
			{
				return 0;
			}
			else
			{
				return base.SpeedChange(skill, ActionCount, OriginSpeed);
			}
		}
		public override Skill SkillSelect(int ActionCount)
		{
			// Odd
			if (BattleSystem.instance.TurnNum % 4 == 2)
			{
				return this.BChar.Skills[1];
			}
			return this.BChar.Skills[0];
		}
	}
	public class AI_BlackRider : AI
	{
		public override Skill SkillSelect(int ActionCount)
		{
			// Odd
			if (BattleSystem.instance.TurnNum % 4 == 3)
			{
				return this.BChar.Skills[1];
			}
			return this.BChar.Skills[0];
		}
	}
	public class AI_PaleRider : AI
	{
		public override Skill SkillSelect(int ActionCount)
		{
			if (ActionCount == 0)
            {
				if (BattleSystem.instance.TurnNum % 4 == 0)
				{
					return this.BChar.Skills[2];
				}
				else
                {
					return this.BChar.Skills[1]; // Mamudoon
				}
			}

			else
            {
				return this.BChar.Skills[0]; // Dark Might
			}
		}
	}
	public class AI_Belial : AI
    {
		public bool usedFlame = false;
        public override Skill SkillSelect(int ActionCount)
		{
			if (BattleSystem.instance.TurnNum == 1 && ActionCount == 0)
			{
				return this.BChar.Skills[0];
			}
			else if (Misc.NumToPer((float)this.BChar.GetStat.maxhp, (float)this.BChar.HP) <= 50f && !usedFlame)
            {
				if (ActionCount == 0)
                {
					BattleSystem.instance.AllyTeam.Add(Skill.TempSkill("S_Sacrifice", BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam), true);
					return this.BChar.Skills[5];
				}
				else
				{
					if (ActionCount == 2)
					{
						usedFlame = true;
					}
					return null;
				}
            }
			return base.SkillRandomSelect(new List<Skill>{this.BChar.Skills[1], this.BChar.Skills[2], this.BChar.Skills[3], this.BChar.Skills[4]});
		}
	}

	public class AI_Beelzebub : AI
	{
		public override int SpeedChange(Skill skill, int ActionCount, int OriginSpeed)
		{
			if (ActionCount == 0)
			{
				return 1;
			}
			else
			{
				return base.SpeedChange(skill, ActionCount, OriginSpeed);
			}
		}

		public bool usedDeath = false;
		public override Skill SkillSelect(int ActionCount)
		{
			if (Misc.NumToPer((float)this.BChar.GetStat.maxhp, (float)this.BChar.HP) <= 50f && !usedDeath)
			{
				if (ActionCount == 0)
				{
					return this.BChar.Skills[7];
				}
				else
				{
					if (ActionCount == 2)
					{
						usedDeath = true;
					}
					return null;
				}
			}
			else if (ActionCount == 0)
            {
				return base.SkillRandomSelect(new List<Skill> { this.BChar.Skills[0], this.BChar.Skills[1], this.BChar.Skills[2]});
			}
			else if (ActionCount == 2 && BattleSystem.instance.TurnNum%2==0)
            {
				return this.BChar.Skills[3];
			}
			return base.SkillRandomSelect(new List<Skill> { this.BChar.Skills[4], this.BChar.Skills[5], this.BChar.Skills[6]});
		}
	}

	public class AI_Angel : AI
	{
		public override int SpeedChange(Skill skill, int ActionCount, int OriginSpeed)
		{
			if (skill.MySkill.KeyID == "S_RedRider_1" || skill.MySkill.KeyID == "S_WhiteRider_1")
			{
				return 0;
			}
			else
			{
				return base.SpeedChange(skill, ActionCount, OriginSpeed);
			}
		}

		public override Skill SkillSelect(int ActionCount)
		{
			if (BattleSystem.instance.TurnNum%2==1)
            {
				return this.BChar.Skills[0];
			}
			return this.BChar.Skills[1];
		}
	}

	public class AI_Metatron : AI
	{

		public override Skill SkillSelect(int ActionCount)
		{
			// Phase change
			if (this.BChar.HP <= 1000)
            {
                //if (ActionCount == 2)
                //{
                //    return null;
                //}
                return this.BChar.Skills[0];
			}

			if (ActionCount == 0)
            {
				if (BattleSystem.instance.TurnNum % 2 == 1)
				{
					return this.BChar.Skills[0]; // Meg
				}
				if (BattleSystem.instance.TurnNum % 2 == 0)
				{
					if (BattleSystem.instance.EnemyTeam.AliveChars.Count == 5)
                    {
						return this.BChar.Skills[0]; // Meg
					}
					return this.BChar.Skills[6]; // Recarm
				}
			}

			return base.SkillRandomSelect(new List<Skill> { this.BChar.Skills[1], this.BChar.Skills[2], this.BChar.Skills[3], this.BChar.Skills[4], this.BChar.Skills[5] });
		}
	}

}
