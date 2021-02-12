using AirCraftCombat.CoreData;
//using AirCraftCombat.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirCraftCombat
{
    [CreateAssetMenu(fileName = "FormationsPositionsData", menuName = "ScriptableObjects/FormationsPositionsDataObject", order = 2)]
    public class FormationsPositionsData : ScriptableObject
    {
        public List<FormationData> formationData = new List<FormationData>();
    }

    [System.Serializable]
    public class FormationData
    {
        public Formation formation;
        public PrefabType prefabType; // Enemy types [ Small, Medium, Large ]
        public List<FormationPoints> formationPoints = new List<FormationPoints>(); 
    }

    /// <summary>
    /// These are enemy formation positions.
    /// Like: Left Corner, Right Corner, Triangle etc
    /// </summary>
    [System.Serializable]
    public class FormationPoints
    {
        public List<Vector3> positions = new List<Vector3>();
    }
}
