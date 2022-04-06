using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] TextMeshProUGUI numberDisplay;
    [SerializeField] GameObject menuDisplay;
    [SerializeField] TextMeshProUGUI helpDisplay;
    [SerializeField] BlockPlacer blockPlacer;
    [SerializeField] UITweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        if(GameState.instance.CurrentState == GameState.State.Menu)
        {
            //numberDisplay.gameObject.SetActive(false);
            //helpDisplay.gameObject.SetActive(false);
            //menuDisplay.SetActive(true);
            tweener.StartClosed(numberDisplay.gameObject);
            tweener.StartClosed(helpDisplay.gameObject);
            tweener.StartOpen(menuDisplay);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckValue(int value)
    {
        Debug.Log("menu value " + value.ToString());
        switch (value)
        {
            case 2001:
                if(GameState.instance.CurrentState == GameState.State.Menu)
                {
                    //Close the menu and open the game
                    tweener.OnClose(menuDisplay, numberDisplay.gameObject);
                }
                else if(GameState.instance.CurrentState == GameState.State.Help)
                {
                    //Close the help display and open the game
                    tweener.OnClose(helpDisplay.gameObject, numberDisplay.gameObject);
                }
                GameState.instance.CurrentState = GameState.State.Game;
                
                //Start spawning the correct blocks
                ObjectPooler.instance.DisablePool("MenuPlacer");
                blockPlacer.UpdateListLength();
                Debug.Log("Block placer called from ui");
                break;
            case 2002:
                //Open the help display and close the menu 
                GameState.instance.CurrentState = GameState.State.Help;
                tweener.OnClose(menuDisplay, helpDisplay.gameObject);

                break;
            case 2003:
                //Menu state
                GameState.instance.CurrentState = GameState.State.Menu;
                break;
            case 2004:
                //Quit the game
                GameState.instance.CurrentState = GameState.State.Quit;
                Application.Quit();
                break;
        }
    }

}
