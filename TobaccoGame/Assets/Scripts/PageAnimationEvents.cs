using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is attached to any page that needs to send an Animation Event.
/// </summary>
public class PageAnimationEvents : MonoBehaviour {

	public void FadeInCongratulationsPage()
    {
        PageManager.Instance.FadeInCongratulationsPage();
    }

    public void FadeInLeaderboardScreen()
    {
        PageManager.Instance.FadeInLeaderboardScreen();
    }

    public void FadeInPage3()
    {
        PageManager.Instance.FadeInPage3();
    }

    public void FadeInPage4()
    {
        PageManager.Instance.FadeInPage4();
    }

    public void FadeInPage5()
    {
        PageManager.Instance.FadeInPage5();
    }

    public void FadeInPage6()
    {
        PageManager.Instance.FadeInPage6();
    }

    public void FadeInPage7()
    {
        PageManager.Instance.FadeInPage7();
    }

    public void TriggerWarningFlicker()
    {
        PageManager.Instance.TriggerWarningFlicker();
    }
}
