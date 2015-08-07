using UnityEngine;
using System.Collections;

namespace Minigame24
{
    public class Cell : MonoBehaviour
    {
        #region variables

        public bool isStatic = false;
        public Resource currentResource;
        public Cell cellUp, cellRight, cellDown, cellLeft;

        #endregion

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!MiniGame24_Manager.instance.isPlay || isStatic)
                return;

            if (UICamera.hoveredObject == gameObject)
            {
                // Поднимаем ресурс с клетки, либо ложим в нее новый, если клетка пуста
                if (Input.GetMouseButtonDown(0))
                {
                    if (currentResource == null)
                        MiniGame24_Manager.instance.SetNewResource(this);
                    else
                        MiniGame24_Manager.instance.SetCurrentResource(currentResource);
                }
                // Перемещаем ресурс в клетку, если он был поднят ранее и клетка пуста
                else if (Input.GetMouseButton(0) && currentResource == null && MiniGame24_Manager.instance.currentResource != null)
                {
                    MiniGame24_Manager.instance.SetResourceToCell(this, MiniGame24_Manager.instance.currentResource);
                }
                else if (Input.GetMouseButtonDown(1) && currentResource != null)
                {
                    currentResource.Rotate();
                    MiniGame24_Manager.instance.CheckWin();
                }

                //else if (Input.GetMouseButtonUp(0) && isEmptyCard && MiniGame6_Manager.instance.currentCard != null)
                //{
                //    Card c = MiniGame6_Manager.instance.currentCard;
                //    if ((Mathf.Abs(c.transform.localPosition.x - transform.localPosition.x) < 1 && (Mathf.Abs(c.transform.localPosition.y - transform.localPosition.y) < 150)) ||
                //       (Mathf.Abs(c.transform.localPosition.y - transform.localPosition.y) < 1 && (Mathf.Abs(c.transform.localPosition.x - transform.localPosition.x) < 150)))
                //    {
                //        Vector3 v = transform.localPosition;
                //        transform.localPosition = c.transform.localPosition;
                //        c.transform.localPosition = v;
                //        MiniGame6_Manager.instance.CheckWin();
                //    }
                //    MiniGame6_Manager.instance.SetCurrentCard(null);
                //}
            }
        }
    }
}