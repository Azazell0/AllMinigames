using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGame14_Manager : MiniGameSingleton<MiniGame14_Manager>
{
    #region variables

    public Animator anim;

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

    void Start()
    {
        StartCoroutine(Anim1());
    }

    public IEnumerator Anim1()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Walk");
        anim.SetTrigger("triggerWalk");
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
    }

	
	public void CloseMenu()
    {
        Hide();
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    /// <returns>true - игрок выиграл</returns>
    public bool CheckWin()
    {
        return false;
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
                Losing();
            }
        }
    }

    protected override MiniGameResult GetResult()
    {
        return MiniGameResult.Bronze;
    }
}