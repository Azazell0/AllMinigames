using UnityEngine;
using Minigame25;
using System.Collections;
using System.Collections.Generic;

public class MiniGame25_Manager : MiniGameSingleton<MiniGame25_Manager>
{
    #region variables

    /// <summary>
    /// Трансформ со всеми спрайтами города
    /// </summary>
    public Transform city;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Объект, которому нужно назначать слой Default
    /// </summary>
    public GameObject defaultLayerObject;
    /// <summary>
    /// Лэйбл для отображения оставшихся станций WiFi на 900 метров
    /// </summary>
    public UILabel labelCount900;
    /// <summary>
    /// Лэйбл для отображения оставшихся станций WiFi на 900 метров
    /// </summary>
    public UILabel labelCount500;
    /// <summary>
    /// Список всех клеток с домами
    /// </summary>
    public List<Cell> listHomeCells;
    /// <summary>
    /// Режим установки станций WiFi
    /// </summary>
    public WiFiPointRadius pointRadiusMode;
    /// <summary>
    /// Текущая станция
    /// </summary>
    public WiFiPoint targetPoint;
    /// <summary>
    /// Максимальное количество станций m500, которое можно установить
    /// </summary>
    private int maxM500Count = 9;
    /// <summary>
    /// Максимальное количество станций m900, которое можно установить
    /// </summary>
    private int maxM900Count = 3;

    public const string pathWiFi500Prefab = "Prefabs/25_WiFi500";
    public const string pathWiFi900Prefab = "Prefabs/25_WiFi900";

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Текущее количество станций m500
    /// </summary>
    private int _m500Count = 0;
    /// <summary>
    /// Текущее количество станций m900
    /// </summary>
    private int _m900Count = 0;

    private List<WiFiPoint> listWiFiPoints;

    #endregion


    void Awake()
    {
        // Для синглтона
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
        if (defaultLayerObject != null)
            MiniGameHelper.SetLayerRecursieve(defaultLayerObject.transform, LayerMask.NameToLayer("Default"));
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        if (listWiFiPoints == null)
            listWiFiPoints = new List<WiFiPoint>();
        else
        {
            foreach (WiFiPoint point in listWiFiPoints)
                if (point != null)
                    Destroy(point.gameObject);
            listWiFiPoints.Clear();
        }
        if (listHomeCells == null)
            listHomeCells = new List<Cell>();
        _m500Count = 0;
        _m900Count = 0;
        UpdateWiFiLabels();
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
    }

    /// <summary>
    /// Устанавливает режим размещения WiFi станций 900 метров
    /// </summary>
    public void SetWiFiMode900m()
    {
        SetWiFiMode(WiFiPointRadius.m900);
    }

    /// <summary>
    /// Устанавливает режим размещения WiFi станций 500 метров
    /// </summary>
    public void SetWiFiMode500m()
    {
        SetWiFiMode(WiFiPointRadius.m500);
    }

    /// <summary>
    /// Устанавливает режим размещения WiFi станций
    /// </summary>
    public void SetWiFiMode(WiFiPointRadius radius)
    {
        pointRadiusMode = radius;
    }

    /// <summary>
    /// Добавляет клетку в массив, содержащий все клетки с домами
    /// </summary>
    /// <param name="cell"></param>
    public void IamCellWithHome(Cell cell)
    {
        if (cell == null || !cell.isCorrect || !cell.isHome)
            return;

        if (listHomeCells == null)
            listHomeCells = new List<Cell>();
        if (!listHomeCells.Contains(cell))
            listHomeCells.Add(cell);
    }

    /// <summary>
    /// Возвращает новую станцию, если это возможно
    /// </summary>
    /// <returns></returns>
    public WiFiPoint GetNewStation()
    {
        GameObject go = null;
        switch(pointRadiusMode)
        {
            case WiFiPointRadius.m500:
                if (_m500Count < maxM500Count)
                {
                    go = Instantiate(Resources.Load(pathWiFi500Prefab)) as GameObject;
                    Vector3 pos = go.transform.position;
                    Vector3 scale = go.transform.localScale;
                    go.transform.parent = city;
                    go.transform.localPosition = pos;
                    go.transform.localScale = scale;
                    _m500Count++;
                }
                break;

            case WiFiPointRadius.m900:
                if (_m900Count < maxM900Count)
                {
                    go = Instantiate(Resources.Load(pathWiFi900Prefab)) as GameObject;
                    Vector3 pos = go.transform.position;
                    Vector3 scale = go.transform.localScale;
                    go.transform.parent = city;
                    go.transform.localPosition = pos;
                    go.transform.localScale = scale;
                    _m900Count++;
                }
                break;
        }

        if (go == null)
            return null;

        targetPoint = go.GetComponent<WiFiPoint>();
        if (targetPoint != null)
            listWiFiPoints.Add(targetPoint);

        UpdateWiFiLabels();
        return targetPoint;
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    public void CheckWin()
    {
        if (!isPlay)
            return;

        List<Cell> list = new List<Cell>(listHomeCells);
        foreach(WiFiPoint point in listWiFiPoints)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (point.CheckCell(list[i].positionX, list[i].positionY))
                    list.RemoveAt(i);
        }

        if (list.Count == 0)
            Win();
    }

    /// <summary>
    /// Обновление лейблов, отображающих количество станций WiFi
    /// </summary>
    private void UpdateWiFiLabels()
    {
        if (labelCount900 != null)
            labelCount900.text = (maxM900Count - _m900Count).ToString() + "/" + maxM900Count.ToString();
        if (labelCount500 != null)
            labelCount500.text = (maxM500Count - _m500Count).ToString() + "/" + maxM500Count.ToString();
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
            return MiniGameResult.TimeOut;
        else
            return MiniGameResult.Gold;
    }
}