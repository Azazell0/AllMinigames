using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame23
{
    public class Point : MonoBehaviour
    {
        #region variables

        public List<Point> listPoints;
        public bool isMousetrap = false;
        public bool isCable = false;
        public bool isPugalka = false;

        #endregion

        
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!MiniGame23_Manager.instance.isPlay)
                return;

            if (UICamera.hoveredObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0) && !isMousetrap && ! isCable && !isPugalka)
                {
                    Pugalka p = MiniGame23_Manager.instance.GetNewPugalka();
                    if (p != null)
                    {
                        p.SetToPoint(this);
                    }
                }
                else if (Input.GetMouseButton(0) && MiniGame23_Manager.instance.currentPugalka != null && !isMousetrap && !isCable && !isPugalka)
                {
                    Pugalka p = MiniGame23_Manager.instance.currentPugalka;
                    if (p != null && p.currentPoint != this)
                        MiniGame23_Manager.instance.currentPugalka.SetToPoint(this);
                }
            }
        }
    }
}