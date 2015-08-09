using UnityEngine;
using System.Collections;

namespace Minigame17
{
    public class Plate : MonoBehaviour
    {
        #region variables

        public PlateType type;

        #endregion


        void Start()
        {

        }

        void Update()
        {

        }
    }

    public enum PlateType { Small, Large}
}