using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Plasma : Tower
{

    public float distanceToEnemyTest;
    public CapsuleCollider laser;
    [SerializeField] ParticleSystem spray;
    List<EnemyHealth> targetsList = new List<EnemyHealth>();
    Tower_PlasmaHead plasmaTargeter;
    LineRenderer lineRenderer;

    int towerType = -1;
    int crystalMaxCharge = 3;

    float crystalDmgInterval = .25f;
    float crystalCurrentBeamTime = 0f;
    float crystalCurrentChargeTime = 0f;
    private int crystalDMGPerStage = 3;
    int crystalBaseMaxDmg = 9;
    int crystalBeamLevel = 0;
    Transform targetLastShot = null;
    ParticleSystem beamParticles;
    float minTowerDmg = 15;
    float maxTowerDmg = 30f;

    float maxCharge;
    float currentChargeTime = 0f;
    bool canFire = false;

    float xIncrease = -1;
    float yIncrease = -1;
    float zLaserPullout = -1;

    //1.25 worked well
    float laserOnTime = .25f;
    float laserCurrentTime = 0f;
    bool laserIsOn = false;

    private int headType = 0;

    Singleton singleton;

    private int baseType = -1;
    private int modificationType = -1;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();

        //laser = transform.GetComponentInChildren<CapsuleCollider>();
        TowerTypeName = "Plasma Tower";
    }


    public override void DelayedStart(int _baseType, int _modificationType)
    {
        base.DelayedStart(_baseType, _modificationType);

        baseType = _baseType;
        modificationType = _modificationType;

        TowerTypeExplanation = "The plasma tower is the only turret that has a strong enough hit to pierce the target completely.  " +
            "This heavy shot comes at a slight cost to accuracy: not enough to entirely miss the target, but it does make it a gamble on where you hit them, " +
            "and in direct correlation, how much damage the shot causes.";

        maxCharge = 4.0f;
        goldCost = (int)TowerCosts.PlasmaTowerCost;
        attackRange = 30;
        currentAttackRange = attackRange;
        towerDmg = 18;
        base.CheckUpgradesForRifledTower(ref towerDmg, ref attackRange, ref engineeringCostReduction);
        CheckAndApplyBuff();

        // make this a function and all 3 are switches with these as defaults

    }


    public override void GetTowerUpgradeTexts(int towerType)
    {
        towerUpgradeDescriptionOne = "Upgrade tower Damage +20%"; // change dmg to +1 charge
        towerUpgradeDescriptionTwo = "Upgrade tower charge speed +20%"; // overwritten in head for +1 charge?  --no leave this faster charging
        towerUpgradeDescriptionThree = "Upgrade laser width +15%, increase laser range 5%"; // 7 longer beam, 15% wider.

        switch (towerType) // add this in at DetermineTowerHeadType
        {
            case -1:
                print("Didn't initialize the variable towerHeadType");
                break;
            case (int)PlasmaHead.Crystal:
                towerUpgradeDescriptionOne = "Upgrade tower max charge +1"; // change dmg to +1 charge

                towerUpgradeDescriptionThree = "Upgrade tower range +15%";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (preferedEnemyBody != null && preferedEnemyBody != targetEnemyBody)
        {
            float distanceToPreferedEnemy = Vector3.Distance(preferedEnemyBody.gameObject.transform.position, gameObject.transform.position);
            if (distanceToPreferedEnemy <= attackRange && targetEnemyBody.isTargetable)
            {
                targetEnemyBody = preferedEnemyBody;
                targetEnemy = preferedEnemyBody.gameObject.transform;
            }
        }
        switch (headType)
        {
            case (int)PlasmaHead.Basic:
                TowerChargedShotAttacks();
                break;
            case (int)PlasmaHead.Crystal:
                if (targetEnemy)
                {
                    objectToPan.LookAt(targetEnemy);
                    FireAtEnemyWithCrystal();
                }
                else
                {
                    Shoot(false);
                    SetTargetEnemy();
                    // split tower, i have this same code twice...
                    //crystalBeamLevel = 0;
                    //crystalCurrentChargeTime = 0f;
                    //maxTowerDmg = crystalBaseMaxDmg;
                    //lineRenderer.widthMultiplier = 1.00f;
                    //beamParticles.enableEmission = false;
                }


                break;
        }
        
    }

    private void TowerChargedShotAttacks()
    {
        if (!canFire)
        {
            currentChargeTime += 1 * Time.deltaTime;
            if (currentChargeTime > maxCharge)
            {
                canFire = true;
                currentChargeTime = 0f;
            }
        }

        if (laserIsOn)
        {
            //spray.emission.SetBurst(1);
            laserCurrentTime += 1 * Time.deltaTime;
            if (laserCurrentTime > laserOnTime)
            {
                //get list first, before turning off object.
                GetListOfEnemies();

                laserCurrentTime = 0f;
                canFire = false;
                laserIsOn = false;
                spray.Emit(10);

                HitEnemies();
                // hit then clear them
                targetsList.Clear();
                plasmaTargeter.ClearEnemies();
                laser.gameObject.SetActive(false);
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

    public void HitEnemies()
    {
        print(targetsList.Count + " enemies in list");

        towerDmg = UnityEngine.Random.Range(minTowerDmg, maxTowerDmg);
        print("Plasma hit for " + towerDmg);
        foreach (EnemyHealth enemy in targetsList)
        {
            try
            {
                enemy.HitByNonProjectile(towerDmg, TowerTypeName);
            } catch(Exception e)
            {
                print("problem hitting the guy " + enemy.name);
                // nothing it may have died since being in the list.
            }
            
        }
    }


    public void GetListOfEnemies()
    {
        plasmaTargeter = GetComponentInChildren<Tower_PlasmaHead>();

        targetsList = plasmaTargeter.getEnemies();
    }


    public override void DetermineTowerTypeBase(int towerInt)
    {
        switch (towerInt)
        {
            case (int)PlasmaBase.Basic:
                //nothing, normal settings?
                TowerBaseExplanation = "Basic base.";
                break;
            //case (int)PlasmaBase.:
            //    TowerBaseExplanation = "Industrial base.";
            //    break;
            default:
                print("Default base, I am towerint of : " + towerInt);
                //nothing
                break;
        }
        currentAttackRange = attackRange;
        currentTowerDmg = towerDmg;
    }


    public override void DetermineTowerHeadType(int towerInt)
    {
        towerHeadType = towerInt;
        GetTowerUpgradeTexts(towerInt);
        switch (towerInt)
        {
            case (int)PlasmaHead.Basic:
                headType = (int)PlasmaHead.Basic;
                laser = transform.GetComponentInChildren<CapsuleCollider>();
                TowerAugmentExplanation = "The default head of the Plasma Turret.  Hits in a line for randomised damage.";
                minTowerDmg = 30;
                maxTowerDmg = 90;
                laser.gameObject.SetActive(false);
                spray = GetComponentInChildren<ParticleSystem>();
                xIncrease = (laser.transform.localScale.x * .15f);
                yIncrease = (laser.transform.localScale.y * .05f);
                zLaserPullout = (laser.transform.position.z * .05f);
                //nothing;
                break;
            case (int)PlasmaHead.Crystal:
                // base is .25 ats so 4-12 DPS, maybe add 1 max per channel buff, ends at 4-20 dmg.  which is 8, 10, 12 DPS  but bad at target swapps.
                //Sounds balanced, avg is 12 DPS. but considering the ramp-up time and randomness, i think its good.
                // maybe make these a co-routine for stacks falling off.
                headType = (int)PlasmaHead.Crystal;
                maxCharge = 1.0f;
                crystalDmgInterval = .25f;
                lineRenderer = GetComponentInChildren<LineRenderer>();
                lineRenderer.SetPosition(0, (gameObject.transform.position + new Vector3(0, 5.5f, 0)));
                lineRenderer.useWorldSpace = true;
                beamParticles = GetComponentInChildren<ParticleSystem>();
                beamParticles.enableEmission = false;
                lineRenderer.enabled = false;
                TowerAugmentExplanation = "The crystal head of the Plasma Turret.  Amplifies the effects for a single target.";
                minTowerDmg = 3f;
                maxTowerDmg = crystalBaseMaxDmg; 
                //nothing;
                break;
            default:
                TowerAugmentExplanation = "The default head of the Plasma Turret.  Hits in a line for randomised damage.";
                break;
        }
        currentAttackRange = attackRange;
        currentTowerDmg = towerDmg;
    }


    public override void GetStringStats()
    {
        TowerStatsTxt = "Plasma Tower Stats \n" +
            "Attack Range = " + attackRange + "\n" +
            "Minimum Attack Damage = " + minTowerDmg + " \n" +
            "Maximum Attack Damage = " + maxTowerDmg + " \n" +
            "Attack Speed = This Tower charges over " + maxCharge  + " seconds \n" +
            "Targetting = Piercing shot through target.";
        if (towerType == (int)PlasmaHead.Crystal)
        {
            TowerStatsTxt += "\n\nBonus Max dmg Per charge level: " + crystalDMGPerStage + "\n" +
                "Max charge level increases : " + crystalMaxCharge;
        }
    }


    private void FireAtEnemy()
    {

        float distanceToEnemy = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position);
        if (distanceToEnemy <= attackRange)
        {
            Shoot(true);
        }
        else
        {
            Shoot(false);
            SetTargetEnemy();
        }
        distanceToEnemyTest = distanceToEnemy;
    }


    private void FireAtEnemyWithCrystal()
    {

        float distanceToEnemy = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position);
        if (distanceToEnemy <= attackRange)
        {
            //Check if this is the same target as before.
            if (targetEnemy != targetLastShot)
            {
                targetLastShot = targetEnemy;

                //if I target swap, reset dmg and timers.
                crystalBeamLevel = 0;
                maxTowerDmg = crystalBaseMaxDmg;
                crystalCurrentChargeTime = 0f;
                lineRenderer.widthMultiplier = 1.00f;
                beamParticles.enableEmission = false;
            }

            //Shoot now
            ShootWithCrystal(true);
        }
        else
        {
            ShootWithCrystal(false);
            SetTargetEnemy();
        }
        distanceToEnemyTest = distanceToEnemy;
    }

    // a better way to swap this? so not setting true every frame?
    private void ShootWithCrystal(bool isActive)
    {
        float baseBeamWith = 1.0f;
        if (isActive)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, (targetEnemy.transform.position));

            crystalCurrentBeamTime += (1 * Time.deltaTime);

            // 0, 1, 2;
            if(crystalBeamLevel < crystalMaxCharge)
            {
                crystalCurrentChargeTime += (1 * Time.deltaTime);

                // this is a cheap way to have it upgrade beam level every second (the (int) truncates the int) so every second it goes up a rank and if rank doesnt match
                // previous rank then it upgrades the beam.
                if (crystalCurrentChargeTime > maxCharge)
                {
                    crystalCurrentChargeTime = 0f;
                    crystalBeamLevel++;
                    maxTowerDmg += crystalDMGPerStage;
                    lineRenderer.widthMultiplier = (baseBeamWith + (.25f * crystalBeamLevel));
                    //if (crystalCurrentChargeTime < 2.0f)
                    //{
                    //    crystalBeamLevel++;
                    //    maxTowerDmg += crystalDMGPerStage;
                    //    lineRenderer.widthMultiplier = 1.30f;
                    //    // add in dmg increase, and in the target swap function restet both the beam level and the charge timer.
                    //} else
                    //{
                    //    crystalBeamLevel++;
                    //    maxTowerDmg += crystalDMGPerStage;
                    //    lineRenderer.widthMultiplier = 1.60f;
                    //}
                }
            }
            if (crystalCurrentBeamTime > .25f)
            {
                crystalCurrentBeamTime = (crystalCurrentBeamTime - .25f);
                float towerDmg = UnityEngine.Random.Range(1, maxTowerDmg);
                //print("Plasma beam dmg = " + towerDmg);
                //TODO NEED TO CHANGE this needs to only get the enemy health on TARGET CHANGE way too process intensive to get 4 times a second.
                try
                {
                    targetEnemyBody.HitByNonProjectile(towerDmg, TowerTypeName); // .hitPoints -= towerDmg;
                }
                catch (Exception e)
                {
                    //Enemy died during sending dmg, no problem.
                }
                //Shifted this to the enemy to refresh health and give gold / die
                //targetEnemyBody.RefreshHealthBar();
                //if (targetEnemyBody.hitPoints < 1)
                //{
                //    try
                //    {
                //        targetEnemyBody.KillsEnemyandAddsGold();
                //    }
                //    catch (Exception ex)
                //    {
                //        print("Exception killing enemy  skipping now");
                //    }
                    
                //}
            }
            //finally look at the last point
            beamParticles.enableEmission = true;
            beamParticles.transform.position = targetEnemyBody.transform.position;
            beamParticles.transform.LookAt(this.transform);
            //laser.gameObject.SetActive(true);

        } else
        {
            lineRenderer.enabled = false;
            beamParticles.enableEmission = false;
        }
    }

    private void Shoot(bool isActive)
    {
        if (canFire && isActive)
        {
            laser.gameObject.SetActive(true);
            laserIsOn = true;
        }
    }


    public override float GetTowerCost()
    {
        float towerCost = 0;
        singleton = Singleton.Instance;

        towerCost = (int)TowerCosts.PlasmaTowerCost;

        float percentToPay = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.alloyResearch);

        towerCost = towerCost * percentToPay;

        return towerCost;
    }


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

        currentUpgradeCost = Mathf.RoundToInt(costReductionForBallisticRange * (engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
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
        //if (gold.upgradeCount < currentUpgradeCost)
        //{
        //    print("Shouldnt allow, not enough parts!!! " + gold.upgradeCount + " < " + currentUpgradeCost);
        //    //return;   Eventually this will stop it.
        //}

        switch (towerHeadType)
        {
            case (int)PlasmaHead.Crystal:
                crystalMaxCharge++;
                break;
            default:
                currentTowerDmg += (.2f * towerDmg);
                maxTowerDmg += (.2f * maxTowerDmg);
                minTowerDmg += (.2f * minTowerDmg);
                break;
        }


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

        maxCharge = (.8f * maxCharge);

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
        int currentUpgradeCost = Mathf.RoundToInt(costReductionForBallisticRange * (engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
        stats = TowerStatsTxt;

        if (!CanPurchaseUpgrade(currentUpgradeCost))
        {
            return;
        }

        switch (towerHeadType)
        {
            case (int)PlasmaHead.Crystal:
                currentAttackRange += (.15f * attackRange);
                break;
            default:
                //float laserY = (laser.transform.localScale.y * .05f);   //  y x
                //float laserXbuff = (laser.transform.localScale.y * .15f);
                laser.transform.localScale += new Vector3(xIncrease, yIncrease, 0f);//(laser.transform.localScale.y + laserY); // needs to be a vector 3
                laser.transform.localPosition += new Vector3(0, 0, zLaserPullout);
                break;
        }


        gold.UpgradeCost(currentUpgradeCost);
        upgradeThreeUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }

}
