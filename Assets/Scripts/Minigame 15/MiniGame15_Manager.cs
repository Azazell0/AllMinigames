using UnityEngine;
using Minigame15;
using System.Collections;
using System.Collections.Generic;

public class MiniGame15_Manager : MiniGameSingleton<MiniGame15_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения количества нажатых объектов
    /// </summary>
    public UILabel labelCount;
    /// <summary>
    /// Трансформ, содержащий все кликабельные объекты
    /// </summary>
    public Transform containerObjects;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Список  объектов в игре
    /// </summary>
    private List<SceneObject> listObjects;
    /// <summary>
    /// Количество нажатых объектов
    /// </summary>
    private int _currentCount = 0;

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

    public void CloseMenu()
    {
        Hide();
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _currentCount = 0;
        if (listObjects == null)
        {
            listObjects = new List<SceneObject>();
            if (containerObjects != null)
                foreach (Transform t in containerObjects)
                {
                    SceneObject so = t.GetComponent<SceneObject>();
                    if (so != null)
                        listObjects.Add(so);
                }
        }
        else foreach (SceneObject so in listObjects)
                if (so != null)
                    so.Reset();

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

    /// <summary>
    /// Нажатие на кликабельный объект
    /// </summary>
    /// <param name="sObject">Указатель на объект</param>
    public void ClickObject(SceneObject sObject)
    {
        if (!isPlay)
            return;

        _currentCount++;
        UpdateCountLabel();
        CheckWin();
    }

    public void UpdateCountLabel()
    {
        if (labelCount != null)
            labelCount.text = _currentCount.ToString() + "/" + listObjects.Count.ToString();
    }

    /// <summary>
    /// Проверка условий победы
    /// </summary>
    private void CheckWin()
    {
        if (_currentCount >= listObjects.Count)
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
        int i = listObjects.Count - _currentCount;
        if (i <= 0)
            return MiniGameResult.Gold;
        if (i <= listObjects.Count * 0.5f)
            return MiniGameResult.Silver;
        return MiniGameResult.Bronze;
    }
}