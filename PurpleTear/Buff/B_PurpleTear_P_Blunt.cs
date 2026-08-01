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
namespace PurpleTear
{
	/// <summary>
	/// Blunt Stance
	/// Attacks apply Staggered. Critical hits apply 2 stacks.
	/// Staggered: CC Debuff (1/4)
	/// CC Resist -8%
	/// At 4 stacks, apply Stun and remove all Staggered.
	/// Current Skills Used: &a
	/// </summary>
    public class B_PurpleTear_P_Blunt:Buff
    {

    }
}