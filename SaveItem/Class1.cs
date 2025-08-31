using GameDataEditor;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using TileTypes;
using System.Reflection.Emit;
using System.Reflection;
using I2.Loc;
using Random = System.Random;
using System.Collections;
using TMPro;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.ModData;
using ChronoArkMod.ModData.Settings;
using ChronoArkMod.Plugin;

namespace SaveItem
{
    [PluginConfig("SaveItem", "SaveItem", "1.0.0")]
    public class SaveItemPlugin : ChronoArkPlugin
    {

        public static string savedItem = "";
        public static bool enableDontDel = false;

        private Harmony harmony;
        public override void Initialize()
        {
            ModInfo modInfo = ModManager.getModInfo("SaveItem");
            enableDontDel = modInfo.GetSetting<ToggleSetting>("enableDontDel").Value;

            this.harmony = new Harmony(base.GetGuid());
            this.harmony.PatchAll();
        }
        public override void Dispose()
        {
            this.harmony.UnpatchSelf();
        }

        [HarmonyPatch(typeof(PasswordWindow), "ApplyBtnClick")]
        class PW_Patch
        {
            static void store(ItemBase item)
            {

                MasterAudio.PlaySound("TimeVaultDoor", 1f, null, 0f, null, null, false, false);
                SaveManager.NowData.SaveedKey = item.itemkey;
                Debug.Log("Stored " + item.itemkey);
                savedItem = item.itemkey;
            }
            static bool Prefix(PasswordWindow __instance)
            {
                string text = __instance.PlayerTextfield.text;
                Debug.Log("text: " + text);
                List<ItemBase> reward = new List<ItemBase>();
                List<ItemBase> itemList = PlayData.ALLITEMLIST;

                if (text.ToLower() == "item")
                {
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 4)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 3)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 2)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 1)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 0)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in PlayData.ALLITEMLIST)
                    {
                        if (item is Item_Passive)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "relic")
                {
                    foreach (ItemBase item in PlayData.ALLITEMLIST)
                    {
                        if (item is Item_Passive)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "equip" || text.ToLower() == "equipment")
                {
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 4)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 3)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 2)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 1)
                        {
                            reward.Add(item);
                        }
                    }
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 0)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "legendary" || text.ToLower() == "yellow")
                {
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 4)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "heroic" || text.ToLower() == "purple")
                {
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 3)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "rare" || text.ToLower() == "blue")
                {
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 2)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "uncommon" || text.ToLower() == "green")
                {
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 1)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "common" || text.ToLower() == "white")
                {
                    foreach (ItemBase item in itemList)
                    {
                        if (item is Item_Equip && item.ItemClassNum == 0)
                        {
                            reward.Add(item);
                        }
                    }
                    UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(
                        reward, new RandomItemBtn.SelectItemClickDel(store), false);
                }

                else if (text.ToLower() == "wipe")
                {
                    SaveManager.NowData.SaveedKey = "";
                    savedItem = "";
                    MasterAudio.PlaySound("Wind_Swoosh_04", 1f, null, 0f, null, null, false, false);
                }

                else
                {
                    return true;
                }

                __instance.PlayerTextfield.text = "";
                return false;
            }

        }

        [HarmonyPatch(typeof(SelectItemUI), "Update")]
        class MoveUI_Patch
        {
            static void Postfix()
            {
                //Debug.Log("Update");
                GamepadManager.IsLayoutMode = false;
            }
        }

        [HarmonyPatch(typeof(PlayData), "GameEndInit")]
        class DontDel2
        {
            [HarmonyPostfix]
            static void Postfix()
            {
                if (enableDontDel)
                {
                    SaveManager.NowData.SaveedKey = savedItem;
                    Debug.Log("Overwrite save item");
                }
            }
        }

        [HarmonyPatch(typeof(CharSelectMainUIV2), "Init")]
        class DontDel3
        {
            [HarmonyPrefix]
            static bool Prefix()
            {
                if (enableDontDel)
                {
                    if (SaveManager.NowData.SaveedKey == "" && savedItem != "")
                    {
                        SaveManager.NowData.SaveedKey = savedItem;
                        Debug.Log("Overwrite save item2");
                    }
                }
                return true;
            }
        }
    }
}


