using UnityEngine;
using Minigame31;
using System.Collections;
using System.Collections.Generic;

public class MiniGame31_Manager : MiniGameSingleton<MiniGame31_Manager>
{
    #region variables

    /// <summary>
    /// Время между инстансами мусора
    /// </summary>
    public float timeBetweenInstance = 2f;
    /// <summary>
    /// Персонаж игры
    /// </summary>
    public Person person;
    /// <summary>
    /// Трансформ, в который будет инстанцироваться мусор
    /// </summary>
    public Transform trashList;
    /// <summary>
    /// Трансформ, содержащий стартовые позиции для мусора
    /// </summary>
    public Transform startPositionsList;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;

    private const string pathTrashPrefab = "Prefabs/Minigame 31/Trash";

    /// <summary>
    /// Прошедшее время с последнего инстанцирования мусора
    /// </summary>
    private float _timeLastInstance = 0f;
    /// <summary>
    /// Значение Drag, использованное при последнем инстанцировании мусора
    /// </summary>
    private float _lastDragInstance = 5f;
    /// <summary>
    /// Позиция в которой был сгенерирован последний мусор
    /// </summary>
    private StartPosition _lastStartPosition;
    /// <summary>
    /// Количество мусора, пойманного игроком
    /// </summary>
    private int _countTrash;
    /// <summary>
    /// Количество мусора, которое игрок уронил
    /// </summary>
    private int _countLooseTrash;
    /// <summary>
    /// Список мусора, невидимого для игрока в данный момент
    /// </summary>
    private List<Trash> _listDeactiveTrash;
    private List<Trash> _listAllTrash;
    private List<StartPosition> _listStartPositions;

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
        _countTrash = 0;
        _countLooseTrash = 0;
        _timeLastInstance = 0f;
        _lastDragInstance = 3f;
        _lastStartPosition = null;

        if (_listDeactiveTrash == null)
            _listDeactiveTrash = new List<Trash>();

        if (_listAllTrash == null)
            _listAllTrash = new List<Trash>();
        else foreach (Trash trash in _listAllTrash)
                if (trash != null)
                {
                    trash.Hide();
                    if (!_listDeactiveTrash.Contains(trash))
                        _listDeactiveTrash.Add(trash);
                }

        if (_listStartPositions == null)
        {
            _listStartPositions = new List<StartPosition>();
            if (startPositionsList != null)
                foreach (Transform t in startPositionsList)
                {
                    StartPosition sp = t.GetComponent<StartPosition>();
                    if (sp != null && !_listStartPositions.Contains(sp))
                        _listStartPositions.Add(sp);
                }
        }

        if (person != null)
            person.SetRightUp();
    }

	
	public void CloseMenu()
    {
        Hide();
    }

    /// <summary>
    /// Устанавливает новый мусор
    /// </summary>
    public void GetTrash()
    {
        if (!isPlay)
            return;

        Trash trash = null;
        if (_listDeactiveTrash.Count == 0)
        {
            trash = MiniGameHelper.InstanceObject<Trash>(pathTrashPrefab, trashList);
            if (trash != null)
                _listAllTrash.Add(trash);
        }
        else
        {
            trash = _listDeactiveTrash[0];
            _listDeactiveTrash.RemoveAt(0);
        }

        if (trash != null)
        {
            trash.gameObject.SetActive(true);
            SetRandomStartPosition(trash);
            Rigidbody r = trash.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.drag = _lastDragInstance;
                _lastDragInstance -= 0.1f;
            }
        }
    }

    /// <summary>
    /// Возвращает рандомную стартовую позицию для мусора
    /// </summary>
    /// <returns>Рандомная глобальная позиция transform.position</returns>
    private void SetRandomStartPosition(Trash trash)
    {
        if (trash == null || _listStartPositions == null || _listStartPositions.Count == 0)
            return;

        StartPosition sp = null;
        if (_listStartPositions.Count == 1)
            sp = _listStartPositions[0];
        else while (sp == null || sp == _lastStartPosition)
            sp = _listStartPositions[Random.Range(0, _listStartPositions.Count)];
        
        if (sp != null)
        {
            trash.transform.position = sp.transform.position;
            trash.currentDirection = sp.direction;
            _lastStartPosition = sp;
        }
    }

    /// <summary>
    /// Игрок поймал мусор
    /// <param name="trash">Указатель на мусор</param>
    /// </summary>
    public void TrashInTheBasket(Trash trash)
    {
        //Debug.Log(trash.currentDirection + "   " + person.currentDirection);
        if (!isPlay)
            return;
        if (person == null)
        {
            Debug.LogError("Person is null");
            return;
        }

        if (trash != null && trash.currentDirection == person.currentDirection)
        {
            _countTrash++;
            trash.Hide();
            if (!_listDeactiveTrash.Contains(trash))
                _listDeactiveTrash.Add(trash);
        }
    }

    /// <summary>
    /// Мусор на полу
    /// </summary>
    /// <param name="trash">Указатель на мусор</param>
    public void TrashOnTheFloor(Trash trash)
    {
        if (trash != null)
        {
            trash.Hide();
            if (!_listDeactiveTrash.Contains(trash))
                _listDeactiveTrash.Add(trash);
            if (isPlay)
                _countLooseTrash++;
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
                Win();
                return;
            }

            _timeLastInstance += Time.deltaTime;
            if (_timeLastInstance > timeBetweenInstance)
            {
                GetTrash();
                _timeLastInstance -= timeBetweenInstance;
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        return (_countLooseTrash <= 0) ? MiniGameResult.Gold : (_countTrash <= 1) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}