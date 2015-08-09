using UnityEngine;
using System.Collections;

namespace Minigame28
{
    public class Car : MonoBehaviour
    {
        #region variables

        public Transform containerPath;

        private UISprite _sprite;

        #endregion

        
        void Awake()
        {
            _sprite = GetComponent<UISprite>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}