using UnityEngine;
using System.Collections;

public class PanelMenu : MonoBehaviour
{
    UIPanel _panel;

    void Start()
    {
        _panel = GetComponent<UIPanel>();
    }

    public void LoadMiniGame1()
    {
        ShowGameDescription<MiniGame1_Manager>(MiniGame1_Manager.instance, 30);
    }

    public void LoadMiniGame2()
    {
        ShowGameDescription<MiniGame2_Manager>(MiniGame2_Manager.instance, 30);
    }

    public void LoadMiniGame3()
    {
        ShowGameDescription<MiniGame3_Manager>(MiniGame3_Manager.instance, 45);
    }

    public void LoadMiniGame4()
    {
        ShowGameDescription<MiniGame4_Manager>(MiniGame4_Manager.instance, 30);
    }

    public void LoadMiniGame5()
    {
        ShowGameDescription<MiniGame5_Manager>(MiniGame5_Manager.instance, 30);
    }

    public void LoadMiniGame6()
    {
        ShowGameDescription<MiniGame6_Manager>(MiniGame6_Manager.instance, 60);
    }

    public void LoadMiniGame7()
    {
        //ShowGameDescription<MiniGame7_Manager>(MiniGame7_Manager.instance);
    }

    public void LoadMiniGame8()
    {
        ShowGameDescription<MiniGame8_Manager>(MiniGame8_Manager.instance, 30);
    }

    public void LoadMiniGame9()
    {
        ShowGameDescription<MiniGame9_Manager>(MiniGame9_Manager.instance, 30);
    }

    public void LoadMiniGame10()
    {
        ShowGameDescription<MiniGame10_Manager>(MiniGame10_Manager.instance, 30);
    }

    public void LoadMiniGame11()
    {
        ShowGameDescription<MiniGame11_Manager>(MiniGame11_Manager.instance, 30);
    }

    public void LoadMiniGame12()
    {
        ShowGameDescription<MiniGame12_Manager>(MiniGame12_Manager.instance, 30);
    }

    public void LoadMiniGame13()
    {
        ShowGameDescription<MiniGame13_Manager>(MiniGame13_Manager.instance, 0);
    }

    public void LoadMiniGame14()
    {
        //ShowGameDescription<MiniGame14_Manager>(MiniGame14_Manager.instance);
    }

    public void LoadMiniGame15()
    {
        ShowGameDescription<MiniGame15_Manager>(MiniGame15_Manager.instance, 30);
    }

    public void LoadMiniGame16()
    {
        //ShowGameDescription<MiniGame16_Manager>(MiniGame16_Manager.instance);
    }

    public void LoadMiniGame17()
    {
        ShowGameDescription<MiniGame17_Manager>(MiniGame17_Manager.instance, 35);
    }

    public void LoadMiniGame18()
    {
        ShowGameDescription<MiniGame18_Manager>(MiniGame18_Manager.instance, 30);
    }

    public void LoadMiniGame19()
    {
        ShowGameDescription<MiniGame19_Manager>(MiniGame19_Manager.instance, 30);
    }

    public void LoadMiniGame20()
    {
        ShowGameDescription<MiniGame20_Manager>(MiniGame20_Manager.instance, 60);
    }

    public void LoadMiniGame21()
    {
        ShowGameDescription<MiniGame21_Manager>(MiniGame21_Manager.instance, 30);
    }

    public void LoadMiniGame22()
    {
        ShowGameDescription<MiniGame22_Manager>(MiniGame22_Manager.instance, 30);
    }

    public void LoadMiniGame23()
    {
        ShowGameDescription<MiniGame23_Manager>(MiniGame23_Manager.instance, 60);
    }

    public void LoadMiniGame24()
    {
        ShowGameDescription<MiniGame24_Manager>(MiniGame24_Manager.instance, 60);
    }

    public void LoadMiniGame25()
    {
        ShowGameDescription<MiniGame25_Manager>(MiniGame25_Manager.instance, 60);
    }

    public void LoadMiniGame26()
    {
        //ShowGameDescription<MiniGame26_Manager>(MiniGame26_Manager.instance);
    }

    public void LoadMiniGame27()
    {
        ShowGameDescription<MiniGame27_Manager>(MiniGame27_Manager.instance, 60);
    }

    public void LoadMiniGame28()
    {
        ShowGameDescription<MiniGame28_Manager>(MiniGame28_Manager.instance, 30);
    }

    public void LoadMiniGame29()
    {
        ShowGameDescription<MiniGame29_Manager>(MiniGame29_Manager.instance, 45);
    }

    public void LoadMiniGame30()
    {
        ShowGameDescription<MiniGame30_Manager>(MiniGame30_Manager.instance, 30);
    }

    public void LoadMiniGame31()
    {
        ShowGameDescription<MiniGame31_Manager>(MiniGame31_Manager.instance, 30);
    }

    public void LoadMiniGame32()
    {
        ShowGameDescription<MiniGame32_Manager>(MiniGame32_Manager.instance, 30);
    }

    public void LoadMiniGame33()
    {
        //ShowGameDescription<MiniGame33_Manager>(MiniGame33_Manager.instance, 30);
    }

    public void Hide()
    {
        if (_panel != null)
            _panel.alpha = 0f;
    }

    public void Show()
    {
        if (_panel != null)
            _panel.alpha = 1f;
    }

    private void ShowGameDescription<T>(T manager, float time) where T : MiniGameSingleton<T>
    {
        manager.NewGame(time);
        Hide();
    }
}