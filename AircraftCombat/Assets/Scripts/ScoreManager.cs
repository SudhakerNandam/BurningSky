using UnityEngine;
using AirCraftCombat.EventSystem;
using AirCraftCombat.CoreData;

namespace AirCraftCombat
{
    public class ScoreManager : MonoBehaviour
    {
        #region Unity Methods

        private void OnEnable()
        {
            AirCraftCombatEventHandler.AddListener(EventID.EVENT_ON_ENEMY_KILL, EventOnEnemyKill);
        }

        private void OnDisable()
        {
            AirCraftCombatEventHandler.RemoveListener(EventID.EVENT_ON_ENEMY_KILL, EventOnEnemyKill);
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// On each enemy kill the scores will update.
        /// </summary>
        private void UpdateScores()
        {
            ++Config.currentScore;
            if (Config.currentScore > Config.highestScore)
                Config.highestScore = Config.currentScore;

            AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_UPDATE_ENEMY_KILL, ((float)Config.currentScore / Config.targetScore));
        }

        private void ShowGameover(PrefabType prefabType)
        {
            if (prefabType == PrefabType.Boss)
            {
                Config.isGameComplete = true;
                UpdateHighestUnlockedLevel();
                PlayerPrefsManager.instance.SetHighScore(Config.highestScore);
                AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_GAME_COMPLETE, null);
            }
        }

        /// <summary>
        /// On Gamecomplete updates highest unlocked level if current level index is equal to highest unlocked level.
        /// </summary>
        private void UpdateHighestUnlockedLevel()
        {
            if (Config.highestUnlockedLevelIndex >= Config.totalLevels - 1)
                return;
            if (Config.currentLevelIndex < Config.highestUnlockedLevelIndex)
                return;

            ++Config.highestUnlockedLevelIndex;
            PlayerPrefsManager.instance.SetHighhestLevelUnlocked(Config.highestUnlockedLevelIndex);
        }

        #endregion

        #region GameListners

        private void EventOnEnemyKill(object arg)
        {
            Utils.Log("ScoreManager,EventOnEnemyKill");
            UpdateScores();
            ShowGameover((PrefabType)arg);
        }

        #endregion
    }

}