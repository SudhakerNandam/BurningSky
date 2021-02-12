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
            CreatePoolItems();
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

        ///<summary>
        ///This will generate random enemy formations until level finishes.
        ///This random formation are taken form fromation data scriptable object.
        ///</summary>
        ///<returns></returns>
        public IEnumerator SpawnWave()
        {
            float speed = Random.Range(levelSpecificData.minEnemySpeed, levelSpecificData.maxEnemySpeed);

            int randomFormationIndex = Random.Range(0, levelSpecificData.formations.Count);
            
            currentFormationType = levelSpecificData.formations[randomFormationIndex];
            //Utils.Log("EnemySpwaner,SpawnWave, currentFormationType : " + currentFormationType + " FormationCount : " + positionsData.formationData[(int)currentFormationType].formationPoints.Count);

            for (int i =0; i < positionsData.formationData[(int)currentFormationType].formationPoints.Count; ++i)
            {
                UpdateFormationEnemyData(Spawn(positionsData.formationData[(int)currentFormationType].prefabType),
                    speed, positionsData.formationData[(int)currentFormationType].formationPoints[i].positions);
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 3.0f));
            SpawnEnemies();
        }


        /// <summary>
        /// This wil return enemy plane. if it is present in the pool,
        /// Otherwise creates one and adds to pool and returns.
        /// </summary>
        /// <param name="type"> </param>
        /// <returns></returns>
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
                            return enemyToReturn = GetEnemy(type, item.objectToPool);
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// On Start it will instantiate enemies and to the pool.
        /// By default it will be Inactive.
        /// </summary>
        private void CreatePoolItems()
        {
            pooleditems = new List<Enemy>();
            foreach (EnemyItem item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    pooleditems.Add(GetEnemy(item.type, item.objectToPool));
                }
            }
        }

        private Vector3 GetStartPositioinForFormation(Formation formation)
        {
            return Vector3.one;
        }

        private Enemy GetEnemy(PrefabType type, GameObject item)
        {
            Enemy behaviour = Instantiate(item, Vector3.zero, Quaternion.identity).GetComponent<Enemy>();

            behaviour.gameObject.SetActive(false);
            behaviour.Type = type;
            pooleditems.Add(behaviour);
            return behaviour;
        }
        /// <summary>
        /// Udpates Enemy enemy speed, formation points and enables the object. 
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="speed"></param>
        /// <param name="formationPoints"></param>
        private void UpdateFormationEnemyData(Enemy enemy, float speed, List<Vector3> formationPoints)
        {
            if (enemy == null)
                return;
            enemy.Init(speed);
            enemy.SetPathPostions(formationPoints);
            enemy.gameObject.SetActive(true);
        }

        /// <summary>
        /// It will generate enemies at random postions at given time intervals till the level complete.
        /// </summary>
        /// <returns></returns>
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
                UpdateFormationEnemyData(Spawn((PrefabType)Random.Range(0, 4)), Random.Range(2,5), postions);
                yield return new WaitForSeconds(levelSpecificData.randomEnemySpwanWaitTime);
            }

            if ((Config.currentScore < Config.targetScore) && !Config.isGameComplete)
                StartCoroutine(SpwanRandomEnemies());
        }


        /// <summary>
        /// It generate boss enemy once target score is achieved
        /// </summary>
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
