using ChronoArkMod;
namespace ExpertPlusMod
{
    public static class ModItemKeys
    {
		/// <summary>
		/// B_Mist4Buff
		/// </summary>
        public static string Buff_B_Mist4Buff = "B_Mist4Buff";
		/// <summary>
		/// 난폭한 광대
		/// </summary>
        public static string Enemy_S2_Pierrot_Bat2 = "S2_Pierrot_Bat2";
		/// <summary>
		/// 프로그램 마스터
		/// </summary>
        public static string Enemy_ProgramMaster = "ProgramMaster";
		/// <summary>
		/// 전기 술사
		/// </summary>
        public static string Enemy_S4_Guard_0_Solo = "S4_Guard_0_Solo";
		/// <summary>
		/// 화염 술사
		/// </summary>
        public static string Enemy_S4_Guard_1_Solo = "S4_Guard_1_Solo";
		/// <summary>
		/// 냉기 술사
		/// </summary>
        public static string Enemy_S4_Guard_2_Solo = "S4_Guard_2_Solo";
		/// <summary>
		/// 켈베로스
		/// </summary>
        public static string Enemy_MBoss_0_R = "MBoss_0_R";
        public static string SkillEffect_SE_FantasyBuff = "SE_FantasyBuff";
		/// <summary>
		/// 견습 메이드
		/// </summary>
        public static string Enemy_UP_LittleMaid = "UP_LittleMaid";
		/// <summary>
		/// 파로스 교단 마법사
		/// </summary>
        public static string Enemy_UP_Pharos_Mage = "UP_Pharos_Mage";
		/// <summary>
		/// 태엽 인형
		/// </summary>
        public static string Enemy_UP_DochiDoll = "UP_DochiDoll";
		/// <summary>
		/// 돌진하는 목마
		/// </summary>
        public static string Enemy_UP_Horse = "UP_Horse";
		/// <summary>
		/// 전기도치
		/// </summary>
        public static string Enemy_UP_AngryDochi = "UP_AngryDochi";
		/// <summary>
		/// 황야 무법자
		/// </summary>
        public static string Enemy_DOWN_Outlaw = "DOWN_Outlaw";
		/// <summary>
		/// 불도치
		/// </summary>
        public static string Enemy_DOWN_MagicDochi = "DOWN_MagicDochi";
		/// <summary>
		/// 파로스 교단 대사제
		/// </summary>
        public static string Enemy_DOWN_Pharos_HighPriest = "DOWN_Pharos_HighPriest";

    }

