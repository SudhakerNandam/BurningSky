using UnityEngine;

namespace AirCraftCombat
{
    /// <summary>
    /// All game related configurations are declared here.
    /// </summary>
    public class Config
    {
        public static int mainmenuSceneIndex = 0;
        public static int gameplaySceneIndex = 1;

        public static int highestUnlockedLevelIndex = 0;
        public static int currentLevelIndex = 0;
        public static Vector3 screenBound = Vector3.zero;

        public static int targetScore = 0;
        public static int currentScore = 0;
        public static int highestScore = 0;

        public static int totalLevels = 5;

        public static bool isGameComplete = false;
    }
}