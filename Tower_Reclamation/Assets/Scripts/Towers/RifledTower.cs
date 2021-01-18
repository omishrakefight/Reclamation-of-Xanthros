using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifledTower : Tower {

    // paramteres of each tower

    //[SerializeField] float attackRange = 32f;
    //public new int goldCost = 50;
    ParticleSystem projectileParticle = null;
    ParticleSystem.EmissionModule emission;
    Singleton singleton;

    [SerializeField] RifledBullet bullet;
    private float attackSpeed = 1.0f;
    private float attackTimer = 0f;

    //[SerializeField] float towerDmg = 12;
    //[SerializeField] private float currentTowerDmg = 12;
    // State of tower

       //TODO 3* tower upgrades, 5* this upgrarade...
    // Buff info
    //bool keepBuffed = false;
    readonly new bool cantargettingModule = true;
    readonly new bool canAlloyReasearch = true;
    readonly new bool canSturdyTank = false;
    readonly new bool canHeavyShelling = true;
    readonly new bool canTowerEngineer = true;

    protected float notATankTower = 0f;
    protected float minRange = 0f;
    float towerMinRangeModifier = .33f;

    private int baseType = -1;
    private int modificationType = -1;

    // minrange = 0, set it if sniper?
    override protected void Start () {
        base.Start();
        goldCost = (int)TowerCosts.RifledTowerCost;

        TowerTypeName = "Rifled Tower";
    }

    override public void DelayedStart(int _baseType, int _modificationType)
    {
        base.DelayedStart(_baseType, _modificationType);

        baseType = _baseType;
        modificationType = _modificationType;

        TowerTypeExplanation = "The rifled tower is a basic tower that fires bullets at a single enemy.  ";
        TowerTypeExplanation += "The damage it causes is mediocre and has no splash, but it has decent range and rate of fire.  ";// 
        TowerTypeExplanation += "This basic tower gets the job done, so long as it is not firing on swarms of enemies.  ";

        minRange = 0f;
        towerDmg = 30f;
        attackRange = 20f;
        currentTowerDmg = 15f;
        currentAttackRange = attackRange;
        base.CheckUpgradesForRifledTower(ref towerDmg, ref attackRange, ref engineeringCostReduction);
        CheckAndApplyBuff();
        currentTowerDmg = towerDmg;
        currentAttackRange = attackRange;
        projectileParticle = GetComponentInChildren<ParticleSystem>();
        emission = projectileParticle.emission;

        //GetTowerUpgradeTexts();
    }

    public override void DetermineTowerTypeBase(int towerInt)
    {
        float towerDmgModifierPercent;
        float towerAttackRangeModifierPercent;
        float towerAttackSpeedModifierPercent;

        switch (towerInt)
        {
            case (int)RifledBase.Basic:
                TowerBaseExplanation = "The default base, with no modifiers.";
                //nothing, normal settings?
                break;
            case (int)RifledBase.Rapid:
                // alien base is +10%?
                towerDmgModifierPercent = .15f;
                towerAttackRangeModifierPercent = .05f;
                towerAttackSpeedModifierPercent = .3f;
                TowerBaseExplanation = "Tower damage -" + (int)(towerDmgModifierPercent * 100f) + '%';
                TowerBaseExplanation += "\nTower attack range -" + (int)(towerAttackRangeModifierPercent * 100f) + '%';
                TowerBaseExplanation += "\nTower attack speed +" + (int)(towerAttackSpeedModifierPercent * 100f) + '%';

                TowerBaseFlavorTxt = "The rapid base has better bullet feed for faster firing, however, " +
                    "the bullets needed to be smaller to keep the tower from breaking down.  This necessitates a weaker hit and shorter range.";

                float changeAmount = 0;
                changeAmount = towerDmg * towerDmgModifierPercent;
                currentTowerDmg -= changeAmount;

                changeAmount = attackRange * towerAttackRangeModifierPercent;
                currentAttackRange -= changeAmount;
                attackRange = currentAttackRange;

                //emission.rateOverTime = (emission.rateOverTime.constant * (1.0f + towerAttackSpeedModifierPercent));
                attackSpeed = attackSpeed * (1 - towerAttackSpeedModifierPercent);
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

        float towerDmgModifierPercent;
        float towerAttackRangeModifierPercent;
        float towerAttackSpeedModifierPercent;

        // base emissio nrate of 1 second
        switch (towerInt)
        {
            case (int)RifledHead.Basic:
                //towerAttackSpeedModifierPercent = 1.9f;
                TowerAugmentExplanation = "Tower attack speed =" + attackSpeed;//(int)((towerAttackSpeedModifierPercent - 1f) * 100f) + '%';
                //emission.rateOverTime = (emission.rateOverTime.constant * towerAttackSpeedModifierPercent);
                //attackSpeed = attackSpeed / towerAttackSpeedModifierPercent;
                //nothing;
                break;
            case (int)RifledHead.Sniper:
                towerDmgModifierPercent = 2.75f;
                towerAttackRangeModifierPercent = 1.75f;
                towerAttackSpeedModifierPercent = 3.5f;

                TowerAugmentExplanation = "Tower damage +" + (int)(towerDmgModifierPercent * 100f) + '%';
                TowerAugmentExplanation += "\nTower attack range +" + (int)((towerAttackRangeModifierPercent -1) * 100f) + '%';
                TowerAugmentExplanation += "\nTower attack speed =" + (int)((towerAttackSpeedModifierPercent) * 100f) + "% slower";
                TowerAugmentExplanation += "\nNEW* tower minimum range = " + (int)(towerMinRangeModifier * 100f) + "% of max range";

                currentAttackRange = attackRange * towerAttackRangeModifierPercent;
                attackRange = currentAttackRange;
                minRange = currentAttackRange * towerMinRangeModifier;
                attackSpeed = attackSpeed * towerAttackSpeedModifierPercent;
                //emission.rateOverTime = (emission.rateOverTime.constant * towerAttackSpeedModifierPercent);
                towerDmg = towerDmg * towerDmgModifierPercent;
                currentTowerDmg = towerDmg;
                break;
            default:
                emission.rateOverTime = (emission.rateOverTime.constant * 2f);
                break;
        }
    }

    public override void GetStringStats()
    {
        TowerStatsTxt = "Rifled Tower Stats \n" +
            "Attack Range = " + currentAttackRange + "\n" +
            "Minimum Attack Range = " + minRange + "\n" +
            "Attack Damage = " + currentTowerDmg + "\n" +
            "Attack speed = " + attackSpeed + " s \n" +
            "Damage Type = Projectile \n" +
            "Targetting = Single Enemy";
    }
    //todo  check towerBuffs - is it in start? does it need a method? sync light tower, Tower.cs and others so its consistent.

    //Waypoint baseWaypoint    For if i pass it here
    //public void TowerBuff()
    //{
    //    attackRange = attackRange * 1.4f;
    //    currentTowerDmg = currentTowerDmg * 1.2f;

    //    keepBuffed = true;
    //}



    //public void TowerUpgrade()
    //{
    //   // attackRange = attackRange * (1.0 + .2 * timesBuffed)

    //   attackRange = attackRange * 1.2f;

    //    if (keepBuffed)
    //    {
    //        TowerBuff();
    //    }
    //}


    // Update is called once per frame
    void Update () {
        // add in here about having a priority target or something
        if (projectileParticle == null)
        {
            return; // not initiallized yet
        }

        if (attackTimer < attackSpeed)
        {
            attackTimer += Time.deltaTime;
        }


        if (preferedEnemyBody != null && preferedEnemyBody != targetEnemyBody)
        {
            float distanceToPreferedEnemy = Vector3.Distance(preferedEnemyBody.gameObject.transform.position, gameObject.transform.position);
            if ((distanceToPreferedEnemy <= currentAttackRange && preferedEnemyBody.isTargetable) && (distanceToPreferedEnemy >= minRange)) //targetEnemyBody.isTargetable
            {
                //print(preferedEnemyBody.gameObject.name);
                targetEnemyBody = preferedEnemyBody;
                targetEnemy = preferedEnemyBody.gameObject.transform;
            }
        }

        if (targetEnemy)
        {
            objectToPan.LookAt(targetEnemy);
            FireAtEnemy();

        }
        else
        {
            Shoot(false);
            SetTargetEnemy();
        }
	}

    public override void GetTowerUpgradeTexts(int towerHead)
    {
        towerUpgradeDescriptionOne = "Upgrade tower Damage +20%";
        towerUpgradeDescriptionTwo = "Upgrade tower attack speed +20%";
        towerUpgradeDescriptionThree = "Upgrade tower range +20%";

        switch (towerHead) // add this in at DetermineTowerHeadType
        {
            case -1:
                print("Didn't initialize the variable towerHeadType");
                break;
            default:
                break;
        }
    }

    override protected Transform GetClosest(Transform transformA, Transform transformB)
    {
        var distanceToA = Vector3.Distance(transform.position, transformA.position);
        var distanceToB = Vector3.Distance(transform.position, transformB.position);

        // Before testing, if the current closest is too close, always forfeit it.
        if (distanceToA < minRange)
        {
            return transformB;
        }
        if (distanceToB < minRange)
        {
            return transformA;
        }

        // test ranges and get closest.
        if (distanceToA <= distanceToB)
        {
            return transformA;
        }
        else
        {
            return transformB;
        }
    }

    private void FireAtEnemy()
    {
        float distanceToEnemy = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position);

        if (distanceToEnemy <= currentAttackRange && targetEnemyBody.isTargetable && distanceToEnemy >= minRange)
        {
            //Shoot(true);
            //New shot setup
            if (attackTimer >= attackSpeed)
            {
                attackTimer = 0f;
                RifledBullet bulletShot = Instantiate(bullet, transform.position, Quaternion.identity);
                float heightOffset = GetComponentInChildren<MeshFilter>().sharedMesh.bounds.extents.y * 1.10f;
                bulletShot.Instantiate(targetEnemy, currentTowerDmg, TowerTypeName, heightOffset);
            }
        }
        else
        {
            //Shoot(false);
            SetTargetEnemy();
        }
    }

    private void Shoot(bool isActive)
    {
        var emissionModule = projectileParticle.emission;
        emissionModule.enabled = isActive;
    }

    public override float GetTowerCost()
    {
        float towerCost = 0;
        singleton = Singleton.Instance;

        towerCost = (int)TowerCosts.RifledTowerCost;

        float percentToPay = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.alloyResearch);

        towerCost = towerCost * percentToPay;

        return towerCost;
    }

    //towerUpgradeDescriptionOne = "Upgrade tower Damage +20%";
    //towerUpgradeDescriptionTwo = "Upgrade tower attack speed +20%";
    //towerUpgradeDescriptionThree = "Upgrade tower range +20%";

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

        currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * costReductionForBallisticRange * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
        newExplanation = towerUpgradeDescriptionThree + "\nCost: " + currentUpgradeCost;
        upgradeTextThree = newExplanation;
    }

    public override void UpgradeBtnOne(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt( ( engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeOneUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
        stats = TowerStatsTxt;

        if (!CanPurchaseUpgrade(currentUpgradeCost))
        {
            return;
        }
        //if (gold.upgradeCount < currentUpgradeCost)
        //{
        //    print("Shouldnt allow, not enough parts!!! " + gold.upgradeCount + " < " + currentUpgradeCost);
        //    //return;   Eventually this will stop it.
        //}

        currentTowerDmg += (.2f * towerDmg);

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

        gold.UpgradeCost(currentUpgradeCost);
        upgradeTwoUsed++;
        anyUpgradeUsed++;
        attackSpeed = (.8f * attackSpeed);
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

        gold.UpgradeCost(currentUpgradeCost);
        upgradeThreeUsed++;
        anyUpgradeUsed++;
        currentAttackRange += (.2f * attackRange);
        if (minRange != 0)
        {
            minRange = (currentAttackRange * towerMinRangeModifier);
        }
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }

}
