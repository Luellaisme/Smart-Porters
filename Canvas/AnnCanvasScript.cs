using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class AnnCanvasScript : MonoBehaviour {
    GameObject chestInfo;
    public static GameObject placeCom;

    //public static GameObject toolsCanvas;
    //public static GameObject toolsButton;
    //public static GameObject toolsText;
    //public static Text toolName;
    //Text toolRemind;

    // Use this for initialization
    void Awake () {

        chestInfo = GameObject.Find("ChestInfo");
        placeCom = GameObject.Find("Place");
        //toolsCanvas = GameObject.Find("Tools Canvas");
        //toolsButton = GameObject.Find("Tools Button");
        //toolsText = GameObject.Find("Tools Text");
        //toolName = GameObject.Find("Tool Name").GetComponent<Text>();

        //toolsCanvas.SetActive(false);
        //toolsButton.SetActive(false);
        //toolsText.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (SPWholeVar.isGameBegin)
        {
            SPWholeVar.announ.SetActive(true);
        }
        else SPWholeVar.announ.SetActive(false);
	}

    //确认Haply归位
    public void OnTargClick()
    {
        SPWholeVar.canChestMove = true;
        placeCom.SetActive(false);
        //第三关，增加关闭材质选框指令
        if (SPWholeVar.isThrdBegin)
            GameObject.Find("Mountain Opt Button").SetActive(false);
        //toolsCanvas.SetActive(true);
    }

    ////点击Tools按钮
    //public void OnToolsClick()
    //{
    //    if (toolsButton.activeSelf)
    //        toolsButton.SetActive(false);
    //    else
    //    {
    //        toolsButton.SetActive(true);
    //    }

    //}

    //public void OnSpringClick()
    //{
    //    toolsText.SetActive(true);
    //    toolName.text = "Spring";
    //    toolsButton.SetActive(false);

    //    if (WindScript.windInfo.activeSelf)
    //        WindScript.windInfo.SetActive(false);
    //}
    //public void OnRopeClick()
    //{
    //    toolsText.SetActive(true);
    //    toolName.text = "Rope";
    //    toolsButton.SetActive(false);

    //    if (WindScript.windInfo.activeSelf)
    //        WindScript.windInfo.SetActive(false);
    //}
}
