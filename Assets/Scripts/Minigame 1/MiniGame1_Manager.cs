using UnityEngine;
using Minigame1;
using System.Collections;
using System.Collections.Generic;

public class MiniGame1_Manager : MiniGameSingleton<MiniGame1_Manager>
{
    #region variables

    public delegate void MiniGameInstrumentAction(Instrument instrument);
    public delegate void MiniGameResourceAction(Resource resource);
    public delegate void MiniGameBreakingAction(Breaking breaking);

    /// <summary>
    /// Эвент совершения ошибки в мини-игре
    /// </summary>
    public static event MiniGameSimpleAction PlayerErrorEvent;
    /// <summary>
    /// Эвент успешного выбора инструмента игроком
    /// </summary>
    public static event MiniGameInstrumentAction SelectInstrumentEvent;
    /// <summary>
    /// Эвент успешного выбора ресурса игроком
    /// </summary>
    public static event MiniGameResourceAction SelectResourceEvent;
    /// <summary>
    /// Эвент успешного исправления поломки игроком
    /// </summary>
    public static event MiniGameBreakingAction FixBreakingEvent;

    /// <summary>
    /// Количество ошибок, при которых игра завершится неудачей
    /// </summary>
    public int errorCountMax = 3;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Контейнер, содержащий в дочерних объектах поломки типа Roof
    /// </summary>
    public GameObject BreakingsRoofContainer;
    /// <summary>
    /// Контейнер, содержащий в дочерних объектах поломки типа Window
    /// </summary>
    public GameObject BreakingsWindowContainer;
    /// <summary>
    /// Контейнер, содержащий в дочерних объектах поломки типа Tube
    /// </summary>
    public GameObject BreakingsTubeContainer;
    /// <summary>
    /// Контейнер, содержащий в дочерних объектах поломки типа WallCrash
    /// </summary>
    public GameObject BreakingsWallCrashContainer;
    /// <summary>
    /// Контейнер, содержащий в дочерних объектах поломки типа WallDirt
    /// </summary>
    public GameObject BreakingsWallDirtContainer;

    /// <summary>
    /// Количество поломок, которые осталось исправить в текущей игре
    /// </summary>
    public int activeBreakingsCount { get { return _activeBreakingCount; } }
    /// <summary>
    /// Количество совершенных ошибок в текущей игре
    /// </summary>
    public int errorCount { get { return _errorCount; } }

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Количество совершенных ошибок в текущей игре
    /// </summary>
    private int _errorCount = 0;
    /// <summary>
    /// Количество поломок, которые осталось исправить в текущей игре
    /// </summary>
    private int _activeBreakingCount = 0;
    /// <summary>
    /// Текущий инструмент
    /// </summary>
    private Instrument _currentInstrument;
    /// <summary>
    /// Текущий ресурс
    /// </summary>
    private Resource _currentResource;

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
        if (_currentInstrument != null)
            _currentInstrument.HideShadow();
        _currentInstrument = null;
        if (_currentResource != null)
            _currentResource.HideShadow();
        _currentResource = null;
        _errorCount = 0;
    }
	
	public void CloseMenu()
    {
        Hide();
    }

    /// <summary>
    /// Инициализация новой игры
    /// </summary>
    /// <param name="time">Время для прохождения</param>
    public void NewGame(float time)
    {
        Init();
        Show();

        MiniGameHelper.ActiveRandomChilds(BreakingsRoofContainer.transform, 1, true);
        MiniGameHelper.ActiveRandomChilds(BreakingsWindowContainer.transform, 1, true);
        MiniGameHelper.ActiveRandomChilds(BreakingsTubeContainer.transform, 1, true);
        MiniGameHelper.ActiveRandomChilds(BreakingsWallCrashContainer.transform, 1, true);
        MiniGameHelper.ActiveRandomChilds(BreakingsWallDirtContainer.transform, 1, true);
        _activeBreakingCount = 5;

        _time = time;
        _isPlay = true;
    }

    /// <summary>
    /// Попытка выбора инструмента
    /// </summary>
    /// <param name="instrument">Указатель на инструмент</param>
    public void WasClickInstrument(Instrument instrument)
    {
        if (!isPlay)
            return;

        if (_currentInstrument != null)
            _currentInstrument.HideShadow();
        _currentInstrument = instrument;
        _currentInstrument.ShowShadow();

        if (_currentInstrument != null)
        {
            Debug.Log("Selected instrument: " + _currentInstrument.type.ToString());
            if (SelectInstrumentEvent != null)
                SelectInstrumentEvent(_currentInstrument);
        }
    }

    /// <summary>
    /// Попытка выбора ресурса
    /// </summary>
    /// <param name="resource">Указатель на ресурс</param>
    public void WasClickResource(Resource resource)
    {
        if (!isPlay)
            return;

        if (_currentResource != null)
            _currentResource.HideShadow();
        _currentResource = resource;
        _currentResource.ShowShadow();

        if (_currentResource != null)
        {
            Debug.Log("Selected resource: " + _currentResource.type.ToString());
            if (SelectResourceEvent != null)
                SelectResourceEvent(_currentResource);
        }

        if (_currentInstrument != null && !_currentInstrument.CkechResource(_currentResource))
                PlayerError();
    }

    /// <summary>
    /// Попытка исправления поломки
    /// </summary>
    /// <param name="breaking">Указатель на поломку</param>
    public void WasClickBreaking(Breaking breaking)
    {
        if (!isPlay || breaking == null || (_currentInstrument == null && _currentResource == null))
            return;

        if (breaking.CheckInstrumentAndResource(_currentInstrument, _currentResource))
        {
            Debug.Log("Fix breaking: " + breaking.type.ToString());
            breaking.Hide();
            _activeBreakingCount--;
            if (FixBreakingEvent != null)
                FixBreakingEvent(breaking);
            if (CheckWin())
                Win();
        }
        else
            PlayerError();
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    /// <returns>true - игрок выиграл</returns>
    public bool CheckWin()
    {
        return (_activeBreakingCount <= 0) ? true : false;
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

    /// <summary>
    /// Игрок совершил ошибку
    /// </summary>
    private void PlayerError()
    {
        _errorCount++;
        Debug.Log("Player error! Count errors: " + errorCount);
        if (PlayerErrorEvent != null)
            PlayerErrorEvent();
        if (_errorCount >= errorCountMax)
            Losing();
    }

    protected override MiniGameResult GetResult()
    {
        if (_time <= 0)
            return MiniGameResult.TimeOut;
        else switch (_errorCount)
            {
                case 0:
                    return MiniGameResult.Gold;
                case 1:
                case 2:
                    return MiniGameResult.Silver;
                default:
                    return MiniGameResult.Bronze;
            }
    }
}