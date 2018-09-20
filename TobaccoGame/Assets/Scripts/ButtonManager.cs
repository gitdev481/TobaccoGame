using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    #region SINGLETON PATTERN
    public static ButtonManager _instance;
    public static ButtonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ButtonManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("ButtonManager");
                    _instance = container.AddComponent<ButtonManager>();
                }

            }
            return _instance;
        }
    }
    #endregion


    float titleScreenFadeTime = 0.5f;
   // public Renderer titleScreenRenderer;
    public Image titleScreenRenderer;
    private bool startTitleScreenFade = false;
  
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        ManageTitleScreenFade();
    }

    public void ManageTitleScreenFade()
    {
        if (!startTitleScreenFade)
            return;

        Material tempMaterial = Instantiate<Material>(titleScreenRenderer.material);
        titleScreenRenderer.material = tempMaterial;
            
           
        Color tempColor = tempMaterial.color;

        tempMaterial.color = new Color(tempColor.r, tempColor.g, tempColor.b, tempColor.a - (Time.deltaTime/ titleScreenFadeTime));
        // Color tempColor = titleScreenRenderer.GetComponent<Renderer>().material.color;
        // tempColor.a -= Time.deltaTime / titleScreenFadeTime;
        // titleScreenRenderer.GetComponent<Renderer>().material.color = tempColor;

        if (tempColor.a <= 0)
           startTitleScreenFade = false;
      
    }

    //Resets the game (goes back to the title screen).
    public void ResetGame()
    {

    }

    //Fades the title screen out and disables it
    public void TitleScreenFadeToGame()
    {
        startTitleScreenFade = true;
    }


    //Controls the functionality of the Page 2 Play Button
    public void Page2PlayButton()
    {
        
    }
}
