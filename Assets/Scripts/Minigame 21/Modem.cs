using UnityEngine;
using System.Collections;

namespace Minigame21
{
    public class Modem : MonoBehaviour
    {
        #region variables

        private UIButton _button;
        private UIImageButton _imageButton;
        private bool _isInit = false;

        #endregion


        void Start()
        {
            Init();
        }

        private void Init()
        {
            if (_isInit)
                return;

            _button = GetComponent<UIButton>();
            _imageButton = GetComponent<UIImageButton>();
            _isInit = true;
        }

        public void Click()
        {
            MiniGame21_Manager.instance.ClickModem(this);
        }

        public void Reset()
        {
            Init();
            SetNormalMode();
        }

        public void SetNormalMode()
        {
            Init();
            if (_imageButton != null)
            {
                _imageButton.normalSprite = _imageButton.hoverSprite = "router";
                _imageButton.pressedSprite = "router_green";
            }
        }

        public void SetGreenMode()
        {
            Init();
            if (_imageButton != null)
            {
                _imageButton.normalSprite = _imageButton.pressedSprite = "router_green";
                _imageButton.hoverSprite = "router";
            }
        }

        public void SetRedMode()
        {
            Init();
            if (_imageButton != null)
            {
                _imageButton.normalSprite = _imageButton.pressedSprite = _imageButton.hoverSprite = "router_red";
            }
        }
    }
}