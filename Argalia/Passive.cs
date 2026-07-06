using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.Plugin;
using ChronoArkMod.Template;
using Debug = UnityEngine.Debug;
using ChronoArkMod.ModData;

namespace Argalia
{
    public class P_Argalia : Passive_Char, IP_TurnEndButtonEnemy
    {
        public override void Init()
        {
            base.Init();
            this.OnePassive = true;
        }

        public void TurnEndButtonEnemy()
        {
            this.BChar.BuffAdd("B_Argalia_Immune", this.BChar);
        }

        public static bool IsResonance(Skill skill, BattleChar target)
        {
            int vibration = -1;
            Buff b = target.BuffReturn("B_Argalia_P");
            if (b != null)
            {
                vibration = b.StackNum;
            }

            // Check for controlled resonance condition first
            if (vibration >= 2)
            {
                foreach (BattleChar bc in BattleSystem.instance.AllyTeam.AliveChars)
                {
                    Buff controlledResonance = bc.BuffReturn("B_Argalia_4");
                    if (controlledResonance != null)
                    {
                        MasterAudio.PlaySound("Resonance", 1f, null, 0f, null, null, false, false);
                        Debug.Log("Controlled Resonance");
                        return true;
                    }
                }
            }

            // Now check vibration == actioncount condition
            int actioncount = -99;
            if (target is BattleEnemy)
            {
                if ((target as BattleEnemy).SkillQueue.Count != 0)
                {
                    actioncount = (target as BattleEnemy).SkillQueue[0].CastSpeed;
                }
            }

            // Offset non swift skill pulling action by 1
            if (skill.NotCount == false)
            {
                actioncount += 1;
            }

            Debug.Log("Resonance Check: Vibration = " + vibration + ", ActionCount = " + actioncount);
            if (vibration == actioncount)
            {
                MasterAudio.PlaySound("Resonance", 1f, null, 0f, null, null, false, false);
                return true;
            }
            else if (vibration == 6 && actioncount >= 6)
            {
                MasterAudio.PlaySound("Resonance", 1f, null, 0f, null, null, false, false);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
