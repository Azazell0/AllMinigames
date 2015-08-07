using UnityEngine;
using System.Collections;

namespace Minigame25
{
    public class WiFiButton : MonoBehaviour
    {
        #region variables

        public WiFiPointRadius radius;

        #endregion


        void OnMouseDown()
        {
            if (MiniGame25_Manager.instance.isPlay)
                MiniGame25_Manager.instance.SetWiFiMode(radius);
        }
    }
}