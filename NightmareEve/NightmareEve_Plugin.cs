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
using TileTypes;
using Random = System.Random;
using ChronoArkMod.ModData.Settings;

namespace NightmareEve
{
    public class NightmareEve_Plugin : ChronoArkPlugin
    {
        private Harmony harmony;
        public static bool LunaticModeEnabled { get; private set; }
        private static readonly string[] NonBossEncounterBuffKeys =
        {
            ModItemKeys.Buff_B_Tarukaja,
            ModItemKeys.Buff_B_Rakukaja,
            ModItemKeys.Buff_B_Sukukaja,
        };
        private static readonly string[] KajaBlacklistedQueueKeys =
        {
            "LastBoss_MasterBattle_1",
        };
        private static readonly Random EncounterRandom = new Random();
        private static bool encounterBuffInitialized;
        private static bool encounterIsBossBattle;
        private static string encounterBuffKey;

        private static readonly string[] ConditionalBossBuffKeys =
        {
            ModItemKeys.Buff_B_Abbadon_LunaticMode,
            ModItemKeys.Buff_B_MinoMedu_LunaticMode,
            ModItemKeys.Buff_B_Matador_LunaticMode,
            ModItemKeys.Buff_B_Horsemen_LunaticMode,
            ModItemKeys.Buff_B_Belial_LunaticMode,
            ModItemKeys.Buff_B_Beelzebub_LunaticMode,
            ModItemKeys.Buff_B_Metatron_LunaticMode,
        };

        public override void Dispose()
        {
            this.harmony.UnpatchSelf();
        }

        public override void Initialize()
        {
            this.harmony = new Harmony(base.GetGuid());
            this.harmony.PatchAll();

            ModInfo modInfo = ModManager.getModInfo("NightmareEve");
            LunaticModeEnabled = modInfo.GetSetting<ToggleSetting>("LunaticMode").Value;
        }

        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class LunaticModeBossBuffGData_Patch
        {
            static void Prefix(ref string dataString)
            {
                Dictionary<string, object> masterJson = Json.Deserialize(dataString) as Dictionary<string, object>;
                if (masterJson == null)
                {
                    return;
                }

                foreach (string buffKey in ConditionalBossBuffKeys)
                {
                    if (!masterJson.TryGetValue(buffKey, out object buffObject))
                    {
                        continue;
                    }

                    Dictionary<string, object> buffEntry = buffObject as Dictionary<string, object>;
                    if (buffEntry == null)
                    {
                        continue;
                    }

                    buffEntry["ClassName"] = GetLunaticModeBossBuffClassName(buffKey);
                }

                foreach (KeyValuePair<string, object> entry in masterJson)
                {
                    Dictionary<string, object> enemyEntry = entry.Value as Dictionary<string, object>;
                    if (enemyEntry == null || !enemyEntry.TryGetValue("_gdeSchema", out object schema) || !Equals(schema, "Enemy"))
                    {
                        continue;
                    }

                    if (!enemyEntry.TryGetValue("Passives", out object passivesObject))
                    {
                        continue;
                    }

                    List<object> passives = passivesObject as List<object>;
                    if (passives == null)
                    {
                        continue;
                    }

                    passives.RemoveAll(passive => ConditionalBossBuffKeys.Contains(passive as string));
                }

                dataString = Json.Serialize(masterJson);
            }
        }

        // add starting items: mistletoe
        [HarmonyPatch(typeof(FieldSystem))]
        class FieldSystem_Patch
        {
            [HarmonyPatch(nameof(FieldSystem.StageStart))]
            [HarmonyPostfix]
            static void StageStartPostfix()
            {
                if (PlayData.TSavedata.StageNum == 0)
                {
                    PartyInventory.InvenM.AddNewItem(ItemBase.GetItem("Mistletoe", 1));
                }
            }
        }

        // Add boss buffs in Lunatic Mode
        [HarmonyPatch(typeof(BattleSystem))]
        class EnemySpawnBuffs_Patch
        {
            [HarmonyPatch(nameof(BattleSystem.Start))]
            [HarmonyPrefix]
            static void StartPrefix()
            {
                encounterBuffInitialized = false;
                encounterIsBossBattle = false;
                encounterBuffKey = string.Empty;
            }

