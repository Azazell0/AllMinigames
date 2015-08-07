using UnityEngine;
using Minigame23;
using System.Collections;
using System.Collections.Generic;

public class MiniGame23_Manager : MiniGameSingleton<MiniGame23_Manager>
{
    #region variables

    /// <summary>
    /// Контейнер, содержащий все ячейки
    /// </summary>
    public Transform containerCells;
    /// <summary>
    /// Контейнер, содержащий все ячейки
    /// </summary>
    public Transform containerPugalka;
    /// <summary>
    /// Список, содержащий всех крыс
    /// </summary>
    public List<Rat> listRats;
    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения количества оставшихся в запасе пугалок
    /// </summary>
    public UILabel labelPugalkaCount;
    public int maxPugalkaCount = 3;

    public Pugalka currentPugalka { get { return _currentPugalka; } }

    private const string pathPugalka = "Prefabs/Minigame 23/Pugalka";

    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;
    private Pugalka _currentPugalka;
    private List<Pugalka> _listPugalka;
    private bool _pugalkaButtonClick = false;
    private int _pugalkaCount = 0;

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
        _currentPugalka = null;
        _pugalkaButtonClick = false;
        _pugalkaCount = 0;
        UpdateCountLabel();

        if (_listPugalka == null)
            _listPugalka = new List<Pugalka>();
        else
        {
            foreach (Pugalka p in _listPugalka)
                Destroy(p.gameObject);
            _listPugalka.Clear();
        }

        if (listRats == null)
            listRats = new List<Rat>();
        else
        {
            foreach (Rat rat in listRats)
                rat.Reset();
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

    public void PugalkaButtonClick()
    {
        _pugalkaButtonClick = true;
    }

    /// <summary>
    /// Устанавливает новую пугалку
    /// </summary>
    /// <returns>Указатель на пугалку</returns>
    public Pugalka GetNewPugalka()
    {
        if (!isPlay || !_pugalkaButtonClick || currentPugalka != null || _pugalkaCount >= maxPugalkaCount)
            return null;

        GameObject go = Instantiate(Resources.Load(pathPugalka)) as GameObject;
        if (go != null)
        {
            Pugalka p = go.GetComponent<Pugalka>();
            if (p != null)
            {
                if (containerPugalka != null)
                {
                    Vector3 scale = go.transform.localScale;
                    Vector3 position = go.transform.localPosition;
                    go.transform.parent = containerPugalka;
                    go.transform.localPosition = position;
                    go.transform.localScale = scale;
                }

                _pugalkaCount++;
                UpdateCountLabel();
                _currentPugalka = p;
                p.SetTransparency(true);
                return p;
            }
        }
        return null;
    }

    /// <summary>
    /// Устанавливает указатель на текущую пугалку
    /// </summary>
    /// <param name="pugalka">Указатель на пугалку</param>
    public void SetCurrentPugalka(Pugalka pugalka)
    {
        if (!isPlay)
            return;

        if (currentPugalka != null)
            currentPugalka.SetTransparency(false);
        _currentPugalka = pugalka;
        if (currentPugalka != null)
            _currentPugalka.SetTransparency(true);
    }

    /// <summary>
    /// Ищет путь к цели
    /// </summary>
    /// <param name="listPath">Список в который помещаются все клетки пути. Первая клетка - стартовая</param>
    /// <param name="allPoints">Рабочий список для запоминания всех проверенных клеток</param>
    /// <param name="previousPoint"></param>
    /// <param name="currentPoint"></param>
    /// <param name="targetPoint"></param>
    /// <returns></returns>
    public bool FindShorterPath (ref List<Point> listPath, ref List<Point> allPoints, Point previousPoint, Point currentPoint, Point targetPoint)
    {
        if (currentPoint == null || targetPoint == null || currentPoint.isPugalka)
            return false;

        if (allPoints.Contains(currentPoint))
            return false;
        allPoints.Add(currentPoint);

        foreach (Point p in currentPoint.listPoints)
        {
            if (p == targetPoint)
            {
                listPath.Insert(0, p);
                listPath.Insert(0, currentPoint);
                return true;
            }
            else if (p == null || p == currentPoint || p == previousPoint)
                continue;
            else
            {
                if (FindShorterPath(ref listPath, ref allPoints, currentPoint, p, targetPoint))
                {
                    listPath.Insert(0, currentPoint);
                    return true;
                }
            }
        }

        return false;

        //if (list.Count == 0)
        //    return false;

        //int i = 0;
        //if (list.Count > 0)
        //{
        //    int count = list[0].Count;
        //    for (int j = 1; j < list.Count; j++)
        //        if (list[j].Count < count)
        //        {
        //            i = j;
        //            count = list[j].Count;
        //        }
        //}

        //listPath.InsertRange(0, list[i]);
        //return true;
    }

    public void KilledRat(Rat rat)
    {
        if (!isPlay || rat == null)
            return;

        rat.Disable();
        if (listRats != null)
            foreach (Rat r in listRats)
                if (r.gameObject.activeSelf)
                    return;
        Win();
    }

    public void EatCable(Point pointCable)
    {
        if (!isPlay || pointCable == null)
            return;

        Losing();
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

    public void UpdateCountLabel()
    {
        if (labelPugalkaCount != null)
            labelPugalkaCount.text = _pugalkaCount.ToString() + "/" + maxPugalkaCount.ToString();
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
        int count = 0;
        if (listRats != null)
            foreach (Rat rat in listRats)
                if (rat.gameObject.activeSelf)
                    count++;

        switch (count)
        {
            case 0:
                return MiniGameResult.Gold;
            case 1:
                return MiniGameResult.Silver;
            default:
                return MiniGameResult.Bronze;
        }
    }
}