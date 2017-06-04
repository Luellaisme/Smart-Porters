
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrincipleCanvas : MonoBehaviour {
    public static int page = 0;
    public static RawImage[] prinPages;
    RawImage prinTexture;
    Texture deTexture;
    Button[] turnPagesButton;

    //关卡素材
    private GameObject prinGuid;
    private RawImage[] prinGuidPages;

    GameObject prinWind;
    RawImage[] prinWindPages;
    GameObject prinWater;
    RawImage[] prinWaterPages;
    GameObject prinMountain;
    RawImage[] prinMountainPages;

    // Use this for initialization
    void Start () {
        prinTexture = SPWholeVar.principle.GetComponent<RawImage>();
        deTexture = prinTexture.texture;
        turnPagesButton = SPWholeVar.principle.GetComponentsInChildren<Button>();
        foreach (Button butn in turnPagesButton)
            butn.gameObject.SetActive(false);

        prinGuid = GameObject.Find("Guid Principles");
        prinGuidPages = prinGuid.GetComponentsInChildren<RawImage>();
        prinGuid.SetActive(false);

        prinWind = GameObject.Find("Wind Principles");
        prinWindPages = prinWind.GetComponentsInChildren<RawImage>();
        prinWind.SetActive(false);
        prinWaterPages = GameObject.Find("Water Principles").GetComponentsInChildren<RawImage>();
        GameObject.Find("Water Principles").SetActive(false);
        prinMountainPages = GameObject.Find("Mountain Principles").GetComponentsInChildren<RawImage>();
        GameObject.Find("Mountain Principles").SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (SPWholeVar.isGameBegin)
        {
            SPWholeVar.principle.SetActive(true);               
        }
        if (SPWholeVar.isGuide && prinPages != prinGuidPages)
        {
            prinPages = prinGuidPages;
            page = 0;
        }
        if (SPWholeVar.isFstBegin && prinPages !=prinWaterPages)
        {
            prinPages = prinWaterPages;
            page = 0;
        }
        if (SPWholeVar.isScdBegin && prinPages != prinWindPages)
        {
            prinPages = prinWindPages;
            page = 0;
        }
        if (SPWholeVar.isThrdBegin && prinPages != prinMountainPages)
        {
            prinPages = prinMountainPages;
            page = 0;
        }
        //开始展示第一页公式，打开翻页按钮
        if (prinPages!=null)
        {
            if (page == 0)
                prinTexture.texture = prinPages[page].texture;
            if (page >= 0 && page <= prinPages.Length - 2)
                turnPagesButton[1].gameObject.SetActive(true); 
            else
                turnPagesButton[1].gameObject.SetActive(false); 
            if (page >= 1 && page <= prinPages.Length-1)           
                turnPagesButton[0].gameObject.SetActive(true); 
            else
                turnPagesButton[0].gameObject.SetActive(false);
        }
    }

    public void OnNextClick()
    {
        
        if (page <= prinPages.Length-1)
        {
            page += 1;
            prinTexture.texture = prinPages[page].texture;
        }
    }
    public void OnPreviousClick()
    {
        if (page >= 1)
        {
            page -= 1;
            prinTexture.texture = prinPages[page].texture;
        }
    }
}
