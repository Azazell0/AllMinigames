using UnityEngine;
using System.Collections;

namespace Minigame10
{
    public class BuildObject : MonoBehaviour
    {
        #region variables

        private SpriteRenderer _renderer;
        private Collider2D _collider2D;

        #endregion


        void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
        }

        /// <summary>
        /// Подсветить объект, на который попал курсор
        /// </summary>
        void OnMouseEnter()
        {
            MiniGame10_Manager.instance.WasMouseOverObject(this);
        }

        /// <summary>
        /// Убрать подсветку с объекта, на который попал курсор
        /// </summary>
        void OnMouseExit()
        {
            MiniGame10_Manager.instance.WasMouseNoMoreOverObject(this);
        }

        void OnMouseDown()
        {
            MiniGame10_Manager.instance.WasClickObject(this);
        }

        /// <summary>
        /// Спрятать объект от игрока
        /// Включает коллайдер
        /// </summary>
        public void Hide()
        {
            if (_renderer != null)
                _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0);
            if (_collider2D != null)
                _collider2D.enabled = true;
        }

        public void HighlightObject()
        {
            if (_renderer != null)
                _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1);
        }

        /// <summary>
        /// Показать объект игроку
        /// Отключает коллайдер
        /// </summary>
        public void Show()
        {
            if (_renderer != null)
                StartCoroutine(SetVisibleRoutine(0.1f, 0.05f));
            if (_collider2D != null)
                _collider2D.enabled = false;
        }

        /// <summary>
        /// Постепенное увеличивание параметра альфаканала до 1
        /// </summary>
        /// <param name="speed">Скорость увеличивания за один шаг</param>
        /// <param name="delay">Задержка между шагами</param>
        /// <returns></returns>
        private IEnumerator SetVisibleRoutine(float speed, float delay)
        {
            if (_renderer == null)
                yield return 0;
            while (_renderer.color.a != 1)
            {
                _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _renderer.color.a + speed);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}