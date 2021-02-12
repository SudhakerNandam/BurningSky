using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirCraftCombat.CoreData;
using System.Linq;

namespace AirCraftCombat
{
    [System.Serializable]
    public class ParticleItem
    {
        public GameObject objectToPool;
        public int amountToPool;
        public bool shouldExpand;
        public ParticleType type;
    }

    public class ParticleSpwaner : MonoBehaviour
    {
        public static ParticleSpwaner instance = null;

        public List<ParticleItem> itemsToPool;
        public List<Particle> pooleditems;

        #region Unity Methods

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

        public void SpwanParticle(Vector3 position, ParticleType type)
        {
            Particle particle = Spawn(type);
            particle.SetPosition(position);
            particle.EnableParticle();
        }

        

        #endregion

        #region Private Methods

        /// <summary>
        /// On Start it will instantiate particle gameobjects and to the pool.
        /// By default it will be Inactive.
        /// </summary>
        private void CreatePoolItems()
        {
            pooleditems = new List<Particle>();
            foreach (ParticleItem item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    pooleditems.Add(GetParticle(item.type, item.objectToPool));
                }
            }
        }

        private Particle GetParticle(ParticleType type,GameObject item)
        {
            Particle particle = Instantiate(item, Vector3.zero, Quaternion.identity).GetComponent<Particle>();

            particle.gameObject.SetActive(false);
            particle.Type = type;
            pooleditems.Add(particle);
            return particle;
        }

        /// <summary>
        /// This wil return Particle. if it is present in the pool,
        /// Otherwise creates one and adds to pool and returns.
        /// </summary>
        /// <param name="type"> </param>
        /// <returns></returns>
        private Particle Spawn(ParticleType type)
        {
            Particle particleToReturn = null;
            var items = pooleditems.Where(x => x.Type == type && !x.gameObject.activeInHierarchy);

            if (items.Count() > 0)
            {
                return items.First();
            }
            else
            {
                foreach (ParticleItem item in itemsToPool)
                {
                    if (item.type == type)
                    {
                        if (item.shouldExpand)
                        {
                            return particleToReturn = GetParticle(type, item.objectToPool);
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }

}