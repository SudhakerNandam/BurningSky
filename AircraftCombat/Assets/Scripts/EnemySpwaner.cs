using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AirCraftCombat.CoreData;

namespace AirCraftCombat
{
    [System.Serializable]
    public class EnemyItem
    {
        public GameObject objectToPool;
        public int amountToPool;
        public bool shouldExpand;
        public PrefabType type;
    }

    public class EnemySpwaner : MonoBehaviour
    {
        public static EnemySpwaner instance = null;

        public List<EnemyItem> itemsToPool;
        public List<Enemy> pooleditems;
        public FormationsPositionsData positionsData;

        public Formation currentFormationType = Formation.Type_1;

        private LevelSpecificData levelSpecificData;

        private float randomEnemySpwanStartTime = 1f;
        private float randomEnemySpwanWaitTime = 2f;

        #region Untiy methods

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            pooleditems = new List<Enemy>();
            foreach (EnemyItem item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    pooleditems.Add(GetEnemyBehaviour(item.type, item.objectToPool));
                }
            }
        }

        #endregion

        #region Public methods
        WaitForSeconds waitOne = new WaitForSeconds(30f);
        WaitForSeconds waitHalf = new WaitForSeconds(1f);

        public void StartSpawningEnemies(LevelSpecificData data)
        {
            levelSpecificData = data;
            Config.targetScore = levelSpecificData.targetKills;
            StartCoroutine(SpwanRandomEnemies());
            SpawnEnemies();
        }

        public void SpawnEnemies()
        {
            if (!Config.isGameComplete)
            {
                if (Config.currentScore < Config.targetScore)
                    StartCoroutine(SpawnWave());
                else
                    SpwanBossEnemy();
            }
        }

        public IEnumerator SpawnWave()
        {
            float speed = Random.Range(levelSpecificData.minEnemySpeed, levelSpecificData.maxEnemySpeed);

            int randomFormationIndex = Random.Range(0, levelSpecificData.formations.Count);
            
            currentFormationType = levelSpecificData.formations[randomFormationIndex];
            //Utils.Log("EnemySpwaner,SpawnWave, currentFormationType : " + currentFormationType + " FormationCount : " + positionsData.formationData[(int)currentFormationType].formationPoints.Count);

            for (int i =0; i < positionsData.formationData[(int)currentFormationType].formationPoints.Count; ++i)
            {
                UpdateFormationEnemy(Spawn(positionsData.formationData[(int)currentFormationType].prefabType),
                    speed, positionsData.formationData[(int)currentFormationType].formationPoints[i].positions);
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 3.0f));
            SpawnEnemies();
        }

        public Enemy Spawn( PrefabType type)
        {
            Enemy enemyToReturn = null;
            var items = pooleditems.Where(x => x.Type == type && !x.gameObject.activeInHierarchy);

            if (items.Count() > 0)
            {
                return items.First();
            }
            else
            {
                foreach (EnemyItem item in itemsToPool)
                {
                    if (item.type == type)
                    {
                        if (item.shouldExpand)
                        {
                            return enemyToReturn = GetEnemyBehaviour(type, item.objectToPool);
                        }
                    }
                }
            }

            return null;
        }
        
        #endregion

        #region Private methods

        private Vector3 GetStartPositioinForFormation(Formation formation)
        {
            return Vector3.one;
        }

        private Enemy GetEnemyBehaviour(PrefabType type, GameObject item)
        {
            Enemy behaviour = Instantiate(item, Vector3.zero, Quaternion.identity).GetComponent<Enemy>();

            behaviour.gameObject.SetActive(false);
            behaviour.Type = type;
            pooleditems.Add(behaviour);
            return behaviour;
        }

        private void UpdateFormationEnemy(Enemy enemy, float speed, List<Vector3> formationPoints)
        {
            if (enemy == null)
                return;
            enemy.Init(speed);
            enemy.SetPathPostions(formationPoints);
            enemy.gameObject.SetActive(true);
        }

        private IEnumerator SpwanRandomEnemies()
        {
            yield return new WaitForSeconds(levelSpecificData.randomEnemySpwanStartTime);
            //Utils.Log("EnemySpwaner, SpwanRandomEnemies");
            List<Vector3> postions = new List<Vector3>();
            for ( int i = 0; i < levelSpecificData.randomEnemeisCount; ++i)
            {
                postions.Clear();
                postions.Add(new Vector3(Random.Range(-Config.screenBound.x, Config.screenBound.x), 0f, Config.screenBound.z));
                postions.Add(new Vector3(postions[0].x, 0f, -Config.screenBound.z));
                SpwanRandomEnemy(Spawn((PrefabType)Random.Range(0, 4)), Random.Range(2,5), postions);
                yield return new WaitForSeconds(levelSpecificData.randomEnemySpwanWaitTime);
            }

            if ((Config.currentScore < Config.targetScore) && !Config.isGameComplete)
                StartCoroutine(SpwanRandomEnemies());
        }

        private void SpwanRandomEnemy(Enemy enemy, float speed, List<Vector3>  positions)
        {
            if (enemy == null)
                return;
            enemy.Init(speed);
            enemy.SetPathPostions(positions);
            enemy.gameObject.SetActive(true);
        }

        private void SpwanBossEnemy()
        {
            Enemy bossEnemy = Spawn(PrefabType.Boss);

            List<Vector3> positions = new List<Vector3>();
            positions.Add(new Vector3(0f, 0f, Config.screenBound.z));
            positions.Add(new Vector3(0, 0f, Config.screenBound.z -2));

            bossEnemy.Init(1f);
            bossEnemy.SetPathPostions(positions);
            bossEnemy.gameObject.SetActive(true);
        }

        #endregion
    }
}
