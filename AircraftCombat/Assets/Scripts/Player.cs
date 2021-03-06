﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirCraftCombat.EventSystem;
using AirCraftCombat.CoreData;

namespace AirCraftCombat
{
    public class Player : MonoBehaviour, IHealth
    {
        public GameObject bulletPrefab;
        [SerializeField] private Transform[] shotPoint1;
        [SerializeField] private float screenWidthOffset = 0.5f;
        [SerializeField] private float screenHeightOffset = 0.5f;
        [SerializeField] private float multiplier = 8f;
        [SerializeField] private GameObject shieldPowerUpObj;
        [SerializeField] private ParticleSystem damageParticle;

        private Rigidbody rgBody = null;
        private Camera mainCamera = null;

        private Vector3 directionVector = Vector3.zero;

        private float fireRate = 0.1f;
        private float lastShotTime = 0f;

        private float health = 100f;

        private float currentHealth = 100f;

        private bool isSpreadPowerupActivated = false;
        private bool isShieldPowerUpActivated = false;

        #region Unity Methods

        private void OnEnable()
        {
            AirCraftCombatEventHandler.AddListener(EventID.EVENT_ON_PLAYERINPUT_DETECTED, EventOnPlayerInputDetected);
            AirCraftCombatEventHandler.AddListener(EventID.EVENT_ON_SPREAD_POWERUP_BUTTON_CLICK, EventOnSpreadPowerButtonClick);
            AirCraftCombatEventHandler.AddListener(EventID.EVENT_ON_SHIELD_BUTTON_CLICK, EventOnShieldPowerButtonClick);
        }

        private void OnDisable()
        {
            AirCraftCombatEventHandler.RemoveListener(EventID.EVENT_ON_PLAYERINPUT_DETECTED, EventOnPlayerInputDetected);
            AirCraftCombatEventHandler.RemoveListener(EventID.EVENT_ON_SPREAD_POWERUP_BUTTON_CLICK, EventOnSpreadPowerButtonClick);
            AirCraftCombatEventHandler.RemoveListener(EventID.EVENT_ON_SHIELD_BUTTON_CLICK, EventOnShieldPowerButtonClick);
        }

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if((Time.time -lastShotTime > fireRate) && !Config.isGameComplete)
            {
                Shoot();
                lastShotTime = Time.time;
            }
            UpdatePlayerPosition();
        }

        private void LateUpdate()
        {
            AdjustPlayerPosition();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Config.isGameComplete)
                return;

            if(other.tag == AircraftCombatStrings.EnemyBulletTag)
            {
                var bullet = other.GetComponent<Bullet>();
                if (bullet && bullet.Type == BulletType.Enemy)
                {
                    TakeBulletDamage(bullet);
                }
            }
            else if(other.tag == AircraftCombatStrings.EnemyPlaneTag)
            {
                var enemy = other.GetComponent<Enemy>();
                if (enemy)
                {
                    TakeEnemyDamage(enemy);
                }
            }
           
        }

        #endregion

        #region Public Methods

        public float GetHealth()
        {
            return 10;
        }

        public void TakeDamage(float damageAmount)
        {
            if (isShieldPowerUpActivated)
                return;
            currentHealth -= damageAmount;
            AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_UPDATE_PLAYER_HEALTH,(currentHealth / health) );
            CheckDamage();
        }

        public void SetHealth()
        {

        }

        #endregion

        #region Private methods

        private void Init()
        {
            mainCamera = Camera.main;
            rgBody = GetComponent<Rigidbody>();
        }

        private void UpdatePlayerPosition()
        {
            if (directionVector != Vector3.zero)
                transform.position = transform.position + directionVector * multiplier;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, directionVector.x * -180);
        }

        private void AdjustPlayerPosition()
        {
            transform.position = new Vector3
                       (
                       Mathf.Clamp(transform.position.x, Config.screenBound.x * -1 + screenWidthOffset, Config.screenBound.x - screenWidthOffset),
                       0f,
                       Mathf.Clamp(transform.position.z, Config.screenBound.z * -1 + screenHeightOffset, Config.screenBound.z - screenHeightOffset)
                       );
        }

        private void Shoot()
        {

            if (isSpreadPowerupActivated)
            {
                SoundManager.instance.PlayerSpreadPowerUpSound();
                ShootBullets(6);
            }
            else
            {
                SoundManager.instance.PlayFireBulletsClip();
                ShootBullets(4);
            }
        }

        private void ShootBullets(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                SpwanBullets(i);
            }
        }

        private void SpwanBullets(int index)
        {
            Bullet newBullet = BulletSpwaner.instance.Spawn(BulletType.Player);
            newBullet.transform.localPosition = new Vector3(shotPoint1[index].transform.position.x, 0f, shotPoint1[index].transform.position.z);
            newBullet.transform.rotation = shotPoint1[index].transform.rotation;
            newBullet.gameObject.SetActive(true);
            newBullet.Fire(newBullet.transform.forward);
        }

        private void DeactivateSpreadPowerUp()
        {
            isSpreadPowerupActivated = false;
        }

        private void DeactivateShieldPowerUp()
        {
            isShieldPowerUpActivated = false;
            ToggleShiledPowerUpObj(false);


        }

        private void CheckDamage()
        {
            if (currentHealth <= 0)
            {
                Config.isGameComplete = true;
                PlayerPrefsManager.instance.SetHighScore(Config.highestScore);
                AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_GAME_COMPLETE, null);
                gameObject.SetActive(false);
            }
        }

        private void ToggleShiledPowerUpObj(bool flag)
        {
            shieldPowerUpObj.SetActive(flag);
        }

        private void TakeBulletDamage(Bullet bullet)
        {
            TakeDamage(bullet.Damage);
            bullet.KillMe();
        }

        private void TakeEnemyDamage(Enemy enemy)
        {
            TakeDamage(enemy.Damage);
            enemy.KillMe();
        }

        #endregion

        #region GameListeners

        private void EventOnPlayerInputDetected(object arg)
        {
            directionVector = (Vector3)arg;
        }

        private void EventOnSpreadPowerButtonClick(object arg)
        {
            isSpreadPowerupActivated = true;
            Invoke("DeactivateSpreadPowerUp", 4f);
        }

        private void EventOnShieldPowerButtonClick(object arg)
        {
            isShieldPowerUpActivated = true;
            ToggleShiledPowerUpObj(true);
            Invoke("DeactivateShieldPowerUp", 4f);
        }

        #endregion
    }
}
