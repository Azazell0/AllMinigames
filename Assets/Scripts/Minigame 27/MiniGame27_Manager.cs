using UnityEngine;
using Minigame27;
using System.Collections;
using System.Collections.Generic;

public class MiniGame27_Manager : MiniGameSingleton<MiniGame27_Manager>
{
    #region variables

    /// <summary>
    /// true - если хотя бы одна тень активна
    /// </summary>
    public bool isShadowBlock { get { return _shadowBlock; } }
    /// <summary>
    /// Выделенная клетка
    /// </summary>
    public Cell currentCell { get { return _currentCell; } }

    /// <summary>
    /// Трансформ, содержащий все клетки
    /// </summary>
    public Transform cellContainer;
    /// <summary>
    /// Трансформ, содержащий все тени
    /// </summary>
    public Transform shadowContainer;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения набранных очков
    /// </summary>
    public UILabel labelPoints;

    private const string pathShadowPrefab = "Prefabs/Minigame 27/Shadow";

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// true - если хотя бы одна тень активна
    /// </summary>
    private bool _shadowBlock = false;
    /// <summary>
    /// Количество набранных очков
    /// </summary>
    private int _points = 0;
    /// <summary>
    /// Выделенная клетка
    /// </summary>
    private Cell _currentCell;
    /// <summary>
    /// Список всех клеток
    /// </summary>
    private List<Cell> _listCell;
    /// <summary>
    /// Список активных теней
    /// </summary>
    private List<Shadow> _listActiveShadow;
    /// <summary>
    /// Список неактивных теней
    /// </summary>
    private List<Shadow> _listDeactiveShadow;

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

    void LateUpdate()
    {
        // Сброс текущей клетки при отпускании левой кнопки мыши
        if (Input.GetMouseButtonUp(0))
            _currentCell = null;
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _points = 0;
        _shadowBlock = false;

        // Добавляем все клетки в список
        if (_listCell == null)
        {
            _listCell = new List<Cell>();
            if (cellContainer != null)
                foreach (Transform t in cellContainer)
                {
                    Cell c = t.GetComponent<Cell>();
                    if (c != null)
                        _listCell.Add(c);
                }
        }
        // Ресет клеток
        else foreach (Cell c in _listCell)
                if (c != null)
                    c.Reset();

        if (_listDeactiveShadow == null)
            _listDeactiveShadow = new List<Shadow>();

        // Деактивируем все тени
        if (_listActiveShadow == null)
            _listActiveShadow = new List<Shadow>();
        else if (_listActiveShadow.Count > 0)
        {
            for (int i = _listActiveShadow.Count - 1; i >= 0; i--)
            {
                _listActiveShadow[i].Hide();
                if (!_listDeactiveShadow.Contains(_listActiveShadow[i]))
                    _listDeactiveShadow.Add(_listActiveShadow[i]);
                _listActiveShadow.RemoveAt(i);
            }
        }

        UpdatePointsLabel();
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

        // При старте игры запускаем тени ко всем клеткам
        foreach (Cell c in _listCell)
            if (c != null)
                SendShadowToCell(c, c.transform.localPosition + new Vector3(0f, 445f, 0f));
    }

    /// <summary>
    /// Установка текущей клетки
    /// </summary>
    /// <param name="cell">Указатель на клетку</param>
    public void SetCurrentCell(Cell cell)
    {
        _currentCell = cell;
    }

    /// <summary>
    /// Отправляет тень от одной клетки к другой (Перемещает объект из клетки в пустую клетку)
    /// </summary>
    /// <param name="fromCell">Клетка, от которой отправляется (тень принимает ее значение)</param>
    /// <param name="toCell">Клетка-цель</param>
    public void SendShadowFromCellToCell(Cell fromCell, Cell toCell)
    {
        if (!isPlay)
            return;

        if (fromCell == null || toCell == null)
            return;

        Shadow shadow = GetShadow();
        if (shadow != null)
        {
            shadow.transform.position = fromCell.transform.position;
            shadow.type = fromCell.type;
            shadow.StartMove(toCell);
            _shadowBlock = true;
            fromCell.type = ObjectType.o0;
        }
    }

    /// <summary>
    /// Отправлятет тени от одной клетки к другой и обратно (Меняет клетки местами)
    /// </summary>
    /// <param name="cell1">Клетка 1</param>
    /// <param name="cell2">Клетка 2</param>
    public void SendShadowBetweenCell(Cell cell1, Cell cell2)
    {
        if (!isPlay)
            return;

        if (cell1 == null || cell2 == null)
            return;

        Shadow shadow1to2 = GetShadow();
        Shadow shadow2to1 = GetShadow();
        if (shadow1to2 != null)
        {
            shadow1to2.transform.position = cell1.transform.position;
            shadow1to2.type = cell1.type;
            shadow1to2.StartMove(cell2);
            _shadowBlock = true;
            cell1.type = ObjectType.o0;
        }
        if (shadow2to1 != null)
        {
            shadow2to1.transform.position = cell2.transform.position;
            shadow2to1.type = cell2.type;
            shadow2to1.StartMove(cell1);
            _shadowBlock = true;
            cell2.type = ObjectType.o0;
        }
    }

