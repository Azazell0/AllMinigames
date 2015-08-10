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
        MiniGame1_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame2()
    {
        //MiniGame2_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame3()
    {
        MiniGame3_Manager.instance.NewGame(45);
        Hide();
    }

    public void LoadMiniGame4()
    {
        MiniGame4_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame5()
    {
        //MiniGame5_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame6()
    {
        MiniGame6_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame7()
    {
        //MiniGame7_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame8()
    {
        MiniGame8_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame9()
    {
        MiniGame9_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame10()
    {
        MiniGame10_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame11()
    {
        MiniGame11_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame12()
    {
        //MiniGame12_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame13()
    {
        MiniGame13_Manager.instance.NewGame();
        Hide();
    }

    public void LoadMiniGame14()
    {
    //    MiniGame14_Manager.instance.NewGame();
    //    Hide();
    }

    public void LoadMiniGame15()
    {
        MiniGame15_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame16()
    {
        //MiniGame16_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame17()
    {
        MiniGame17_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame18()
    {
        MiniGame18_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame19()
    {
        MiniGame19_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame20()
    {
        MiniGame20_Manager.instance.NewGame(60);
        Hide();
    }

    public void LoadMiniGame21()
    {
        //MiniGame21_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame22()
    {
        //MiniGame22_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame23()
    {
        MiniGame23_Manager.instance.NewGame(60);
        Hide();
    }

    public void LoadMiniGame24()
    {
        MiniGame24_Manager.instance.NewGame(60);
        Hide();
    }

    public void LoadMiniGame25()
    {
        MiniGame25_Manager.instance.NewGame(60);
        Hide();
    }

    public void LoadMiniGame26()
    {
        //MiniGame26_Manager.instance.NewGame();
        //Hide();
    }

    public void LoadMiniGame27()
    {
        MiniGame27_Manager.instance.NewGame(60);
        Hide();
    }

    public void LoadMiniGame28()
    {
        MiniGame28_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame29()
    {
        MiniGame29_Manager.instance.NewGame(45);
        Hide();
    }

    public void LoadMiniGame30()
    {
        MiniGame30_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame31()
    {
        MiniGame31_Manager.instance.NewGame(30);
        Hide();
    }

    public void LoadMiniGame32()
    {
        MiniGame32_Manager.instance.NewGame(30);
        Hide();
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
}