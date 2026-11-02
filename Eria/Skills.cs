using ChronoArkMod;
using System;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
using UnityEngine;
namespace Eria
{
    // Paint (Fixed): pick a mix to create in hand. Cancelling gives the fixed ability back.
    public class S_Eria_0 : Skill_Extended
    {
        private bool chosen = false;

        public override void Init()
        {
            this.APChange = -99;
        }

        // Paint skill -> paint it consumes. Listed in the order they're shown.
        public static readonly List<KeyValuePair<string, string[]>> Recipes = new List<KeyValuePair<string, string[]>>
        {
            new KeyValuePair<string, string[]>("S_Eria_RedBlueYellow", new string[] { P_Eria.Red, P_Eria.Blue, P_Eria.Yellow }),
            new KeyValuePair<string, string[]>("S_Eria_RedRed", new string[] { P_Eria.Red, P_Eria.Red }),
            new KeyValuePair<string, string[]>("S_Eria_BlueBlue", new string[] { P_Eria.Blue, P_Eria.Blue }),
            new KeyValuePair<string, string[]>("S_Eria_YellowYellow", new string[] { P_Eria.Yellow, P_Eria.Yellow }),
            new KeyValuePair<string, string[]>("S_Eria_RedBlue", new string[] { P_Eria.Red, P_Eria.Blue }),
            new KeyValuePair<string, string[]>("S_Eria_RedYellow", new string[] { P_Eria.Red, P_Eria.Yellow }),
            new KeyValuePair<string, string[]>("S_Eria_BlueYellow", new string[] { P_Eria.Blue, P_Eria.Yellow }),
            new KeyValuePair<string, string[]>("S_Eria_White", new string[0]),
        };

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            List<Skill> list = new List<Skill>();
            foreach (KeyValuePair<string, string[]> recipe in Recipes)
            {
                if (CanMix(this.BChar, recipe.Value))
                {
                    list.Add(Skill.TempSkill(recipe.Key, this.BChar, this.BChar.MyTeam));
                }
            }
            BattleSystem.DelayInput(this.Select(list));
        }

