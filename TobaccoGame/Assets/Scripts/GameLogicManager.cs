using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for handling general game logic.
/// </summary>
public class GameLogicManager : MonoBehaviour {


    #region SINGLETON PATTERN
    public static GameLogicManager _instance;
    public static GameLogicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameLogicManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("GameLogicManager");
                    _instance = container.AddComponent<GameLogicManager>();
                }

            }
            return _instance;
        }
    }
    #endregion



    #region variables

    private bool startTimer = false;

    private float maxGameTime = 60f;
    private float currentGameTime = 0f;

    public Text timerText;

    public Ball ball;
    public Paddle paddle;

    public float playerScore = 0;
    public Text scoreText;
    public float tileDestroyedScore = 5;
    private float scoreMultiplier = 2;
   
    //scored = false, died = true
    private bool didPlayerScoreOrDie = false;
    public bool isGameOver = false;
    private bool pauseTimer = false;
    #endregion

    void Start ()
    {
        currentGameTime = maxGameTime;
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        startTimer = true;
        UIManager.Instance.HideClickToReleaseImage();
        Ball.Instance.FireBallUponReset();
    }

    /// <summary>
    /// Increases the player's score.
    /// </summary>
    public void IncreaseScore(float amount)
    {
        playerScore += amount;
    }

    /// <summary>
    /// Doubles the player's current score.
    /// </summary>
    public void DoubleCurrentScore()
    {
        playerScore *= scoreMultiplier;
    }

	void Update ()
    {
        ManageTimer();
        ManageScoreText();
    }

    /// <summary>
    /// Manages the game timer.
    /// </summary>
    public void ManageTimer()
    {
        if (!startTimer)
            return;
        if (pauseTimer)
            return;

        if (currentGameTime > 0)
        {
            currentGameTime -= Time.deltaTime;
            timerText.text = currentGameTime.ToString("f2");
        }
        else
        {
            startTimer = false;
            currentGameTime = maxGameTime;
            TimerFinished();
        }

    }

    /// <summary>
    /// Keeps the score display up to date.
    /// </summary>
    public void ManageScoreText()
    {
        scoreText.text =  "" + playerScore.ToString("0000");
    }

    /// <summary>
    /// Called when the in-game timer is finished.
    /// </summary>
    public void TimerFinished()
    {
        ball.StopBall();
        PauseTimer();
        PageManager.Instance.FadeInCongratulationsPage();
        timerText.text = "00.00";
    }

    /// <summary>
    /// Updates the current flavour the player must aim for.
    /// </summary>
    public void UpdateCurrentFlavour()
    {
        if (TileManager.Instance.tileFlavours.Count == 0)
            return;

        TileManager.Instance.ChooseRandomFlavour();
        int flavour = TileManager.Instance.currentFlavour;
        UIManager.Instance.SetFlavourLabel(flavour);
        ball.UpdateBallFlavour(flavour);
        paddle.UpdatePaddleFlavour(flavour);
    }

    /// <summary>
    /// Sets the variable responsible for determining if the player scored or died (scored = false, died = true).
    /// </summary>
    /// <param name="answer"></param>
    public void SetDidPlayerScoreOrDieBool(bool answer)
    {
        didPlayerScoreOrDie = answer;
    }

    /// <summary>
    /// Checks whether or not the player scored (score multiplier opportunity) or died.
    /// </summary>
    /// <returns></returns>
    public bool DidPlayerScoreOrDie()
    {
        if (!didPlayerScoreOrDie)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    /// <returns></returns>
    public bool CheckIfGameIsOver()
    {
        if (ball.playerLives < 0)
        {
            isGameOver = true;
            return true;
        }
        else
        {
            isGameOver = false;
            return false;
        }
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        ball.FireBallUponReset();
        ResumeTimer();
    }

    /// <summary>
    /// Pause the game timer.
    /// </summary>
    public void PauseTimer()
    {
        pauseTimer = true;
    }

    /// <summary>
    /// Resume the game timer.
    /// </summary>
    public void ResumeTimer()
    {
        pauseTimer = false;
    }

    /// <summary>
    /// Checks if the game is paused.
    /// </summary>
    /// <returns></returns>
    public bool isGamePaused()
    {
        if (pauseTimer)
            return true;
        else
            return false;
    }
   
}
