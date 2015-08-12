using UnityEngine;
using Minigame10;
using System.Collections;
using System.Collections.Generic;

public class MiniGame10_Manager : MiniGameSingleton<MiniGame10_Manager>
{
    #region variables

    public delegate void MiniGameBuildObjectAction(BuildObject obj);

    /// <summary>
    /// Эвент клика на один из объектов 
    /// </summary>
    public static event MiniGameBuildObjectAction ClickEvent;

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    /// <summary>
    /// Лэйбл для отображения оставшихся объектов, которые нужно найти
    /// </summary>
    public UILabel labelCount;
    /// <summary>
    /// Объект, которому нужно назначать слой Default
    /// </summary>
    public GameObject defaultLayerObject;
    /// <summary>
    /// Список объектов, которые игроку необходимо найти
    /// </summary>
    public List<BuildObject> buildObjectsList;
    /// <summary>
    /// Количество объектов, которые игроку необходимо найти на данный момент
    /// </summary>
    public int HideElementsCount { get { return (_buildObjectsCopyList == null) ? 0 : _buildObjectsCopyList.Count; } }

    /// <summary>
    /// Список объектов, которые игроку необходимо найти на данный момент
    /// При старте игры является копией buildObjectsList
    /// </summary>
    private List<BuildObject> _buildObjectsCopyList;
    /// <summary>
    /// Время до окончания игры
    /// </summary>
    private float _time = 0f;

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

        Init();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        if (buildObjectsList == null)
            buildObjectsList = new List<BuildObject>();
        else foreach (BuildObject obj in buildObjectsList)
                if (obj != null)
                    obj.Hide();

        _buildObjectsCopyList = new List<BuildObject>(buildObjectsList);
        UpdateCountLabel();
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
    /// Обновление лейбла, отображающего количество найденных элементов
    /// </summary>
    private void UpdateCountLabel()
    {
        if (labelCount != null)
            labelCount.text = (buildObjectsList.Count - HideElementsCount).ToString() + "/" + buildObjectsList.Count.ToString();
    }

    /// <summary>
    /// Указатель мыши больше не на объекте
    /// </summary>
    /// <param name="obj">Объект, по которому произошел клик</param>
    public void WasMouseNoMoreOverObject(BuildObject obj)
    {
        if (!_isPlay)
            return;

        if (obj != null)
        {
            if (_buildObjectsCopyList.Contains(obj))
                obj.Hide();
            //else
            //    obj.Hide(); obj.Show();
        }
    }

    /// <summary>
    /// Указатель мыши на объекте
    /// </summary>
    /// <param name="obj">Объект, по которому произошел клик</param>
    public void WasMouseOverObject(BuildObject obj)
    {
        if (!_isPlay)
            return;

        if (obj != null && _buildObjectsCopyList.Contains(obj))
        {
            obj.HighlightObject();
        }
    }

    /// <summary>
    /// Произошел клик по объекту
    /// </summary>
    /// <param name="obj">Объект, по которому произошел клик</param>
    public void WasClickObject(BuildObject obj)
    {
        if (!_isPlay)
            return;

        // Если по объекту еще не кликали и он есть в списке...
        if (obj != null && _buildObjectsCopyList.Contains(obj))
        {
            obj.Show();
            // Удаляем объект из списка
            _buildObjectsCopyList.Remove(obj);
            if (ClickEvent != null)
                ClickEvent(obj);
            Debug.Log("Hide objects count: " + HideElementsCount);
            UpdateCountLabel();

            // Проверяем условия победы
            if (CheckWin())
                Win();
        }
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    /// <returns>true - игрок выиграл</returns>
    public bool CheckWin()
    {
        return (HideElementsCount == 0) ? true : false;
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
                Losing();
        }
    }

    /// <summary>
    /// Возвращает результат игры
    /// </summary>
    /// <returns></returns>
    protected override MiniGameResult GetResult()
    {
        if (_time <= 0 && HideElementsCount > 4)
            return MiniGameResult.TimeOut;
        else if (HideElementsCount == 0)
            return MiniGameResult.Gold;
        else if (HideElementsCount > 0 && HideElementsCount < 2)
            return MiniGameResult.Silver;
        else return MiniGameResult.Bronze;
    }
}