        private IEnumerator Select(List<Skill> list)
        {
            chosen = false;
            yield return BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Mix), ScriptLocalization.System_SkillSelect.CreateSkill, true, true, true, false, true);
            if (!chosen)
            {
                // Cancelled: take 2 off the cooldown (BasicSkillRefill itself takes 1, so take 1 more first),
                // then refresh the fixed ability so it can be used again
                BattleAlly ally = this.BChar as BattleAlly;
                if (ally != null && ally.MyBasicSkill != null && ally.MyBasicSkill.CoolDownNum >= 1)
                {
                    ally.MyBasicSkill.CoolDownNum--;
                }
                this.BChar.MyTeam.BasicSkillRefill(this.BChar, this.BChar.BattleBasicskillRefill);
            }
            yield break;
        }

        public static bool CanMix(BattleChar eria, string[] paints)
        {
            foreach (string color in P_Eria.Colors)
            {
                int need = Array.FindAll(paints, p => p == color).Length;
                if (P_Eria.PaintCount(eria, color) < need)
                {
                    return false;
                }
            }
            return true;
        }

        public void Mix(SkillButton mybutton)
        {
            chosen = true;
            string key = mybutton.Myskill.MySkill.KeyID;
            foreach (KeyValuePair<string, string[]> recipe in Recipes)
            {
                if (recipe.Key == key)
                {
                    foreach (string paint in recipe.Value)
                    {
                        P_Eria.UsePaint(this.BChar, paint);
                    }
                    break;
                }
            }
            Skill skill = Skill.TempSkill(key, this.BChar, this.BChar.MyTeam);
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }
    }

    // Rose
    public class S_Eria_1 : Skill_Extended
    {
    }

    // Vin de Sanguine
    public class S_Eria_2 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            if (Targets.Count == 1)
            {
                Targets[0].BuffAdd("B_Eria_2", this.BChar);
            }
        }
    }

    // Coup de Grâce
    public class S_Eria_3 : Skill_Extended
    {
        private bool triggered = false;

        public override void Init()
        {
            base.Init();
            this.SkillBasePlus.Target_BaseDMG = (int)(this.BChar.GetStat.reg * 1f);
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            triggered = false;
        }
        public override void FixedUpdate()
        {
            this.SkillBasePlus.Target_BaseDMG = (int)(this.BChar.GetStat.reg * 1f);
            base.FixedUpdate();
        }
        public override void SkillKill(SkillParticle SP)
        {
            base.SkillKill(SP);
            this.Restore();
        }
        public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
        {
            if (DMG >= 50)
            {
                this.Restore();
            }
        }
        // Only once per cast, even if the skill both kills and deals 50+ damage
        private void Restore()
        {
            if (triggered)
            {
                return;
            }
            triggered = true;
            foreach (BattleChar b in BattleSystem.instance.AllyTeam.AliveChars)
            {
                b.Heal(this.BChar, 0, (int)(b.GetStat.maxhp * 0.33), 0);

                List<Buff> list = new List<Buff>();
                foreach (Buff buff in b.Buffs)
                {
                    if (buff.BuffData.Debuff && !buff.BuffData.Cantdisable && !buff.BuffData.Hide && !buff.DestroyBuff)
                    {
                        list.Add(buff);
                    }
                }
                if (list.Count != 0)
                {
                    b.BuffRemove(list.Random(b.GetRandomClass().Main).BuffData.Key, false);
                }
            }
        }
    }

    // Lily
    public class S_Eria_4 : Skill_Extended
    {
    }

    // Cieux Bleus
    public class S_Eria_5 : Skill_Extended
    {
    }

    // La Rivière
    public class S_Eria_6 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (Buff buff in Targets[0].GetBuffs(BattleChar.GETBUFFTYPE.ALLDEBUFF, true, false))
            {
                buff.SelfDestroy(false);
            }
        }
    }

    // Dandelion
    public class S_Eria_7 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            // At Death's Door, raise the healing gauge by the heal amount so the heal goes past it (overheal)
            BattleChar target = Targets[0];
            if (target.BuffFind(GDEItemKeys.Buff_B_Neardeath, false))
            {
                target.Recovery += (int)(this.BChar.GetStat.reg * 1f);
            }
        }
    }

    // Récolte d'Automne
    public class S_Eria_8 : Skill_Extended
    {
        public override void SkillUseSingleAfter(Skill SkillD, List<BattleChar> Targets)
        {
            base.SkillUseSingleAfter(SkillD, Targets);
            foreach (BattleChar b in Targets)
            {
                b.Overload = 0;
            }
        }
    }

    // Hirondelle Sacrée
    public class S_Eria_9 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.Draw(1);
        }
    }

    // Le Jardin Fleuri: no heal of its own, casts 3 AoE heals in a row (red, blue, yellow flower)
    public class S_Eria_10 : Skill_Extended
    {
        public static readonly string[] Flowers = new string[] { "S_Eria_10_0", "S_Eria_10_1", "S_Eria_10_2" };

        // Each flower heals for 60% of Healing Power (HEAL_Per 60 on its skill effect)
        public override string DescExtended(string desc)
        {
            // &b: blue flower barrier (60% of Healing Power, added in Cast)
            return base.DescExtended(desc).Replace("&a", ((int)(this.BChar.GetStat.reg * 0.6f)).ToString())
                .Replace("&b", ((int)(this.BChar.GetStat.reg * 0.6f)).ToString());
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (string flower in Flowers)
            {
                BattleSystem.DelayInputAfter(this.Cast(flower));
            }
        }

        private IEnumerator Cast(string key)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            // Jardin itself has no target, so each flower hits every ally still alive at that moment
            List<BattleChar> allies = new List<BattleChar>(BattleSystem.instance.AllyTeam.AliveChars);

            // ParticleOut doesn't run the flower skill's SkillUseSingle, so the extra flower effects happen here
            if (key == "S_Eria_10_1")
            {
                // Blue flower: barrier based on Healing Power (added the way Control's skill adds barriers)
                foreach (BattleChar b in allies)
                {
                    Buff buff = b.BuffAdd("B_Eria_10_1", this.BChar);
                    if (buff != null)
                    {
                        buff.BarrierHP += (int)(this.BChar.GetStat.reg * 0.6f);
                    }
                }
            }
            else if (key == "S_Eria_10_2")
            {
                // Yellow flower: 1 Mana for each ally already at full health, and the heal overheals
                int mana = 0;
                foreach (BattleChar b in allies)
                {
                    if (b.HP >= b.GetStat.maxhp)
                    {
                        mana++;
                    }
                    b.Recovery += (int)(this.BChar.GetStat.reg * 0.6f);
                }
                BattleSystem.instance.AllyTeam.AP += mana;
            }

            Skill skill = Skill.TempSkill(key, this.BChar, this.BChar.MyTeam);
            skill.FreeUse = true;
            skill.PlusHit = true;
            this.BChar.ParticleOut(skill, allies);
            yield break;
        }
    }
    // Red flower: heal + next attack +50% damage (B_Eria_10 from the skill effect)
    public class S_Eria_10_0 : Skill_Extended
    {
    }
    // Blue flower: heal (barrier is added by S_Eria_10)
    public class S_Eria_10_1 : Skill_Extended
    {
    }
    // Yellow flower: heal (overheal and Mana are handled by S_Eria_10)
    public class S_Eria_10_2 : Skill_Extended
    {
    }

    // Couleurs Vivantes
    public class S_Eria_11 : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            // The skill effect gives 1 stack; this makes it 2/2
            foreach (BattleChar b in Targets)
            {
                b.BuffAdd("B_Eria_11", this.BChar);
            }
        }
    }

    // ---------- Paint skills ----------

    // Page Blanche
    public class S_Eria_White : Skill_Extended
    {
    }

    // Femme Fatale
    public class S_Eria_RedRed : Skill_Extended
    {
        private int HitDamage()
        {
            return (int)(this.BChar.GetStat.reg * 0.6f);
        }

        public override string DescExtended(string desc)
        {
            return base.DescExtended(desc).Replace("&a", this.HitDamage().ToString());
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            int stacks = 0;
            foreach (BattleChar e in BattleSystem.instance.EnemyTeam.AliveChars)
            {
                foreach (Buff buff in e.GetBuffs(BattleChar.GETBUFFTYPE.ALLDEBUFF, false, false))
                {
                    stacks += buff.StackNum;
                }
            }
            int dmg = this.HitDamage();
            // Queue each hit separately so they play one after another with a pause in between
            for (int i = 0; i < stacks; i++)
            {
                BattleSystem.DelayInputAfter(this.Hit(dmg));
            }
        }

        private IEnumerator Hit(int dmg)
        {
            yield return new WaitForSecondsRealtime(0.15f);
            List<BattleChar> alive = BattleSystem.instance.EnemyTeam.AliveChars;
            if (alive.Count == 0)
            {
                yield break;
            }
            Skill skill = Skill.TempSkill("S_Eria_RedRed_0", this.BChar, this.BChar.MyTeam);
            skill.FreeUse = true;
            skill.PlusHit = true;
            foreach (Skill_Extended ex in skill.AllExtendeds)
            {
                if (ex is S_Eria_RedRed_0)
                {
                    (ex as S_Eria_RedRed_0).Dmg = dmg;
                    ex.SkillBasePlus.Target_BaseDMG = dmg;
                }
            }
            this.BChar.ParticleOut(this.MySkill, skill, alive.Random(this.BChar.GetRandomClass().Main));
            yield break;
        }
    }

    // Femme Fatale additional attack: one hit per debuff stack
    public class S_Eria_RedRed_0 : Skill_Extended
    {
        public int Dmg = 0;

        public override void Init()
        {
            base.Init();
            // The effect has no Attack scaling (DMG_Per 0), so mark it as an attack or the hit deals nothing
            this.IsDamage = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.SkillBasePlus.Target_BaseDMG = Dmg;
        }

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            this.SkillBasePlus.Target_BaseDMG = Dmg;
        }
    }

    // Putréfaction
    public class S_Eria_RedBlue : Skill_Extended
    {
    }

    // Le Soleil
    public class S_Eria_RedYellow : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            BattleSystem.instance.AllyTeam.AP += 2;
            BattleSystem.instance.AllyTeam.Draw(2);
            BattleSystem.instance.AllyTeam.LucyChar.BuffAdd("B_Eria_RedYellow", this.BChar);
        }
    }

    // Glacier
    public class S_Eria_BlueBlue : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            foreach (Buff buff in Targets[0].Buffs)
            {
                if (buff.BuffData.Debuff || buff.BuffData.Hide || buff.TimeUseless || buff.DestroyBuff)
                {
                    continue;
                }
                foreach (StackBuff stack in buff.StackInfo)
                {
                    stack.RemainTime += 2;
                }
            }
        }
    }

    // Forêt Éternelle
    public class S_Eria_BlueYellow : Skill_Extended
    {
    }

    // Paradis des Fous
    public class S_Eria_YellowYellow : Skill_Extended
    {
        public BattleChar TargetTemp;

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            TargetTemp = Targets[0];
            int num = Targets[0].Recovery - Targets[0].HP;
            Targets[0].Heal(this.BChar, (float)num, false, false, null);

            List<Skill> list = new List<Skill>();
            list.Add(Skill.TempSkill("S_Eria_YellowYellow_0", this.BChar, this.BChar.MyTeam));
            list.Add(Skill.TempSkill("S_Eria_YellowYellow_1", this.BChar, this.BChar.MyTeam));
            BattleSystem.instance.EffectDelays.Enqueue(BattleSystem.I_OtherSkillSelect(list, new SkillButton.SkillClickDel(this.Choose), ScriptLocalization.System_SkillSelect.EffectSelect, false, false, true, false, true));
        }

        public void Choose(SkillButton mybutton)
        {
            if (mybutton.Myskill.MySkill.KeyID == "S_Eria_YellowYellow_0")
            {
                TargetTemp.BuffAdd("B_Eria_YellowYellow", this.BChar);
            }
            else
            {
                InventoryManager.Reward(ItemBase.GetItem(GDEItemKeys.Item_Misc_Gold, 10));
            }
        }
    }
    // Paradis des Fous choices (handled by S_Eria_YellowYellow)
    public class S_Eria_YellowYellow_0 : Skill_Extended
    {
    }
    public class S_Eria_YellowYellow_1 : Skill_Extended
    {
    }

    // Soleil Mort
    public class S_Eria_RedBlueYellow : Skill_Extended
    {
    }

    public class S_Eria_X : Skill_Extended
    {
        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

        }
    }
}
