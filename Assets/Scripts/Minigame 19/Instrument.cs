using UnityEngine;
using System.Collections;

namespace Minigame19
{
    public class Instrument : MonoBehaviour
    {
        #region variables

        public InstrumentType type;

        private UISprite _sprite;

        #endregion

        
        void Awake()
        {
            _sprite = GetComponent<UISprite>();
        }

        public void Hide()
        {
            if (_sprite == null)
                _sprite = GetComponent<UISprite>();
            if (_sprite != null)
                _sprite.alpha = 0f;
        }

        public void Show()
        {
            if (_sprite == null)
                _sprite = GetComponent<UISprite>();
            if (_sprite != null)
                _sprite.alpha = 1f;
        }

        public IEnumerator HideCoroutine(float speed)
        {
            if (_sprite == null)
                _sprite = GetComponent<UISprite>();
            if (_sprite != null)
                while (_sprite.alpha > 0)
                {
                    _sprite.alpha -= speed;
                    yield return new WaitForSeconds(0.03f);
                }
        }

        public IEnumerator ShowCoroutine(float speed)
        {
            if (_sprite == null)
                _sprite = GetComponent<UISprite>();
            if (_sprite != null)
                while (_sprite.alpha < 1)
                {
                    _sprite.alpha += speed;
                    yield return new WaitForSeconds(0.03f);
                }
        }
    }

    public enum InstrumentType { Saw, Chisel, Nippers, Hammer, Fretsaw, Trowel, Pliers, Drill, Axe, Wrench}
}