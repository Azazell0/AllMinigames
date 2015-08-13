using UnityEngine;
using System.Collections;

public class MiniGameResults : MonoBehaviour
{
    #region variables

    /// <summary>
    /// Окно с результатами игры
    /// </summary>
    public UISprite spriteResults;
    public UISprite spriteGoldMedal, spriteSilverMedal, spriteBronzeMedal;
    public UILabel labelMimiGameName, labelMedalName, labelDescription;

    private MonoBehaviour _currentManager;
    private float _time;

    #endregion


    /// <summary>
    /// Отображает окно с результатами игры
    /// </summary>
    public void ShowResults(MiniGameResult result, MonoBehaviour manager, float TimeGame, string GameName, string Description)
    {
        _currentManager = manager;
        _time = TimeGame;

        if (spriteResults != null)
            spriteResults.gameObject.SetActive(true);
        if (spriteGoldMedal != null)
            spriteGoldMedal.gameObject.SetActive(result == MiniGameResult.Gold);
        if (spriteSilverMedal != null)
            spriteSilverMedal.gameObject.SetActive(result == MiniGameResult.Silver);
        if (spriteBronzeMedal != null)
            spriteBronzeMedal.gameObject.SetActive(result == MiniGameResult.Bronze);

        if (labelMimiGameName != null)
            labelMimiGameName.text = GameName;
        if (labelDescription != null)
            labelDescription.text = Description;

        if (labelMedalName != null)
            switch (result)
            {
                case MiniGameResult.Gold:
                    labelMedalName.text = "Золото";
                    break;

                case MiniGameResult.Silver:
                    labelMedalName.text = "Серебро";
                    break;

                case MiniGameResult.Bronze:
                    labelMedalName.text = "Бронза";
                    break;
            }
    }

    public void ClickReplay()
    {
        if (_currentManager != null)
            _currentManager.SendMessage("NewGame", _time, SendMessageOptions.DontRequireReceiver);
        Hide();
    }

    public void ClickExit()
    {
        if (_currentManager != null)
            _currentManager.SendMessage("CloseMenu", SendMessageOptions.DontRequireReceiver);
        Hide();
    }

    public void Hide()
    {
        if (spriteResults != null)
            spriteResults.gameObject.SetActive(false);
    }
}

public enum MiniGameResult { Bronze, Silver, Gold}