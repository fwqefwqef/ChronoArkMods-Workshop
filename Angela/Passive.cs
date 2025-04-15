using GameDataEditor;
using I2.Loc;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Angela
{
    public class P_Angela : Passive_Char, IP_PlayerTurn
    {
        public List<string> AbnoLv1 = new List<string> { "S_Angela_Red1", "S_Angela_Red2", "S_Angela_Red3", "S_Angela_Green1", "S_Angela_Green2", "S_Angela_Green3" };
        public List<string> AbnoLv2 = new List<string> { "S_Angela_Red4", "S_Angela_Red5", "S_Angela_Red6", "S_Angela_Green4", "S_Angela_Green5", "S_Angela_Green6" };
        public List<string> AbnoLv3 = new List<string> { "S_Angela_Red7", "S_Angela_Red8", "S_Angela_Red9", "S_Angela_Green7", "S_Angela_Green8", "S_Angela_Green9" };
        
        public List<string> AbnoLv1Red = new List<string> { "S_Angela_Red1", "S_Angela_Red2", "S_Angela_Red3" };
        public List<string> AbnoLv2Red = new List<string> { "S_Angela_Red4", "S_Angela_Red5", "S_Angela_Red6" };
        public List<string> AbnoLv3Red = new List<string> { "S_Angela_Red7", "S_Angela_Red8", "S_Angela_Red9" };
       
        public List<string> AbnoLv1Green = new List<string> { "S_Angela_Green1", "S_Angela_Green2", "S_Angela_Green3" };
        public List<string> AbnoLv2Green = new List<string> { "S_Angela_Green4", "S_Angela_Green5", "S_Angela_Green6" };
        public List<string> AbnoLv3Green = new List<string> { "S_Angela_Green7", "S_Angela_Green8", "S_Angela_Green9" };
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public void Turn()
        {
            List<Skill> list = new List<Skill>();
            List<string> alreadyUsed = new List<string>();

            if (BattleSystem.instance.TurnNum == 2) // Abno 1 Selection
            {
                Debug.Log("Angela's Abno 1 Selection");

                // Pick Green
                string s = AbnoLv1Green.Random();
                alreadyUsed.Add(s);
                list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));

                // Pick Red
                s = AbnoLv1Red.Random();
                alreadyUsed.Add(s);
                list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));


                int loop = this.BChar.Info.LV - 2;
                if (loop < 1)
                {
                    loop = 1;
                }

                // Pick Random to fill remaining slots
                for (int i = 0; i < loop; i++)
                {
                    while(alreadyUsed.Contains(s))
                    {
                        s = AbnoLv1.Random();
                    }
                    alreadyUsed.Add(s);
                    list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));
                }
            }

            else if (BattleSystem.instance.TurnNum == 4) // Abno 2 Selection
            {
                Debug.Log("Angela's Abno 2 Selection");

                // Pick Green
                string s = AbnoLv2Green.Random();
                alreadyUsed.Add(s);
                list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));

                // Pick Red
                s = AbnoLv2Red.Random();
                alreadyUsed.Add(s);
                list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));

                // Pick Random to fill remaining slots
                int loop = this.BChar.Info.LV - 2;
                if (loop < 1)
                {
                    loop = 1;
                }

                for (int i = 0; i < loop; i++)
                {
                    while (alreadyUsed.Contains(s))
                    {
                        s = AbnoLv2.Random();
                    }
                    alreadyUsed.Add(s);
                    list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));
                }
            }

            else if (BattleSystem.instance.TurnNum == 6) // Abno 3 Selection
            {
                Debug.Log("Angela's Abno 3 Selection");

                // Pick Green
                string s = AbnoLv3Green.Random();
                alreadyUsed.Add(s);
                list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));

                // Pick Red
                s = AbnoLv3Red.Random();
                alreadyUsed.Add(s);
                list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));

                // Pick Random to fill remaining slots
                int loop = this.BChar.Info.LV - 2;
                if (loop < 1)
                {
                    loop = 1;
                }

                for (int i = 0; i < loop; i++)
                {
                    while (alreadyUsed.Contains(s))
                    {
                        s = AbnoLv3.Random();
                    }
                    alreadyUsed.Add(s);
                    list.Add(Skill.TempSkill(s, this.BChar, this.BChar.MyTeam));
                }
            }

            BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.TargetSelect), "", false, false, true, false, false));
        }

        private void TargetSelect(SkillButton MyButton)
        {

            if (MyButton.Myskill.TargetTypeKey == "ally")
            {
                List<Skill> list = new List<Skill>();
                foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
                {
                    list.Add(Skill.TempSkill((MyButton.Myskill.MySkill.KeyID), b, BattleSystem.instance.AllyTeam));
                }
                BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Cast), "", false, false, true, false, true));
            }

            else
            {
                // cast the abnormality page: Obsession / Confession / Dark Flame / Magic Trick
                Skill skill = MyButton.Myskill;
                BattleSystem.DelayInput(BattleSystem.instance.ForceAction(skill, null, false, false, true));
            }
        }

       private void Cast(SkillButton MyButton)
       {
            BattleChar b = MyButton.Myskill.Master;
            Skill skill = MyButton.Myskill;
            BattleSystem.DelayInput(BattleSystem.instance.ForceAction(skill, b, false, false, true));
       }
    }
}
