using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// This class is attached to each row of tiles, and handle the functionality of that tile row.
/// </summary>
public class TileRow : MonoBehaviour {

    #region variables
    public enum TileFlavour { Apple, Bourbon, Cherry, Creme, Ginger, Ice, Latte, Lemon, Mango, Menthol, Tobacco }
    public TileFlavour tileFlavour;

    public List<Sprite> tileSprites = new List<Sprite>(11);
    public List<Image> tiles = new List<Image>();
    public bool inFrontRow = false;
    public bool isTileBottom = true;
    public bool setupViaSpawn = false;

    #endregion

    void Start ()
    {
        if(!setupViaSpawn)
            SetTileSprites();
    }

    /// <summary>
    /// Sets the sprites of all tiles in the row to be the correct flavour.
    /// </summary>
    public void SetTileSprites()
    {
        tiles = gameObject.GetComponentsInChildren<Image>().ToList();
        
        foreach (Image tile in tiles)
        {
            tile.sprite = tileSprites[(int)tileFlavour];
        }
    }
}
