﻿using UnityEngine;
using Minigame20;
using System.Collections;
using System.Collections.Generic;

public class MiniGame20_Manager : MiniGameSingleton<MiniGame20_Manager>
{
    #region variables

    /// <summary>
    /// Количество камер, которое нужно найти
    /// </summary>
    public int camerasCount = 10;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для указания количества найденных камер
    /// </summary>
    public UILabel labelCount;
    /// <summary>
    /// Трансформ, содержащий все камеры
    /// </summary>
    public Transform rootCameras;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Текущее количество найденных камер
    /// </summary>
    private int _cameraCount = 0;

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

    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
    }
    
    void Start ()
    {
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
        _cameraCount = 0;
        if (rootCameras != null)
            foreach (Transform t in rootCameras)
            {
                CameraPoint c = t.GetComponent<CameraPoint>();
                if (c != null)
                    c.Reset();
            }
        if (labelCount != null)
            labelCount.text = _cameraCount.ToString() + "/" + camerasCount.ToString();

        Show();

        _time = time;
        _isPlay = true;
    }

    /// <summary>
    /// Была найдена камера
    /// </summary>
    public void WasFindCamera()
    {
        if (!isPlay)
            return;

        _cameraCount++;
        if (labelCount != null)
            labelCount.text = _cameraCount.ToString() + "/" + camerasCount.ToString();
        CheckWin();
    }

    /// <summary>
    /// Проверка условий победы
    /// </summary>
    public void CheckWin()
    {
        if (!isPlay)
            return;

        if (_cameraCount >= camerasCount)
            Win();
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
        if (_time <= 0 && ((camerasCount - _cameraCount) >= 5))
            return MiniGameResult.TimeOut;
        else
            switch (camerasCount - _cameraCount)
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