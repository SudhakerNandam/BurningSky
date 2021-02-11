using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AirCraftCombat
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private bool BillboardX = true;
        [SerializeField] private bool BillboardY = true;
        [SerializeField] private bool BillboardZ = true;
        [SerializeField] private float OffsetToCamera;

        [SerializeField] private Image healthBar;

        protected Vector3 localStartPosition;

        private Camera mainCamera;

        #region UnityMethods

        private void  Start()
        {
            mainCamera = Camera.main;
            localStartPosition = transform.localPosition;
        }

        private void FixedUpdate()
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                                                               mainCamera.transform.rotation * Vector3.up);
            if (!BillboardX || !BillboardY || !BillboardZ)
                transform.rotation = Quaternion.Euler(BillboardX ? transform.rotation.eulerAngles.x : 0f, BillboardY ? transform.rotation.eulerAngles.y : 0f, BillboardZ ? transform.rotation.eulerAngles.z : 0f);
            transform.localPosition = localStartPosition;
            transform.position = transform.position + transform.rotation * Vector3.forward * OffsetToCamera;
        }

        #endregion

        #region Public Methods

        public void UpdateHealthBar(float damageAmount)
        {
            healthBar.fillAmount = damageAmount;
        }

        public void SetHealthbar(int fillAmount)
        {
            healthBar.fillAmount = fillAmount;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
