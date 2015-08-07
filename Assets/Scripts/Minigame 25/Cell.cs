using UnityEngine;
using System;
using System.Collections;

namespace Minigame25
{
    public class Cell : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Находится ли в клетке здание
        /// </summary>
        public bool isHome = false;
        /// <summary>
        /// Установленный в доме WiFi
        /// </summary>
        public WiFiPoint pointWiFi;

        /// <summary>
        /// Корректна ли клетка
        /// </summary>
        public bool isCorrect { get { return _correctCell; } }
        /// <summary>
        /// Позиция клетки по координате X
        /// </summary>
        public int positionX { get { return _positionX; } }
        /// <summary>
        /// Позиция клетки по координате Y
        /// </summary>
        public int positionY { get { return _positionY; } }

        /// <summary>
        /// Корректна ли клетка
        /// </summary>
        private bool _correctCell = false;
        /// <summary>
        /// Позиция клетки по координате X
        /// </summary>
        private int _positionX = -1;
        /// <summary>
        /// Позиция клетки по координате Y
        /// </summary>
        private int _positionY = -1;

        #endregion


        void Start()
        {
            Init();
        }

        void OnMouseDown()
        {
            if (!MiniGame25_Manager.instance.isPlay || !isHome)
                return;

            // Если в ячейке нет станции, устанавливаем новую, если не достигли максимума
            if (pointWiFi == null)
            {
                WiFiPoint p = MiniGame25_Manager.instance.GetNewStation();
                if (p != null)
                    SetWiFiPointToCell(p);
            }
            // Иначе начинаем перемещать имеющуюся станцию
            else
            {
                MiniGame25_Manager.instance.targetPoint = pointWiFi;
            }
        }

        void OnMouseUp()
        {
            if (!MiniGame25_Manager.instance.isPlay || !isHome)
                return;

            // Проверяем условия победы
            //if (MiniGame25_Manager.instance.targetPoint == pointWiFi)
            MiniGame25_Manager.instance.targetPoint = null;
            MiniGame25_Manager.instance.CheckWin();
        }

        void OnMouseEnter()
        {
            if (!MiniGame25_Manager.instance.isPlay || !isHome || pointWiFi != null)
                return;

            // Перемещение станции в новую клетку
            if (Input.GetMouseButton(0) && MiniGame25_Manager.instance.targetPoint != null)
                SetWiFiPointToCell(MiniGame25_Manager.instance.targetPoint);
        }

        /// <summary>
        /// Инициализация клетки - парсинг имени GameObject'a и получение координат
        /// </summary>
        private void Init()
        {
            try
            {
                int index1 = name.IndexOf('(');
                int index2 = name.IndexOf(')');
                int index3 = name.LastIndexOf('(');
                int index4 = name.LastIndexOf(')');

                int length1 = index2 - 1 - index1;
                int length2 = index4 - 1 - index3;
                if (length1 <= 0 || length2 <= 0)
                    return;

                _positionX = Convert.ToInt32(name.Substring(index1 + 1, length1));
                _positionY = Convert.ToInt32(name.Substring(index3 + 1, length2));

                _correctCell = true;
                if (isHome)
                    MiniGame25_Manager.instance.IamCellWithHome(this);
            }
            catch
            {
                Debug.LogWarning("Incorrect cell");
            }
        }

        /// <summary>
        /// Устанавливает связь между станцией WiFi и ячейкой карты
        /// </summary>
        /// <param name="point">Указатель на станцию WiFi</param>
        private void SetWiFiPointToCell(WiFiPoint point)
        {
            if (point == null)
                return;

            if (point.targetCell != null)
                point.targetCell.pointWiFi = null;

            Transform t = point.transform;
            t.position = new Vector3(transform.position.x, transform.position.y, t.position.z);
            pointWiFi = point;
            point.targetCell = this;
        }

        /// <summary>
        /// Устанавливает связь между станцией WiFi и ячейкой карты
        /// </summary>
        /// <param name="point">Указатель на станцию WiFi</param>
        private void SetWiFiPointToCell(GameObject point)
        {
            if (point == null)
                return;

            WiFiPoint p = point.GetComponent<WiFiPoint>();
            if (p != null)
                SetWiFiPointToCell(p);
        }
    }
}