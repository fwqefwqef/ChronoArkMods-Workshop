﻿using GameDataEditor;
using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TileTypes;
using System.Reflection.Emit;
using System.Reflection;
using I2.Loc;
using System.Collections;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.ModData;
using ChronoArkMod.ModData.Settings;
using ChronoArkMod.Plugin;

namespace BossChanges
{
    [PluginConfig("BossChanges", "BossChanges", "1.0.0")]
    public class ExpertPlusBossPlugin : ChronoArkPlugin
    {
        public const string GUID = "windy.bosschanges";
        public const string version = "1.0.0";
        public static bool EnableBossChanges = true;

        private Harmony harmony;


        public override void Initialize()
        {
            ModInfo modInfo = ModManager.getModInfo("ExpertPlusMod");
            EnableBossChanges = modInfo.GetSetting<ToggleSetting>("EnableBossChanges").Value;
            this.harmony = new Harmony(base.GetGuid());
            if (EnableBossChanges)
            {
                this.harmony.PatchAll();
            }
        }
        public override void Dispose()
        {
            if (EnableBossChanges)
            {
                this.harmony.UnpatchSelf();
            }
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
                    //Debug.Log(e);

                    if (((Dictionary<string, object>)e.Value).ContainsKey("_gdeSchema"))
                    {

                        // Living Armor: Give all of his attacks +30% debuff chance
                        if (e.Key == "SE_Armor_1_T")
                        {
                            List<int> a = new List<int>();
                            a.Add(120);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = a;
                        }

                        if (e.Key == "SE_Armor_2_T")
                        {
                            List<int> a = new List<int>();
                            a.Add(90);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = a;
                        }

                        if (e.Key == "SE_Armor_3_T")
                        {
                            List<int> a = new List<int>();
                            a.Add(105);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = a;
                        }

                        //HP reverted
                        if (e.Key == "S1_ArmorBoss")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 90;
                        }

                        // Cerberus: Gives Roar Healing Reduced Debuff
                        if (e.Key == "SE_MBoss_2_T")
                        {
                            List<string> a = new List<string>();
                            a.Add("B_S4_King_P1_1_T");
                            (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = a;
                        }
                        //HP Reverted
                        if (e.Key == "MBoss_0")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 110;
                        }

                        // Witch: Add an action count, add dark spark to skill list, change speed
                        if (e.Key == "S1_WitchBoss")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["spd"] = 0;

                            //add action count
                            List<int> a = new List<int>();
                            a.Add(100);
                            (masterJson[e.Key] as Dictionary<string, object>)["PlusActCount"] = a;

                            //add dark spark
                            List<string> b = new List<string>();
                            b.Add("S_Witch_0");
                            b.Add("S_Witch_1");
                            b.Add("S_Statue1_1");
                            (masterJson[e.Key] as Dictionary<string, object>)["Skills"] = b;
                            
                            //change witch attack and accuracy
                            (masterJson[e.Key] as Dictionary<string, object>)["atk"] = 5;
                            (masterJson[e.Key] as Dictionary<string, object>)["hit"] = 90;
                        }

                        // Golem: Add Follower Passive
                        if (e.Key == "Boss_Golem")
                        {
                            List<string> a = new List<string>();
                            a.Add("B_Golem_P");
                            a.Add("B_Reaper_3");
                            (masterJson[e.Key] as Dictionary<string, object>)["Passives"] = a;
                        }

                        // Parade Tank: Kaboom! now stuns, Stun chance set to 90%
                        if (e.Key == "SE_MBoss2_0_1_T")
                        {
                            // Stun
                            List<string> a = new List<string>();
                            a.Add("B_Common_Rest");
                            (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = a;

                            // Stun Chance
                            List<int> b = new List<int>();
                            b.Add(90);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = b;
                        }

                        // Parade Tank: Kaboom! gained Tracking
                        if (e.Key == "S_MBoss2_0_1")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Track"] = true;
                        }

                        // TFK Shield Pulse: Damange Doubled, 100% Stun Chance
                        if (e.Key == "SE_S4_King_P2Start_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 66;

                            // Stun
                            List<string> a = new List<string>();
                            a.Add("B_Common_Rest");
                            (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = a;

                            // Stun Chance
                            List<int> b = new List<int>();
                            b.Add(100);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = b;
                        }

                        // Joker: HP increased by 5
                        //if (e.Key == "S2_Joker")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 200;
                        //}

                        // Joker Card: bind
                        if (e.Key == "S_Joker_0")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["NotChuck"] = true;
                        }

                        // Joker Hidden Dagger: Hit rate increased 82 -> 100
                        if (e.Key == "SE_Joker_1_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["HIT"] = 100;
                        }

                        if (e.Key == "B_Joker_1_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["TagPer"] = 100;
                        }

                        //if (e.Key == "S_Joker_2")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Target"] = "enemy";
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Particle"] = "Particle/Enemy/Reaper_3";
                        //}

                        //if (e.Key == "SE_Joker_2_T")
                        //{
                        //    List<string> a = new List<string>();
                        //    a.Add("B_Common_Rest");
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = a;

                        //    List<int> b = new List<int>();
                        //    b.Add(999);
                        //    (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = b;
                        //}

                        //Bomber Clown Fight: Added 3 damage balloons
                        if (e.Key == "Queue_S2_BombClown")
                        {
                            List<string> a = new List<string>();
                            // Current Bug: bomb balloons will not spawn in despair mode and screw up bomber clown's position, this is a temp fix.
                            a.Add("S2_BoomBalloon");
                            a.Add("S2_BoomBalloon");
                            a.Add("S2_BombClownBoss");
                            a.Add("S2_BoomBalloon");
                            (masterJson[e.Key] as Dictionary<string, object>)["Enemys"] = a;
                        }

                        // Bomber Clown: Add 40 HP
                        if (e.Key == "S2_BombClownBoss")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = (long)((masterJson[e.Key] as Dictionary<string, object>)["maxhp"]) + 40;
                        }

                        // Bomb Balloon: Added 20% crit chance
                        if (e.Key == "SE_BoomBalloon_1_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["CRI"] = 20;
                        }

                        // Bomber Clown Time Bomb: Cost increased to 1
                        //if (e.Key == "S_BombClown_B_0")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["UseAp"] = 1;
                        //}

                        //// Death Sentence Damage: 26 -> 9999
                        //if (e.Key == "SE_Boss_Reaper_0_PlusHit_T")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["DMG_Base"] = 9999;
                        //}

