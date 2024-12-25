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
using System.Reflection.Emit;
using Random = System.Random;

namespace NightmareEve
{
    public class NightmareEve_Plugin : ChronoArkPlugin
    {
        private Harmony harmony;
        public override void Dispose()
        {
            this.harmony.UnpatchSelf();
        }

        public override void Initialize()
        {
            this.harmony = new Harmony(base.GetGuid());
            this.harmony.PatchAll();

        }

        [HarmonyPatch(typeof(BattleSystem), "CustomBGM")]
        class BGMPatch
        {
            [HarmonyPostfix]
            static void Postfix()
            {

            }
        }

        [HarmonyPatch(typeof(ArkCode), "Update")]
        class ArkCodePatch
        {
            [HarmonyPostfix]
            static void Postfix(ArkCode __instance)
            {
                //Debug.Log("xxxxxxxxxxxxxxx");
                foreach (GameObject g in __instance.UnlockMainNPCList)
                {
                    g.SetActive(false);
                }
                foreach (GameObject g in __instance.UnlockQuestNPCList)
                {
                    g.SetActive(false);
                }
                __instance.AzarNomalNPC.SetActive(false);
                __instance.Rerin.SetActive(false);
                __instance.NPCs.SetActive(false);
                //__instance.LucyRoom.SetActive(false);
                //__instance.MainArk.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(ArkCode), "ArkBGMPlay")]
        class ArkCodePatch2
        {
            [HarmonyPrefix]
            static bool Prefix(ArkCode __instance)
            {
                Debug.Log("bgm");
                MasterAudio.FadeOutAllOfSound("Ark_ambience_loop", 4f);
                MasterAudio.FadeBusToVolume("BGM", 1f, 4f, null, false, false);
                MasterAudio.FadeBusToVolume("StoryBGM", 1f, 4f, null, false, false);
                MasterAudio.FadeOutAllOfSound("bangjoo_side_loop", 4f);
                MasterAudio.FadeOutAllOfSound("bangjoo_side_ambience", 4f);
                return false;
            }
        }

        // add starting item
        [HarmonyPatch(typeof(FieldSystem))]
        class FieldSystem_Patch
        {
            [HarmonyPatch(nameof(FieldSystem.StageStart))]
            [HarmonyPostfix]
            static void StageStartPostfix()
            {
                //if (!DespairMode)
                //{
                if (PlayData.TSavedata.StageNum == 0 /*&& !PlayData.TSavedata.GameStarted*/)
                {
                    PartyInventory.InvenM.AddNewItem(ItemBase.GetItem("Mistletoe"));
                }

            }
        }


        // Smokescreen in MG2
        [HarmonyPatch(typeof(FieldStore))]
        class FieldStore_Patch
        {
            [HarmonyPatch(nameof(FieldStore.Init))]
            [HarmonyPostfix]
            static void Postfix(FieldStore __instance)
            {
                if (PlayData.TSavedata.StageNum == 1)
                {
                    __instance.StoreItems.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_RedWing));
                }
            }
        }

