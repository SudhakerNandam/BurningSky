using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirCraftCombat.EventSystem;
using AirCraftCombat.CoreData;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace AirCraftCombat
{
    public class GameplayUiManager : MonoBehaviour
    {
        [SerializeField] private Image playerHealthBar;
        [SerializeField] private TextMeshProUGUI lblPlayerHealthPercantage;
        [SerializeField] private Image enemyKillBar;
        [SerializeField] private TextMeshProUGUI lblEnemyKillsPercentage;

        [SerializeField] private TextMeshProUGUI lblGameoverHighScore;

        [SerializeField] private GameObject gameoverPannel;

        [SerializeField] private Button btnSpreadPowerUp;
        [SerializeField] private Button btnShiledPowerUp;

        #region Unity Methods

        private void OnEnable()
        {
            AirCraftCombatEventHandler.AddListener(EventID.EVENT_ON_UPDATE_PLAYER_HEALTH, EventOnUdpatePlayerHealth);
            AirCraftCombatEventHandler.AddListener(EventID.EVENT_ON_UPDATE_ENEMY_KILL, EventOnUdpateEnemyKills);
            AirCraftCombatEventHandler.AddListener(EventID.EVENT_ON_GAME_COMPLETE, EventOnGameComplete);
        }

        private void OnDisable()
        {
            AirCraftCombatEventHandler.RemoveListener(EventID.EVENT_ON_UPDATE_PLAYER_HEALTH, EventOnUdpatePlayerHealth);
            AirCraftCombatEventHandler.RemoveListener(EventID.EVENT_ON_UPDATE_ENEMY_KILL, EventOnUdpateEnemyKills);
            AirCraftCombatEventHandler.RemoveListener(EventID.EVENT_ON_GAME_COMPLETE, EventOnGameComplete);
        }

        private void Start()
        {
            Init();
        }

        #endregion

        #region Public Methods

        public void OnSpreadPowerUpButtonClick()
        {
            ToggleBtnSpreadPowerUpInteractable(false);
            AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_SPREAD_POWERUP_BUTTON_CLICK, PowerUp.Spread );
            Invoke("UpdateSpreadPowerInteractable", 10f);
        }

        public void OnShieldPowerUpButtonClick()
        {
            ToggleBtnShiledPowerUpInteractable(false);
            AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_SHIELD_BUTTON_CLICK, PowerUp.Shield);
            Invoke("UpdateShieldPowerInteractable", 10f);
        }

        public void OnHomeButtonClick()
        {
            SceneManager.LoadScene(Config.mainmenuSceneIndex);
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            ToggleGameOverPannel(false);
        }

        private void UpdatePlayerHealth(float damage)
        {
            //Utils.Log("PlayerHealth : " + playerHealthBar.fillAmount + "Damage : " + damage);
            playerHealthBar.fillAmount = damage;
            
            lblPlayerHealthPercantage.text = ((int)(playerHealthBar.fillAmount * 100)).ToString() + "%";
        }

        private void ToggleGameOverPannel(bool flag)
        {
            gameoverPannel.SetActive(flag);
        }

        private void UpdateSpreadPowerInteractable()
        {
            ToggleBtnSpreadPowerUpInteractable(true);
        }

        private void UpdateShieldPowerInteractable()
        {
            ToggleBtnShiledPowerUpInteractable(true);
        }

        private void ToggleBtnSpreadPowerUpInteractable(bool flag)
        {
            btnSpreadPowerUp.interactable = flag;
        }

        private void ToggleBtnShiledPowerUpInteractable(bool flag)
        {
            btnShiledPowerUp.interactable = flag;
        }


        #endregion

        #region GameListners

        private void EventOnUdpatePlayerHealth(object obj)
        {
            UpdatePlayerHealth((float)obj);
        }

        private void EventOnUdpateEnemyKills(object obj)
        {
            Utils.Log("GameplayManager,EventOnUdpateEnemyKills : FillAmount " + (float)obj);
            enemyKillBar.fillAmount = (float)obj;
            lblEnemyKillsPercentage.text = ((int)(enemyKillBar.fillAmount * 100)).ToString() + "%";
        }

        private void EventOnGameComplete(object arg)
        {
            lblGameoverHighScore.text = Config.highestScore.ToString();
            ToggleGameOverPannel(true);
        }

        #endregion
    }
}
