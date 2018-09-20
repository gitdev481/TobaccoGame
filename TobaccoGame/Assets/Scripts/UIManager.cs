using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {


    #region SINGLETON PATTERN
    public static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("UIManager");
                    _instance = container.AddComponent<UIManager>();
                }

            }
            return _instance;
        }
    }
    #endregion


    #region variables
    [SerializeField]
    private List<string> flavourNames = new List<string>() { "GREEN APPLE", "BOURBON CARAMEL", "CHERRY CRUSH", "TOBACCO CRÈME", "GINSENG GINGER","BLUE ICE","CAFÈ LATTE", "EUCALYPTUS LEMON", "MANGO APRICOT", "MENTHOL", "TOBACCO FLAVOUR"};
    public Text flavourLabelText;
    #endregion

    public void SetFlavourLabel(int flavour)
    {
        flavourLabelText.text = flavourNames[flavour];
    }
}