        // Ban Pain Sharing
        [HarmonyPatch]
        public class PainSharePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch(typeof(UI_BloodyMistUp), "LevelUnlock")]
            private static IEnumerable<CodeInstruction> BloodyMist3Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (CodeInstruction instruction in instructions)
                {
                    bool flag = instruction.Is(OpCodes.Callvirt, AccessTools.Method(typeof(List<Skill>), "Add", null, null));
                    if (flag)
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PainSharePatch), "AddOrNot", null, null));
                    }
                    else
                    {
                        yield return instruction;
                    }
                }
                yield break;
            }

            // Token: 0x06000008 RID: 8 RVA: 0x0000214C File Offset: 0x0000034C
            private static void AddOrNot(List<Skill> list, Skill skill)
            {
                bool dontDelete = false;
                if (dontDelete)
                {
                    list.Add(skill);
                }
                else
                {
                    bool flag = skill.MySkill.KeyID != GDEItemKeys.Skill_S_BloodyMist3_0;
                    if (flag)
                    {
                        list.Add(skill);
                    }
                }
            }
        }

        // Make TFK the second fight in BMist4
        [HarmonyPatch(typeof(BloodyMist))]
        [HarmonyPatch(nameof(BloodyMist.SomeOneDead))]
        class BMist3Accumulated
        {
            [HarmonyPrefix]
            static bool Prefix(BloodyMist __instance)
            {
                Debug.Log("BMist not added");
                return false;
            }
        }

        // Make TFK the second fight in BMist4
        [HarmonyPatch(typeof(BloodyMist))]
        [HarmonyPatch(nameof(BloodyMist.DoubleBattle))]
        class TFKGrave
        {
            [HarmonyPrefix]
            static bool Prefix(BloodyMist __instance)
            {
                if (PlayData.TSavedata.bMist != null && PlayData.TSavedata.bMist.Level == 4)
                {
                    __instance.Level4DoubleBoss = true;
                    FieldSystem.instance.BattleStart(new GDEEnemyQueueData("Queue_S3_Reaper"), StageSystem.instance.StageData.BattleMap.Key, false, false, "", "", false);
                    return false;
                }
                return true;
            }
        }

        // Modify gdata.json
        // Double Enemy Attack Power
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
                        // Despair Mode: Give Stat Bonuses
                        if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Enemy"))
                        {
                            if (((Dictionary<string, object>)e.Value)["Boss"].Equals(false))
                            {
                                Debug.Log("Here");
                                (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = (long)((masterJson[e.Key] as Dictionary<string, object>)["maxhp"]) * 1.2;
                                (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((masterJson[e.Key] as Dictionary<string, object>)["atk"]) * 1.2;
                            }
                        }
                    }
                }
            }
        }

        // Chaos Mode Randomization  
        static List<string> tier1 = new List<string>() { "S1_Statue1", "S1_Dochi", "S1_Maid", "S1_Table", "S1_Statue2", "S1_Pharos_Mage", "S1_Pharos_Healer", "S1_Pharos_Tanker", "S1_LittleMaid" };
        static List<string> tier2 = new List<string>() { "S1_Statue1", "S1_Dochi", "S1_Maid", "S1_Statue2", "S1_Pharos_Mage", "S1_Pharos_Tanker", "S1_LittleMaid", "S1_Butler", "S1_Pharos_Warrior" };
        static List<string> tier3 = new List<string>() { "S2_Pierrot_Bat", "S2_DochiDoll", "S2_Horse", "S2_Pierrot_Axe", "S2_Ghost", "S1_Pharos_Warrior", "S2_Pharos_Healer", "S2_Pharos_Mage" };
        static List<string> tier4 = new List<string>() { "S2_Pierrot_Bat", "S2_Horse", "S2_Pierrot_Axe", "S2_Pharos_Mage", "S2_PharosWitch", "S2_Pharos_Warrior", "S2_Pharos_Tanker", "SR_Gunner", "S1_Carpenterdoll", "S3_Wolf" };
        static List<string> tier5 = new List<string>() { "S3_SnowGiant_0", "S3_Pharos_Tanker", "S3_Pharos_HighPriest", "S3_Pharos_Assassin", "S2_Animatronics" };
        static List<string> tier6 = new List<string>() { "S4_MagicDochi", "S4_AngryDochi", "S4_Summoner", "S1_Armor", "S3_Deathbringer", "S3_Fugitive", "SR_Samurai", "MBoss_0_R" };
        static List<string> tier7 = new List<string>() { "SR_GuitarList", "SR_Shotgun", "SR_Blade" };
        static List<string>[] tiers = { tier1, tier2, tier3, tier4, tier5, tier6, tier7 };
        static string Randomize(string key, int upscale)
        {
            int j = PlayData.TSavedata.StageNum;
            for (int i = j; i < tiers.Length; i++)
            {
                foreach (string enemy in tiers[i])
                {
                    if (enemy == key)
                    {
                        if (upscale == 1)
                        {
                            if (i == tiers.Length - 1)
                            {
                                return tiers[i].Random();
                            }
                            return tiers[i + 1].Random();
                        }

                        else if (upscale == 2)
                        {
                            if (i == tiers.Length - 1)
                            {
                                return tiers[i].Random();
                            }
                            else if (i == tiers.Length - 2)
                            {
                                return tiers[i + 1].Random();
                            }
                            return tiers[i + 2].Random();
                        }

                        else
                        {
                            return tiers[i].Random();
                        }
                    }
                }
            }
            Debug.Log("Randomize failed, not on list");
            return key; // not found
        }

        // Chaos Mode enemy change, Curse apply rules
        [HarmonyPatch(typeof(BattleSystem))]
        class BattleSystem_Patch
        {
            [HarmonyPatch(nameof(BattleSystem.CreatEnemy))]
            [HarmonyPrefix]
            static bool CreatEnemyPrefix(BattleSystem __instance, string EnemyString, Vector3 Pos, bool CustomPos, ref BattleEnemy __result, bool Curse = false)
            {
                // Here
                string enemyKey = EnemyString;
                if (true)
                {
                    if (Curse)
                    {
                        Debug.Log("Summoning " + enemyKey + ", unchanged = curse");
                    }
                    //else if (__instance.BossBattle) // this is not working currently
                    //{
                    //    Debug.Log("Summoning " + enemyKey + ", unchanged = boss battle");
                    //}
                    else
                    {
                        // Roll dice
                        Random rand = new Random();
                        int a = rand.Next(1, 101);

                        // change within same tier
                        if (a <= 20)
                        {
                            Debug.Log("Randomizing " + enemyKey + ", same tier");
                            enemyKey = Randomize(enemyKey, 0);
                            Debug.Log("Summoning " + enemyKey);
                        }
                        // change within higher tier
                        else if (a <= 40)
                        {
                            Debug.Log("Randomizing " + enemyKey + ", upscaled 1");
                            enemyKey = Randomize(enemyKey, 1);
                            Debug.Log("Summoning " + enemyKey);
                        }
                        // else don't change enemydata
                        else
                        {
                            Debug.Log("Summoning " + enemyKey + ", unchanged = highroll");
                        }
                    }
                }

                GDEEnemyData gdeenemyData = new GDEEnemyData(enemyKey);
                // end of edit

                GameObject gameObject = Misc.UIInst(__instance.G_Enemy);
                gameObject.SetActive(true);
                BattleEnemy component = gameObject.GetComponent<BattleEnemy>();
                if (!SaveManager.NowData.unlockList.FoundMonster.Contains(gdeenemyData.Key))
                {
                    SaveManager.NowData.unlockList.FoundMonster.Add(gdeenemyData.Key);
                }

                component.init(gdeenemyData, __instance);

                if (Curse)
                {
                    // Misty Garden 1: Ban robust
                    if (PlayData.TSavedata.StageNum == 0)
                    {
                        List<string> list = new List<string> { "B_CursedMob_0", "B_CursedMob_1", "B_CursedMob_3", "B_CursedMob_4", "B_CursedMob_5" };
                        component.BuffAdd(list.Random(), component, false, 0, false, -1, false);
                    }
                    else
                    {
                        List<string> list = new List<string> { "B_CursedMob_0", "B_CursedMob_1", "B_CursedMob_2", "B_CursedMob_3", "B_CursedMob_4", "B_CursedMob_5" };
                        component.BuffAdd(list.Random(), component, false, 0, false, -1, false);
                    }
                }

                if (__instance.BossBattle && !component.Boss)
                {
                    Pos -= new Vector3(0f, 0f, 0.3f);
                }
                component.PosObject.transform.localPosition = Pos;
                component.PosObject.transform.localScale = new Vector3(1f, 1f, 1f);
                component.CharObject.transform.localScale = new Vector3(1f, 1f, 1f);
                if (__instance.battlecamera.enabled)
                {
                    component.TargetLook();
                }
                component.UseCustomPos = CustomPos;
                UnityEngine.Object.Instantiate<GameObject>(__instance.EnemyOut, component.PosObject.transform).transform.localPosition = new Vector3(0f, 0f, 0f);
                foreach (IP_EnemyAwake ip_EnemyAwake in BattleSystem.instance.IReturn<IP_EnemyAwake>())
                {
                    if (ip_EnemyAwake != null)
                    {
                        ip_EnemyAwake.EnemyAwake(component);
                    }
                }
                __result = component;
                return false;
            }
        }

        // Curse Code
        private static void KnuthShuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = UnityEngine.Random.Range(i, list.Count); // Don't select from the entire array on subsequent loops
                T temp = list[i]; list[i] = list[j]; list[j] = temp;
            }
        }

        static private List<MapTile> ogCursedTiles = new List<MapTile>();

        [HarmonyPatch(typeof(StageSystem))]
        class Generate_Cursed_Battles_Patch
        {
            [HarmonyPatch(nameof(StageSystem.InstantiateIsometric))]
            [HarmonyPostfix]
            static void InstantiateIsometric(HexMap ___Map)
            {
                if (!PlayData.TSavedata.IsLoaded)
                {
                    //if (___Map.StageData.Key != GDEItemKeys.Stage_Stage_Crimson)
                    //{
                    // checks are required for both tile battles and building battles
                    List<MapTile> battleList =
                        ___Map.EventTileList.FindAll(x => (x.Info.Type is Monster) ||
                        (x.TileEventObject != null && x.TileEventObject.ObjectData != null && x.TileEventObject.Monster));

                    ogCursedTiles = battleList.FindAll(x => x.Info.Cursed == true);
                    int curseCount = ogCursedTiles.Count;

                    KnuthShuffle(battleList);
                    foreach (MapTile mt in battleList)
                    {
                        // curses all fights
                        if (curseCount >= 4)
                            break;
                        if (mt.Info.Cursed == false)
                        {
                            mt.Info.Cursed = true;
                            curseCount++;
                        }
                    }
                    //}
                }
            }
        }

        // Fixes Golem AI and Averages Stats for life link. 
        [HarmonyPatch(typeof(BattleTeam), nameof(BattleTeam.MyTurn))]
        class AvarageLifeLinkMaxHpPatch
        {
            static void Postfix()
            {
                if (BattleSystem.instance != null)
                {
                    if (BattleSystem.instance.TurnNum == 0)
                    {
                        List<float> linkMaxHps = new List<float>();
                        foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
                        {
                            if (battleEnemy.BuffFind(GDEItemKeys.Buff_P_Guard_LifeShare, false))
                            {
                                linkMaxHps.Add((float)battleEnemy.Info.get_stat.maxhp);
                            }
                        }

                        if (linkMaxHps.Count > 0)
                        {
                            int avgMaxHp = (int)linkMaxHps.Average();
                            //Debug.Log(avgMaxHp);
                            foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
                            {
                                if (battleEnemy.BuffFind(GDEItemKeys.Buff_P_Guard_LifeShare, false))
                                {
                                    battleEnemy.BuffReturn(GDEItemKeys.Buff_P_Guard_LifeShare).PlusStat.maxhp += avgMaxHp - battleEnemy.Info.get_stat.maxhp;
                                }
                            }
                        }
                    }
                }
            }
        }

        // reset max hp modification on in case of uncurse
        [HarmonyPatch(typeof(SkillExtended_UnCurse), nameof(SkillExtended_UnCurse.SkillUseSingle))]
        class UncurseCardPatch
        {
            static void Prefix()
            {
                foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
                {
                    if (battleEnemy.BuffFind(GDEItemKeys.Buff_P_Guard_LifeShare, false))
                    {
                        battleEnemy.BuffReturn(GDEItemKeys.Buff_P_Guard_LifeShare).PlusStat.maxhp = 0;
                    }
                }
            }
        }

        // Fixes Ruby Sapphire Cursed HP
        [HarmonyPatch(typeof(BattleTeam), nameof(BattleTeam.MyTurn))]
        class TwinsMaxHpPatch
        {
            static void Postfix()
            {
                if (BattleSystem.instance != null)
                {
                    if (BattleSystem.instance.TurnNum == 0)
                    {
                        List<float> linkMaxHps = new List<float>();
                        foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
                        {
                            if (battleEnemy.BuffFind("B_S2_MainBoss_1_Right_P", false) || battleEnemy.BuffFind("B_S2_MainBoss_1_Left_P", false))
                            {
                                linkMaxHps.Add((float)battleEnemy.Info.get_stat.maxhp);
                            }
                        }

                        if (linkMaxHps.Count > 0)
                        {
                            int avgMaxHp = (int)linkMaxHps.Average();
                            //Debug.Log(avgMaxHp);
                            foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
                            {
                                if (battleEnemy.BuffFind("B_S2_MainBoss_1_Right_P", false))
                                {
                                    battleEnemy.BuffReturn("B_S2_MainBoss_1_Right_P").PlusStat.maxhp += avgMaxHp - battleEnemy.Info.get_stat.maxhp;
                                }
                                if (battleEnemy.BuffFind("B_S2_MainBoss_1_Left_P", false))
                                {
                                    battleEnemy.BuffReturn("B_S2_MainBoss_1_Left_P").PlusStat.maxhp += avgMaxHp - battleEnemy.Info.get_stat.maxhp;
                                }
                            }
                        }
                    }
                }
            }
        }

        // reset max hp modification on in case of uncurse
        [HarmonyPatch(typeof(SkillExtended_UnCurse), nameof(SkillExtended_UnCurse.SkillUseSingle))]
        class UncurseCardPatch2
        {
            static void Prefix()
            {
                foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
                {
                    if (battleEnemy.BuffFind("B_S2_MainBoss_1_Right_P", false))
                    {
                        battleEnemy.BuffReturn("B_S2_MainBoss_1_Right_P").PlusStat.maxhp = 0;
                    }
                    if (battleEnemy.BuffFind("B_S2_MainBoss_1_Left_P", false))
                    {
                        battleEnemy.BuffReturn("B_S2_MainBoss_1_Left_P").PlusStat.maxhp = 0;
                    }
                }
            }
        }

        // makes golems function properly with multiple actions
        // TODO add config option to disable AI modification for potential compatibility issues
        class SactuaryGolemAIPatch
        {
            // acting speed is hardcoded and independent of party speed
            private static int SanctuaryGolemSpeed(int actionCount, int totalActionNumber)
            {
                int result = 99;
                if (actionCount == totalActionNumber)
                {
                    result = 99;
                }
                else
                {
                    result = 4 + actionCount * 2;
                }
                return result;
            }


            [HarmonyPatch(typeof(AI_S4_Golem))]
            class GreenGolemPatch
            {
                [HarmonyPatch(nameof(AI_S4_Golem.SkillSelect))]
                [HarmonyPrefix]
                static bool SkillSelectPrefix(ref Skill __result, AI_S4_Golem __instance)
                {
                    __result = __instance.BChar.Skills[0];
                    return false;
                }

                [HarmonyPatch(nameof(AI_S4_Golem.SpeedChange))]
                [HarmonyPrefix]
                static bool SpeedChangetPrefix(ref int __result, Skill skill, int ActionCount, int OriginSpeed, AI_S4_Golem __instance)
                {
                    __result = SanctuaryGolemSpeed(ActionCount, __instance.BChar.Info.PlusActCount.Count);
                    return false;
                }

            }

            [HarmonyPatch(typeof(AI_S4_Golem2))]
            class YellowGolemPatch
            {
                [HarmonyPatch(nameof(AI_S4_Golem2.SkillSelect))]
                [HarmonyPrefix]
                static bool SkillSelectPrefix(ref Skill __result, AI_S4_Golem2 __instance)
                {
                    __result = __instance.BChar.Skills[0];
                    return false;
                }
                [HarmonyPatch(nameof(AI_S4_Golem2.SpeedChange))]
                [HarmonyPrefix]
                static bool SpeedChangetPrefix(ref int __result, Skill skill, int ActionCount, int OriginSpeed, AI_S4_Golem2 __instance)
                {
                    __result = SanctuaryGolemSpeed(ActionCount, __instance.BChar.Info.PlusActCount.Count);
                    return false;
                }
            }
        }

        //Lift Card Placement at bottom
        [HarmonyPatch(typeof(BattleTeam))]
        class Move_Uncurse_Card_Patch
        {
            [HarmonyPatch(nameof(BattleTeam.MyTurn))]
            [HarmonyPostfix]
            static void MyTurnPostfix()
            {
                if (BattleSystem.instance != null)
                {
                    if (BattleSystem.instance.TurnNum == 0)
                    {
                        Skill temp = BattleSystem.instance.AllyTeam.Skills.Find(x => x.MySkill.KeyID == GDEItemKeys.Skill_S_UnCurse);
                        if (temp != null)
                        {
                            BattleSystem.instance.AllyTeam.Skills.Remove(temp);
                            BattleSystem.instance.AllyTeam.Add(temp, true);
                        }
                    }
                }
            }
        }


        // Ban Miniboss Curse from CW
        [HarmonyPatch(typeof(BattleSystem), nameof(BattleSystem.CurseEnemySelect))]

        class CurseEnemySelect
        {
            static void Postfix(ref string __result, List<GDEEnemyData> Enemydatas, BattleSystem __instance)
            {
                var cwMiniBosses = new HashSet<string>() { GDEItemKeys.Enemy_SR_GuitarList, GDEItemKeys.Enemy_SR_Shotgun, GDEItemKeys.Enemy_SR_Blade, GDEItemKeys.Enemy_SR_Outlaw, GDEItemKeys.Enemy_SR_Sniper };
                var mbList = Enemydatas.FindAll(data => !cwMiniBosses.Contains(data.Key));
                if (mbList.Count > 0)
                {
                    __result = mbList.Random().Key;
                }
            }
        }
    }
}