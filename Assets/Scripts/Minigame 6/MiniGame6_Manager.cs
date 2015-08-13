using UnityEngine;
using Minigame6;
using System.Collections;
using System.Collections.Generic;

public class MiniGame6_Manager : MiniGameSingleton<MiniGame6_Manager>
{
    #region variables

    /// <summary>
    /// Карта, которая сейчас используется
    /// </summary>
    public Card currentCard { get { return _currentCard; } }

    /// <summary>
    /// Контейнер, содержащий все карты
    /// </summary>
    public Transform CardContainer;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;

    /// <summary>
    /// Карта, которая сейчас используется
    /// </summary>
    private Card _currentCard;

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
        if (CardContainer != null)
        {
            // Перемешивание карт
            for (int i = 0; i < 2; i++)
            {
                Transform t1 = CardContainer.GetChild(UnityEngine.Random.Range(0, CardContainer.childCount));
                Transform t2 = CardContainer.GetChild(UnityEngine.Random.Range(0, CardContainer.childCount));
                if (t1 != t2)
                {
                    Vector3 v = t1.position;
                    t1.position = t2.position;
                    t2.position = v;
                }
            }
        }
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
    /// Устанавливает карту, над которой производится действие
    /// </summary>
    /// <param name="card">Указатель на карту</param>
    public void SetCurrentCard(Card card)
    {
        if (!isPlay)
            return;

        _currentCard = card;
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    public void CheckWin()
    {
        if (!isPlay || CardContainer == null)
            return;

        foreach(Transform t in CardContainer)
        {
            Card c = t.GetComponent<Card>();
            if (c != null && c.transform.position != c.startPosition)
                return;
        }

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
        return (_time > 0) ? MiniGameResult.Gold : MiniGameResult.Bronze;
    }
}