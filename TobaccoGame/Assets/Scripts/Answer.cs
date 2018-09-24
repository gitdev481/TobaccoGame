using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is attached to every answer in the game.
/// </summary>
public class Answer : MonoBehaviour {

    #region variables
    public bool isAnswerCorrect = false;
    public Sprite[] answerResultSprites;
    public GameObject answerResultImage;
    public Question question;
    #endregion


    /// <summary>
    /// Called when the answer button is clicked. 
    /// </summary>
    public void AnswerClicked()
    {
        if (UIManager.Instance.questionFinished)
            return;

        ShowAnswerSprite();
        question.CheckIfCorrectAnswerClicked(isAnswerCorrect);
    }

    /// <summary>
    /// Set the corresponding answer sprite and show the sprite.
    /// </summary>
    public void ShowAnswerSprite()
    {
        if (question.questionAttempts <= 0)
            return;

        if (!isAnswerCorrect)
            answerResultImage.GetComponent<Image>().sprite = answerResultSprites[0];
        else
            answerResultImage.GetComponent<Image>().sprite = answerResultSprites[1];
        answerResultImage.SetActive(true);
    }

    /// <summary>
    /// Hides the answer sprite.
    /// </summary>
    public void HideAnswerSprite()
    {
        answerResultImage.SetActive(false);
    }
}
