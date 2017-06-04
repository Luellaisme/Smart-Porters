using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MountainScript : MonoBehaviour {

    MoveChestScript moveChest;
    GameObject mountainOpt;
    GameObject mountain;
    public Material woodMaterial;
    public Material mentalMaterial;
    public Material rockMaterial;
    public Material glassMaterial;
    Toggle mountainWood;
    Toggle mountainMental;
    Toggle mountainRock;
    Toggle mountainGlass;
    float woodAngle;
    float mentalAngle;
    float rockAngle;
    float glassAngle;
    float woodDeltaH;
    float mentalDeltaH;
    float rockDeltaH;
    float glassDeltaH;
    float mountainPos;

    // Use this for initialization
    void Start()
    {
        moveChest = new MoveChestScript();
        mountainOpt = GameObject.Find("Mountain Options");
        mountain = GameObject.Find("Mountain");
        mountainWood = GameObject.Find("Wood").GetComponent<Toggle>();
        mountainMental = GameObject.Find("Mental").GetComponent<Toggle>();
        mountainRock = GameObject.Find("Rock").GetComponent<Toggle>();
        mountainGlass = GameObject.Find("Glass").GetComponent<Toggle>();

        woodAngle = float.Parse(GameObject.Find("WoodAngle").GetComponent<Text>().text);
        mentalAngle = float.Parse(GameObject.Find("MentalAngle").GetComponent<Text>().text);
        rockAngle = float.Parse(GameObject.Find("RockAngle").GetComponent<Text>().text);
        glassAngle = float.Parse(GameObject.Find("GlassAngle").GetComponent<Text>().text);

        SPWholeVar.toThisEnd = false;
        SPWholeVar.isCeleOver = false;
        mountainPos = mountain.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (SPWholeVar.isThrdBegin)
        {
            SPWholeVar.thrdLevel.SetActive(true);
            if (mountainWood.isOn || mountainMental.isOn || mountainRock.isOn || mountainGlass.isOn)
            {
                mountain.transform.position = new Vector3(mountain.transform.position.x, mountainPos, mountain.transform.position.z);
                mountainOpt.SetActive(false);
                //Wood
                if(mountainWood.isOn)
                {
                    SPWholeVar.ThrdMateralFlag = 1;
                    SPWholeVar.charMountain.transform.rotation = Quaternion.AngleAxis(woodAngle, Vector3.forward);
                    SPWholeVar.character.transform.rotation = Quaternion.AngleAxis(woodAngle, Vector3.forward);
                    mountain.transform.rotation = Quaternion.AngleAxis(woodAngle, Vector3.forward);
                    woodDeltaH = (mountain.transform.position.x - SPWholeVar.character.transform.position.x) * Mathf.Tan(woodAngle / 180f * Mathf.PI)
                        + 1 / 2f * mountain.transform.localScale.y * (1f - 1f/ Mathf.Cos(woodAngle / 180f * Mathf.PI));
                    mountain.transform.position += new Vector3(0, woodDeltaH, 0);
                    if(woodMaterial)
                        mountain.GetComponent<Renderer>().material = woodMaterial;
                    mountainWood.isOn=false;
                }
                //Mental
                if (mountainMental.isOn)
                {
                    SPWholeVar.ThrdMateralFlag = 2;
                    SPWholeVar.charMountain.transform.rotation = Quaternion.AngleAxis(mentalAngle, Vector3.forward);
                    SPWholeVar.character.transform.rotation = Quaternion.AngleAxis(mentalAngle, Vector3.forward);
                    mountain.transform.rotation = Quaternion.AngleAxis(mentalAngle, Vector3.forward);
                    mentalDeltaH = (mountain.transform.position.x - SPWholeVar.character.transform.position.x) * Mathf.Tan(mentalAngle / 180f * Mathf.PI)
                        + 1 / 2f * mountain.transform.localScale.y * (1f - 1f / Mathf.Cos(mentalAngle / 180f * Mathf.PI));
                    mountain.transform.position += new Vector3(0, mentalDeltaH, 0);
                    if (mentalMaterial)
                        mountain.GetComponent<Renderer>().material = mentalMaterial;
                    mountainMental.isOn = false;
                }
                //Rock
                if (mountainRock.isOn)
                {
                    SPWholeVar.ThrdMateralFlag = 3;
                    SPWholeVar.charMountain.transform.rotation = Quaternion.AngleAxis(rockAngle, Vector3.forward);
                    SPWholeVar.character.transform.rotation = Quaternion.AngleAxis(rockAngle, Vector3.forward);
                    mountain.transform.rotation = Quaternion.AngleAxis(rockAngle, Vector3.forward);
                    rockDeltaH = (mountain.transform.position.x - SPWholeVar.character.transform.position.x) * Mathf.Tan(rockAngle / 180f * Mathf.PI)
                        + 1 / 2f * mountain.transform.localScale.y * (1f - 1f / Mathf.Cos(rockAngle / 180f * Mathf.PI));
                    mountain.transform.position += new Vector3(0, rockDeltaH, 0);
                    if (rockMaterial)
                        mountain.GetComponent<Renderer>().material = rockMaterial;
                    mountainRock.isOn = false;
                }
                //Glass
                if (mountainGlass.isOn)
                {
                    SPWholeVar.ThrdMateralFlag = 4;
                    SPWholeVar.charMountain.transform.rotation = Quaternion.AngleAxis(glassAngle, Vector3.forward);
                    SPWholeVar.character.transform.rotation = Quaternion.AngleAxis(glassAngle, Vector3.forward);
                    mountain.transform.rotation = Quaternion.AngleAxis(glassAngle, Vector3.forward);
                    glassDeltaH = (mountain.transform.position.x - SPWholeVar.character.transform.position.x) * Mathf.Tan(glassAngle / 180f * Mathf.PI)
                        + 1 / 2f * mountain.transform.localScale.y * (1f - 1f / Mathf.Cos(glassAngle / 180f * Mathf.PI));
                    mountain.transform.position += new Vector3(0, glassDeltaH, 0);
                    if (glassMaterial)
                        mountain.GetComponent<Renderer>().material = glassMaterial;
                    mountainGlass.isOn = false;
                }
                AnnCanvasScript.placeCom.SetActive(true);
            }
            if (SPWholeVar.canChestMove)
            {
                moveChest.MoveChest();
            }
            if (SPWholeVar.isCeleOver)    //
            {
                SPWholeVar.isThrdBegin = false;
                SPWholeVar.isGameBegin = false;
                SPWholeVar.isSuccess = true;
            }
        }
        else SPWholeVar.thrdLevel.SetActive(false);
    }

    //打开mountainOptions选项
    public void OnOptionsClick()
    {
        if(!SPWholeVar.canChestMove)
        {
            mountainOpt.SetActive(true);
            if (AnnCanvasScript.placeCom.activeSelf)
                AnnCanvasScript.placeCom.SetActive(false);
        }
    }
}
