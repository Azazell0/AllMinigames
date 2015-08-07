using UnityEngine;
using System.Collections;

namespace Minigame1
{
    public class Breaking : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Тип поломки
        /// </summary>
        public Minigame1_BreakingType type;

        #endregion

        /// <summary>
        /// Нажатие на поломку
        /// </summary>
        public void Click()
        {
            if (MiniGame1_Manager.instance.isPlay)
                MiniGame1_Manager.instance.WasClickBreaking(this);
        }

        /// <summary>
        /// Скрыть поломку
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Показать поломку
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Проверка корректности инструмента и ресурса для данной поломки
        /// </summary>
        /// <param name="instrument">Указатель на инструмент</param>
        /// <param name="resource">Указатель на ресурс</param>
        /// <returns>true - инструмент и ресурс корректный</returns>
        public bool CheckInstrumentAndResource(Instrument instrument, Resource resource)
        {
            if (instrument == null || resource == null)
                return false;

            switch (type)
            {
                case Minigame1_BreakingType.Roof:
                    return (instrument.type == Minigame1_InstrumentType.Hammer && resource.type == Minigame1_ResourceType.Slate) ? true : false;

                case Minigame1_BreakingType.Tube:
                    return (instrument.type == Minigame1_InstrumentType.Screwdriver && resource.type == Minigame1_ResourceType.Tube) ? true : false;

                case Minigame1_BreakingType.WallCrash:
                    return (instrument.type == Minigame1_InstrumentType.Spatula && resource.type == Minigame1_ResourceType.Putty) ? true : false;

                case Minigame1_BreakingType.WallDirt:
                    return (instrument.type == Minigame1_InstrumentType.Brush && resource.type == Minigame1_ResourceType.Paint) ? true : false;

                case Minigame1_BreakingType.Window:
                    return (instrument.type == Minigame1_InstrumentType.GlassCutter && resource.type == Minigame1_ResourceType.Glass) ? true : false;

                case Minigame1_BreakingType.Null:
                    return false;

                default:
                    return false;
            }
        }
    }

    public enum Minigame1_BreakingType { Null, WallDirt, WallCrash, Roof, Tube, Window }
}