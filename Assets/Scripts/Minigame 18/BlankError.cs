using UnityEngine;
using System.Collections;

namespace Minigame18
{
    public class BlankError : MonoBehaviour
    {
        #region variables

        public bool wasFind { get { return _wasFind; } }

        private bool _wasFind = false;
        private UISprite _sprite;
        private UIButton _button;

        #endregion

        
        void Start()
        {
            _sprite = GetComponent<UISprite>();
            _button = GetComponent<UIButton>();

            MiniGame18_Manager.instance.RegisteryBlankError(this);
            Reset();
        }

        public void Click()
        {
            _wasFind = true;
            if (_button != null)
                _button.enabled = false;
            if (_sprite != null)
                _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1);
            MiniGame18_Manager.instance.WasFindBlankError();
        }

        /// <summary>
        /// Переустанавливает камеру для новой игры
        /// </summary>
        public void Reset()
        {
            _wasFind = false;
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