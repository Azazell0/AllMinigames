using UnityEngine;
using System;
using System.Collections;

namespace Minigame30
{
    public class Car : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Цвет машины
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
        /// Скорость машины
        /// </summary>
        public float speed { get { return _speed; } }

        /// <summary>
        /// Цвет машины
        /// </summary>
        private MiniGame30_Color _color;
        /// <summary>
        /// Скорость машины
        /// </summary>
        private float _speed = 1f;
        private UISprite _sprite;

        #endregion

        
        void Awake()
        {
            _sprite = GetComponent<UISprite>();
        }

        void Update()
        {
            if (!MiniGame30_Manager.instance.isPlay)
                return;

            transform.localPosition += new Vector3(0f, -_speed * Time.deltaTime, 0f);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!MiniGame30_Manager.instance.isPlay)
                return;

            if (other.name == "EndLine")
            {
                MiniGame30_Manager.instance.CarInTheEndLine(this);
            }
            else
            {
                Garage g = other.GetComponent<Garage>();
                if (g != null)
                    MiniGame30_Manager.instance.CarInTheGarage(this, g);
            }
        }

        /// <summary>
        /// Скрыть машину
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Показать машину
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Установить рандомные настройки машины (Цвет)
        /// </summary>
        /// <param name="Speed">Скорость движения (Speed >= 0)</param>
        public void SetRandomSettings(float Speed = 1f)
        {
            _speed = Mathf.Abs(Speed);
            color = (MiniGame30_Color)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(MiniGame30_Color)).Length));
        }

        /// <summary>
        /// Обновляет спрайт машины в соответствии с цветом машины
        /// </summary>
        private void UpdateSprite()
        {
            if (_sprite == null)
                return;
            switch(color)
            {
                case MiniGame30_Color.Black:
                    _sprite.spriteName = "30_car_black";
                    break;
                case MiniGame30_Color.Red:
                    _sprite.spriteName = "30_car_red";
                    break;
                case MiniGame30_Color.Green:
                    _sprite.spriteName = "30_car_green";
                    break;
                case MiniGame30_Color.Yellow:
                    _sprite.spriteName = "30_car_yellow";
                    break;
            }
        }
    }

    public enum MiniGame30_Color { Black, Red, Green, Yellow}
}