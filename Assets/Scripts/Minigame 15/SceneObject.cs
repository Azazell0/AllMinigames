using UnityEngine;
using System.Collections;

namespace Minigame15
{
    public class SceneObject : MonoBehaviour
    {
        #region variables

        private UISprite _sprite;
        private BoxCollider _boxCollider;

        #endregion

        
        void Start()
        {
            _sprite = GetComponent<UISprite>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void Click()
        {
            MiniGame15_Manager.instance.ClickObject(this);
            StartCoroutine(Hide(0.08f));
        }

        public void Reset()
        {
            if (_boxCollider != null)
                _boxCollider.enabled = true;
            if (_sprite != null)
                _sprite.alpha = 1;
        }

        private IEnumerator Hide(float speed)
        {
            if (_boxCollider != null)
                _boxCollider.enabled = false;
            if (_sprite != null)
                while (_sprite.alpha > 0)
                {
                    _sprite.alpha -= speed;
                    yield return new WaitForSeconds(0.05f);
                }
        }

        private IEnumerator Show(float speed)
        {
            if (_boxCollider != null)
                _boxCollider.enabled = true;
            if (_sprite != null)
                while (_sprite.alpha < 0)
                {
                    _sprite.alpha += speed;
                    yield return new WaitForSeconds(0.05f);
                }
        }
    }
}