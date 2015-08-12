using UnityEngine;
using System.Collections;

namespace Minigame21
{
    public class Counter : MonoBehaviour
    {
        #region variables

        public Modem modem;
        public UISprite spriteWay;

        private UIToggle _toggle;
        private bool _isInit = false;

        #endregion


        void Start()
        {
            Init();
        }

        private void Init()
        {
            if (_isInit)
                return;

            _toggle = GetComponent<UIToggle>();
            _isInit = true;
        }

        public void Reset()
        {
            Init();
            if (_toggle != null)
                _toggle.value = false;
            if (spriteWay != null)
                spriteWay.alpha = 0f;
        }

        public void SetToggle(bool b)
        {
            if (_toggle != null)
                _toggle.value = b;
        }

        public void SetWay()
        {
            StartCoroutine(SetWayCoroutine());
        }

        private IEnumerator SetWayCoroutine()
        {
            if (spriteWay != null)
                while (spriteWay.alpha < 1)
                {
                    spriteWay.alpha += 0.04f;
                    yield return new WaitForSeconds(0.03f);
                }
        }
    }
}