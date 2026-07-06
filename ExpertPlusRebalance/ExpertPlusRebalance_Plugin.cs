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
using System.Text.RegularExpressions;

namespace ExpertPlusRebalance
{
    public class ExpertPlusRebalance_Plugin : ChronoArkPlugin
    {
        private Harmony harmony;
        public static string json = "";
        public override void Dispose()
        {
            this.harmony.UnpatchSelf();
        }

        public override void Initialize()
        {
            this.harmony = new Harmony(base.GetGuid());
            this.harmony.PatchAll();
        }

        // Modify gdata.json
        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class ModifyGData
        {
            static void Prefix(ref string dataString)
            {
                Debug.Log("Modifying gdata.json!!!!");
                Dictionary<string, object> masterJson = (Json.Deserialize(dataString) as Dictionary<string, object>);
                foreach (var e in masterJson)
                {
                    if (((Dictionary<string, object>)e.Value).ContainsKey("_gdeSchema"))
                    {
                        // Hein
                        if (e.Key == "S_Hein_6")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;
                        }
                        if (e.Key == "S_Hein_14")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Effect_Self"] = "SE_Strangth";
                        }
                        if (e.Key == "SE_Hein_14_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 75;
                        }

                        // Trisha
                        if (e.Key == "S_Trisha_7")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NoBasicSkill"] = false;
                        }
                        if (e.Key == "SE_Trisha_11_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["CRI"] = 0;
                        }
                        if (e.Key == "B_Trisha_0_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["LifeTime"] = 0;
                        }

                        // Azar
                        if (e.Key == "S_Azar_2")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["UseAp"] = 2;
                        }
                        if (e.Key == "S_Azar_7")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["UseAp"] = 3;
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;
                            List<string> list = new List<string>();
                            list.Add("Trisha_5_Ex");
                            (masterJson[e.Key] as Dictionary<string, object>)["SKillExtendedItem"] = list;
                        }

