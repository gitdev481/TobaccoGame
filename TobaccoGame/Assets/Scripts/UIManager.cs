using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for controlling most of the UI.
/// </summary>
public class UIManager : MonoBehaviour {


    #region SINGLETON PATTERN
    public static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("UIManager");
                    _instance = container.AddComponent<UIManager>();
                }

            }
            return _instance;
        }
    }
    #endregion


    #region variables
    [SerializeField]
    private List<string> flavourNames = new List<string>() { "GREEN APPLE", "BOURBON CARAMEL", "CHERRY CRUSH", "TOBACCO CRÈME", "GINSENG GINGER","BLUE ICE","CAFÈ LATTE", "EUCALYPTUS LEMON", "MANGO APRICOT", "MENTHOL", "TOBACCO FLAVOUR"};
    public List<Color> flavourColours = new List<Color>();
    public Text flavourLabelText;
    private float fivePointTimerThreshold = 0.5f;
    private float fivePointTimer = 0f;
    private bool fivePointTimerStarted = false;
    private float questionFinishedTimer = 0f;
    private float questionFinishedTimerThreshold = 2f;
    private bool questionFinishedTimerStarted = false;
    private float ballFireTimer = 0f;
    private float ballFireTimerThreshold = 1f;
    private bool ballFireTimerStarted = false;
    private float youMissedTimer = 0f;
    private float youMissedTimerThreshold = 1f;
    private bool youMissedTimerStarted = false;
    public GameObject fivePointCircleDisplay;
    public GameObject fivePointScoreDisplay;
    public GameObject questionBackdrop;
    public GameObject doublePointsImage;
    public Image deathPointsImage;
    public Sprite[] deathPointsSprites;
    public GameObject[] questions;
    public bool questionFinished = false;
    private int questionCount = 0;
    public GameObject youMissedImage;
    public Image livesImage;
    public Sprite[] livesSprites;
    public Text playerNameText;
    public Text finalScoreText;
    public string playerNameString;
    public GameObject leaderboardScreen;
    public GameObject flavourLabel;
    public GameObject tileParent;
    public GameObject resetButton;
    public InputField playerNameInputField;
    public Animator timerAnimator;
    public Animator redPlayButtonAnimator;
    public GameObject clickToReleaseImage;
    public GameObject enterYourNameLabel;
    #endregion

    /// <summary>
    /// Restarts the game by reloading the scene.
    /// </summary>
    public void ReloadGame()
    {
        SceneManager.LoadScene("BreakoutGame");
    }

    /// <summary>
    /// Resets the lives image sprite.
    /// </summary>
    public void ResetLivesImage()
    {
        livesImage.sprite = livesSprites[0];
    }

    private void Update()
    {
        ManageFivePointTimer();
        ManageQuestionFinishedTimer();
        ManageBallFireTimer();
        ManageYouMissedTimer();
    }

    /// <summary>
    /// Hides and disables the tutorial pages, as well as fading the timer in.
    /// </summary>
    public void HideTutorial()
    {
        PageManager.Instance.FadeOutTutorial();
        FadeInTimer();
        FadeOutRedPlayButton();
        ShowClickToReleaseImage();
        ShowFlavourLabel();
    }

    /// <summary>
    /// Fades the timer in.
    /// </summary>
    public void FadeInTimer()
    {
        timerAnimator.SetTrigger("FadeIn");
    }

    /// <summary>
    /// Fades out the red Play button.
    /// </summary>
    public void FadeOutRedPlayButton()
    {
        redPlayButtonAnimator.SetTrigger("FadeOut");
    }

    /// <summary>
    /// Shows the 'Click to Release' text.
    /// </summary>
    public void ShowClickToReleaseImage()
    {
        clickToReleaseImage.SetActive(true);
    }

    /// <summary>
    /// Shows the flavour label;
    /// </summary>
    public void ShowFlavourLabel()
    {
        flavourLabel.SetActive(true);
    }

    /// <summary>
    /// Hides the 'Click to Release' text.
    /// </summary>
    public void HideClickToReleaseImage()
    {
        clickToReleaseImage.SetActive(false);
    }

    /// <summary>
    /// Sets the player name string
    /// </summary>
    public void SetPlayerName()
    {
        playerNameString =  playerNameInputField.text.ToUpper();
    }


    /// <summary>
    /// Sets the flavour label to be the correct flavour and colour.
    /// </summary>
    /// <param name="flavour"></param>
    public void SetFlavourLabel(int flavour)
    {
        flavourLabelText.text = flavourNames[flavour];
        flavourLabelText.color = flavourColours[flavour];
    }

    /// <summary>
    /// Manages the display of the 'five point' images that are shown when the player scores five points.
    /// </summary>
    public void ManageFivePointTimer()
    {
        if (!fivePointTimerStarted)
            return;

        if(fivePointTimer < fivePointTimerThreshold)
        {
            fivePointTimer += Time.deltaTime;
        }
        else
        {
            HideFivePointDisplay();
            fivePointTimerStarted = false;
            fivePointTimer = 0;
        }
    }


    /// <summary>
    /// Manages the delay before the hiding the question.
    /// </summary>
    public void ManageQuestionFinishedTimer()
    {
        if (!questionFinishedTimerStarted)
            return;

        if (questionFinishedTimer < questionFinishedTimerThreshold)
        {
            questionFinishedTimer += Time.deltaTime;
        }
        else
        {
            HideQuestionScreen();
            Ball.Instance.ResetBallPosition();
            questionFinishedTimerStarted = false;
            ballFireTimerStarted = true;
            questionFinishedTimer = 0;
        }
    }


    /// <summary>
    /// Manages the delay before the ball is fired again and play resumes.
    /// </summary>
    public void ManageBallFireTimer()
    {
        if (!ballFireTimerStarted)
            return;

        if (ballFireTimer < ballFireTimerThreshold)
        {
            ballFireTimer += Time.deltaTime;
        }
        else
        {
            GameLogicManager.Instance.ResumeGame();
            ballFireTimerStarted = false;
            ballFireTimer = 0;
        }
    }

    /// <summary>
    /// Manages the delay after the "You Missed!" Image appears.
    /// </summary>
    public void ManageYouMissedTimer()
    {
        if (!youMissedTimerStarted)
            return;

        if (youMissedTimer < youMissedTimerThreshold)
        {
            youMissedTimer += Time.deltaTime;
        }
        else
        {
           if(GameLogicManager.Instance.CheckIfGameIsOver())
            {
                PageManager.Instance.FadeInRanOutOfLives();
            }
            else
            {
                UpdateLivesSprite();
                GameLogicManager.Instance.SetDidPlayerScoreOrDieBool(true);
                UIManager.Instance.ShowQuestionScreen(true);
            }
            HideYouMissedImage();
            youMissedTimerStarted = false;
            youMissedTimer = 0;
        }
    }

    /// <summary>
    /// Updates the sprite of the points badge that is shown on the death question.
    /// </summary>
    public void UpdateDeathPointsSprite(int questionAttempts)
    {
        deathPointsImage.sprite = deathPointsSprites[questionAttempts];
    }

    /// <summary>
    /// Change the lives image depending on how many lives the player has left
    /// </summary>
    public void UpdateLivesSprite()
    {
        livesImage.sprite = livesSprites[livesSprites.Length- Ball.Instance.playerLives -1];
    }

    /// <summary>
    /// Shows the "You Missed" image and starts the delay.
    /// </summary>
    public void StartYouMissedDisplay()
    {
        ShowYouMissedImage();
        youMissedTimerStarted = true;
    }

    /// <summary>
    /// Starts the 'five point' display by enabling the boolean.
    /// </summary>
    public void StartFivePointDisplay()
    {
        ShowFivePointDisplay();
        fivePointTimerStarted = true;
    }

    /// <summary>
    /// Enables the five point display.
    /// </summary>
    public void ShowFivePointDisplay()
    {
        fivePointCircleDisplay.SetActive(true);
        fivePointScoreDisplay.SetActive(true);
    }

    /// <summary>
    /// Disables the five point display.
    /// </summary>
    private void HideFivePointDisplay()
    {
        fivePointCircleDisplay.SetActive(false);
        fivePointScoreDisplay.SetActive(false);
    }


    /// <summary>
    /// Shows the death points image.
    /// </summary>
    public void ShowDeathPointsImage()
    {
        deathPointsImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the death points image.
    /// </summary>
    public void HideDeathPointsImage()
    {
        deathPointsImage.gameObject.SetActive(false);
    }


    /// <summary>
    /// Shows the question screen.
    /// </summary>
    /// <param name="didPlayerScoreOrDie"></param>
    public void ShowQuestionScreen(bool didPlayerScoreOrDie)
    {
        questionBackdrop.SetActive(true);

        if (!didPlayerScoreOrDie)
            ShowDoublePointsImage();

        if (questionCount > questions.Length - 1)
            questionCount = 0;

        questions[questionCount].SetActive(true);
        questions[questionCount].GetComponent<Question>().SetupQuestion();

        questionCount++;
    }


    /// <summary>
    /// Hides the question screen.
    /// </summary>
    private void HideQuestionScreen()
    {
        questionFinished = false;
        questionBackdrop.SetActive(false);
        doublePointsImage.SetActive(false);
        UIManager.Instance.HideDeathPointsImage();
        if (questionCount - 1 < 0)
        {
            questions[0].GetComponent<Question>().HideAllAnswerSprites();
            questions[0].SetActive(false);
        }
        else
        {
            questions[questionCount - 1].GetComponent<Question>().HideAllAnswerSprites();
            questions[questionCount - 1].SetActive(false);
        }
    }


    /// <summary>
    /// Shows the double points image.
    /// </summary>
    public void ShowDoublePointsImage()
    {
        doublePointsImage.SetActive(true);
    }

    /// <summary>
    /// Shows the "AHHH! YOU MISSED" message.
    /// </summary>
    public void ShowYouMissedImage()
    {
        youMissedImage.SetActive(true);
    }


    /// <summary>
    /// Hides the "AHHH! YOU MISSED" message.
    /// </summary>
    public void HideYouMissedImage()
    {
        youMissedImage.SetActive(false);

    }

    /// <summary>
    /// Start the question finished timer.
    /// </summary>
    public void TriggerQuestionFinished()
    {    
        questionFinishedTimerStarted = true;
    }

    /// <summary>
    /// Sets up the strings on the Congratulations page.
    /// </summary>
    public void SetupCongratulationsPage()
    {
        playerNameText.text = playerNameString;
        finalScoreText.text = GameLogicManager.Instance.playerScore.ToString();
    }


    /// <summary>
    /// Enables the leaderboard screen.
    /// </summary>
    public void ShowLeaderboardScreen()
    {
        leaderboardScreen.SetActive(true);
    }

    /// <summary>
    /// Hides various UI objects in preparation to show the leaderboard.
    /// </summary>
    public void HideUIForLeaderboard()
    {
        Paddle.Instance.gameObject.SetActive(false);
        Ball.Instance.gameObject.SetActive(false);
        tileParent.SetActive(false);
        flavourLabel.SetActive(false);
        resetButton.SetActive(false);
    }


    /// <summary>
    /// Checks whether or not the player name has been entered.
    /// </summary>
    /// <returns></returns>
    public bool CheckIfNameEntered()
    {
        if (playerNameInputField.text.Length == 0)
        {
            ShowEnterNameLabel();
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Shows the 'Please enter your name' label.
    /// </summary>
    public void ShowEnterNameLabel()
    {
        enterYourNameLabel.SetActive(true);
    }
}
