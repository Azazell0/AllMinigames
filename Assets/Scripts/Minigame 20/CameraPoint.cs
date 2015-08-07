using UnityEngine;
using System.Collections;

namespace Minigame20
{
    public class CameraPoint : MonoBehaviour
    {
        #region variables

        public bool wasFind = false;

        private UISprite _sprite;
        private UIButton _button;

        #endregion

        // Use this for initialization
        void Start()
        {
            _sprite = GetComponent<UISprite>();
            _button = GetComponent<UIButton>();
        }

        //void Update()
        //{
        //    if (!MiniGame20_Manager.instance.isPlay)
        //        return;

        //    if (UICamera.hoveredObject == gameObject && !wasFind)
        //    {
        //        if (_sprite != null)
        //            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0.5f);
        //    }
        //}

        public void Click()
        {
            wasFind = true;
            if (_button != null)
                _button.enabled = false;
            if (_sprite != null)
                _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1);
            MiniGame20_Manager.instance.WasFindCamera();
        }

        /// <summary>
        /// Переустанавливает камеру для новой игры
        /// </summary>
        public void Reset()
        {
            wasFind = false;
            if (_sprite != null)
                _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0f);
            if (_button != null)
            {
                _button.enabled = true;
                _button.defaultColor = new Color(_button.defaultColor.r, _button.defaultColor.g, _button.defaultColor.b, 0f);
            }
        }
    }
}