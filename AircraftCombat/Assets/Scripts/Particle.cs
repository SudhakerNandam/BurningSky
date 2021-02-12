using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirCraftCombat.CoreData;

namespace AirCraftCombat
{
    public class Particle : MonoBehaviour
    {
        public ParticleType particleType;

        public ParticleType Type { get { return particleType; } set { particleType = value; } }

        #region Unity Methods

        #endregion

        #region Public Methods

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void EnableParticle()
        {
            ToggleParticle(true);
            Invoke("DisableParticle", 1f);
        }

        #endregion

        #region Private Methods

        private void DisableParticle()
        {
            ToggleParticle(false);
        }

        private void ToggleParticle(bool flag)
        {
            gameObject.SetActive(flag);
        }

        #endregion
    }

}