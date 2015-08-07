using UnityEngine;
using System.Collections;

namespace Minigame30
{
    public class Garage : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Цвет гаража
        /// </summary>
        public MiniGame30_Color color
        {
            get { return _color; }
            set
            {
                _color = value;
                UpdateSprite();
            }
        }

        /// <summary>
        /// Цвет гаража
        /// </summary>
        protected MiniGame30_Color _color;
        protected UISprite _sprite;

        #endregion


        void Awake()
        {
            _sprite = GetComponent<UISprite>();
        }

        /// <summary>
        /// Установка нового цвета гаража
        /// </summary>
        /// <param name="Color">Новый цвет</param>
        public void SetColor(MiniGame30_Color Color)
        {
            if (color != Color)
                color = Color;
        }

        /// <summary>
        /// Скрыть гараж
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Показать гараж
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Обновляет спрайт машины в соответствии с цветом гаража
        /// </summary>
        private void UpdateSprite()
        {
            if (_sprite == null)
                return;
            switch (color)
            {
                case MiniGame30_Color.Black:
                    _sprite.spriteName = "30_garage_black";
                    break;
                case MiniGame30_Color.Red:
                    _sprite.spriteName = "30_garage_red";
                    break;
                case MiniGame30_Color.Green:
                    _sprite.spriteName = "30_garage_green";
                    break;
                case MiniGame30_Color.Yellow:
                    _sprite.spriteName = "30_garage_yellow";
                    break;
            }
        }
    }
}