using UnityEngine;
using System.Collections;

namespace Minigame6
{
    public class Card : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Пустая карта
        /// </summary>
        public bool isEmptyCard = false;

        /// <summary>
        /// Начальная позиция карты
        /// </summary>
        public Vector3 startPosition { get { return _startPosition; } }

        /// <summary>
        /// Начальная позиция карты
        /// </summary>
        private Vector3 _startPosition;

        #endregion


        void Awake()
        {
            _startPosition = transform.position;
        }

        void Update()
        {
            if (!MiniGame6_Manager.instance.isPlay)
                return;

            if (UICamera.hoveredObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0) && !isEmptyCard)
                    MiniGame6_Manager.instance.SetCurrentCard(this);
                else if (Input.GetMouseButtonUp(0) && isEmptyCard && MiniGame6_Manager.instance.currentCard != null)
                {
                    Card c = MiniGame6_Manager.instance.currentCard;
                    if ((Mathf.Abs(c.transform.localPosition.x - transform.localPosition.x) < 1 && (Mathf.Abs(c.transform.localPosition.y - transform.localPosition.y) < 150)) ||
                       (Mathf.Abs(c.transform.localPosition.y - transform.localPosition.y) < 1 && (Mathf.Abs(c.transform.localPosition.x - transform.localPosition.x) < 150)))
                    {
                        Vector3 v = transform.localPosition;
                        transform.localPosition = c.transform.localPosition;
                        c.transform.localPosition = v;
                        MiniGame6_Manager.instance.CheckWin();
                    }
                    MiniGame6_Manager.instance.SetCurrentCard(null);
                }
            }
        }
    }
}