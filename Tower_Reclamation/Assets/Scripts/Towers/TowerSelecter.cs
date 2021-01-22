using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelecter : MonoBehaviour
{
    //#region obsolete
    [Header("Tower Blueprint")]
    [SerializeField] Dropdown towerBarrel;
    [SerializeField] Dropdown towerTurret;
    [SerializeField] Dropdown towerBase;

    Tower newTower;
    Tower decidedTower;

    [Header("Tower 1 saved")]
    [SerializeField] GameObject TowerOnePanel;
    [SerializeField] Text tower1Type;
    [SerializeField] Text tower1Augment;
    [SerializeField] Text tower1Base;

    [Header("Tower 2 saved")]
    [SerializeField] GameObject Tower2Panel;
    [SerializeField] Text tower2Type;
    [SerializeField] Text tower2Augment;
    [SerializeField] Text tower2Base;

    [Header("Tower 3 saved")]
    [SerializeField] GameObject Tower3Panel;
    [SerializeField] Text tower3Type;
    [SerializeField] Text tower3Augment;
    [SerializeField] Text tower3Base;
    //[Header("Rifle Towers")]
    //[SerializeField] Tower basicRifledTower;


    //[Header("Flame Towers")]
    //[SerializeField] Tower basicFlameTower;
    //[SerializeField] Tower tallFlameTower;
    //[SerializeField] Tower heavyFlameTower;
    //[SerializeField] Tower lightFlameTower;
    //[SerializeField] Tower alienFlameTower;


    //[Header("Lightening Towers")]
    //[SerializeField] Tower basicLightTower;

    //[Header("Plasma Towers")]
    //[SerializeField] Tower basicPlasmaTower;

    //[Header("Ice Towers")]
    //[SerializeField] Tower basicIceTower;

    //#endregion

    [Header("Room Items")]
    //[SerializeField] GameObject TowerOnePanel;
    //[SerializeField] Text tower1Type;
    //[SerializeField] Text tower1Augment;
    //[SerializeField] Text tower1Base;

    [SerializeField] GameObject TowerTwoPanel;
    [SerializeField] GameObject TowerThreePanel;


    [SerializeField] public Text TowerTypeDescription;
    [SerializeField] public Text TowerAugmentDescription;
    [SerializeField] public Text TowerBaseDescription;
    [SerializeField] public Text TowerBaseFlavorTxt;
    [SerializeField] public Text TowerStatsTxt;
    PlayerTowerLog towerLog = null;

    Dictionary<string, int> knownTowerParts;

    public int towerSlotSelected = 1;
    bool skipImageReset = false;
    #region TowerParts
    //#TowerParts
    float turnSpeed = 6f;
    GameObject tower = null;

    [SerializeField] GameObject towerPlaceholder;
    Vector3 towerPosition;
    Bounds bound;
    BoxCollider collider;

    [SerializeField] GameObject empty;

    [Header("Flame Base")]
    [SerializeField] Tower basicFlameTowerBase;
    [SerializeField] Tower tallFlameTowerBase;
    [SerializeField] Tower heavyFlameTowerBase;
    [SerializeField] Tower lightFlameTowerBase;
    [SerializeField] Tower alienFlameTowerBase;

    [Header("Flame Head")]
    [SerializeField] GameObject basicFlameTowerHead;
    [SerializeField] GameObject flameThrowerFlameTowerHead;
    [SerializeField] GameObject mortarFlameTowerHead;

    [Header("Rifle Tower Base")]
    [SerializeField] Tower basicRifledTowerBase;
    [SerializeField] Tower rapidRifledTowerBase;

    [Header("Rifle Tower Head")]
    [SerializeField] GameObject basicRifledTowerHead;
    [SerializeField] GameObject sniperRifledTowerHead;

    [Header("Plasma Tower Base")]
    [SerializeField] Tower basicPlasmaTowerBase;

    [Header("Plasma Tower Head")]
    [SerializeField] GameObject basicPlasmaTowerHead;
    [SerializeField] GameObject crystalPlasmaTowerHead;

    [Header("Lightening Tower Base")]
    [SerializeField] Tower basicLightTowerBase;
    [SerializeField] Tower rapidLightTowerBase;


    [Header("Ice Tower Base")]
    [SerializeField] Tower basicIceTowerBase;
    [SerializeField] Tower industrialIceTowerBase;

    [Header("Ice Tower Head")]
    [SerializeField] GameObject basicIceTowerHead;
    private bool changingTowerType = false;
    #endregion

    Singleton singleton;
    // Use this for initialization
    void Start()
    {
        // this value is for the turret room only sandbox.
        //towerPosition = new Vector3(5.2f, -1f, -2.70f);

        //this value is for the base turret room.
        singleton = FindObjectOfType<Singleton>();
        towerPosition = towerPlaceholder.transform.position;

        towerBase.value = 0;
        towerTurret.value = 0;
        towerBarrel.value = 0;

        
        if (towerBarrel.value == 0 && towerTurret.value == 0 && towerBase.value == 0)
        {
            changingTowerType = true;
            ResetTowerPicture();
            //ResetNumbersOnBaseChange();
            //FocusRifledTowers();
        }
        collider = tower.GetComponentInChildren<BoxCollider>();
        bound = collider.bounds;

        //if (!singleton.towerOneName.Equals(""))
        //{
        IEnumerator start;
        start = DelayedStart();

        StartCoroutine(start);

        //}
        //UpdateTowersAvailable();
    }

    public IEnumerator DelayedStart()
    {
        yield return null;

        LoadTowerOne();

        if (singleton.towerOneName == "")
        {
            tower1Type.text = "None";
            tower1Augment.text = "None";
            tower1Base.text = "None";
        }
        else
        {
            LoadSavedTowers(singleton.towerOneName, tower1Type, tower1Augment, tower1Base, singleton.towerOneHeadType, singleton.towerOneBaseType);
        }

        if (singleton.towerTwoName == "")
        {
            tower2Type.text = "None";
            tower2Augment.text = "None";
            tower2Base.text = "None";
        }
        else
        {
            LoadSavedTowers(singleton.towerTwoName, tower2Type, tower2Augment, tower2Base, singleton.towerTwoHeadType, singleton.towerTwoBaseType);
        }

        if (singleton.towerThreeName == "")
        {
            tower3Type.text = "None";
            tower3Augment.text = "None";
            tower3Base.text = "None";
        }
        else
        {
            LoadSavedTowers(singleton.towerThreeName, tower3Type, tower3Augment, tower3Base, singleton.towerThreeHeadType, singleton.towerThreeBaseType);
        }

        yield return null;
    }

        // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //newTower.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);
            var dtx = Input.GetAxis("Mouse X") * turnSpeed;
            // var dty = Input.GetAxis("Mouse Y") * turnSpeed;
            var pivot = bound.center;

            if (tower != null)
            {
                tower.transform.RotateAround(pivot, Vector3.up, dtx);
            } else
            {
                print("used newtiower");
                newTower.transform.RotateAround(pivot, Vector3.up, dtx);
            }
        } else
        {
            var pivot = bound.center;
            //newTower.transform.RotateAround(pivot, Vector3.up, 10f);
            tower.transform.RotateAround(pivot, Vector3.up, .1175f);
        }

    }

    /// <summary>
    /// Dictionary string name value is the int.  Match it on the name, see how i get the name to add to the towers.  O i get it from teh focus function
    /// So change focus.  Ill need to get it from somewhere and make sure names match.  Then i can use that dictionary to fetch numbers without them haveing to be 
    /// in a specific order.  Then i can pass that to thte function to form the towers.
    /// </summary>

    public void ResetNumbersOnBaseChange()
    {
        changingTowerType = true;
        towerBarrel.value = 0;
        towerBase.value = 0;

        //SetTowerBaseAndHead();
        SetTowerBaseAndHead2();
        ResetTowerPicture();
        changingTowerType = false;
    }

    public void LoadSavedTowers(string towerName, Text towerType, Text towerBase, Text towerModifier, int augmentInt, int baseInt)
    {
        if (towerLog == null)
        {
            towerLog = FindObjectOfType<PlayerTowerLog>();
        }
        knownTowerParts = new Dictionary<string, int>();

        knownTowerParts = towerLog.GetTowerParts(towerName);//singleton.towerOneName
        List<string> towerPartsList = new List<string>(knownTowerParts.Keys);
        Dictionary<string, int> augments = new Dictionary<string, int>(); //{ "Basic Barrel", "Sniper Barrel" };
        Dictionary<string, int> bases = new Dictionary<string, int>(); //{ "Basic Base", "Rapid Base" };// we removed some of the supports inside the turret, it allows for easier bullet managment, but comes at the cost of resistance.  We increase rate of fire but lower impact.

        foreach (string s in towerPartsList)
        {
            if (s.ToLower().Contains("base"))
            {
                bases.Add(s, knownTowerParts[s]);
            }
            else
            {
                augments.Add(s, knownTowerParts[s]);
            }
        }

        foreach (KeyValuePair<string, int> x in bases)
        {
            if (x.Value == baseInt)
            {
                towerBase.text = x.Key.ToString(); //tower1Base.text
            }
        }

        foreach (KeyValuePair<string, int> x in augments)
        {
            if (x.Value == augmentInt)
            {
                towerModifier.text = x.Key.ToString(); //tower1Augment.text
            }
        }
        towerType.text = towerName; //tower1type
        //tower1Augment.text = singleton.towerOneHeadType.ToString();
        //tower1Base.text = singleton.towerOneBaseType.ToString();

    }

    // Sent from RandomTowerBlueprints.  it calls this function.
    public void UpdateTowersAvailable(List<string> towersKnown)
    {
        towerTurret.ClearOptions();

        towerTurret.AddOptions(towersKnown);
    }

    public void UpdateTowerType()
    {
        TowerTypeDescription.text = FindObjectOfType<Tower>().GetTypeExplanation();
    }

    public void UpdateTowerAugment()
    {
        TowerAugmentDescription.text = FindObjectOfType<Tower>().GetAugmentExplanation();
    }

    public void UpdateTowerBase()
    {
        TowerBaseDescription.text = FindObjectOfType<Tower>().GetBaseExplanation();
        TowerBaseFlavorTxt.text = FindObjectOfType<Tower>().GetBaseFlavorTxt();
        float length = LayoutUtility.GetPreferredHeight(TowerBaseDescription.rectTransform) + 20f;
        //print(length);
        TowerBaseFlavorTxt.transform.position = (TowerBaseDescription.transform.position - (new Vector3(0f, length, 0f)));
        //TowerBaseFlavorTxt.transform.Translate(new Vector3(0f, -length, 0f));
    }

    public void UpdateTowerStats()
    {
        TowerStatsTxt.text = FindObjectOfType<Tower>().GetTowerStatsExplanation();
    }

    public void ResetTowerPicture()
    {
        if(skipImageReset)
        {
            return;
        }

        Tower towerBase = null;
        GameObject towerHead = null;
        int baseInt = 0, AugmentInt = 0;
        string towerName = "Don't care";

        if (tower == null)
        {
           // DestroyObject(newTower.gameObject);
        } else
        {
            DestroyObject(tower.gameObject);
        }

        decidedTower = PickTower(ref towerBase, ref towerHead, ref baseInt, ref AugmentInt, ref towerName);

        SpawnTowerForViewing(towerPosition, towerBase, towerHead);

        // delayed start and determine types pass in the enum for its TYPE and it initiallizes it specially. ** this is needed in base so it can display its stats.
        tower.GetComponentInChildren<Tower>().DelayedStart(baseInt, AugmentInt);
        tower.GetComponentInChildren<Tower>().DetermineTowerTypeBase(baseInt);
        tower.GetComponentInChildren<Tower>().DetermineTowerHeadType(AugmentInt);
        // it never goes in and fetches the type....
        // i am never seeing what kinda tower it is for the update to the words.

        tower.transform.localScale = new Vector3(.3f, .3f, .3f);

        TowerTypeDescription.text = tower.GetComponentInChildren<Tower>().GetTypeExplanation();
        TowerAugmentDescription.text = tower.GetComponentInChildren<Tower>().GetAugmentExplanation();
        TowerBaseDescription.text = tower.GetComponentInChildren<Tower>().GetBaseExplanation();
        TowerBaseFlavorTxt.text = tower.GetComponentInChildren<Tower>().GetBaseFlavorTxt();
        // maybe add this to the end of all the others? it will be called on any change.
        tower.GetComponentInChildren<Tower>().GetStringStats();
        TowerStatsTxt.text = tower.GetComponentInChildren<Tower>().GetTowerStatsExplanation();

    }

    // plug this into the vector 3 for position, instead of the defaulted 0,0,0
    public void SpawnTowerForViewing(Vector3 position, Tower towerBase, GameObject towerHead)
    {
        var container = new GameObject();
        container.name = towerBase.name;
        container.transform.position = position;

        //  FOR FUTURE  
        // maybe do a case statement, that returns a vector 3.  This fills in the instantiation place.
        // this can be done with the generic being the 'head' location.  This case, though, allows for more tower combinations. 
        // IE this case could supply 'back attachment' for lightening tower.  That or I could make them 1 part only, light towers are full peices only?


        // FOR NOW  I could go into the Tower Selecter and make that one function first.  I get info from there, so it needs to work first (also fastest to test.
        // I CAN hardcode stuff i dont have yet with this hack.  For slow and light tower, put it as an empty object.  It will get added, not throw an excepttion  AND be invisible and take low power.
        //print(towerBase.name);
        float headHeight = ((towerBase.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.extents.y) * .94f); //This is to account for bigger meshes    // + (obj2.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y));
        var tBase = Instantiate(towerBase, position, Quaternion.identity);
        tBase.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // use this for the placement
        var tHead = Instantiate(towerHead, (position + new Vector3(0, headHeight, 0)), Quaternion.identity); //new Vector3(0, headHeight, 0)
        tHead.transform.Rotate(0, 180f, 0);
        tBase.transform.parent = container.transform;
        tHead.transform.parent = tBase.transform;
        try
        {
            tHead.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        catch (NullReferenceException noTowerHead)
        {
            // nothing, not all towers have a head.
        }

        //not needed in base but w/e
        tBase.SetHead(tHead.transform);

        tower = container;
    }


    private void FocusDynamicTowerType(Dictionary<string, int> towerParts)
    {
        towerBase.ClearOptions();
        towerBarrel.ClearOptions();
        List<string> towerPartsList = new List<string>(towerParts.Keys);
        List<string> augments = new List<string>(); //{ "Basic Barrel", "Sniper Barrel" };
        List<string> bases = new List<string>(); //{ "Basic Base", "Rapid Base" };// we removed some of the supports inside the turret, it allows for easier bullet managment, but comes at the cost of resistance.  We increase rate of fire but lower impact.

        foreach(string s in towerPartsList)
        {
            if (s.ToLower().Contains("base"))
            {
                bases.Add(s);
            } else
            {
                augments.Add(s);
            }
        }

        towerBarrel.AddOptions(augments);
        towerBarrel.RefreshShownValue();
        towerBase.AddOptions(bases);
        towerBase.RefreshShownValue(); 
    }

    public Tower SetTowerBaseAndHead2()
    {
        if (towerLog == null)
        {
            towerLog = FindObjectOfType<PlayerTowerLog>();
        }
        knownTowerParts = new Dictionary<string, int>();
        List<Dropdown.OptionData> list = towerTurret.options;

        string tower = list[towerTurret.value].text;//towerTurret.options(towserTurret.value).text;
        knownTowerParts = towerLog.GetTowerParts(tower);
        FocusDynamicTowerType(knownTowerParts);

        return decidedTower;
    }

    public Tower SetTowerBaseAndHead2(string towerType)
    {
        if (towerLog == null)
        {
            towerLog = FindObjectOfType<PlayerTowerLog>();
        }

        int towerIndex = towerTurret.options.FindIndex(option => option.text == towerType);
        towerTurret.value = towerIndex;

        knownTowerParts = new Dictionary<string, int>();
        knownTowerParts = towerLog.GetTowerParts(towerType);
        FocusDynamicTowerType(knownTowerParts);

        return decidedTower;
    }

    public void TurnPanelInvisible(GameObject obj)
    {
        Color color = obj.GetComponent<Image>().color;
        color.a = 0f;
        obj.GetComponent<Image>().color = color;
    }

    public void TurnPanelVisible(GameObject obj)
    {
        Image i = obj.GetComponent<Image>();
        Color c = i.color;
        c.a = .25f;
        i.color = c;
    }


    public void LoadTowerOne()
    {
        if (singleton == null)
        {
            singleton = FindObjectOfType<Singleton>();
        }

        skipImageReset = true;

        TurnPanelVisible(TowerOnePanel);
        TurnPanelInvisible(Tower2Panel);
        TurnPanelInvisible(Tower3Panel);

        towerBarrel.value = 0;
        towerBase.value = 0;
        towerSlotSelected = 1;

        if (singleton.towerOneName.Equals(""))
        {
            skipImageReset = false;
            ResetTowerPicture();
            return;
        }

        changingTowerType = true;

        SetTowerBaseAndHead2(singleton.towerOneName);
        changingTowerType = false;

        towerBarrel.value = singleton.towerOneHeadType;
        towerBase.value = singleton.towerOneBaseType;

        skipImageReset = false;
        ResetTowerPicture();
    }
    public void LoadTowerTwo()
    {
        if (singleton == null)
        {
            singleton = FindObjectOfType<Singleton>();
        }

        skipImageReset = true;

        TurnPanelInvisible(TowerOnePanel);
        TurnPanelVisible(Tower2Panel);
        TurnPanelInvisible(Tower3Panel);

        towerBarrel.value = 0;
        towerBase.value = 0;
        towerSlotSelected = 2;

        if (singleton.towerTwoName.Equals(""))
        {
            skipImageReset = false;
            ResetTowerPicture();
            return;
        }

        changingTowerType = true;

        SetTowerBaseAndHead2(singleton.towerTwoName);
        changingTowerType = false;

        towerBarrel.value = singleton.towerTwoHeadType;
        towerBase.value = singleton.towerTwoBaseType;

        skipImageReset = false;
        ResetTowerPicture();
    }
    public void LoadTowerThree()
    {
        if (singleton == null)
        {
            singleton = FindObjectOfType<Singleton>();
        }

        skipImageReset = true;

        TurnPanelInvisible(TowerOnePanel);
        TurnPanelInvisible(Tower2Panel);
        TurnPanelVisible(Tower3Panel);

        towerBarrel.value = 0;
        towerBase.value = 0;
        towerSlotSelected = 3;

        if (singleton.towerThreeName.Equals(""))
        {
            skipImageReset = false;
            ResetTowerPicture();
            return;
        }

        changingTowerType = true;

        SetTowerBaseAndHead2(singleton.towerThreeName);
        changingTowerType = false;

        towerBarrel.value = singleton.towerThreeHeadType;
        towerBase.value = singleton.towerThreeBaseType;

        skipImageReset = false;
        ResetTowerPicture();
    }

    public void InitializeOptionsOnRoomSwap()
    {
        if (towerLog == null)
        {
            towerLog = FindObjectOfType<PlayerTowerLog>();
        }
        knownTowerParts = new Dictionary<string, int>();
        List<Dropdown.OptionData> list = towerTurret.options;

        string tower = list[towerTurret.value].text;//towerTurret.options(towserTurret.value).text;
        knownTowerParts = towerLog.GetTowerParts(tower);
        FocusDynamicTowerType(knownTowerParts);
    }

    // Need to change this so it is also if contains, then also pull the values out of the functions.
    // scrap what I said, just plug in the select, its gotten from the list so should match perfectly.  Then get the stuff inside that dictionary and populate and filters. 
    public Tower SetTowerBaseAndHead()
    {
        List<Dropdown.OptionData> list = towerTurret.options;

        string tower = list[towerTurret.value].text;//towerTurret.options(towerTurret.value).text;
        if (tower.Equals("RifledTower"))
        {
            FocusRifledTowers();
        }
        if (tower.Equals("AssaultTower"))
        {
            FocusAssaultTowers();
        }
        if (tower.Equals("FlameTower"))
        {
            FocusFireTowers();
        }
        if (tower.Equals("LighteningTower"))
        {
            FocusLighteningTowers();
        }
        if (tower.Equals("PlasmaTower"))
        {
            FocusPlasmaTowers();
        }
        if (tower.Equals("SlowTower"))
        {
            FocusSlowTowers();
        }

        return decidedTower;
    }

    /// <summary>
    /// Make a dictionary or something.  Needs a way to keep track of these and associate them with the value of their enums.  Need it to not be linked
    /// to the order in which they are added, that way if they unlock 'alien' and 'basic', alien is value 4, but position 2 on dropdown.
    /// </summary>

    private void FocusFireTowers()
    {
        towerBase.ClearOptions();
        towerBarrel.ClearOptions();
        List<string> fireBarrels = new List<string> { "Basic Barrel", "Flame Thrower" };
        List<string> fireBases = new List<string> { "Basic Base", "Tall Base", "Heavy Base", "Light Base", "Alien Base" };
        towerBarrel.AddOptions(fireBarrels);
        towerBarrel.RefreshShownValue();
        towerBase.AddOptions(fireBases);
        towerBase.RefreshShownValue();
    }

    private void FocusRifledTowers()
    {
        towerBase.ClearOptions();
        towerBarrel.ClearOptions();
        List<string> Barrels = new List<string> { "Basic Barrel" , "Sniper Barrel" };
        List<string> rifledBases = new List<string> { "Basic Base", "Rapid Base" };// we removed some of the supports inside the turret, it allows for easier bullet managment, but comes at the cost of resistance.  We increase rate of fire but lower impact.
        towerBarrel.AddOptions(Barrels);
        towerBarrel.RefreshShownValue();
        towerBase.AddOptions(rifledBases);
        towerBase.RefreshShownValue();
    }

    private void FocusAssaultTowers() 
    {
        towerBase.ClearOptions();
        towerBarrel.ClearOptions();
        List<string> Barrels = new List<string> { "Basic Barrel" };
        List<string> assaultBases = new List<string> { "Basic Base" };
        towerBarrel.AddOptions(Barrels);
        towerBarrel.RefreshShownValue();
        towerBase.AddOptions(assaultBases);
        towerBase.RefreshShownValue();
    }

    private void FocusLighteningTowers()
    {
        towerBase.ClearOptions();
        towerBarrel.ClearOptions();
        List<string> Barrels = new List<string> { "Basic Barrel" };
        List<string> lighteningBases = new List<string> { "Basic Base", "Rapid Base" };
        towerBarrel.AddOptions(Barrels);
        towerBarrel.RefreshShownValue();
        towerBase.AddOptions(lighteningBases);
        towerBase.RefreshShownValue();
    }

    private void FocusSlowTowers()
    {
        towerBase.ClearOptions();
        towerBarrel.ClearOptions();
        List<string> Barrels = new List<string> { "Basic Barrel" };
        List<string> slowBases = new List<string> { "Basic Base", "Industrial Base" };
        towerBarrel.AddOptions(Barrels);
        towerBarrel.RefreshShownValue();
        towerBase.AddOptions(slowBases);
        towerBase.RefreshShownValue();
        print("finished resetting the slow tower");
    }

    private void FocusPlasmaTowers()
    {
        towerBase.ClearOptions();
        towerBarrel.ClearOptions();
        List<string> Barrels = new List<string> { "Basic Barrel" };
        List<string> plasmaBases = new List<string> { "Basic Base" };
        towerBarrel.AddOptions(Barrels);
        towerBarrel.RefreshShownValue();
        towerBase.AddOptions(plasmaBases);
        towerBase.RefreshShownValue();
    }

    /// <summary>
    /// Since FindTower() in singleton is the one I need, I put the proxy here
    /// to cover the broken reference when towerroom is deleted on load.
    /// </summary>
    /// more precisely, The button references the new singleton which kills itself on spawning in, then this is a blnak reference.
    public void SaveTowerToSingleton()
    {
        singleton = Singleton.Instance;
        singleton.FindTower();
        UpdateSavedTowerSetups();
    }

    public void UpdateSavedTowerSetups()
    {
        switch (towerSlotSelected)
        {
            case 1:
                tower1Type.text = towerTurret.options[towerTurret.value].text;
                tower1Augment.text = towerBarrel.options[towerBarrel.value].text;
                tower1Base.text = towerBase.options[towerBase.value].text;
                break;
            case 2:
                tower2Type.text = towerTurret.options[towerTurret.value].text;
                tower2Augment.text = towerBarrel.options[towerBarrel.value].text;
                tower2Base.text = towerBase.options[towerBase.value].text;
                break;
            case 3:
                tower3Type.text = towerTurret.options[towerTurret.value].text;
                tower3Augment.text = towerBarrel.options[towerBarrel.value].text;
                tower3Base.text = towerBase.options[towerBase.value].text;
                break;
        }
    }

    public Tower PickTower(ref Tower turretBase, ref GameObject towerHead, ref int baseType, ref int towerBarrelType, ref string tower)
    {
        List<Dropdown.OptionData> list = towerTurret.options;
        tower = list[towerTurret.value].text;

        //if (towerLog == null)
        //{
            towerLog = FindObjectOfType<PlayerTowerLog>();
        //}

        if (changingTowerType)
        {
            baseType = 0; 
            towerBarrelType = 0;
            // I only enter it once per 'change'
            //SetTowerBaseAndHead2();
            changingTowerType = false;
        } else
        {
            knownTowerParts = towerLog.GetTowerParts(tower);
            // knownTowerParts is my list of parts for this given tower type., i pass in the name of the dropdown to access its enum int.
            baseType =  knownTowerParts[towerBase.options[towerBase.value].text];
            towerBarrelType = knownTowerParts[towerBarrel.options[towerBarrel.value].text];
        }


        //towerTurret.options(towerTurret.value).text;

        if (tower.Contains("Rifled"))
        {
            FocusRifledTowers(ref turretBase, ref towerHead, towerBarrelType, baseType);
        }
        //if (tower.Contains("AssaultTower"))
        //{
        //    FocusAssaultTowers(ref turretBase, ref towerHead, towerBarrelType, baseType);
        //}
        if (tower.Contains("Flame"))
        {
            FocusFireTowers(ref turretBase, ref towerHead, towerBarrelType, baseType);
        }
        if (tower.Contains("Light"))
        {
            FocusLighteningTowers(ref turretBase, ref towerHead, towerBarrelType, baseType);
        }
        if (tower.Contains("Plasma"))
        {
            FocusPlasmaTowers(ref turretBase, ref towerHead, towerBarrelType, baseType);
        }
        if (tower.Contains("Frost"))
        {
            FocusSlowTowers(ref turretBase, ref towerHead, towerBarrelType, baseType);
        }

        return decidedTower;
    }


    private void FocusFireTowers(ref Tower turretBase, ref GameObject turretHead, int barrelVal, int baseVal)
    {
        switch (barrelVal)
        {
            case (int)FlameHead.Basic:
                turretHead = basicFlameTowerHead;
                break;
            case (int)FlameHead.FlameThrower:
                turretHead = flameThrowerFlameTowerHead;
                break;
            case (int)FlameHead.Mortar:
                turretHead = mortarFlameTowerHead;
                break;
            default:
                print("Error with selecting fire Barrel, value is appearing as : " + towerBarrel.value);
                break;
        }

        switch (towerBase.value)
        {
            case (int)FlameBase.Basic:
                turretBase = basicFlameTowerBase;
                break;
            case (int)FlameBase.Tall:
                turretBase = tallFlameTowerBase;
                break;
            case (int)FlameBase.Heavy:
                turretBase = heavyFlameTowerBase;
                break;
            case (int)FlameBase.Light:
                turretBase = lightFlameTowerBase;
                break;
            case (int)FlameBase.Alien:
                turretBase = alienFlameTowerBase;
                break;
            default:
                print("Error with selecting fire Base, value is appearing as : " + towerBase.value);
                break;
        }
    }

    private void FocusRifledTowers(ref Tower turretBase, ref GameObject turretHead, int barrelVal, int baseVal)
    {
        switch (barrelVal)
        {
            case (int)RifledHead.Basic:
                turretHead = basicRifledTowerHead;
                break;
            case (int)RifledHead.Sniper:
                turretHead = sniperRifledTowerHead;
                break;
            default:
                turretHead = basicRifledTowerHead;

                print("Error with selecting Barrel, value is appearing as : " + towerBarrel.value);
                break;
        }

        switch (baseVal)
        {
            case (int)RifledBase.Basic:
                turretBase = basicRifledTowerBase;
                break;
            case (int)RifledBase.Rapid:
                turretBase = rapidRifledTowerBase;
                break;
            default:
                print("Error with selecting  Base, value is appearing as : " + towerBase.value);
                break;
        }
    }

    private void FocusAssaultTowers(ref Tower turretBase, ref GameObject turretHead, int barrelVal, int baseVal)
    {
        switch (barrelVal)
        {
            case (int)RifledHead.Basic:
                turretHead = basicRifledTowerHead;
                break;
            default:
                print("Error with selecting fire Barrel, value is appearing as : " + towerBarrel.value);
                break;
        }

        switch (baseVal)
        {
            case (int)RifledBase.Basic:
                turretBase = basicRifledTowerBase;
                break;
            default:
                print("Error with selecting fire Base, value is appearing as : " + towerBase.value);
                break;
        }
    }

    private void FocusLighteningTowers(ref Tower turretBase, ref GameObject turretHead, int barrelVal, int baseVal)
    {
        switch (barrelVal)
        {
            case (int)LightningHead.Basic:
                turretHead = empty;
                break;
            default:
                print("Error with selecting  Barrel, value is appearing as : " + towerBarrel.value);
                break;
        }

        switch (baseVal)
        {
            case (int)LightningBase.Basic:
                turretBase = basicLightTowerBase;
                break;
            case (int)LightningBase.Rapid:
                turretBase = rapidLightTowerBase;
                break;
            default:
                print("Error with selecting  Base, value is appearing as : " + towerBase.value);
                break;
        }
    }

    private void FocusSlowTowers(ref Tower turretBase, ref GameObject turretHead, int barrelVal, int baseVal)
    {
        switch (barrelVal)
        {
            case (int)IceHead.Basic:
                turretHead = basicIceTowerHead;
                break;
            default:
                print("Error with selecting  Barrel, value is appearing as : " + towerBarrel.value);
                break;
        }

        switch (baseVal)
        {
            case (int)IceBase.Basic:
                turretBase = basicIceTowerBase;
                break;
            case (int)IceBase.Industrial:
                turretBase = industrialIceTowerBase;
                break;
            default:
                print("Error with selecting  Base, value is appearing as : " + towerBase.value);
                break;
        }
    }

    private void FocusPlasmaTowers(ref Tower turretBase, ref GameObject turretHead, int barrelVal, int baseVal)
    {
        switch (barrelVal)
        {
            case (int)PlasmaHead.Basic:
                turretHead = basicPlasmaTowerHead;
                break;
            case (int)PlasmaHead.Crystal:
                turretHead = crystalPlasmaTowerHead;
                break;
            default:
                print("Error with selecting  Barrel, value is appearing as : " + towerBarrel.value);
                break;
        }

        switch (baseVal)
        {
            case (int)PlasmaBase.Basic:
                turretBase = basicPlasmaTowerBase;
                break;
            default:
                print("Error with selecting  Base, value is appearing as : " + towerBase.value);
                break;
        }
    }


    //public void UpdateTowerType()
    //{
    //    TowerTypeDescription.text = FindObjectOfType<Tower>().GetTypeExplanation();
    //}

    //public void UpdateTowerAugment()
    //{
    //    TowerAugmentDescription.text = FindObjectOfType<Tower>().GetAugmentExplanation();
    //}

    //public void UpdateTowerBase()
    //{
    //    TowerBaseDescription.text = FindObjectOfType<Tower>().GetBaseExplanation();
    //}
}