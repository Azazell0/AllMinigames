using UnityEngine;
using Minigame19;
using System.Collections;
using System.Collections.Generic;

public class MiniGame19_Manager : MiniGameSingleton<MiniGame19_Manager>
{
    #region variables

    /// <summary>
    /// Контейнер, содержащий все инструменты
    /// </summary>
    public Transform containerInstruments;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения количества правильных ответов
    /// </summary>
    public UILabel labelCount;
    public UIButton button1, button2, button3, button4;
    public UILabel button1Label, button2Label, button3Label, button4Label;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Количество неправильных ответов
    /// </summary>
    private int _errorCount = 0;
    /// <summary>
    /// Количество правильных ответов
    /// </summary>
    private int _correctAnswerCount = 0;
    private Instrument currentInstrument;
    /// <summary>
    /// Список всех инструментов
    /// </summary>
    private List<Instrument> _listInstruments;
    /// <summary>
    /// Список всех инструментов в текущей игре
    /// </summary>
    private List<Instrument> _listInstrumentsNow;
    private List<string> _listInstrumentStrings;

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
        _errorCount = 0;
        _correctAnswerCount = 0;

        currentInstrument = null;

        if (_listInstruments == null)
            MiniGameHelper.FindChildObjects<Instrument>(containerInstruments, ref _listInstruments);
        foreach (Instrument instrument in _listInstruments)
            instrument.Hide();

        _listInstrumentsNow = new List<Instrument>(_listInstruments);

        if (_listInstrumentStrings == null)
        {
            _listInstrumentStrings = new List<string>();
            foreach (Instrument instrument in _listInstruments)
            {
                string s = ConvertInstrumentTypeToString(instrument.type);
                if (!_listInstrumentStrings.Contains(s))
                    _listInstrumentStrings.Add(s);
            }
        }

        UpdateCountLabel();
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
        Init();
        Show();

        _time = time;
        _isPlay = true;
        StartCoroutine(SetNewRandomInstrument());
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    public void CheckWin()
    {
        if (!isPlay || _listInstrumentsNow == null)
            return;

        if (_listInstrumentsNow.Count == 0)
            Win();
    }

    public void ClickButton1()
    {
        if (!_isPlay || currentInstrument == null || button1Label == null)
            return;

        if (CheckAnswer(currentInstrument.type, button1Label.text))
            _correctAnswerCount++;
        else
            _errorCount++;
        UpdateCountLabel();

        StartCoroutine(SetNewRandomInstrument());
    }

    public void ClickButton2()
    {
        if (!_isPlay || currentInstrument == null || button2Label == null)
            return;

        if (CheckAnswer(currentInstrument.type, button2Label.text))
            _correctAnswerCount++;
        else
            _errorCount++;
        UpdateCountLabel();

        StartCoroutine(SetNewRandomInstrument());
    }

    public void ClickButton3()
    {
        if (!_isPlay || currentInstrument == null || button3Label == null)
            return;

        if (CheckAnswer(currentInstrument.type, button3Label.text))
            _correctAnswerCount++;
        else
            _errorCount++;
        UpdateCountLabel();

        StartCoroutine(SetNewRandomInstrument());
    }

    public void ClickButton4()
    {
        if (!_isPlay || currentInstrument == null || button4Label == null)
            return;

        if (CheckAnswer(currentInstrument.type, button4Label.text))
            _correctAnswerCount++;
        else
            _errorCount++;
        UpdateCountLabel();

        StartCoroutine(SetNewRandomInstrument());
    }

    public void UpdateCountLabel()
    {
        if (labelCount != null)
            labelCount.text = _correctAnswerCount.ToString();
    }

    public string ConvertInstrumentTypeToString(InstrumentType type)
    {
        switch (type)
        {
            case InstrumentType.Saw:
                return "Пила";

            case InstrumentType.Chisel:
                return "Стамеска";

            case InstrumentType.Nippers:
                return "Кусачки";

            case InstrumentType.Hammer:
                return "Молоток";

            case InstrumentType.Fretsaw:
                return "Лобзик";

            case InstrumentType.Trowel:
                return "Кельма";

            case InstrumentType.Pliers:
                return "Плоскогубцы";

            case InstrumentType.Drill:
                return "Дрель";

            case InstrumentType.Axe:
                return "Топор";

            case InstrumentType.Wrench:
                return "Гаечный ключ";

            default:
                return "";
        }
    }

    private bool CheckAnswer(InstrumentType type, string s)
    {
        return (ConvertInstrumentTypeToString(type) == s);
    }

    private IEnumerator SetNewRandomInstrument()
    {
        if (_isPlay && _listInstrumentsNow != null)
        {
            if (_listInstrumentsNow.Count == 0)
            {
                currentInstrument = null;
                Win();
            }
            else
            {
                SetActiveButtons(false);
                if (currentInstrument != null)
                    yield return StartCoroutine(currentInstrument.HideCoroutine(0.05f));

                int i = Random.Range(0, _listInstrumentsNow.Count);
                currentInstrument = _listInstrumentsNow[i];
                _listInstrumentsNow.RemoveAt(i);

                string s1 = "", s2 = "", s3 = "", s4 = "";
                s1 = ConvertInstrumentTypeToString(currentInstrument.type);
                MiniGameHelper.GetRandomObjectIfMay(_listInstrumentStrings, ref s2, s1);
                MiniGameHelper.GetRandomObjectIfMay(_listInstrumentStrings, ref s3, s1, s2);
                MiniGameHelper.GetRandomObjectIfMay(_listInstrumentStrings, ref s4, s1, s2, s3);

                List<string> l = new List<string>();
                l.Add(s1); l.Add(s2); l.Add(s3); l.Add(s4);
                MiniGameHelper.ListRandomSort<string>(ref l, 10);

                SetButtonsLabelText(l[0], l[1], l[2], l[3]);

                if (currentInstrument != null)
                    yield return StartCoroutine(currentInstrument.ShowCoroutine(0.05f));
                SetActiveButtons(true);
            }
        }
    }

    private void SetActiveButtons(bool b)
    {
        if (button1 != null)
            button1.enabled = b;
        if (button2 != null)
            button2.enabled = b;
        if (button3 != null)
            button3.enabled = b;
        if (button4 != null)
            button4.enabled = b;
    }

    private void SetButtonsLabelText(string s1, string s2, string s3, string s4)
    {
        if (button1Label != null)
            button1Label.text = s1;
        if (button2Label != null)
            button2Label.text = s2;
        if (button3Label != null)
            button3Label.text = s3;
        if (button4Label != null)
            button4Label.text = s4;
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
        int r = _errorCount + ((currentInstrument != null) ? 1 : 0) + _listInstrumentsNow.Count;
        return (r == 0) ? MiniGameResult.Gold : (r <= 2) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}