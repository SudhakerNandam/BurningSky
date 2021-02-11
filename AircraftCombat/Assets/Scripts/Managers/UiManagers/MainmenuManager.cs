using UnityEngine;
using TMPro;

namespace AirCraftCombat
{
    public class MainmenuManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lblHighScore;

        #region Unity Methods

        private void Start()
        {
            Init();
        }

        #endregion

        #region Public Methods

       

        #endregion

        #region Private Methods

        private void Init()
        {
            UpdateHighScoreLbl();
        }

        private void UpdateHighScoreLbl()
        {
            Config.highestScore = PlayerPrefsManager.instance.GetHighScore();
            lblHighScore.text = Config.highestScore.ToString();
        }

        #endregion
    }
}
