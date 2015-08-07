using UnityEngine;
using System.Collections;

namespace Minigame9
{
    public class Thought : MonoBehaviour
    {
        #region varibles

        public PersonThought thoughtType
        {
            get { return _thoughtType; }
            set { _thoughtType = value; UpdateThoughtType(); }
        }

        private PersonThought _thoughtType = PersonThought.Refueling;
        private UIImageButton _imageButton;
        private UISprite _sprite;

        #endregion

        
        void Start()
        {
            _imageButton = GetComponent<UIImageButton>();
            _sprite = GetComponent<UISprite>();
        }

        private void UpdateThoughtType()
        {
            string s1 = "", s2 = "";
            switch(thoughtType)
            {
                case PersonThought.Basement:
                    s1 = "09_basement";
                    s2 = "09_basement_allocation";
                    break;
                case PersonThought.Intercom:
                    s1 = "09_intercom";
                    s2 = "09_intercom_allocation";
                    break;
                case PersonThought.Nursery:
                    s1 = "09_nursery";
                    s2 = "09_nursery_allocation";
                    break;
                case PersonThought.Refueling:
                    s1 = "09_ refueling";
                    s2 = "09_ refueling_allocation";
                    break;
                case PersonThought.Security:
                    s1 = "09_security";
                    s2 = "09_security_allocation";
                    break;
                case PersonThought.Shop:
                    s1 = "09_ shop";
                    s2 = "09_ shop_allocation";
                    break;
                case PersonThought.Stadium:
                    s1 = "09_stadium";
                    s2 = "09_stadium_allocation";
                    break;
                default:
                    break;
            }

            //if (_imageButton != null)
            //{
            //    _imageButton.normalSprite = _imageButton.disabledSprite = s1;
            //    _imageButton.hoverSprite = _imageButton.pressedSprite = s2;
            //}
            if (_sprite == null)
                _sprite = GetComponent<UISprite>();
            if (_sprite != null)
            {
                _sprite.spriteName = s1;
            }
        }

        public IEnumerator StartAnimation()
        {
            if (_sprite != null)
            {
                _sprite.alpha = 0;
                while (_sprite.alpha < 1)
                {
                    yield return new WaitForSeconds(0.03f);
                    _sprite.alpha += 0.05f;
                }
            }
        }

        public void Reset()
        {
            if (_imageButton == null && _sprite != null)
                _sprite.alpha = 0;
        }
    }

    public enum PersonThought { None = 0, Refueling, Shop, Basement, Intercom, Nursery, Security, Stadium }
}