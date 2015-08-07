using UnityEngine;
using System.Collections;

namespace Minigame1
{
    public class Instrument : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Тип инструмента
        /// </summary>
        public Minigame1_InstrumentType type;
        /// <summary>
        /// Тень под инструментом
        /// </summary>
        public Transform shadowTransform;

        #endregion


        /// <summary>
        /// Нажатие на инструмент
        /// </summary>
        public void Click()
        {
            if (MiniGame1_Manager.instance.isPlay)
                MiniGame1_Manager.instance.WasClickInstrument(this);
        }

        /// <summary>
        /// Показывает тень под инструментом
        /// </summary>
        public void ShowShadow()
        {
            if (shadowTransform != null)
                shadowTransform.gameObject.SetActive(true);
        }

        /// <summary>
        /// Скрывает тень под инструментом
        /// </summary>
        public void HideShadow()
        {
            if (shadowTransform != null)
                shadowTransform.gameObject.SetActive(false);
        }

        /// <summary>
        /// Проверка корректности ресурса для данного инструмента
        /// </summary>
        /// <param name="resource">Указатель на ресурс</param>
        /// <returns>true - ресурс корретный</returns>
        public bool CkechResource(Resource resource)
        {
            if (resource == null)
                return false;

            switch (type)
            {
                case Minigame1_InstrumentType.Brush:
                    return (resource.type == Minigame1_ResourceType.Paint) ? true : false;

                case Minigame1_InstrumentType.GlassCutter:
                    return (resource.type == Minigame1_ResourceType.Glass) ? true : false;

                case Minigame1_InstrumentType.Hammer:
                    return (resource.type == Minigame1_ResourceType.Slate) ? true : false;

                case Minigame1_InstrumentType.Screwdriver:
                    return (resource.type == Minigame1_ResourceType.Tube) ? true : false;

                case Minigame1_InstrumentType.Spatula:
                    return (resource.type == Minigame1_ResourceType.Putty) ? true : false;

                case Minigame1_InstrumentType.Null:
                    return true;

                default:
                    return false;
            }
        }
    }

    public enum Minigame1_InstrumentType { Null, Brush, Hammer, Screwdriver, GlassCutter, Spatula }
}