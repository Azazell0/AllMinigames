using UnityEngine;
using System.Collections;

namespace Minigame30
{
    public class GarageCell : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Гараж, находящийся в клетке
        /// </summary>
        public Garage garage;

        #endregion


        void Update()
        {
            if (!MiniGame30_Manager.instance.isPlay)
                return;

            if (Input.GetMouseButtonDown(0) && UICamera.hoveredObject == this.gameObject)
                MiniGame30_Manager.instance.ClickCell(this);
        }
    }
}