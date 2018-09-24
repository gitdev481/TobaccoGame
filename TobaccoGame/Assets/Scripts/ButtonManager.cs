using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls various in-game buttons.
/// </summary>
public class ButtonManager : MonoBehaviour {

    #region SINGLETON PATTERN
    public static ButtonManager _instance;
    public static ButtonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ButtonManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("ButtonManager");
                    _instance = container.AddComponent<ButtonManager>();
                }

            }
            return _instance;
        }
    }
    #endregion

    /// <summary>
    /// Fades the title screen out and disables it.
    /// </summary>
    public void TitleScreenFadeToGame()
    {
        PageManager.Instance.FadeOutTitleScreen();
        SoundManager.Instance.PlaySound("backgroundmusic");
    }


    /// <summary>
    /// Controls the functionality of the Page 2 Play Button.
    /// </summary>
    public void Page2PlayButton()
    {
        if (UIManager.Instance.CheckIfNameEntered())
        {
            UIManager.Instance.SetPlayerName();
            PageManager.Instance.FadeOutPage2();
            UIManager.Instance.redPlayButtonAnimator.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Controls the functionality of the red/blue play button.
    /// </summary>
    public void RedOrBluePlayButton()
    {
        UIManager.Instance.HideTutorial();
    }
}
