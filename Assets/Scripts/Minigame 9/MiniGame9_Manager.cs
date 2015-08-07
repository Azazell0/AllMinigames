using UnityEngine;
using Minigame9;
using System;
using System.Collections;
using System.Collections.Generic;

public class MiniGame9_Manager : MiniGameSingleton<MiniGame9_Manager>
{
    #region variables

    /// <summary>
    /// Эвент победы
    /// </summary>
    public event MiniGameSimpleAction WinEvent;

    public GameObject protocol;
    /// <summary>
    /// Список всех персонажей
    /// </summary>
    public List<Person> listPersons;

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Текущий персонаж, которого выбрал игрок
    /// </summary>
    private Person _currentPerson;
    /// <summary>
    /// Список желаний, которые выбрал игрок у персонажей
    /// </summary>
    private List<PersonThought> _listChoisesThoughtes;
    /// <summary>
    /// Корректное желание в текущей игре
    /// </summary>
    private PersonThought _currentThought;

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
        _currentPerson = null;

        if (listPersons == null)
            listPersons = new List<Person>();
        else foreach (Person p in listPersons)
                if (p == null)
                    listPersons.Remove(p);
                else
                    p.Reset();

        if (_listChoisesThoughtes == null)
            _listChoisesThoughtes = new List<PersonThought>();
        else _listChoisesThoughtes.Clear();

        if (protocol != null)
            protocol.SetActive(false);

        int countValues = Enum.GetValues(typeof(PersonThought)).Length;
        _currentThought = (PersonThought)(UnityEngine.Random.Range(1, countValues));

        List<List<PersonThought>> aListPersons = new List<List<PersonThought>>();
        for (int i = 0; i < 10; i++)
        {
            List<PersonThought> list = new List<PersonThought>();
            // Для первых восьми персонажей добавляем мысль, которую игроку нужно найти
            if (i < 8)
                list.Add(_currentThought);
            // Добавляем случайные мысли
            while (list.Count < 3)
            {
                PersonThought pt = (PersonThought)(UnityEngine.Random.Range(1, countValues));
                if (!list.Contains(pt) && pt != _currentThought)
                    list.Add(pt);
            }
            // Сортируем список случайным образом
            SortListRandom(list, 10);
            aListPersons.Add(list);
        }
        SortListRandom(aListPersons, 20);

        // Устанавливаеи мысли персонажам
        for(int i = 0; i < listPersons.Count; i++)
        {
            if (aListPersons.Count > i)
                listPersons[i].SetThoughtes(aListPersons[i]);
        }
    }

    /// <summary>
    /// Рандомная сортировка списка
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="iterationsCount"></param>
    private void SortListRandom<T>(List<T> list, int iterationsCount)
    {
        for (int j = 0; j < iterationsCount; j++)
        {
            T tmp = list[0];
            list.RemoveAt(0);
            list.Insert(UnityEngine.Random.Range(0, list.Count), tmp);
        }
    }

    /// <summary>
    /// Инициализация новой игры
    /// </summary>
    /// <param name="time">Время для прохождения</param>
    public void NewGame(float time)
    {
        Init();

        _time = time;
        _isPlay = true;

        Show();
    }

    /// <summary>
    /// Попытка выбора персонажа
    /// </summary>
    /// <param name="person">Указатель на персонажа</param>
    public void WasClickPerson(Person person)
    {
        if (!isPlay)
            return;

        if (listPersons.Contains(person))
        {
            if (_currentPerson != null)
                _currentPerson.ShowThoughtes(false);
            if (_currentPerson == person)
                _currentPerson.ShowThoughtes(false);
            else
            {
                _currentPerson = person;
                _currentPerson.ShowThoughtes(true);
            }
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
            if (_time <= 0)
            {
                Debug.Log("Time is out!");
                Win();
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        int i = 0;
        foreach (Person p in listPersons)
            if (p != null && p.thoughtEnd.thoughtType == _currentThought)
                i++;

        switch (i)
        {
            case 10:
            case 9:
            case 8:
                return MiniGameResult.Gold;
            case 7:
            case 6:
                return MiniGameResult.Silver;
            case 0:
                return MiniGameResult.TimeOut;
            default:
                return MiniGameResult.Bronze;
        }
    }

    /// <summary>
    /// Победа в игре
    /// </summary>
    protected override void Win()
    {
        _isPlay = false;
        Debug.Log("WIN!");
        StartCoroutine(ShowResults());
        if (WinEvent != null)
            WinEvent();
    }

    private IEnumerator ShowResults()
    {
        if (protocol != null)
            protocol.SetActive(true);
        if (resultsMenu != null)
            resultsMenu.ShowResults(GetResult());
        yield return new WaitForSeconds(4f);
        if (resultsMenu != null)
            resultsMenu.Hide();
    }
}