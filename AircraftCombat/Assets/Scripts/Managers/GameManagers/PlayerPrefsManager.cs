using UnityEngine;

namespace AirCraftCombat
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        public static PlayerPrefsManager instance = null;

        #region Unity Methods

        public void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void SetHighScore(int highScore)
        {
            PlayerPrefs.SetInt(Keys.KEY_HIGH_SCORE, highScore);
        }

        public int GetHighScore()
        {
            return PlayerPrefs.GetInt(Keys.KEY_HIGH_SCORE, 0);
        }

        public void SetHighhestLevelUnlocked(int highestUnlockedLevel)
        {
            PlayerPrefs.SetInt(Keys.KEY_HIGHEST_LEVEL_UNLOCKED, highestUnlockedLevel);
        }

        public int GetHighestLevelUnlocked()
        {
            return PlayerPrefs.GetInt(Keys.KEY_HIGHEST_LEVEL_UNLOCKED, 0);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
