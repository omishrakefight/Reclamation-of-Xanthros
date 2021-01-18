using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class RandomTowerBlueprints : MonoBehaviour
{

    private int amountOfTowers;
    public int amountOfUndiscoveredTowers;
    MoreInformationPanel prompt;
    [SerializeField] MoreInformationPanel moreInformationPrompt;
    bool[] towers;
    PlayerTowerLog towerLog;
    [SerializeField] TowerSelecter turretTypes;
    Singleton singleton;
    List<string> undiscoveredTowers = new List<string>();
    List<string> discoveredTowers = new List<string>();

    //buttons
    [Header("Button One")]
    [SerializeField] Button towerButtonOne;
    [SerializeField] Text buttonOneText;
    [SerializeField] Image buttonOneImage;

    [Header("Button Two")]
    [SerializeField] Button towerButtonTwo;
    [SerializeField] Text buttonTwoText;
    [SerializeField] Image buttonTwoImage;

    [Header("Button Three")]
    [SerializeField] Button towerButtonThree;
    [SerializeField] Text buttonThreeText;
    [SerializeField] Image buttonThreeImage;

    [Header("Images")]
    [SerializeField] Sprite FlameTowerPic;
    [SerializeField] Sprite PlasmaTowerPic;
    [SerializeField] Sprite SlowTowerPic;
    [SerializeField] Sprite AssaultTowerPic;
    [SerializeField] Sprite LightningTowerPic;
    [SerializeField] Sprite AlreadyKnown;

    [Header("Tutorial Images")]
    [SerializeField] Texture SelectionButtonImage;


    bool towerOneInUse = false;
    bool towerTwoInUse = false;
    bool towerThreeInUse = false;

    private const string AssaultTower = "Its mechanically like the Rifled Tower, except the focus is on attack speed rather than accuracy.  " +
        "The shorter barrel and fire time lends to more of a spray and pray tactic better against big enemies than the smaller ones.";
    private const string FlameTower = "It’s a flamethrower attached to a tower.  " +
        "This tower has a short range area of effect attack that puts a DOT on enemies.  " +
        "Unfortunately, enemies are either burning or not so stacking is less effective.";
    private const string LightningTower = "Charges up ions that when released causes a centered lightning strike.  " +
        "This tower has a short range and slow fire time, having to charge up between each hit.  " +
        "When it does fire, it hits every enemy in a small circle around the tower.";
    private const string PlasmaTower = "This is a rail gun in functionality.  " +
        "It fires a shot that has very good pierce and is able to hit multiple enemies if lined up properly.  " +
        "The tower has a slow fire rate, having to charge each shot; however, the shots deal good damage and can hit multiple enemies.";
    private const string SlowTower = "A simpler tower idea founded on the discovery that these aliens are cold blooded.  " +
        "This tower sprays a slush mix into the air and spreads it out with giant spinning fan-blades.  " +
        "The purpose of this is to slow the aliens down as their blood temperature drops.";

    // Use this for initialization
    void Start()
    {
        //towerLog = GetComponent<PlayerTowerLog>();
        //print("got component!");
        //towers = towerLog.towers1;
        //amountOfTowers = towers.Length;
        //GetAmountOfUndiscoveredTowers();
    }

    // call this from PlayerTowerLog because this laods first and is reliant on that script.
    // also calledo nload.
    public void ManualStart()
    {
        towerLog = GetComponent<PlayerTowerLog>();
        singleton = Singleton.Instance;
        //print(towerLog+ "I am tower log!");
        towers = towerLog.towers1;
        amountOfTowers = towers.Length;
        GetAmountOfUndiscoveredTowers();
        //this populates the buttons and checks if you can learn
        CheckIfCanLearnMoreTowers();

        turretTypes.UpdateTowersAvailable(GetDiscoveredTowers());

    }

    public void ManualStart2(Dictionary<string, Dictionary<string, int>> knownTowerTypes, Dictionary<string, Dictionary<string, int>> learnableTowerTypes)
    {
        towerLog = GetComponent<PlayerTowerLog>();
        singleton = Singleton.Instance;
        amountOfTowers = knownTowerTypes.Keys.Count;
        List<string> towersICanLearn = GetAmountOfUndiscoveredTowers2(knownTowerTypes, learnableTowerTypes);
        //This here is the problem, when i check if i can learn more towers, i check amount of undiscovered towers.
        //Right below I commented out where I initialize that variable.  Find out why i even need it.  Maybe set limit to 3 or amountdiscovered

        //amountOfUndiscoveredTowers = learnableTowerTypes.Keys.Count;
        //GetAmountOfUndiscoveredTowers();
        //this populates the buttons and checks if you can learn
        CheckIfCanLearnMoreTowers2(towersICanLearn);
        List<string> discoveredTowers = new List<string>(knownTowerTypes.Keys);
        turretTypes.UpdateTowersAvailable(discoveredTowers);

    }

    private void CheckIfCanLearnMoreTowers2(List<string> towersICanLearn)
    {
        //TODO change to a specific towers lost pic or something.
        if (singleton.isHasLearnedATower)
        {
            PickTowers2(towersICanLearn);
            towerButtonOne.interactable = false;
            towerButtonTwo.interactable = false;
            towerButtonThree.interactable = false;
        }
        else
        {
            PickTowers2(towersICanLearn);
        }
    }

    private void PickTowers2(List<string> towersICanLearn)
    {
        towerOneInUse = false;
        towerTwoInUse = false;
        towerThreeInUse = false;
        int limit = 3;
        // limit = btn count, amount is how many are new?
        amountOfUndiscoveredTowers = towersICanLearn.Count;

        for (int x = 0; x < limit; x++)
        {
            int rando = UnityEngine.Random.Range(0, towersICanLearn.Count);
            if (x == 0)
            {
                if (amountOfUndiscoveredTowers == 0)
                {
                    towerButtonOne.GetComponentInChildren<Text>().text = "LOCKED";
                    buttonOneText.text = SetupNewButton("no new towers", ref buttonOneImage);

                }
                else
                {
                    towerButtonOne.GetComponentInChildren<Text>().text = towersICanLearn[rando];
                    buttonOneText.text = SetupNewButton(towersICanLearn[rando], ref buttonOneImage);
                    //SetupNewButton(buttonOneText.text, ref buttonOneImage);

                    towersICanLearn.RemoveAt(rando);
                    amountOfUndiscoveredTowers--;
                    towerOneInUse = true;
                    //print("x was 0");
                }
            }
            if (x == 1)
            {
                if (amountOfUndiscoveredTowers == 0)
                {
                    towerButtonTwo.GetComponentInChildren<Text>().text = "LOCKED";
                    buttonTwoText.text = SetupNewButton("no new towers", ref buttonTwoImage);
                }
                else
                {
                    towerButtonTwo.GetComponentInChildren<Text>().text = towersICanLearn[rando];
                    buttonTwoText.text = SetupNewButton(towersICanLearn[rando], ref buttonTwoImage);
                    //SetupNewButton(buttonTwoText.text, ref buttonTwoImage);

                    towersICanLearn.RemoveAt(rando);
                    amountOfUndiscoveredTowers--;
                    towerTwoInUse = true;
                    //print("x was 1");
                }
            }
            if (x == 2)
            {
                if (amountOfUndiscoveredTowers == 0)
                {
                    towerButtonThree.GetComponentInChildren<Text>().text = "LOCKED";
                    buttonThreeText.text = SetupNewButton("no new towers", ref buttonThreeImage);
                }
                else
                {
                    towerButtonThree.GetComponentInChildren<Text>().text = towersICanLearn[rando];
                    buttonThreeText.text = SetupNewButton(towersICanLearn[rando], ref buttonThreeImage);
                    //SetupNewButton(buttonThreeText.text, ref buttonThreeImage);

                    towersICanLearn.RemoveAt(rando);
                    amountOfUndiscoveredTowers--;
                    towerThreeInUse = true;
                    //print("x was 2");
                }
            }
            //print(rando + " is rando number");
            //print(amountOfUndiscoveredTowers + " is undiscovered towers");
        }
    }

    /// <summary>
    /// This loops through the known towers and the learnable towers.  If it finds a match it removes it.  Only the learnable towers that are NOT in known towers
    /// will be passed back to be 'learned' in computer room.  This is because learnable also constitutes the parts, not just the towers.
    /// </summary>
    /// <param name="knownTowerTypes"></param>
    /// <param name="learnableTowerTypes"></param>
    /// <returns></returns>
    private List<String> GetAmountOfUndiscoveredTowers2(Dictionary<string, Dictionary<string, int>> knownTowerTypes, Dictionary<string, Dictionary<string, int>> learnableTowerTypes)
    {
        // this is a list made from the LEARNABLE towers, i then check what towers I know for dupes and delete them from list.
        List<string> towersFromLearnable = new List<string>(learnableTowerTypes.Keys);
        List<string> towersICanLearn = new List<string>();


        for (int x = 0; x < towersFromLearnable.Count; x++)
        {
            if (!knownTowerTypes.ContainsKey(towersFromLearnable[x]))
            {
                towersICanLearn.Add(towersFromLearnable[x]);
            }
        }
        return towersICanLearn;

    }



    public void CheckIfCanLearnMoreTowers()
    {
        //TODO change to a specific towers lost pic or something.
        if (singleton.isHasLearnedATower)
        {
            PickTowers();
            towerButtonOne.interactable = false;
            towerButtonTwo.interactable = false;
            towerButtonThree.interactable = false;
        }
        else
        {
            PickTowers();
        }
    }

    public void ButtonOne()
    {
        // how to get the reference to a booleanspot by a string buttonName
        LearnedANewTower2(towerButtonOne.GetComponentInChildren<Text>().text);
        //if (towerTwoInUse)
        //{
        //    undiscoveredTowers.Add(towerButtonTwo.GetComponentInChildren<Text>().text);
        //    amountOfUndiscoveredTowers++;
        //}
        //if (towerThreeInUse)
        //{
        //    undiscoveredTowers.Add(towerButtonThree.GetComponentInChildren<Text>().text);
        //    amountOfUndiscoveredTowers++;
        //}
        //PickTowers();
        singleton.isHasLearnedATower = true;

        towerButtonThree.interactable = false;
        towerButtonTwo.interactable = false;
    }

    public void ButtonTwo()
    {
        // how to get the reference to a booleanspot by a string buttonName
        LearnedANewTower2(towerButtonTwo.GetComponentInChildren<Text>().text);

        singleton.isHasLearnedATower = true;

        towerButtonOne.interactable = false;
        towerButtonThree.interactable = false;
    }

    public void ButtonThree()
    {
        // how to get the reference to a booleanspot by a string buttonName
        LearnedANewTower2(towerButtonThree.GetComponentInChildren<Text>().text);

        singleton.isHasLearnedATower = true;

        towerButtonOne.interactable = false;
        towerButtonTwo.interactable = false;
    }

    private List<string> GetDiscoveredTowers()
    {

        discoveredTowers.Clear();
        if (towers[(int)Towers.RifledTower] == true)
        {
            discoveredTowers.Add("RifledTower");
        }
        if (towers[(int)Towers.AssaultTower] == true)
        {
            discoveredTowers.Add("AssaultTower");
        }
        if (towers[(int)Towers.FlameTower] == true)
        {
            discoveredTowers.Add("FlameTower");
        }
        if (towers[(int)Towers.LighteningTower] == true)
        {
            discoveredTowers.Add("LighteningTower");
        }
        if (towers[(int)Towers.PlasmaTower] == true)
        {
            discoveredTowers.Add("PlasmaTower");
        }
        if (towers[(int)Towers.SlowTower] == true)
        {
            discoveredTowers.Add("SlowTower");
        }
        //print(discoveredTowers+ "discovered towers");
        return discoveredTowers;
    }

    private void GetAmountOfUndiscoveredTowers()
    {
        undiscoveredTowers.Clear();
        if (towers[(int)Towers.RifledTower] == false)
        {
            undiscoveredTowers.Add("RifledTower");
        }
        if (towers[(int)Towers.AssaultTower] == false)
        {
            undiscoveredTowers.Add("AssaultTower");
        }
        if (towers[(int)Towers.FlameTower] == false)
        {
            undiscoveredTowers.Add("FlameTower");
        }
        if (towers[(int)Towers.LighteningTower] == false)
        {
            undiscoveredTowers.Add("LighteningTower");
        }
        if (towers[(int)Towers.PlasmaTower] == false)
        {
            undiscoveredTowers.Add("PlasmaTower");
        }
        if (towers[(int)Towers.SlowTower] == false)
        {
            undiscoveredTowers.Add("SlowTower");
        }
        amountOfUndiscoveredTowers = undiscoveredTowers.Count;
    }

    public void LearnedANewTower(string buttonName)
    {
        //print("trying to learn " + buttonName);
        if (buttonName.Equals("RifledTower"))
        {
            towerLog.towers1[(int)Towers.RifledTower] = true;
            //print("tower[ " + (int)Towers.RifledTower + "] should be true");
        }
        else if (buttonName.Equals("AssaultTower"))
        {
            towerLog.towers1[(int)Towers.AssaultTower] = true;
            //print("tower[ " + (int)Towers.AssaultTower + "] should be true");

        }
        else if (buttonName.Equals("FlameTower"))
        {
            towerLog.towers1[(int)Towers.FlameTower] = true;
            //print("tower[ " + (int)Towers.FlameTower + "] should be true");

        }
        else if (buttonName.Equals("LighteningTower"))
        {
            towerLog.towers1[(int)Towers.LighteningTower] = true;
            //print("tower[ " + (int)Towers.LighteningTower + "] should be true");

        }
        else if (buttonName.Equals("PlasmaTower"))
        {
            towerLog.towers1[(int)Towers.PlasmaTower] = true;
            //print("tower[ " + (int)Towers.PlasmaTower + "] should be true");

        }
        else if (buttonName.Equals("SlowTower"))
        {
            towerLog.towers1[(int)Towers.SlowTower] = true;
            //print("tower[ " + (int)Towers.SlowTower + "] should be true");

        }

        turretTypes.UpdateTowersAvailable(GetDiscoveredTowers());

    }

    /// <summary>
    /// This function takes in the button name of a learned tower, and then pulls it from LEARNABLE to KNOWN.
    /// </summary>
    /// <param name="buttonName"></param>
    public void LearnedANewTower2(string buttonName)
    {
        Dictionary<string, Dictionary<string, int>> knownTowerTypes = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> learnableTowerTypes = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, int> towerParts = new Dictionary<string, int>();

        if (towerLog == null)
        {
            towerLog = FindObjectOfType<PlayerTowerLog>();
        }

        // here I will add in both references, minus and add
        towerLog.GetKnownAndLearnableTowerRef(ref knownTowerTypes, ref learnableTowerTypes);

        // this is where I get it and delete, LATER TODO I will come in here and take slect parts out.
        towerParts = learnableTowerTypes[buttonName];
        knownTowerTypes.Add(buttonName, towerParts);
        learnableTowerTypes.Remove(buttonName);

        List<string> knownTowerNameList = new List<string>(knownTowerTypes.Keys);

        turretTypes.UpdateTowersAvailable(knownTowerNameList);
    }

    // is passed in towerButtonOne.GetComponentInChildren<Text>().text
    public string SetupNewButton(string towerTextDescription, ref Image image)
    {
        // since it is a pain to pass a reference to button.text, im just going to return the string i want.
        string towerDescription = "";
        if (towerTextDescription.Contains("Rifled"))
        {
            towerLog.towers1[(int)Towers.RifledTower] = true;
            print("tower[ " + (int)Towers.RifledTower + "] should be true");
        }
        //else if (towerTextDescription.Equals("AssaultTower"))
        //{
        //    towerDescription = AssaultTower;
        //    image.sprite = AssaultTowerPic;

        //}
        else if (towerTextDescription.Contains("Flame"))
        {
            towerDescription = FlameTower;
            image.sprite = FlameTowerPic;
        }
        else if (towerTextDescription.Contains("Light"))
        {
            towerDescription = LightningTower;
            image.sprite = LightningTowerPic;

        }
        else if (towerTextDescription.Contains("Plasma"))
        {
            towerDescription = PlasmaTower;
            image.sprite = PlasmaTowerPic;

        }
        else if (towerTextDescription.Contains("Frost"))
        {
            towerDescription = SlowTower;
            image.sprite = SlowTowerPic;

        }
        else
        {
            towerDescription = "Already known.";
            image.sprite = AlreadyKnown;
        }

        return towerDescription;
    }

    // Update is called once per frame
    void Update()
    {
        //PickTowers();
    }

    public void PickTowers()
    {
        towerOneInUse = false;
        towerTwoInUse = false;
        towerThreeInUse = false;
        int limit = 3;
        //if(amountOfUndiscoveredTowers < 3)
        //{
        //    limit = amountOfUndiscoveredTowers;
        //}
        for (int x = 0; x < limit; x++)
        {
            int rando = UnityEngine.Random.Range(0, amountOfUndiscoveredTowers);
            if (x == 0)
            {
                if (amountOfUndiscoveredTowers == 0)
                {
                    towerButtonOne.GetComponentInChildren<Text>().text = "LOCKED";
                    buttonOneText.text = SetupNewButton("no new towers", ref buttonOneImage);

                }
                else
                {
                    towerButtonOne.GetComponentInChildren<Text>().text = undiscoveredTowers[rando];
                    buttonOneText.text = SetupNewButton(undiscoveredTowers[rando], ref buttonOneImage);
                    //SetupNewButton(buttonOneText.text, ref buttonOneImage);

                    undiscoveredTowers.RemoveAt(rando);
                    amountOfUndiscoveredTowers--;
                    towerOneInUse = true;
                    //print("x was 0");
                }
            }
            if (x == 1)
            {
                if (amountOfUndiscoveredTowers == 0)
                {
                    towerButtonTwo.GetComponentInChildren<Text>().text = "LOCKED";
                    buttonTwoText.text = SetupNewButton("no new towers", ref buttonTwoImage);
                }
                else
                {
                    towerButtonTwo.GetComponentInChildren<Text>().text = undiscoveredTowers[rando];
                    buttonTwoText.text = SetupNewButton(undiscoveredTowers[rando], ref buttonTwoImage);
                    //SetupNewButton(buttonTwoText.text, ref buttonTwoImage);

                    undiscoveredTowers.RemoveAt(rando);
                    amountOfUndiscoveredTowers--;
                    towerTwoInUse = true;
                    //print("x was 1");
                }
            }
            if (x == 2)
            {
                if (amountOfUndiscoveredTowers == 0)
                {
                    towerButtonThree.GetComponentInChildren<Text>().text = "LOCKED";
                    buttonThreeText.text = SetupNewButton("no new towers", ref buttonThreeImage);
                }
                else
                {
                    towerButtonThree.GetComponentInChildren<Text>().text = undiscoveredTowers[rando];
                    buttonThreeText.text = SetupNewButton(undiscoveredTowers[rando], ref buttonThreeImage);
                    //SetupNewButton(buttonThreeText.text, ref buttonThreeImage);

                    undiscoveredTowers.RemoveAt(rando);
                    amountOfUndiscoveredTowers--;
                    towerThreeInUse = true;
                    //print("x was 2");
                }
            }
            //print(rando + " is rando number");
            //print(amountOfUndiscoveredTowers + " is undiscovered towers");
        }

    }

    public void CheckIfExplainComputerRoom()
    {
        // if computer is not explained, explain it.
        if (!singleton.GetIsComputerExplained())
        {
            float waitTime = .75f;
            singleton.SetIsComputerExplained(true);

            StartCoroutine(ComputerTutorial(waitTime));
        }

    }

    IEnumerator ComputerTutorial(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        string computerButtonExplained = "The computer hacker has found these tower files clicking one locks you out of learning the others from this computer.  " +
            "This lets you find new parts in the engineering room, or set it up to be used in the next mission in the turret room.";

        List<string> promptTexts = new List<string>() { computerButtonExplained };
        List<Texture> prompImages = new List<Texture>() { SelectionButtonImage };
        prompt = Instantiate(moreInformationPrompt, transform.position, Quaternion.identity, gameObject.transform);
        prompt.DelayedInitialization(prompImages, promptTexts);
    }
}
