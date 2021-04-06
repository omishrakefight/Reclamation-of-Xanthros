using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SaveSerializedObject  {

    public bool[] towerList;
    public int currentLevel = 1;

    public bool hasChosenEnemies = false;
    public int[] enemyList;
    public int[] enemyOption1List;
    public int[] enemyOption2List;

    public bool hasChosenATower = false;

    //Tinker room
    public int[] currentUpgradeLevels;
    public int[] learnableUpgrades;
    public int[] possibleOptions;
    public List<int> upgradedThisRound;
    public bool hasPicked;
    public int currentPickNum;
    public int maxPickNum;

    public List<int> List;
    public Dictionary<string, int> dic;
    public Dictionary<string, Dictionary<string, int>> knownTowersAndParts;
    public Dictionary<string, Dictionary<string, int>> learnableTowersAndParts;
    public Dictionary<string, Dictionary<string, int>> unlearnableTowersAndParts;


    public TowerObject towerOne = new TowerObject();
    public TowerObject towerTwo = new TowerObject();
    public TowerObject towerThree = new TowerObject();


    public bool isTutorial = false;
    public bool hasExplainedComputerRoom = false;
    public bool hasExplainedEngineerRoom = false;
    public bool hasExplainedTinkerRoom = false;
    public bool hasExplainedTurretRoom = false;
    public bool hasExplainedMeetingRoom = false;

    public string zoneName = "";

    // add the bool

    /// <summary>
    /// I need to make more parts.  I am going to assimilate them into a single dictionary of learned ones string int,
    ///  then I need to make the keys strings that have tower type at front (flame), and end with other type (base) and name in middle.\
    ///  could change enums to be spaced every 20, and then have a big bool array for towers parts known.
    /// </summary>

    public SaveSerializedObject()
    {
    }

    public void SaveZoneName(string zone)
    {
        zoneName = zone;
    }

    public void SaveTowerPreset(int towerNum, string name, int headType, int baseType) // add more in here for passins.
    {

        try
        {

            switch (towerNum)
            {
                case 1:
                    towerOne = new TowerObject();
                    towerOne.SetTowerName(name);
                    towerOne.SetTowerHeadType(headType);
                    towerOne.SetTowerBaseType(baseType);
                    //towerOne.   add aprameters in the function call.
                    break;
                case 2:
                    towerTwo = new TowerObject();
                    towerTwo.SetTowerName(name);
                    towerTwo.SetTowerHeadType(headType);
                    towerTwo.SetTowerBaseType(baseType);
                    break;
                case 3:
                    towerThree = new TowerObject();
                    towerThree.SetTowerName(name);
                    towerThree.SetTowerHeadType(headType);
                    towerThree.SetTowerBaseType(baseType);
                    break;
            }

        }
        catch (Exception e)
        {
            //print(e.Message.ToString());
        }
    }

    public void SaveTinkerRoomInfo(int[] _currentUpgradeLevels, int[] _learnableUpgrades, int[] _possibleOptions,  int _currentPickNum, int _maxPickNum, List<int> _upgradedAlready)//bool _hasPicked,
    {
        currentUpgradeLevels = _currentUpgradeLevels;
        learnableUpgrades = _learnableUpgrades;
        possibleOptions = _possibleOptions;
        //hasPicked = _hasPicked;
        upgradedThisRound = _upgradedAlready;
        maxPickNum = _maxPickNum;
        currentPickNum = _currentPickNum;
    }

    public void SetTutorial(bool _isTutorial)
    {
        isTutorial = _isTutorial;
    }

    public void IsHasChosenATower(bool chosen)
    {
        hasChosenATower = chosen;
    }

    public void SaveTowers(bool[] towerListSaves)
    {
        towerList = towerListSaves;
    }

    public void SaveTowersAndParts(Dictionary<string, Dictionary<string, int>> _knownTowersAndParts, Dictionary<string, Dictionary<string, int>> _learnableTowersAndParts, Dictionary<string, Dictionary<string, int>> _unlearnableTowersAndParts)
    {
        knownTowersAndParts = _knownTowersAndParts;
        learnableTowersAndParts = _learnableTowersAndParts;
        unlearnableTowersAndParts = _unlearnableTowersAndParts;
    }


    public void IsHasChosenEnemies(bool hasChosen)
    {
        hasChosenEnemies = hasChosen;
    }

    public void SaveEnemiesChosen(int[] enemies)
    {
        enemyList = enemies;
    }

    public void SaveEnemyOptions(int[] option1, int[] option2)
    {
        enemyOption1List = option1;
        enemyOption2List = option2;
    }

    public void UpdateCurrentLevel()
    {
        
    }

    //public void UpdateTowerList(bool[] newTowerList)
    //{
    //    towerList = newTowerList;
    //}
    //Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
}
