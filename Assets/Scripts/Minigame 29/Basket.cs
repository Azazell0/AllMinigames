using UnityEngine;
using System.Collections;

namespace Minigame29
{
    public class Basket : MonoBehaviour
    {
        #region variables

        public float xLocalMin = 0;
        public float xLocalMax = 0;

        #endregion


        void Update()
        {
            if (MiniGame29_Manager.instance.isPlay && UICamera.hoveredObject != null)
            {
                transform.position = new Vector3(UICamera.lastHit.point.x, transform.position.y, transform.position.z);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, xLocalMin, xLocalMax), transform.localPosition.y, transform.localPosition.z);
            }
        }
    }
}