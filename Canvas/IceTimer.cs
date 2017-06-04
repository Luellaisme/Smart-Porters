using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceTimer : MonoBehaviour
{
    Image fill;
    Image background;
    float timeLeft;
    float timer = 1f;

    // Use this for initialization
    void Start()
    {
        fill = GameObject.Find("TimerFill").GetComponent<Image>();
        background = GameObject.Find("TimerBack").GetComponent<Image>();
        timeLeft = SPWholeVar.timeTotal;
    }

    // Update is called once per frame
    void Update()
    {
        if (SPWholeVar.isGameBegin)   //开始计时条件
        {
            timeLeft -= Time.deltaTime;
            timer = timeLeft / SPWholeVar.timeTotal;
            if (timeLeft > 0)
            {
                fill.rectTransform.sizeDelta = new Vector2(background.rectTransform.rect.width * (timer - 1f), fill.rectTransform.sizeDelta.y);
                fill.rectTransform.anchoredPosition = new Vector2(background.rectTransform.rect.width * (timer - 1f)/2f, fill.rectTransform.sizeDelta.y);
            }
            else
            {
                SPWholeVar.isLose = true;
                SPWholeVar.isGameBegin = false;
                Debug.Log("lose");
            }
        }
    }
}
