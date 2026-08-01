using System;
using System.Collections.Generic;
using System.Reflection;
using DarkTonic.MasterAudio;
using GameDataEditor;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace NightmareEve
{
    internal static class FiendSoulUtility
    {
        private const string FiendSoulBuffKey = "B_FiendSoul";
        private const string FiendSoulKey = "FiendSoul";
        private static readonly Dictionary<string, string> NextMistletoeByKey = new Dictionary<string, string>
        {
            { "Mistletoe", "Mistletoe_1" },
            { "Mistletoe_1", "Mistletoe_2" },
            { "Mistletoe_2", "Mistletoe_3" },
            { "Mistletoe_3", "Mistletoe_4" },
            { "Mistletoe_4", "Mistletoe_5" },
            { "Mistletoe_5", "Mistletoe_6" },
        };

        private static readonly FieldInfo UIEnchantUseItemField = AccessTools.Field(typeof(UI_Enchant), "UseItem");
        private static readonly FieldInfo UIEnchantTextField = AccessTools.Field(typeof(UI_Enchant), "Text");
        private static readonly FieldInfo UIManagerInstanceField = AccessTools.Field(typeof(UIManager), "inst");
        private static readonly FieldInfo ItemEquipItemScriptField = AccessTools.Field(typeof(Item_Equip), "ItemScript");
        private static readonly FieldInfo MistletoeEffectField = AccessTools.Field(typeof(Mistletoe_1), "Effect");

        public static bool IsFiendSoul(ItemBase item)
        {
            return item != null && item.itemkey == FiendSoulKey;
        }

        public static ItemBase GetUseItem(UI_Enchant ui)
        {
            return UIEnchantUseItemField?.GetValue(ui) as ItemBase;
        }

        public static void SetUseItem(UI_Enchant ui, ItemBase item)
        {
            UIEnchantUseItemField?.SetValue(ui, item);
        }

        public static void SetTitle(UI_Enchant ui, string title)
        {
            TextMeshProUGUI text = UIEnchantTextField?.GetValue(ui) as TextMeshProUGUI;
            if (text != null)
            {
                text.text = title;
            }
        }

        public static bool HasUpgradeableMistletoe()
        {
            foreach (ItemBase item in EnumerateMistletoeTargets())
            {
                if (CanUpgrade(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryUpgrade(ItemBase target, out string errorMessage)
        {
            errorMessage = null;

            if (!(target is Item_Equip targetEquip))
            {
                errorMessage = "Fiend Soul can only be used on Mistletoe.";
                return false;
            }

            if (!NextMistletoeByKey.TryGetValue(targetEquip.itemkey, out string nextKey))
            {
                errorMessage = IsAnyMistletoe(targetEquip)
                    ? "That Mistletoe cannot be upgraded further."
                    : "Fiend Soul can only be used on Mistletoe.";
                return false;
            }

            bool? oldEffect = ReadEffect(targetEquip);
            if (!TryReplaceMistletoe(targetEquip, nextKey, oldEffect))
            {
                errorMessage = "Failed to upgrade that Mistletoe.";
                return false;
            }

            RefreshInventories();
            RefreshPartyWindows();
            return true;
        }

        public static void PlayUpgradeSound()
        {
            MasterAudio.PlaySound("Crystal", 1f, null, 0f, null, null, false, false);
        }

        public static void ShowMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (FieldSystem.instance != null)
            {
                EffectView.SimpleTextout(FieldSystem.instance.TopWindow.transform, message, 1f, false, 1f);
                return;
            }

            Debug.Log(message);
        }

        public static void ApplyFieldUsePatch(Dictionary<string, object> masterJson)
        {
            if (!masterJson.TryGetValue(FiendSoulKey, out object entryObject))
            {
                return;
            }

            Dictionary<string, object> entry = entryObject as Dictionary<string, object>;
            if (entry == null)
            {
                return;
            }

            entry["FieldUse"] = "FiendSoul";
        }

        public static void ApplyBuffClassPatch(Dictionary<string, object> masterJson)
        {
            if (!masterJson.TryGetValue(FiendSoulBuffKey, out object entryObject))
            {
                return;
            }

            Dictionary<string, object> entry = entryObject as Dictionary<string, object>;
            if (entry == null)
            {
                return;
            }

            entry["ClassName"] = "NightmareEve.B_FiendSoul";
        }

        private static bool CanUpgrade(ItemBase item)
        {
            return item != null && NextMistletoeByKey.ContainsKey(item.itemkey);
        }

        private static bool TryReplaceMistletoe(Item_Equip currentItem, string nextKey, bool? oldEffect)
        {
            Item_Equip newItem = ItemBase.GetItem(nextKey) as Item_Equip;
            if (newItem == null)
            {
                return false;
            }

            newItem.MyManager = currentItem.MyManager;
            WriteEffect(newItem, oldEffect);

            bool replacedInManager = ReplaceInManagerInventory(currentItem, newItem);
            bool replacedInInventory = ReplaceInInventory(currentItem, newItem);
            bool replacedInPartyEquipment = ReplaceInPartyEquipment(currentItem, newItem);
            return replacedInManager || replacedInInventory || replacedInPartyEquipment;
        }

        private static bool ReplaceInManagerInventory(ItemBase currentItem, ItemBase newItem)
        {
            if (currentItem?.MyManager?.InventoryItems == null)
            {
                return false;
            }

            bool replaced = false;
            List<ItemBase> items = currentItem.MyManager.InventoryItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (!ReferenceEquals(items[i], currentItem))
                {
                    continue;
                }

                items[i] = newItem;
                replaced = true;
            }

            return replaced;
        }

        private static bool ReplaceInInventory(ItemBase currentItem, ItemBase newItem)
        {
            if (PlayData.TSavedata?.Inventory == null)
            {
                return false;
            }

            bool replaced = false;
            for (int i = 0; i < PlayData.TSavedata.Inventory.Count; i++)
            {
                if (!ReferenceEquals(PlayData.TSavedata.Inventory[i], currentItem))
                {
                    continue;
                }

                PlayData.TSavedata.Inventory[i] = newItem;
                replaced = true;
            }

            return replaced;
        }

        private static bool ReplaceInPartyEquipment(ItemBase currentItem, ItemBase newItem)
        {
            if (PlayData.TSavedata?.Party == null)
            {
                return false;
            }

            bool replaced = false;
            foreach (Character character in PlayData.TSavedata.Party)
            {
                if (character?.Equip == null)
                {
                    continue;
                }

                for (int i = 0; i < character.Equip.Count; i++)
                {
                    if (!ReferenceEquals(character.Equip[i], currentItem))
                    {
                        continue;
                    }

                    character.Equip[i] = newItem;
                    replaced = true;
                }
            }

            return replaced;
        }

        private static IEnumerable<ItemBase> EnumerateMistletoeTargets()
        {
            if (PlayData.TSavedata == null)
            {
                yield break;
            }

            foreach (ItemBase item in PlayData.TSavedata.Inventory)
            {
                if (item != null)
                {
                    yield return item;
                }
            }

            foreach (Character character in PlayData.TSavedata.Party)
            {
                if (character == null)
                {
                    continue;
                }

                foreach (ItemBase item in character.Equip)
                {
                    if (item != null)
                    {
                        yield return item;
                    }
                }
            }
        }

        private static bool IsAnyMistletoe(ItemBase item)
        {
            return item != null
                && !string.IsNullOrEmpty(item.itemkey)
                && item.itemkey.StartsWith("Mistletoe", StringComparison.Ordinal);
        }

        private static bool? ReadEffect(Item_Equip item)
        {
            EquipBase script = ItemEquipItemScriptField?.GetValue(item) as EquipBase;
            if (script == null || MistletoeEffectField == null || !MistletoeEffectField.DeclaringType.IsInstanceOfType(script))
            {
                return null;
            }

            return (bool)MistletoeEffectField.GetValue(script);
        }

        private static void WriteEffect(Item_Equip item, bool? effect)
        {
            if (!effect.HasValue || MistletoeEffectField == null)
            {
                return;
            }

            EquipBase script = ItemEquipItemScriptField?.GetValue(item) as EquipBase;
            if (script == null || !MistletoeEffectField.DeclaringType.IsInstanceOfType(script))
            {
                return;
            }

            MistletoeEffectField.SetValue(script, effect.Value);
        }

        private static void RefreshInventories()
        {
            PartyInventory.InvenM?.ItemUpdateFromInven();

            if (FieldSystem.instance == null)
            {
                return;
            }

            foreach (AllyWindow allyWindow in FieldSystem.instance.PartyWindow)
            {
                allyWindow?.MyInven?.ItemUpdateFromInven();
            }
        }

        private static void RefreshPartyWindows()
        {
            if (FieldSystem.instance == null)
            {
                RefreshCharStatUi();
                return;
            }

            foreach (AllyWindow allyWindow in FieldSystem.instance.PartyWindow)
            {
                allyWindow?.EquipUpdate();
            }

            RefreshCharStatUi();
        }

        private static void RefreshCharStatUi()
        {
            UIManager uiManager = UIManagerInstanceField?.GetValue(null) as UIManager;
            if (uiManager?.CharstatUI == null)
            {
                return;
            }

            CharStatV4 charStat = uiManager.CharstatUI.GetComponent<CharStatV4>();
            if (charStat == null)
            {
                return;
            }

            charStat.Equip?.ItemUpdateFromInven();
            charStat.UpdateCharEquip();
        }
    }

    [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
    internal static class FiendSoulGDataPatch
    {
        private static void Prefix(ref string dataString)
        {
            Dictionary<string, object> masterJson = Json.Deserialize(dataString) as Dictionary<string, object>;
            if (masterJson == null)
            {
                return;
            }

            FiendSoulUtility.ApplyFieldUsePatch(masterJson);
            FiendSoulUtility.ApplyBuffClassPatch(masterJson);
            dataString = Json.Serialize(masterJson);
        }
    }

    [HarmonyPatch(typeof(UI_Enchant), nameof(UI_Enchant.Start))]
    internal static class FiendSoulEnchantStartPatch
    {
        private static void Postfix(UI_Enchant __instance)
        {
            if (!FiendSoulUtility.IsFiendSoul(FiendSoulUtility.GetUseItem(__instance)))
            {
                return;
            }

            FiendSoulUtility.SetTitle(__instance, "Select a Mistletoe");
        }
    }

    [HarmonyPatch(typeof(UI_Enchant), nameof(UI_Enchant.ItemUse))]
    internal static class FiendSoulEnchantItemUsePatch
    {
        private static bool Prefix(UI_Enchant __instance, ItemObject select)
        {
            ItemBase useItem = FiendSoulUtility.GetUseItem(__instance);
            if (!FiendSoulUtility.IsFiendSoul(useItem))
            {
                return true;
            }

            if (select == null || select.Item == null)
            {
                return false;
            }

            if (!FiendSoulUtility.TryUpgrade(select.Item, out string errorMessage))
            {
                FiendSoulUtility.ShowMessage(errorMessage);
                return false;
            }

            FiendSoulUtility.PlayUpgradeSound();
            __instance.SelfDestroy();
            useItem.MyManager?.DelItem(useItem, 1);
            return false;
        }
    }
}

namespace UseItem
{
    public class FiendSoul : UseitemBase
    {
        public override bool Use()
        {
            if (!NightmareEve.FiendSoulUtility.HasUpgradeableMistletoe())
            {
                NightmareEve.FiendSoulUtility.ShowMessage("No upgradeable Mistletoe was found.");
                return false;
            }

            GameObject uiObject = UIManager.InstantiateActiveAddressable(
                new GDEGameobjectDatasData(GDEItemKeys.GameobjectDatas_GUI_ItemEnchant).Gameobject_Path,
                AddressableLoadManager.ManageType.None);
            UI_Enchant ui = uiObject.GetComponent<UI_Enchant>();
            NightmareEve.FiendSoulUtility.SetUseItem(ui, this.MyItem);
            return false;
        }
    }
}
