using AirCraftCombat.CoreData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirCraftCombat
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelDataObject", order = 1)]
    public class LevelData : ScriptableObject
    {
        public List<LevelSpecificData> levelData = new List<LevelSpecificData>();
    }

    [System.Serializable]
    public class LevelSpecificData
    {
        public float minEnemySpeed;
        public float maxEnemySpeed;
        public float randomEnemySpwanStartTime;
        public float randomEnemySpwanWaitTime;
        public int randomEnemeisCount;
        public int targetKills;
        public List<Formation> formations = new List<Formation>(); // Diffenet enemy formations
    }
}
