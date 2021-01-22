using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public sealed class Singleton : MonoBehaviour {

    const string towerNumTag = "Tower Number Dropdown";
    TowerFactory towerFactory;
    [SerializeField] Text levelText;
    // do not put a singleton in first map, it has static base turret for level one.
    public List<int> enemyList = new List<int>();
    // this holds the set tower choices
    public Tower tempTower;
    public Tower towerOne;
    public Tower towerTwo;
    public Tower towerThree;

    public Tower towerOneBase = null;
    public GameObject towerOneHead = null;
    public int towerOneBaseType = -1;
    public int towerOneHeadType = -1;
    public string towerOneName = "";

    public Tower towerTwoBase = null;
    public GameObject towerTwoHead = null;
    public int towerTwoBaseType = -1;
    public int towerTwoHeadType = -1;
    public string towerTwoName = "";

    public Tower towerThreeBase = null;
    public GameObject towerThreeHead = null;
    public int towerThreeBaseType = -1;
    public int towerThreeHeadType = -1;
    public string towerThreeName = "";

    static public Dictionary<string, float> towerDamages = new Dictionary<string, float>();

    protected Dropdown dropdown;

    public EnemyHealth preferedTargetEnemy = null;


    TowerSelecter towerSelector;

    int towerButton = 0;
    public static Singleton Instance { get; private set; }

    [SerializeField] public int scenesChanged;
    public int level = 2;
    private int waveEnemyDifficultyChecker = 0;

    public bool isHasPickedAPath = false;
    public bool isHasLearnedATower = false;
    public bool ishasLearnedTinker = false;

    private bool isTutorial = false;
    private bool hasExplainedComputerRoom = false;
    private bool hasExplainedEngineeringRoom = false;
    private bool hasExplainedTinkerRoom = false;
    private bool hasExplainedTurretRoom = false;
    private bool hasExplainedMeetingRoom = false;

    [Header("Rifle Tower")]
    [SerializeField] public Tower basicRifledTowerBase;
    [SerializeField] public GameObject basicRifledTowerHead;

    public void TowerOne()
    { 
        towerFactory = new TowerFactory();
        towerFactory.AddTower(towerOne);
    }

    // TODO something with the load destroying references, that i cannot apply new towers (old references bad !!! THE TOWER BUTTONS!!!!
    // This keeps the old towers, they are never reset.  Look into this, maybe the singleton is getting destroyed?  either way they persist when everything else wipes maybe error there.  They have a bad towerfactory!!!
    // it ge ts routed to creation wiht a singleton it gets destroyed.  After load it doesnt work it has no activation.
    //Fix, route the function through something  that persists, THEN that thing calls the singleton function.  Roundabout but works.

    // Use this for initialization
    void Start()
    {
        if (towerOneBase == null)
        {
            towerOneName = "Rifled Towers";
            towerOneBase = basicRifledTowerBase;
            towerOneHead = basicRifledTowerHead;
            towerOneBaseType = (int)RifledBase.Basic;
            towerOneHeadType = (int)RifledHead.Basic;
        }

        levelText.text = "Level : " + level.ToString();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            
        }
    }

    public void LoadTowerPreset(int towerNum, string name, int headType, int baseType) // add more in here for passins.
    {
        switch (towerNum)
        {
            case 1:
                towerOneName = name;
                towerOneHeadType = headType;
                towerOneBaseType = baseType;
                break;
            case 2:
                towerTwoName = name;
                towerTwoHeadType = headType;
                towerTwoBaseType = baseType;
                break;
            case 3:
                towerThreeName = name;
                towerThreeHeadType = headType;
                towerThreeBaseType = baseType;
                break;
        }
    }

    public void FindTower()
    {
        Tower towerBase = null;
        GameObject towerHead = null;
        int baseType = -1;
        int headType = -1;
        string towerName = "";
        //dropdown = GameObject.FindGameObjectWithTag(towerNumTag).GetComponent<Dropdown>();
        towerSelector = FindObjectOfType<TowerSelecter>();
        tempTower = towerSelector.PickTower(ref towerBase, ref towerHead, ref baseType, ref headType, ref towerName);
        //temp tower holds the new tower, swtich determines what button it takes over.need to convert to Tower instetad of towerDmG
        switch (towerSelector.towerSlotSelected)
        {
            case 1:
                //towerOne = tempTower;
                towerOneBase = towerBase;
                towerOneHead = towerHead;
                towerOneBaseType = baseType;
                towerOneHeadType = headType;
                towerOneName = towerName;
                //print(towerOne.name);
                //FindObjectOfType<TowerButton1>().UpdateName();
                break;
            case 2:
                towerTwoBase = towerBase;
                towerTwoHead = towerHead;
                towerTwoBaseType = baseType;
                towerTwoHeadType = headType;
                towerTwoName = towerName;
                break;
            case 3:
                towerThreeBase = towerBase;
                towerThreeHead = towerHead;
                towerThreeBaseType = baseType;
                towerThreeHeadType = headType;
                towerThreeName = towerName;
                //towerThree = tempTower;
                //FindObjectOfType<TowerButton3>().UpdateName();
                break;
            default:
                break;
        }


    }

 //           __   Slant font           __           ____            ____                                      __     _                
 //          / /   ___  _   __  ___    / /          /  _/   ____    / __/  ____    _____   ____ ___   ____ _  / /_   (_)  ____    ____ 
 //         / /   / _ \| | / / / _ \  / /           / /    / __ \  / /_   / __ \  / ___/  / __ `__ \ / __ `/ / __/  / /  / __ \  / __ \
 //        / /___/  __/| |/ / /  __/ / /          _/ /    / / / / / __/  / /_/ / / /     / / / / / // /_/ / / /_   / /  / /_/ / / / / /
 //       /_____/\___/ |___/  \___/ /_/          /___/   /_/ /_/ /_/     \____/ /_/     /_/ /_/ /_/ \__,_/  \__/  /_/   \____/ /_/ /_/ 
                                                                                                                             
                                
     public void SetIsTutorial(bool _isTutorial)
     {
        isTutorial = _isTutorial;
     }

    public void SetBaseTutorialStatus(bool computerRoom, bool engineeringRoom, bool tinkerRoom, bool turretRoom, bool meetingRoom)
    {
        hasExplainedComputerRoom = computerRoom;
        hasExplainedEngineeringRoom = engineeringRoom;
        hasExplainedTinkerRoom = tinkerRoom;
        hasExplainedTurretRoom = turretRoom;
        hasExplainedMeetingRoom = meetingRoom;
    }
    public void SetIsComputerExplained(bool explained)
    {
        hasExplainedComputerRoom = explained;
    }
    public void SetIsEngineeringExplained(bool explained)
    {
        hasExplainedEngineeringRoom = explained;
    }
    public void SetIsTinkerExplained(bool explained)
    {
        hasExplainedTinkerRoom = explained;
    }
    public void SetIsTurretExplained(bool explained)
    {
        hasExplainedTurretRoom = explained;
    }
    public void SetIsMeetingExplained(bool explained)
    {
        hasExplainedMeetingRoom = explained;
    }

    // THESE ARE HIDDEN IN THE BUTTONS.  When room swapping it triggers a check to see fi it needs to explain it.
    public bool GetIsComputerExplained()
    {
        return hasExplainedComputerRoom;
    }
    public bool GetIsEngineeringExplained()
    {
        return hasExplainedEngineeringRoom;
    }
    public bool GetIsTinkerExplained()
    {
        return hasExplainedTinkerRoom;
    }
    public bool GetIsTurretExplained()
    {
        return hasExplainedTurretRoom;
    }
    public bool GetIsMeetingExplained()
    {
        return hasExplainedMeetingRoom;
    }

    public bool GetIsTutorial()
    {
        return isTutorial;
    }

     public void LevelCleared()
     {
        level++;
        print(level + "is the level now!!!!!!");
        levelText.text = "Level : " + level.ToString();

        // loops all towers that damaged this round and tells you how well they did.  Then clears.
        //foreach (string key in towerDamages.Keys)
        //{
        //    print(key + ": " + towerDamages[key]);
        //}
        towerDamages.Clear();
     }

    /// <summary>
    /// TODO check if it needs optimizing.  This adds the tower damage caused every frame, and by which towers.  Im not sure if this is too much.
    /// </summary>
    /// <param name="towerTypeName"></param>
    /// <param name="damageToAdd"></param>
    public static void AddTowerDamage(string towerTypeName, float damageToAdd)
    {
        if (towerDamages.ContainsKey(towerTypeName))
        {
            float currentDmg = towerDamages[towerTypeName];
            currentDmg += damageToAdd;
            towerDamages[towerTypeName] = currentDmg;
        } else
        {
            towerDamages.Add(towerTypeName, damageToAdd);
        }
    }

    public void LoadLevel(int loadedLevel)
    {
        level = loadedLevel;
        levelText.text = "Level : " + level.ToString();
    }

    public void SetLevel(int level)
    {
        this.level = level;
        levelText.text = "Level : " + level.ToString();
    }


    // Update is called once per frame
    void Update()
    {

    }

