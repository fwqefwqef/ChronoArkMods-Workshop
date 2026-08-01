using ChronoArkMod;
namespace RedMist
{
    public static class ModItemKeys
    {
		/// <summary>
		/// Manifest E.G.O
		/// Upon defeating an enemy or dealing 50+ damage with attacks, remove overload and gain +2 Attack Power (Max 10)
		/// If this condition is not met at least once per turn, stun the user (200%) and remove this buff.
		/// Current Damage Dealt: &a
		/// Condition Achieved: &b
		/// </summary>
        public static string Buff_B_RedMist_0 = "B_RedMist_0";
		/// <summary>
		/// Focus Spirit
		/// </summary>
        public static string Buff_B_RedMist_4 = "B_RedMist_4";
		/// <summary>
		/// Ruddled Welts
		/// After each attack, take non-lethal pain damage equal to 33% of max HP. Attack Power increases by half of missing health.
		/// </summary>
        public static string Buff_B_RedMist_6 = "B_RedMist_6";
		/// <summary>
		/// Fragile
		/// </summary>
        public static string Buff_B_RedMist_ArmorDown = "B_RedMist_ArmorDown";
		/// <summary>
		/// Bleed
		/// </summary>
        public static string Buff_B_RedMist_Bleed = "B_RedMist_Bleed";
		/// <summary>
		/// The Red Mist
		/// Passive:
		/// At the start of turn 2, create Manifest E.G.O in hand.
		/// Manifest E.G.O - 0 Cost, Swiftness, Excluded after 1 turn
		/// Infinite turn Buff: 
		/// Upon defeating an enemy or dealing 50+ damage with attacks, remove overload and gain +2 Attack Power (Max 10)
		/// If this condition is not met at least once per turn, stun the user (200%) and remove this buff.
		/// If Manifest E.G.O is inactive, create it in hand at the start of next turn.
		/// </summary>
        public static string Character_RedMist = "RedMist";
        public static string SkillEffect_SE_Tick_B_RedMist_Bleed = "SE_Tick_B_RedMist_Bleed";
        public static string SkillEffect_SE_T_S_RedMist_0 = "SE_T_S_RedMist_0";
        public static string SkillEffect_SE_T_S_RedMist_1 = "SE_T_S_RedMist_1";
        public static string SkillEffect_SE_T_S_RedMist_10 = "SE_T_S_RedMist_10";
        public static string SkillEffect_SE_T_S_RedMist_11 = "SE_T_S_RedMist_11";
        public static string SkillEffect_SE_T_S_RedMist_1_0 = "SE_T_S_RedMist_1_0";
        public static string SkillEffect_SE_T_S_RedMist_2 = "SE_T_S_RedMist_2";
        public static string SkillEffect_SE_T_S_RedMist_3 = "SE_T_S_RedMist_3";
        public static string SkillEffect_SE_T_S_RedMist_4 = "SE_T_S_RedMist_4";
        public static string SkillEffect_SE_T_S_RedMist_5 = "SE_T_S_RedMist_5";
        public static string SkillEffect_SE_T_S_RedMist_6 = "SE_T_S_RedMist_6";
        public static string SkillEffect_SE_T_S_RedMist_7_0 = "SE_T_S_RedMist_7_0";
        public static string SkillEffect_SE_T_S_RedMist_8 = "SE_T_S_RedMist_8";
        public static string SkillEffect_SE_T_S_RedMist_9 = "SE_T_S_RedMist_9";
        public static string SkillEffect_SE_T_S_RedMist_LucyD = "SE_T_S_RedMist_LucyD";
		/// <summary>
		/// Smiling Body
		/// </summary>
        public static string Enemy_SmilingBody = "SmilingBody";
		/// <summary>
		/// Manifest E.G.O
		/// </summary>
        public static string Skill_S_RedMist_0 = "S_RedMist_0";
		/// <summary>
		/// Spear
		/// Recast for each skill played by the user this turn. (Including this skill)
		/// Current skill num: &a
		/// </summary>
        public static string Skill_S_RedMist_1 = "S_RedMist_1";
		/// <summary>
		/// Greater Split: Vertical
		/// </summary>
        public static string Skill_S_RedMist_10 = "S_RedMist_10";
		/// <summary>
		/// Greater Split: Horizontal
		/// Cost is reduced by 1 whenever an enemy is defeated.
		/// </summary>
        public static string Skill_S_RedMist_11 = "S_RedMist_11";
		/// <summary>
		/// Spear
		/// </summary>
        public static string Skill_S_RedMist_1_0 = "S_RedMist_1_0";
		/// <summary>
		/// Upstanding Slash
		/// Draw 1 skill prioritizing the user's skills.
		/// </summary>
        public static string Skill_S_RedMist_2 = "S_RedMist_2";
		/// <summary>
		/// Level Slash
		/// Upon defeating an enemy, restore 3 mana. 
		/// </summary>
        public static string Skill_S_RedMist_3 = "S_RedMist_3";
		/// <summary>
		/// Focus Spirit
		/// Shuffle all skills of this character in the discard pile back into the draw pile, and draw 2 skills prioritizing this character.
		/// </summary>
        public static string Skill_S_RedMist_4 = "S_RedMist_4";
		/// <summary>
		/// Onrush
		/// Upon defeating an enemy, recast this skill on a random enemy.
		/// </summary>
        public static string Skill_S_RedMist_5 = "S_RedMist_5";
		/// <summary>
		/// Ruddled Welts
		/// Take non-lethal Pain damage equal to 33% of max HP. 
		/// </summary>
        public static string Skill_S_RedMist_6 = "S_RedMist_6";
		/// <summary>
		/// Laughter
		/// Spawn a Smiling Body on the map. It has 50HP, takes doubled damage from attacks, and its action reduces the armor of all allies and enemies.
		/// </summary>
        public static string Skill_S_RedMist_7 = "S_RedMist_7";
		/// <summary>
		/// Laughter
		/// </summary>
        public static string Skill_S_RedMist_7_0 = "S_RedMist_7_0";
		/// <summary>
		/// Dipsia
		/// After cast, chain heal 20% Max HP for each enemy hit.
		/// </summary>
        public static string Skill_S_RedMist_8 = "S_RedMist_8";
		/// <summary>
		/// Smile
		/// Upon defeating an enemy, create a copy of this skill in hand.
		/// </summary>
        public static string Skill_S_RedMist_9 = "S_RedMist_9";
		/// <summary>
		/// Prey 
		/// Draw 2 skills.
		/// </summary>
        public static string Skill_S_RedMist_LucyD = "S_RedMist_LucyD";

    }

    public static class ModLocalization
    {

    }
}