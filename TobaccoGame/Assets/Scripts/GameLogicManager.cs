using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    #endregion

    void Start ()
    {
        currentGameTime = maxGameTime;
        startTimer = true;
    }
	
	

	void Update ()
    {
        ManageTimer();
    }

    public void ManageTimer()
    {
        if (!startTimer)
            return;

        if (currentGameTime > 0)
        {
            currentGameTime -= Time.deltaTime;
            timerText.text = currentGameTime.ToString("f2");
        }
        else
        {
            startTimer = false;
            TimerFinished();
        }

    }

    public void TimerFinished()
    {

    }


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

    

}
