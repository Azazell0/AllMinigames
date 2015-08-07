using UnityEngine;
using System.Collections;

namespace Minigame23
{
    public class Pugalka : MonoBehaviour
    {
        public Point currentPoint;

        private UISprite _sprite;

        // Use this for initialization
        void Start()
        {
            _sprite = GetComponent<UISprite>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!MiniGame23_Manager.instance.isPlay)
                return;

            if (Input.GetMouseButtonDown(0) && UICamera.hoveredObject == gameObject)
            {
                MiniGame23_Manager.instance.SetCurrentPugalka(this);
            }
            if (Input.GetMouseButtonUp(0) && MiniGame23_Manager.instance.currentPugalka == this)
            {
                MiniGame23_Manager.instance.SetCurrentPugalka(null);
            }
        }

        public void SetTransparency(bool b)
        {
            if (_sprite != null)
                _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, (b) ? 0.5f : 1f);
            if (currentPoint != null)
                currentPoint.isPugalka = !b;
            if (b)
                currentPoint = null;
        }

        public void SetToPoint(Point p)
        {
            currentPoint = p;
            if (currentPoint != null)
                transform.localPosition = currentPoint.transform.localPosition;
            else
                transform.localPosition = new Vector3(-10000, -10000, 0);
        }
    }
}