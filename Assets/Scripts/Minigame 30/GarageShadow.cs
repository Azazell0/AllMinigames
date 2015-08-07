using UnityEngine;
using System.Collections;

namespace Minigame30
{
    public class GarageShadow : Garage
    {
        void Awake()
        {
            _sprite = GetComponent<UISprite>();
        }
    }
}