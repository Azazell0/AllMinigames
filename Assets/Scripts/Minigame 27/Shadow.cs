using UnityEngine;
using System;
using System.Collections;

namespace Minigame27
{
    public class Shadow : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Клетка к которой движется тень
        /// </summary>
        public Cell targetCell;
        /// <summary>
        /// Тип объекта, находящегося в тени
        /// </summary>
        public ObjectType type
        {
            get { return _type; }
            set { _type = value; UpdateSprite(); }
        }

        /// <summary>
        /// Тип объекта, находящегося в тени
        /// </summary>
        private ObjectType _type = ObjectType.o0;
        /// <summary>
        /// true - тень двигается в данный момент
        /// </summary>
        private bool _isActive = false; 
        /// <summary>
        /// Позиция цели, к которой двигается тень
        /// </summary>
        private Vector3 _targetPosition = Vector3.zero;
        /// <summary>
        /// Стартовая позиция, откуда тень начинает движение
        /// </summary>
        private Vector3 _startPosition = Vector3.zero;
        /// <summary>
        /// Спрайт тени
        /// </summary>
        private UISprite _sprite;

        #endregion


        void Awake()
        {
            _sprite = GetComponent<UISprite>();
        }

        void Update()
        {
            // Движение тени
            if (_isActive && targetCell != null)
            {
                Vector3 v = Vector3.zero;
                if (Vector3.Distance(transform.position, _targetPosition) < 0.25f)
                    v = (_targetPosition - transform.position) * Time.deltaTime * 3;
                else
                    v = (_targetPosition - _startPosition) * Time.deltaTime * 0.7f;
                transform.position += v;
                if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
                    Stop();
            }
        }

        /// <summary>
        /// Установка случайного объекта
        /// </summary>
        public void SetRandomType()
        {
            _type = (ObjectType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(ObjectType)).Length);
            UpdateSprite();
        }

        /// <summary>
        /// Начинает движение к клетке
        /// </summary>
        /// <param name="target">Клетка-цель</param>
        public void StartMove(Cell target)
        {
            targetCell = target;
            _targetPosition = targetCell.transform.position;
            _isActive = true;
            _startPosition = transform.position;
            Show();
        }

        /// <summary>
        /// Прекращает движение и говорит менеджеру переместить тень в список неактивных
        /// </summary>
        private void Stop()
        {
            _isActive = false;
            targetCell.SetObject(type);
            targetCell = null;
            _targetPosition = Vector3.zero;
            MiniGame27_Manager.instance.ShadowFinished(this);
            Hide();
        }

        /// <summary>
        /// Скрыть тень
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Показать тень
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Обновление спрайта для отображения корректного объекта в тени
        /// </summary>
        private void UpdateSprite()
        {
            if (_sprite != null)
            {
                string s = "";
                switch (type)
                {
                    case ObjectType.o1:
                        s = "27_lopata";
                        break;

                    case ObjectType.o2:
                        s = "27_lopata2";
                        break;

                    case ObjectType.o3:
                        s = "27_parovoz";
                        break;

                    case ObjectType.o4:
                        s = "27_pesochn1";
                        break;

                    case ObjectType.o5:
                        s = "27_pesochn2";
                        break;

                    case ObjectType.o6:
                        s = "27_pyramid";
                        break;

                    case ObjectType.o7:
                        s = "27_vedro1";
                        break;

                    case ObjectType.o8:
                        s = "27_vedro2";
                        break;
                }

                _sprite.spriteName = s;
            }
        }
    }
}