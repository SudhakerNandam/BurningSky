using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AirCraftCombat
{
    public class LevelMapManager : MonoBehaviour
    {
        [SerializeField] Button[] levelMapButtons;

        #region Unity Methods

        private void Start()
        {
            Init();
        }

        #endregion

        #region Public Methods

        public void OnLevelButtonClick(int levelIndex)
        {
            Config.currentLevelIndex = levelIndex;
            SceneManager.LoadScene(Config.gameplaySceneIndex);
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            Config.highestUnlockedLevelIndex = PlayerPrefsManager.instance.GetHighestLevelUnlocked();
            UpdateLevelButtonState();
        }

        private void UpdateLevelButtonState()
        {
            for(int i = 0; i <= Config.highestUnlockedLevelIndex; ++i)
            {
                levelMapButtons[i].interactable = true;
            }
        }

        #endregion
    }
}