                        // Breathtaker and Death Scythe: Discards top skill
                        //if (e.Key == "S_Boss_Reaper_1")
                        //{
                        //    List<string> a = new List<string>();
                        //    a.Add("Extended_Hein_5");
                        //    (masterJson[e.Key] as Dictionary<string, object>)["SkillExtended"] = a;
                        //    //Debug.Log("Updated Breathtaker");
                        //}
                        //if (e.Key == "S_Boss_Reaper_0")
                        //{
                        //    List<string> a = new List<string>();
                        //    a.Add("Extended_Hein_5");
                        //    (masterJson[e.Key] as Dictionary<string, object>)["SkillExtended"] = a;
                        //    Debug.Log("Reloaded");
                        //}
                        // Death Sentence: Applies Healing Reduction
                        if (e.Key == "SE_Boss_Reaper_0_PlusHit_T")
                        {
                            List<string> a = new List<string>();
                            a.Add("B_S4_King_P1_1_T");
                            (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = a;

                            // Bleed Chance
                            List<int> b = new List<int>();
                            b.Add(120);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = b;
                        }

                        //Pharos Leader: Add enemies in beginning
                        if (e.Key == "Queue_S3_PharosLeader")
                        {
                            List<string> a = new List<string>();
                            //a.Add("S3_Wolf");
                            a.Add("S2_PharosWitch");
                            a.Add("S3_Boss_Pope");
                            (masterJson[e.Key] as Dictionary<string, object>)["Enemys"] = a;
                            //Vector3 pos1 = new Vector3();
                            //pos1.x = -7; pos1.y = 0; pos1.z = 0;
                            //Vector3 pos2 = new Vector3();
                            //pos2.x = 7; pos2.y = 0; pos2.z = 0;
                            //Vector3 pos3 = new Vector3();
                            //pos3.x = 0; pos3.y = 0; pos3.z = 0;
                            //List<Vector3> positions = new List<Vector3>();
                            //positions.Add(pos1);
                            //positions.Add(pos2);
                            //positions.Add(pos3);
                            //(masterJson[e.Key] as Dictionary<string, object>)["Pos"] = positions;

                        }

                        //Pharos Leader Rising Dominance: Add Summon
                        if (e.Key == "S_S3_Pope_3")
                        {
                            List<string> a = new List<string>();
                            a.Add("Extended_Azar_2");
                            (masterJson[e.Key] as Dictionary<string, object>)["SkillExtended"] = a;
                        }

                        //Consecration, add all 3 debuffs
                        //if (e.Key == "SE_TheLight_4_T")
                        //{
                        //    List<string> a = new List<string>();
                        //    a.Add("B_Common_Rest");
                        //    a.Add("B_TheLight_1_T");
                        //    a.Add("B_TheLight_2_T");
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = a;

                        //    List<int> b = new List<int>();
                        //    b.Add(999);
                        //    b.Add(999);
                        //    b.Add(999);
                        //    (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = b; 
                        //}

                        // Karaela Armor Debuff: Lifetime and Max Stack + 1
                        if (e.Key == "B_TheLight_1_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["LifeTime"] = 3;
                            (masterJson[e.Key] as Dictionary<string, object>)["MaxStack"] = 3;
                        }

                        // Karaela Burn Debuff: Lifetime and Max Stack +1
                        if (e.Key == "B_TheLight_2_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["LifeTime"] = 4;
                            (masterJson[e.Key] as Dictionary<string, object>)["MaxStack"] = 4;
                        }

                        // Burning Cross: +5HP
                        if (e.Key == "S3_Boss_TheLight_Cross")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 25;
                        }

                        // Dorchi Swords: Cast extended heal
                        if (e.Key == "S1_BossDorchiX_Sword")
                        {
                            List<string> a = new List<string>();
                            a.Add("S_Public_15");
                            (masterJson[e.Key] as Dictionary<string, object>)["Skills"] = a;
                            (masterJson[e.Key] as Dictionary<string, object>)["reg"] = 10;
                        }

                    //SirDorchi: CC and Bleed accuracy +10 %
                        if (e.Key == "SE_DorchiX_1_T")
                        {
                            List<int> a = new List<int>();
                            a.Add(30);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = a;
                        }
                        if (e.Key == "SE_DorchiX_2_T")
                        {
                            List<int> a = new List<int>();
                            a.Add(60);
                            (masterJson[e.Key] as Dictionary<string, object>)["BuffPlusTagPer"] = a;
                        }

                        // Time Eater Minion: gain an action count
                        if (e.Key == "MBoss2_1_0")
                        {
                            List<int> a = new List<int>();
                            a.Add(1);
                            (masterJson[e.Key] as Dictionary<string, object>)["PlusActCount"] = a;
                        }

