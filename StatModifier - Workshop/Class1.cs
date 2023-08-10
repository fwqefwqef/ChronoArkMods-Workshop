using BepInEx;
using BepInEx.Configuration;
using GameDataEditor;
using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TileTypes;
using System.Reflection.Emit;
using System.Reflection;
using I2.Loc;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.ModData;
using ChronoArkMod.ModData.Settings;
using ChronoArkMod.Plugin;

namespace DamageModifier
{
    [PluginConfig("StatModifier", "StatModifier", "1.0.0")]
    public class DamagePlugin : ChronoArkPlugin
    {
        public const string GUID = "windy.statmod";
        public const string version = "1.0.0";
        private Harmony harmony;

        public static double enemyMult;
        public static double playerMult;
        public static double enemyHPMult;
        public static double playerHPMult;

        public override void Initialize()
        {
            ModInfo modInfo = ModManager.getModInfo("StatModifier");
            enemyMult = Double.Parse(modInfo.GetSetting<InputFieldSetting>("enemyMult").Value);
            playerMult = Double.Parse(modInfo.GetSetting<InputFieldSetting>("playerMult").Value);
            enemyHPMult = Double.Parse(modInfo.GetSetting<InputFieldSetting>("enemyHPMult").Value);
            playerHPMult = Double.Parse(modInfo.GetSetting<InputFieldSetting>("playerHPMult").Value);

            this.harmony = new Harmony(base.GetGuid());
            this.harmony.PatchAll();
        }
        public override void Dispose()
        {
            this.harmony.UnpatchSelf();
        }


        // Modify gdata.json
        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class ModifyGData
        {
            static void Prefix(ref string dataString)
            {
                Dictionary<string, object> masterJson = (Json.Deserialize(dataString) as Dictionary<string, object>);
                foreach (var e in masterJson)
                {
                    if (((Dictionary<string, object>)e.Value).ContainsKey("_gdeSchema"))
                    {
                        if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Enemy"))
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["atk"] * enemyMult);
                            (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["maxhp"] * enemyHPMult);

                        }

                        else if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Character"))
                        {
                            ((masterJson[e.Key] as Dictionary<string, object>)["ATK"] as Dictionary<string, object>)["x"] = (long)((long)((masterJson[e.Key] as Dictionary<string, object>)["ATK"] as Dictionary<string, object>)["x"] * playerMult);
                            ((masterJson[e.Key] as Dictionary<string, object>)["ATK"] as Dictionary<string, object>)["y"] = (long)((long)((masterJson[e.Key] as Dictionary<string, object>)["ATK"] as Dictionary<string, object>)["y"] * playerMult);
                            ((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["x"] = (long)((long)((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["x"] * playerHPMult);
                            ((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["y"] = (long)((long)((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["y"] * playerHPMult);
                        }
                    }
                }
                dataString = Json.Serialize(masterJson);
            }
        }

    }
}