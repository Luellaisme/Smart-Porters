using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPWholeVar : MonoBehaviour {

    //主界面边界比值
    public static float leftEdge = 0.198f;
    public static float rightEdge = 0.784f;
    public static float bottomEdge = 0.388f;
    public static float topEdge = 0.956f;

    //宝箱移动终点坐标
    public static float endPoint = 0.7f;

    //游戏时长
    public static float timeTotal = 600f;
    //能力强度
    public static float energyTotal = 100f;

    //声明场景物体变量
    public static GameObject character;
    public static GameObject chest;
    public static GameObject charWind;
    public static GameObject charWater;
    public static GameObject charMountain;
    public static GameObject charFinal;

    public static GameObject terrainGuide;
    public static GameObject terrainWind;
    public static GameObject terrainWater;
    public static GameObject terrainMountain;

    public static Camera camGuide;
    public static Camera camWind;
    public static Camera camWater;
    public static Camera camMountain;

    public static GameObject WindWall;

    //声明场景画布变量
    public static GameObject calibration;
    public static GameObject prevOn;
    public static GameObject guide;
    public static GameObject announ;
    public static GameObject principle;
    public static GameObject fstLevel;
    public static GameObject scdLevel;
    public static GameObject thrdLevel;

    public static GameObject passThis;
    public static GameObject successGame;
    public static GameObject loseGame;
    
    //声明执行指令
    public static bool isCalibrate = false;
    public static bool isPrevOn = false;
    public static bool isGuide = false;
    public static bool isGameBegin = false;
    public static bool isFstBegin =false;
    public static bool isScdBegin = false;
    public static bool isThrdBegin = false;

    public static bool canChestMove = false;
    public static bool toThisEnd = false;
    public static bool isCeleOver = false;
    public static bool isSuccess = false;
    public static bool isLose = false;

    public static int ThrdMateralFlag = 0;
    //声明执行过程调用变量
    public static Camera curCam;

    void Awake()
    {
        //定义场景物体变量
        character = GameObject.Find("Main Character");
        chest = GameObject.Find("TreasureChest");
        charWind = GameObject.Find("Character_Wind");
        charWater = GameObject.Find("Character_Water");
        charMountain = GameObject.Find("Character_Mountain");
        charFinal = GameObject.Find("Character_Final");

        terrainGuide = GameObject.Find("Terrain_Guide");
        terrainWind = GameObject.Find("Terrain_Wind");
        terrainWater = GameObject.Find("Terrain_Water");
        terrainMountain = GameObject.Find("Terrain_Mountain");

        curCam = null;
        camGuide = GameObject.Find("Guide Camera").GetComponent<Camera>();
        camWind = GameObject.Find("Windzone Camera").GetComponent<Camera>();
        camWater = GameObject.Find("Water Camera").GetComponent<Camera>();
        camMountain = GameObject.Find("Mountain Camera").GetComponent<Camera>();

        //定义场景画布变量
        calibration = GameObject.Find("Calibration");
        //prevOn = GameObject.Find("Previously On");
        guide = GameObject.Find("Guide");
        principle = GameObject.Find("Prin");
        fstLevel = GameObject.Find("FstLevel");
        scdLevel = GameObject.Find("ScdLevel");
        thrdLevel = GameObject.Find("ThrdLevel");
        passThis = GameObject.Find("Pass");
        successGame = GameObject.Find("Success");
        loseGame = GameObject.Find("Lose");
        announ = GameObject.Find("Announ");

        //风墙
        WindWall = GameObject.Find("WindWall");
    }

    // Use this for initialization
    void Start () {
        character.SetActive(false);

        terrainGuide.SetActive(false);
        terrainWind.SetActive(false);
        terrainWater.SetActive(false);
        terrainMountain.SetActive(false);

        calibration.SetActive(false);
        guide.SetActive(false);
        principle.SetActive(false);

        fstLevel.SetActive(false);
        scdLevel.SetActive(false);
        thrdLevel.SetActive(false);
        passThis.SetActive(false);
        successGame.SetActive(false);
        loseGame.SetActive(false);
        //camWind.enabled = false;
        announ.SetActive(false);

        isCalibrate = true;
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
