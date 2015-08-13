using UnityEngine;
using System.Collections;

public class MiniGame32_Manager : MiniGameSingleton<MiniGame32_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;

    /// <summary>
    /// Количество совершенных игроком ошибок
    /// </summary>
    private int _errorCount = 0;

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
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _errorCount = 0;
    }

	
	public void CloseMenu()
    {
        Hide();
    }

    public void CorrectButtonClick()
    {
        Win();
    }

    public void OtherButtonClick()
    {
        _errorCount++;
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
                return;
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        if (_time <= 0)
            return MiniGameResult.Bronze;
        return (_errorCount <= 0) ? MiniGameResult.Gold : (_errorCount <= 1) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}