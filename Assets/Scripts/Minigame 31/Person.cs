using UnityEngine;
using System.Collections;

namespace Minigame31
{
    public class Person : MonoBehaviour
    {
        #region variables

        public Direction currentDirection
        {
            get { return _currentDirection; }
            set
            {
                _currentDirection = value;
                switch(_currentDirection)
                {
                    case Direction.LeftDown:
                        SetLeftDown();
                        break;
                    case Direction.LeftUp:
                        SetLeftUp();
                        break;
                    case Direction.RightDown:
                        SetRightDown();
                        break;
                    case Direction.RightUp:
                        SetRightUp();
                        break;
                }
            }
        }
        public UISprite spriteRightUp, spriteRightDown, spriteLeftUp, spriteLeftDown;
        public Collider colliderC;

        private Direction _currentDirection;

        #endregion


        void Start()
        {
            currentDirection = Direction.RightUp;
        }

        public void SetRightUp()
        {
            _currentDirection = Direction.RightUp;
            SetSpriteActive(spriteRightUp);
        }

        public void SetRightDown()
        {
            _currentDirection = Direction.RightDown;
            SetSpriteActive(spriteRightDown);
        }

        public void SetLeftUp()
        {
            _currentDirection = Direction.LeftUp;
            SetSpriteActive(spriteLeftUp);
        }

        public void SetLeftDown()
        {
            _currentDirection = Direction.LeftDown;
            SetSpriteActive(spriteLeftDown);
        }

        private void SetSpriteActive(UISprite sprite)
        {
            if (spriteRightUp != null)
                spriteRightUp.gameObject.SetActive((spriteRightUp == sprite) ? true : false);
            if (spriteRightDown != null)
                spriteRightDown.gameObject.SetActive((spriteRightDown == sprite) ? true : false);
            if (spriteLeftUp != null)
                spriteLeftUp.gameObject.SetActive((spriteLeftUp == sprite) ? true : false);
            if (spriteLeftDown != null)
                spriteLeftDown.gameObject.SetActive((spriteLeftDown == sprite) ? true : false);

            if (colliderC != null)
            {
                switch (_currentDirection)
                {
                    case Direction.RightUp:
                        colliderC.transform.localPosition = new Vector3(77, 22, 0);
                        break;
                    case Direction.RightDown:
                        colliderC.transform.localPosition = new Vector3(77, -37, 0);
                        break;
                    case Direction.LeftUp:
                        colliderC.transform.localPosition = new Vector3(-77, 22, 0);
                        break;
                    case Direction.LeftDown:
                        colliderC.transform.localPosition = new Vector3(-77, -37, 0);
                        break;
                }
            }
        }
    }

    public enum Direction { RightUp, RightDown, LeftDown, LeftUp}
}