    /// <summary>
    /// Отправляет тень к указанной клетке из указанной локальной позиции (Инстанцирует новый объект и помещает в пустую клетку)
    /// </summary>
    /// <param name="toCell">Цель</param>
    /// <param name="startLocalPosition">Стартовая локальная позиция</param>
    public void SendShadowToCell(Cell toCell, Vector3 startLocalPosition)
    {
        if (!isPlay)
            return;

        if (toCell == null)
            return;
        
        Shadow shadow = GetShadow();
        if (shadow != null)
        {
            shadow.transform.localPosition = startLocalPosition;
            shadow.SetRandomType();
            shadow.StartMove(toCell);
            _shadowBlock = true;
        }
    }

    /// <summary>
    /// Тень закончила движение
    /// Помещаем ее в список неактивных теней
    /// </summary>
    /// <param name="shadow">Указатель на тень</param>
    public void ShadowFinished(Shadow shadow)
    {
        // Помещаем из списка с активными тенями в неактивный
        if (_listActiveShadow.Contains(shadow))
            _listActiveShadow.Remove(shadow);
        if (!_listDeactiveShadow.Contains(shadow))
            _listDeactiveShadow.Add(shadow);

        // Если активных теней нет - снимаем блок и проверяем все клетки
        if (_listActiveShadow.Count == 0)
        {
            _shadowBlock = false;

            List<Cell> listMaxElements = new List<Cell>();

            foreach (Cell c in _listCell)
            {
                List<Cell> list = new List<Cell>();
                if (c.ThreeInARow(ref list))
                {
                    if (list.Count > listMaxElements.Count)
                        listMaxElements = new List<Cell>(list);
                }
            }

            if (listMaxElements.Count > 0)
            {
                Debug.Log(listMaxElements.Count);
                EatCells(listMaxElements);
            }
        }
    }

    /// <summary>
    /// Обновление лейбла, отображающего набранные очки
    /// </summary>
    public void UpdatePointsLabel()
    {
        if (labelPoints != null)
            labelPoints.text = _points.ToString();
    }

    /// <summary>
    /// Съедает собранные в ряд клетки
    /// </summary>
    /// <param name="list">Список с клетками</param>
    private void EatCells(List<Cell> list)
    {
        if (list == null || list.Count == 0)
            return;

        _points += (list.Count <= 3) ? 1 : 2;
        UpdatePointsLabel();

        // Удаляем объекты из все клеток в списке
        foreach (Cell c in list)
            c.type = ObjectType.o0;

        // Словарь нужен для рассчета корректной позиции в которую нужно поместить тень с новым объектом
        Dictionary<int, int> dic = new Dictionary<int, int>();

        // Для всех пустых клеток инстанцируем тени с новыми объектами
        while (list.Count > 0)
        {
            Cell c = list[0];
            list.RemoveAt(0);
            Cell cellUp = c.cellUp;
            
            int i = 1;
            while (true)
            {
                if (cellUp == null)
                    break;
                if (cellUp.type != ObjectType.o0)
                    break;
                cellUp = cellUp.cellUp;
                i++;
            }
            if (cellUp != null)
            {
                SendShadowFromCellToCell(cellUp, c);
                list.Add(cellUp);
            }
            else
            {
                if (!dic.ContainsKey(c.positionX))
                    dic.Add(c.positionX, 0);
                else
                {
                    int value = dic[c.positionX];
                    dic.Remove(c.positionX);
                    dic.Add(c.positionX, value + 1);
                }
                SendShadowToCell(c, c.transform.localPosition + new Vector3(0f, 83 * (i + dic[c.positionX]), 0f));
            }
        }
    }

    /// <summary>
    /// Создает тень (Или достает из списка неактивных теней)
    /// </summary>
    /// <returns></returns>
    private Shadow GetShadow()
    {
        if (_listDeactiveShadow.Count > 0)
        {
            Shadow s = _listDeactiveShadow[0];
            _listDeactiveShadow.RemoveAt(0);
            if (!_listActiveShadow.Contains(s))
                _listActiveShadow.Add(s);
            return s;
        }
        else
        {
            GameObject go = Instantiate(Resources.Load(pathShadowPrefab)) as GameObject;
            if (go != null)
            {
                if (shadowContainer != null)
                {
                    Transform t = go.transform;
                    Vector3 vPosition = t.localPosition;
                    Vector3 vScale = t.localScale;
                    t.parent = shadowContainer;
                    t.localPosition = vPosition;
                    t.localScale = vScale;
                }
                Shadow shadow = go.GetComponent<Shadow>();
                if (shadow != null)
                    _listActiveShadow.Add(shadow);
                return shadow;
            }
        }
        return null;
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
        }
    }

    protected override MiniGameResult GetResult()
    {
        if (_points > 16)
            return MiniGameResult.Gold;
        if (_points > 12)
            return MiniGameResult.Silver;
        if (_points > 5)
            return MiniGameResult.Bronze;
        return MiniGameResult.TimeOut;

    }
}