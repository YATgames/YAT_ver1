namespace Assets.Scripts.UI
{
	public enum PopupStyle
	{
		None = 0,  //없음

        // Popup Base - Scene 개념
        Splash,
        Lobby,
        Login,
        CreateUser,
        Inventory,
        Custom,
        Settings,
        Combine,
        NFC,
        Achievement,

        // Popup Sub - Alert 형태 팝업
        TopUI,
        DisplayInven,
        AchieveSynergyInfo,
        AchieveReward,
        BottomUI,
        InventoryExplain,
        CreateProduction,
        CaseInven,
        CombineExplain,
        DailyCheck,
    }
}
