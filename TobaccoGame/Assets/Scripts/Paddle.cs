using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for functionality of the paddle.
/// </summary>
public class Paddle : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

    #region SINGLETON PATTERN
    public static Paddle _instance;
    public static Paddle Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Paddle>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("Paddle");
                    _instance = container.AddComponent<Paddle>();
                }

            }
            return _instance;
        }
    }
    #endregion

    #region variables
    Vector3 mousePosition;
    Vector3 wantedPosition;
    private bool paddleHeld = false;
    public List<Sprite> paddleFlavours = new List<Sprite>();
    private bool firstClick = false;
    private float paddleSizeMeasurement;
    public Canvas gameCanvas;
    Vector2 pos;
    #endregion

    void Start()
    {
        paddleSizeMeasurement = transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
       
    }

    void Update()
    {
        UpdatePaddlePosition();
    }

    /// <summary>
    /// Detect current clicks on the GameObject (the one with the script attached).
    /// </summary>
    /// <param name="pointerEventData"></param>
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!firstClick)
        {
            GameLogicManager.Instance.StartGame();
            firstClick = true;
        }
        paddleHeld = true;
    }

    /// <summary>
    /// Detect if clicks are no longer registering.
    /// </summary>
    /// <param name="pointerEventData"></param>
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        paddleHeld = false;
    }

    /// <summary>
    /// Moves the paddle in relation to the player's finger movement.
    /// </summary>
    public void UpdatePaddlePosition()
    {
        if (GameLogicManager.Instance.isGamePaused())
            return;
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        ManageTouchMovement();
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        ManageMouseMovement();
#endif

         wantedPosition = new Vector3(Mathf.Clamp(gameCanvas.transform.TransformPoint(pos).x, 0 + paddleSizeMeasurement, Screen.width - paddleSizeMeasurement), transform.position.y, 0);

        if (paddleHeld)
        {
            transform.position = wantedPosition;
        }
    }

    /// <summary>
    /// Manages mouse movement.
    /// </summary>
    public void ManageMouseMovement()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameCanvas.transform as RectTransform, Input.mousePosition, gameCanvas.worldCamera, out pos);
    }

    /// <summary>
    /// Manages touch movement.
    /// </summary>
    public void ManageTouchMovement()
    {
        Touch touch = Input.GetTouch(0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameCanvas.transform as RectTransform, touch.position, gameCanvas.worldCamera, out pos);
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
