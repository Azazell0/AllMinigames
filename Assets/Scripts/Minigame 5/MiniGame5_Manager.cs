using UnityEngine;
using Minigame5;
using System.Collections;
using System.Collections.Generic;

public class MiniGame5_Manager : MiniGameSingleton<MiniGame5_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    public List<ButtonToggle> listButtons;

    private int _errorCount = 0;
    private bool _isCoroutine = false;
    private ButtonToggle _currentButton; 

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
        if (!_isCoroutine)
            Hide();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _errorCount = 0;
        _isCoroutine = false;
        _currentButton = null;

        if (listButtons == null)
            listButtons = new List<ButtonToggle>();
        foreach (ButtonToggle bt in listButtons)
            if (bt != null)
                bt.Reset();

        if (listButtons.Count > 0)
            _currentButton = listButtons[0];
    }

    public void ClickButtonToggle(ButtonToggle bt)
    {
        if (!isPlay || bt == null)
            return;
        
        if (bt != _currentButton)
        {
            bt.SetToggleValue(false);
            _errorCount++;
            return;
        }
        StartCoroutine(bt.Set());
        int index = listButtons.IndexOf(_currentButton);
        if (index >= 0)
        {
            if (index < listButtons.Count - 1)
                _currentButton = listButtons[index + 1];
            else
                Win();
        }
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
        if (_time <= 0)
            return MiniGameResult.Bronze;
        return (_errorCount == 0) ? MiniGameResult.Gold : (_errorCount > 0 && _errorCount < 3) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}