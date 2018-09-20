using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Ball : MonoBehaviour {


    #region variables

    public GameObject ballSpawn;
    private Vector3 ballSpawnPoint;
    private Rigidbody2D ballRigidBody;
    private float ballFireForce = 900f;
    public List<Sprite> ballFlavours = new List<Sprite>();

    private float collisionDelayTimer = 0f;
    private float collisionDelayTimerThreshold = 0.1f;
    private bool collisionDelayTimerStarted = false;
    #endregion

    void Start ()
    {
        ballRigidBody = gameObject.GetComponent<Rigidbody2D>();
        FireBallUponReset();
    }
	
	
	void Update ()
    {
        ManageCollisionCheckDelay();

    }

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
        ballSpawnPoint = ballSpawn.transform.position;
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
    }

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

                    if (CheckIfAllFrontRowsDestroyed())
                    {
                        TileManager.Instance.MoveAllTilesDown();
                        if (collision.gameObject.GetComponent<TileRow>().isTileBottom)
                        {
                            TileManager.Instance.SpawnNewRowOfTiles(true);
                        }
                    }

                    //GameLogicManager.Instance.UpdateCurrentFlavour();
                }
                collisionDelayTimerStarted = true;
            }
        }
       
    }

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
        {
            return false;
        }
        else
        {
            return true;
        }
     // return  (rowStillInFront > 0) ?  true :  false;
               
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


}
