using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame2
{
    public class House : MonoBehaviour
    {
        #region variables

        public RepairType repairType;
        public List<UISprite> listVisibleSprites;
        public List<UISprite> listUnvisibleSprites;

        private bool _isInit = false;

        #endregion


        void Start()
        {
            Init();
        }

        void Update()
        {

        }

        public void Reset()
        {
            Init();

            foreach (UISprite sprite in listVisibleSprites)
                if (sprite != null)
                    sprite.alpha = 1f;
            foreach (UISprite sprite in listUnvisibleSprites)
                if (sprite != null)
                    sprite.alpha = 0f;
        }

        public void Hide()
        {
            Init();

            foreach (UISprite sprite in listVisibleSprites)
                if (sprite != null)
                    sprite.alpha = 0f;
            foreach (UISprite sprite in listUnvisibleSprites)
                if (sprite != null)
                    sprite.alpha = 1f;
        }

        public IEnumerator Repair()
        {
            if (listVisibleSprites != null && listUnvisibleSprites != null)
                if (listVisibleSprites.Count > 0)
                {
                    while (listVisibleSprites[0].alpha > 0)
                    {
                        foreach (UISprite s in listVisibleSprites)
                            s.alpha -= 0.04f;
                        foreach (UISprite s in listUnvisibleSprites)
                            s.alpha += 0.04f;
                        yield return new WaitForSeconds(0.03f);
                    }
                }
        }

        private void Init()
        {
            if (_isInit)
                return;

            if (listVisibleSprites == null)
                listVisibleSprites = new List<UISprite>();
            if (listUnvisibleSprites == null)
                listUnvisibleSprites = new List<UISprite>();

            _isInit = true;
        }
    }

    public enum RepairType { Hammer, Bitumen, Stick, PartRepair, Stocker, FullRepair, Heating}
}