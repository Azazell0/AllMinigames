﻿using UnityEngine;
using Minigame18;
using System.Collections;
using System.Collections.Generic;

public class MiniGame18_Manager : MiniGameSingleton<MiniGame18_Manager>
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
    /// Список со всеми ошибками
    /// </summary>
    private List<BlankError> _listBlankError;

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
        if (_listBlankError == null)
            _listBlankError = new List<BlankError>();
        else foreach (BlankError be in _listBlankError)
            if (be != null)
                be.Reset();

        if (_listBlankError.Count > 0)
            UpdateCountLabel();
    }

	
	public void CloseMenu()
    {
        Hide();
    }

    /// <summary>
    /// найдена ошибка в бланке
    /// </summary>
    public void WasFindBlankError()
    {
        UpdateCountLabel();
        ChechWin();
    }

    /// <summary>
    /// Регистрирует ошибку в бланке
    /// </summary>
    /// <param name="error">Указатель на ошибку</param>
    public void RegisteryBlankError(BlankError error)
    {
        if (_listBlankError == null)
            _listBlankError = new List<BlankError>();
        if (!_listBlankError.Contains(error))
            _listBlankError.Add(error);
    }

    /// <summary>
    /// Проверка на победу
    /// </summary>
    private void ChechWin()
    {
        foreach (BlankError error in _listBlankError)
            if (!error.wasFind)
                return;
        Win();
    }

    /// <summary>
    /// Обновляет лейбл с указание количества найденных ошибок
    /// </summary>
    private void UpdateCountLabel()
    {
        if (labelCount != null)
            labelCount.text = GetFindErrorsCount().ToString() + "/" + _listBlankError.Count.ToString();
    }

    /// <summary>
    /// Возвращает количество найденных ошибок
    /// </summary>
    /// <returns></returns>
    private int GetFindErrorsCount()
    {
        int i = 0;
        foreach (BlankError error in _listBlankError)
            if (error.wasFind)
                i++;
        return i;
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
                if (GetFindErrorsCount() < 3)
                    Losing();
                else
                    Win();
                return;
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        int i = GetFindErrorsCount();
        if (i >= 10)
            return MiniGameResult.Gold;
        else if (i >= 8)
            return MiniGameResult.Silver;
        else
            return MiniGameResult.Bronze;
    }
}