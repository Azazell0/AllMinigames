using UnityEngine;
using System.Collections;

namespace Minigame3
{
    public class Person : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Вес персонажа
        /// </summary>
        public int weight = 0;

        /// <summary>
        /// true - персонаж выбран в данный момент
        /// </summary>
        public bool isSelected { get { return (_toggle != null) ? _toggle.value : false; } }
        /// <summary>
        /// true - персонаж был отправлен в лифт
        /// </summary>
        public bool isFinish { get { return (_sprite != null) ? (_sprite.alpha < 0.5) ? true : false : false; } }

        private UISprite _sprite;
        private UIToggle _toggle;
        private BoxCollider _boxCollider;

        #endregion

        
        void Start()
        {
            _sprite = GetComponent<UISprite>();
            _toggle = GetComponent<UIToggle>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        /// <summary>
        /// Нажатие на персонажа
        /// </summary>
        public void ClickPerson()
        {
            if (MiniGame3_Manager.instance.isPlay)
                MiniGame3_Manager.instance.WasClickPerson(this);
        }

        /// <summary>
        /// Сброс настроек персонажа на первоначальные значения
        /// </summary>
        public void Reset()
        {
            if (_toggle != null)
                _toggle.value = false;
            if (_boxCollider != null)
                _boxCollider.enabled = true;
            if (_sprite != null)
                _sprite.alpha = 1f;
        }

        /// <summary>
        /// Скрывает персонажа
        /// </summary>
        public void Hide()
        {
            StartCoroutine(HideCoroutine());
        }

        /// <summary>
        /// корутина постепенного скрытия персонажа
        /// </summary>
        /// <returns></returns>
        private IEnumerator HideCoroutine()
        {
            if (_boxCollider != null)
                _boxCollider.enabled = false;
            if (_sprite != null)
            {
                while (_sprite.alpha > 0)
                {
                    _sprite.alpha -= 0.05f;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        /// <summary>
        /// Показывает персонажа
        /// </summary>
        public void Show()
        {
            StartCoroutine(ShowCoroutine());
        }

        /// <summary>
        /// корутина постепенного показа персонажа
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowCoroutine()
        {
            if (_boxCollider != null)
                _boxCollider.enabled = true;
            if (_sprite != null)
            {
                while (_sprite.alpha < 1)
                {
                    _sprite.alpha += 0.05f;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}