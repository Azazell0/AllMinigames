using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame9
{
    public class Person : MonoBehaviour
    {
        #region variables

        public UIButton buttonPerson;
        public Thought thought1, thought2, thought3, thoughtEnd;

        private bool _isThoughtesShow = false;

        #endregion

        
        void Start()
        {

        }

        void Update()
        {
            
        }

        public void Click()
        {
            MiniGame9_Manager.instance.WasClickPerson(this);
        }

        public void ShowThoughtes(bool b)
        {
            if (thought1 != null)
                thought1.gameObject.SetActive(b);
            if (thought2 != null)
                thought2.gameObject.SetActive(b);
            if (thought3 != null)
                thought3.gameObject.SetActive(b);
            _isThoughtesShow = b;
        }

        public void SetThoughtes(List<PersonThought> list)
        {
            if (list.Count > 0)
                thought1.thoughtType = list[0];
            if (list.Count > 1)
                thought2.thoughtType = list[1];
            if (list.Count > 2)
                thought3.thoughtType = list[2];
        }

        public void SelectThought1()
        {
            if (thought1 != null && thoughtEnd != null)
            {
                thoughtEnd.thoughtType = thought1.thoughtType;
                thoughtEnd.StartCoroutine(thoughtEnd.StartAnimation());
            }
            ShowThoughtes(false);
        }
        public void SelectThought2()
        {
            if (thought2 != null && thoughtEnd != null)
            {
                thoughtEnd.thoughtType = thought2.thoughtType;
                thoughtEnd.StartCoroutine(thoughtEnd.StartAnimation());
            }
            ShowThoughtes(false);
        }
        public void SelectThought3()
        {
            if (thought3 != null && thoughtEnd != null)
            {
                thoughtEnd.thoughtType = thought3.thoughtType;
                thoughtEnd.StartCoroutine(thoughtEnd.StartAnimation());
            }
            ShowThoughtes(false);
        }

        public void Reset()
        {
            _isThoughtesShow = false;
            if (buttonPerson != null)
                buttonPerson.enabled = true;
            if (thoughtEnd != null)
                thoughtEnd.Reset();
            ShowThoughtes(false);
        }
    }
}