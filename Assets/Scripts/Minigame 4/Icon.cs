using UnityEngine;
using System.Collections;

namespace Minigame4
{
    public class Icon : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// true - Toggle на данный момент установлен
        /// </summary>
        public bool value { get { return (_toggle == null) ? false : _toggle.value; } }
        /// <summary>
        /// Имя спрайта
        /// </summary>
        public string spriteName { get { return (_sprite == null) ? "" : _sprite.spriteName; } }
        /// <summary>
        /// Указатель на корректную для иконки тень
        /// </summary>
        public IconShadow correctPair;
        /// <summary>
        /// Указатель на выбранную игроком тень
        /// </summary>
        public IconShadow selectPair;

        private UISprite _sprite;
        private UIToggle _toggle;
        private UIButtonScale _buttonScale;
        private BoxCollider _boxCollider;

        #endregion

        
        void Start()
        {
            _sprite = GetComponent<UISprite>();
            _toggle = GetComponent<UIToggle>();
            _buttonScale = GetComponent<UIButtonScale>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void Reset()
        {
            if (_toggle != null)
                _toggle.value = false;
            if (_boxCollider != null)
                _boxCollider.enabled = true;
            transform.localScale = Vector3.one;
            if (_buttonScale != null)
                _buttonScale.enabled = true;
        }

        /// <summary>
        /// Соединить иконку и тень в пару
        /// </summary>
        /// <param name="shadow">Указатель на тень</param>
        public void SetPair(IconShadow shadow)
        {
            selectPair = shadow;

            bool b = (selectPair == null) ? true : false;
            if (_toggle != null)
                _toggle.value = !b;
            if (_boxCollider != null)
                _boxCollider.enabled = b;
            if (_buttonScale != null)
                _buttonScale.enabled = b;
            transform.localScale = Vector3.one;
        }

        public void ChangeToggleValue()
        {
            if (MiniGame4_Manager.instance.isPlay)
                if (_toggle != null)
                    MiniGame4_Manager.instance.ClickToggleIcon(this);
        }
    }
}