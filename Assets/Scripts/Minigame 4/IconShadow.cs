using UnityEngine;
using System.Collections;

namespace Minigame4
{
    public class IconShadow : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// true - Toggle на данный момент установлен
        /// </summary>
        public bool value { get { return (_toggle == null) ? false : _toggle.value; } }
        /// <summary>
        /// Указатель на корректную для тени иконку
        /// </summary>
        public Icon correctPair;
        /// <summary>
        /// Указатель на выбранную игроком иконку
        /// </summary>
        public Icon selectPair;
        public UISprite correctSprite;
        public UISprite bigGreySprite;

        private UISprite _sprite;
        private UIToggle _toggle;
        private UIButtonScale _buttonScale;
        private BoxCollider _boxCollider;

        #endregion

        
        void Start()
        {
            _sprite = GetComponent<UISprite>();
            _toggle = GetComponent<UIToggle>();
            _buttonScale = GetComponent<UIButtonScale>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void Reset()
        {
            if (_toggle != null)
                _toggle.value = false;
            if (_boxCollider != null)
                _boxCollider.enabled = true;
            transform.localScale = Vector3.one;
            if (_buttonScale != null)
                _buttonScale.enabled = true;
            if (correctSprite != null)
                correctSprite.alpha = 0f;

            selectPair = null;
            SetCurrentSprite();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="icon"></param>
        public void SetPair(Icon icon)
        {
            selectPair = icon;
            SetCurrentSprite();

            bool b = (selectPair == null) ? true : false;
            if (_toggle != null)
                _toggle.value = !b;
            if (_boxCollider != null)
                _boxCollider.enabled = b;
            if (_buttonScale != null)
                _buttonScale.enabled = b;
            transform.localScale = Vector3.one;
        }

        public void ShowCorrectPair()
        {
            StartCoroutine(ShowCorrectPairCoroutine());
        }

        public void ChangeToggleValue()
        {
            if (MiniGame4_Manager.instance.isPlay)
                if (_toggle != null)
                    MiniGame4_Manager.instance.ClickToggleIconShadow(this);
        }

        public void SetToggleValue(bool b)
        {
            if (_toggle != null)
                _toggle.value = b;
        }

        public void SetToggleEnable(bool b)
        {
            if (_toggle != null)
                _toggle.enabled = b;
        }

        private IEnumerator ShowCorrectPairCoroutine()
        {
            if (correctSprite != null)
            {
                while (correctSprite.alpha < 1)
                {
                    correctSprite.alpha += 1f * 0.04f;
                    yield return new WaitForSeconds(0.04f);
                }
            }
        }

        private void SetCurrentSprite()
        {
            if (bigGreySprite == null)
                return;
            
            if (selectPair == null)
                bigGreySprite.spriteName = "icon_grey";
            else switch(selectPair.spriteName)
                {
                    case "icon_gaz":
                        bigGreySprite.spriteName = "icon_fiolet";
                        break;

                    case "icon_electro":
                        bigGreySprite.spriteName = "icon_yellow";
                        break;

                    case "icon_water":
                        bigGreySprite.spriteName = "icon_blue";
                        break;

                    case "icon_warm":
                        bigGreySprite.spriteName = "icon_red";
                        break;

                    default:
                        bigGreySprite.spriteName = "icon_grey";
                        break;
                }
        }
    }
}