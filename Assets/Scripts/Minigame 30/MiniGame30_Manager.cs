using UnityEngine;
using Minigame30;
using System.Collections;
using System.Collections.Generic;

public class MiniGame30_Manager : MiniGameSingleton<MiniGame30_Manager>
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
    /// Трансформ, содержащий все машины
    /// </summary>
    public Transform containerCars;
    /// <summary>
    /// Трансформ, содержащий позиции, на которых могут появляться машины
    /// </summary>
    public Transform containerCarStartPositions;
    /// <summary>
    /// Тарнсформ, содержащий все гаражные клетки
    /// </summary>
    public Transform containerGarageCells;
    /// <summary>
    /// Тень гаража
    /// </summary>
    public GarageShadow garageShadow;
    /// <summary>
    /// Текущий цвет устанавливаемого гаража
    /// </summary>
    public MiniGame30_Color currentColor;
    /// <summary>
    /// Время между инстанцированием машин
    /// </summary>
    public float timeBetweenInstance = 2f;
    /// <summary>
    /// Задержка перед инстанцированием первой машины
    /// </summary>
    public float startDelay = 3f;
    /// <summary>
    /// Стартовая скорость первой машины
    /// </summary>
    public float startSpeed = 100f;
    /// <summary>
    /// Ускорение для каждой последующей инстанцируемой машины
    /// </summary>
    public float speedAcceleration = 5f;

    private const string pathGarage = "Prefabs/Minigame 30/Garage";
    private const string pathCar = "Prefabs/Minigame 30/Car";

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Время до появления следующей машины
    /// </summary>
    private float _timeToNextCarInstant = 3f;
    /// <summary>
    /// Список машин
    /// </summary>
    private List<Car> _listCars;
    /// <summary>
    /// Список неактивных машин
    /// </summary>
    private List<Car> _listDeactiveCars;
    /// <summary>
    /// Список всех гаражных клеток
    /// </summary>
    private List<GarageCell> _listGarageCells;
    /// <summary>
    /// Список стартовых позиций, где могут появляться машины
    /// </summary>
    private List<Vector3> _listStartPositions;
    /// <summary>
    /// Позиция, в которой появилась последняя машина
    /// </summary>
    private Vector3 _lastInstancePosition;
    /// <summary>
    /// Количество нажатых объектов
    /// </summary>
    private int _currentCount = 0;
    /// <summary>
    /// Количество совершенных ошибок
    /// </summary>
    private int _errorCount = 0;
    /// <summary>
    /// Текущая клетка с гаражем, на которую указывает курсор
    /// </summary>
    private GarageCell _currentCell;

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

    void FixedUpdate()
    {
        // Установка тени гаража
        if (_isPlay && garageShadow != null && UICamera.hoveredObject != null)
        {
            GarageCell cell = UICamera.hoveredObject.GetComponent<GarageCell>();
            if (_currentCell != cell)
            {
                _currentCell = cell;
                garageShadow.transform.position = (_currentCell == null) ? new Vector3(-10000, -10000, 0) : _currentCell.transform.position;
            }
        }
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _currentCount = 0;
        _errorCount = 0;
        _timeToNextCarInstant = startDelay;
        _currentCell = null;
        _lastInstancePosition = new Vector3(-141, -123123, 1234);

        UpdateCountLabel();

        if (_listDeactiveCars == null)
            _listDeactiveCars = new List<Car>();
        if (_listCars == null)
            _listCars = new List<Car>();
        // Переносим все машины, оставшиеся с прошлой игры в список неактивных
        else foreach(Car c in _listCars)
            if (c != null)
            {
                c.Hide();
                if (!_listDeactiveCars.Contains(c))
                    _listDeactiveCars.Add(c);
            }

        // Заносим все гаражные клетки в список и скрываем их
        if (_listGarageCells == null)
            MiniGameHelper.FindChildObjects<GarageCell>(containerGarageCells, ref _listGarageCells);
        
        foreach (GarageCell cell in _listGarageCells)
            if (cell != null && cell.garage != null)
                cell.garage.Hide();

        // Заносим в список возможные стартовые позиции для машин
        if (_listStartPositions == null)
        {
            _listStartPositions = new List<Vector3>();
            if (containerCarStartPositions != null)
                foreach (Transform t in containerCarStartPositions)
                    _listStartPositions.Add(t.localPosition);
        }
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

    #region Buttons Events

    public void CloseMenu()
    {
        Hide();
    }

    public void ClickRedButton()
    {
        if (!_isPlay)
            return;

        currentColor = MiniGame30_Color.Red;
        garageShadow.SetColor(currentColor);
    }

    public void ClickBlackButton()
    {
        if (!_isPlay)
            return;

        currentColor = MiniGame30_Color.Black;
        garageShadow.SetColor(currentColor);
    }

    public void ClickGreenButton()
    {
        if (!_isPlay)
            return;

        currentColor = MiniGame30_Color.Green;
        garageShadow.SetColor(currentColor);
    }

    public void ClickYellowButton()
    {
        if (!_isPlay)
            return;

        currentColor = MiniGame30_Color.Yellow;
        garageShadow.SetColor(currentColor);
    }

    #endregion

    /// <summary>
    /// Клик по гаражной клетке
    /// </summary>
    /// <param name="cell">Указатель на клетку</param>
    public void ClickCell(GarageCell cell)
    {
        if (!_isPlay || cell == null || garageShadow == null)
            return;

        // Показываем гараж в клетке и устанавливаем ему цвет. если гаража нет - инстанцируем
        if (cell.garage == null)
            cell.garage = MiniGameHelper.InstanceObject<Garage>(pathGarage, cell.transform);
        if (cell.garage != null)
        {
            cell.garage.Show();
            cell.garage.SetColor(garageShadow.color);
        }
    }

    /// <summary>
    /// Инстанцирует новую машину
    /// </summary>
    public void GetNewCar()
    {
        if (!_isPlay)
            return;

        Car c = (_listDeactiveCars == null || _listDeactiveCars.Count == 0) ? MiniGameHelper.InstanceObject<Car>(pathCar, containerCars) : _listDeactiveCars[0];

        if (c != null)
        {
            if (!_listCars.Contains(c))
                _listCars.Add(c);
            if (_listDeactiveCars.Contains(c))
                _listDeactiveCars.Remove(c);

            c.Show();

            MiniGameHelper.GetRandomObjectIfMay<Vector3>(_listStartPositions, ref _lastInstancePosition, _lastInstancePosition);
            c.transform.localPosition = _lastInstancePosition;
            c.SetRandomSettings(startSpeed + (_currentCount + _errorCount) * speedAcceleration);
        }
    }

    /// <summary>
    /// Машина припарковалась в гараж
    /// </summary>
    /// <param name="cCar">Указатель на машину</param>
    /// <param name="gGarage">Указатель на гараж</param>
    public void CarInTheGarage(Car cCar, Garage gGarage)
    {
        if (!_isPlay || cCar == null || gGarage == null)
            return;

        gGarage.Hide();
        cCar.Hide();
        if (!_listDeactiveCars.Contains(cCar))
            _listDeactiveCars.Add(cCar);

        if (cCar.color == gGarage.color)
        {
            _currentCount++;
            UpdateCountLabel();
        }
        else
            _errorCount++;
    }

    /// <summary>
    /// Машина не припарковалась в гараж
    /// </summary>
    /// <param name="cCar">Указатель на машину</param>
    public void CarInTheEndLine(Car cCar)
    {
        if (!_isPlay || cCar == null)
            return;

        cCar.Hide();
        if (!_listDeactiveCars.Contains(cCar))
            _listDeactiveCars.Add(cCar);
        _errorCount++;
    }

    /// <summary>
    /// Обновление лейбла, отображающего количество успешно припаркованных машин
    /// </summary>
    public void UpdateCountLabel()
    {
        if (labelCount != null)
            labelCount.text = _currentCount.ToString();
    }

    /// <summary>
    /// Проверка оставшегося времени до конца игры
    /// </summary>
    private void CheckTime()
    {
        if (_isPlay)
        {
            _time -= Time.deltaTime;
            _timeToNextCarInstant -= Time.deltaTime;

            if (labelTime != null)
                labelTime.text = (((int)_time / 60)).ToString("00") + ":" + ((int)_time % 60).ToString("00");

            if (_timeToNextCarInstant <= 0)
            {
                GetNewCar();
                _timeToNextCarInstant = timeBetweenInstance;
            }

            if (_time <= 0)
            {
                Debug.Log("Time is out!");
                Win();
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        return (_errorCount == 0) ? MiniGameResult.Gold : (_errorCount == 1) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}