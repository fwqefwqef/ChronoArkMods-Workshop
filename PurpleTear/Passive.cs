using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using GameDataEditor;
using UnityEngine;

namespace PurpleTear
{
    public class P_PurpleTear : Passive_Char, IP_PlayerTurn, IP_SkillUse_User_After
    {
        private const int StanceSkillsRequired = 4;
        public int skillsPlayedInStance;
        private bool stanceSelectionPending;

        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
            this.skillsPlayedInStance = 0;
            this.stanceSelectionPending = false;
        }

        public void Turn()
        {
            if (this.CurrentStanceBuffKey() == null)
            {
                this.StanceSelect();
            }
        }

        public void SkillUseAfter(Skill SkillD)
        {
            if (SkillD == null || SkillD.Master != this.BChar || this.stanceSelectionPending)
            {
                return;
            }

            string keyID = SkillD.MySkill.KeyID;
            if (this.BuffKeyForStanceSkill(keyID) != null || this.CurrentStanceBuffKey() == null)
            {
                return;
            }
            if (!SkillD.PlusHit)
            {
                this.skillsPlayedInStance++;
            }
            if (this.skillsPlayedInStance >= StanceSkillsRequired)
            {
                this.ForceStanceChange();
            }
        }

        public void ForceStanceChange()
        {
            this.skillsPlayedInStance = 0;
            this.StanceSelect();
        }

        public void StanceSelect()
        {
            if (this.stanceSelectionPending)
            {
                return;
            }

            string currentStance = this.CurrentStanceBuffKey();
            List<Skill> list = new List<Skill>();
            this.AddStanceOption(list, "S_PurpleTear_Slash", "B_PurpleTear_P_Slash", currentStance);
            this.AddStanceOption(list, "S_PurpleTear_Pierce", "B_PurpleTear_P_Pierce", currentStance);
            this.AddStanceOption(list, "S_PurpleTear_Blunt", "B_PurpleTear_P_Blunt", currentStance);
            this.AddStanceOption(list, "S_PurpleTear_Guard", "B_PurpleTear_P_Guard", currentStance);

            if (list.Count == 0)
            {
                return;
            }

            this.LimitStanceOptions(list, 2);

            this.stanceSelectionPending = true;
            BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Del), "Choose a stance.", false, false, true, false, false));
        }

        public void Del(SkillButton Mybutton)
        {
            string buffKey = this.BuffKeyForStanceSkill(Mybutton.Myskill.MySkill.KeyID);
            if (buffKey == null)
            {
                this.stanceSelectionPending = false;
                return;
            }

            this.RemoveStanceBuffs();
            this.BChar.BuffAdd(buffKey, this.BChar);
            this.skillsPlayedInStance = 0;
            this.stanceSelectionPending = false;
        }

        private void AddStanceOption(List<Skill> list, string skillKey, string buffKey, string currentStance)
        {
            if (buffKey != currentStance)
            {
                list.Add(Skill.TempSkill(skillKey, this.BChar, this.BChar.MyTeam));
            }
        }

        private void LimitStanceOptions(List<Skill> list, int maxOptions)
        {
            while (list.Count > maxOptions)
            {
                int index = this.BChar.GetRandomClass().Main.RandomInt(0, list.Count);
                list.RemoveAt(index);
            }
        }

        private string BuffKeyForStanceSkill(string skillKey)
        {
            if (skillKey == "S_PurpleTear_Slash")
            {
                return "B_PurpleTear_P_Slash";
            }
            if (skillKey == "S_PurpleTear_Pierce")
            {
                return "B_PurpleTear_P_Pierce";
            }
            if (skillKey == "S_PurpleTear_Blunt")
            {
                return "B_PurpleTear_P_Blunt";
            }
            if (skillKey == "S_PurpleTear_Guard")
            {
                return "B_PurpleTear_P_Guard";
            }
            return null;
        }

        private string CurrentStanceBuffKey()
        {
            if (this.BChar.BuffFind("B_PurpleTear_P_Slash"))
            {
                return "B_PurpleTear_P_Slash";
            }
            if (this.BChar.BuffFind("B_PurpleTear_P_Pierce"))
            {
                return "B_PurpleTear_P_Pierce";
            }
            if (this.BChar.BuffFind("B_PurpleTear_P_Blunt"))
            {
                return "B_PurpleTear_P_Blunt";
            }
            if (this.BChar.BuffFind("B_PurpleTear_P_Guard"))
            {
                return "B_PurpleTear_P_Guard";
            }
            return null;
        }

        private void RemoveStanceBuffs()
        {
            this.BChar.BuffRemove("B_PurpleTear_P_Slash", true);
            this.BChar.BuffRemove("B_PurpleTear_P_Pierce", true);
            this.BChar.BuffRemove("B_PurpleTear_P_Blunt", true);
            this.BChar.BuffRemove("B_PurpleTear_P_Guard", true);
        }
    }
}
