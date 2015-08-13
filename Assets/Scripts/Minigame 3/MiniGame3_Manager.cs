using UnityEngine;
using Minigame3;
using System.Collections;
using System.Collections.Generic;

public class MiniGame3_Manager : MiniGameSingleton<MiniGame3_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения количества оставшихся в запасе пугалок
    /// </summary>
    public UILabel labelWeight;
    /// <summary>
    /// Кнопка запуска лифта
    /// </summary>
    public UIButton buttonElevator;
    /// <summary>
    /// Указатель на лифт
    /// </summary>
    public Elevator elevator;
    /// <summary>
    /// Максимально допустимый вес
    /// </summary>
    public int maxWeight = 300;
    /// <summary>
    /// Трансформ, содержащий всех персонажей
    /// </summary>
    public Transform containerPersons;
    /// <summary>
    /// Объект для блокирования нажатий на персонажей во время анимации лифта
    /// </summary>
    public GameObject blockObject;

    /// <summary>
    /// Общий вес выбранных персонажей
    /// </summary>
    private int _weight = 0;
    /// <summary>
    /// Количество запусков лифта
    /// </summary>
    private int _countElevatorStat = 0;
    /// <summary>
    /// Список, содержащий всех персонажей
    /// </summary>
    private List<Person> _listPersons;
    /// <summary>
    /// true - в данный момент работает корутина
    /// </summary>
    private bool _isCoroutineWork = false;

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
        if (!_isCoroutineWork)
            Hide();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _isCoroutineWork = false;
        _countElevatorStat = 0;
        _weight = 0;
        UpdateWeightLabel();

        if (_listPersons == null)
            MiniGameHelper.FindChildObjects(containerPersons, ref _listPersons);
        else foreach (Person p in _listPersons)
                if (p != null)
                    p.Reset();
    }

    /// <summary>
    /// Попытка выбора персонажа
    /// </summary>
    /// <param name="person">Указатель на персонажа</param>
    public void WasClickPerson(Person person)
    {
        if (!isPlay || person == null)
            return;

        _weight += (person.isSelected) ? person.weight : -person.weight;
        _weight = Mathf.Clamp(_weight, 0, 10000);
        UpdateWeightLabel();
    }

    /// <summary>
    /// Нажатие на кнопку пуска лифта
    /// </summary>
    public void ElevatorButtonClick()
    {
        if (!isPlay || _weight > maxWeight)
            return;
        StartCoroutine(ElevatorButtonClickCoroutine());
    }

    /// <summary>
    /// Корутина, обрабатывающая запуск лифта
    /// </summary>
    private IEnumerator ElevatorButtonClickCoroutine()
    {
        _isCoroutineWork = true;

        // Скрываем всех персонажей, которых игрок выбрал перед запуском лифта
        if (_listPersons != null)
            foreach (Person p in _listPersons)
                if (p.isSelected)
                    p.Hide();

        // Отключаем кнопку лифта и ждем, пока анимация лифта проиграется
        ButtonElevatorActive(false);
        yield return StartCoroutine(elevator.StartElevator());
        ButtonElevatorActive(true);
        _weight = 0;
        UpdateWeightLabel();
        _countElevatorStat++;

        // Проверяем, остались ли неотправленные персонажи
        Person per = _listPersons.Find(delegate(Person pp) { return pp.isFinish == false; });
        if (per == null)
            Win();

        _isCoroutineWork = false;
    }

    /// <summary>
    /// Обновление лейбла, отображающего общий вес персонажей в кабине лифта
    /// </summary>
    public void UpdateWeightLabel()
    {
        if (labelWeight != null)
            labelWeight.text = _weight.ToString();
    }

    /// <summary>
    /// Переключение состояния кнопки лифта и замена текстуры
    /// </summary>
    /// <param name="b">true - игрок может нажимать кнопку лифта</param>
    private void ButtonElevatorActive(bool b)
    {
        if (buttonElevator != null)
        {
            UISprite s = buttonElevator.GetComponent<UISprite>();
            if (s != null)
                s.spriteName = (b) ? "3_buttonON" : "3_buttonOFF";
            buttonElevator.enabled = b;
        }
        if (blockObject != null)
            blockObject.SetActive(!b);
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
        if (_time <= 0)
            return MiniGameResult.Bronze;
        switch(_countElevatorStat)
        {
            case 1:
            case 2:
                return MiniGameResult.Gold;
            case 3:
                return MiniGameResult.Silver;
            default:
                return MiniGameResult.Bronze;
        }
    }
}