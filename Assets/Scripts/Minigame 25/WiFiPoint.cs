using UnityEngine;
using System.Collections;

namespace Minigame25
{
    public class WiFiPoint : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Радиус действия
        /// </summary>
        public WiFiPointRadius radius;
        /// <summary>
        /// Диаметр действия в клетках
        /// </summary>
        public int diametrInCells = 5;
        /// <summary>
        /// Считать ли покрытие в угловых ячейках
        /// </summary>
        public bool deleteCellInAngle = false;
        /// <summary>
        /// Ячейка, в которой на данный момент находится WiFi
        /// </summary>
        public Cell targetCell;

        #endregion

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool CheckCell(int x, int y)
        {
            if (targetCell != null)
            {
                int rx = Mathf.Abs(targetCell.positionX - x);
                int ry = Mathf.Abs(targetCell.positionY - y);
                int d = (int)(diametrInCells / 2);

                if (rx > d || ry > d || (deleteCellInAngle && rx == d && ry == d))
                    return false;
                return true;
            }

            return false;
        }
    }

    public enum WiFiPointRadius { m500, m900 }
}