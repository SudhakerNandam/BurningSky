using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirCraftCombat
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance = null;

        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip explosionClip;
        [SerializeField] private AudioClip spreadPowerUpClip;
        [SerializeField] private AudioClip damageClip;
        [SerializeField] private AudioClip fireBulletsClip;

        #region Unity Methods

        private void Awake()
        {
            instance = this;
        }

        #endregion

        #region Public Methods

        public void PlayerExplosionSound()
        {
            audioSource.PlayOneShot(explosionClip);
        }

        public void PlayerSpreadPowerUpSound()
        {
            audioSource.PlayOneShot(spreadPowerUpClip);
        }

        public void PlayTakeDamageClip()
        {
            audioSource.PlayOneShot(damageClip);
        }

        public void PlayFireBulletsClip()
        {
            audioSource.PlayOneShot(fireBulletsClip);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
