using UnityEngine;
using Minigame28;
using System.Collections;

public class MiniGame28_Manager : MiniGameSingleton<MiniGame28_Manager>
{
    #region variables

    /// <summary>
    /// Лэйбл для отображения оставшегося времени
    /// </summary>
    public UILabel labelTime;
    public UISprite spriteCarLarge;
    public Car car1, car2, car3;

    private Car _currentCar;

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
    
    /// <summary>
    /// Инициализация
    /// </summary>
    protected override void Init()
    {
        _currentCar = null;
        if (spriteCarLarge != null)
            spriteCarLarge.alpha = 1f;

        if (car1 != null)
            car1.Reset();
        if (car2 != null)
            car2.Reset();
        if (car3 != null)
            car3.Reset();
    }

	
	public void CloseMenu()
    {
        Hide();
    }

    public void CorrectButtonClick()
    {
        Win();
    }

    public void ClickButton1()
    {
        StartMove(car1);
    }

    public void ClickButton2()
    {
        StartMove(car2);
    }

    public void ClickButton3()
    {
        StartMove(car3);
    }

    public void Finish()
    {
        Win();
    }

    private void StartMove(Car car)
    {
        if (!_isPlay || _currentCar != null)
            return;

        if (car != null)
        {
            _currentCar = car;
            _currentCar.Move();
            if (spriteCarLarge != null)
                spriteCarLarge.alpha = 0f;
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
        }
    }

    protected override MiniGameResult GetResult()
    {
        if (_time <= 0)
            return MiniGameResult.Bronze;
        return (_currentCar == car3) ? MiniGameResult.Gold : (_currentCar == car1) ? MiniGameResult.Silver : MiniGameResult.Bronze;
    }
}