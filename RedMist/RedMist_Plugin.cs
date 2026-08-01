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
using HarmonyLib;
using Debug = UnityEngine.Debug;
using ChronoArkMod.ModData;
namespace RedMist
{
    public class RedMist_Plugin: ChronoArkPlugin
    {
        private Harmony harmony;

        public override void Dispose()
        {
            if (this.harmony != null)
            {
                this.harmony.UnpatchSelf();
                this.harmony = null;
            }
        }

        public override void Initialize()
        {
            this.harmony = new Harmony("redmist.particletint.patch");
            this.harmony.PatchAll();
        }
    }
}
