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
using ChronoArkMod;
using ChronoArkMod.ModData;
using ChronoArkMod.ModData.Settings;
using ChronoArkMod.Plugin;
using DarkTonic.MasterAudio;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ExpertPlusMod
{
    [PluginConfig("ExpertPlusMod", "ExpertPlusMod", "1.0.0")]
    public class ExpertPlusPlugin : ChronoArkPlugin
    {
        private Harmony harmony;

        //public static bool PermaMode = false;
        public static bool VanillaCurses = false;
        public static bool DespairMode = false;
        public static bool CursedBosses = false;
        public static bool ChaosMode = false;
        public static bool EnableBossChanges = false;
        public static bool ExpertPlusPlus = false;

        public static string json = "";

        public override void Initialize()
        {
            ModInfo modInfo = ModManager.getModInfo("ExpertPlusMod");
            //PermaMode = modInfo.GetSetting<ToggleSetting>("PermaMode").Value;
            VanillaCurses = modInfo.GetSetting<ToggleSetting>("VanillaCurses").Value;
            DespairMode = modInfo.GetSetting<ToggleSetting>("DespairMode").Value;
            CursedBosses = modInfo.GetSetting<ToggleSetting>("CursedBosses").Value;
            ChaosMode = modInfo.GetSetting<ToggleSetting>("ChaosMode").Value;
            EnableBossChanges = modInfo.GetSetting<ToggleSetting>("EnableBossChanges").Value;
            ExpertPlusPlus = modInfo.GetSetting<ToggleSetting>("ExpertPlusPlus").Value;

            this.harmony = new Harmony(base.GetGuid());
            this.harmony.PatchAll();

            BanSave.Init();
            Debug.Log(Application.persistentDataPath + "/ExpertPlusClear.txt");
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
                        // Despair Mode: Give Stat Bonuses
                        //if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Enemy"))
                        //{
                        //    if (DespairMode.Value)
                        //    {
                        //        (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((masterJson[e.Key] as Dictionary<string, object>)["maxhp"]) * 1.2;
                        //        (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((masterJson[e.Key] as Dictionary<string, object>)["atk"]) * 1.2;
                        //        (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((masterJson[e.Key] as Dictionary<string, object>)["reg"]) * 2;
                        //    }
                        //}

                        // Despair Mode: Golden Apple can be used on fallen allies, cannot be used in battle
                        //if (e.Key == "GoldenApple")
                        //{
                        //    if (DespairMode.Value)
                        //    {
                        //        (masterJson[e.Key] as Dictionary<string, object>)["Target"] = "deathally";
                        //        (masterJson[e.Key] as Dictionary<string, object>)["active_battle"] = false;
                        //    }
                        //}

                        //if (e.Key == "LucysNecklace4" || e.Key == "LucysNecklace3" || e.Key == "LucysNecklace2")
                        //{
                        //    if (PermaMode)
                        //    {
                        //        (masterJson[e.Key] as Dictionary<string, object>)["Charge"] = 1;
                        //    }
                        //}

                        if (e.Key == "RedHammer")
                        {
                            //(masterJson[e.Key] as Dictionary<string, object>)["stack"] = 1;
                            (masterJson[e.Key] as Dictionary<string, object>)["Stack"] = 5;
                                //(masterJson[e.Key] as Dictionary<string, object>)["MaxStack"] = 5;
                        }

                        /// Despair Mode
                        if (e.Key == "S4_King_0")
                        {
                            if (DespairMode)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 1200;
                            }
                        }

                        if (e.Key == "LBossFirst")
                        {
                            if (DespairMode)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = 420;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;
                            }
                        }

                        if (e.Key == "Queue_S4_King")
                        {
                            if (DespairMode)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 18;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;
                            }
                        }
                        if (e.Key == "Queue_Witch")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("Boss_Golem");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 14;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 17;
                            }
                        }

                        if (e.Key == "Stage1_2")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("Queue_Witch");
                                (masterJson[e.Key] as Dictionary<string, object>)["Bosses"] = a;
                            }
                        }

                        if (e.Key == "Queue_DorchiX")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("S1_WitchBoss");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;

                                List<string> b = new List<string>();
                                b.Add("Boss_Golem");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 24;
                            }
                        }

                        if (e.Key == "Shiranui_Queue")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("MBoss2_0");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 15;

                                List<string> b = new List<string>();
                                b.Add("S2_Joker");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 17;
                            }
                        }

                        if (e.Key == "Casino_Queue")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("MBoss2_0");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 15;

                                List<string> b = new List<string>();
                                b.Add("S2_Joker");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 17;
                            }
                        }

                        if (e.Key == "Queue_S2_Joker")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("MBoss2_0");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 14;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 17;
                            }
                        }

                        if (e.Key == "Stage2_1")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("Queue_S2_Joker");
                                (masterJson[e.Key] as Dictionary<string, object>)["Bosses"] = a;
                            }
                        }

                        if (e.Key == "CrimsonQueue_GunManBoss")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("SR_Shotgun");
                                a.Add("SR_Outlaw");
                                a.Add("SR_Blade");
                                a.Add("SR_Sniper");
                                //a.Add("SR_Tumbledochi");
                                (masterJson[e.Key] as Dictionary<string, object>)["Enemys"] = a;

                                List<string> b = new List<string>();
                                b.Add("SR_GunManBoss");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 7;

                                (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = true;
                                Dictionary<string, int> pos1 = new Dictionary<string, int>();
                                pos1.Add("x", -10);
                                pos1.Add("y", 0);
                                pos1.Add("z", 6);
                                Dictionary<string, int> pos2 = new Dictionary<string, int>();
                                pos2.Add("x", 5);
                                pos2.Add("y", 0);
                                pos2.Add("z", 0);
                                Dictionary<string, int> pos3 = new Dictionary<string, int>();
                                pos3.Add("x", 0);
                                pos3.Add("y", 0);
                                pos3.Add("z", 2);
                                Dictionary<string, int> pos4 = new Dictionary<string, int>();
                                pos4.Add("x", -4);
                                pos4.Add("y", 0);
                                pos4.Add("z", -1);
                                //Dictionary<string, int> pos5 = new Dictionary<string, int>();
                                //pos5.Add("x", -10);
                                //pos5.Add("y", 0);
                                //pos5.Add("z", -1);
                                List<Dictionary<string, int>> c = new List<Dictionary<string, int>>();
                                c.Add(pos2);
                                c.Add(pos3);
                                c.Add(pos4);
                                c.Add(pos1);
                                //c.Add(pos5);
                                (masterJson[e.Key] as Dictionary<string, object>)["Pos"] = c;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 14;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 17;
                            }
                        }

                        if (e.Key == "Queue_S2_MainBoss_Luby")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("S2_BoomBalloon");
                                a.Add("S2_BoomBalloon");
                                a.Add("S2_BombClownBoss");
                                a.Add("S2_BoomBalloon");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;


                                List<string> b = new List<string>();
                                b.Add("MBoss2_1");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;

                                List<Vector3> c = new List<Vector3>();
                                (masterJson[e.Key] as Dictionary<string, object>)["Pos"] = c;
                                (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = false;

                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 24;
                            }
                        }

                        if (e.Key == "Stage2_2")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("Queue_S2_MainBoss_Luby");
                                (masterJson[e.Key] as Dictionary<string, object>)["Bosses"] = a;
                            }
                        }

                        if (e.Key == "Queue_S3_Reaper")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("S3_Boss_TheLight");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Lock"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;

                                List<string> b = new List<string>();
                                b.Add("Queue_S3_PharosLeader");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 24;
                            }
                        }

                        if (e.Key == "Queue_S3_TheLight")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("S3_Boss_TheLight");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Lock"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;

                                List<string> b = new List<string>();
                                b.Add("S3_Pharos_HighPriest");
                                b.Add("S3_Boss_Reaper");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 24;
                            }
                        }

                        if (e.Key == "Queue_S3_PharosLeader")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("S3_Boss_TheLight");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Lock"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;

                                List<string> b = new List<string>();
                                b.Add("S3_Pharos_HighPriest");
                                b.Add("S3_Boss_Reaper");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 24;
                            }
                        }

                        if (e.Key == "Queue_FanaticBoss")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("S3_Boss_TheLight");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                                (masterJson[e.Key] as Dictionary<string, object>)["Lock"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = false;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 99;
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 21;

                                List<string> b = new List<string>();
                                b.Add("S3_Pharos_HighPriest");
                                b.Add("S3_Boss_Reaper");
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = b;
                                (masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 100;
                            }
                            if (DespairMode && CursedBosses)
                            {
                                (masterJson[e.Key] as Dictionary<string, object>)["CustomeFogTurn"] = 24;
                            }
                        }

                        if (e.Key == "Stage3")
                        {
                            if (DespairMode)
                            {
                                List<string> a = new List<string>();
                                a.Add("Queue_S3_PharosLeader");
                                (masterJson[e.Key] as Dictionary<string, object>)["Bosses"] = a;
                            }
                        }

                        if (e.Key == "GoldenBread")
                        {
                            //if (PermaMode)
                            //{
                            //    (masterJson[e.Key] as Dictionary<string, object>)["Target"] = "ally";
                            //}
                        }

                        /// Enemy Hordes ///

                        /// Sanctuary ///

                        // Pikachu Living Armor Horde: Send Pikachu in Wave 2
                        if (e.Key == "FQ_6_5")
                        {
                            List<string> a = new List<string>();
                            a.Add("S4_AngryDochi");
                            (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;
                            (masterJson[e.Key] as Dictionary<string, object>)["Lock"] = false;
                            (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = false;
                            (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 2;
                            //(masterJson[e.Key] as Dictionary<string, object>)["Wave3"] = a;
                            //(masterJson[e.Key] as Dictionary<string, object>)["Wave3Turn"] = 3;
                        }

                        // 2 Hedgehog Fight: Add Pharos Mage on Wave 2
                        if (e.Key == "FQ_6_4")
                        {
                            List<string> a = new List<string>();
                            //a.Add("S2_PharosWitch");
                            a.Add("S2_Pharos_Mage");
                            (masterJson[e.Key] as Dictionary<string, object>)["Wave2"] = a;

                            (masterJson[e.Key] as Dictionary<string, object>)["Lock"] = false;
                            (masterJson[e.Key] as Dictionary<string, object>)["UseCustomPosition"] = false;
                            (masterJson[e.Key] as Dictionary<string, object>)["Wave2Turn"] = 2;
                        }

                        // Ascension Mode, drop rates reduced
                        //if (e.Key == "BossEquipRandomDrop")
                        //{
                        //    if (AscensionMode.Value)
                        //    {
                        //        /* 0/8/55/30/5 -> 0/4/35/10/1 */
                        //        (masterJson[e.Key] as Dictionary<string, object>)["Common"] = 0;
                        //        (masterJson[e.Key] as Dictionary<string, object>)["UnCommon"] = 4;
                        //        (masterJson[e.Key] as Dictionary<string, object>)["Rare"] = 35;
                        //        (masterJson[e.Key] as Dictionary<string, object>)["Unique"] = 10;
                        //        (masterJson[e.Key] as Dictionary<string, object>)["Legendary"] = 1;
                        //        (masterJson[e.Key] as Dictionary<string, object>)["NoItem"] = 50;
                        //    }
                        //}

                        //if (e.Key == "BattleRandomDrop")
                        //{
                        //    if (AscensionMode.Value)
                        //    {
                        //        (masterJson[e.Key] as Dictionary<string, object>)["NoItem"] = 100;
                        //    }
                        //}

                        ///// Misty Garden 1 ///

                        //// 1 maid fight: Add another maid 
                        //if (e.Key == "FQ_1_2")
                        //{
                        //    List<string> a = new List<string>();
                        //    a.Add("S1_Maid");
                        //    a.Add("S1_Maid");
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Enemys"] = a;
                        //}

                        ///// Misty Garden 2 ///

                        //// Carpenter Doll fight: add 1 Pharos Mage
                        //if (e.Key == "FQ_2_1")
                        //{
                        //    List<string> a = new List<string>();
                        //    a.Add("S1_CarpenterDoll");
                        //    a.Add("S1_Pharos_Mage");
                        //    (masterJson[e.Key] as Dictionary<string, object>)["Enemys"] = a;
                        //}
                    }
                }
                dataString = Json.Serialize(masterJson);
                json = dataString;
            }
        }

        // Exp++: Add global stat buff, attack power, armor
        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class ModifyGData2
        {
            static void Prefix(ref string dataString)
            {

                    Dictionary<string, object> masterJson = (Json.Deserialize(dataString) as Dictionary<string, object>);
                    foreach (var e in masterJson)
                    {
                        if (((Dictionary<string, object>)e.Value).ContainsKey("_gdeSchema"))
                        {
                            if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Enemy"))
                            {
                                if (ExpertPlusPlus)
                                {
                                    (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["atk"] * 1.1);
                                    (masterJson[e.Key] as Dictionary<string, object>)["reg"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["reg"] * 2);
                                    (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["maxhp"] * 1.1);
                                }
                            }
                        }
                    }
                    dataString = Json.Serialize(masterJson);
                    json = dataString;
                

            }
        }

        // Boost stats further at BM4c
        //[HarmonyPatch(typeof(BloodyMist), nameof(BloodyMist.IncreaseLevel))]
        //class BMist4Stats
        //{
        //    static void Postfix(BloodyMist __instance)
        //    {
        //        if (__instance.Level == 4)
        //        {
        //            Dictionary<string, object> masterJson = (Json.Deserialize(json) as Dictionary<string, object>);
        //            foreach (var e in masterJson)
        //            {
        //                if (((Dictionary<string, object>)e.Value).ContainsKey("_gdeSchema"))
        //                {
        //                    if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Enemy"))
        //                    {
        //                        (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["atk"] * 1.1);
        //                        (masterJson[e.Key] as Dictionary<string, object>)["reg"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["reg"] * 2);
        //                        (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["maxhp"] * 1.1);
        //                    }
        //                }
        //            }
        //            json = Json.Serialize(masterJson);
        //            var initMethod = AccessTools.Method(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText), new Type[] { typeof(string) });
        //            initMethod.Invoke(null, new object[] { json });
        //        }
        //    }
        //}

        // add starting items: dummy data, transcendent
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
                        // identifies lifting scroll
                        if (PlayData.TSavedata.IdentifyItems.Find((string x) => x == GDEItemKeys.Item_Scroll_Scroll_Uncurse) == null)
                        {
                            PlayData.TSavedata.IdentifyItems.Add(GDEItemKeys.Item_Scroll_Scroll_Uncurse);
                        }

                        // Add 1 lifting scroll
                        PartyInventory.InvenM.AddNewItem(ItemBase.GetItem(GDEItemKeys.Item_Scroll_Scroll_Uncurse, 1));

                    //}
                }
                if (CursedBosses && DespairMode && PlayData.TSavedata.StageNum == 0)
                {
                    PartyInventory.InvenM.AddNewItem(ItemBase.GetItem(GDEItemKeys.Item_Consume_EquipPouch, 1));
                    if (ExpertPlusPlus)
                    {
                        PartyInventory.InvenM.AddNewItem(ItemBase.GetItem(GDEItemKeys.Item_Consume_SkillBookLucy_Rare, 2));
                    }
                    else
                    {
                        PartyInventory.InvenM.AddNewItem(ItemBase.GetItem(GDEItemKeys.Item_Consume_SkillBookLucy_Rare, 3));
                    }

                    // identifies transfer scroll
                    if (PlayData.TSavedata.IdentifyItems.Find((string x) => x == GDEItemKeys.Item_Scroll_Scroll_Transfer) == null)
                    {
                        PlayData.TSavedata.IdentifyItems.Add(GDEItemKeys.Item_Scroll_Scroll_Transfer);
                    }
                    PartyInventory.InvenM.AddNewItem(ItemBase.GetItem(GDEItemKeys.Item_Scroll_Scroll_Transfer, 1));
                }

            }
        }

        // Decisive Strike Desc Manual Patch
        [HarmonyPatch(typeof(S_Lucy_25))]
        class S_Lucy_25_Patch
        {
            [HarmonyPatch(nameof(S_Lucy_25.HandInit))]
            [HarmonyPrefix]
            static bool Prefix(S_Lucy_25 __instance)
            {
                Debug.Log("Decisive Strike Desc");
                string language = LocalizationManager.CurrentLanguage;
                if (language == "Korean")
                {
                    __instance.MySkill.MySkill.Description = "적의 체력을 0으로 만듭니다.\n대상이 보스일경우에는, 보스의 체력이 40 % 이하일 때 사용 가능합니다.\n1지역 클리어 할 때 마다 비용이 1 증가합니다.\n<b>사용하면 덱에서 제거</b> 됩니다.\n(이 스킬은 '소원에 샘' 이외의 방법으로 얻을 수 없습니다)";
                }
                else if (language == "English")
                {
                    __instance.MySkill.MySkill.Description = "Reduce the target's HP to 0.\nBosses can be killed at 40% HP.\nCost is increased by 1 per stage cleared.\n<b>Removed from deck when used.</b>\n(This skill can only be obtained in Fountain of Wishes.)";
                }
                else if (language == "Japanese")
                {
                    __instance.MySkill.MySkill.Description = "敵の体力を0にする。\nターゲットがボスキャラクターの場合、体力40%以下で使用できる。\nこのスキルのコストはエリアをクリアする度、1上がる。\n<b>このスキルは、使用するとデッキから削除</b>される。\n(このスキルは「願いの泉」でしか習得できない。)";
                }
                else if (language == "Chinese")
                {
                    __instance.MySkill.MySkill.Description = "将敌人的体力值变为0点。\n若目标是BOSS，当目标体力值降到40%及以下时立即死亡。\n每当通过1个区域时费用增加1点。\n<b>使用后将从牌库中放逐。</b>\n（此技能无法通过‘许愿泉水’以外的方式获得。）";
                }
                else if (language == "Chinese-TW")
                {
                    __instance.MySkill.MySkill.Description = "將敵人的體力值變成0點。\n若目標是BOSS，當目標體力值降到40%及以下時立即死亡。\n每當通過1個區域時費用增加1點。\n<b>使用後將從牌庫中放逐。 </b>\n（此技能無法通過「許願泉水」以外的方式獲得。）";
                }
                return true;
            }
        }

        // Sniper Curse can be removed by lifting scroll
        [HarmonyPatch(typeof(SkillExtended_UnCurse))]
        class SniperCurse_Patch
        {
            [HarmonyPatch(nameof(SkillExtended_UnCurse.SkillUseSingle))]
            [HarmonyPrefix]
            static bool Prefix(SkillExtended_UnCurse __instance)
            {
                //Debug.Log("Vanished Char");
                foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyTeam.AliveChars_Vanish)
                {
                    foreach (Buff buff in battleEnemy.Buffs)
                    {
                        if (buff is B_CursedMob)
                        {
                            (buff as B_CursedMob).Uncurse();
                        }
                    }
                }
                foreach (BattleEnemy battleEnemy2 in BattleSystem.instance.EnemyTeam.AliveChars_Vanish)
                {
                    battleEnemy2.BuffRemove(GDEItemKeys.Buff_B_CursedMob_0, false);
                    battleEnemy2.BuffRemove(GDEItemKeys.Buff_B_CursedMob_1, false);
                    battleEnemy2.BuffRemove(GDEItemKeys.Buff_B_CursedMob_2, false);
                    battleEnemy2.BuffRemove(GDEItemKeys.Buff_B_CursedMob_3, false);
                    battleEnemy2.BuffRemove(GDEItemKeys.Buff_B_CursedMob_4, false);
                    battleEnemy2.BuffRemove(GDEItemKeys.Buff_B_CursedMob_5, false);
                }
                BattleSystem.instance.CurseBattle = false;
                PartyInventory.InvenM.DelItem(GDEItemKeys.Item_Scroll_Scroll_Uncurse, 1);
                AddressableLoadManager.Instantiate(__instance.MySkill.MySkill.Particle_Path, AddressableLoadManager.ManageType.None);

                return false;
            }
        }

        // Chaos Mode Randomization  
        static List<string> tier1 = new List<string>() { "S1_Statue1", "S1_Dochi", "S1_Maid", "S1_Table", "S1_Statue2", "S1_Pharos_Mage", "S1_Pharos_Healer", "S1_Pharos_Tanker", "S1_LittleMaid" };
        static List<string> tier2 = new List<string>() { "S1_Statue1", "S1_Dochi", "S1_Maid", "S1_Statue2", "S1_Pharos_Mage", "S1_Pharos_Tanker", "S1_LittleMaid", "S1_Butler", "S1_Pharos_Warrior" };
        static List<string> tier3 = new List<string>() { "S2_Pierrot_Bat", "S2_DochiDoll", "S2_Horse", "S2_Pierrot_Axe", "S2_Ghost", "S1_Pharos_Warrior", "S2_Pharos_Healer", "S2_Pharos_Mage" };
        static List<string> tier4 = new List<string>() { "S2_Pierrot_Bat", "S2_Horse", "S2_Pierrot_Axe", "S2_Pharos_Mage", "S2_PharosWitch", "S2_Pharos_Warrior", "S2_Pharos_Tanker", "SR_Gunner", "S1_Carpenterdoll", "S3_Wolf" };
        static List<string> tier5 = new List<string>() { "S3_SnowGiant_0", "S3_Pharos_Tanker", "S3_Pharos_HighPriest", "S3_Pharos_Assassin", "S2_Animatronics" };
        static List<string> tier6 = new List<string>() { "S4_MagicDochi", "S4_AngryDochi", "S4_Summoner", "S1_Armor", "S3_Deathbringer", "S3_Fugitive", "SR_Samurai", "S4_Guard_0_Solo", "S4_Guard_1_Solo", "S4_Guard_2_Solo", "MBoss_0_R" };
        static List<string> tier7 = new List<string>() { "SR_GuitarList", "SR_Shotgun", "SR_Blade"};
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
                if (ChaosMode)
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
                        else if (ExpertPlusPlus && a <= 55)
                        {
                            Debug.Log("Randomizing " + enemyKey + ", upscaled 2");
                            enemyKey = Randomize(enemyKey, 2);
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

                // Here
                if (CursedBosses)
                {
                    // Marauding & Executioner Ban
                    if (gdeenemyData.Boss == true && (gdeenemyData.Key == GDEItemKeys.Enemy_ProgramMaster || gdeenemyData.Key == GDEItemKeys.Enemy_SR_GunManBoss || gdeenemyData.Key == GDEItemKeys.Enemy_Boss_Golem || gdeenemyData.Key == "TheDealer" || gdeenemyData.Key == GDEItemKeys.Enemy_ProgramMaster2 || gdeenemyData.Key == GDEItemKeys.Enemy_S2_Shiranui || gdeenemyData.Key == "S3_Boss_Reaper" 
                        || gdeenemyData.Key == GDEItemKeys.Enemy_S2_MainBoss_1_0 || gdeenemyData.Key == GDEItemKeys.Enemy_S2_MainBoss_1_1 || gdeenemyData.Key == GDEItemKeys.Enemy_S4_King_0))
                    {
                        List<string> list = new List<string> { "B_CursedMob_1", "B_CursedMob_2", "B_CursedMob_3", "B_CursedMob_5" };
                        string curse = list.Random();
                        Debug.Log("Boss curse: " + curse);
                        component.BuffAdd(curse, component, false, 0, false, -1, false);
                    }
                    // Marauding Ban
                    else if (gdeenemyData.Boss == true && (gdeenemyData.Key == "S1_WitchBoss" || gdeenemyData.Key == GDEItemKeys.Enemy_S1_BossDorchiX))
                    {
                        List<string> list = new List<string> { "B_CursedMob_1", "B_CursedMob_2", "B_CursedMob_3", "B_CursedMob_4", "B_CursedMob_5" };
                        string curse = list.Random();
                        Debug.Log("Boss curse: " + curse);
                        component.BuffAdd(curse, component, false, 0, false, -1, false);
                    }
                    // Horrifying Ban
                    else if (gdeenemyData.Boss == true && (gdeenemyData.Key == "S1_ArmorBoss" || gdeenemyData.Key == "MBoss_0" || gdeenemyData.Key == GDEItemKeys.Enemy_S2_MainBoss_1_1))
                    {
                        List<string> list = new List<string> { "B_CursedMob_0", "B_CursedMob_1", "B_CursedMob_2", "B_CursedMob_4", "B_CursedMob_5" };
                        string curse = list.Random();
                        Debug.Log("Boss curse: " + curse);
                        component.BuffAdd(curse, component, false, 0, false, -1, false);
                    }
                    //Executioner Ban
                    else if (gdeenemyData.Boss == true && ( 
                        gdeenemyData.Key == GDEItemKeys.Enemy_S1_BossDorchiX || gdeenemyData.Key == GDEItemKeys.Enemy_MBoss2_0
                        || gdeenemyData.Key == GDEItemKeys.Enemy_MBoss2_1
                        || gdeenemyData.Key == GDEItemKeys.Enemy_S3_Boss_Pope || gdeenemyData.Key == GDEItemKeys.Enemy_S3_Boss_TheLight 
                        || gdeenemyData.Key == GDEItemKeys.Enemy_LBossFirst))
                    {
                        List<string> list = new List<string> { "B_CursedMob_0", "B_CursedMob_1", "B_CursedMob_2", "B_CursedMob_3", "B_CursedMob_5" };
                        string curse = list.Random();
                        Debug.Log("Boss curse: " + curse);
                        component.BuffAdd(curse, component, false, 0, false, -1, false);
                    }
                    else if (gdeenemyData.Boss == true)
                    {
                        List<string> list = new List<string> { "B_CursedMob_0", "B_CursedMob_1", "B_CursedMob_2", "B_CursedMob_3", "B_CursedMob_4", "B_CursedMob_5" };
                        string curse = list.Random();
                        Debug.Log("Boss curse: " + curse);
                        component.BuffAdd(curse, component, false, 0, false, -1, false);
                    }
                }
                if (Curse)
                {
                    // Misty Garden 1: Ban robust
                    if (PlayData.TSavedata.StageNum == 0) {
                        List<string> list = new List<string> { "B_CursedMob_0", "B_CursedMob_1", "B_CursedMob_3", "B_CursedMob_4", "B_CursedMob_5" };
                        component.BuffAdd(list.Random(), component, false, 0, false, -1, false);
                    }
                    else
                    {
                        List<string> list = new List<string> { "B_CursedMob_0", "B_CursedMob_1", "B_CursedMob_2", "B_CursedMob_3", "B_CursedMob_4", "B_CursedMob_5" };
                        component.BuffAdd(list.Random(), component, false, 0, false, -1, false);
                    }
                }
                // end of edit
                if (ExpertPlusPlus && PlayData.TSavedata.bMist != null && PlayData.TSavedata.bMist.Level == 4 )
                {
                    component.BuffAdd("B_Mist4Buff", component);
                    Debug.Log("Added hidden buff"); 
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

        // The below function freezes the game in Azar fight. 

        //[HarmonyPatch(typeof(BattleTeam))]
        //class asdfasdf
        //{
        //    [HarmonyPatch(nameof(BattleTeam.MyTurn))]
        //    [HarmonyPrefix]
        //    static bool Prefix()
        //    {
        //        Debug.Log(StageSystem.instance.Map.StageData.Key);
        //        return true;
        //    }
        //}

        // Mana reduced by 1 per charcter. Cannot fall below 3. (start of turn)
        //[HarmonyPatch(typeof(BattleTeam))]
        //class ManaRemove_Patch
        //{
        //    [HarmonyPatch(nameof(BattleTeam.MyTurn))]
        //    [HarmonyPrefix]
        //    static bool Prefix(BattleTeam __instance)
        //    {
        //        __instance.DummyCharAlly.Dummy = true;
        //        foreach (BattleChar battleChar in __instance.AliveChars)
        //        {
        //            battleChar.ActionCount = 1;
        //            battleChar.Overload = 0;
        //            battleChar.ActionNum = 0;
        //            battleChar.SkillUseDraw = false;
        //        }
        //        __instance.LucyAlly.ActionCount = 1;
        //        __instance.LucyAlly.Overload = 0;
        //        __instance.LucyAlly.ActionNum = 0;
        //        __instance.UsedDeckToDeckNum = 0;
        //        __instance.DiscardCount = __instance.GetDiscardCount;
        //        __instance.WaitCount = 1 + PlayData.PartySpeed;
        //        if (__instance.WaitCount >= 3)
        //        {
        //            __instance.WaitCount = 3;
        //        }
        //        if (__instance.WaitCount <= 1)
        //        {
        //            __instance.WaitCount = 1;
        //        }
        //        if (__instance.AliveChars.Find((BattleChar a) => a.Info.KeyData == GDEItemKeys.Character_LucyC) != null)
        //        {
        //            __instance.WaitCount = 1;
        //        }
        //        // Here
        //        if (StageSystem.instance.Map.StageData.Key != null && StageSystem.instance.Map.StageData.Key != GDEItemKeys.Stage_Stage_Crimson)
        //        {
        //            // nothing
        //        }
        //        else
        //        {
        //            int deadCount = BattleSystem.instance.AllyTeam.Chars.Count - BattleSystem.instance.AllyTeam.AliveChars.Count;
        //            Debug.Log("Dead count: " + deadCount);
        //            if (StageSystem.instance.Map.StageData.Key != "Stage_Crimson") // Not Crimson
        //            {
        //                if (__instance.MAXAP - deadCount > 3)
        //                {
        //                    __instance.AP = __instance.MAXAP - deadCount;
        //                }
        //                else
        //                {
        //                    __instance.AP = 3;
        //                }
        //            }
        //        }
        //        __instance.TurnActionNum = 0;
        //        List<BattleChar> list = new List<BattleChar>();
        //        AccessTools.FieldRef<BattleTeam, List<BattleChar>> G_Ref = AccessTools.FieldRefAccess<List<BattleChar>>(typeof(BattleTeam), "G_AliveChars");
        //        list.AddRange(G_Ref(__instance));
        //        List<BattleChar.TickInfo> list2 = new List<BattleChar.TickInfo>();
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            list2.Add(list[i].TickDamageReturn());
        //        }
        //        for (int j = 0; j < list.Count; j++)
        //        {
        //            List<Buff> list3 = new List<Buff>();
        //            list3.AddRange(list[j].Buffs);
        //            for (int k = 0; k < list3.Count; k++)
        //            {
        //                Buff buff = list3[k];
        //                list3[k].TurnUpdate();
        //                if (list3.Count == 0)
        //                {
        //                    break;
        //                }
        //                if (k >= list3.Count)
        //                {
        //                    k--;
        //                }
        //                else if (buff != list3[k])
        //                {
        //                    k--;
        //                }
        //            }
        //        }
        //        for (int l = 0; l < list.Count; l++)
        //        {
        //            list[l].TickUpdate(list2[l]);
        //        }
        //        foreach (BattleChar battleChar2 in list)
        //        {
        //            battleChar2.BattleUpdate();
        //        }
        //        if (__instance.LucyChar.IsLucyNoC)
        //        {
        //            __instance.LucyChar.TickUpdate(__instance.LucyChar.TickDamageReturn());
        //            __instance.LucyChar.BattleUpdate();
        //            List<Buff> list4 = new List<Buff>();
        //            list4.AddRange(__instance.LucyChar.Buffs);
        //            for (int m = 0; m < list4.Count; m++)
        //            {
        //                Buff buff2 = list4[m];
        //                list4[m].TurnUpdate();
        //                if (list4.Count == 0)
        //                {
        //                    break;
        //                }
        //                if (m >= list4.Count)
        //                {
        //                    m--;
        //                }
        //                else if (buff2 != list4[m])
        //                {
        //                    m--;
        //                }
        //            }
        //        }
        //        AccessTools.FieldRef<BattleTeam, int> G2_Ref = AccessTools.FieldRefAccess<int>(typeof(BattleTeam), "G_AliveCharGetFrame");
        //        G2_Ref(__instance) = 0;
        //        int num = PlayData.GetDraw;
        //        BattleSystem.instance.ActWindow.gameObject.SetActive(true);
        //        if (BattleSystem.instance.TurnNum == 0)
        //        {
        //            num = PlayData.GetDraw + __instance.AliveChars.Count;
        //            if (PlayData.TSavedata.SpRule != null)
        //            {
        //                num += PlayData.TSavedata.SpRule.RuleChange.PlusFirstTurnDraw;
        //            }
        //            List<Skill> list5 = new List<Skill>();
        //            list5.AddRange(__instance.Skills_Deck);
        //            foreach (Skill skill in list5)
        //            {
        //                foreach (Skill_Extended skill_Extended in skill.AllExtendeds)
        //                {
        //                    skill_Extended.BattleStartDeck(__instance.Skills_Deck);
        //                }
        //            }
        //            foreach (IP_FirstDrawBefore ip_FirstDrawBefore in BattleSystem.instance.IReturn<IP_FirstDrawBefore>())
        //            {
        //                if (ip_FirstDrawBefore != null)
        //                {
        //                    ip_FirstDrawBefore.FirstDrawBefore(__instance.Skills_Deck);
        //                }
        //            }
        //        }
        //        __instance.Draw(num);
        //        for (int n = 0; n < __instance.Chars.Count; n++)
        //        {
        //            __instance.BasicSkillRefill(__instance.Chars[n], __instance.Skills_Basic[n]);
        //        }
        //        return false;
        //    }
        //}

        //[HarmonyPatch(typeof(BattleChar))]
        //class ManaRemove2_Patch
        //{
        //    [HarmonyPatch(nameof(BattleChar.AllyDeadCheck))]
        //    [HarmonyPrefix]
        //    static bool Prefix(BattleChar __instance)
        //    {
        //        bool flag = false;
        //        if (__instance.Info.Passive is P_Phoenix)
        //        {
        //            flag = true;
        //        }
        //        if (flag)
        //        {
        //            if (__instance.Info.Hp <= 0)
        //            {
        //                if (!__instance.BuffFind(GDEItemKeys.Buff_B_Phoenix_P, false))
        //                {
        //                    __instance.BuffAdd(GDEItemKeys.Buff_B_Phoenix_P, __instance.MyTeam.DummyChar, false, 0, false, -1, false);
        //                }
        //                if (!__instance.BuffFind(GDEItemKeys.Buff_B_Phoenix_P_0, false))
        //                {
        //                    __instance.BuffAdd(GDEItemKeys.Buff_B_Phoenix_P_0, __instance.MyTeam.DummyChar, false, 0, false, -1, false);
        //                }
        //            }
        //            else if (__instance.BuffFind(GDEItemKeys.Buff_B_Phoenix_P_0, false))
        //            {
        //                __instance.BuffRemove(GDEItemKeys.Buff_B_Phoenix_P_0, true);
        //            }
        //        }
        //        else if (__instance.Recovery <= 0)
        //        {
        //            __instance.Dead(false);

        //            //Here
        //            //Debug.Log("Stage: " + StageSystem.instance.Map.StageData.Key);
        //            if (StageSystem.instance.Map.StageData.Key != GDEItemKeys.Stage_Stage_Crimson)
        //            {
        //                BattleSystem.instance.AllyTeam.AP--;
        //                //Debug.Log("Mana decreased due to death");
        //            }
        //        }
        //        else
        //        {
        //            __instance.UI.CharAni.SetBool("Dead", false);
        //            if (__instance.Info.Hp <= 0)
        //            {
        //                if (!__instance.BuffFind(GDEItemKeys.Buff_B_Neardeath, false))
        //                {
        //                    __instance.BuffAdd(GDEItemKeys.Buff_B_Neardeath, __instance.MyTeam.DummyChar, false, 0, false, -1, false);
        //                    foreach (IP_NearDeath ip_NearDeath in __instance.IReturn<IP_NearDeath>(null))
        //                    {
        //                        if (ip_NearDeath != null)
        //                        {
        //                            ip_NearDeath.NearDeath();
        //                        }
        //                    }
        //                    if (!__instance.BuffFind(GDEItemKeys.Buff_B_S3_Pope_P_2, false))
        //                    {
        //                        if (__instance.Info.KeyData == GDEItemKeys.Character_Phoenix)
        //                        {
        //                            __instance.BattleInfo.ScriptOut.LowHPAlly();
        //                            __instance.BattleInfo.ScriptOut.LowHP(__instance);
        //                        }
        //                        else if (Misc.RandomPer(100, 40))
        //                        {
        //                            __instance.BattleInfo.ScriptOut.LowHP(__instance);
        //                        }
        //                        else
        //                        {
        //                            __instance.BattleInfo.ScriptOut.LowHPAlly();
        //                        }
        //                    }
        //                }
        //                __instance.UI.CharAni.SetBool("NearDead", true);
        //                TutorialSystem.TutorialFlag(15);
        //            }
        //            else
        //            {
        //                if (__instance.BuffFind(GDEItemKeys.Buff_B_Neardeath, false))
        //                {
        //                    __instance.BuffRemove(GDEItemKeys.Buff_B_Neardeath, true);
        //                }
        //                __instance.UI.CharAni.SetBool("NearDead", false);
        //            }
        //        }
        //        return false;
        //    }
        //}

        // Change stats for cursed mob, change rewards
        [HarmonyPatch(typeof(B_CursedMob))]
        class Curse_Reward_Patch
        {
            [HarmonyPatch(nameof(B_CursedMob.Init))]
            [HarmonyPostfix]
            public static void Postfix(B_CursedMob __instance, List<ItemBase> ___Itemviews)
            {
                if (!VanillaCurses)
                {
                    // HP increase dropped to 40%, remove debuff resistance buff
                    __instance.PlusPerStat.MaxHP = __instance.PlusPerStat.MaxHP - 20;
                    __instance.PlusStat.RES_CC = __instance.PlusStat.RES_CC - 15f;
                    __instance.PlusStat.RES_DEBUFF = __instance.PlusStat.RES_DEBUFF - 15f;
                    __instance.PlusStat.RES_DOT = __instance.PlusStat.RES_DOT - 15f;
                    //Remove extra action count but increase atk by +40%  
                    //__instance.PlusPerStat.Damage = __instance.PlusPerStat.Damage + 40;
                    //__instance.BChar.Info.PlusActCount.Remove(1);
                }
                if (ExpertPlusPlus)
                {
                    __instance.PlusStat.hit = __instance.PlusStat.hit + 15;
                    __instance.PlusStat.HIT_CC = __instance.PlusStat.HIT_CC + 15;
                    __instance.PlusStat.HIT_DEBUFF = __instance.PlusStat.HIT_DEBUFF + 15;
                    __instance.PlusStat.HIT_DOT = __instance.PlusStat.HIT_DOT + 15;
                }

                //Gold reward reduced to 50
                if (___Itemviews.RemoveAll(x => x.itemkey == GDEItemKeys.Item_Misc_Gold) > 0)
                {
                    ___Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_Gold, 50));
                }

                // Drop uncommons on Bloody Park 2 as well. Item reward reworked. 

                // Cursed Boss: Heroic or Legendary Equip
                if ((__instance.BChar as BattleEnemy).Boss)
                {
                    if (ExpertPlusPlus)
                    {
                        ___Itemviews.RemoveAll(x => x.itemkey == GDEItemKeys.Item_Misc_Gold);
                        ___Itemviews.RemoveAll(x => x.itemkey == GDEItemKeys.Item_Scroll_Scroll_Uncurse);

                        ___Itemviews.Add(ItemBase.GetItem(PlayData.GetEquipRandom(3)));
                        __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_Soul, 3));

                        Random rand = new Random();
                        int a = rand.Next(1, 3); // 1-2
                        if (a == 1)
                        {
                            __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_RedHammer));
                        }
                        else
                        {

                        }
                        return;
                    }
                    else
                    {
                        ___Itemviews.RemoveAll(x => x.itemkey == GDEItemKeys.Item_Misc_Gold);
                        ___Itemviews.RemoveAll(x => x.itemkey == GDEItemKeys.Item_Scroll_Scroll_Uncurse);
                        Random rand = new Random();
                        int a = rand.Next(1, 3); // 1-2
                        __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_Soul, 3));
                        if (a == 1)
                        {
                            ___Itemviews.Add(ItemBase.GetItem(PlayData.GetEquipRandom(4)));
                        }
                        else
                        {
                            ___Itemviews.Add(ItemBase.GetItem(PlayData.GetEquipRandom(3)));
                            __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_RedHammer));
                        }
                        return;
                    }
                }

                bool flag = false;
                if (__instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S2_Horse || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S2_Pharos_Mage || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S2_PharosWitch || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S3_Fugitive || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S3_Pharos_Assassin || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S3_Deathbringer || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S3_Pharos_Tanker || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S3_SnowGiant_0 || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S3_Pharos_HighPriest)
                {
                    flag = true;
                }
                if (flag)
                {
                    if (___Itemviews.RemoveAll(x => x.GetisEquip) > 0 || PlayData.TSavedata.StageNum == 3)
                    {
                        Random rand = new Random();

                        int a = rand.Next(1, 101); // 1-100
                        // 59% 100G, 25% Useful Scrap Metal, 5% Herb, 5% Tablet, 5% Shield Generator, 1% Celestial
                        if (a <= 59)
                        {
                            __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_Gold, 100));
                        }
                        if (a == 60)
                        {
                            __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_Celestial));
                        }
                        if (a >= 61 && a <= 85)
                        {
                            ItemBase item = ItemBase.GetItem(PlayData.GetEquipRandom(1));
                            (item as Item_Equip).Curse = EquipCurse.RandomCurse(item as Item_Equip);
                            __instance.Itemviews.Add(item);
                        }
                        if (a >= 86 && a <= 90)
                        {
                            __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_Herb));
                        }
                        if (a >= 91 && a <= 95)
                        {
                            __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_SodaWater));

                        }
                        if (a >= 96 && a <= 100)
                        {
                            __instance.Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_SmallBarrierMachine));
                        }
                    }
                }

                // Misty Garden 2 removed lifting scroll
                if (PlayData.TSavedata.StageNum == 1)
                {
                    if (___Itemviews.RemoveAll(x => x.itemkey == GDEItemKeys.Item_Scroll_Scroll_Uncurse) > 0)
                    {
                        ___Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_Gold, 50));

                    }
                }

                // Hard Sanctuary Fights: Extra Reward
                flag = false;
                if ((__instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S4_Guard_1 /*|| __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S4_Golem || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S4_Golem2 || __instance.BChar.Info.KeyData == GDEItemKeys.Enemy_S4_Summoner*/))
                {
                    flag = true;
                }

                // Sanctuary cursed enemies
                if (PlayData.TSavedata.StageNum == 5)
                {
                    // Hard Fight: Drop legendary
                    if (flag)
                    {
                        ___Itemviews.Add(ItemBase.GetItem(PlayData.GetEquipRandom(4)));
                    }
                    // Easy Fight: Drop 100G
                    else
                    {
                        ___Itemviews.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_Gold, 100));
                    }
                }

                // Despair Mode: Sniper remove action count
                //if (DespairMode.Value)
                //{
                //    if (__instance.BChar.Info.KeyData == GDEItemKeys.Enemy_SR_Sniper)
                //    {
                //        __instance.BChar.Info.PlusActCount.RemoveAt(__instance.BChar.Info.PlusActCount.Count - 1);
                //    }
                //}

            }
        }

        // Sturdy Curse reworked - Shield only goes up once, Armor +20%
        //[HarmonyPatch(typeof(B_CursedMob_2))]
        //class Robust_Patch
        //{
        //    // Shield doesn't refresh after being removed. Skip the method entirely
        //    [HarmonyPatch(nameof(B_CursedMob_2.Turn))]
        //    [HarmonyPrefix]
        //    static bool Prefix()
        //    {
        //        if (VanillaCurses.Value)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    // Add Armor +20%, Add one-time Shield
        //    [HarmonyPatch(nameof(B_CursedMob_2.Init))]
        //    [HarmonyPostfix]
        //    static void Postfix(B_CursedMob_2 __instance)
        //    {
        //        if (!VanillaCurses.Value)
        //        {
        //            __instance.PlusStat.def = 20f;
        //            __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_Armor_P_1, __instance.BChar, false, 0, false, -1, false);
        //        }
        //    }


        //[HarmonyPatch(nameof(Buff.DescExtended))]
        //[HarmonyPostfix]
        //static void DescExtendedPostfix(ref string __result, Buff __instance)
        //{
        //    if (__instance is B_CursedMob_2)
        //    {
        //        __result = "Gain 1 action count\nBlock one incoming attack.";
        //    }
        //}
        //}


        // Add Infinite Skillbook in mg2 shop
        [HarmonyPatch(typeof(FieldStore))]
        class FieldStore_Patch
        {
            [HarmonyPatch(nameof(FieldStore.Init))]
            [HarmonyPostfix]
            static void Postfix(FieldStore __instance)
            {
                if (CursedBosses & DespairMode)
                {
                    if (PlayData.TSavedata.StageNum == 1)
                    {
                        __instance.StoreItems.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_SkillBookInfinity));
                    }
                }
            }
        }

        // Add Pain Debuff Resist -10% to Pierrot (and living armor) debuff
        //[HarmonyPatch(typeof(B_Pierrot_Bat_1_T))]
        //class PierrotDebuff_Patch
        //{
        //    [HarmonyPatch(nameof(B_Pierrot_Bat_1_T.Init))]
        //    [HarmonyPostfix]
        //    static void Postfix(B_Pierrot_Bat_1_T __instance)
        //    {
        //        __instance.PlusStat.RES_DOT = -10f;
        //    }
        //}

        /// <summary>
        /// Despair Mode
        /// </summary>
        /// 

        // Despair Mode: Do not spawn Lifting Scroll in battle.
        [HarmonyPatch(typeof(B_CursedMob), "BattleStart")]
        class CursedStart_Patch
        {
            static bool Prefix(B_CursedMob __instance)
            {
                Debug.Log("Stage Number: " + PlayData.TSavedata.StageNum);
                // living armor and cerberus: spawn 1 cost decisive strike
                if (CursedBosses && BattleSystem.instance.BossBattle && PlayData.TSavedata.StageNum == 0)
                {
                    Debug.Log("spawn");
                    Skill s = Skill.TempSkill(GDEItemKeys.Skill_S_Lucy_25, BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam);
                    s.AP = 2;
                    BattleSystem.instance.AllyTeam.Add(s, true);
                }
                if (DespairMode)
                {
                    return false;
                }
                return true;
            }
        }

        // Make decisive strike work like it used to
        [HarmonyPatch(typeof(S_Lucy_25), "Special_PointerEnter")]
        class DecisivePatch
        {
            static bool Prefix(BattleChar Char)
            {
                if (Char is BattleEnemy)
                {
                    if ((Char as BattleEnemy).Boss)
                    {
                        if (40f >= Misc.NumToPer((float)Char.GetStat.maxhp, (float)Char.HP))
                        {
                            EffectView.TextOutSimple(Char, ScriptLocalization.CharText_Ilya.Kill);
                        }
                    }
                    else if (100f >= Misc.NumToPer((float)Char.GetStat.maxhp, (float)Char.HP))
                    {
                        EffectView.TextOutSimple(Char, ScriptLocalization.CharText_Ilya.Kill);
                    }
                }
                return false;
            }
        }


        // Crimson Boss Battle: Spawn Decisive Strike
        [HarmonyPatch(typeof(B_Sniper_0), nameof(B_Sniper_0.Turn1))]
        class SniperDrawFire_Patch
        {
            static bool Prefix(B_Sniper_0 __instance)
            {
                if (DespairMode && !ExpertPlusPlus)
                {
                    if (BattleSystem.instance.TurnNum == 1)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            BattleSystem.instance.AllyTeam.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Sniper_1, BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam), true);
                        }
                        // Crimson Boss Fight
                        if (BattleSystem.instance.MainQueueData.Wave2Turn == 8 || PlayData.BattleQueue == GDEItemKeys.EnemyQueue_CrimsonQueue_GunManBoss)
                        {
                            Skill s = Skill.TempSkill(GDEItemKeys.Skill_S_Lucy_25, BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam);
                            s.AP = -1;
                            BattleSystem.instance.AllyTeam.Add(s, true);
                            Skill s2 = Skill.TempSkill(GDEItemKeys.Skill_S_Lucy_25, BattleSystem.instance.AllyTeam.LucyChar, BattleSystem.instance.AllyTeam);
                            s2.AP = -1;
                            //BattleSystem.instance.AllyTeam.Add(s2, true);
                        }
                    }
                    return false;
                }
                return true;
            }
        }

        // Godo 20 soulstones
        [HarmonyPatch(typeof(BattleSystem), "ClearBattle")]
        class GD20_Patch
        {
            static void Postfix(BattleSystem __instance)
            {
                Debug.Log(__instance.MainQueueData.Key);
                if (DespairMode)
                {
                    if (__instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_CrimsonQueue_GunManBoss)
                    {
                        __instance.Reward.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_Soul, 20));
                    }
                }
            }
        }

        // Expertplusplus: despawn campfire
        [HarmonyPatch(typeof(StageSystem), "IsoSetEventObject")]
        class RWCamp
        {
            [HarmonyPrefix]
            static bool Prefix(HexMap hexmap, int i)
            {
                if (ExpertPlusPlus)
                {
                    if (hexmap.EventTileList[i].Info.Type is Redwilder_Camp)
                    {
                        Debug.Log("Attempt Crimson Camp despawn");
                        return false;
                    }
                }
                return true;
            }
        }

        // Time Trial: time adjusted
        [HarmonyPatch(typeof(EventBattle_TrialofTime), "SetTimer")]

        class TimeTrialBosses
        {
            static bool Prefix(ref float __result)
            {
                if (DespairMode)
                {
                    float result = 600f;
                    if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Garden_Midboss)
                    {
                        result = 30f;
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_MBoss_0)
                    {
                        result = 40f;
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_Witch)
                    {
                        if (PlayData.TSavedata.SwordSanctuary)
                        {
                            result = 400f;
                        }
                        else
                        {
                            result = 200f;
                        }
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_Golem)
                    {
                        if (PlayData.TSavedata.SwordSanctuary)
                        {
                            result = 400f;
                        }
                        else
                        {
                            result = 200f;
                        }
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_DorchiX)
                    {
                        result = 420f;
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_S2_Joker)
                    {
                        result = 240f;
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_MBoss2_0)
                    {
                        result = 240f;
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_S2_MainBoss_Luby)
                    {
                        result = 540f;
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_S2_BombClown)
                    {
                        result = 540f;
                    }
                    else if (BattleSystem.instance.MainQueueData.Key == GDEItemKeys.EnemyQueue_Queue_S2_TimeEater)
                    {
                        result = 540f;
                    }
                    __result = result;
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Permadeath Mode
        /// </summary>

        // Permadeath Mode: Ban Medical Tent
        //[HarmonyPatch(typeof(RandomEventBaseScript))]
        //class MedTent_Patch
        //{
        //    [HarmonyPatch(nameof(RandomEventBaseScript.EventOpen_Base))]
        //    [HarmonyPostfix]
        //    static void Postfix(RandomEventBaseScript __instance)
        //    {
        //        if (PermaMode)
        //        {
        //            if (__instance is RE_Medicaltent)
        //            {
        //                //Debug.Log("Here");
        //                __instance.ButtonOff(0);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Ascension Mode
        /// </summary>

        // Ascension Mode: Add Slow Response to deck
        //[HarmonyPatch(typeof(StartPartySelect))]
        //class Ascension_Patch
        //{
        //    [HarmonyPatch(nameof(StartPartySelect.Apply))]
        //    [HarmonyPostfix]
        //    static void Postfix(StartPartySelect __instance)
        //    {
        //        // If Ascension Mode, add Slow Response
        //        if (AscensionMode.Value && PlayData.TSavedata.StageNum == 0)
        //        {
        //            //Debug.Log("Added Slow Response");
        //            PlayData.TSavedata.LucySkills.Add(GDEItemKeys.Skill_S_LucyCurse_Late);

        //            //Debug.Log("Relic Slots reduced");
        //            PlayData.TSavedata.Passive_Itembase.Remove(null);
        //            PlayData.TSavedata.Passive_Itembase.Remove(null);
        //            PlayData.TSavedata.ArkPassivePlus -= 2;
        //        }
        //    }
        //}

        // Ascension Mode: Equip Slots reduced
        //[HarmonyPatch(typeof(FieldSystem))]
        //class Ascension_Patch2
        //{
        //    [HarmonyPatch(nameof(FieldSystem.PartyAdd), new Type[] { typeof(GDECharacterData), typeof(int) })]
        //    [HarmonyPrefix]
        //    static bool Prefix(GDECharacterData CData, int Levelup = 0)
        //    {
        //        if (AscensionMode.Value)
        //        {
        //            Character character = new Character();
        //            character.Set_AllyData(CData);
        //            character.Hp = character.get_stat.maxhp;
        //            PlayData.TSavedata.DonAliveChars.Add(CData.Key);
        //            PlayData.TSavedata.Party.Add(character);
        //            if (FieldSystem.instance != null)
        //            {
        //                FieldSystem.instance.PartyWindowInit();
        //            }
        //            UIManager.inst.CharstatUI.GetComponent<CharStatV3>().Init();
        //            for (int i = 0; i < Levelup; i++)
        //            {
        //                UIManager.inst.CharstatUI.GetComponent<CharStatV3>().CWindows[PlayData.TSavedata.Party.Count - 1].Upgrade(true);
        //            }

        //            //Remove equip slot here
        //            //Debug.Log("Removed equip slot");
        //            character.Equip.Remove(null);
        //            return false;
        //        }
        //        return true;
        //    }
        //}

        // Ascension Mode: Reduce Potion Num
        //[HarmonyPatch(typeof(BattleSystem))]
        //class Ascension_Patch3
        //{
        //    [HarmonyPatch(nameof(BattleSystem.Start))]
        //    [HarmonyPostfix]
        //    static void Postfix()
        //    {
        //        // If Ascension Mode, reduce potion num
        //        if (AscensionMode.Value)
        //        {
        //            //Debug.Log("Potion Slots reduced");
        //            BattleSystem.instance.AllyTeam.MaxPotionNum = 2;
        //        }
        //    }
        //}
        //// Ascension Mode: Ilya Swords buff (compensation for equip slot reduced)
        //[HarmonyPatch(typeof(EItem.Ilya_Sword_0))]
        //class Ascension_Patch4
        //{
        //    [HarmonyPatch(nameof(EItem.Ilya_Sword_0.Init))]
        //    [HarmonyPostfix]
        //    static void Postfix(EItem.Ilya_Sword_0 __instance)
        //    {
        //        // If Ascension Mode, buff stats
        //        if (AscensionMode.Value)
        //        {
        //            __instance.PlusStat.cri = 15f;
        //        }
        //    }
        //}

        //// Ascension Mode: Ilya Swords buff (compensation for equip slot reduced)
        //[HarmonyPatch(typeof(EItem.Ilya_Sword_1))]
        //class Ascension_Patch5
        //{
        //    [HarmonyPatch(nameof(EItem.Ilya_Sword_1.Init))]
        //    [HarmonyPostfix]
        //    static void Postfix(EItem.Ilya_Sword_1 __instance)
        //    {
        //        // If Ascension Mode, buff stats
        //        if (AscensionMode.Value)
        //        {
        //            __instance.PlusStat.hit = 10f;
        //        }
        //    }
        //}

        //Ascension Mode : Equip Slot Centered
        //[HarmonyPatch(typeof(ChildClear), "Start")]
        //class CenterItemSlotPatch
        //{
        //    static void Postfix(ChildClear __instance)
        //    {
        //        if (AscensionMode.Value)
        //        {
        //            var transform = __instance.GetComponent<Transform>();
        //            if (transform.name == "EquipAlign")
        //            {
        //                // party view
        //                if (transform.parent.parent.name == "CloseView")
        //                {
        //                    transform.localPosition = new Vector3(transform.localPosition.x - 30f, transform.localPosition.y, transform.localPosition.z);
        //                }
        //                // blacksmith
        //                //if (transform.parent.name == "EquipView")
        //                //{
        //                //    transform.localPosition = new Vector3(transform.localPosition.x - 35f, transform.localPosition.y, transform.localPosition.z);
        //                //}
        //            }
        //        }
        //    }
        //}

        // Cursed Bosses: Parade tank cant double kaboom

        [HarmonyPatch(typeof(AI_MBoss2_0_0))]
        class Paradetank_patch
        {
            [HarmonyPatch(nameof(AI_MBoss2_0_0.SkillSelect))]
            [HarmonyPrefix]
            static bool Prefix(int ActionCount, AI_MBoss2_0_0 __instance, ref Skill __result)
            {
                if (CursedBosses)
                {
                    // Marauding
                    if (__instance.BChar.BuffFind("B_CursedMob_0"))
                    {
                        if (__instance.FirstTurn)
                        {
                            __result = __instance.BChar.Skills[0];
                        }
                        else if (__instance.Ready >= 3)
                        {
                            __result = __instance.BChar.Skills[0];
                            __instance.Ready = 0;
                        }
                        else if (__instance.Ready >= 1)
                        {
                            __result = __instance.BChar.Skills[2];
                        }
                        else
                        {
                            __result = __instance.BChar.Skills[1];
                        }
                        return false;
                    }

                    // Not Marauding
                    else
                    {
                        if (__instance.FirstTurn)
                        {
                            __result = __instance.BChar.Skills[0];
                        }
                        else if (__instance.Ready >= 2)
                        {
                            __result = __instance.BChar.Skills[0];
                            __instance.Ready = 0;
                        }
                        else if (__instance.Ready >= 1)
                        {
                            __result = __instance.BChar.Skills[2];
                        }
                        else
                        {
                            __result = __instance.BChar.Skills[1];
                        }
                        return false;
                    }
                }
                return true;
            }
        }

        //Witch casts dark spark when minions are active
        [HarmonyPatch(typeof(AI_Witch))]
        class WitchPatch
        {
            [HarmonyPatch(nameof(AI_Witch.SkillSelect))]
            [HarmonyPrefix]
            static bool Prefix(ref Skill __result, AI_Witch __instance, int ActionCount)
            {
                if (EnableBossChanges)
                {
                    if (ActionCount == 1 && BattleSystem.instance.EnemyList.Count != 1)
                    {
                        __result = __instance.BChar.Skills[1]; //berserk
                    }
                    else if (ActionCount == 1 && BattleSystem.instance.EnemyList.Count == 1)
                    {
                        __result = __instance.BChar.Skills[0]; //summon
                    }
                    else
                    {
                        __result = __instance.BChar.Skills[2]; //dark spark
                    }
                    return false;
                }

                // No Boss Changes
                else
                {
                    if (ActionCount == 1 && BattleSystem.instance.EnemyList.Count == 1)
                    {
                        __result = __instance.BChar.Skills[0];
                        return false;
                    }
                    if (BattleSystem.instance.EnemyList.Count > 1)
                    {
                        __result = __instance.BChar.Skills[1];
                        return false;
                    }
                }
                __result = null;
                return false;
            }
        }

        [HarmonyPatch(typeof(AI_ProgramMaster), "SpeedChange")]
        class DoubleEradicatePatch
        {
            [HarmonyPostfix]
            static void Postfix(Skill skill, int ActionCount, int OriginSpeed, AI_ProgramMaster __instance, ref int __result)
            {
                if (BattleSystem.instance.TurnNum == 1 && ActionCount == 2)
                {
                    __result = OriginSpeed;
                }
            }
        }

        [HarmonyPatch(typeof(AI_Boss_Reaper))]
        class Reaper_patch
        {
            [HarmonyPatch(nameof(AI_Boss_Reaper.SkillSelect))]
            [HarmonyPrefix]
            static bool Prefix(int ActionCount, AI_Boss_Reaper __instance, ref Skill __result)
            {
                if (ActionCount == 1)
                {
                    __result = __instance.BChar.Skills[0];
                    return false;
                }
                if (ActionCount == 0 && BattleSystem.instance.EnemyList.Count == 1)
                {
                    __result = __instance.BChar.Skills[3];
                    return false;
                }
                __result = __instance.SkillRandomSelect(new List<Skill>
                {
                    __instance.BChar.Skills[1],
                    __instance.BChar.Skills[2]
                });
                return false;
            }
        }

        // TFK speedchange
        [HarmonyPatch(typeof(Ai_King))]
        class TFK_Speed_Patch
        {
            [HarmonyPatch(nameof(Ai_King.SpeedChange))]
            [HarmonyPrefix]
            static bool Prefix(Skill skill, int ActionCount, int OriginSpeed, Ai_King __instance, ref int __result)
            {
                if (CursedBosses)
                {
                    if (skill.MySkill.KeyID == GDEItemKeys.Skill_S_S4_King_P2_0)
                    {
                        if (ActionCount == 1)
                        {
                            __result = 2;
                            return false;
                        }
                        else if (ActionCount == 2 && __instance.BChar.BuffFind("B_CursedMob_0"))
                        {
                            __result = 3;
                            return false;
                        }
                        else
                        {
                            __result = 99;
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        // Amplify Time delete
        [HarmonyPatch(typeof(B_Mboss2_1_P2))]
        class AmplifyTime_Delete
        {
            [HarmonyPatch(nameof(B_Mboss2_1_P2.Turn1))]
            [HarmonyPostfix]
            static void Postfix(B_Mboss2_1_P2 __instance)
            {
                bool find = false;
                foreach (BattleChar b in BattleSystem.instance.EnemyTeam.AliveChars)
                {
                    if (b.Info.KeyData == "MBoss2_1")
                    {
                        find = true;
                    }
                }
                if (find)
                {

                }
                else
                {
                    __instance.SelfDestroy();
                }
            }
        }

        // Insurmountable Patch
        //[HarmonyPatch(typeof(B_DuelistWill))]
        //[HarmonyPatch(nameof(B_DuelistWill.HPChange))]
        //class DuelistWillPatch
        //{
        //    [HarmonyPostfix]
        //    static void Postfix(B_DuelistWill __instance)
        //    {
        //        if (BattleSystem.instance.MainQueueData.Key != "CrimsonQueue_GunManBoss")
        //        {
        //            return;
        //        }
        //        __instance.BChar.BuffAdd(GDEItemKeys.Buff_B_S4_King_P_0, __instance.BChar, false, 0, false, -1, false);
        //    }
        //}

        // Neo Code
        //Every Fight is Cursed
        // Fisher–Yates shuffle
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
                                if (battleEnemy.BuffFind("B_S2_MainBoss_1_Left_P", false)) {
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


        //Permadeath Mode: No revival in campfire
        //[HarmonyPatch(typeof(CampUI))]
        //[HarmonyPatch(nameof(CampUI.Init))]
        //[HarmonyDebug]
        //class CampfireRevival_Patch
        //{
        //    static bool CheckIncapacitated(CampUI __instance, bool incapacitated)
        //    {
        //        return incapacitated && !PermaMode.Value;
        //    }

        //    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //    {
        //        foreach (var ci in instructions)
        //        {
        //            if (ci.Is(OpCodes.Ldfld, AccessTools.Field(typeof(Character), nameof(Character.Incapacitated))))
        //            {
        //                Debug.Log("inject");
        //                yield return ci;
        //                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CampfireRevival_Patch), nameof(CampfireRevival_Patch.CheckIncapacitated)));

        //            }
        //            else
        //            {
        //                yield return ci;
        //            }
        //        }
        //    }
        //}


        // Permadeath Mode: No revival in campfire
        //[HarmonyPatch(typeof(CampUI))]
        //[HarmonyPatch(nameof(CampUI.Init))]
        //class CampfireRevival_Patch
        //{
        //    [HarmonyPrefix]
        //    static bool Prefix(CampUI __instance, Camp Sc)
        //    {
        //        __instance.MainCampScript = Sc;
        //        if (!__instance.MainCampScript.Healed)
        //        {
        //            __instance.MainCampScript.Healed = true;
        //            foreach (Character character in PlayData.TSavedata.Party)
        //            {
        //                bool flag = false;
        //                if (character.Incapacitated && !PermaMode) //Here 1 line changed
        //                {
        //                    character.Incapacitated = false;
        //                    character.Hp = 1;
        //                    flag = true;
        //                    if (SaveManager.NowData.GameOptions.Difficulty == 0)
        //                    {
        //                        character.HealHP((int)Misc.PerToNum((float)character.get_stat.maxhp, 18f), true);
        //                    }
        //                    else if (SaveManager.NowData.GameOptions.Difficulty == 2)
        //                    {
        //                        character.HealHP((int)Misc.PerToNum((float)character.get_stat.maxhp, 10f), true);
        //                    }
        //                }
        //                if (SaveManager.NowData.GameOptions.Difficulty == 2)
        //                {
        //                    if (!flag)
        //                    {
        //                        character.HealHP((int)Misc.PerToNum((float)character.get_stat.maxhp, 20f), true);
        //                    }
        //                }
        //                else if (SaveManager.NowData.GameOptions.Difficulty == 1)
        //                {
        //                    character.HealHP((int)Misc.PerToNum((float)character.get_stat.maxhp, 60f), true);
        //                }
        //                else if (!flag)
        //                {
        //                    character.HealHP((int)Misc.PerToNum((float)character.get_stat.maxhp, 35f), true);
        //                }
        //                if (character.Passive != null)
        //                {
        //                    IP_CampFire ip_CampFire = character.Passive as IP_CampFire;
        //                    if (ip_CampFire != null)
        //                    {
        //                        ip_CampFire.Camp();
        //                    }
        //                }
        //                foreach (ItemBase itemBase in character.Equip)
        //                {
        //                    if (itemBase != null)
        //                    {
        //                        IP_CampFire ip_CampFire2 = (itemBase as Item_Equip).ItemScript as IP_CampFire;
        //                        if (ip_CampFire2 != null)
        //                        {
        //                            ip_CampFire2.Camp();
        //                        }
        //                    }
        //                }
        //            }
        //            foreach (ItemBase itemBase2 in PlayData.TSavedata.Inventory)
        //            {
        //                if (itemBase2 is Item_Equip)
        //                {
        //                    IP_CampFire ip_CampFire3 = (itemBase2 as Item_Equip).ItemScript as IP_CampFire;
        //                    if (ip_CampFire3 != null)
        //                    {
        //                        ip_CampFire3.Camp();
        //                    }
        //                }
        //            }
        //        }
        //        if (SaveManager.Difficalty != 2)
        //        {
        //            foreach (ItemBase itemBase3 in PartyInventory.InvenM.InventoryItems)
        //            {
        //                if (itemBase3 != null && (itemBase3.itemkey == GDEItemKeys.Item_Active_LucysNecklace || itemBase3.itemkey == GDEItemKeys.Item_Active_LucysNecklace2 || itemBase3.itemkey == GDEItemKeys.Item_Active_LucysNecklace3 || itemBase3.itemkey == GDEItemKeys.Item_Active_LucysNecklace4))
        //                {
        //                    Item_Active item_Active = itemBase3 as Item_Active;
        //                    int chargeNow = item_Active.ChargeNow;
        //                    item_Active.ChargeNow = chargeNow + 1;
        //                }
        //            }
        //        }
        //        if (PlayData.TSavedata.StageNum == 1 || PlayData.TSavedata.StageNum == 3)
        //        {
        //            if (PlayData.TSavedata.SpRule == null || !PlayData.TSavedata.SpRule.RuleChange.CantNewPartymember)
        //            {
        //                if (SaveManager.NowData.GameOptions.CasualMode)
        //                {
        //                    if (PlayData.TSavedata.StageNum == 1 && PlayData.TSavedata.Party.Count <= 2)
        //                    {
        //                        __instance.MainCampScript.CasualPartyAdd = true;
        //                    }
        //                    else if (PlayData.TSavedata.StageNum == 3 && PlayData.TSavedata.Party.Count <= 3)
        //                    {
        //                        __instance.MainCampScript.CasualPartyAdd = true;
        //                    }
        //                }
        //                __instance.Button_AddParty.gameObject.SetActive(true);
        //                PlayData.TSavedata.NowMaxMemberNum++;
        //            }
        //            else
        //            {
        //                __instance.Button_AddParty.gameObject.SetActive(false);
        //            }
        //            __instance.MainCampScript.Enforce = true;
        //        }
        //        else
        //        {
        //            __instance.Button_AddParty.gameObject.SetActive(false);
        //            __instance.Button_Enforce.gameObject.SetActive(true);
        //        }
        //        if (PlayData.TSavedata.Party.Count >= 4)
        //        {
        //            __instance.Button_AddParty.gameObject.SetActive(false);
        //        }
        //        if (PlayData.TSavedata.Party.Find((Character a) => a.GetData.Key == GDEItemKeys.Character_Leryn) != null)
        //        {
        //            __instance.Button_LerynPassive.gameObject.SetActive(true);
        //        }
        //        bool active;
        //        if (SaveManager.NowData.unlockList.UnlockSpecialRuleKey.Contains("BloodyMist"))
        //        {
        //            if (PlayData.TSavedata.StageNum == 4)
        //            {
        //                if (PlayData.TSavedata.bMist != null)
        //                {
        //                    if (PlayData.TSavedata.bMist.Level < 3)
        //                    {
        //                        active = true;
        //                    }
        //                    else
        //                    {
        //                        active = true;
        //                        if (!SaveManager.NowData.unlockList.UnlockSpecialRuleKey.Contains("BloodyMistLV4"))
        //                        {
        //                            __instance.BloodyMistLockObj.SetActive(true);
        //                            __instance.BloodyMistParticle.SetActive(false);
        //                            __instance.Button_BloodyMist.SetActive(false);
        //                            __instance.Button_BloodyMist.GetComponent<SimpleTooltip>().enabled = false;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    active = true;
        //                }
        //            }
        //            else
        //            {
        //                active = (PlayData.TSavedata.StageNum < 4);
        //            }
        //            List<string> list = new List<string>();
        //            string tooltipString = BloodyMist.LV1_Advantage + "\n" + Misc.InputColor(BloodyMist.LV1_Penalty, "FF0000");
        //            list.Add(BloodyMist.LV2_Advantage + "\n" + Misc.InputColor(BloodyMist.LV2_Penalty, "FF0000"));
        //            list.Add(BloodyMist.LV3_Advantage + "\n" + Misc.InputColor(BloodyMist.LV3_Penalty, "FF0000"));
        //            list.Add(BloodyMist.LV4_Advantage + "\n" + Misc.InputColor(BloodyMist.LV4_Penalty, "FF0000"));
        //            if (PlayData.TSavedata.bMist != null)
        //            {
        //                tooltipString = list[PlayData.TSavedata.bMist.Level - 1];
        //                if (PlayData.TSavedata.bMist.BeforeClearCampUI)
        //                {
        //                    __instance.BloodyMistButton();
        //                }
        //            }
        //            __instance.Button_BloodyMist.GetComponent<SimpleTooltip>().TooltipString = tooltipString;
        //        }
        //        else
        //        {
        //            active = false;
        //        }
        //        __instance.Button_BloodyMist.gameObject.SetActive(active);
        //        if (GamepadManager.IsPad) // not sure if this works
        //        {
        //            MethodInfo methodInfo = typeof(CampUI).GetMethod("Co_Delay2", BindingFlags.NonPublic | BindingFlags.Instance);
        //            var parameters = new object[] { };
        //            ((MonoBehaviour)__instance).StartCoroutine((IEnumerator)methodInfo.Invoke(__instance, parameters));
        //        }
        //        return false;
        //    }
        //}

        // Show ExpertPlusMod in result screen
        [HarmonyPatch(typeof(ResultUI))]
        [HarmonyPatch(nameof(ResultUI.Init))]
        class ResultScreenPatch
        {
            [HarmonyPostfix]
            static void Postfix(ResultUI __instance)
            {
                __instance.DifficultyObj.SetActive(true);
                __instance.BloodyMistObj.SetActive(true);
                Sprite sprite = AddressableLoadManager.LoadAsyncCompletion<Sprite>(new GDEImageDatasData(GDEItemKeys.ImageDatas_Image_BloodyMist).Sprites_Path[3], AddressableLoadManager.ManageType.Stage);
                __instance.BloodyMistImage.sprite = sprite;
                string cleartext = "ExpertPlusMod";
                if (PlayData.TSavedata.bMist != null)
                {
                    cleartext = ScriptLocalization.System_Mode.BloodyMist + " " + PlayData.TSavedata.bMist.Level.ToString() + "\n" + cleartext;
                }
                if (ExpertPlusPlus)
                {
                    cleartext += "\n+ExpertPlusPlus";
                }
                else
                {
                    if (DespairMode)
                    {
                        cleartext += "\n+Despair Mode";
                    }
                    //if (PermaMode)
                    //{
                    //    cleartext += "\n+Permadeath Mode";
                    //}
                    if (VanillaCurses)
                    {
                        cleartext += "\n+Vanilla Curses";
                    }
                    if (CursedBosses)
                    {
                        cleartext += "\n+Cursed Bosses";
                    }
                    if (ChaosMode)
                    {
                        cleartext += "\n+Chaos Mode";
                    }
                }

                __instance.BloodyMistText.text = cleartext;
                List<string> list2 = new List<string>();
                string text = "";
                if (DespairMode)
                {
                    list2.Add("<b>Despair Mode</b>\n1. Lifting Scrolls do not spawn in battle.\n2. After Misty Garden 1, fight all possible bosses for each stage. Godo and TFK fight is harder.\n");
                }
                //if (PermaMode)
                //{
                //    list2.Add("<b>Permadeath Mode</b>\nCampfires cannot revive allies. Removed revive option in Medical Tent. Golden Bread cannot be used on fallen allies.\n");
                //}
                if (VanillaCurses)
                {
                    list2.Add("<b>Vanilla Curses</b>\nReverts the nerfs to Cursed Mob stats.\n");
                }
                if (CursedBosses)
                {
                    list2.Add("<b>Cursed Bosses</b>\nCurses bosses.\n");
                }
                if (ChaosMode)
                {
                    list2.Add("<b>Chaos Mode</b>\nEnemies gain a low chance to mutate into enemies from the same stage or next stage.\nDoes not trigger for Cursed mobs or Boss fights.\n");
                }
                if (ExpertPlusPlus)
                {
                    list2.Add("<b>ExpertPlusPlus</b>\nAre you a masochist?");
                }
                for (int l = 0; l < list2.Count; l++)
                {
                    text += list2[l] + "\n";
                }
                __instance.BloodyMistObj.GetComponent<SimpleTooltip>().TooltipString = text;
            }
        }

        // Despair Mode Blood Mist 4 //

        // Makes TFK the second fight
        [HarmonyPatch(typeof(BloodyMist))]
        [HarmonyPatch(nameof(BloodyMist.DoubleBattle))]
        class TFKGrave
        {
            [HarmonyPrefix]
            static bool Prefix(BloodyMist __instance)
            {
                if (DespairMode && PlayData.TSavedata.bMist != null && PlayData.TSavedata.bMist.Level == 4)
                {
                    __instance.Level4DoubleBoss = true;
                    GDEItemKeys.EnemyQueue_Queue_S4_King = "LBossFirst_Queue";
                    FieldSystem.instance.BattleStart(new GDEEnemyQueueData("Queue_S4_King"), StageSystem.instance.StageData.BattleMap.Key, false, false, "", "", false);

                    List<ItemBase> list = new List<ItemBase>();
                    list.Add(ItemBase.GetItem(GDEItemKeys.Item_Consume_Herb));
                    InventoryManager.Reward(list);

                    Debug.Log("Changed to LBoss");
                    Debug.Log(PlayData.TSavedata.StageNum);
                    return false;
                }
                return true;
            }
        }

        // Revive and heal between tfk bloodmist4
        [HarmonyPatch(typeof(BloodyMist))]
        [HarmonyPatch(nameof(BloodyMist.BattleEnd))]
        class TFKGrave2
        {
            [HarmonyPostfix]
            static void Postfix(BloodyMist __instance)
            {
                if (DespairMode && PlayData.TSavedata.bMist != null && PlayData.TSavedata.bMist.Level == 4 && PlayData.TSavedata.StageNum != 5)
                {
                    if (PlayData.BattleQueue == GDEItemKeys.EnemyQueue_Queue_S3_PharosLeader || PlayData.BattleQueue == GDEItemKeys.Enemy_S3_Boss_Reaper || PlayData.BattleQueue == GDEItemKeys.EnemyQueue_Queue_S3_TheLight)
                    {
                        foreach (BattleChar b in BattleSystem.instance.AllyTeam.Chars)
                        {
                            if (b.Info.Incapacitated)
                            {
                                b.Info.Incapacitated = false;
                                b.HP = 1;
                                Debug.Log("healed");
                            }
                            int num = (int)Misc.PerToNum((float)b.GetStat.maxhp, 400f);
                            b.Heal(b, (float)num, false);
                        }
                    }
                }
                //return true;
            }
        }

        // Legend + Legend = Rare Enchanted Legend
        [HarmonyPatch(typeof(CampAnvilEvent))]
        [HarmonyPatch(nameof(CampAnvilEvent.B1Fuc))]
        class LegendEnchant
        {
            [HarmonyPrefix]
            static bool Prefix(CampAnvilEvent __instance)
            {
                if (!__instance.CombineBtn.interactable)
                {
                    return true;
                }
                if (__instance.InventoryItems[0] != null && __instance.InventoryItems[1] != null)
                {
                    if (__instance.InventoryItems[0] is Item_Equip && __instance.InventoryItems[1] is Item_Equip)
                    {
                        Debug.Log("Item 1: "+__instance.InventoryItems[0].itemkey);
                        Debug.Log("Item 2: "+__instance.InventoryItems[1].itemkey);
                        if (__instance.InventoryItems[0].ItemClassNum == 4 && __instance.InventoryItems[1].ItemClassNum == 4)
                        {
                            List<ItemBase> items = new List<ItemBase> {
                            ItemBase.GetItem(__instance.InventoryItems[0].itemkey),
                            ItemBase.GetItem(__instance.InventoryItems[0].itemkey),
                            ItemBase.GetItem(__instance.InventoryItems[0].itemkey),
                            ItemBase.GetItem(__instance.InventoryItems[1].itemkey),
                            ItemBase.GetItem(__instance.InventoryItems[1].itemkey),
                            ItemBase.GetItem(__instance.InventoryItems[1].itemkey),
                            };
                            List<string> list = new List<string> { "CurseEn_NightMare", "CurseEn_Spike", "CurseEn_Giant", "CurseEn_Dream", "CurseEn_Rapid" };
                            
                            
                            List<string> list2 = list.Random(RandomClassKey.CursedEnchant, 3);
                            list2.AddRange(list.Random(RandomClassKey.CursedEnchant, 3));

                            for (int i=0;i<6;i++)
                            {
                                (items[i] as Item_Equip).Enchant = ItemEnchant.NewEnchant((items[i] as Item_Equip), list2[i]);
                                (items[i] as Item_Equip)._Isidentify = true;
                            }

                            MasterAudio.PlaySound("Anvil", 1f, null, 0f, null, null, false, false);
                            UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>().Init(items, null, false);
                            __instance.DelItem(0);
                            __instance.DelItem(1);
                            PlayData.TSavedata.AnvilCount--;
                            return false;
                        }
                    }
                    return true;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(P_King))]
        [HarmonyPatch(nameof(P_King.HPChange))]
        class TFKBattleEnd
        {
            [HarmonyPrefix]
            static bool Prefix(P_King __instance)
            {

                if (__instance.MainAI.Phase == 2 && __instance.BChar.HP <= 0 && !__instance.LastAttkacted)
                {
                    __instance.BChar.Dead(false, false);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(P_King))]
        [HarmonyPatch(nameof(P_King.Dead))]
        class TFKBattleEnd2
        {
            [HarmonyPrefix]
            static bool Prefix(P_King __instance)
            {
                List<ItemBase> list = new List<ItemBase>();
                list.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_TimeMoney, 7));
                InventoryManager.Reward(list);

                foreach (BattleChar b in BattleSystem.instance.AllyTeam.Chars)
                {
                    if (b.Info.Incapacitated)
                    {
                        b.Info.Incapacitated = false;
                        b.HP = 1;
                        Debug.Log("healed");
                    }
                    int num = (int)Misc.PerToNum((float)b.GetStat.maxhp, 400f);
                    b.Heal(b, (float)num, false);
                }
                return false;
            }
        }

        //[HarmonyPatch(typeof(P_King))]
        //[HarmonyPatch(nameof(P_King.BattleEnd))]
        //class TFKBattleEnd2
        //{
        //    [HarmonyPrefix]
        //    static bool Prefix()
        //    {
        //        BattleSystem.instance.Reward.Add(ItemBase.GetItem(GDEItemKeys.Item_Misc_TimeMoney, 7));
        //        SaveManager.NowData.statistics.BossKIllAdd(BattleSystem.instance.MainQueueData.Key);
        //        BattleSystem.instance.ClearEnabled = true;
        //        BattleSystem.instance.StepStartEndBattle(BattleSystem.BattleEndType.ClearBattle);
        //        return false;
        //    }   
        //}

        //[HarmonyPatch(typeof(P_King))]
        //[HarmonyPatch(nameof(P_King.BattleEndafter))]
        //class TFKBattleEnd2
        //{
        //    [HarmonyPrefix]
        //    static bool Prefix(P_King __instance, ref IEnumerator __result)
        //    {
        //        //InventoryManager.Reward(new List<ItemBase>
        //        //        {
        //        //        ItemBase.GetItem(GDEItemKeys.Item_Misc_TimeMoney, 7),
        //        //        });
        //        BattleSystem.instance.BattleEnd(false, false);
        //        return false;
        //    }
        //}

        [HarmonyPatch(typeof(DataCollectMgr), "GameEnd")]
        class ResetBoss
        {
            [HarmonyPostfix]
            static void Postfix(bool Win)
            {
                if (PlayData.TSavedata.StageNum != 4) // Stagenum = 4 is despair mode tfk in white grave
                {
                    GDEItemKeys.EnemyQueue_Queue_S4_King = "Queue_S4_King";
                    Debug.Log("Reset Sanctuary Boss");

                    if (Win)
                    {
                        foreach (Character character in PlayData.TSavedata.Party)
                        {
                            BanSave.BanCharacterKeys.Add(character.KeyData);
                            Debug.Log("Added " + character.KeyData);

                            if (ExpertPlusPlus)
                            {
                                BanSave.BanCharacterKeys.Add(character.KeyData+"+");
                                Debug.Log("Added " + character.KeyData);
                            }
                        }
                        BanSave.WriteBanKeys();
                    }
                }

                //if (PlayData.TSavedata.bMist.Level == 4)
                //{
                //    Dictionary<string, object> masterJson = (Json.Deserialize(json) as Dictionary<string, object>);
                //    foreach (var e in masterJson)
                //    {
                //        if (((Dictionary<string, object>)e.Value).ContainsKey("_gdeSchema"))
                //        {
                //            if (((Dictionary<string, object>)e.Value)["_gdeSchema"].Equals("Enemy"))
                //            {
                //                (masterJson[e.Key] as Dictionary<string, object>)["atk"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["atk"] / 1.1);
                //                (masterJson[e.Key] as Dictionary<string, object>)["reg"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["reg"] / 2);
                //                (masterJson[e.Key] as Dictionary<string, object>)["maxhp"] = (long)((long)(masterJson[e.Key] as Dictionary<string, object>)["maxhp"] / 1.1);
                //            }
                //        }
                //    }
                //    json = Json.Serialize(masterJson);
                //    var initMethod = AccessTools.Method(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText), new Type[] { typeof(string) });
                //    initMethod.Invoke(null, new object[] { json });
                //}
            }
        }
        // Despair Mode Blood Mist 4 End //

        // Clear Border for ExpertPlus
        [HarmonyPatch(typeof(CharSelectButtonV2), nameof(CharSelectButtonV2.Init))]
        class ClearBorder
        {
            [HarmonyPostfix]
            static void Postfix(CharSelectButtonV2 __instance)
            {
                string plus = __instance.data.Key + "+"; // Expert++ Clear
                if (BanSave.BanCharacterKeys.Contains(plus))
                {
                    __instance.ClearIcon.gameObject.SetActive(false);

                    GameObject gameObject = Utils.creatGameObject("border", __instance.transform);
                    Image bg = gameObject.AddComponent<Image>();
                    Utils.getSprite("red.png", bg);
                    Utils.ImageResize(bg, new Vector2(120f, 165f), Vector2.zero);
                    __instance.ClearIcon = bg;
                    __instance.ClearIcon.sprite = bg.sprite;
                    __instance.ClearIcon.gameObject.SetActive(true);

                    Debug.Log("border switch attempted");
                }
                else if (BanSave.BanCharacterKeys.Contains(__instance.data.Key))
                {
                    __instance.ClearIcon.gameObject.SetActive(false);

                    GameObject gameObject = Utils.creatGameObject("border", __instance.transform);
                    Image bg = gameObject.AddComponent<Image>();
                    Utils.getSprite("purple.png", bg);
                    Utils.ImageResize(bg, new Vector2(120f, 165f), Vector2.zero);
                    __instance.ClearIcon = bg;
                    __instance.ClearIcon.sprite = bg.sprite;
                    __instance.ClearIcon.gameObject.SetActive(true);

                    Debug.Log("border switch attempted");
                }
            }
        }

        [HarmonyPatch(typeof(CharSelectButtonV2), nameof(CharSelectButtonV2.Click))]
        class SelectBox
        {
            [HarmonyPostfix]
            static void Postfix(CharSelectButtonV2 __instance)
            {
                string plus = __instance.data.Key + "+"; // Expert++ Clear
                if (BanSave.BanCharacterKeys.Contains(plus))
                {
                    __instance.Box.color = Misc.HexColor("9c3f64");
                    Debug.Log("Box Color Switch Attempted");
                }
                else if (BanSave.BanCharacterKeys.Contains(__instance.data.Key))
                {
                    //__instance.Box.color = Misc.HexColor("EFB2AB");
                    __instance.Box.color = Misc.HexColor("cdabef");
                    Debug.Log("Box Color Switch Attempted");
                    //GameObject gameObject = Utils.creatGameObject("line", __instance.transform);
                    //Image line = gameObject.AddComponent<Image>();
                    //Utils.getSprite("redline.png", line);
                    //__instance.SetLine(line.sprite);
                    //Debug.Log("Line Color Switch Attempted");
                }
            }
        }
    }
}