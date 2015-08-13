using UnityEngine;
using Minigame4;
using System.Collections;
using System.Collections.Generic;

public class MiniGame4_Manager : MiniGameSingleton<MiniGame4_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Трансформ, содержащий все иконки
    /// </summary>
    public Transform containerIcons;

    /// <summary>
    /// Список всех иконок
    /// </summary>
    private List<Icon> listIcons;
    /// <summary>
    /// Список всех теней иконок
    /// </summary>
    private List<IconShadow> listIconsShadow;
    /// <summary>
    /// Выбранная на данный момент иконка
    /// </summary>
    private Icon _currentIcon;
    /// <summary>
    /// Выбранная на данный момент тень
    /// </summary>
    private IconShadow _currentShadow;
    /// <summary>
    /// true - на данный момент работает корутина
    /// </summary>
    private bool _isCoroutine = false;

    #endregion


    void Awake()
    {
        // Для синглтона
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        _timeToGame = 0f;
        _minigameName = "";
        _minigameDescription = "";
    }

    void Update()
    {
        if (_isPlay)
            CheckTime();
    }

    public void CloseMenu()
    {
        if (!_isCoroutine)
            Hide();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _currentIcon = null;

        if (listIcons == null)
            MiniGameHelper.FindChildObjects<Icon>(containerIcons, ref listIcons);
        foreach (Icon icon in listIcons)
            if (icon != null)
                icon.Reset();

        if (listIconsShadow == null)
            MiniGameHelper.FindChildObjects<IconShadow>(containerIcons, ref listIconsShadow);
        foreach (IconShadow shadow in listIconsShadow)
            if (shadow != null)
                shadow.Reset();
        MiniGameHelper.ListRandomSortTransformPositions<IconShadow>(ref listIconsShadow, 10);
    }

    public void ClickToggleIcon(Icon icon)
    {
        if (!isPlay || icon == null)
            return;

        if (_currentIcon != null && _currentIcon != icon)
            _currentIcon.SetPair(null);

        _currentIcon = (icon.value) ? icon : null;
        if (_currentShadow != null)
            CheckSelectedPair();
    }

    public void ClickToggleIconShadow(IconShadow shadow)
    {
        if (!isPlay || shadow == null)
            return;

        if (_currentShadow != null && _currentShadow != shadow)
            _currentShadow.SetPair(null);

        _currentShadow = (shadow.value) ? shadow : null;
        if (_currentIcon != null)
            CheckSelectedPair();
    }

    private void CheckSelectedPair()
    {
        if (!isPlay || _currentIcon == null || _currentShadow == null)
            return;

        _currentIcon.SetPair(_currentShadow);
        _currentShadow.SetPair(_currentIcon);
        _currentIcon = null;
        _currentShadow = null;

        CheckWin();
    }

    public void CheckWin()
    {
        if (!isPlay || listIcons == null || listIconsShadow == null)
            return;

        foreach (Icon icon in listIcons)
            if (icon.selectPair == null)
                return;
        
        foreach (IconShadow shadow in listIconsShadow)
            if (shadow.selectPair == null)
                return;
        
        foreach (IconShadow shadow in listIconsShadow)
            shadow.ShowCorrectPair();
        Win();
    }

    protected override void Win()
    {
        _isPlay = false;
        StartCoroutine(WinCoroutine(3f));
        
    }

    protected IEnumerator WinCoroutine(float delay)
    {
        _isCoroutine = true;
        yield return new WaitForSeconds(delay);
        base.Win();
        _isCoroutine = false;
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
        int i = 0;
        foreach (Icon icon in listIcons)
            if (icon.selectPair != icon.correctPair)
                i++;

        return (i == 0) ? MiniGameResult.Gold : (i == 1 || i == 2) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}