using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Paddle : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {


    #region variables
    Vector3 mousePosition;
    Vector3 wantedPosition;
    private bool paddleHeld = false;
    public List<Sprite> paddleFlavours = new List<Sprite>();
    #endregion



    /// <summary>
    /// Detect current clicks on the GameObject (the one with the script attached).
    /// </summary>
    /// <param name="pointerEventData"></param>
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked.
        Debug.Log(name + "Game Object Click in Progress");
        paddleHeld = true;
    }

    /// <summary>
    /// Detect if clicks are no longer registering.
    /// </summary>
    /// <param name="pointerEventData"></param>
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + "No longer being clicked");
        paddleHeld = false;
    }

    void Update ()
    {
        UpdatePaddlePosition();
    }

    /// <summary>
    /// Moves the paddle in relation to the player's finger movement.
    /// </summary>
    public void UpdatePaddlePosition()
    {
        mousePosition = Input.mousePosition;
        // wantedPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, transform.position.y, 10));
        // wantedPosition = new Vector3(Mathf.Clamp(mousePosition.x,-(Screen.width/2)+100, 1800), transform.position.y, 0);
        wantedPosition = new Vector3(Mathf.Clamp(mousePosition.x, 250, 1800), transform.position.y, 0);

        if (paddleHeld)
        {
            transform.position = wantedPosition;
        }
    }


    /// <summary>
    /// Changes the flavour of the paddle.
    /// </summary>
    /// <param name="flavour"></param>
    public void UpdatePaddleFlavour(int flavour)
    {
        GetComponent<Image>().sprite = paddleFlavours[flavour];
    }
}
