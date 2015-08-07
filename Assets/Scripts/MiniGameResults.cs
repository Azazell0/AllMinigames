using UnityEngine;
using System.Collections;

public class MiniGameResults : MonoBehaviour
{
    /// <summary>
    /// Окно с результатами игры
    /// </summary>
    public UISprite spriteResults;
    public UISprite spriteGoldMedal, spriteSilverMedal, spriteBronzeMedal;
    public UILabel labelTimeIsOut;

    /// <summary>
    /// Отображает окно с результатами игры
    /// </summary>
    public void ShowResults(MiniGameResult result)
    {
        if (spriteResults != null)
            spriteResults.gameObject.SetActive(true);
        if (spriteGoldMedal != null)
            spriteGoldMedal.gameObject.SetActive(result == MiniGameResult.Gold);
        if (spriteSilverMedal != null)
            spriteSilverMedal.gameObject.SetActive(result == MiniGameResult.Silver);
        if (spriteBronzeMedal != null)
            spriteBronzeMedal.gameObject.SetActive(result == MiniGameResult.Bronze);
        if (labelTimeIsOut != null)
            labelTimeIsOut.gameObject.SetActive(result == MiniGameResult.TimeOut);
    }

    public void Hide()
    {
        if (spriteResults != null)
            spriteResults.gameObject.SetActive(false);
    }
}

public enum MiniGameResult { TimeOut, Bronze, Silver, Gold}