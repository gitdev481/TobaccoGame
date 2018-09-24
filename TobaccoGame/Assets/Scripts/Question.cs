using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for question logic.
/// </summary>
public class Question : MonoBehaviour {

    public GameObject[] answers;
    public int questionAttempts = 1;

    /// <summary>
    /// Sets up the question attempts.
    /// </summary>
    public void SetupQuestion()
    {
        if (!GameLogicManager.Instance.DidPlayerScoreOrDie())
        {
            questionAttempts = 1;
        }
        else
        {
            questionAttempts = 3;
        }
    }

    /// <summary>
    /// Checks if the correct answer has been chosen by the player.
    /// </summary>
    /// <param name="isAnswerCorrect"></param>
    public void CheckIfCorrectAnswerClicked(bool isAnswerCorrect)
    {
        if (UIManager.Instance.questionFinished)
            return;
        UIManager.Instance.UpdateDeathPointsSprite(questionAttempts - 1);
        if (isAnswerCorrect)
        {
            SoundManager.Instance.PlaySound("correctsound");
           
            if (!GameLogicManager.Instance.DidPlayerScoreOrDie())
            {
                GameLogicManager.Instance.DoubleCurrentScore();
                UIManager.Instance.TriggerQuestionFinished();
            }
            else
            {
                GameLogicManager.Instance.IncreaseScore(questionAttempts);
                UIManager.Instance.ShowDeathPointsImage();
                UIManager.Instance.TriggerQuestionFinished();
            }
        }
        else
        {
            SoundManager.Instance.PlaySound("incorrectsound");
        }
        questionAttempts--;
        
        CheckIfOutOfAttempts();
    }

    /// <summary>
    /// Resets all the answer sprites.
    /// </summary>
    public void HideAllAnswerSprites()
    {
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].GetComponent<Answer>().HideAnswerSprite();
        }
    }

  
    /// <summary>
    /// Checks if the player has ran out of question attempts.
    /// </summary>
    public void CheckIfOutOfAttempts()
    {
        if(questionAttempts <= 0)
        {
            UIManager.Instance.questionFinished = true;
            UIManager.Instance.TriggerQuestionFinished();
        }
    }
}
