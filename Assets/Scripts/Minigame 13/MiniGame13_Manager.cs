using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MiniGame13_Manager : MiniGameSingleton<MiniGame13_Manager>
{
    #region variables

    public UILabel textWaterCold1, textWaterCold2, textWaterHot1, textWaterHot2, textElectric1, textElectric2;
    public UIInput inputWaterCold, inputWaterHot, inputElectric;

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
        
    }

    protected override MiniGameResult GetResult()
    {
        return MiniGameResult.Gold;
    }
    
    void Start ()
    {
	}
	
	void FixedUpdate ()
    {
        if (_isPlay)
            if (CheckWin())
                Win();
	}

    public void CloseMenu()
    {
        Hide();
    }

    /// <summary>
    /// Инициализация новой игры
    /// </summary>
    public void NewGame()
    {
        if (textWaterCold1 != null)
            textWaterCold1.text = Random.Range(0, 10000).ToString("0000") + ".";
        if (textWaterCold2 != null)
            textWaterCold2.text = Random.Range(0, 1000).ToString("000");
        if (textWaterHot1 != null)
            textWaterHot1.text = Random.Range(0, 10000).ToString("0000") + ".";
        if (textWaterHot2 != null)
            textWaterHot2.text = Random.Range(0, 1000).ToString("000");
        if (textElectric1 != null)
            textElectric1.text = Random.Range(0, 1000000).ToString("000000") + ".";
        if (textElectric2 != null)
            textElectric2.text = Random.Range(0, 100).ToString("00");

        SetInteractable(true, true);

        _isPlay = true;

        Show();
    }

    /// <summary>
    /// Проверка условий, необходимых для победы
    /// </summary>
    /// <returns>true - игрок выиграл</returns>
    public bool CheckWin()
    {
        if (CheckStrings(textWaterCold1.text, textWaterCold2.text, inputWaterCold.value))
            if (CheckStrings(textWaterHot1.text, textWaterHot2.text, inputWaterHot.value))
                if (CheckStrings(textElectric1.text, textElectric2.text, inputElectric.value))
                    return true;
        return false;
    }

    /// <summary>
    /// Проверка равенства строк на счетчике и в поле для введения числа
    /// </summary>
    /// <param name="textField1">Строка на счетчике до запятой (включая запятую)</param>
    /// <param name="textField2">Строка на счетчике после запятой</param>
    /// <param name="inputString">Строка в поле для введения числа</param>
    /// <returns>true - строки равны</returns>
    private bool CheckStrings(string textField1, string textField2, string inputString)
    {
        return (textField1 + textField2 == inputString);
    }

    /// <summary>
    /// Устанавливает параметр Interactable для всех InputFields
    /// </summary>
    /// <param name="b"></param>
    /// <param name="deleteText">true - удалить текст из InputFields.text</param>
    private void SetInteractable(bool b, bool deleteText)
    {
        if (inputWaterCold != null)
        {
            if (deleteText)
                inputWaterCold.value = "";
            inputWaterCold.enabled = b;
        }
        if (inputWaterHot != null)
        {
            if (deleteText)
                inputWaterHot.value = "";
            inputWaterHot.enabled = b;
        }
        if (inputElectric != null)
        {
            if (deleteText)
                inputElectric.value = "";
            inputElectric.enabled = b;
        }
    }
}