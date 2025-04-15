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
        public const string GUID = "windy.saveitem";
        public const string version = "1.0.0";

        private Harmony harmony;
        public override void Initialize()
        {
            ModInfo modInfo = ModManager.getModInfo("SaveItem");

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
                Debug.Log("Stored " +item.itemkey);
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
                
                else
                {
                    return true;
                }

                __instance.PlayerTextfield.text = "";
                return false;
            }
        }

    }
}


