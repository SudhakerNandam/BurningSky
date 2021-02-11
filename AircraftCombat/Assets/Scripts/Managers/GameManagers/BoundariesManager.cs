using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirCraftCombat
{
    public class BoundariesManager : MonoBehaviour
    {
        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            //Utils.Log(other.gameObject.name);
            var bullet = other.GetComponent<Bullet>();

            bullet?.KillMe();
        }

        #endregion

        #region Public Methods

        #endregion

        #region Privte Methods

        #endregion
    }
}
