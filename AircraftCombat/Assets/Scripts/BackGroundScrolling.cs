using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirCraftCombat
{
    public class BackGroundScrolling : MonoBehaviour
    {
        [SerializeField] private Vector3 startPotion;
        [SerializeField] private Vector3 endPosition;

        private float speed = 1f;
        private float endPos = 0f;

        #region Unity Methods

        private void Start()
        {
            Mesh m = gameObject.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
            Bounds b = m.bounds;
            endPos = b.size.y - 2;
        }

        void Update()
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.z <= -endPos)
                transform.position = startPotion;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
