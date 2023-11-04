using BepInEx;
using BepInEx.Configuration;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ChronoArkMod;
using ChronoArkMod.ModData;
using ChronoArkMod.ModData.Settings;
using ChronoArkMod.Plugin;


namespace FirstRareFull
{
    [PluginConfig("FirstRareFull", "FirstRareFull", "1.0.0")]
    public class FirstRarePlugin : ChronoArkPlugin
    {
        public const string GUID = "org.windy.firstrare";
        public const string version = "0.1.0";

        public static bool firstlearn = true;

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

        // Golden Skillbook show all options
        [HarmonyPatch(typeof(UseItem.SkillBookCharacter_Rare), "Use")]
        class RareOptions
        {
            [HarmonyPrefix]
            static bool Prefix(UseItem.SkillBookCharacter_Rare __instance, ref bool __result)
            {
                if (firstlearn)
                {
                    List<Skill> list = new List<Skill>();
                    List<BattleAlly> battleallys = PlayData.Battleallys;
                    BattleTeam tempBattleTeam = PlayData.TempBattleTeam;
                    for (int i = 0; i < PlayData.TSavedata.Party.Count; i++)
                    {
                        bool flag = false;
                        if (PlayData.TSavedata.SpRule == null || !PlayData.TSavedata.SpRule.RuleChange.CharacterRareSkillInfinityGet)
                        {
                            using (List<CharInfoSkillData>.Enumerator enumerator = PlayData.TSavedata.Party[i].SkillDatas.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    if (enumerator.Current.Skill.Rare)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            if (PlayData.TSavedata.Party[i].BasicSkill.Rare)
                            {
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            // Changed Here
                            List<GDESkillData> gdeskillData = PlayData.GetMySkills(PlayData.TSavedata.Party[i].KeyData, true).GroupBy(x => x.KeyID).Select(x => x.First()).ToList();
                            if (gdeskillData != null)
                            {
                                foreach (GDESkillData skill in gdeskillData)
                                {
                                    list.Add(Skill.TempSkill(skill.KeyID, battleallys[i], tempBattleTeam));
                                    Debug.Log(skill.Name);
                                }
                            }
                        }
                    }
                    if (list.Count == 0)
                    {
                        EffectView.SimpleTextout(FieldSystem.instance.TopWindow.transform, ScriptLocalization.System.CantRareSkill, 1f, false, 1f);
                        __result = false;
                    }
                    foreach (Skill skill in list)
                    {
                        if (!SaveManager.IsUnlock(skill.MySkill.KeyID, SaveManager.NowData.unlockList.SkillPreView))
                        {
                            SaveManager.NowData.unlockList.SkillPreView.Add(skill.MySkill.KeyID);
                        }
                    }
                    PlayData.TSavedata.UseItemKeys.Add(GDEItemKeys.Item_Consume_SkillBookCharacter_Rare);
                    FieldSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(__instance.SkillAdd), ScriptLocalization.System_Item.SkillAdd, false, true, true, true, true));
                    __result = true;
                    firstlearn = false;
                    return false;
                }
                else return true;
            }
        }


        [HarmonyPatch(typeof(DataCollectMgr), "GameEnd")]
        class RareTurnBackOn
        {
            [HarmonyPostfix]
            static void Postfix()
            {
                firstlearn = true;
                Debug.Log("Rare Turned Back On");
            }
        }
    }
}
