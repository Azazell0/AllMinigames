using UnityEngine;
using Minigame24;
using System.Collections;
using System.Collections.Generic;

public class MiniGame24_Manager : MiniGameSingleton<MiniGame24_Manager>
{
    #region variables

    /// <summary>
    /// Ресурс, который сейчас используется
    /// </summary>
    public Resource currentResource { get { return _currentResource; } }

    /// <summary>
    /// Контейнер, содержащий все ячейки
    /// </summary>
    public Transform FieldCell;
    /// <summary>
    /// Контейнер, содержащий все ресурсы на поле
    /// </summary>
    public Transform Field;
    /// <summary>
    /// Стартовый статический ресурс
    /// </summary>
    public Resource resourceStart;
    /// <summary>
    /// конечный статический ресурс
    /// </summary>
    public Resource resourceFinish;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;

    private const string pathTubeLine = "Prefabs/Minigame 24/TubeLine";
    private const string pathTubeAngle = "Prefabs/Minigame 24/TubeAngle";
    private const string pathController = "Prefabs/Minigame 24/Controller";
    private const string pathCounter = "Prefabs/Minigame 24/Counter";
    private const string pathPump = "Prefabs/Minigame 24/Pump";
    private const string pathValve = "Prefabs/Minigame 24/Valve";

    /// <summary>
    /// Ресурс, который сейчас используется
    /// </summary>
    private Resource _currentResource;
    /// <summary>
    /// Текущий тип инстанциируемого ресурса
    /// </summary>
    private ResourceType _currentResourceType;
    /// <summary>
    /// Список всех ресурсов, которые входят в построенную систему
    /// </summary>
    private List<Resource> systemList;

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
        _currentResource = null;
        _currentResourceType = ResourceType.None;

        if (systemList == null)
            systemList = new List<Resource>();
        else
            systemList.Clear();

