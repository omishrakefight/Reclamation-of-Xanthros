using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Tower : MonoBehaviour {

    public int goldCost = 60;
    protected GoldManagement gold;

    // paramteres of each tower
    [SerializeField] public float attackRange = 9f;
    protected float currentAttackRange = 0;

    protected int towerHeadType = -1;

    protected string TowerTypeExplanation = "";
    protected string TowerAugmentExplanation = "";
    protected string TowerBaseExplanation = "";
    protected string TowerBaseFlavorTxt = "";
    protected string TowerStatsTxt = "";
    protected string TowerTypeName = "";

    protected string towerUpgradeDescriptionOne = "";
    protected string towerUpgradeDescriptionTwo = "";
    protected string towerUpgradeDescriptionThree = "";

    protected int upgradeOneUsed = 0;
    protected int upgradeTwoUsed = 0;
    protected int upgradeThreeUsed = 0;
    protected int anyUpgradeUsed = 0;
    protected float anyUpgradeCostInc = .06f;
    protected float thisUpgradeCostInc = .04f;
    protected float baseUpgradePercent = .22f;
    protected float costReductionForBallisticRange = .66f;
    protected float engineeringCostReduction = 1f;

    [SerializeField] public float towerDmg = 30;
    [SerializeField] protected float currentTowerDmg = 30;

    public List<EnemyHealth> sceneEnemies;
    [SerializeField] public Transform targetEnemy;
    [SerializeField] protected Transform objectToPan;
    public EnemyHealth targetEnemyBody;
    public EnemyHealth preferedEnemyBody = null;

    // for tinker upgrades
    public bool cantargettingModule = false;
    public bool canAlloyReasearch = false;
    public bool canSturdyTank = false;
    public bool canHeavyShelling = false;
    public bool canTowerEngineer = false;

    protected bool testingTowerUpgrades = true;

    // Use this for initialization
    // Buff info
    public bool keepBuffed = false;

    protected virtual void Start()
    {
        try
        {
            // combat initializations.  If tower is spawned in base these might catch.
            sceneEnemies = EnemySpawner.EnemyAliveList;
        }
        catch (Exception e)
        {
            print(e.Message + ":  Error is initialization object name: " + gameObject.name);
        }
        //preferedEnemyBody = FindObjectOfType<Singleton>().preferedTargetEnemy;
    }

    public void TowerBuff()
    {
        keepBuffed = true;
    }

    public float GetAttackRange()
    {
        return currentAttackRange;
    }

    public void CheckAndApplyBuff()
    {
        //currentAttackRange = attackRange;
        // nothing if it is unbuffed 
        if (!keepBuffed) { }
        else
        {
            print(towerDmg + " = towerdmg, current = " + currentTowerDmg);
            attackRange += (currentAttackRange * .15f);
            towerDmg += (currentTowerDmg * .15f);
            print("towerDMG after 2nd buff " + towerDmg);
        }
    }

    public void TowerUpgrade()
    {
        // attackRange = attackRange * (1.0 + .2 * timesBuffed)

        //attackRange = attackRange * 1.2f;

        if (keepBuffed)
        {
            TowerBuff();
        }
    }

    // returns how much dmg this tower does.
    public float Damage(ref string towerName)
    {
        towerName = TowerTypeName;
        return currentTowerDmg;
    }


    public void SetTargetEnemy()
    {
        bool getNext = false;
        Transform closestEnemy;
        //var sceneEnemies = FindObjectsOfType<EnemyHealth>();
        if (sceneEnemies.Count == 0) { return; }

        try
        {
            closestEnemy = sceneEnemies[0].transform; // change default?
        }
        catch (Exception e)
        {
            return; // if it bugs out selecting target, lost 1 frame and re-try next.
        }
        
        // This meanas grab next in comparisons
        if (!sceneEnemies[0].isTargetable)
        {
            getNext = true;
        }

        try
        {
            foreach (EnemyHealth testEnemy in sceneEnemies)
            {
                if (testEnemy.isTargetable) // checking for health script enabled
                {
                    if (getNext)
                    {
                        closestEnemy = testEnemy.transform;
                        getNext = false;
                    }

                    closestEnemy = GetClosest(closestEnemy, testEnemy.transform);
                } else
                {
                    int x = 0;
                }
            }

            targetEnemy = closestEnemy;
            targetEnemyBody = targetEnemy.GetComponentInChildren<EnemyHealth>();
        }
        catch (Exception e)
        {
            print("Failed target acquisition try again: whomp whomp");
        }

    }

    protected virtual Transform GetClosest(Transform transformA, Transform transformB)
    {
        var distanceToA = Vector3.Distance(transform.position, transformA.position);
        var distanceToB = Vector3.Distance(transform.position, transformB.position);

        if (distanceToA <= distanceToB)
        {
            return transformA;
        }
        else
        {
            return transformB;
        }
    }

    public virtual float GetTowerCost()
    {
        return 1000f;
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void CheckUpgradesForTankTower(ref float towerDmg, ref float TankAOERange, ref float upgradeReduction)
    {
        Singleton singleton = Singleton.Instance;
        float percentModifier = 1.0f;
        float multiplyFodder = 1.0f;
        float amountToAdd = 0f;

        float baseTowerDmg = towerDmg;
        float baseTankAOERange = TankAOERange;

        percentModifier = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.pressurizedTank);

        // Pressurized tank is 75% dmg 25% range.  AOE towers get more AOE than dmg.
        percentModifier = multiplyFodder - percentModifier;
        amountToAdd = ((percentModifier * baseTowerDmg) * .75f);
        towerDmg += amountToAdd;

        amountToAdd = ((percentModifier * baseTankAOERange) * .25f);
        TankAOERange += amountToAdd;

        percentModifier = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.targettingModule);
        percentModifier = multiplyFodder - percentModifier;
        amountToAdd = (percentModifier * baseTankAOERange);
        TankAOERange += amountToAdd;

        upgradeReduction = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.towerEngineer);
    }

    public void CheckUpgradesForRifledTower(ref float towerDmg, ref float towerRange, ref float upgradeReduction)
    {
        Singleton singleton = Singleton.Instance;
        float percentModifier = 1.0f;
        float multiplyFodder = 1.0f;
        float amountToAdd = 0f;

        float baseTowerDmg = towerDmg;
        float baseTowerRange = towerRange;

        percentModifier = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.heavyShelling);

        // Heavy shelling is more dmg oriented and gets full value.
        percentModifier = multiplyFodder - percentModifier;
        amountToAdd = (percentModifier * baseTowerDmg);
        towerDmg += amountToAdd;

        percentModifier = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.targettingModule);
        percentModifier = multiplyFodder - percentModifier;
        amountToAdd = (percentModifier * baseTowerRange);
        towerRange += amountToAdd;

        upgradeReduction = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.towerEngineer);
    }

    // split this into 2, one for rifled towers and one for tank towers.
    // increase this, tanks increase dmg, shelling increases dmg for bullets, and silver wiring can do range?
    public void CheckWhichUpgradesAreApplicable(ref float towerDmg, ref float TankAOERange)
    {
        Singleton singleton = Singleton.Instance;
        float percentModifier = 1.0f;
        float multiplyFodder = 1.0f;
        float amountToAdd = 0f;
        //print("Checking if tower can be upgraded. . .");
        //if (canHeavyShelling)
        //{
            percentModifier = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.heavyShelling);
            //since most are a reduction and this is a dmg buff, i mius from 2 and multiply by difference.
             multiplyFodder = 1.0f;
            percentModifier = multiplyFodder - percentModifier;
        //print("percentModifier = " + percentModifier + ", and percent * tower dmg = " + (percentModifier * towerDmg));
            amountToAdd = (percentModifier * towerDmg);
            towerDmg += amountToAdd;
        //}
        //if (canSturdyTank)
        //{
            //print("Can heavy shelling, calculating");
            percentModifier = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.pressurizedTank);
            //since most are a reduction and this is a  buff, i mius from 2 and multiply by difference.
             multiplyFodder = 1.0f;
            percentModifier = multiplyFodder - percentModifier;
            amountToAdd = (percentModifier * TankAOERange);
            //do overlap sphere and range is the diameter?  then attack range could work easily.
            TankAOERange += amountToAdd;
        //}
        //throw new NotImplementedException();
    }

    // Maybe also include ref goldcost? so buttons can be inactive unless you have enough gold?
    public virtual void InitializeUpgradeOptionTexts(ref string description1, ref string description2, ref string description3, ref string stats)
    {
        description1 = towerUpgradeDescriptionOne;
        description2 = towerUpgradeDescriptionTwo;
        description3 = towerUpgradeDescriptionThree;

        GetStringStats();
        stats = TowerStatsTxt;
    }

    public virtual void UpgradeBtnOne(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        print("Not Overriden in the base tower.");
        GetStringStats();
        stats = TowerStatsTxt;
    }
    public virtual void UpgradeBtnTwo(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)//, ref string upgradeTextTwo)
    {
        print("Not Overriden in the base tower.");
        GetStringStats();
        stats = TowerStatsTxt;
    }
    public virtual void UpgradeBtnThree(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)//, ref string upgradeTextThree)
    {
        print("Not Overriden in the base tower.");
        GetStringStats();
        stats = TowerStatsTxt;
    }

    public virtual void GetTowerUpgradeTexts(int headType)
    {
        print("Not implemented yet");
        throw new NotImplementedException();
    }

    public virtual void GetStringStats()
    {
        // this is overriden in each tower.
        TowerStatsTxt = "";
    }

    public void SetNewTowerDmg(float newDamage)
    {
        currentTowerDmg = newDamage;
        towerDmg = newDamage;
    }


    public bool CanPurchaseUpgrade(int currentUpgradeCost)
    {
        if (gold.upgradeCount < currentUpgradeCost)
        {
            print("Shouldnt allow, not enough parts!!! " + gold.upgradeCount + " < " + currentUpgradeCost);
            if (!testingTowerUpgrades)
            {
                return false;
            }
        }
        return true;
    }


    public string GetTowerStatsExplanation()
    {
        return TowerStatsTxt;
    }
    public string GetTypeExplanation()
    {
        return TowerTypeExplanation;
    }
    public string GetAugmentExplanation()
    {
        return TowerAugmentExplanation;
    }
    public string GetBaseExplanation()
    {
        print("I am in the tower. name: " + gameObject.name);
        return TowerBaseExplanation;
    }
    public string GetBaseFlavorTxt()
    {
        return TowerBaseFlavorTxt;
    }

    public void SetHead(Transform towerHead)
    {
        objectToPan = towerHead;
    }

    public virtual void DetermineTowerTypeBase(int towerInt)
    {
        // Just a basic overrriden function, same method signature though so I can work off 'Tower' script type.
    }
    public virtual void DetermineTowerHeadType(int towerInt)
    {
    }
    public virtual void DelayedStart(int baseType, int modificationType)
    {
        gold = FindObjectOfType<GoldManagement>();
    }
}
