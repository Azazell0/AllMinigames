using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MiniGame12_Manager : MiniGameSingleton<MiniGame12_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    public UILabel labelCounter1, labelCounter2, labelCounter3, labelCounter4, labelCounterSum1, labelCounterSum2, labelCounterSum3, labelCounterSum4;
    public UIInput inputCounter1, inputCounter2, inputCounter3, inputCounter4;
    public UISprite spriteRaznica, spriteResult;

    private float _timeLastLabelsUpdate = 0f;
    private bool _isCoroutine = false;
    private const int sumCounter1 = 8080;
    private const int sumCounter2 = 8620;
    private const int sumCounter3 = 9560;
    private const int sumCounter4 = 26260;

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
        _timeLastLabelsUpdate = 0f;
        _isCoroutine = false;

        MiniGameHelper.UIInputSetInteractable(true, true, inputCounter1, inputCounter2, inputCounter3, inputCounter4);
        MiniGameHelper.UILabelReset("", labelCounter1, labelCounter2, labelCounter3, labelCounter4);
        MiniGameHelper.UILabelReset(sumCounter1.ToString(), labelCounterSum1);
        MiniGameHelper.UILabelReset(sumCounter2.ToString(), labelCounterSum2);
        MiniGameHelper.UILabelReset(sumCounter3.ToString(), labelCounterSum3);
        MiniGameHelper.UILabelReset(sumCounter4.ToString(), labelCounterSum4);
        MiniGameHelper.SetSpriteAlpha(0, spriteRaznica, spriteResult);
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

    public void Input1Submit()
    {
        InputSubmit(inputCounter1, labelCounterSum1, sumCounter1);
    }

    public void Input2Submit()
    {
        InputSubmit(inputCounter2, labelCounterSum2, sumCounter2);
    }

    public void Input3Submit()
    {
        InputSubmit(inputCounter3, labelCounterSum3, sumCounter3);
    }

    public void Input4Submit()
    {
        InputSubmit(inputCounter4, labelCounterSum4, sumCounter4);
    }

    private void InputSubmit(UIInput input, UILabel labelSum, int startSum)
    {
        if (!_isPlay || input == null)
            return;

        int i = (input.value.Length > 0) ? Convert.ToInt32(input.value) : 0;

        if (labelSum != null)
            labelSum.text = (startSum + i).ToString();
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    public void CheckWin()
    {
        if (CheckString(labelCounter1, "420"))
            if (CheckString(labelCounter2, "380"))
                if (CheckString(labelCounter3, "440"))
                    if (CheckString(labelCounter4, "1240"))
                    {
                        MiniGameHelper.UIInputSetInteractable(false, false, inputCounter1, inputCounter2, inputCounter3, inputCounter4);
                        StartCoroutine(WinCoroutine());
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
            _timeLastLabelsUpdate -= Time.deltaTime;

            if (labelTime != null)
                labelTime.text = (((int)_time / 60)).ToString("00") + ":" + ((int)_time % 60).ToString("00");

            if (_timeLastLabelsUpdate < 0)
            {
                _timeLastLabelsUpdate += 0.1f;
                Input1Submit();
                Input2Submit();
                Input3Submit();
                Input4Submit();
                CheckWin();
            }

            if (_time <= 0)
            {
                Debug.Log("Time is out!");
                StartCoroutine(WinCoroutine());
            }
        }
    }

    private IEnumerator WinCoroutine()
    {
        _isCoroutine = true;
        _isPlay = false;
        if (spriteRaznica != null)
        {
            yield return StartCoroutine(MiniGameHelper.SetSpriteVisible(spriteRaznica, 0.03f));
            yield return new WaitForSeconds(3f);
        }
        if (spriteResult != null)
        {
            yield return StartCoroutine(MiniGameHelper.SetSpriteVisible(spriteResult, 0.03f));
            yield return new WaitForSeconds(3f);
        }
        _isCoroutine = false;
        base.Win();
    }

    private bool CheckString(UILabel labelCount, string i)
    {
        if (labelCount == null || labelCount.text.Length == 0)
            return false;

        string s = labelCount.text.Replace("|","");
        return (s == i);
    }

    protected override MiniGameResult GetResult()
    {
        if (CheckString(labelCounter1, "420"))
            if (CheckString(labelCounter2, "380"))
                if (CheckString(labelCounter3, "440"))
                    if (CheckString(labelCounter4, "1240"))
                        return MiniGameResult.Gold;
        return MiniGameResult.Bronze;
    }
}