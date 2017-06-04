using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScript : MonoBehaviour
{
    Image fill;
    Image background;
    Text time;
    float energyLeft;
    float energyDeSpeed = 1f;
    float energyInSpeed = 0.5f;
    float energyRate = 1f;
    public static float energyDeCoff = 1f;

    float timer;
    float energyAddTime = 3f;

    public static bool hasEnergy = true;

    private bool scdFlag = true;
    private Vector3 waterPos;
    private Vector3 startPos;

    private float waterY;

    private float yChange = 0.1f;
    // Use this for initialization
    void Start()
    {
        time = GameObject.Find("EnergyTime").GetComponent<Text>();
        fill = GameObject.Find("EnergyFill").GetComponent<Image>();
        background = GameObject.Find("EnergyBack").GetComponent<Image>();
        time.enabled = false;

        energyLeft = SPWholeVar.energyTotal;
        timer = energyAddTime;

        waterY = GameObject.Find("WaterProDaytime").transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(SPWholeVar.isGameBegin)
        {
            energyRate = energyLeft / SPWholeVar.energyTotal;
            if ( Serial.energyForce > 0.01f)   ////移动开始消耗能量、、、、、
            {
                energyDeCoff = Serial.energyForce;
                energyLeft -= energyDeSpeed * energyDeCoff;
            }
            if (energyLeft > 0)
            {
                fill.rectTransform.sizeDelta = new Vector2(background.rectTransform.rect.width * (energyRate - 1f), fill.rectTransform.sizeDelta.y);
                fill.rectTransform.anchoredPosition = new Vector2(background.rectTransform.rect.width * (energyRate - 1f) / 2f, fill.rectTransform.sizeDelta.y);
                if (timer == energyAddTime || energyLeft>= SPWholeVar.energyTotal)
                {
                    hasEnergy = true;
                    time.enabled = false;
                }
            }
            if(energyLeft<=0 || timer<energyAddTime)
            {
                time.enabled = true;
                time.text = "Time: " + timer.ToString("0.") + "s";
                hasEnergy = false;
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    hasEnergy = true;
                    timer = energyAddTime;
                }

                if (SPWholeVar.isFstBegin)
                {
                    //if (scdFlag)
                    //{
                    //    scdFlag = false;
                    //    waterPos = new Vector3(SPWholeVar.character.transform.position.x, waterY, SPWholeVar.character.transform.position.z);
                    //    //startPos = SPWholeVar.character.transform.position;
                    //    print("1");
                    //}
                    if (SPWholeVar.character.transform.position.y < waterY - yChange * 12)
                    {
                        SPWholeVar.character.transform.Translate(0, yChange * 2, 0);
                    }
                    else if (SPWholeVar.character.transform.position.y > waterY - yChange * 10)
                    {
                        SPWholeVar.character.transform.Translate(0, -yChange * 2, 0);
                    }

                }
            }
            else
            {
                scdFlag = true;
            }
            if (energyLeft < SPWholeVar.energyTotal)
            {
                energyLeft += energyInSpeed;
            }
        }

    }
   
}
