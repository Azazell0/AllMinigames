using UnityEngine;
using Minigame17;
using System.Collections;
using System.Collections.Generic;

public class MiniGame17_Manager : MiniGameSingleton<MiniGame17_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Список всех плашек, которые нужно расставить игроку
    /// </summary>
    public List<Plate> listPlates;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;

    #endregion


    void Awake()
    {
        // Для синглтона
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        if (listPlates == null)
            listPlates = new List<Plate>();
        foreach (Plate plate in listPlates)
            plate.Reset();
    }
	
    void Update ()
    {
        if (_isPlay)
            CheckTime();
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
        Show();
        Init();

        _time = time;
        _isPlay = true;
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
            if (GetResult() == MiniGameResult.Gold)
            {
                Win();
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        int i = 0;
        foreach (Plate p in listPlates)
            if (p != null)
                if (p.currentCell != p.correctCell)
                    i++;

        return (i <= 0) ? MiniGameResult.Gold : (i == 1) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}