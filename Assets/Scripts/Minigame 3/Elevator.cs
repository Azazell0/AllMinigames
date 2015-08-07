using UnityEngine;
using System.Collections;

namespace Minigame3
{
    public class Elevator : MonoBehaviour
    {
        #region variables

        public UISprite spriteFloor1ElevatorClose2, spriteFloor1ElevatorOpen, spriteFloor2ElevatorClose2, spriteFloor2ElevatorOpen, spriteFloor3ElevatorClose2, spriteFloor3ElevatorOpen;

        /// <summary>
        /// Задержка в анимации между заменой текстур двери
        /// </summary>
        public float doorDelay = 0.5f;
        /// <summary>
        /// Задержка анимации между перемещениями по этажам
        /// </summary>
        public float floorDelay = 1.2f;

        #endregion


        public IEnumerator StartElevator()
        {
            if (spriteFloor1ElevatorOpen != null)
                spriteFloor1ElevatorOpen.gameObject.SetActive(false);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor1ElevatorClose2 != null)
                spriteFloor1ElevatorClose2.gameObject.SetActive(false);

            yield return new WaitForSeconds(floorDelay);

            if (spriteFloor2ElevatorClose2 != null)
                spriteFloor2ElevatorClose2.gameObject.SetActive(true);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor2ElevatorOpen != null)
                spriteFloor2ElevatorOpen.gameObject.SetActive(true);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor2ElevatorOpen != null)
                spriteFloor2ElevatorOpen.gameObject.SetActive(false);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor2ElevatorClose2 != null)
                spriteFloor2ElevatorClose2.gameObject.SetActive(false);

            yield return new WaitForSeconds(floorDelay);

            if (spriteFloor3ElevatorClose2 != null)
                spriteFloor3ElevatorClose2.gameObject.SetActive(true);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor3ElevatorOpen != null)
                spriteFloor3ElevatorOpen.gameObject.SetActive(true);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor3ElevatorOpen != null)
                spriteFloor3ElevatorOpen.gameObject.SetActive(false);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor3ElevatorClose2 != null)
                spriteFloor3ElevatorClose2.gameObject.SetActive(false);

            yield return new WaitForSeconds(floorDelay * 1.5f);

            if (spriteFloor1ElevatorClose2 != null)
                spriteFloor1ElevatorClose2.gameObject.SetActive(true);
            yield return new WaitForSeconds(doorDelay);
            if (spriteFloor1ElevatorOpen != null)
                spriteFloor1ElevatorOpen.gameObject.SetActive(true);
        }
    }
}