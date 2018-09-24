using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// This class is responsible for the ball logic and functionality.
/// </summary>
public class Ball : MonoBehaviour {

    #region SINGLETON PATTERN
    public static Ball _instance;
    public static Ball Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Ball>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("Ball");
                    _instance = container.AddComponent<Ball>();
                }

            }
            return _instance;
        }
    }
    #endregion


    #region variables

    public GameObject ballSpawn;
    private Rigidbody2D ballRigidBody;
    private float ballFireForce = 1200f;
    public List<Sprite> ballFlavours = new List<Sprite>();

    private float collisionDelayTimer = 0f;
    private float collisionDelayTimerThreshold = 0.1f;
    private bool collisionDelayTimerStarted = false;
    private int destroyedTilesCount = 0;
    private int destroyedTilesCountThreshold = 5;
    public int playerLives = 0;
    private const int maxPlayerLives = 3;
    private float referenceResolution = 2048;
    #endregion

    void Start ()
    {
        ballFireForce /= (referenceResolution / Screen.width); 
        playerLives = maxPlayerLives;
        ballRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }
	
	void Update ()
    {
        ManageCollisionCheckDelay();
    }

    /// <summary>
    /// Manages the collision check delay.
    /// </summary>
    public void ManageCollisionCheckDelay()
    {
        if (!collisionDelayTimerStarted)
            return;
        if (collisionDelayTimer < collisionDelayTimerThreshold)
        {
            collisionDelayTimer += Time.deltaTime;
        }
        else
        {
            collisionDelayTimer = 0;
            collisionDelayTimerStarted = false;
        }
        
    }

    /// <summary>
    /// Resets the ball's position.
    /// </summary>
    public void ResetBallPosition()
    {
        transform.position = ballSpawn.transform.position;
    }

    /// <summary>
    /// Fires the ball at an angle when the ball is reset.
    /// </summary>
    public void FireBallUponReset()
    {
        Vector3 dir = Quaternion.AngleAxis(135f, Vector3.forward) * Vector3.right;
        ballRigidBody.AddForce(dir * ballFireForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Changes the flavour of the ball.
    /// </summary>
    /// <param name="flavour"></param>
    public void UpdateBallFlavour(int flavour)
    {
        GetComponent<Image>().sprite = ballFlavours[flavour];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ManageTileCollisions(collision);
        ManagePlayerDeaths(collision);
    }


    /// <summary>
    /// Responsible for ball logic when it collides with a tile.
    /// </summary>
    /// <param name="collision"></param>
    public void ManageTileCollisions(Collision2D collision)
    {
        if (collisionDelayTimerStarted)
            return;
        
        if (collision.gameObject.tag == "LEFT" || collision.gameObject.tag == "RIGHT")
        {
            if (collision.gameObject.GetComponent<TileRow>().inFrontRow)
            {
                //if the tileflavour is the same as the ball flavour.
                if ((int)collision.gameObject.GetComponent<TileRow>().tileFlavour == TileManager.Instance.currentFlavour)
                {
                    TileManager.Instance.RemoveFlavourFromList(TileManager.Instance.currentFlavour);
                    RemoveAllFrontRowsWithCurrentFlavour(TileManager.Instance.currentFlavour);
                    GameLogicManager.Instance.IncreaseScore(GameLogicManager.Instance.tileDestroyedScore);
                    UIManager.Instance.StartFivePointDisplay();
                    destroyedTilesCount++;
                    if (CheckIfAllFrontRowsDestroyed())
                    {
                       
                        if (collision.gameObject.GetComponent<TileRow>().isTileBottom)
                        {
                            TileManager.Instance.MoveAllTilesDown(true);
                            TileManager.Instance.SpawnNewRowOfTiles(true);
                        }
                        else
                        {
                            TileManager.Instance.MoveAllTilesDown(false);
                            TileManager.Instance.SpawnNewRowOfTiles(false);
                        }
                    }

                    CheckForDoublePointsOpportunity();
                    //GameLogicManager.Instance.UpdateCurrentFlavour();
                }
                collisionDelayTimerStarted = true;
            }
        }
    }

    /// <summary>
    /// Checks if all the front rows are destroyed.
    /// </summary>
    /// <returns></returns>
    public bool CheckIfAllFrontRowsDestroyed()
    {
        TileRow[] tileRows = GameObject.FindObjectsOfType<TileRow>().ToArray();
        int rowStillInFront = 0;
        foreach (TileRow row in tileRows.ToArray())
        {
            if (row.inFrontRow)
            {
                rowStillInFront++;
            }
        }
        if(rowStillInFront> 0)
            return false;     
        else        
            return true;       
    }

    /// <summary>
    /// Destroys all front rows that have the same flavour as the ball.
    /// </summary>
    /// <param name="flavour"></param>
    public void RemoveAllFrontRowsWithCurrentFlavour(int flavour)
    {
        TileRow[] tileRows = GameObject.FindObjectsOfType<TileRow>().ToArray();

        foreach (TileRow row in tileRows.ToArray())
        {
            if (row.inFrontRow)
            {
                if ((int)row.tileFlavour == flavour)
                {
                    row.inFrontRow = false;
                    Destroy(row.gameObject);
                    GameLogicManager.Instance.UpdateCurrentFlavour();
                }
            }
        }
    }


    /// <summary>
    /// Responsible for ball logic when the ball hits the bottom of the screen
    /// </summary>
    /// <param name="collision"></param>
    public void ManagePlayerDeaths(Collision2D collision)
    {
        if (collision.gameObject.tag == "KILLCOLLIDER")
        {
            playerLives--;
            ResetDestroyedTilesCount();
            StopBall();
            GameLogicManager.Instance.PauseTimer();

            UIManager.Instance.StartYouMissedDisplay();
        }

    }

    /// <summary>
    /// Checks whether the player has destroyed enough tiles for a double points opportunity.
    /// </summary>
    public void CheckForDoublePointsOpportunity()
    {
        if(destroyedTilesCount >= destroyedTilesCountThreshold)
        {
            StopBall();
            GameLogicManager.Instance.PauseTimer();
            GameLogicManager.Instance.SetDidPlayerScoreOrDieBool(false);
            UIManager.Instance.ShowQuestionScreen(false);
            destroyedTilesCount = 0;
        }
    }

    /// <summary>
    /// Resets the destroyed tiles count.
    /// </summary>
    private void ResetDestroyedTilesCount()
    {
        destroyedTilesCount = 0;
    }

    /// <summary>
    /// Stops the ball in its place.
    /// </summary>
    public void StopBall()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}
