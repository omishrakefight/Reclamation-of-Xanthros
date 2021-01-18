using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerLog : MonoBehaviour {
    /// <summary>
    /// MAYBE MAKE THIS INTO A DICTIONARY OR LIST ANYWAYS.
    /// SAVE IT AS AN ARRAY (FINE) BUT USE IT AS A DYNAMIC CONTAINER FOR ACTUAL GAME USE.
    /// MAKES IT EASIER TO ADD AND REMOVE.
    /// </summary>
    //Dictionary<string, bool> towers;
    public bool[] towers1 = null;
    bool startNew = true;

    //knownTowersAndParts = _knownTowersAndParts;
    //    learnableTowersAndParts = _learnableTowersAndParts;
    //    unlearnableTowersAndParts

    public Dictionary<string, int> towerParts;
    public Dictionary<string, Dictionary<string, int>> knownTowersAndParts;
    public Dictionary<string, Dictionary<string, int>> learnableTowersAndParts;
    public Dictionary<string, Dictionary<string, int>> unlearnableTowersAndParts;
    //int numberOfTowers = 6;

    //basic starting tower
    //bool hasRifled = true;

    //bool hasAssaultTower = false;
    //bool hasFlameTurret = false;
    //bool hasLighteningTower = false;
    //bool hasPlasmaTurret = false;
    //bool hasSlowTurret = false;


    // Use this for initialization
    void Start()
    {
        if (startNew) {
            towers1 = new bool[]
            {
            true,  // Rifled Tower
            false, // Assault Tower
            false, // Flame Tower
            false, // Lightening Tower
            false, // Plasma Tower
            false  // Slow Tower
            };

            // Rifled tower is basic tower to start with.
            knownTowersAndParts = new Dictionary<string, Dictionary<string, int>>();

            towerParts = new Dictionary<string, int>() {
                { "Basic Augment", (int)RifledHead.Basic },
                { "Sniper Augment", (int)RifledHead.Sniper },
                { "Basic Base", (int)RifledBase.Basic },
                { "Rapid Base", (int)RifledBase.Rapid }
            };

            knownTowersAndParts.Add("Rifled Towers", towerParts);

            towerParts = new Dictionary<string, int>() {
                { "Basic Augment", (int)FlameHead.Basic },
                { "FlameThrower Augment", (int)FlameHead.FlameThrower },
                { "Mortar Augment", (int)FlameHead.Mortar },
                { "Basic Base", (int)FlameBase.Basic },
                { "Tall Base", (int)FlameBase.Tall }
            };
            knownTowersAndParts.Add("Flame Towers", towerParts);




            //  Towers we need to learn, all besides rifled.
            learnableTowersAndParts = new Dictionary<string, Dictionary<string, int>>();

            towerParts = new Dictionary<string, int>() {
                { "Basic Augment", (int)FlameHead.Basic },
                { "FlameThrower Augment", (int)FlameHead.FlameThrower },
                { "Basic Base", (int)FlameBase.Basic },
                { "Tall Base", (int)FlameBase.Tall }
            };
            learnableTowersAndParts.Add("Flame Towers", towerParts);

            towerParts = new Dictionary<string, int>() {
                { "Basic Augment", (int)IceHead.Basic }, 
                //{ "Industrial Augment", (int)FlameHead.FlameThrower },
                { "Basic Base", (int)IceBase.Basic },
                { "Industrial Base", (int)IceBase.Industrial }
            };
            learnableTowersAndParts.Add("Frost Tower", towerParts);

            /////=================continue here
            towerParts = new Dictionary<string, int>() {
                { "Basic Augment", (int)LightningHead.Basic },
                //{ "Industrial Augment", (int)FlameHead.FlameThrower },
                { "Basic Base", (int)LightningBase.Basic },
                { "Rapid Base", (int)LightningBase.Rapid }
            };
            learnableTowersAndParts.Add("Lightning Tower", towerParts);

            towerParts = new Dictionary<string, int>() {
                { "Basic Augment", (int)PlasmaHead.Basic },
                { "Crystal Augment", (int)PlasmaHead.Crystal },
                //{ "Industrial Augment", (int)FlameHead.FlameThrower },
                { "Basic Base", (int)PlasmaBase.Basic }
                //{ "Rapid Base", (int)PlasmaBase.Basic }
            };
            learnableTowersAndParts.Add("Plasma Tower", towerParts);

        }



        //print("loaded " + towers1.Length + " towers");

        //GetComponent<RandomTowerBlueprints>().ManualStart();
        GetComponent<RandomTowerBlueprints>().ManualStart2(knownTowersAndParts, learnableTowersAndParts);

        //gana pull this from saved file hopefully.
        //towers.Add("hasRifled", true);
        //towers.Add("hasFlameTower", false);
        //towers.Add("hasLighteningTower", false);
        //towers.Add("hasPlasmaTower", false);
        //towers.Add("hasSlowTower", false);
        //towers.Add("hasAssaultTower", false);

    }

    public Dictionary<string, int> GetTowerParts(string towerKey) //Dictionary<string, int> GetTowerParts(string towerKey)
    {
        return knownTowersAndParts[towerKey]; //Dictionary<string, int> towerParts = 
    }

    public void GetKnownAndLearnableTowerRef(ref Dictionary<string, Dictionary<string, int>> knownTowerTypesRef, ref Dictionary<string, Dictionary<string, int>> learnableTowerTypesRef)
    {
        knownTowerTypesRef = knownTowersAndParts;
        learnableTowerTypesRef = learnableTowersAndParts;
    }

    //I do it in seperate parts here so that I can call the functions as parameters, the Save object will hodl reference, but not the Load / Save script.
    public Dictionary<string, Dictionary<string, int>> SaveKnownTowersAndParts()
    {
        return knownTowersAndParts;
    }
    public Dictionary<string, Dictionary<string, int>> SaveLearnableTowersAndParts()
    {
        return learnableTowersAndParts;
    }
    public Dictionary<string, Dictionary<string, int>> SaveUnlearnableTowersAndParts()
    {
        return unlearnableTowersAndParts;
    }

    //public void SaveTowersAndParts(ref Dictionary<string, Dictionary<string, int>> knownTowerTypesRef, ref Dictionary<string, Dictionary<string, int>> learnableTowerTypesRef, ref Dictionary<string, Dictionary<string, int>> unlearnableTowerTypesRef)
    //{
    //    knownTowerTypesRef = knownTowersAndParts;
    //    learnableTowerTypesRef = learnableTowersAndParts;
    //    unlearnableTowerTypesRef = unlearnableTowersAndParts;
    //}

    public void GetTowersFromGame()
    {
        // pull them in instead of just having the dumb bools above.
    }

    public bool[] SaveTowers()
    {
        return towers1;
    }


    public void LoadTowersAndParts(Dictionary<string, Dictionary<string, int>> _knownTowersAndParts, Dictionary<string, Dictionary<string, int>> _learnableTowersAndParts, Dictionary<string, Dictionary<string, int>> _unlearnableTowersAndParts)
    {
        knownTowersAndParts = _knownTowersAndParts;
        learnableTowersAndParts = _learnableTowersAndParts;
        unlearnableTowersAndParts = _unlearnableTowersAndParts;

        GetComponent<RandomTowerBlueprints>().ManualStart2(knownTowersAndParts, learnableTowersAndParts);
        startNew = false;
    }

    public void LoadTowers(bool[] loadedTowers)
    {
        this.towers1 = loadedTowers;

        GetComponent<RandomTowerBlueprints>().ManualStart();
        startNew = false;
    }

    //public Dictionary<string, bool> SaveTowers()
    //{
    //    return towers;
    //}

    //public void LoadTowers(Dictionary<string, bool> loadedTowers)
    //{
    //    this.towers = loadedTowers;
    //}

    // Update is called once per frame
    void Update()
    {

    }


}

// TODO KYLE check if these are global and free
//public enum Towers
//{
//    RifledTower = 0,
//    AssaultTower = 1,
//    FlameTower = 2,
//    LighteningTower = 3,
//    PlasmaTower = 4,
//    SlowTower = 5

//}