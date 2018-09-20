using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private Vector3 moveDownVector = new Vector3(0, -59, 0);
    public GameObject bottomTilesSpawnPosition;
    public GameObject tilesBottomPrefab;
    public GameObject tilesTopPrefab;
    public GameObject spawnedTilesParent;

    private int nextLeftRowFlavour;
    private int nextRightRowFlavour;
    #endregion

    void Start () {
		
	}
	
	
	void Update () {
		
	}

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



    public void MoveAllTilesDown()
    {
        FindAllTiles();

        for(int i = 0; i < allTileRows.Count; i++)
        {
            if (allTileRows[i].GetComponent<TileRow>().isTileBottom)
            {
                allTileRows[i].gameObject.transform.localPosition = -moveDownVector;
            }
            else
            {
                allTileRows[i].gameObject.transform.localPosition = moveDownVector;
            }
            
        }

    }

    private void FindAllTiles()
    {
        allTileRows.AddRange(GameObject.FindGameObjectsWithTag("LEFT"));
        allTileRows.AddRange(GameObject.FindGameObjectsWithTag("RIGHT"));
    }

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
        }
        temp.transform.SetParent(spawnedTilesParent.transform, false);
        temp.transform.GetChild(0).GetComponent<TileRow>().setupViaSpawn = true;
        temp.transform.GetChild(0).GetComponent<TileRow>().tileFlavour = (TileRow.TileFlavour)nextLeftRowFlavour;
        temp.transform.GetChild(0).GetComponent<TileRow>().SetTileSprites();
        temp.transform.GetChild(1).GetComponent<TileRow>().setupViaSpawn = true;
        temp.transform.GetChild(1).GetComponent<TileRow>().tileFlavour = (TileRow.TileFlavour)nextRightRowFlavour;
        temp.transform.GetChild(1).GetComponent<TileRow>().SetTileSprites();
    }

    public void SetupTileFlavours(int childIndex)
    {

    }

    public void SetupNextFlavours(bool isLeftRow, int flavour)
    {
        if (isLeftRow)
            nextLeftRowFlavour = flavour;
        else
            nextRightRowFlavour = flavour;
    }
}