                        // TFK Black Fog: Moved to Turn 18->12
                        if (e.Key == "Queue_S4_King")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 12;
                        }

                        // TFK 3rd Phase: Bleeding buffed to Phase 1 Bleeding
                        if (e.Key == "SE_S4_King_P2_1_T")
                        {
                            List<string> a = new List<string>();
                            a.Add("B_S4_King_P1_2_T");
                            (masterJson[e.Key] as Dictionary<string, object>)["Buffs"] = a;
                        }

                        //TFK: HP + 100
                        if (e.Key == "S4_King_0")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 1000;

                        }

                        //Godo Revolver Fanning: Damage increased
                        if (e.Key == "SE_Gunmman_2_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 100;
                        }

                        //Impale: Changed to Null
                        if (e.Key == "S_S4_King_LastAttack")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Category"] = "null";
                        }

                        // Final Boss Changes
                        
                        // Full Moon Slash = Tracking
                        //if (e.Key == "S_LBossFirst_2")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Track"] = true;
                        //}
                        //if (e.Key == "S_LBossFirst_2_Plus")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Track"] = true;
                        //}

                        // Storming Blade = Tracking
                        //if (e.Key == "S_LBossFirst_Blast")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Track"] = true;
                        //}

                        // HP Increase
                        //if (e.Key == "LBossFirst")
                        //{
                        //    (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 420;
                        //}

                        // Illusion Wave Base DMG and Card Cutting DMG = increased
                        if (e.Key == "SE_LBossFirst_Cut_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 50;
                        }

                        if (e.Key == "SE_LBossFirst_Cut_Plushit_T")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["DMG_Per"] = 75;
                        }

                        // Destroy Illusion Sword = Single Target
                        if (e.Key == "S_LBossFirst_LucySwordremove")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["Target"] = "enemy";
                            (masterJson[e.Key] as Dictionary<string, object>)["Particle"] = "";
                        }

                        // Veiled Sword = Cast Extended Heal
                        if (e.Key == "LBossFirst_Summons_1")
                        {
                            List<string> a = new List<string>();
                            a.Add("S_Public_15");
                            (masterJson[e.Key] as Dictionary<string, object>)["Skills"] = a;
                            (masterJson[e.Key] as Dictionary<string, object>)["reg"] = 10;
                        }

                        if (e.Key == "TheDealer")
                        {
                            (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 450;
                            (masterJson[e.Key] as Dictionary<string, object>)["atk"] = 16;
                        }

                    }
                }
                dataString = Json.Serialize(masterJson);
            }
        }

        static System.Collections.IEnumerator WindyTurns(B_Joker_P_0 __instance)
        {
            if (BattleSystem.instance.TurnNum == 1)
            {
                yield return BattleText.InstBattleText_Co(__instance.BChar, ScriptLocalization.CharText_Enemy_Joker.Talk_0, false, 0, 0f);
                yield return BattleText.InstBattleText_Co(__instance.BChar, ScriptLocalization.CharText_Enemy_Joker.Talk_1, false, 0, 0f);
                yield return BattleText.InstBattleText_Co(__instance.BChar, ScriptLocalization.CharText_Enemy_Joker.Talk_2, false, 0, 0f);
            }
            else if (BattleSystem.instance.TurnNum == 3)
            {
                yield return BattleText.InstBattleText_Co(__instance.BChar, ScriptLocalization.CharText_Enemy_Joker.Talk_4, false, 0, 0f);
            }
            MasterAudio.PlaySound("Laugh_03", 1f, null, 0f, null, null, false, false);
            List<Skill> temp = new List<Skill>();
            foreach (Skill skill in BattleSystem.instance.AllyTeam.Skills)
            {
                if (skill.MySkill.KeyID != GDEItemKeys.Skill_S_Joker_0)
                {
                    temp.Add(skill);
                }
            }
            BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(temp, new SkillButton.SkillClickDel(__instance.Del), "......", false, true, true, false, true));
            yield break;
        }

        //Joker card spawns in hand instead of deck
        [HarmonyPatch(typeof(B_Joker_P_0))]
        class JokerPatch
        {

            [HarmonyPatch(nameof(B_Joker_P_0.Turn1))]
            [HarmonyPrefix]
            static bool Turn1(B_Joker_P_0 __instance)
            {
                if (BattleSystem.instance.TurnNum % 2 == 1)
                {
                    List<Skill> list = new List<Skill>();
                    foreach (Skill skill in BattleSystem.instance.AllyTeam.Skills_Deck)
                    {
                        if (skill.MySkill.KeyID != GDEItemKeys.Skill_S_Joker_0)
                        {
                            list.Add(skill);
                        }
                    }
                    List<Skill> list2 = new List<Skill>();
                    for (int i = 0; i < 3; i++)
                    {
                        if (list.Count == 0)
                        {
                            break;
                        }
                        Skill item = list.Random<Skill>();
                        list2.Add(item);
                        list.Remove(item);
                    }
                    if (list2.Count != 0)
                    {
                        BattleSystem.DelayInput(WindyTurns(__instance));
                    }
                }
                return false;
            }

            [HarmonyPatch(nameof(B_Joker_P_0.Del))]
            [HarmonyPrefix]
            static bool Prefix2(B_Joker_P_0 __instance, SkillButton Mybutton)
            {
                int index = BattleSystem.instance.AllyTeam.Skills.FindIndex((Skill a) => a == Mybutton.Myskill);
                Skill s = BattleSystem.instance.AllyTeam.Skills[index];
                Skill joker = Skill.TempSkill(GDEItemKeys.Skill_S_Joker_0, BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam);
                BattleSystem.instance.AllyTeam.Skills.Remove(s);
                BattleSystem.instance.AllyTeam.Add(joker, true);
                if (BattleSystem.instance.TurnNum == 1)
                {
                    BattleSystem.DelayInput(BattleText.InstBattleText_Co(__instance.BChar, ScriptLocalization.CharText_Enemy_Joker.Talk_3, false, 0, 0f));
                }
                return false;
            }
        }

        // Joker Summon gains taunt
        [HarmonyPatch(typeof(SkillExtended_Joker_2))]
        class JokerSummon_Patch
        {
            [HarmonyPatch(nameof(SkillExtended_Joker_2.SkillUseSingle))]
            [HarmonyPrefix]
            static bool Prefix()
            {
                int count = BattleSystem.instance.EnemyTeam.AliveChars.Count;
                Debug.Log("Enemy count: " + count);
                if (count <= 2)
                {
                    BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos("S2_Pierrot_Bat2", null));
                }
                else
                {
                    BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos("S2_Ballon", null));
                }
                return false;
            }
        }

        //Joker gains shadow curtain after cuff trick activates
        //[HarmonyPatch(typeof(B_Joker_P_0))]
        //class JokerPatch
        //{
        //    [HarmonyPatch(nameof(B_Joker_P_0.SKillUseHand_Team))]
        //    [HarmonyPrefix]
        //    static bool Prefix(B_Joker_P_0 __instance, Skill skill)
        //    {
        //        if (!skill.Master.IsLucy)
        //        {
        //            BattleChar battleChar = BattleSystem.instance.EnemyList.Find((BattleEnemy a) => a.Info.KeyData == GDEItemKeys.Enemy_S2_Joker);
        //            if (battleChar != null)
        //            {
        //                battleChar.BuffAdd(GDEItemKeys.Buff_B_Joker_P, battleChar, false, 0, false, -1, false);
        //                Buff buff = battleChar.BuffReturn(GDEItemKeys.Buff_B_Joker_P, false);
        //                if (buff != null && buff.StackNum >= 4)
        //                {
        //                    buff.SelfDestroy();
        //                    skill.Master.Damage(battleChar, 8, false, false, true, 100, false, false);
        //                    skill.Master.BuffAdd(GDEItemKeys.Buff_B_Common_Rest, battleChar, false, 150, false, -1, false);
        //                    //if (!__instance.CCFlag)
        //                    //{
        //                    //    __instance.CCFlag = true;
        //                    //    BattleSystem.DelayInput(BattleText.InstBattleText_Co(__instance.BChar, ScriptLocalization.CharText_Enemy_Joker.CC, false, 0, 0f));
        //                    //}

        //                    // Shadow Curtain Buff
        //                    battleChar.BuffAdd(GDEItemKeys.Buff_B_CamouflageCloak, battleChar, false, 0, false, -1, false);
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //}

        ////Bomber Clown summons balloon clown + 3 explosive balloons
        //[HarmonyPatch(typeof(S_BombClown_0))]
        //class BombClownSummonPatch
        //{
        //    [HarmonyPatch(nameof(S_BombClown_0.SkillUseSingle))]
        //    [HarmonyPostfix]
        //    static void Postfix()
        //    {
        //        BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S2_BoomBalloon));
        //        BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S2_BoomBalloon));
        //    }
        //}

        ////Bomber Clown AI: summon regardless of enemy count
        //[HarmonyPatch(typeof(AI_BomeClown))]
        //class BombClownAIPatch
        //{
        //    [HarmonyPatch(nameof(AI_BomeClown.SkillSelect))]
        //    [HarmonyPrefix]
        //    static bool Prefix(AI_BomeClown __instance, int ActionCount, ref Skill __result)
        //    {
        //        if (ActionCount == 0)
        //        {
        //            if (BattleSystem.instance.TurnNum == 1 || BattleSystem.instance.TurnNum == 4 || BattleSystem.instance.TurnNum == 7)
        //            {
        //                __result = __instance.BChar.Skills[1]; //summon
        //            }
        //            else
        //            {
        //                __result = __instance.BChar.Skills[2]; //burn debuff
        //            }
        //        }

        //        else
        //        {
        //            if (ActionCount == 1 || ActionCount == 2)
        //            {
        //                __result = __instance.BChar.Skills[0]; //bombs away
        //            }
        //            else
        //            {
        //                __result = __instance.SkillSelect(ActionCount);
        //            }
        //        }

        //        return false;
        //    }
        //}

        // Shiranui: Dance of Flame now lowers stats
        [HarmonyPatch(typeof(B_Shiranui_3_T))]
        class FlameDancePatch
        {
            [HarmonyPatch(nameof(B_Shiranui_3_T.Init))]
            [HarmonyPostfix]
            static void Postfix(B_Shiranui_3_T __instance)
            {
                __instance.PlusPerStat.Heal = -20;
            }
        }

        // Void Summon: Void Bursts immediately
        //[HarmonyPatch(typeof(AI_ProgramMaster_0))]
        //class VoidPatch
        //{
        //    [HarmonyPatch(nameof(AI_ProgramMaster_0.SkillSelect))]
        //    [HarmonyPrefix]
        //    static bool Prefix(AI_ProgramMaster_0 __instance, ref Skill __result)
        //    {
        //        __result = __instance.BChar.Skills[1];
        //        return false;
        //    }
        //}

        //Gambler: cannot use gamble battle twice in the same turn
       [HarmonyPatch(typeof(AI_TheDealer))]
        class TheDealerPatch
        {
            [HarmonyPatch(nameof(AI_TheDealer.SkillSelect))]
            [HarmonyPostfix]
            static void Postfix(int ActionCount, AI_ProgramMaster_0 __instance, ref Skill __result)
            {
                List<Skill> skillList = new List<Skill> { __instance.BChar.Skills[1], __instance.BChar.Skills[2], __instance.BChar.Skills[3] };
                if (BattleSystem.instance.TurnNum == 1)
                {
                    //if (ActionCount > 1)
                    //{
                    //    __result = skillList.Random();
                    //}
                }
                else
                {
                    if (ActionCount > 3)
                    {
                        __result = null;
                    }
                }
            }
        }

        //Fast Forward: summons a cursed watch
        [HarmonyPatch(typeof(SkillExtended_MBoss2_1_5))]
        class FastForwardPatch
        {
            [HarmonyPatch(nameof(SkillExtended_MBoss2_1_5.SkillUseSingle))]
            [HarmonyPostfix]
            static void Postfix()
            {
                Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_LucyCurse_CursedClock, BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam);
                skill.isExcept = true;
                BattleSystem.instance.AllyTeam.Add(skill, true);
            }
        }

        //Ruby & Sapphire: Alternates Taunt Status
        [HarmonyPatch(typeof(AI_S2_MainBoss_1_0))]
        class RubyPatch
        {
            [HarmonyPatch(nameof(AI_S2_MainBoss_1_0.SkillSelect))]
            [HarmonyPostfix]
            static void Postfix(AI_S2_MainBoss_1_0 __instance)
            {
                bool afterBOH = (float)__instance.BChar.HP <= Misc.PerToNum((float)__instance.BChar.GetStat.maxhp, 66f) && __instance.PT != 3;

                // Remove Taunt/Shield on odd turns
                if (BattleSystem.instance.TurnNum % 2 == 1)
                {
                    if (__instance.BChar.BuffFind(GDEItemKeys.Buff_B_EnemyTaunt, false))
                    {
                        __instance.BChar.BuffReturn(GDEItemKeys.Buff_B_EnemyTaunt, false).SelfDestroy();
                    }

                    if (afterBOH && __instance.BChar.BuffFind(GDEItemKeys.Buff_B_S2_MainBoss_1_0, false))
                    {
                        __instance.BChar.BuffReturn(GDEItemKeys.Buff_B_S2_MainBoss_1_0, false).SelfDestroy();
                    }

                }
                // Taunt/Shield on even turns
                else
                {
                    __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_EnemyTaunt, __instance.BChar, false, 0, false, -1, false);

                    if (afterBOH)
                    {
                        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_S2_MainBoss_1_0, __instance.BChar, false, 0, false, -1, false);
                    }
                }
            }
        }

        //Ruby & Sapphire: Alternates Taunt Status
        [HarmonyPatch(typeof(AI_S2_MainBoss_1_1))]
        class SapphirePatch
        {
            [HarmonyPatch(nameof(AI_S2_MainBoss_1_1.SkillSelect))]
            [HarmonyPrefix]
            static bool Prefix(AI_S2_MainBoss_1_1 __instance, ref Skill __result)
            {

                // Sapphire casts BOH
                if (__instance.LeftAI.PT == 3)
                {
                    Skill a = Skill.TempSkill(GDEItemKeys.Skill_S_S2_Mainboss_1_Left_3, __instance.BChar, __instance.BChar.MyTeam);
                    a.ExtendedDelete("Extended_S2_MainBoss_1_Left_3");
                    a.ExtendedDelete();
                    __result = a;
                }
                else
                {
                    __result = __instance.BChar.Skills[__instance.LeftAI.PT];
                }

                bool afterBOH = (float)__instance.BChar.HP <= Misc.PerToNum((float)__instance.BChar.GetStat.maxhp, 66f) && __instance.LeftAI.PT != 3;

                // Remove Taunt/Shield on even turns
                if (BattleSystem.instance.TurnNum % 2 == 0)
                {
                    if (__instance.BChar.BuffFind(GDEItemKeys.Buff_B_EnemyTaunt, false))
                    {
                        __instance.BChar.BuffReturn(GDEItemKeys.Buff_B_EnemyTaunt, false).SelfDestroy();
                    }

                    if (afterBOH && __instance.BChar.BuffFind(GDEItemKeys.Buff_B_S2_MainBoss_1_0, false))
                    {
                        __instance.BChar.BuffReturn(GDEItemKeys.Buff_B_S2_MainBoss_1_0, false).SelfDestroy();
                    }

                }
                // Taunt/Shield on odd turns
                else
                {
                    __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_EnemyTaunt, __instance.BChar, false, 0, false, -1, false);

                    if (afterBOH)
                    {
                        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_S2_MainBoss_1_0, __instance.BChar, false, 0, false, -1, false);
                    }
                }

                return false;
            }
            [HarmonyPatch(nameof(AI_S2_MainBoss_1_1.SpeedChange))]
            [HarmonyPrefix]
            static bool Prefix2(AI_S2_MainBoss_1_1 __instance, ref int __result, Skill skill, int ActionCount, int OriginSpeed)
            {
                //If BOH Turn
                if (__instance.LeftAI.PT == 3)
                {
                    //Action in 1 count
                    __result = 1;
                }
                else
                {
                    __result = OriginSpeed;
                }
                return false;
            }
        }

            //BOH Counter shield goes to whoever is currently taunting instead of random
            [HarmonyPatch(typeof(Extended_S2_MainBoss_1_Left_3))]
            class CounterShieldPatch
            {
                [HarmonyPatch(nameof(Extended_S2_MainBoss_1_Left_3.SkillUseSingle))]
                [HarmonyPrefix]
                static bool Prefix(Extended_S2_MainBoss_1_Left_3 __instance)
                {
                    Skill skill = null;
                    BattleEnemy battleEnemy = new BattleEnemy();
                    foreach (BattleEnemy battleEnemy2 in BattleSystem.instance.EnemyList)
                    {
                        if (battleEnemy2.Info.KeyData == GDEItemKeys.Enemy_S2_MainBoss_1_1)
                        {
                            battleEnemy = battleEnemy2;
                            skill = Skill.TempSkill(GDEItemKeys.Skill_S_S2_Mainboss_1_Right_3, battleEnemy2, battleEnemy2.MyTeam);
                            break;
                        }
                    }
                    if (skill != null)
                    {
                        foreach (Skill_Extended skill_Extended in skill.AllExtendeds)
                        {
                            if (skill_Extended.Special_Target() != null)
                            {
                                battleEnemy.SaveTarget = skill_Extended.Special_Target();
                            }
                        }
                        battleEnemy.SelectedSkill = skill;

                        // Odd Turn: Give to Sapphire
                        if (BattleSystem.instance.TurnNum % 2 == 1)
                        {
                            BattleSystem.instance.EnemyList[1].BuffAdd(GDEItemKeys.Buff_B_S2_MainBoss_1_0, __instance.BChar, false, 0, false, -1, false);

                        }
                        // Even Turn: Give to Ruby
                        else
                        {
                            BattleSystem.instance.EnemyList[0].BuffAdd(GDEItemKeys.Buff_B_S2_MainBoss_1_0, __instance.BChar, false, 0, false, -1, false);

                        }
                        battleEnemy.UseSkill();
                    }

                    return false;
                }

            // Removes sapphire stigma from Ruby's BOH
            [HarmonyPatch(nameof(Extended_S2_MainBoss_1_Left_3.AttackEffectSingle))]
            [HarmonyPostfix]
            static bool Prefix(Extended_S2_MainBoss_1_Left_3 __instance, BattleChar hit, SkillParticle SP, int DMG, int Heal)
            {
                return false;
            }

        }

            //Remove RS counter dialogue, game keeps showing it to you when alternating counter status and it got annoying very quickly
            [HarmonyPatch(typeof(B_S2_MainBoss_1_0))]
            class CounterTextPatch
            {
                [HarmonyPatch(nameof(B_S2_MainBoss_1_0.Awake))]
                [HarmonyPrefix]
                static bool Prefix(B_S2_MainBoss_1_0 __instance)
                {
                    return false;
                }
            }

        //Reaper Summon: Applies Dark Sanctuary to self
        //[HarmonyPatch(typeof(SkillExtended_Reaper_3))]
        //class ReaperSummonPatch
        //{
        //    [HarmonyPatch(nameof(SkillExtended_Reaper_3.SkillUseSingle))]
        //    [HarmonyPostfix]
        //    static void Postfix(SkillExtended_Reaper_3 __instance)
        //    {
        //        bool lowHP = (float)__instance.BChar.HP <= Misc.PerToNum((float)__instance.BChar.GetStat.maxhp, 85f);
        //        if (lowHP)
        //        {
        //            __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_ShadowPriest_13_T, __instance.BChar, false, 0, false, -1, false);
        //        }
        //    }
        //}

        ////Pharos Leader: followers  gain follower buff
        [HarmonyPatch(typeof(Skill_Extended_B_S3_Boss_Pope_P_0))]
        class PharosLeaderSummonPatch
        {
            [HarmonyPatch(nameof(Skill_Extended_B_S3_Boss_Pope_P_0.FixedUpdate))]
            [HarmonyPostfix]
            static void Postfix(Skill_Extended_B_S3_Boss_Pope_P_0 __instance)
            {
                foreach (BattleChar battleChar in BattleSystem.instance.EnemyTeam.AliveChars)
                {
                    if (!(battleChar as BattleEnemy).Boss && !battleChar.BuffFind(GDEItemKeys.Buff_B_MBoss2_1_0_P, false))
                    {
                        battleChar.BuffAdd(GDEItemKeys.Buff_B_MBoss2_1_0_P, __instance.BChar, false, 0, false, -1, false);
                    }
                }
            }
        }

        // Reaper follower buff: remove health negative, starts at 0%
        [HarmonyPatch(typeof(B_Reaper_3))]
        class ReaperBuffPatch
        {
            [HarmonyPatch(nameof(B_Reaper_3.Init))]
            [HarmonyPostfix]
            static void Postfix(B_Reaper_3 __instance)
            {
                //__instance.PlusPerStat.MaxHP = 0;
                __instance.PlusPerStat.Damage = (__instance.StackNum - 1) * 15;
            }
        }

        // Strange Idea
        [HarmonyPatch(typeof(B_S3_Pope_1_T))]
        class StrangeIdeaPatch
        {
            [HarmonyPatch(nameof(B_S3_Pope_1_T.FixedUpdate))]
            [HarmonyPostfix]
            static void Postfix(B_S3_Pope_1_T __instance)
            {
                int num = 0;
                foreach (Skill skill in BattleSystem.instance.AllyTeam.Skills)
                {
                    if (skill.Master == __instance.BChar)
                    {
                        num++;
                    }
                }
                __instance.PlusDamageTick = num * 6;
            }
        }

        public static int reaperCount = 0;

        [HarmonyPatch(typeof(Buff))]
        class ReaperStart
        {
            [HarmonyPatch(nameof(Buff.Init))]
            [HarmonyPostfix]
            static void Postfix(Buff __instance)
            {
                if (__instance is B_Enemy_Boss_Reaper_P)
                {
                    reaperCount = 0;
                    Debug.Log("Buff Init Reaper");
                }
            }
        }

        // Reaper AI : Change summon types
        [HarmonyPatch(typeof(SkillExtended_Reaper_3))]
        class ReaperSummonPatch
        {
            [HarmonyPatch(nameof(SkillExtended_Reaper_3.SkillUseSingle))]
            [HarmonyPrefix]
            static bool Prefix(SkillExtended_Reaper_3 __instance)
            {
                switch (reaperCount % 3)
                {
                    case 0:
                        BattleSystem.instance.StartCoroutine(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S3_Fugitive, null));
                        if (__instance.BChar is BattleEnemy && (__instance.BChar as BattleEnemy).istaunt)
                        {
                            __instance.BChar.BuffScriptReturn("Common_Buff_EnemyTaunt").SelfDestroy(false);
                        }
                        reaperCount++;
                        break;
                    case 1:
                        BattleSystem.instance.StartCoroutine(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S3_Deathbringer, null));
                        if (__instance.BChar is BattleEnemy && (__instance.BChar as BattleEnemy).istaunt)
                        {
                            __instance.BChar.BuffScriptReturn("Common_Buff_EnemyTaunt").SelfDestroy(false);
                        }
                        reaperCount++;
                        break;
                    case 2:
                        BattleSystem.instance.StartCoroutine(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S3_Pharos_HighPriest, null));
                        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_EnemyTaunt, __instance.BChar, false, 0, false, -1, false);
                        reaperCount++;
                        break;
                    default:
                        return false;
                }
                return false;
            }
        }

        //[HarmonyPatch(typeof(Buff))]
        //class StrangeIdeaDesc_Patch
        //{

        //    [HarmonyPatch(nameof(Buff.DescExtended), new Type[] { })]
        //    [HarmonyPostfix]
        //    static void DescExtendedPostfix(ref string __result, Buff __instance)
        //    {
        //        if (__instance is B_S3_Pope_1_T)
        //        {
        //            __result = "Take <color=purple>6 Pain damage</color> for each skill this character has in hand.";
        //        }
        //    }
        //}

        //Joker Card: Increased Damage
        [HarmonyPatch(typeof(SKillExtedned_Joker_0_Effect))]
        class JokerCardPatch
        {
            [HarmonyPatch(nameof(SKillExtedned_Joker_0_Effect.ParticleOut_Before))]
            [HarmonyPrefix]
            static bool Prefix(SKillExtedned_Joker_0_Effect __instance, List<BattleChar> Targets)
            {

                int Dmg = 0;
                if (Targets[0].BuffFind(GDEItemKeys.Buff_B_Common_Rest, false))
                {
                    Dmg += 15;
                }
                if (Targets[0].BuffFind(GDEItemKeys.Buff_B_Joker_1_T, false))
                {
                    Dmg += 15;
                }
                __instance.SkillBasePlus.Target_BaseDMG = Dmg;
                if (Dmg != 0)
                {
                    __instance.IsDamage = true;
                }

                return false;
            }
        }

        ////Change Joker Card Desc
        //[HarmonyPatch(typeof(Skill_Extended))]
        //class JokerDesc_Patch
        //{
        //    [HarmonyPatch(nameof(Skill_Extended.DescExtended))]
        //    [HarmonyPostfix]
        //    static void DescExtendedPostfix(ref string __result, Skill_Extended __instance)
        //    {
        //        if (__instance is SkillExtended_Joker_0)
        //        {
        //            __result = "Draw a skill from the deck.\nWhen cast or discarded, all allies take 15 damage for each <b>Bleeding or Stunned</b> debuff. \n<color=#919191>How did this get here...</color>";
        //        }
        //    }
        //}//

        //Unused Skill Extended: Reaper Discard death sentence marked skill
        //[HarmonyPatch(typeof(Extended_Hein_5))]
        //class DiscardTopPatch
        //{
        //    [HarmonyPatch(nameof(Extended_Hein_5.SkillUseSingle))]
        //    [HarmonyPrefix]
        //    static bool Prefix(Extended_Hein_5 __instance, Skill SkillD, List<BattleChar> Targets)
        //    {

        //            foreach (Skill s in BattleSystem.instance.AllyTeam.Skills)
        //            {
        //                //Debug.Log("Here");
        //                if (s.MySkill.User != "LucyDraw" && s.MySkill.User != "Lucy")
        //                {
        //                //Debug.Log("Not Lucy");
        //                    s.Delete(false);
        //                    break;
        //                }
        //            }
        //        return false;
        //    }
        //}

        //Karaela Armor Debuff: -15% debuff resist power
        [HarmonyPatch(typeof(TheLight_1_T))]
        class KaraelaArmorDebuff
        {
            [HarmonyPatch(nameof(TheLight_1_T.Init))]
            [HarmonyPostfix]
            static void Postfix(TheLight_1_T __instance)
            {
                __instance.PlusStat.RES_CC = (float)(-15 * __instance.StackNum);
                __instance.PlusStat.RES_DEBUFF = (float)(-15 * __instance.StackNum);
                __instance.PlusStat.RES_DOT = (float)(-15 * __instance.StackNum);
            }
        }

        //Karaela Burn Debuff: -15% debuff resist power
        //[HarmonyPatch(typeof(TheLight_2_T))]
        //class KaraelaBurnDebuff
        //{
        //    [HarmonyPatch(nameof(TheLight_2_T.Init))]
        //    [HarmonyPostfix]
        //    static void Postfix(TheLight_2_T __instance)
        //    {
        //        __instance.PlusStat.RES_CC = (float)(-15 * __instance.StackNum);
        //        __instance.PlusStat.RES_DEBUFF = (float)(-15 * __instance.StackNum);
        //        __instance.PlusStat.RES_DOT = (float)(-15 * __instance.StackNum);

        //        __instance.PlusStat.crihit = 0;
        //    }
        //}

        //Consecration: Deals 120% of Max HP
        [HarmonyPatch(typeof(SkillExtended_TheLight_4))]
        class ConsecrationDebuff
        {
            [HarmonyPatch(nameof(SkillExtended_TheLight_4.DamageChange))]
            [HarmonyPrefix]
            static bool Prefix(SkillExtended_TheLight_4 __instance, BattleChar Target, ref int __result)
            {
                Debug.Log(Target.Info.G_get_stat.maxhp);
                 __result = (int)(Target.Info.G_get_stat.maxhp * 1.2);
                 return false;
            }
        }

        //Unused Skill Extend: Summon Pharos Mage 
        [HarmonyPatch(typeof(Extended_Azar_2))]
        class MageSummonPatch
        {
            [HarmonyPatch(nameof(Extended_Azar_2.SkillUseSingleAfter))]
            [HarmonyPrefix]
            static bool Prefix(Extended_Azar_2 __instance)
            {
                BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S2_PharosWitch));
                //BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S3_Wolf));
                //BattleSystem.DelayInput(BattleSystem.instance.NewEnemyAutoPos(GDEItemKeys.Enemy_S3_Wolf));
                
                
                //BattleSystem.instance.CreatEnemy(GDEItemKeys.Enemy_S3_Wolf, new Vector3(5f, 0f, 0f), true, false);
                //BattleSystem.instance.CreatEnemy(GDEItemKeys.Enemy_S3_Wolf, new Vector3(-5f, 0f, -0.5f), true, false);
                BattleSystem.instance.EnemyTeam.UpdateEnemyList();
                return false;
            }
        }

        // Sir Dorchi: gain Berserk and Speed+1 in phase 2
        [HarmonyPatch(typeof(P_DorchiX))]
        class DorchiXPatch
        {
            [HarmonyPatch(nameof(P_DorchiX.HPChange))]
            [HarmonyPrefix]
            static bool Prefix(P_DorchiX __instance)
            {
                if (!__instance.Phase)
                {
                    if (__instance.BChar.HP <= 1)
                    {
                        __instance.BChar.Info.Hp = 1;
                        __instance.Phase = true;
                        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_DorchiX_Barrier, __instance.BChar, false, 0, false, -1, false);
                        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_Witch_1_S_T, __instance.BChar, false, 0, false, -1, false);
                        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_BoomBalloon_P_0, __instance.BChar, false, 0, false, -1, false);
                        ((__instance.BChar as BattleEnemy).MyComponent as C_DorchiX).Battle3.Activate();
                        BattleSystem.DelayInput(__instance.DorchiX3After());
                    }
                }
                return true;
            }
        }

       //TFK: Phase 2 starts at 500HP
       [HarmonyPatch(typeof(P_King))]
        class TFKPatch
        {
            [HarmonyPatch(nameof(P_King.HPChange))]
            [HarmonyPrefix]
            static bool Prefix(P_King __instance)
            {
                    if (__instance.MainAI.Phase == 1 && __instance.BChar.HP <= __instance.BChar.GetStat.maxhp/2)
                    {
                        __instance.BChar.Info.Hp = __instance.BChar.GetStat.maxhp / 2;
                        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_S4_King_P1_Half, __instance.BChar, false, 0, false, -1, false);
                        if (__instance.MainAI.Phase == 1)
                        {
                            (__instance.BChar as BattleEnemy).ChangeSprite(((__instance.BChar as BattleEnemy).MyComponent as C_King).Phase_1_2);
                            __instance.BChar.UI.CharShake.ShakeEndbled(50f, 20f, 60);
                        }
                    }
                return true;
            }
        }

        // This code doesnt work
        // TFK: Shackle stuns 2 people
        //[HarmonyPatch(typeof(S_S4_King_P1_0))]
        //class TFKPatch
        //{
        //    [HarmonyPatch(nameof(S_S4_King_P1_0.SkillUseSingle))]
        //    [HarmonyPrefix]
        //    static bool Prefix(S_S4_King_P1_0 __instance)
        //    {
        //        if (BattleSystem.instance.AllyTeam.AliveChars.Count <= 2)
        //        {
        //            List<Skill> list = new List<Skill>();
        //            List<BattleAlly> list2 = new List<BattleAlly>();
        //            list2.AddRange(BattleSystem.instance.AllyList.Random(2));
        //            foreach (BattleAlly battleAlly in list2)
        //            {
        //                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_S4_King_P1_0_0, battleAlly, battleAlly.MyTeam));
        //            }
        //            if (list.Count != 0)
        //            {
        //                BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(__instance.Del), ScriptLocalization.System_SkillSelect.King_0, false, false, true, false, true));
        //            }
        //        }
        //        else
        //        {
        //            List<Skill> list = new List<Skill>();
        //            List<BattleAlly> list2 = new List<BattleAlly>();
        //            list2.AddRange(BattleSystem.instance.AllyList.Random(3));
        //            foreach (BattleAlly battleAlly in list2)
        //            {
        //                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_S4_King_P1_0_0, battleAlly, battleAlly.MyTeam));
        //            }
        //            if (list.Count != 0)
        //            {
        //                BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(__instance.Del), ScriptLocalization.System_SkillSelect.King_0, false, false, true, false, true));
        //            }

        //            //something here that removes stunned guy from list1

        //            if (list.Count != 0)
        //            {
        //                BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(__instance.Del), ScriptLocalization.System_SkillSelect.King_0, false, false, true, false, true));
        //            }
        //        }
        //        return false;
        //    }
        //}

        //TFK Shackle: Starts at 2 stacks
        [HarmonyPatch(typeof(S_S4_King_P1_0))]
        class TFKShacklePatch
        {
            [HarmonyPatch(nameof(S_S4_King_P1_0.Del))]
            [HarmonyPostfix]
            static void Postfix(S_S4_King_P1_0 __instance, SkillButton Mybutton)
            {
                Mybutton.Myskill.Master.BuffAdd(GDEItemKeys.Buff_B_S4_King_minion_0_0_T, __instance.BChar, false, 0, false, -1, false);
            }
        }
        // (This one is the shackle from phase 2)
        [HarmonyPatch(typeof(S_S4_King_P2Start))]
        class TFKPatch2
        {
            [HarmonyPatch(nameof(S_S4_King_P2Start.Del))]
            [HarmonyPostfix]
            static void Postfix(S_S4_King_P2Start __instance, SkillButton Mybutton)
            {
                Mybutton.Myskill.Master.BuffAdd(GDEItemKeys.Buff_B_S4_King_minion_0_0_T, __instance.BChar, false, 0, false, -1, false);
            }
        }

        // Shackle: Receiving Crit Chance +100%, Debuff Resist -100%, Aggro Greatly Increased
        [HarmonyPatch(typeof(B_S4_King_minion_0_0_T))]
        class ShacklePatch
        {
            [HarmonyPatch(nameof(B_S4_King_minion_0_0_T.Init))]
            [HarmonyPostfix]
            static void Postfix(B_S4_King_minion_0_0_T __instance)
            {
                __instance.PlusStat.AggroPer = 80;
                __instance.PlusStat.crihit = 100;
            }
        }

        // TFK: Impale someone at the beginning of 3rd phase
        [HarmonyPatch(typeof(S_S4_King_P2_1))]
        class ImpalePatch
        {

            [HarmonyPatch(nameof(S_S4_King_P2_1.SkillUseSingle))]
            [HarmonyPrefix]

            static bool Prefix(S_S4_King_P2_1 __instance)
            {

                void Del(SkillButton Mybutton)
                {
                    Mybutton.CharData.Dead(false);
                    Debug.Log("Dead");
                    Debug.Log("Stage: " + StageSystem.instance.Map.StageData.Key);
                    if (StageSystem.instance.Map.StageData.Key != GDEItemKeys.Stage_Stage_Crimson)
                    {
                        BattleSystem.instance.AllyTeam.AP--;
                    }
                }

                if (!__instance.BChar.BuffFind(GDEItemKeys.Buff_B_S4_King_P2_P, false))
                {
                    if(BattleSystem.instance.AllyTeam.AliveChars.Count > 2 && !(__instance.BChar.HP<100))
                    {
                        List<BattleChar> list = new List<BattleChar>();
                        list.AddRange(BattleSystem.instance.AllyTeam.AliveChars);
                        List<Skill> list2 = new List<Skill>();
                        Skill s = new Skill();
                        foreach (BattleChar battleChar in list)
                        {
                            s = Skill.TempSkill(GDEItemKeys.Skill_S_S4_King_LastAttack, battleChar, battleChar.MyTeam);
                            list2.Add(s);
                        }
                        BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list2, new SkillButton.SkillClickDel(Del), string.Empty, false, false, true, false, true));
                    }
                    __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_S4_King_P2_P, __instance.BChar, false, 0, false, -1, false);
                    __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_Armor_P_1, __instance.BChar, false, 0, false, -1, false);
                    (__instance.BChar as BattleEnemy).ChangeSprite(((__instance.BChar as BattleEnemy).MyComponent as C_King).Phase_2);
                }
                    return false;
            }
        }

        //// Sir Dorchi: summon berserked grass hedgehogs
        //[HarmonyPatch(typeof(S_DorchiX_0))]
        //class DorchiXSummonPatch
        //{
        //    [HarmonyPatch(nameof(S_DorchiX_0.SkillUseSingle))]
        //    [HarmonyPostfix]
        //    static void Postfix(S_DorchiX_0 __instance)
        //    {

        //        Debug.Log("Dochis added");
        //        BattleSystem.instance.CreatEnemy(GDEItemKeys.Enemy_S1_Dochi_L, new Vector3(-4.8f, 0f, 2.64f), true, false);
        //        BattleSystem.instance.CreatEnemy(GDEItemKeys.Enemy_S1_Dochi_L, new Vector3(4.8f, 0f, 2.64f), true, false);
        //    }
        //}

        //Sir Dorchi: final move leaf slash damage increased
        [HarmonyPatch(typeof(S_DorchiX_0))]
        class DorchiXSummonPatch
        {
            [HarmonyPatch(nameof(S_DorchiX_0.AttackEffectSingle))]
            [HarmonyPrefix]
            static bool Prefix(S_DorchiX_0 __instance, BattleChar hit)
            {
                hit.Damage(__instance.BChar, (int)((float)hit.GetStat.maxhp * 0.99f), false, true, false, 100, false, false, false);
                return false;
            }
        }

        ////Duelist Godo : Revolver Pannning draws skills
        //[HarmonyPatch(typeof(AI_Gunman))]
        //class GodoPatch
        //{
        //    [HarmonyPatch(nameof(AI_Gunman.SkillSelect))]
        //    [HarmonyPostfix]
        //    static void Postfix(AI_Gunman __instance, int ActionCount)
        //    {
        //        if (__instance.Phase == 2 && ActionCount == 0)
        //        {
        //            int num = 10 - BattleSystem.instance.AllyTeam.Skills.Count;

        //            BattleSystem.instance.AllyTeam.Draw(num);
        //            Debug.Log("Draw!!!!");
        //        }
        //    }
        //}

        ////Duelist Godo Double Tap : discard and draw 1 skill
        //[HarmonyPatch(typeof(S_Gunman_1))]
        //class DoubleTapPatch
        //{
        //    [HarmonyPatch(nameof(S_Gunman_1.SkillUseSingle))]
        //    [HarmonyPostfix]
        //    static void Postfix(S_Gunman_1 __instance, List<BattleChar> Targets)
        //    {

        //        if (Targets[0].MyTeam.Skills.Count >= 1)
        //        {
        //            Skill skill = Targets[0].MyTeam.Skills[0];
        //            skill.Delete(false);
        //        }

        //        BattleSystem.instance.AllyTeam.Draw(1);
        //    }
        //}

        ////Change Double Tap Desc
        //[HarmonyPatch(typeof(Skill_Extended))]
        //class ImitateDesc_Patch
        //{
        //    [HarmonyPatch(nameof(Skill_Extended.DescExtended))]
        //    [HarmonyPostfix]
        //    static void DescExtendedPostfix(ref string __result, Skill_Extended __instance)
        //    {
        //        if (__instance is S_Gunman_1)
        //        {
        //            __result = "If the target is not at death's door, attack the target again. Discard the top skill in your hand and draw 1.";
        //        }
        //    }
        //}

        ////Change Impale Desc
        //[HarmonyPatch(typeof(Skill_Extended))]
        //class LastAttack_Patch
        //{
        //    [HarmonyPatch(nameof(Skill_Extended.DescExtended))]
        //    [HarmonyPostfix]
        //    static void DescExtendedPostfix(ref string __result, Skill_Extended __instance)
        //    {
        //        if (__instance is S4_King_LastAttack)
        //        {
        //            __result = "<color=red>Execute the target.</color>";
        //        }
        //    }
        //}

        //Godo Speed Change : Change to Action Count 1->2
        //[HarmonyPatch(typeof(AI_Gunman))]
        //class SpeedGunman
        //{
        //    [HarmonyPatch(nameof(AI_Gunman.SpeedChange))]
        //    [HarmonyPrefix]
        //    static bool Prefix(Skill skill, int ActionCount, int OriginSpeed, ref int __result)
        //    {
        //        if (skill.MySkill.KeyID == GDEItemKeys.Skill_S_Gunman_2)
        //        {
        //            __result = 2;
        //            Debug.Log("Speed Changed");
        //        }
        //        else
        //        {
        //            __result = OriginSpeed;
        //            Debug.Log("Origin Speed");
        //        }
        //        return false;
        //    }
        //}

        //Duelist Godo Ultimate : attack 2 times
        //[HarmonyPatch(typeof(S_Gunmman_2))]
        //class GodoUltimatePatch
        //{
        //    [HarmonyPatch(nameof(S_Gunmman_2.SkillUseSingle))]
        //    [HarmonyPrefix]
        //    static bool Prefix(S_Gunmman_2 __instance, Skill SkillD, List<BattleChar> Targets)
        //    {
        //        ((__instance.BChar as BattleEnemy).Ai as AI_Gunman).Phase = 3;
        //        __instance.BChar.BuffRemove(GDEItemKeys.Buff_B_GunmanBoss_Phase2, true);
        //        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_SR_GunManBoss_P2, __instance.BChar, false, 0, false, -1, false);
        //        List<BattleChar> list = new List<BattleChar>();
        //        for (int i = 0; i < BattleSystem.instance.AllyTeam.Skills.Count; i++)
        //        {
        //            if (!BattleSystem.instance.AllyTeam.Skills[i].Master.IsLucy)
        //            {
        //                list.Add(BattleSystem.instance.AllyTeam.Skills[i].Master);
        //            }
        //        }

        //        // Add 2 bullets
        //        BattleSystem.DelayInput(__instance.Whisles());
        //        for (int j = 0; j < list.Count; j++)
        //        {
        //            if (list.Count == 1)
        //            {
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], true, true));
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], true, true));
        //                Debug.Log("Double attack");
        //            }
        //            else if (j == list.Count - 1)
        //            {
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], true, false));
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], true, true));
        //            }
        //            else if (j == 0)
        //            {
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], false, true));
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], true, true));
        //            }
        //            else
        //            {
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], false, false));
        //                BattleSystem.DelayInput(__instance.PlusAttack(list[j], true, true));
        //            }
        //        }
        //        BattleSystem.DelayInputAfter(__instance.BackgroundOff());

        //        return false;
        //    }
        //}

        // Final Boss

        // Destroy Illusion Sword: single target
        [HarmonyPatch(typeof(S_LBossFirst_LucySwordremove))]
        class IllusionDestroy_Patch
        {
            [HarmonyPatch(nameof(S_LBossFirst_LucySwordremove.SkillUseSingle))]
            [HarmonyPrefix]
            static bool Prefix(Skill SkillD, List<BattleChar> Targets)
            {
                ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Particle/LastBoss/Lboss_Lucy_SwordBreakP"), new Vector3(0f, 0f, 15f), Quaternion.identity)).GetComponent<SkillParticle>().init(null, BattleSystem.instance.AllyTeam.LucyAlly, new List<BattleChar>());
                BattleSystem.instance.GetBattleValue<BV_LBossFirst_LucySwordremove>().UseNum--;
                Targets[0].Dead(false);
                if (BattleSystem.instance.GetBattleValue<BV_LBossFirst_LucySwordremove>().UseNum <= 0)
                {
                    SkillD.Except();
                }
                return false;
            }
        }

        // Program Master

        [HarmonyPatch(typeof(P_ProgramMaster_LucyAttack))]
        class Stance_patch
        {
            [HarmonyPatch(nameof(P_ProgramMaster_LucyAttack.Init))]
            [HarmonyPostfix]
            static void Postfix(P_ProgramMaster_LucyAttack __instance)
            {
                __instance.HP = 150;
            }

            [HarmonyPatch(nameof(P_ProgramMaster_LucyAttack.Turn))]
            [HarmonyPostfix]
            static void Postfix2(P_ProgramMaster_LucyAttack __instance)
            {
                __instance.HP = 150;
            }
        }

        //// Fantasy = Discard all and draw equal to amount
        //[HarmonyPatch(typeof(S_LBossFirst_4))]
        //class Fantasy_Patch
        //{
        //    [HarmonyPatch(nameof(S_LBossFirst_4.SkillUseSingle))]
        //    [HarmonyPrefix]
        //    static bool Prefix()
        //    {
        //        int discarded = 0;
        //        while (BattleSystem.instance.AllyTeam.Skills.Count > 0)
        //        {
        //            if (BattleSystem.instance.AllyTeam.Skills[0].ExtendedFind(GDEItemKeys.SkillExtended_LBossFirst_Sword, true) != null)
        //            {
        //                BattleSystem.instance.AllyTeam.Skills[0].Except();
        //            }
        //            else 
        //            {
        //                BattleSystem.instance.AllyTeam.Skills[0].Delete();
        //                discarded++;
        //            }
        //        }
        //        BattleSystem.instance.AllyTeam.Draw(discarded);
        //        return false;
        //    }
        //}

        ////Fantasy = Discard all and draw equal to amount
        //[HarmonyPatch(typeof(Skill_Extended))]
        //class Fantasy_Patch2
        //{
        //    [HarmonyPatch(nameof(Skill_Extended.DescExtended))]
        //    [HarmonyPostfix]
        //    static void Postfix(ref string __result, Skill_Extended __instance)
        //    {
        //        if (__instance is S_LBossFirst_4)
        //        {
        //            __result = "Discard all skills in hand. If the skill has an Ultimate Illusion Sword buff, <b>Exclude</b> it instead. Draw skills equal to discarded amount.";
        //        }
        //    }
        //}

    }

}



