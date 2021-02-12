using System.Collections.Generic;
using UnityEngine;
using AirCraftCombat.CoreData;
using AirCraftCombat.EventSystem;

namespace AirCraftCombat
{
    public class Enemy : MonoBehaviour, IHealth
    {
        [SerializeField] private float enemyHealth;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private List<Transform> bulletSpwanPositions;
        [SerializeField] private float damageAmount = 5f;

        private float speed = 1.5f;

        private PrefabType prefabType;
        private float fireRate = 3f;
        private float lastShotTime = 0f;


        private List<Vector3> pathPositions = new List<Vector3>();

        private int currentPathIndex = 0;
        private float currentEnemyHealth = 0f;

        private bool isHealthBarEnabled = false;

        public float Damage { get { return damageAmount; } }
        public PrefabType Type { get { return prefabType; } set { prefabType = value; } }

        #region Unity Methods

        private void Update()
        {
            Shoot();
            UpdatePosition();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsNotInScreenBound())
                return;
            var bullet = other.GetComponent<Bullet>();
            if (bullet && (bullet.Type == BulletType.Player))
            {
                TakeDamage(bullet.Damage);
                SoundManager.instance.PlayTakeDamageClip();
                bullet.KillMe();
            }
        }

        #endregion

        #region Public Methods

        public void Init(float speed)
        {
            currentEnemyHealth = enemyHealth;
            currentPathIndex = 0;
            this.speed = speed;
            healthBar.SetHealthbar(1);
            isHealthBarEnabled = false;
            ToggleHealthBar(false);
        }

        public void SetPathPostions(List<Vector3> pathPositions)
        {
            this.pathPositions.Clear();
            this.pathPositions = pathPositions.GetRange(0, pathPositions.Count);
            transform.position = pathPositions[currentPathIndex];
        }

        public float GetHealth()
        {
            return currentEnemyHealth;
        }

        public void TakeDamage(float damageAmount)
        {
            if(!isHealthBarEnabled)
                ToggleHealthBar(true);
            currentEnemyHealth -= damageAmount;
            //Utils.Log("Enemy,TakeDamage, CurrentEnemyHealth : " + currentEnemyHealth);
            healthBar.UpdateHealthBar(currentEnemyHealth / enemyHealth);
            CheckDamage();
        }

        public void SetHealth()
        {

        }

        public void KillMe()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        private bool IsNotInScreenBound()
        {
            return (transform.position.z < (Config.screenBound.z - 0.5) && (transform.position.x > (-Config.screenBound.x +0.1))) ? false : true;
        }

        private void ToggleHealthBar(bool flag)
        {
            healthBar.gameObject.SetActive(flag);
        }

        private void SpwanBullet()
        {
            for (int i = 0; i < bulletSpwanPositions.Count; ++i)
            {
                Bullet newBullet = BulletSpwaner.instance.Spawn(BulletType.Enemy);
                newBullet.transform.localPosition = new Vector3(bulletSpwanPositions[i].position.x, 0f, bulletSpwanPositions[i].position.z);
                newBullet.transform.rotation = Quaternion.identity;
                newBullet.gameObject.SetActive(true);
                newBullet.Speed = 0.5f;
                newBullet.Fire(LevelManager.instance.GetPlayerBehaviuorPositon() - newBullet.transform.position);
            }
        }

        private void CheckDamage()
        {
            if (currentEnemyHealth <= 0)
            {
                AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_ENEMY_KILL, prefabType);
                SoundManager.instance.PlayerExplosionSound();
                ParticleSpwaner.instance.SpwanParticle(transform.position, ParticleType.Damage);
                KillMe();
            }
        }

        private void Shoot()
        {
            if (!IsNotInScreenBound())
            {
                if (Time.time - lastShotTime > fireRate)
                {
                    SpwanBullet();
                    lastShotTime = Time.time;
                }
            }
        }

        private void UpdatePosition()
        {
            if (currentPathIndex < pathPositions.Count - 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, pathPositions[currentPathIndex + 1], speed * Time.deltaTime);
                transform.LookAt(pathPositions[currentPathIndex + 1]);
                if (Vector3.Distance(transform.position, pathPositions[currentPathIndex + 1]) < 0.5f)
                {
                    currentPathIndex++;
                }
            }
            else if (currentPathIndex == pathPositions.Count - 1)
            {
                if(prefabType != PrefabType.Boss)
                    gameObject.SetActive(false);
            }
        }

        #endregion
    }
}