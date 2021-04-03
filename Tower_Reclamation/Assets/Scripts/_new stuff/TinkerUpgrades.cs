using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TinkerUpgrades : MonoBehaviour {

    //TODO make sure that this flips the bool in singleton such that the button stops glowing.  Make sure singleton is updated to this bool on loading a saved game.
    public static List<int> currentUpgradeLevels = new List<int>();
    public static List<int> learnableUpgrades = new List<int>();
    public static List<int> possibleOptionsFromSave = new List<int>();
    public static List<int> possibleOptions = new List<int>();
    public static List<int> possibleOptionsFromNewBase = new List<int>();
    public static List<int> learnedUpgrades = new List<int>();


    protected static List<int> pickedUpgrades = new List<int>();
    //public static bool hasPicked;
    public static int currentPickNum = 0;
    public static int maxPickNum = 2;

    public bool isSelected = false;
    public static int numSelected;
    public int randomPick;
    public Color baseColor;
    int version = 0;

    // this is the location in current upgrades, gotten with rando in learable
    private int chosenNumber;

    static private bool isLoaded = false;
    static private bool loadMeOnce = true;

    // KYLE CHECK to see if I only need one of these, set it on a button, then custom set up each serialized field, same script though.
    [SerializeField] Text Hint;
    [SerializeField] Text description;
    string selectedDescription = "";
    // maybe do a button
    [SerializeField] Text buttonName;
    // Use this for initialization

    // Have a single array that keeps track of the highest version IE [0, 0, 4, 1, 1], means no upgrades first two and a mark 4 on the third.  Have the array known.
    // maybe store that way for saves, but it is easier to utilize seperate.  have them as ints, and then have silver wiring = 0.  upgrade, silver wiring = 1.  
    // Base it off the ints, an do a switchh to keep it easier?
	void Start () {
        if (!isLoaded && loadMeOnce)
        {
            learnableUpgrades = new List<int>() { 0, 1, 2, 3, 4 };
            currentUpgradeLevels = new List<int>() { 0, 0, 0, 0, 0 };
            // not completely true, but to make sure it doesnt loop
            numSelected = 0;
            maxPickNum = 2;
            loadMeOnce = false;
            //hasPicked = false;
            Hint.text = hintPickMore + (maxPickNum - currentPickNum) + " more."; ;

            // since i stop this from happening twice, loop all immediately.
            Transform parent = transform.parent;
            TinkerUpgrades[] tinkerBtns = parent.GetComponentsInChildren<TinkerUpgrades>();
            foreach (TinkerUpgrades upgrades in tinkerBtns)
            {
                upgrades.PickTower();
            }
            //PickTower();
            //print(" only once!!!");
            
        }

        
        baseColor = buttonName.GetComponentInParent<Button>().GetComponent<Image>().color;
    }

    public void UpdateDescription()
    {
        description.text = selectedDescription;

        if (isSelected)
        {
            numSelected--;
            buttonName.GetComponentInParent<Button>().GetComponent<Image>().color = baseColor;
            isSelected = false;
            pickedUpgrades.Remove(chosenNumber);
        }
        else 
        {
            //if (numSelected < 2 && !hasPicked) // if (numSelected < upgradesLeft)
            //{
            //    numSelected++;
            //    buttonName.GetComponentInParent<Button>().GetComponent<Image>().color = Color.cyan;
            //    isSelected = true;
            //    pickedUpgrades.Add(chosenNumber);
            //}
            if (numSelected < (maxPickNum - currentPickNum)) // && !hasPicked) // if (numSelected < upgradesLeft)
            {
                numSelected++;
                buttonName.GetComponentInParent<Button>().GetComponent<Image>().color = Color.cyan;
                isSelected = true;
                pickedUpgrades.Add(chosenNumber);
            }
        }
    }

    // Select the buttons that are highlighted.  This loops and adds back to the list of learnable upgrades.
    //It also ups the upgrade level.
    public void Selected()
    {
        if (maxPickNum > currentPickNum) // tODO change this to if has 0 count upgrades, and fitler that way
        {
            Transform parent = transform.parent;

            TinkerUpgrades[] tinkerBtns = parent.GetComponentsInChildren<TinkerUpgrades>();
            print("test.count = " + tinkerBtns.Length);
            foreach (TinkerUpgrades upgrades in tinkerBtns)
            {
                if (upgrades.isSelected)
                {
                    numSelected--;
                    currentPickNum++;
                    upgrades.isSelected = false;
                    currentUpgradeLevels[upgrades.chosenNumber]++;
                    DetermineIfHasAnotherUpgrade(upgrades.chosenNumber);
                    learnedUpgrades.Add(upgrades.chosenNumber);
                    upgrades.buttonName.GetComponentInParent<Button>().GetComponent<Image>().color = baseColor;
                    upgrades.buttonName.GetComponentInParent<Button>().interactable = false;//.IsInteractable();// = false;
                }
                else
                {
                    //print("select not Saved");
                    //learnableUpgrades.Add(upgrades.chosenNumber);
                }
            }

            if (currentPickNum >= maxPickNum) {

                //hasPicked = true;
                Hint.text = outOfResearch;
                Singleton.Instance.SendUpdateTinkerUpgrades(currentUpgradeLevels);
                Singleton.Instance.ishasLearnedTinker = true;
            }
            else
            {
                Hint.text = hintPickMore + (maxPickNum - currentPickNum) + " more."; 
                // add in button disable and remove lightup.
            }
        }
    }

    public void DetermineIfHasAnotherUpgrade(int position)
    {
        switch (position)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                if(currentUpgradeLevels[position] >= 4)
                {
                    learnableUpgrades.Remove(position);
                }
                break;
            default:
                break;
        }
    }

    public void AddToBackupList()
    {
        foreach(int x in possibleOptionsFromSave)
        {
            possibleOptions.Add(x);
        }
    }

    public void PickTower()
    {
        try
        {
            if (learnableUpgrades.Count == 0)
            {
                buttonName.text = "Nothing New";
                selectedDescription = "Nothing new could be found.";
                // KYLE TODO do better than simply not exist/  Maybe disable or put in that selecting them does a different action andl iterally is a waste.
                GetComponent<Button>().interactable = false;
                //print("nothing new");
                return;
            }

            // if it is a loaded base, special load.
            if (possibleOptions.Count > 0)
            {
                // if first loop after instantiation changes false, need a 2nd round reset
                GetComponent<Button>().interactable = true;

                if (possibleOptions.Count == 0)
                {
                    buttonName.text = "Nothing New";
                    selectedDescription = "Nothing new could be found.";
                    // KYLE TODO do better than simply not exist/  Maybe disable or put in that selecting them does a different action andl iterally is a waste.
                    //gameObject.SetActive(false);
                    GetComponent<Button>().interactable = false;
                    return;
                }
                chosenNumber = possibleOptions[0];
                possibleOptions.RemoveAt(0);

            } // else do a normal new load.
            else
            {
                // rework this,  Make it use the second  array.
                //Problem here, is riddled with.  If I dont select, or other crap they dont get removed.  I think it would be easier to instead
                // use the second list like when I load.  Then who cares if that list gets botched.
                possibleOptionsFromNewBase.Clear();
                possibleOptions.Clear();

                foreach (int possibleOption in learnableUpgrades)
                {
                    possibleOptionsFromNewBase.Add(possibleOption);
                }

                for(int i = 0; i < 4; i++)
                {
                    randomPick = UnityEngine.Random.Range(0, possibleOptionsFromNewBase.Count);
                    chosenNumber = possibleOptionsFromNewBase[randomPick];
                    possibleOptions.Add(chosenNumber);
                    possibleOptionsFromSave.Add(chosenNumber);
                    //print("Rando pick is " + randomPick + "   And learnable count is: " + (learnableUpgrades.Count));
                    possibleOptionsFromNewBase.RemoveAt(randomPick);
                }

                // this is only hit if !possibleOptions.Count > 0) which means it needs initialization and a re-try.
                this.PickTower();
                return;
            }

            version = currentUpgradeLevels[chosenNumber] + 1;
            //print("I have chosen: " + chosenNumber);

            DetermineButtonNameAndDescription(version);
        }
        catch (Exception e)
        {
            buttonName.text = "Nothing New";
            selectedDescription = "Nothing new could be found.";
            gameObject.SetActive(false);
        }
    }

    private void DowngradeAndTurnButtonInactive()
    {
        version = currentUpgradeLevels[chosenNumber];

        DetermineButtonNameAndDescription(version);

        buttonName.GetComponentInParent<Button>().GetComponent<Image>().color = baseColor;
        buttonName.GetComponentInParent<Button>().interactable = false;//.IsInteractable();// = false;
    }

    private void DetermineButtonNameAndDescription(int version)
    {
        switch (chosenNumber)
        {
            case 0:
                buttonName.text = "Targetting Module: Mark " + version.ToString();
                switch (version)
                {
                    case 1:
                        selectedDescription = targettingModuleI;
                        break;
                    case 2:
                        selectedDescription = targettingModuleII;
                        break;
                    case 3:
                        selectedDescription = targettingModuleIII;
                        break;
                    case 4:
                        selectedDescription = targettingModuleIV;
                        break;
                }
                break;
            case 1:   //silver o, alloy 1, pressurized tank 2, heavy shelling 3, tower engineer 4
                buttonName.text = "Alloy Research: Mark " + version.ToString();
                switch (version)
                {
                    case 1:
                        selectedDescription = alloyReasearchI;
                        break;
                    case 2:
                        selectedDescription = alloyReasearchII;
                        break;
                    case 3:
                        selectedDescription = alloyReasearchIII;
                        break;
                    case 4:
                        selectedDescription = alloyReasearchIV;
                        break;
                }
                break;
            case 2:
                buttonName.text = "Pressurized Tanks: Mark " + version.ToString();
                switch (version)
                {
                    case 1:
                        selectedDescription = sturdyTankI;
                        break;
                    case 2:
                        selectedDescription = sturdyTankII;
                        break;
                    case 3:
                        selectedDescription = sturdyTankIII;
                        break;
                    case 4:
                        selectedDescription = sturdyTankIV;
                        break;
                }
                break;
            case 3:
                buttonName.text = "Heavy Shelling: Mark " + version.ToString();
                switch (version)
                {
                    case 1:
                        selectedDescription = heavyShellingI;
                        break;
                    case 2:
                        selectedDescription = heavyShellingII;
                        break;
                    case 3:
                        selectedDescription = heavyShellingIII;
                        break;
                    case 4:
                        selectedDescription = heavyShellingIV;
                        break;
                }
                break;
            case 4:
                buttonName.text = "Tower Engineering: Mark " + version.ToString();
                switch (version)
                {
                    case 1:
                        selectedDescription = towerEngineerI;
                        break;
                    case 2:
                        selectedDescription = towerEngineerII;
                        break;
                    case 3:
                        selectedDescription = towerEngineerIII;
                        break;
                    case 4:
                        selectedDescription = towerEngineerIV;
                        break;
                }
                break;
        }
    }

    //public List<int> UpdateTinkerUpgrades()
    //{
    //    return currentUpgradeLevels;
    //}


    public int[] SaveCurrentUpgradeLevels()
    {
        return currentUpgradeLevels.ToArray();
    }
    public int[] SaveLearnableUpgrades()
    {
        return learnableUpgrades.ToArray();
    }
    public int[] SavePossibleOptions()
    {
        return possibleOptionsFromSave.ToArray();
    }
    //public bool SaveHasPicked()
    //{
    //    return hasPicked;
    //}
    public int SaveMaxPickNum()
    {
        return maxPickNum;
    }
    public int SaveCurrentPickNum()
    {
        return currentPickNum;
    }
    public List<int> SaveLearnedUpgrades()
    {
        return learnedUpgrades;
    }

    /// <summary>
    /// This loads the upgrades saved information (whats learned / can be leared ect...  This excludes if one has already learned something.
    /// </summary>
    /// <param name="_currentUpgradeLevels"></param>
    /// <param name="_learnableUpgrades"></param>
    /// <param name="_possibleOptions"></param>
    /// <param name="_hasPicked"></param>
    /// <param name="_hasAlreadyRolledForUpgrades"></param>
    public void LoadInfoAndSavedOptions(int[] _currentUpgradeLevels, int[] _learnableUpgrades, int[] _possibleOptions, bool _hasAlreadyRolledForUpgrades, int _currentPickNum, int _maxPickNum, List<int> _learnedUpgrades)
    { //bool _hasPicked
        isLoaded = true;// _hasAlreadyRolledForUpgrades;
        //hasPicked = _hasPicked;
        currentPickNum = _currentPickNum;
        maxPickNum = _maxPickNum;
        currentUpgradeLevels.Clear();
        learnableUpgrades.Clear();
        possibleOptionsFromSave.Clear();
        possibleOptions.Clear();
        learnedUpgrades.Clear();

        numSelected =  0;

        learnedUpgrades = _learnedUpgrades;
        //        if (hasPicked)
        if (maxPickNum <= currentPickNum)
        {
            Singleton.Instance.ishasLearnedTinker = true;
            Hint.text = outOfResearch;
        }
        else
        {
            Hint.text = hintPickMore + (maxPickNum - currentPickNum) + " more.";
        }

        foreach (int x in _currentUpgradeLevels)
        {
            currentUpgradeLevels.Add(x);
        }
        foreach (int x in _learnableUpgrades)
        {
            learnableUpgrades.Add(x);
        }
        foreach (int x in _possibleOptions)
        {
            possibleOptionsFromSave.Add(x);
            possibleOptions.Add(x);
        }

        if (learnableUpgrades.Count == 0)
        {
            // then theres nothing more you can learn
            Singleton.Instance.ishasLearnedTinker = true;
        } else if(learnableUpgrades.Count < maxPickNum)
        {
            maxPickNum = learnableUpgrades.Count;
        }

        Transform parent = transform.parent;
        TinkerUpgrades[] tinkerBtns = parent.GetComponentsInChildren<TinkerUpgrades>();
        foreach (TinkerUpgrades upgrades in tinkerBtns) {
            upgrades.PickTower();

            foreach (int AlreadyUpgraded in learnedUpgrades)
            {
                if (upgrades.chosenNumber == AlreadyUpgraded)
                {
                    upgrades.DowngradeAndTurnButtonInactive();
                }
            }
        }


        //give singleton the upgrades
        Singleton.Instance.SendUpdateTinkerUpgrades(currentUpgradeLevels);
    }

    public List<int> GetTinkerUpgrades()
    {
        return currentUpgradeLevels;
    }

    // hmm not bad if I count on getting 10 of these with 4 upgrades each, 40 upgrades is not bad.
    string hintPickMore = "You have more options to research, please pick ";
    string outOfResearch = "You have no new options to research";

    string targettingModuleI = "With an updated sensor kit, the tower can now target units further away.";
    string alloyReasearchI = "By studying the art of alloy smelting, one can produce more quantity of the metals.  Increasing supply has lowered the cost.";
    string sturdyTankI = "With research into more compressed tanks, we can put the product inside under more pressure, primarily increasing its effectiveness, but also slightly its range.";
    string heavyShellingI = "With study into the carpaces of the aliens, we have found out how to make the ballistics penetrate them more easily.";
    string towerEngineerI = "Through a better study of towers, the ramping cost of continuous upgrades is cheaper.";
    //Throught

    string targettingModuleII = "With an updated sensor kit, the tower can now target units even further away. Mark II";
    string alloyReasearchII = "By studying the art of alloy smelting, one can produce more quantity of the metals.  Increasing supply has lowered the cost.  Mark II";
    string sturdyTankII = "With research into more compressed tanks, we can put the product inside under more pressure, primarily increasing its effectiveness, but also slightly its range.  Mark II";
    string heavyShellingII = "With study into the carpaces of the aliens, we have found out how to make the ballistics penetrate them more easily.  Mark II";
    string towerEngineerII = "Through a better study of towers, the ramping cost of continuous upgrades is cheaper.  Mark II";

    string targettingModuleIII = "With an updated sensor kit, the tower can now target units even further away.  Mark III";
    string alloyReasearchIII = "By studying the art of alloy smelting, one can produce more quantity of the metals.  Increasing supply has lowered the cost.   Mark III";
    string sturdyTankIII = "With research into more compressed tanks, we can put the product inside under more pressure, primarily increasing its effectiveness, but also slightly its range.   Mark III";
    string heavyShellingIII = "With study into the carpaces of the aliens, we have found out how to make the ballistics penetrate them more easily.  Mark III";
    string towerEngineerIII = "Through a better study of towers, the ramping cost of continuous upgrades is cheaper.   Mark III";

    string targettingModuleIV = "With an updated sensor kit, the tower can now target units even further away.  Mark IV";
    string alloyReasearchIV = "By studying the art of alloy smelting, one can produce more quantity of the metals.  Increasing supply has lowered the cost.   Mark IV";
    string sturdyTankIV = "With research into more compressed tanks, we can put the product inside under more pressure, primarily increasing its effectiveness, but also slightly its range.   Mark IV";
    string heavyShellingIV = "With study into the carpaces of the aliens, we have found out how to make the ballistics penetrate them more easily.   Mark IV";
    string towerEngineerIV = "Through a better study of towers, the ramping cost of continuous upgrades is cheaper.   Mark IV";
    //string x = "";
    //string x = "";
    //string x = "";
    //string x = "";
    string scavengerI = "Through an edept scavenging eye, you start with more tower resources.";
    string usableCart = "Finding a cart that was repairable, you can now cart some metal beams from site to site, increasing Base life.";
    string rifleI = "In desperate situation, people will do anything.  But YOU found the rifle so its yours right?";
    string tarI = "In desperation, people will do anything.  This shows them were the tar and fire is for when the aliens make it to your bases door.";

    // check level layout and monster quantity.  earn this?
    // maybe have final upgrades for if you complete a full set? like starting gold +++++ is start wiht a free tower?

}