//          ______                                      
//         / ____/  ____     ___     ____ ___     __ __
//        / __/    / __ \   / _ \   / __ `__ \   / / / /
//       / /___   / / / /  /  __/  / / / / / /  / /_/ / 
//      /_____/  /_/ /_/   \___/  /_/ /_/ /_/   \__, /  
//                                             /____/ 


    public void SetPreferedEnemy(EnemyHealth newEnemy)
    {
        Tower[] towers = FindObjectsOfType<Tower>();
        if (towers.Length != 0) {
            foreach (Tower tower in towers)
            {
                tower.preferedEnemyBody = newEnemy;
            }
            FindObjectOfType<PreferedEnemyPanel>().SetTargetEnemy(newEnemy);
        }
    }

    public void DecidedPath(List<int> chosenEnemies)
    {
        enemyList = chosenEnemies;
        //foreach (int x in enemyList)
        //{
        //    print(x);
        //}
    }

    public List<int> GetEnemyList()
    {
        return enemyList;
    }

    // Maybe move this out of here and to a script inside of base room control / meeting room?
    public List<int> CreateEnemyList(List<int> newList)
    {
        for (int x = 0; x < 5; x++)
        {
            waveEnemyDifficultyChecker = 8;
            while(waveEnemyDifficultyChecker > 0)
            {
                newList.Add(PickARandoEnemy());
            }

            newList.Add(-1);
        }   
        return newList;
    }

    public int PickARandoEnemy()
    {
        int enemy = 0;
        int rng = UnityEngine.Random.Range(0, 100);
        if(rng < 75)
        { // change to max reg enemy.
            enemy = UnityEngine.Random.Range(1, 5);
            waveEnemyDifficultyChecker -= 1;
        } else
        {
            enemy = UnityEngine.Random.Range(20, 22);
            waveEnemyDifficultyChecker -= 2;
        }

        return enemy;
    }

    public void LoadEnemyList(int[] enemies)
    {
        enemyList.Clear();
        foreach(int enemy in enemies)
        {
            enemyList.Add(enemy);
        }
    }



    public void MassDelayedStart()
    {
        if(towerSelector == null){
            towerSelector = FindObjectOfType<TowerSelecter>();
        }
        
        IEnumerator start;
        start = towerSelector.DelayedStart();

        StartCoroutine(start);
    }
    //private static Singleton instance = null;
    //private static readonly object padlock = new object();

    //Singleton()
    //{
    //    int scenesChanged = 0;

    //}

    //public static Singleton Instance
    //{
    //    get
    //    {
    //        lock (padlock)
    //        {
    //            if (instance == null)
    //            {
    //                instance = new Singleton();
    //            }
    //            return instance;
    //        }
    //    }
    //}

    //    _______       __                __ __                            __         
    //   /_ __ (_)___  / / _____ _____   / / / /___  ____ __________ _____/ /__ _____
    //    / / / / __ \/ //_/ _ \/ ___/  / / / / __ \/ __ `/ ___/ __ `/ __  / _ \/ ___/
    //   / / / / / / / ,< /  __/ /     / /_/ / /_/ / /_/ / /  / /_/ / /_/ /  __(__  )
    //  /_/ /_/_/ /_/_/|_|\___/_/      \____/ .___/\__, /_/   \__,_/\__,_/\___/____/  
    //                                     /_/    /____/                                          
    private List<int> tinkerUpgrades = null;

    public void SendUpdateTinkerUpgrades(List<int> _tinkerUpgrades)
    {
        CheckIfNeedList();
        tinkerUpgrades = _tinkerUpgrades;
    }

    public void GetUpdateTinkerUpgrades()
    {
        TinkerUpgrades upgrades = FindObjectOfType<TinkerUpgrades>();
        tinkerUpgrades = upgrades.GetTinkerUpgrades();
    }

    public int GetResearchLevel(int x)
    {
        CheckIfNeedList();
        return tinkerUpgrades[x];
    }

    private void CheckIfNeedList()
    {
        if (tinkerUpgrades == null)
        {
            TinkerUpgrades upgrades = FindObjectOfType<TinkerUpgrades>();
            tinkerUpgrades = upgrades.GetTinkerUpgrades();
        }
    }

    /// <summary>
    /// This is a generic function to fetch the percentage (high end decimal) of change.  IE mark one will return .92.
    /// You pass in the Tinker upgrade number, and this gets the version then percent.snippingsertserpow
    /// </summary>  tester
    /// <param name="tinkerUpgrade"></param>
    /// <returns></returns>
    public float GetPercentageModifier(int tinkerUpgrade)
    {
        int version = -1;
        try
        {
            CheckIfNeedList();
            version = tinkerUpgrades[tinkerUpgrade];
        } catch (Exception e)
        {
            version = -1;
            // to see if this will throw an exception below, or hit default.
        }
        float returnPercentModifier = 1.0f;
        try
        {
            switch (version)
            {
                case 0:
                    //this is 100 - 100 is 0% bonus
                    returnPercentModifier = 100.0f;
                    break;
                case 1:
                    returnPercentModifier = (float)TinkerUpgradePercent.mark1;
                    break;
                case 2:
                    returnPercentModifier = (float)TinkerUpgradePercent.mark2;
                    break;
                case 3:
                    returnPercentModifier = (float)TinkerUpgradePercent.mark3;
                    break;
                case 4:
                    returnPercentModifier = (float)TinkerUpgradePercent.mark4;
                    break;
                default:
                    // this is probably a 0, sent as -1, first run things are unlearned.
                    //Debug.Log("Error, case exceeded expected");
                    //print("Error, case exceeded expected");
                    returnPercentModifier = 100.0f;
                    break;
            }
            returnPercentModifier = returnPercentModifier / 100f;
        } catch (Exception e)
        {
            Debug.Log("Error, tinkerUpgrade not found");
            print("Error, tinkerUpgrade not found");
        }
        return returnPercentModifier;
    }

    public bool targettingModule = false;
    public bool goldWiring = false;
    public bool platinumWiring = false;
    public bool diamondWiring = false;

    public bool alloyReasearchI = false;
    public bool alloyReasearchII = false;
    public bool alloyReasearchIII = false;
    public bool alloyReasearchIV = false;

    public bool sturdyTankI = false;
    public bool sturdyTankII = false;
    public bool sturdyTankIII = false;
    public bool sturdyTankIV = false;

    public bool heavyShellingI = false;
    public bool heavyShellingII = false;
    public bool heavyShellingIII = false;
    public bool heavyShellingIV = false;

    public bool towerEngineerI = false;
    public bool towerEngineerII = false;
    public bool towerEngineerIII = false;
    public bool towerEngineerIV = false;


}
