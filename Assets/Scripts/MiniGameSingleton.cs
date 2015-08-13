using UnityEngine;

public abstract class MiniGameSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region variables

    protected  static T _instance;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject o = new GameObject(typeof(T).Name);
                _instance = o.AddComponent<T>();
            }
            return _instance;
        }
    }

    public delegate void MiniGameSimpleAction();

    /// <summary>
    /// Эвент победы
    /// </summary>
    public event MiniGameSimpleAction WinEvent;
    /// <summary>
    /// Эвент проигрыша
    /// </summary>
    public event MiniGameSimpleAction LosingEvent;

    /// <summary>
    /// Текущий режим игры
    /// true - идет игра, false - пауза
    /// </summary>
    public bool isPlay { get { return _isPlay; } }

    /// <summary>
    /// Тело меню
    /// </summary>
    public GameObject body;
    /// <summary>
    /// Окно с результатами игры
    /// </summary>
    public MiniGameResults resultsMenu;

    /// <summary>
    /// Текущий режим игры
    /// true - идет игра, false - пауза
    /// </summary>
    protected bool _isPlay = false;
    /// <summary>
    /// Время до окончания игры
    /// </summary>
    protected float _time = 0f;
    protected float _timeToGame = 0f;
    protected string _minigameName = "";
    protected string _minigameDescription = "";

    #endregion


    /// <summary>
    /// Инициализация новой игры
    /// </summary>
    /// <param name="time">Время для прохождения</param>
    public virtual void NewGame(float time)
    {
        _timeToGame = time;
        Init();
        Show();

        _time = time;
        _isPlay = true;
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// Возвращает результат игры
    /// </summary>
    /// <returns></returns>
    protected abstract MiniGameResult GetResult();

    /// <summary>
    /// Скрыть меню
    /// </summary>
    public virtual void Hide()
    {
        _isPlay = false;
        if (body != null)
            body.SetActive(false);
        if (resultsMenu != null)
            resultsMenu.Hide();
        PanelMenu pm = FindObjectOfType<PanelMenu>();
        if (pm != null)
            pm.Show();
    }

    /// <summary>
    /// Показать меню
    /// </summary>
    public virtual void Show()
    {
        if (body != null)
            body.SetActive(true);
    }

    /// <summary>
    /// Победа в игре
    /// </summary>
    protected virtual void Win()
    {
        _isPlay = false;
        Debug.Log("WIN!");
        if (resultsMenu != null)
            resultsMenu.ShowResults(GetResult(), this, _timeToGame, _minigameName, _minigameDescription);
        if (WinEvent != null)
            WinEvent();
    }

    /// <summary>
    /// Проигрыш в игре
    /// </summary>
    protected virtual void Losing()
    {
        _isPlay = false;
        Debug.Log("LOSING");
        if (resultsMenu != null)
            resultsMenu.ShowResults(GetResult(), this, _timeToGame, _minigameName, _minigameDescription);
        if (LosingEvent != null)
            LosingEvent();
    }
}