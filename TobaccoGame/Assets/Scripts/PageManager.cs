using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages the pages in the game.
/// </summary>
public class PageManager : MonoBehaviour {

    #region SINGLETON PATTERN
    public static PageManager _instance;
    public static PageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PageManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("PageManager");
                    _instance = container.AddComponent<PageManager>();
                }

            }
            return _instance;
        }
    }
    #endregion

    #region variables
    public Animator titleScreenAnimator;
    public Animator page8Animator;
    public Animator page9Animator;
    public Animator leaderboardScreenAnimator;
    public Animator tutorialParentAnimator;
    public Animator page2Animator;
    public Animator page3Animator;
    public Animator page4Animator;
    public Animator page5Animator;
    public Animator page6Animator;
    public Animator page7Animator;
    public Animator warningSignAnimator;
    #endregion

    public void FadeOutTitleScreen()
    {
        titleScreenAnimator.SetTrigger("FadeOut");
    }

    public void FadeInRanOutOfLives()
    {
        page8Animator.SetTrigger("FadeIn");

    }
    public void FadeInCongratulationsPage()
    {
        UIManager.Instance.SetupCongratulationsPage();
        page9Animator.SetTrigger("FadeIn");
    }

    public void FadeInLeaderboardScreen()
    {
        UIManager.Instance.HideUIForLeaderboard();
        UIManager.Instance.ShowLeaderboardScreen();
        leaderboardScreenAnimator.SetTrigger("FadeIn");
    }

    public void FadeOutTutorial()
    {
        tutorialParentAnimator.SetTrigger("FadeOut");
    }

    public void FadeOutPage2()
    {
        page2Animator.SetTrigger("FadeOut");
    }

    public void FadeInPage3()
    {
        page3Animator.SetTrigger("FadeIn");
    }

    public void FadeInPage4()
    {
        page4Animator.SetTrigger("FadeIn");
    }

    public void FadeInPage5()
    {
        page5Animator.SetTrigger("FadeIn");
    }

    public void FadeInPage6()
    {
        page6Animator.SetTrigger("FadeIn");
    }

    public void FadeInPage7()
    {
        page7Animator.SetTrigger("FadeIn");
    }

    public void TriggerWarningFlicker()
    {
        warningSignAnimator.SetTrigger("FadeIn");
    }

}
