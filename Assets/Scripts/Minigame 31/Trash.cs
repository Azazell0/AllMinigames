using UnityEngine;
using System.Collections;

namespace Minigame31
{
    public class Trash : MonoBehaviour
    {
        #region variables

        public Direction currentDirection;

        #endregion

        void OnTriggerEnter(Collider other)
        {
            switch(other.name)
            {
                case "Collider":
                    MiniGame31_Manager.instance.TrashInTheBasket(this);
                    break;

                case "ColliderGameEnd":
                    MiniGame31_Manager.instance.TrashOnTheFloor(this);
                    break;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}