        if (Field != null)
            foreach(Transform t in Field)
            {
                Resource r = t.GetComponent<Resource>();
                if (r != null)
                    if (r.currentCell != null && !r.currentCell.isStatic)
                        Destroy(t.gameObject);
            }
    }
	
    void Update ()
    {
        if (_isPlay)
        {
            CheckTime();
            if (Input.GetMouseButtonUp(0))
                _currentResource = null;
        }
    }

    public void CloseMenu()
    {
        Hide();
    }

    public void SetCurrentResource(Resource resource)
    {
        if (!isPlay)
            return;

        _currentResource = resource;
    }

    /// <summary>
    /// Устанавливает новый ресурс в клетку
    /// </summary>
    /// <param name="cell">Клетка</param>
    public void SetNewResource(Cell cell)
    {
        if (!isPlay || cell == null || cell.currentResource != null)
            return;

        string s = "";
        switch (_currentResourceType)
        {
            case ResourceType.TubeLine:
                s = pathTubeLine;
                break;
            case ResourceType.TubeAngle:
                s = pathTubeAngle;
                break;
            case ResourceType.Controller:
                s = pathController;
                break;
            case ResourceType.Counter:
                s = pathCounter;
                break;
            case ResourceType.Pump:
                s = pathPump;
                break;
            case ResourceType.Valve:
                s = pathValve;
                break;
        }
        if (s.Length == 0)
            return;

        GameObject go = Instantiate(Resources.Load(s)) as GameObject;
        if (go != null)
        {
            if (Field != null)
            {
                Vector3 scale = go.transform.localScale;
                go.transform.parent = Field;
                go.transform.localScale = scale;
            }

            Resource r = go.GetComponent<Resource>();
            if (r != null)
                SetResourceToCell(cell, r);
        }
    }

    /// <summary>
    /// Устанавливает существующий ресурс в клетку
    /// </summary>
    /// <param name="cell">Клетка</param>
    /// <param name="resource">Ресурс</param>
    public void SetResourceToCell(Cell cell, Resource resource)
    {
        if (!isPlay)
            return;

        if (resource.currentCell != null)
            resource.currentCell.currentResource = null;
        resource.currentCell = cell;
        cell.currentResource = resource;
        resource.transform.position = cell.transform.position;
        CheckWin();
    }

    /// <summary>
    /// Устанавливает текущий ресурс
    /// </summary>
    /// <param name="type">Тип ресурса</param>
    public void SetCurrentResourceType(ResourceType type)
    {
        if (!isPlay)
            return;

        _currentResourceType = type;
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    public void CheckWin()
    {
        if (!isPlay || resourceStart == null || resourceFinish == null )
            return;

        if (systemList == null)
            systemList = new List<Resource>();
        else
            systemList.Clear();
        if (FindFinish(systemList, resourceStart, resourceFinish, Direction.Down, Direction.Left))
        {
            // Проверяем, чтобы в собранной системе были все необходимые модули
            bool b1 = false, b2 = false, b3 = false, b4 = false;
            foreach (Resource r in systemList)
            {
                if (r == null)
                    continue;
                switch(r.type)
                {
                    case ResourceType.Controller:
                        b1 = true;
                        break;
                    case ResourceType.Counter:
                        b2 = true;
                        break;
                    case ResourceType.Pump:
                        b3 = true;
                        break;
                    case ResourceType.Valve:
                        b4 = true;
                        break;
                }
            }

            if (b1 == b2 == b3 == b4 == true)
                Win();
        }
    }

    private bool FindFinish(List<Resource> list, Resource start, Resource finish, Direction startDirection, Direction finishDirection)
    {
        if (start == null)
            return false;
        // Добавляем стартовый элемент, он заведомо корректный
        list.Add(start);
        
        // Получаем ячейку стартового элемента
        Cell cell = start.currentCell;
        if (cell == null)
            return false;
        
        // Получаем направления, которые соединяет стартовый элемент
        List<Direction> dirList = start.GetDirections();
        if (dirList == null)
            return false;
        // Ищем новое направление
        Direction newDirection = Direction.Down;
        bool b = false;
        foreach (Direction d in dirList)
            if (d != startDirection)
            {
                newDirection = d;
                b = true;
            }
        if (!b)
            return false;
        
        // Ищем соседнюю клетку по найденному направлению
        Cell newCell = null;
        switch(newDirection)
        {
            case Direction.Up:
                newCell = cell.cellUp;
                break;
            case Direction.Right:
                newCell = cell.cellRight;
                break;
            case Direction.Down:
                newCell = cell.cellDown;
                break;
            case Direction.Left:
                newCell = cell.cellLeft;
                break;
        }
        
        if (newCell == null || newCell.currentResource == null)
            return false;

        // Смотрим, есть ли у элемента в новой клетке проход к текущей
        List<Direction> l = newCell.currentResource.GetDirections();
        if (l == null || !l.Contains(GetInvertDirection(newDirection)))
            return false;
        
        // Если на соседней клетке искомый элемент, проверяем, можем ли мы до него дойти с текущей клетки
        if (newCell.currentResource == finish)
        {
            if ((finishDirection == Direction.Up && newDirection == Direction.Down) ||
                (finishDirection == Direction.Right && newDirection == Direction.Left) ||
                (finishDirection == Direction.Down && newDirection == Direction.Up) ||
                (finishDirection == Direction.Left && newDirection == Direction.Right))
            {
                list.Add(finish);
                return true;
            }
            else return false;
        }
        else return FindFinish(list, newCell.currentResource, finish, GetInvertDirection(newDirection), finishDirection);
    }

    private Direction GetInvertDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Right:
                return Direction.Left;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            default:
                return Direction.Up;
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

        if (Field == null)
        {
            Debug.LogWarning("Field == null");
            return MiniGameResult.Gold;
        }
        int count = systemList.Count;
        if (count < 15)
            return MiniGameResult.Gold;
        else if (count < 18)
            return MiniGameResult.Silver;
        else return MiniGameResult.Bronze;
    }
}