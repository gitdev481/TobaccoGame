using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to detect the flavours of the bottom row of tiles.
/// </summary>
public class FlavourDetectionWall : MonoBehaviour {

    #region variables
    private int flavourCount;
    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LEFT" || collision.tag == "RIGHT")
        {
            int flavour = (int)collision.GetComponent<TileRow>().tileFlavour;
            //setting up flavours for next row spawn.
            if(collision.tag == "LEFT")
            {
                TileManager.Instance.SetupNextFlavours(true, flavour);
            }
            else if (collision.tag == "RIGHT")
            {
                TileManager.Instance.SetupNextFlavours(false, flavour);
            }
            collision.GetComponent<TileRow>().inFrontRow = true;
            TileManager.Instance.AddFlavourToList(flavour);
            flavourCount++;
            if (flavourCount == 2)
            {
                GameLogicManager.Instance.UpdateCurrentFlavour();

                flavourCount = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "LEFT" || collision.tag == "RIGHT")
        {
            //int flavour = (int)collision.GetComponent<TileRow>().tileFlavour;
            //TileManager.Instance.RemoveFlavourFromList(flavour);

            //GameLogicManager.Instance.UpdateCurrentFlavour();
        }
    }


   
}
