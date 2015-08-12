using UnityEngine;
using Minigame21;
using System.Collections;
using System.Collections.Generic;

public class MiniGame21_Manager : MiniGameSingleton<MiniGame21_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Список всех счетчиков
    /// </summary>
    public List<Counter> listCounters;
    /// <summary>
    /// Список всех модемов
    /// </summary>
    public List<Modem> listModems;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    private Counter _currentCounter;
    private int _counrErrors = 0;
    private List<Counter> _listCounters;

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
        Hide();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _counrErrors = 0;
        _currentCounter = null;

        if (listCounters == null)
            listCounters = new List<Counter>();
        else foreach (Counter m in listCounters)
                if (m != null)
                    m.Reset();
        if (listCounters.Count > 0)
            _currentCounter = listCounters[Random.Range(0, listCounters.Count)];

        if (_currentCounter != null)
            _currentCounter.SetToggle(true);

        _listCounters = new List<Counter>(listCounters);

        if (listModems == null)
            listModems = new List<Modem>();
        else foreach (Modem m in listModems)
                if (m != null)
                    m.Reset();
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

    public void ClickModem(Modem modem)
    {
        if (!_isPlay || modem == null || _currentCounter == null)
            return;

        if (_currentCounter.modem == modem)
        {
            _currentCounter.SetWay();
            _currentCounter.SetToggle(false);
            _listCounters.Remove(_currentCounter);
            modem.SetGreenMode();
            if (_listCounters.Count > 0)
            {
                _currentCounter = _listCounters[Random.Range(0, _listCounters.Count)];
                _currentCounter.SetToggle(true);
            }
            else
                Win();
        }
        else
        {
            _counrErrors++;
            modem.SetRedMode();
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
                Win();
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        if (_listCounters.Count > 0)
            return MiniGameResult.Bronze;
        return (_counrErrors == 0) ? MiniGameResult.Gold : (_counrErrors == 1 || _counrErrors == 2) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}