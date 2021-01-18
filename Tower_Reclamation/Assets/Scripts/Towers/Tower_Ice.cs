using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Ice : Tower {

    [SerializeField] Light blueLight;
    public float range;
    protected float chillDuration = .5f;
    protected float chillDurationIncrease = .75f;
    protected float preFlippedChillAmount = 0f;
    protected float slowUpgrade = .05f; // 5f easy for fun
    protected float chillAmount = 0f;
    Singleton singleton;
    // Use this for initialization

    readonly new bool cantargettingModule = true;
    readonly new bool canAlloyReasearch = true;
    readonly new bool canSturdyTank = true;
    readonly new bool canHeavyShelling = false;
    readonly new bool canTowerEngineer = true;

    protected float notAProjectileTurret = 0f;

    private int baseType = -1;
    private int modificationType = -1;

    override protected void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // maybe hard code it? try wihtout the 1/2
        ChillAura(this.transform.position, range);
        range = blueLight.range;
    }

    public override void DelayedStart(int _baseType, int _modificationType)
    {
        base.DelayedStart(_baseType, _modificationType);

        baseType = _baseType;
        modificationType = _modificationType;

        TowerTypeExplanation = "The Ice Tower works off the research of Jack Kvate, proving that the Xenos were cold blooded. " +
                    "The base holds compartmentalized fluids, while the propellers at the top spin to both mix and disperse the slush, chilling " +
                    "the immediate area.  This should slow the Xeno that travel too close.";

        preFlippedChillAmount = .33f;
        blueLight.range = 16;
        range = blueLight.range;
        goldCost = (int)TowerCosts.SlowTowerCost;
        base.CheckUpgradesForTankTower(ref chillAmount, ref range, ref engineeringCostReduction);

        chillAmount = 1f - preFlippedChillAmount;
        currentAttackRange = range;
    }

    public override void GetTowerUpgradeTexts(int headType)
    {
        towerUpgradeDescriptionOne = "Upgrade tower Slow percent +5%";
        towerUpgradeDescriptionTwo = "Upgrade tower lingering slow duration 1 second";
        towerUpgradeDescriptionThree = "Upgrade tower AOE +15%";

        switch (headType) // add this in at DetermineTowerHeadType
        {
            case -1:
                print("Didn't initialize the variable towerHeadType");
                break;
            default:
                break;
        }
    }


    void ChillAura(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponentInParent<EnemyHealth>())
            {
                //hitColliders[i].SendMessage("AddDamage");
                hitColliders[i].gameObject.GetComponentInParent<EnemyMovement>().gotChilled(chillAmount, chillDuration);
            }
            i++;
        }
    }

    //TODO -- Maybe have beam towers do 50% effect every second ramps up to 3 (150%)
    //This is shared 50% around the main target (while focusing beam).  Stacks fall off 1 sec at a time.
    public override void DetermineTowerTypeBase(int towerInt)
    {
        switch (towerInt)
        {
            case (int)IceBase.Basic:
                //nothing, normal settings?
                TowerBaseExplanation = "Basic base.";
                break;
            case (int)IceBase.Industrial:
                TowerBaseExplanation = "Industrial base.";
                //TowerBaseExplanation = "Slow amount Speed = -" + (Mathf.RoundToInt((1 - speedDecimalModifier) * 100)).ToString() + "% \n";
                //TowerBaseExplanation += "Trigger Range = " + (AOERange.radius).ToString() + " \n";
                //TowerBaseExplanation += "Damage Range = -" + (Mathf.RoundToInt((1 - attackRangeDeimalModifier) * 100)).ToString() + "% \n";
                //TowerBaseExplanation += "Damage = -" + (Mathf.RoundToInt((1 - damageDeimalModifier) * 100)).ToString() + "% \n";
                break;
            default:
                print("Default base, I am towerint of : " + towerInt);
                //nothing
                break;
        }
    }

    public override void DetermineTowerHeadType(int towerInt)
    {
        towerHeadType = towerInt;
        GetTowerUpgradeTexts(towerInt);

        switch (towerInt)
        {
            case (int)IceHead.Basic:
                TowerAugmentExplanation = "The default tower blades, with no modifiers.  Mixes and sprays the chemicals in a decent area";
                //nothing;
                break;
            //case (int)LightningHead.Basic:
            //    attackAreaType = "Long";
            //    TowerAugmentExplanation = "The flamethrower head, changes the attack area.  This version turns it, making it a long cone rather than wide cone.";

            //    head.ChangeParticleTime(1.5f);
            //    attackRange = head.SetTowerTypeFlameThrower();
            //    break;
            default:
                TowerAugmentExplanation = "The default tower blades, with no modifiers.  Mixes and sprays the chemicals in a decent area";
                break;
        }
    }

    public override void GetStringStats()
    {
        TowerStatsTxt = "Ice Tower Stats \n" +
            "Area Range = " + range + "\n" +
            "Attack Damage = 0 \n" +
            "Attack Speed = This Tower has a constant effect \n" +
            "Slow Amount = " + preFlippedChillAmount.ToString() + "% \n" +
            "Targetting = AOE centered on tower.";
    }


    public override float GetTowerCost()
    {
        float towerCost = 0;
        singleton = Singleton.Instance;

        towerCost = (int)TowerCosts.SlowTowerCost;

        float percentToPay = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.alloyResearch);

        towerCost = towerCost * percentToPay;

        return towerCost;
    }

    new public void SetHead(Transform towerHead)
    {
        //Do nothing, this tower doesnt have a swivelHead so doesnt matter
    }


    //towerUpgradeDescriptionOne = "Upgrade tower Slow percent +5%";
    //towerUpgradeDescriptionTwo = "Upgrade tower lingering slow duration 1 second";
    //towerUpgradeDescriptionThree = "Upgrade tower AOE +15%";

    public override void InitializeUpgradeOptionTexts(ref string description1, ref string description2, ref string description3, ref string stats)
    {
        description1 = towerUpgradeDescriptionOne;
        description2 = towerUpgradeDescriptionTwo;
        description3 = towerUpgradeDescriptionThree;

        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out description1, out description2, out description3);
    }
    private void GetUpgradeCosts(out string upgradeTextOne, out string upgradeTextTwo, out string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        // this shenanigins, multiplies the base cost of the tower times .03x the times its EVER been upgraded, + .05 times the specific upgrade, + the .20% starting upgrade base.
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeOneUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
        string newExplanation = towerUpgradeDescriptionOne + "\nCost: " + currentUpgradeCost;
        upgradeTextOne = newExplanation;

        currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeTwoUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
        newExplanation = towerUpgradeDescriptionTwo + "\nCost: " + currentUpgradeCost;
        upgradeTextTwo = newExplanation;

        currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
        newExplanation = towerUpgradeDescriptionThree + "\nCost: " + currentUpgradeCost;
        upgradeTextThree = newExplanation;
    }


    public override void UpgradeBtnOne(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeOneUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
        stats = TowerStatsTxt;

        if (!CanPurchaseUpgrade(currentUpgradeCost))
        {
            return;
        }
        //if (gold.upgradeCount < currentUpgradeCost || preFlippedChillAmount >= .75f) // cant add more than 
        //{
        //    print("Shouldnt allow, not enough parts!!! " + gold.upgradeCount + " < " + currentUpgradeCost + "  |||| or chilled for more than .75%");
        //    //return;   Eventually this will stop it.
        //}

        preFlippedChillAmount += slowUpgrade;
        chillAmount = (1 - preFlippedChillAmount);

        gold.UpgradeCost(currentUpgradeCost);
        upgradeOneUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }
    public override void UpgradeBtnTwo(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeTwoUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
        stats = TowerStatsTxt;

        if (!CanPurchaseUpgrade(currentUpgradeCost))
        {
            return;
        }

        chillDuration += chillDurationIncrease;


        gold.UpgradeCost(currentUpgradeCost);
        upgradeTwoUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }
    public override void UpgradeBtnThree(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
        stats = TowerStatsTxt;

        if (!CanPurchaseUpgrade(currentUpgradeCost))
        {
            return;
        }

        blueLight.range += (.15f * blueLight.range);
        range += (.15f * range);
        currentAttackRange = blueLight.range;

        gold.UpgradeCost(currentUpgradeCost);
        upgradeThreeUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }

    //towerUpgradeDescriptionOne = "Upgrade tower Slow percent +5%";
    //towerUpgradeDescriptionTwo = "Upgrade tower lingering slow duration 1 second";
    //towerUpgradeDescriptionThree = "Upgrade tower AOE +15%";
    //public override void UpgradeBtnOne(ref string stats)
    //{
    //    currentTowerDmg += (.2f * towerDmg);
    //    GetStringStats();
    //    stats = TowerStatsTxt;
    //}
    //public override void UpgradeBtnTwo(ref string stats)
    //{
    //    chargeTime = (.8f * chargeTime);
    //    GetStringStats();
    //    stats = TowerStatsTxt;
    //}
    //public override void UpgradeBtnThree(ref string stats)
    //{
    //    currentAttackRange += (.15f * attackRange);

    //    GetStringStats();
    //    stats = TowerStatsTxt;
    //}
}
