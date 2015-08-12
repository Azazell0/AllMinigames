using UnityEngine;
using Minigame22;
using System.Collections;
using System.Collections.Generic;

public class MiniGame22_Manager : MiniGameSingleton<MiniGame22_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения количества ненайденных источников шума
    /// </summary>
    public UILabel labelCount;
    /// <summary>
    /// Трансформ, содержащий все источники шума
    /// </summary>
    public Transform containerNoiseSources;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Список всех источников шума
    /// </summary>
    private List<NoiseSource> _listNoiseSources;
    /// <summary>
    /// Список источников шума, которые осталось найти в текущей игре
    /// </summary>
    private List<NoiseSource> _listNoiseSourcesNow;

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
        if (_listNoiseSources == null)
            MiniGameHelper.FindChildObjects<NoiseSource>(containerNoiseSources, ref _listNoiseSources);
        _listNoiseSourcesNow = new List<NoiseSource>(_listNoiseSources);
        UpdateCountLabel();
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

    public void WasFindNoise(NoiseSource ns)
    {
        if (!_isPlay || ns == null)
            return;

        _listNoiseSourcesNow.Remove(ns);
        UpdateCountLabel();
        if (_listNoiseSourcesNow.Count == 0)
            Win();
    }

    public void UpdateCountLabel()
    {
        if (labelCount != null)
            labelCount.text = _listNoiseSourcesNow.Count.ToString();
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
        return (_listNoiseSourcesNow.Count == 0) ? MiniGameResult.Gold : (_listNoiseSourcesNow.Count == 1) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}