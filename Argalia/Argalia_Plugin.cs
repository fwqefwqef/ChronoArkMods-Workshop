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
using HarmonyLib;
namespace Argalia
{
    public class Argalia_Plugin: ChronoArkPlugin
    {
        private Harmony harmony;

        public override void Dispose()
        {
            this.harmony?.UnpatchSelf();
        }

        public override void Initialize()
        {
            this.harmony = new Harmony(base.GetGuid());
            this.harmony.PatchAll();
        }

        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        private static class ArgaliaGDataPatch
        {
            private static readonly string[] SkillListKeys =
            {
                "SkillExtended",
                "SKillExtendedItem",
                "PlusViewBuffList",
                "PlusKeyWords",
                "SubParticle_Path",
            };

            private static void Prefix(ref string dataString)
            {
                Dictionary<string, object> masterJson = Json.Deserialize(dataString) as Dictionary<string, object>;
                if (masterJson == null)
                {
                    return;
                }

                FixArgaliaCharacter(masterJson);
                FixArgaliaSkills(masterJson);

                dataString = Json.Serialize(masterJson);
            }

            private static void FixArgaliaCharacter(Dictionary<string, object> masterJson)
            {
                if (!TryGetEntry(masterJson, "Argalia", out Dictionary<string, object> characterData))
                {
                    return;
                }

                if (!masterJson.ContainsKey("S_Argalia_0"))
                {
                    characterData["FirstSkill"] = "S_Argalia_1";
                }
            }

            private static void FixArgaliaSkills(Dictionary<string, object> masterJson)
            {
                foreach (KeyValuePair<string, object> entry in masterJson)
                {
                    if (!entry.Key.StartsWith("S_Argalia_", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (!(entry.Value is Dictionary<string, object> skillData))
                    {
                        continue;
                    }

                    if (!skillData.TryGetValue("_gdeSchema", out object schema) || !Equals(schema, "Skill"))
                    {
                        continue;
                    }

                    EnsureList(skillData, SkillListKeys);
                }
            }

            private static void EnsureList(Dictionary<string, object> data, IEnumerable<string> keys)
            {
                foreach (string key in keys)
                {
                    if (!data.ContainsKey(key) || data[key] == null)
                    {
                        data[key] = new List<string>();
                    }
                }
            }

            private static bool TryGetEntry(Dictionary<string, object> masterJson, string key, out Dictionary<string, object> entry)
            {
                entry = null;
                if (!masterJson.TryGetValue(key, out object rawEntry))
                {
                    return false;
                }

                entry = rawEntry as Dictionary<string, object>;
                return entry != null;
            }
        }
    }
}
