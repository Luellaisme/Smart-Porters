using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUIScript : MonoBehaviour {

    public Canvas introCanvas;
    public Canvas menuCanvas;
    public Canvas quitCanvas;
    public Button skipText;
    public Button playText;
    public Button quitText; 

    public static Canvas menuCanvas1;
    public static Canvas playCanvas1;
    public static Text skipText1;

    // Use this for initialization
    void Start () {
        menuCanvas1 = menuCanvas.GetComponent<Canvas>();
        menuCanvas.enabled = false;
        quitCanvas.enabled = false;
        Screen.SetResolution(912, 608, true);
    }

    //skip按钮操作
    public void SkipPress()
    {
        menuCanvas.enabled = true;
        introCanvas.enabled = false;        
        skipText.enabled = false;
        Debug.Log("yes");
    }

    //Play按钮操作
    public void PlayPress()
    {
        //quitCanvas.enabled = false;
        //menuCanvas.enabled = false;
        //playCanvas.enabled = true;
        SceneManager.LoadScene("Main Scene");
    }


    ////Try按钮操作
    //public void TryPress()
    //{
    //    SceneManager.LoadScene("Main Scene"); 
    //}


    //Quit按钮操作
    public void QuitPress()
    {
        quitCanvas.enabled = true;
        menuCanvas.enabled = false;
        playText.enabled = false;
        quitText.enabled = false;
    }

    //No按钮操作
    public void NoPress()
    {
        quitCanvas.enabled = false;
        menuCanvas.enabled = true;
        playText.enabled = true;
        quitText.enabled = true;
    }

    //Yes按钮操作
    public void ExitGame()
    {
        Application.Quit();
    }

	// Update is called once per frame
	void Update () { 
    }
}
