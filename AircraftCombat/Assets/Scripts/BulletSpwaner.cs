﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirCraftCombat.CoreData;

namespace AirCraftCombat
{
    [System.Serializable]
    public class BulletItem
    {
        public GameObject objectToPool;
        public int amountToPool;
        public bool shouldExpand;
        public BulletType type;
    }

    public class BulletSpwaner : MonoBehaviour
    {
        public static BulletSpwaner instance = null;

        public List<BulletItem> itemsToPool;
        public List<Bullet> pooleditems;

        #region Unity methods

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            CreatePoolItems();
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// This wil return Bullet. if it is present in the pool,
        /// Otherwise creates one and adds to pool and returns.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Bullet Spawn(BulletType type)
        {
            var items = pooleditems.Where(x => x.Type == type && !x.gameObject.activeInHierarchy);

            if (items.Count() > 0)
            {
                return items.First();
            }
            else
            {
                foreach (BulletItem item in itemsToPool)
                {
                    if (item.type == type)
                    {
                        if (item.shouldExpand)
                        {
                            return GetBulletBehaviour(type, item.objectToPool);
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// On Start it will instantiate bullets and to the pool.
        /// By default it will be Inactive.
        /// </summary>
        private void CreatePoolItems()
        {
            pooleditems = new List<Bullet>();
            foreach (BulletItem item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    pooleditems.Add(GetBulletBehaviour(item.type, item.objectToPool));
                }
            }
        }

        private Bullet GetBulletBehaviour(BulletType type, GameObject item)
        {
            Bullet behaviour = Instantiate(item, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();

            behaviour.gameObject.SetActive(false);
            behaviour.Type = type;
            pooleditems.Add(behaviour);
            return behaviour;
        }

        #endregion
    }
}
