using UnityEngine;
using System.Collections;

namespace Minigame29
{
    public class Product : MonoBehaviour
    {
        #region variables

        public ProductType productType;
        public ProductState productState;

        private bool _isGo = false;

        #endregion

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_isGo)
            {
                transform.localPosition -= new Vector3(0f, 300 * Time.deltaTime, 0f);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "Basket")
            {
                MiniGame29_Manager.instance.WasPutProduct(this);
            }
            else if (other.name == "Floor")
            {
                Destroy(this.gameObject);
            }
        }

        public Product Generate(Transform tParent)
        {
            GameObject go = Instantiate(this.gameObject);
            if (go != null)
            {
                if (tParent != null)
                {
                    go.transform.parent = tParent;
                    go.transform.localPosition = transform.localPosition;
                    go.transform.localScale = transform.localScale;
                }

                BoxCollider bc = go.GetComponent<BoxCollider>();
                if (bc != null)
                    bc.enabled = true;

                Product p = go.GetComponent<Product>();
                if (p != null)
                    p.Go();
                return p;
            }
            return null;
        }

        public void Go()
        {
            _isGo = true;
        }
    }

    public enum ProductType { Usual, Vegetarian}
    public enum ProductState { Usual, Generator}
}