            [HarmonyPatch(nameof(BattleSystem.CreatEnemy))]
            [HarmonyPostfix]
            static void Postfix(string EnemyString, ref BattleEnemy __result)
            {
                if (__result == null)
                {
                    return;
                }

                if (!NightmareEve_Plugin.LunaticModeEnabled)
                {
                    return;
                }

                if (!encounterBuffInitialized)
                {
                    encounterIsBossBattle = __result.Boss;
                    encounterBuffInitialized = true;

                    if (!encounterIsBossBattle && !IsKajaBuffBlacklistedForCurrentEncounter())
                    {
                        encounterBuffKey = NonBossEncounterBuffKeys[EncounterRandom.Next(NonBossEncounterBuffKeys.Length)];
                    }
                }

                if (!encounterIsBossBattle && !__result.Boss && !string.IsNullOrEmpty(encounterBuffKey) && !__result.BuffFind(encounterBuffKey, false))
                {
                    __result.BuffAdd(encounterBuffKey, __result);
                }

                if (EnemyString == "Abbadon")
                {
                    __result.BuffAdd("B_Abbadon_LunaticMode", __result);
                }
                else if (EnemyString == "Minotaur" || EnemyString == "Medusa")
                {
                    __result.BuffAdd("B_MinoMedu_LunaticMode", __result);
                }
                else if (EnemyString == "Matador")
                {
                    __result.BuffAdd("B_Matador_LunaticMode", __result);
                }
                else if (EnemyString == "BlackRider" || EnemyString == "RedRider" || EnemyString == "WhiteRider" || EnemyString == "PaleRider")
                {
                    __result.BuffAdd("B_Horsemen_LunaticMode", __result);
                }
                else if (EnemyString == "Belial")
                {
                    __result.BuffAdd("B_Belial_LunaticMode", __result);
                }
                else if (EnemyString == "Beelzebub")
                {
                    __result.BuffAdd("B_Beelzebub_LunaticMode", __result);
                }
                else if (EnemyString == "Metatron")
                {
                    __result.BuffAdd("B_Metatron_LunaticMode", __result);
                }
            }
        }

        private static bool IsKajaBuffBlacklistedForCurrentEncounter()
        {
            string battleQueueKey = PlayData.BattleQueue;
            if (string.IsNullOrEmpty(battleQueueKey))
            {
                return false;
            }

            return KajaBlacklistedQueueKeys.Contains(battleQueueKey);
        }

        private static string GetLunaticModeBossBuffClassName(string buffKey)
        {
            if (buffKey == ModItemKeys.Buff_B_Abbadon_LunaticMode)
            {
                return "NightmareEve.B_Abbadon_LunaticMode";
            }
            else if (buffKey == ModItemKeys.Buff_B_MinoMedu_LunaticMode)
            {
                return "NightmareEve.B_MinoMedu_LunaticMode";
            }
            else if (buffKey == ModItemKeys.Buff_B_Matador_LunaticMode)
            {
                return "NightmareEve.B_Matador_LunaticMode";
            }
            else if (buffKey == ModItemKeys.Buff_B_Horsemen_LunaticMode)
            {
                return "NightmareEve.B_Horsemen_LunaticMode";
            }
            else if (buffKey == ModItemKeys.Buff_B_Belial_LunaticMode)
            {
                return "NightmareEve.B_Belial_LunaticMode";
            }
            else if (buffKey == ModItemKeys.Buff_B_Beelzebub_LunaticMode)
            {
                return "NightmareEve.B_Beelzebub_LunaticMode";
            }
            else if (buffKey == ModItemKeys.Buff_B_Metatron_LunaticMode)
            {
                return "NightmareEve.B_Metatron_LunaticMode";
            }

            return string.Empty;
        }

        private static void SetClassName(Dictionary<string, object> masterJson, string key, string className)
        {
            if (!masterJson.TryGetValue(key, out object entryObject))
            {
                return;
            }

            Dictionary<string, object> entry = entryObject as Dictionary<string, object>;
            if (entry == null)
            {
                return;
            }

            entry["ClassName"] = className;
        }

        [HarmonyPatch(typeof(BloodyMist))]
        [HarmonyPatch(nameof(BloodyMist.DoubleBattle))]
        class BMist4Disable
        {
            [HarmonyPrefix]
            static bool Prefix(BloodyMist __instance)
            {
                return false;
            }
        }
    }
}
