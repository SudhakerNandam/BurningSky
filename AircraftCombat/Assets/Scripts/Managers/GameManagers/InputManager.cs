using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirCraftCombat.EventSystem;

namespace AirCraftCombat
{
    public class InputManager : MonoBehaviour
    {
        private Camera mainCamera = null;
        private Vector3 startPostion = Vector3.zero;
        private Vector3 endPosition = Vector3.zero;
        private Vector3 direction = Vector3.zero;

        #region Unity Methods

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            PlayerInput();
        }

        #endregion

        #region Public Methods
        

        #endregion

        #region Private Methods

        private void PlayerInput()
        {
            if (Input.GetMouseButtonDown(0))
                UpdateStartingTouchPosition(mainCamera.ScreenToViewportPoint(Input.mousePosition));
            if (Input.GetMouseButton(0))      
            {
                UpdateEndTouchPosition(mainCamera.ScreenToViewportPoint(Input.mousePosition));
                direction = endPosition - startPostion;
                TriggerPlayerInput();
                UpdateStartingTouchPosition(mainCamera.ScreenToViewportPoint(Input.mousePosition));
            }
            if(Input.GetMouseButtonUp(0))
            {
                direction = Vector3.zero;
                TriggerPlayerInput();
            }
        }

        private void UpdateStartingTouchPosition(Vector3 touchPosition)
        {
            startPostion = touchPosition;
            startPostion.z = startPostion.y;
            startPostion.y = 0f;
        }

        private void UpdateEndTouchPosition(Vector3 touchPosition)
        {
            endPosition = touchPosition;
            endPosition.z = endPosition.y;
            endPosition.y = 0f;
        }

        private void TriggerPlayerInput()
        {
            AirCraftCombatEventHandler.TriggerEvent(EventID.EVENT_ON_PLAYERINPUT_DETECTED, direction);
        }

        #endregion
    }
}
