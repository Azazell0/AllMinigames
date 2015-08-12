using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame5
{
    public class ButtonToggle : MonoBehaviour
    {
        #region variables

        public Vector3 targetVector;
        public UISprite spriteDescription;
        public List<UISprite> listSpriteActive;
        public List<UISprite> listSpriteDeactive;

        private Vector3 _startVector;
        private UIToggle _toggle;
        private UISprite _sprite;
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

            _startVector = transform.localPosition;
            _toggle = GetComponent<UIToggle>();
            _sprite = GetComponent<UISprite>();

            if (listSpriteActive == null)
                listSpriteActive = new List<UISprite>();
            if (listSpriteDeactive == null)
                listSpriteDeactive = new List<UISprite>();

            _isInit = true;
        }

        public void Reset()
        {
            Init();

            transform.localPosition = _startVector;
            foreach (UISprite s in listSpriteActive)
                if (s != null)
                    s.alpha = 0f;
            foreach (UISprite s in listSpriteDeactive)
                if (s != null)
                    s.alpha = 1f;
            if (spriteDescription != null)
                spriteDescription.alpha = 1f;
            if (_toggle != null)
            {
                _toggle.enabled = true;
                _toggle.value = false;
            }
        }

        public void SetToggleValue(bool b)
        {
            if (_toggle != null)
                _toggle.value = b;
        }

        public IEnumerator Set()
        {
            if (_toggle != null)
                _toggle.enabled = false;
            //if (_sprite != null)
            //    _sprite.depth++;
            if (spriteDescription != null)
                spriteDescription.alpha = 0f;

            while (Vector3.Distance(transform.localPosition, targetVector) > 0.1f)
            {

                transform.localPosition += (targetVector - transform.localPosition).normalized * Vector3.Distance(transform.localPosition, targetVector) * 0.1f;
                yield return new WaitForSeconds(0.03f);
            }
            transform.localPosition = targetVector;

            if (listSpriteActive != null && listSpriteDeactive != null && (listSpriteActive.Count > 0 || listSpriteDeactive.Count > 0))
            {
                if (listSpriteActive.Count > 0)
                {
                    while (listSpriteActive[0].alpha < 1)
                    {
                        foreach (UISprite s in listSpriteActive)
                            s.alpha += 0.4f;
                        foreach (UISprite s in listSpriteDeactive)
                            s.alpha -= 0.4f;
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else if (listSpriteDeactive.Count > 0)
                {
                    while (listSpriteDeactive[0].alpha > 0)
                    {
                        foreach (UISprite s in listSpriteActive)
                            s.alpha += 0.4f;
                        foreach (UISprite s in listSpriteDeactive)
                            s.alpha -= 0.4f;
                        yield return new WaitForSeconds(0.05f);
                    }
                }
            }

            //if (_sprite != null)
            //    _sprite.depth--;
        }

        public void Click()
        {
            if (_toggle != null && _toggle.value)
                MiniGame5_Manager.instance.ClickButtonToggle(this);
        }
    }
}