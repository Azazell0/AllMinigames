using UnityEngine;
using System.Collections;

namespace Minigame1
{
    public class Resource : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// тип инструмента
        /// </summary>
        public Minigame1_ResourceType type;
        /// <summary>
        /// Трансформ тени ресурса
        /// </summary>
        public Transform shadowTransform;

        #endregion


        /// <summary>
        /// Нажатие на ресурс
        /// </summary>
        public void Click()
        {
            MiniGame1_Manager.instance.WasClickResource(this);
        }

        /// <summary>
        /// Показывает тень под ресурсом
        /// </summary>
        public void ShowShadow()
        {
            if (shadowTransform != null)
                shadowTransform.gameObject.SetActive(true);
        }

        /// <summary>
        /// Скрывает тень под ресурсом
        /// </summary>
        public void HideShadow()
        {
            if (shadowTransform != null)
                shadowTransform.gameObject.SetActive(false);
        }
    }

    public enum Minigame1_ResourceType { Null, Paint, Slate, Tube, Glass, Putty }
}