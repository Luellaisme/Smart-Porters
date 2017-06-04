using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManageScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SPWholeVar.isLose)
            SPWholeVar.loseGame.SetActive(true);
        if(SPWholeVar.isSuccess)
            SPWholeVar.successGame.SetActive(true);
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene("Main Scene");
        SPWholeVar.isSuccess = false;
        SPWholeVar.isLose = false;
        SPWholeVar.isGameBegin = false;
        SPWholeVar.isGuide = false;
        SPWholeVar.isFstBegin = false;
        SPWholeVar.isScdBegin = false;
        SPWholeVar.isThrdBegin = false;
        SPWholeVar.isCalibrate = true;
      
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }
}
