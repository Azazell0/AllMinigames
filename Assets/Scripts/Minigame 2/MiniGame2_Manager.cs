using UnityEngine;
using Minigame2;
using System.Collections;
using System.Collections.Generic;

public class MiniGame2_Manager : MiniGameSingleton<MiniGame2_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    public List<House> listHouses;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// true - в данный момент работает корутина
    /// </summary>
    private bool _isCoroutineWork = false;
    private RepairType _choosedType;
    private House _currentHouse;

    #endregion


    void Awake()
    {
        // Для синглтона
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    void Update()
    {
        if (_isPlay)
            CheckTime();
    }

    public void CloseMenu()
    {
        if (!_isCoroutineWork)
            Hide();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _isCoroutineWork = false;
        _currentHouse = null;

        if (listHouses == null)
            listHouses = new List<House>();

        if (listHouses.Count > 0)
            _currentHouse = listHouses[Random.Range(0, listHouses.Count)];
        foreach(House h in listHouses)
            if (h != null)
            {
                if (h == _currentHouse)
                    h.Reset();
                else
                    h.Hide();
            }
    }

    /// <summary>
    /// Инициализация новой игры
    /// </summary>
    /// <param name="time">Время для прохождения</param>
    public void NewGame(float time)
    {
        Init();
        Show();

        _time = time;
        _isPlay = true;
    }

    public void ClickButton1()
    {
        StartCoroutine(CheckRepair(RepairType.Hammer));
    }

    public void ClickButton2()
    {
        StartCoroutine(CheckRepair(RepairType.Bitumen));
    }

    public void ClickButton3()
    {
        StartCoroutine(CheckRepair(RepairType.Stick));
    }

    public void ClickButton4()
    {
        StartCoroutine(CheckRepair(RepairType.PartRepair));
    }

    public void ClickButton5()
    {
        StartCoroutine(CheckRepair(RepairType.Stocker));
    }

    public void ClickButton6()
    {
        StartCoroutine(CheckRepair(RepairType.FullRepair));
    }

    public void ClickButton7()
    {
        StartCoroutine(CheckRepair(RepairType.Heating));
    }

    private IEnumerator CheckRepair(RepairType type)
    {
        if (_isPlay && !_isCoroutineWork)
        {
            _choosedType = type;

            if (_currentHouse != null && _currentHouse.repairType == _choosedType)
            {
                _isCoroutineWork = true;
                yield return StartCoroutine(_currentHouse.Repair());
                _isCoroutineWork = false;
            }

            Win();
        }
    }

    /// <summary>
    /// Проверка оставшегося времени до конца игры
    /// </summary>
    private void CheckTime()
    {
        if (_isPlay)
        {
            _time -= Time.deltaTime;

            if (labelTime != null)
                labelTime.text = (((int)_time / 60)).ToString("00") + ":" + ((int)_time % 60).ToString("00");

            if (_time <= 0)
            {
                Debug.Log("Time is out!");
                Losing();
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        if (_time <= 0)
            return MiniGameResult.Bronze;
        if (_currentHouse == null)
            return MiniGameResult.Bronze;
        if (_currentHouse.repairType == _choosedType)
            return MiniGameResult.Gold;
        return MiniGameResult.Bronze;
    }
}