    public static class ModLocalization
    {
		/// <summary>
		/// Korean:
		/// 낮은 확률로 적이 같은 스테이지나 다음 스테이지의 적으로 변이합니다.
		/// 저주받은 몹이나 보스 전투에서는 발동하지 않습니다.
		/// English:
		/// Enemies gain a low chance to mutate into enemies from the same stage or next stage.
		/// Does not trigger for Cursed mobs or Boss fights.
		/// Japanese:
		/// 低い確率で敵が同じステージや次のステージの敵に変化します。
		/// 呪われた敵は変化しません。
		/// Chinese:
		/// 敌人有小概率变异为同阶段或下阶段的敌人。
		/// 被诅咒的敌人没有改变。
		/// Chinese-TW:
		/// 敵人有小機率變異為同階段或下階段的敵人。
		/// 被诅咒的敌人没有改变。
		/// </summary>
        public static string ExpertPlusModChaosModeDesc => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/ChaosMode/Desc");
		/// <summary>
		/// Korean:
		/// 혼돈 모드
		/// English:
		/// Chaos Mode
		/// Japanese:
		/// カオスモード
		/// Chinese:
		/// 混沌模式
		/// Chinese-TW:
		/// 混沌模式
		/// </summary>
        public static string ExpertPlusModChaosModeDisplay => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/ChaosMode/Display");
		/// <summary>
		/// Korean:
		/// 보스에게 저주를 부여하며, 저주받은 보스 처치 시 추가 보상을 얻음.
		/// English:
		/// Curses bosses. Gain additional rewards from defeating cursed bosses.
		/// Japanese:
		/// ボスに呪いを与え、呪われたボスを倒すと追加の報酬が得られます。
		/// Chinese:
		/// 对BOSS造成诅咒，击败被诅咒的BOSS后可获得额外奖励。
		/// Chinese-TW:
		/// 對BOSS造成詛咒，擊敗被詛咒的BOSS後可獲得額外獎勵。
		/// </summary>
        public static string ExpertPlusModCursedBossesDesc => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/CursedBosses/Desc");
		/// <summary>
		/// Korean:
		/// 저주받은 보스
		/// English:
		/// Cursed Bosses
		/// Japanese:
		/// 呪われたボス
		/// Chinese:
		/// 诅咒BOSS
		/// Chinese-TW:
		/// 詛咒BOSS
		/// </summary>
        public static string ExpertPlusModCursedBossesDisplay => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/CursedBosses/Display");
		/// <summary>
		/// Korean:
		/// 현재 숙련 난이도가 너무 쉽다고 느껴지시는 분들을 위해 만든 모드입니다.
		/// English:
		/// This mod is intended to serve as a higher difficulty for players who find the current Expert Mode too easy. 
		/// Japanese:
		/// このMODはエキスパートよりもっと難易度が高くなります。エキスパートは簡単だと思う人向けに作られています。
		/// Chinese:
		/// 该mod为那些认为现在的专家难度mod太简单的玩家们提供.
		/// Chinese-TW:
		/// 該mod為那些認為現在的專家難度mod太簡單的玩家們提供.
		/// </summary>
        public static string ExpertPlusModDescription => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/Description");
		/// <summary>
		/// Korean:
		/// 경고: 매우 어려움.
		/// 1. 전투 내 저주 해제 스크롤 사용 불가.
		/// 2. 안개정원1 이후, 해당 스테이지에서 나오는 모든 보스를 한 전투 안에 차례대로 싸워야 함. 결투사 고도, 잊혀진 왕 난이도 상향.
		/// (대부분 보스 기믹은 보스가 죽으면 사라짐. 혈무4 선택 시 하얀무덤 두번째 싸움에서 잊혀진 왕 소환.)
		/// English:
		/// Warning: Very Difficult.
		/// 1. Lifting Scrolls cannot be used in battle.
		/// 2. After Misty Garden 1, fight all possible bosses for each stage. Godo, TFK, Final Boss are harder. 
		/// (Most boss gimmicks are removed when they die. If you select Blood Mist 4, fight TFK as the second boss fight in White Graveyard.)
		/// Japanese:
		/// 警告：超難しい
		/// －戦闘中：解呪スクロールは使用できません。
		/// －霧の庭園2から全ての登場可能ボスはボス戦に現れます（一つずつ）
		/// （ゴードーと忘れられた王はもっと難しくになります）
		/// （一つずつのボスのギミックは倒された後で解除されます）
		/// Chinese:
		/// 警告：该可选项将导致游戏难度急剧提升
		/// 诅咒解除卷轴无法在战斗中使用
		/// 庭院1-1、血色荒野、圣域之外的Boss战将会连战该地区所有的boss.
		/// 增强戈多、被遗忘的王.
		/// (绝大部分Boss的机制在被击杀后会被移除)
		/// Chinese-TW:
		/// 警告：該可選項將導致遊戲難度急劇提升
		/// 詛咒解除捲軸無法在戰鬥中使用
		/// 庭院1-1、血色荒野、聖域之外的Boss戰將會連戰該地區所有的boss.
		/// 增強戈多、被遺忘的王.
		/// (絕大部分Boss的機制在被擊殺後會被移除)
		/// </summary>
        public static string ExpertPlusModDespairModeDesc => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/DespairMode/Desc");
		/// <summary>
		/// Korean:
		/// 절망 모드
		/// English:
		/// Despair Mode
		/// Japanese:
		/// 絶望
		/// Chinese:
		/// 绝望模式
		/// Chinese-TW:
		/// 絕望模式
		/// </summary>
        public static string ExpertPlusModDespairModeDisplay => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/DespairMode/Display");
		/// <summary>
		/// Korean:
		/// 보스 변경사항을 원하지 않으면 꺼주세요.
		/// English:
		/// Turn off this option if you don't want the boss changes.
		/// Japanese:
		/// ボスを変更したくない場合はこれをオフにします。
		/// Chinese:
		/// 如果您不希望对BOSS进行更改，请禁用此选项。
		/// Chinese-TW:
		/// 如果您不希望對BOSS進行更改，請禁用此選項。
		/// </summary>
        public static string ExpertPlusModEnableBossChangesDesc => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/EnableBossChanges/Desc");
		/// <summary>
		/// Korean:
		/// 보스 변경사항
		/// English:
		/// Boss Changes
		/// Japanese:
		/// ボスの変更
		/// Chinese:
		/// Boss改动
		/// Chinese-TW:
		/// Boss改動
		/// </summary>
        public static string ExpertPlusModEnableBossChangesDisplay => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/EnableBossChanges/Display");
		/// <summary>
		/// Korean:
		/// English:
		/// Masochist Mode!!!
		/// Japanese:
		/// Chinese:
		/// Chinese-TW:
		/// </summary>
        public static string ExpertPlusModExpertPlusPlusDesc => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/ExpertPlusPlus/Desc");
		/// <summary>
		/// Korean:
		/// English:
		/// ExpertPlusPlus
		/// Japanese:
		/// Chinese:
		/// Chinese-TW:
		/// </summary>
        public static string ExpertPlusModExpertPlusPlusDisplay => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/ExpertPlusPlus/Display");
		/// <summary>
		/// Korean:
		/// － 캠프파이어에서 아군 소생 불가. 의료 텐트에서 소생 옵션 사라짐. 황금 빵으로 아군 소생 불가.
		/// － 루시의 목걸이 사용 횟수 1로 감소. 
		/// English:
		/// － Campfires cannot revive allies. Remove revive option in Medical Tent. Golden Bread cannot be used on fallen allies.
		/// － Lucy’s Necklace charge reduced to 1. Make it count.
		/// Japanese:
		/// #NAME?
		/// Chinese:
		/// 篝火无法复活队友. 移除医疗帐篷中的篝火选项. 黄金面包不可对无法战斗的队友使用. 还请珍惜这唯一的复活机会。
		/// Chinese-TW:
		/// 篝火無法復活隊友. 移除醫療帳篷中的篝火選項. 黃金麵包不可對無法戰鬥的隊友使用. 還請珍惜這唯一的複活機會。
		/// </summary>
        public static string ExpertPlusModPermaModeDesc => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/PermaMode/Desc");
		/// <summary>
		/// Korean:
		/// 영구죽음 모드
		/// English:
		/// Permadeath Mode
		/// Japanese:
		/// パーマデス
		/// Chinese:
		/// 解除诅咒
		/// Chinese-TW:
		/// 解除詛咒
		/// </summary>
        public static string ExpertPlusModPermaModeDisplay => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/PermaMode/Display");
		/// <summary>
		/// Korean:
		/// 숙련+ 모드
		/// English:
		/// ExpertPlusMod
		/// Japanese:
		/// エキスパート＋ MOD
		/// Chinese:
		/// 专家难度+ Mod
		/// Chinese-TW:
		/// 專家難度+ Mod
		/// </summary>
        public static string ExpertPlusModTitle => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/Title");
		/// <summary>
		/// Korean:
		/// #NAME?
		/// English:
		/// #NAME?
		/// Japanese:
		/// #NAME?
		/// Chinese:
		/// 恢复诅咒共通效果至游戏本体水平。
		/// 该mod基于被削弱的诅咒共通效果而设计, 但如果你想要在该mod下挑战原版诅咒，请开启此可选项
		/// Chinese-TW:
		/// 恢復詛咒共通效果至遊戲本體水平。
		/// 該mod基於被削弱的詛咒共通效果而設計, 但如果你想要在該mod下挑戰原版詛咒，請開啟此可選項
		/// </summary>
        public static string ExpertPlusModVanillaCursesDesc => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/VanillaCurses/Desc");
		/// <summary>
		/// Korean:
		/// 일반 저주
		/// English:
		/// Vanilla Curses
		/// Japanese:
		/// バニラ呪い
		/// Chinese:
		/// 原版诅咒
		/// Chinese-TW:
		/// 原版詛咒
		/// </summary>
        public static string ExpertPlusModVanillaCursesDisplay => ModManager.getModInfo("ExpertPlusMod").localizationInfo.SystemLocalizationUpdate("ExpertPlusMod/VanillaCurses/Display");

    }
}