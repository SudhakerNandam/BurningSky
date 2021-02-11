using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirCraftCombat.CoreData;

namespace AirCraftCombat
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private BulletType bulletType;
        [SerializeField] private float damageAmount = 5f;
        private Rigidbody rb;
        private float startTime = 0f;
        private float distance = 0f;
        private float timeInterval = 0f;

        public float Speed { get { return speed; } set { speed = value;} }
        public float Damage { get { return damageAmount; } }
        public BulletType Type { get { return bulletType;} set { bulletType = value; } }

        #region Unity Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        #endregion

        #region Public Methods

        public void Fire(Vector3 fireDirection)
        {
            rb.velocity = fireDirection * speed;
        }

        public void KillMe()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
