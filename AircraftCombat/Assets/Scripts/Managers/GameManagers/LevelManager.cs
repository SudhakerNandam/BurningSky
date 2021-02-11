using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirCraftCombat
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance = null;
        public LevelData levelData;

        [SerializeField] private Player playerBehaviour;
        public Player Player { get { return playerBehaviour; } }

        #region Untiy Methods

        private void Awake()
        {
            instance = this;
            Config.screenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.y));
            Utils.Log("LevelManager,Awake, ScreenBound : " + Config.screenBound);
        }

        private void Start()
        {
            Init();
        }

        #endregion

        #region Public Methods

        public Vector3 GetPlayerBehaviuorPositon()
        {
            return playerBehaviour.transform.position;
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            ResetVars();
            CreateLevel();
        }

        private void ResetVars()
        {
            Config.isGameComplete = false;
            Config.currentScore = 0;
        }

        private void CreateLevel()
        {
            CreateEnemy();
        }

        private void CreateEnemy()
        {
            EnemySpwaner.instance.StartSpawningEnemies(levelData.levelData[Config.currentLevelIndex]);
        }

        #endregion

    }

}