using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class manages tile spawning.
/// </summary>
public class TileManager : MonoBehaviour {


    #region SINGLETON PATTERN
    public static TileManager _instance;
    public static TileManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TileManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("TileManager");
                    _instance = container.AddComponent<TileManager>();
                }

            }
            return _instance;
        }
    }
    #endregion


    #region variables
    [SerializeField]
    public List<int> tileFlavours = new List<int>();
    public int currentFlavour;
    private List<GameObject> allTileRows = new List<GameObject>();
    private Vector3 moveDownTopVector = new Vector3(0, -59, 0);
    private Vector3 moveDownBottomVector = new Vector3(0, -139, 0);
    public GameObject bottomTilesSpawnPosition;
    public GameObject topTilesSpawnPosition;
    public GameObject tilesBottomPrefab;
    public GameObject tilesTopPrefab;
    public GameObject spawnedTilesParent;

    private int nextLeftRowFlavour;
    private int nextRightRowFlavour;
    #endregion



    /// <summary>
    /// Adds the flavour from the row to the list.
    /// </summary>
    /// <param name="flavour"></param>
    public void AddFlavourToList(int flavour)
    {
        tileFlavours.Add(flavour);
    }

    /// <summary>
    /// Removes the flavour from the list when the ball clears it.
    /// </summary>
    /// <param name="flavour"></param>
    public void RemoveFlavourFromList(int flavour)
    {
        foreach (int item in tileFlavours.ToArray())
        {
            if(item == flavour)
            {
                tileFlavours.Remove(item);
            }
        }
    }

    /// <summary>
    /// Chooses a random flavour that the player must aim for.
    /// </summary>
    public void ChooseRandomFlavour()
    {
        int randomInt = Random.Range(0, tileFlavours.Count);
        currentFlavour = tileFlavours[randomInt];
    }


    /// <summary>
    /// Moves all tiles down.
    /// </summary>
    /// <param name="tilesWereBottom"></param>
    public void MoveAllTilesDown(bool tilesWereBottom)
    {
        FindAllTiles();

        for(int i = 0; i < allTileRows.Count; i++)
        {
            if (allTileRows[i].GetComponent<TileRow>().isTileBottom)
            {
                if (tilesWereBottom)
                {
                    allTileRows[i].gameObject.transform.localPosition -= moveDownTopVector;
                }
                else
                {
                    allTileRows[i].gameObject.transform.localPosition -= moveDownBottomVector;
                }
                
            }
            else
            {
                if (tilesWereBottom)
                {
                    allTileRows[i].gameObject.transform.localPosition += moveDownTopVector;
                }
                else
                {
                    allTileRows[i].gameObject.transform.localPosition += moveDownBottomVector;
                }
            }
            
        }

        allTileRows.Clear();

    }

    /// <summary>
    /// Finds all tiles in the game and adds them to the list.
    /// </summary>
    private void FindAllTiles()
    {
        allTileRows.AddRange(GameObject.FindGameObjectsWithTag("LEFT"));
        allTileRows.AddRange(GameObject.FindGameObjectsWithTag("RIGHT"));
    }

    /// <summary>
    /// Spawns a new row of tiles.
    /// </summary>
    /// <param name="isBottomTiles"></param>
    public void SpawnNewRowOfTiles(bool isBottomTiles)
    {
        GameObject temp;
        if (isBottomTiles)
        {
            temp = Instantiate(tilesBottomPrefab);
           
            temp.transform.localPosition = bottomTilesSpawnPosition.transform.localPosition;
        }
        else
        {
             temp = Instantiate(tilesTopPrefab);
             temp.transform.localPosition = topTilesSpawnPosition.transform.localPosition;
             temp.transform.GetChild(0).GetComponent<TileRow>().isTileBottom = false;
             temp.transform.GetChild(1).GetComponent<TileRow>().isTileBottom = false;
        }
        temp.transform.SetParent(spawnedTilesParent.transform, false);
        temp.transform.GetChild(0).GetComponent<TileRow>().setupViaSpawn = true;
        temp.transform.GetChild(0).GetComponent<TileRow>().tileFlavour = (TileRow.TileFlavour)nextLeftRowFlavour;
        temp.transform.GetChild(0).GetComponent<TileRow>().SetTileSprites();
        temp.transform.GetChild(1).GetComponent<TileRow>().setupViaSpawn = true;
        temp.transform.GetChild(1).GetComponent<TileRow>().tileFlavour = (TileRow.TileFlavour)nextRightRowFlavour;
        temp.transform.GetChild(1).GetComponent<TileRow>().SetTileSprites();
    }

    /// <summary>
    /// Sets up the next flavours for the upcoming tiles.
    /// </summary>
    /// <param name="isLeftRow"></param>
    /// <param name="flavour"></param>
    public void SetupNextFlavours(bool isLeftRow, int flavour)
    {
        if (isLeftRow)
            nextLeftRowFlavour = flavour;
        else
            nextRightRowFlavour = flavour;
    }
}
