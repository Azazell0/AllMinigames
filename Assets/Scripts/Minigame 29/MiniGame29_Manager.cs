using UnityEngine;
using Minigame29;
using System.Collections;
using System.Collections.Generic;

public class MiniGame29_Manager : MiniGameSingleton<MiniGame29_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения количества найденных ошибок
    /// </summary>
    public UILabel labelCount;
    /// <summary>
    /// Трансформ, содержащий в себе все экземпляры продуктов из которых будут генерироваться продукты с коллайдерами
    /// </summary>
    public Transform ProductGeneratorsContainer;

    /// <summary>
    /// Количество собранных вегетарианских продуктов
    /// </summary>
    private int _productCount = 0;
    /// <summary>
    /// Список продуктов-генераторов
    /// </summary>
    private List<Product> _listProductGenerators;
    /// <summary>
    /// Список всех созданных продуктов
    /// </summary>
    private List<Product> _listProducts;
    /// <summary>
    /// Время до следующей генерации продукта
    /// </summary>
    private float _timeToNextGenerate = 3f;

    #endregion


    void Awake()
    {
        // Для синглтона
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }

    void Update()
    {
        if (_isPlay)
            CheckTime();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _productCount = 0;
        _timeToNextGenerate = 3f;
        UpdateCountLabel();

        if (_listProductGenerators == null)
        {
            _listProductGenerators = new List<Product>();
            if (ProductGeneratorsContainer != null)
                foreach(Transform t in ProductGeneratorsContainer)
                {
                    Product p = t.GetComponent<Product>();
                    if (p != null)
                        _listProductGenerators.Add(p);
                }
        }

        ResetListProducts();
    }
	
	public void CloseMenu()
    {
        Hide();
    }

    /// <summary>
    /// В корзину был пойман продукт
    /// </summary>
    /// <param name="product">Указатель на продукт</param>
    public void WasPutProduct(Product product)
    {
        if (!_isPlay || product == null)
            return;

        switch(product.productType)
        {
            case ProductType.Vegetarian:
                _productCount++;
                UpdateCountLabel();
                Destroy(product.gameObject);
                break;

            case ProductType.Usual:
                ResetListProducts();
                Win();
                break;
        }
    }

    private void DestroyProduct(Product product)
    {
        if (product == null)
            return;

        _listProducts.Remove(product);
        Destroy(product.gameObject);
    }

    /// <summary>
    /// Обновляет лейбл с указание количества пойманных вегетарианских продуктов
    /// </summary>
    private void UpdateCountLabel()
    {
        if (labelCount != null)
            labelCount.text = _productCount.ToString();
    }

    /// <summary>
    /// Проверка оставшегося времени до конца игры
    /// </summary>
    private void CheckTime()
    {
        if (_isPlay)
        {
            _time -= Time.deltaTime;
            _timeToNextGenerate -= Time.deltaTime;

            if (labelTime != null)
                labelTime.text = (((int)_time / 60)).ToString("00") + ":" + ((int)_time % 60).ToString("00");

            if (_timeToNextGenerate <= 0)
            {
                if (_listProductGenerators.Count > 0)
                {
                    Product p = _listProductGenerators[Random.Range(0, _listProductGenerators.Count)];
                    if (p != null)
                    {
                        Product pp = p.Generate(ProductGeneratorsContainer);
                        if (pp != null)
                            _listProducts.Add(pp);
                    }
                }
                _timeToNextGenerate += 1;
            }

            if (_time <= 0)
            {
                ResetListProducts();
                Win();
            }
        }
    }

    /// <summary>
    /// Удаляет все сгенерированные продукты
    /// </summary>
    private void ResetListProducts()
    {
        if (_listProducts == null)
            _listProducts = new List<Product>();
        else
        {
            foreach (Product p in _listProducts)
                if (p != null)
                    Destroy(p.gameObject);
            _listProducts.Clear();
        }
    }

    protected override MiniGameResult GetResult()
    {
        if (_productCount >= 13)
            return MiniGameResult.Gold;
        else if (_productCount >= 10)
            return MiniGameResult.Silver;
        else return MiniGameResult.Bronze;
    }
}