                        // Charon
                        if (e.Key == "S_ShadowPriest_13")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;
                        }
                        if (e.Key == "ShadowPriest")
                        {
                            ((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["x"] = 20;
                            ((masterJson[e.Key] as Dictionary<string, object>)["MAXHP"] as Dictionary<string, object>)["y"] = 30;
                        }

                        // Silverstein
                        if (e.Key == "S_SilverStein_4")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Except"] = false;
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = true;
                        }

                        // Johan
                        if (e.Key == "SE_Mement_1_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 125;
                        }
                        if (e.Key == "SE_Mement_3_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 135;
                        }

                        // Ilya
                        if (e.Key == "SE_Ilya_0_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 95;
                        }
                        if (e.Key == "SE_Ilya_9_Rare_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 130;
                        }

                        // Sizz
                        if (e.Key == "SE_Sizz_4_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 140;
                        }
                        if (e.Key == "B_Sizz_4_S")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["LifeTime"] = 1;
                        }

                        // Pressel
                        if (e.Key == "SE_Priest_2_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 200;
                        }

                        // Huz
                        if (e.Key == "S_Queen_10")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NoBasicSkill"] = true;
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;

                            List<string> list = new List<string>();
                            list.Add("Trisha_5_Ex");
                            (masterJson[e.Key] as Dictionary<string, object>)["SKillExtendedItem"] = list;
                        }
                        if (e.Key == "SE_Queen_6_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 140;
                        }
                        if (e.Key == "SE_Queen_11_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 150;
                        }
                        if (e.Key == "B_Queen_1_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["LifeTime"] = 0;
                        }

                        // Leryn
                        if (e.Key == "Leryn")
                        {
                            ((masterJson[e.Key] as Dictionary<string, object>)["HIT_DEBUFF"] as Dictionary<string, object>)["x"] = 15;
                            ((masterJson[e.Key] as Dictionary<string, object>)["HIT_DEBUFF"] as Dictionary<string, object>)["y"] = 55;
                        }
                        if (e.Key == "S_Leryn_6")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["IgnoreTaunt"] = true;
                        }
                        if (e.Key == "S_Leryn_9")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NotCount"] = true;
                        }
                        if (e.Key == "B_Leryn_10_Rare")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["LifeTime"] = 1;
                        }
                        if (e.Key == "S_Leryn_11_Rare")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;
                        }

                        // Miss Chain
                        if (e.Key == "S_MissChain_0")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NotCount"] = true;
                        }
                        if (e.Key == "SE_MissChain_T_4")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 75;
                            (masterJson[e.Key] as Dictionary<string, object>)["HIT"] = 100;
                        }
                        if (e.Key == "SE_MissChain_11_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 100;
                        }
                        if (e.Key == "S_MissChain_1")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["UseAp"] = 2;
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;
                        }

                        // Momori
                        if (e.Key == "SE_Momori_3_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 120;
                        }

                        // Phoenix
                        if (e.Key == "S_Phoenix_1")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["UseAp"] = 1;
                        }
                        if (e.Key == "S_Phoenix_8")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;
                        }
                        if (e.Key == "SE_Phoenix_3_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 175;
                        }
                        if (e.Key == "SE_Phoenix_7_T")
                        {
                            List<string> list = new List<string>();
                            list.Add("B_Phoenix_7_T");
                            list.Add("B_Strangth");
                            (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = list;
                        }

                        // Narhan
                        if (e.Key == "S_Control_2")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NotCount"] = true;
                        }

                        // Ironheart
                        if (e.Key == "S_Prime_6")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["UseAp"] = 1;
                        }
                        if (e.Key == "S_Prime_1")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NotCount"] = false;
                        }
                        if (e.Key == "S_Prime_12")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NotCount"] = true;
                        }
                        if (e.Key == "S_Prime_8")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["UseAp"] = 1;
                            (masterJson[e.Key] as Dictionary<string, object>)["Disposable"] = false;
                        }

                        // Helia
                        if (e.Key == "TW_Red")
                        {
                            ((masterJson[e.Key] as Dictionary<string, object>)["HIT_DOT"] as Dictionary<string, object>)["x"] = 15;
                            ((masterJson[e.Key] as Dictionary<string, object>)["HIT_DOT"] as Dictionary<string, object>)["y"] = 50;
                        }
                        if (e.Key == "SE_TW_Red_R0_T")
                        {
                            List<string> list = new List<string>();
                            list.Add("B_TW_Red_3_T");
                            (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = list;
                        }
                        // Selena
                        if (e.Key == "SE_TW_Blue_7_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["HEAL_Per"] = 100;
                        }
                        if (e.Key == "SE_TW_Blue_R0_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["HEAL_Per"] = 80;
                        }
                    }

                }
                dataString = Json.Serialize(masterJson);
                json = dataString;
            }
        }

            [HarmonyPatch(typeof(Extended_Trisha_3), nameof(Extended_Trisha_3.AttackEffectSingle))]
            class IllusionStrikePatch
            {
                [HarmonyPrefix]
                static bool Prefix(Extended_Trisha_3 __instance)
                {
                    return false;
                }
            }

            [HarmonyPatch(typeof(Skill_Extended), nameof(Skill_Extended.SkillUseSingle))]
            class IllusionStrikePatch3
            {
                [HarmonyPostfix]
                static void Postfix(Skill SkillD, List<BattleChar> Targets, Skill_Extended __instance)
                {
                    if (__instance is Extended_Trisha_3 == false)
                    {
                        return;
                    }
                    Debug.Log("Illusion Strike");

                    // Access Private variable
                    AccessTools.FieldRef<Extended_Trisha_3, BattleChar> TargetTemp = AccessTools.FieldRefAccess<BattleChar>(typeof(Extended_Trisha_3), "TargetTemp");
                    TargetTemp(__instance as Extended_Trisha_3) = Targets[0];

                    List<Skill> list = new List<Skill>();
                    if (Targets[0].IsDead)
                    {
                        using (List<BattleEnemy>.Enumerator enumerator = BattleSystem.instance.EnemyList.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                BattleEnemy battleEnemy = enumerator.Current;
                                battleEnemy.BuffAdd(GDEItemKeys.Buff_B_Trisha_3_S, __instance.BChar, false, 0, false, -1, false);
                            }
                            return;
                        }
                    }
                    list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Trisha_3_0, __instance.MySkill.Master, __instance.MySkill.Master.MyTeam));
                    list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Trisha_3_1, __instance.MySkill.Master, __instance.MySkill.Master.MyTeam));
                    BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel((__instance as Extended_Trisha_3).Del), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, false));
                }
            }

            [HarmonyPatch(typeof(Extended_Azar_9), nameof(Extended_Azar_9.SkillUseSingle))]
            class SwordofInfinityPatch
            {
                [HarmonyPostfix]
                static void Postfix(Extended_Azar_9 __instance)
                {
                    if (__instance.MySkill.ExtendedFind_DataName(GDEItemKeys.SkillExtended_Azar_Ex_0) != null)
                    {
                        Skill skill = __instance.MySkill;
                        BattleSystem.instance.AllyTeam.Skills_UsedDeck.Add(skill);
                    }
                }
            }

            [HarmonyPatch(typeof(B_ShadowPriest_11_T), nameof(B_ShadowPriest_11_T.Dead))]
            class DarkBlessingPatch
            {
                [HarmonyPrefix]
                static bool Prefix(B_ShadowPriest_11_T __instance)
                {
                    List<BattleChar> list = new List<BattleChar>();
                    list.AddRange(__instance.BChar.MyTeam.AliveChars);
                    list.Remove(__instance.BChar);
                    if (list.Count == 0)
                    {
                        Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_ShadowPriest_11, __instance.Usestate_F, __instance.Usestate_F.MyTeam);
                        BattleSystem.instance.AllyTeam.Add(skill, true);
                    }

                    BattleChar battleChar = list.Random(__instance.BChar.GetRandomClass().Main);
                    for (int i = 0; i < __instance.StackNum; i++)
                    {
                        battleChar.BuffAdd(GDEItemKeys.Buff_B_ShadowPriest_11_T, __instance.Usestate_L, false, 500, false, __instance.StackInfo[i].RemainTime, false);
                    }

                    return false;
                }
            }

            [HarmonyPatch(typeof(S_ShadowPriest_3), nameof(S_ShadowPriest_3.SkillUseSingle))]
            class DarkBarrierPatch
            {
                [HarmonyPrefix]
                static bool Prefix(Skill SkillD, List<BattleChar> Targets, S_ShadowPriest_3 __instance)
                {
                    Targets[0].Damage(__instance.BChar, 12, false, true, false, 0, false, false, false);
                    return false;
                }
            }
            [HarmonyPatch(typeof(S_ShadowPriest_3), nameof(S_ShadowPriest_3.DescExtended))]
            class DarkBarrierPatch2
            {
                [HarmonyPostfix]
                static void Postfix(string desc, ref string __result)
                {
                    __result = desc.Replace("&a", "12");
                }
            }
            [HarmonyPatch(typeof(B_ShadowPriest_3_T), nameof(B_ShadowPriest_3_T.Init))]
            class DarkBarrierPatch3
            {
                [HarmonyPrefix]
                static bool Prefix(B_ShadowPriest_3_T __instance)
                {
                    __instance.PlusStat.RES_DOT = 33f;
                    __instance.BarrierHP += (int)Misc.PerToNum(__instance.Usestate_L.GetStat.atk, 188f);
                    return false;
                }
            }

            [HarmonyPatch(typeof(Mement_4_Ex), nameof(Mement_4_Ex.Init))]
            class ImitatePatch
            {
                [HarmonyPrefix]
                static bool Prefix(Mement_4_Ex __instance)
                {
                    __instance.PlusPerStat.Damage = -50;
                    return false;
                }
            }

            [HarmonyPatch(typeof(S_Ilya_3), nameof(S_Ilya_3.IlyaWaste))]
            class DescendSheathePatch
            {
                [HarmonyPrefix]
                static bool Prefix(S_Ilya_3 __instance)
                {
                    BattleSystem.instance.AllyTeam.Draw(2);
                    return false;
                }
            }

            [HarmonyPatch(typeof(S_Ilya_8_Rare), nameof(S_Ilya_8_Rare.Del))]
            class ThunderFrostPatch
            {
                [HarmonyPrefix]
                static bool Prefix(SkillButton Mybutton, S_Ilya_8_Rare __instance)
                {
                    Mybutton.Myskill.FreeUse = true;
                    if (Mybutton.Myskill.MySkill.KeyID == GDEItemKeys.Skill_S_Ilya_8_0)
                    {
                        if (BattleSystem.instance.EnemyList.Count >= 1)
                        {
                            BattleSystem.instance.EnemyList.Random(__instance.BChar.GetRandomClass().Main).BuffAdd(GDEItemKeys.Buff_B_Ilya_4_T, __instance.BChar, false, 10, false, -1, false);
                        }
                        BattleSystem.instance.AllyTeam.LucyAlly.BuffAdd(GDEItemKeys.Buff_B_Ilya_8_lucy, __instance.BChar, false, 0, false, -1, false);
                    }
                    if (Mybutton.Myskill.MySkill.KeyID == GDEItemKeys.Skill_S_Ilya_8_1)
                    {
                        List<Skill> list = new List<Skill>();
                        list.AddRange(__instance.MySkill.Master.MyTeam.Skills);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].CharinfoSkilldata == __instance.MySkill.CharinfoSkilldata)
                            {
                                list.RemoveAt(i);
                                break;
                            }
                        }
                        if (list.Count >= 1)
                        {
                            BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(__instance.SkillButton), ScriptLocalization.System_SkillSelect.WasteSkill, false, false, true, false, false));
                        }
                    }

                    return false;
                }
            }

            [HarmonyPatch(typeof(B_Ilya_4_T), nameof(B_Ilya_4_T.Buffadded))]
            class FrostbitePatch
            {
                [HarmonyPrefix]
                static bool Prefix(BattleChar BuffUser, BattleChar BuffTaker, Buff addedbuff, B_Ilya_4_T __instance)
                {
                    if (BuffTaker == __instance.BChar && addedbuff.BuffData.Debuff)
                    {
                        if (__instance.StackNum == 2)
                        {
                            BattleSystem.DelayInput(__instance.Damage((int)(__instance.Usestate_F.GetStat.atk * 0.56f)));
                            return false;
                        }
                        BattleSystem.DelayInput(__instance.Damage((int)(__instance.Usestate_F.GetStat.atk * 0.33f)));
                    }
                    return false;
                }
            }

        //[HarmonyPatch(typeof(Buff), nameof(Buff.DescExtended))]
        //class FrostbitePatch2
        //{
        //    [HarmonyPrefix]
        //    static bool Prefix(string desc, Buff __instance, ref string __result)
        //    {
        //        if (__instance is B_Ilya_4_T)
        //        {
        //            Debug.Log("Frost replacing");
        //            if (__instance.StackNum == 2)
        //            {
        //                __result = desc;
        //                Regex.Replace(__result, @"\d+", ((int)(__instance.Usestate_F.GetStat.atk * 0.56f)).ToString());
        //                return false;
        //            }
        //            __result = desc;
        //            Regex.Replace(__result, @"\d+", ((int)(__instance.Usestate_F.GetStat.atk * 0.33f)).ToString());
        //        }
        //        return false;
        //    }
        //}

        [HarmonyPatch(typeof(B_Joey_T_0), nameof(B_Joey_T_0.Init))]
            class ChemicalWeaponPatch
            {
                [HarmonyPostfix]
                static void Postfix(B_Joey_T_0 __instance)
                {
                    __instance.PlusPerStat.Damage = 15;
                }
            }

        [HarmonyPatch(typeof(Extended_Priest_2), nameof(Extended_Priest_2.DescExtended))]
        class FirstClassPatch
        {
            [HarmonyPrefix]
            static bool Prefix(string desc, Extended_Priest_2 __instance, ref string __result)
            {
                __result = desc.Replace("&a", BattleChar.CalculationResult(__instance.BChar.GetStat.reg, 15, 0).ToString()).Replace("&b", ((int)(__instance.BChar.GetStat.atk * 1.5f)).ToString());
                return false;
            }
        }
        [HarmonyPatch(typeof(Extended_Priest_2), nameof(Extended_Priest_2.FixedUpdate))]
            class FirstClassPatch2
            {
                [HarmonyPrefix]
                static bool Prefix(Extended_Priest_2 __instance)
                {
                    if (__instance.PassiveDraw)
                    {
                        __instance.SkillBasePlus.Target_BaseDMG = (int)(__instance.BChar.GetStat.atk * 1.5f);
                    }
                    return false;
                }
            }
            [HarmonyPatch(typeof(Extended_Priest_2), nameof(Extended_Priest_2.SkillUseSingle))]
            class FirstClassPatch3
            {
                [HarmonyPrefix]
                static bool Prefix(Skill SkillD, List<BattleChar> Targets, Extended_Priest_2 __instance)
                {

                    if (__instance.PassiveDraw)
                    {
                        __instance.SkillBasePlus.Target_BaseDMG = (int)(__instance.BChar.GetStat.atk * 1.5f);
                    }
                    return false;
                }
            }

            [HarmonyPatch(typeof(B_Queen_6), nameof(B_Queen_6.Init))]
            class CrackPatch
            {
                [HarmonyPostfix]
                static void Postfix(B_Queen_6 __instance)
                {
                    if (__instance.View)
                    {
                        __instance.PlusPerStat.Damage = (int)(-__instance.BChar.GetStat.reg * 1.5f);
                        return;
                    }
                    if (__instance.Usestate_F != null)
                    {
                        __instance.PlusPerStat.Damage = (int)(-__instance.Usestate_F.GetStat.reg * 1.5f);
                    }
                }
            }

            [HarmonyPatch(typeof(B_Leryn_10), nameof(B_Leryn_10.TurnEnd))]
            class MobiusPatch
            {
                [HarmonyPrefix]
                static bool Prefix(B_Leryn_10 __instance)
                {

                    return false;
                }
            }

        [HarmonyPatch(typeof(B_MissChain_P), nameof(B_MissChain_P.Init))]
        class BurnnnPatch
        {
            [HarmonyPostfix]
            static void Postfix(B_MissChain_P __instance)
            {
                __instance.PlusPerStat.Damage = 15;
            }
        }

        [HarmonyPatch(typeof(Extended_MissChain_T_6), nameof(Extended_MissChain_T_6.Del))]
        class PursuitPatch
        {
            [HarmonyPrefix]
            static bool Prefix(SkillButton Mybutton, Extended_MissChain_T_6 __instance)
            {
                Mybutton.Myskill.Master.MyTeam.ForceDrawF(Mybutton.Myskill, (List<BattleTeam.DrawInput>)null);
                Skill_Extended skill_Extended = new Skill_Extended();
                skill_Extended.APChange = -1;
                Mybutton.Myskill.ExtendedAdd(skill_Extended);

                return false;
            }
        }

        [HarmonyPatch(typeof(B_Lian_6_T), nameof(B_Lian_6_T.BuffStat))]
        class RelentlessSwipePatch
        {
            [HarmonyPostfix]
            static void Postfix(B_Lian_6_T __instance)
            {
                __instance.PlusStat.def = -25f;
            }
        }

        [HarmonyPatch(typeof(B_Lian_3_S), nameof(B_Lian_3_S.BuffStat))]
        class BringitonPatch
        {
            [HarmonyPostfix]
            static void Postfix(B_Lian_6_T __instance)
            {
                __instance.PlusStat.def = 25f;
            }
        }

        [HarmonyPatch(typeof(Buff), nameof(Buff.FixedUpdate))]
        class PreparationPatch
        {
            [HarmonyPostfix]
            static void Postfix(Buff __instance)
            {
                if (__instance is B_Lian_10_S)
                {
                    __instance.PlusPerStat.Damage = (int)(__instance.BChar.GetStat.def);
                }
            }
        }

        [HarmonyPatch(typeof(B_Momori_6_T), nameof(B_Momori_6_T.BuffStat))]
        class AnyStrongGuysPatch
        {
            [HarmonyPostfix]
            static void Postfix(B_Momori_6_T __instance)
            {
                __instance.PlusStat.HEALTaken = 15f;
            }
        }

        [HarmonyPatch(typeof(S_Momori_1), nameof(S_Momori_1.TargetSelectExcept))]
        class LoserLoserPatch
        {
            [HarmonyPrefix]
            static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(B_Control_P), nameof(B_Control_P.BuffStat))]
        class IdentifiedPatch
        {
            [HarmonyPostfix]
            static void Postfix(B_Control_P __instance)
            {
                __instance.PlusStat.RES_CC = -15f;
            }
        }

        [HarmonyPatch(typeof(S_Control_12), nameof(S_Control_12.Del))]
        class MentalistPatch
        {
            [HarmonyPrefix]
            static bool Prefix(SkillButton Mybutton, S_Control_12 __instance)
            {
                if (Mybutton.Myskill.MySkill.KeyID == GDEItemKeys.Skill_S_Control_12_0)
                {
                    __instance.TargetTemp.BuffAdd(GDEItemKeys.Buff_B_Control_12_0_T, __instance.BChar, false, 0, false, -1, false).BarrierHP += (int)((float)__instance.BChar.GetStat.maxhp * 0.7f);
                }
                if (Mybutton.Myskill.MySkill.KeyID == GDEItemKeys.Skill_S_Control_12_1)
                {
                    List<Buff> buffs = __instance.TargetTemp.GetBuffs(BattleChar.GETBUFFTYPE.CC, true, false);
                    foreach (Buff buff in buffs)
                    {
                        buff.SelfDestroy(false);
                    }
                    List<Buff> buffs2 = __instance.TargetTemp.GetBuffs(BattleChar.GETBUFFTYPE.DEBUFF, true, false);
                    foreach (Buff buff in buffs2)
                    {
                        buff.SelfDestroy(false);
                    }
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(S_Prime_6), nameof(S_Prime_6.AttackEffectSingle))]
        class DuelPatch
        {
            [HarmonyPrefix]
            static bool Prefix(Extended_Prime_S_4 __instance)
            {
                return false;
            }
        }


        [HarmonyPatch(typeof(Extended_Prime_S_4), nameof(Extended_Prime_S_4.SkillUseSingle))]
        class FrontlineCoverPatch
        {
            [HarmonyPrefix]
            static bool Prefix(Extended_Prime_S_4 __instance)
            {
                __instance.BChar.MyTeam.partybarrier.BarrierHP += (int)((float)__instance.BChar.GetStat.maxhp * 0.8f);
                return false;
            }
        }
        [HarmonyPatch(typeof(Extended_Prime_S_4), nameof(Extended_Prime_S_4.DescExtended))]
        class FrontlineCoverPatch2
        {
            [HarmonyPrefix]
            static bool Prefix(string desc, Extended_Prime_S_4 __instance, ref string __result)
            {
                __result = desc.Replace("&a", ((int)((float)__instance.BChar.GetStat.maxhp * 0.8f)).ToString());
                return false;
            }
        }

        [HarmonyPatch(typeof(B_Prime_13_T), nameof(B_Prime_13_T.BuffStat))]
        class HighEnergyPatch
        {
            [HarmonyPostfix]
            static void Postfix(B_Prime_13_T __instance)
            {
                __instance.PlusStat.HEALTaken = 30f;
            }
        }

        [HarmonyPatch(typeof(Extended_Prime_S_0), nameof(Extended_Prime_S_0.FixedUpdate))]
        class ShieldBashPatch
        {
            [HarmonyPrefix]
            static bool Prefix(Extended_Prime_S_0 __instance)
            {
                AccessTools.FieldRef<Extended_Prime_S_0, int> barrierHP = AccessTools.FieldRefAccess<int>(typeof(Extended_Prime_S_0), "Barrierhp");
                barrierHP(__instance) = 0;
                foreach (BattleAlly battleAlly in __instance.BChar.BattleInfo.AllyList)
                {
                    barrierHP(__instance) += battleAlly.BarrierHP;
                }
                if (barrierHP(__instance) >= 1)
                {
                    __instance.SkillBasePlus.Target_BaseDMG = (int)((float)barrierHP(__instance) * 2f);
                    (__instance as Skill_Extended).SkillParticleOn();
                    return false;
                }
                __instance.SkillBasePlus.Target_BaseDMG = 0;
                (__instance as Skill_Extended).SkillParticleOff();

                return false;
            }
        }

        [HarmonyPatch(typeof(S_Prime_11), nameof(S_Prime_11.SkillUseTeam))]
        class ShieldofRetributionPatch
        {
            [HarmonyPostfix]
            static void Postfix(S_Prime_11 __instance)
            {
                __instance.LeftNum = 99;
            }
        }

    }
    }
