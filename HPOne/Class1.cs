using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TileTypes;
using System.Reflection.Emit;
using System.Reflection;
using I2.Loc;
using ChronoArkMod;
using ChronoArkMod.ModData;
using ChronoArkMod.ModData.Settings;
using ChronoArkMod.Plugin;
using GameDataEditor;
using DarkTonic.MasterAudio;

namespace DamageModifier
{
    [PluginConfig("HPOne", "HPOne", "1.0.0")]
    public class DamagePlugin : ChronoArkPlugin
    {
        public const string GUID = "windy.hpmod";
        public const string version = "1.0.0";
        private Harmony harmony;

        public override void Initialize()
        {
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
                        if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Character"))
                        {
                            ((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["x"] = 1;
                            ((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["y"] = 1;
                        }
                    }
                }
                dataString = Json.Serialize(masterJson);
            }
        }

        // Equip HP = 0
        [HarmonyPatch(typeof(ArkCode), "Start")]
        class ItemChange
        {
            static void Postfix()
            {
                Debug.Log("Changed equip stats");
                List<ItemBase> itemList = PlayData.ALLITEMLIST;
                foreach (ItemBase item in itemList)
                {
                    if (item is Item_Equip)
                    {
                        ((Item_Equip)item).ItemScript.PlusPerStat.MaxHP = 0;
                        ((Item_Equip)item).ItemScript.PlusStat.maxhp = 0;
                    }
                    else  if (item is Item_Passive)
                    {
                        ((Item_Passive)item).ItemScript.PlusPerStat.MaxHP = 0;
                    }
                }
            }
        }

        // Golden Bread HP gain = 0
        [HarmonyPatch(typeof(UI_Camp), "ItemUse")]
        class GoldBread
        {
            static void Postfix(ItemObject select)
            {
                if (select.Item.itemkey == GDEItemKeys.Item_Consume_GoldenBread)
                {
                    TempSaveData tsavedata = PlayData.TSavedata;
                    tsavedata.PartyPlusStat.maxhp -= 3;
                }
            }
        }

        // Cheesecake HP = 0
        [HarmonyPatch(typeof(PItem.CheeseCake), "Init")]
        class CheeseCake
        {
            static void Postfix(PItem.CheeseCake __instance)
            {
                __instance.PlusStat.maxhp = 0;
            }
        }

    }
}