﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        //  transform.GetChild(0).GetComponent<Image>().sprite = tileSprites[0];
        if(!setupViaSpawn)
            SetTileSprites();

    }

    public void SetTileSprites()
    {
        tiles = gameObject.GetComponentsInChildren<Image>().ToList();
        
        foreach (Image tile in tiles)
        {
            tile.sprite = tileSprites[(int)tileFlavour];
        }
    }
}
