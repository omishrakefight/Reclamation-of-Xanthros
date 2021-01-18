using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LighteningTower : Tower {

    // Todo try to make this a physics.OverlapSphere.
    //[SerializeField] public SphereCollider attackAOE;
    [SerializeField] public float chargeTime = 8f;
    [SerializeField] public float currentChargeTime = 0;
    public float damagePercent = 1.0f; // damage is reduced by 10% for each enemy hit.
    public float damageReductionNumber = .05f;
    public bool isCharged = false;
    Singleton singleton;
    bool reducedCost = false;
    //public new int goldCost = 80;

    [SerializeField] protected Light charge;
    [SerializeField] protected ParticleSystem projectileParticle;
    [SerializeField] protected SphereCollider AOERange;
    protected LineRenderer secondLightning;

    //For tinker upgrades
    readonly new bool cantargettingModule = true;
    readonly new bool canAlloyReasearch = true;
    readonly new bool canSturdyTank = true;
    readonly new bool canHeavyShelling = false;
    readonly new bool canTowerEngineer = true;

    private GameObject target;
    private LineRenderer lineRend;

    private float arcLength = 1.25f;
    private float arcVariation = 1.25f;
    private float inaccuracy = 0.75f;
    private float timeOfZap = 0.40f;
    private float delayBetweenTargetJump = .10f;
    private float zapTimer;

    private int baseType = -1;
    private int modificationType = -1;

    //List<EnemyMovement> sceneEnemies;
    List<EnemyHealth> targets;

    protected override void Start()
    {
        //ZapTarget(FindObjectOfType<EnemyMovement>().gameObject);
        base.Start();
        //sceneEnemies = EnemySpawner.EnemyAliveList;  DONE IN BASE.
        lineRend = gameObject.GetComponent<LineRenderer>();
        zapTimer = 0;
        lineRend.SetVertexCount(1);
        GetSecondLineRender();
        secondLightning.SetVertexCount(1);

        TowerTypeName = "Lightning Tower";
    }

    public override void DelayedStart(int _baseType, int _modificationType)
    {
        base.DelayedStart(_baseType, _modificationType);

        baseType = _baseType;
        modificationType = _modificationType;
        TowerTypeExplanation = "The Lightning tower takes time storing ions.  When it is completely full, it starts sensing for nearby enemies. " +
            "Upon trigger, it releases the energy which arcs between all nearby enemies (which are oppositely charged) for massvie damage";


        chargeTime = 9f;
        singleton = Singleton.Instance;

        attackRange = 18;

        // i neeed the initialization to ge tthe turret specific stats, just make another function in here that checks and modifies, it doesnth ave to be in towerfactory.
        towerDmg = 75;
        //goldCost = (int)TowerCosts.LighteningTowerCost;

        if (!keepBuffed) { }


        if (keepBuffed)
        {
            // TODO make these additive, get the percent number, find it as an int (the difference) and add it to dmg so it wont double dip dmg boostes
            attackRange = attackRange * 1.3f;
            towerDmg = towerDmg * 1.2f;
            //attackAOE.radius = attackAOE.radius * 1.4f;
        }
        base.CheckUpgradesForTankTower(ref towerDmg, ref attackRange, ref engineeringCostReduction);
        currentTowerDmg = towerDmg;
        AOERange.radius = (attackRange * .60f);
        currentAttackRange = attackRange;
    }

    public override void GetTowerUpgradeTexts(int headType)
    {
        towerUpgradeDescriptionOne = "Upgrade tower Damage +20%";
        towerUpgradeDescriptionTwo = "Upgrade tower charge speed +20%"; // on the turret form +1 charge?
        towerUpgradeDescriptionThree = "Upgrade tower AOE +15% \nThis increases the range it will hit enemies, not the range it triggers an attack.";

        switch (headType) // add this in at DetermineTowerHeadType
        {
            case -1:
                print("Didn't initialize the variable towerHeadType");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// gets the children of this gameobject, then cycles
    /// </summary>
    private void GetSecondLineRender()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach(Transform obj in children)
        {
            if (obj != this.transform)
            {
                secondLightning = obj.GetComponentInChildren<LineRenderer>();
                if (!(secondLightning == null))
                {
                    break;
                }
            }
        }
    }

    public override void DetermineTowerHeadType(int towerInt)
    {
        towerHeadType = towerInt;
        GetTowerUpgradeTexts(towerInt);
        switch (towerInt)
        {
            case (int)LightningHead.Basic:
                TowerAugmentExplanation = "The default tower module, with no modifiers.";
                //nothing;
                break;
            case (int)LightningHead.Static:

                TowerAugmentExplanation = "Tank time to max charge = +50% slower \n" +
                    "Tank charge bonus = +20% charge speed for each enemy nearby." +
                    "The Static augment puts less focus on storing electrical ions, and instead tries to harness it from the enemies.";

                break;
            default:
                TowerAugmentExplanation = "The default tower module, with no modifiers.";
                break;

        }
    }

    public override void DetermineTowerTypeBase(int towerInt)
    {
        switch (towerInt)
        {
            case (int)LightningBase.Basic:
                //nothing, normal settings?
                TowerBaseExplanation = "Basic base.";
                break;
            case (int)LightningBase.Rapid:
                float speedDecimalModifier = .30f;
                float damageDeimalModifier = .35f;
                float attackRangeDeimalModifier = .80f;
                damageReductionNumber = .10f;


                print("Im doing rapid base");
                towerDmg = (towerDmg * .35f);
                currentTowerDmg = (currentTowerDmg * damageDeimalModifier);
                chargeTime = (chargeTime * speedDecimalModifier);
                //AOERange.radius = (AOERange.radius * .75f);
                attackRange = attackRange * attackRangeDeimalModifier;

                //TowerBaseExplanation = "Charge Speed = +" + ((int)((1 / speedDecimalModifier) * 100)).ToString() + "% \n";
                //TowerBaseExplanation = "Trigger Range = " + (AOERange.radius).ToString() + " \n";
                //TowerBaseExplanation = "Damage Range = -" + ((int)((1 - attackRange).ToString() + " \n";
                //TowerBaseExplanation += "Damage = -" + ((int)((1 - damageDeimalModifier) * 100)).ToString() + "% \n";
                TowerBaseExplanation = "Charge Speed = -" + (Mathf.RoundToInt((1 - speedDecimalModifier) * 100)).ToString() + "% \n";
                TowerBaseExplanation += "Trigger Range = " + (AOERange.radius).ToString() + " \n";
                TowerBaseExplanation += "Damage Range = -" + (Mathf.RoundToInt((1 - attackRangeDeimalModifier) * 100)).ToString() + "% \n";
                TowerBaseExplanation += "Damage = -" + (Mathf.RoundToInt((1 - damageDeimalModifier) * 100)).ToString() + "% \n";
                TowerBaseExplanation += "Damage Reduction per bounce = +100% \n"; // work here damage reduction
                break;
            default:
                print("Default base, I am towerint of : " + towerInt);
                //nothing
                break;
        }
        AOERange.radius = (attackRange * .60f);
        currentAttackRange = attackRange;
    }

    private void CheckAllEnemyRange()//List<EnemyMovement> targets)
    {
        foreach (EnemyHealth enemy in sceneEnemies)
        {
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < currentAttackRange)
            {
                targets.Add(enemy);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isCharged)
        {
            //targets.Clear();
            targets = new List<EnemyHealth>();
            print("I am charged and enemies are nearby!!");
            CheckAllEnemyRange();
            //var sceneEnemies = FindObjectsOfType<EnemyMovement>();
            for (int i = 0; i < targets.Count; i++)
            {
                try
                {
                    // Trigger lightning animation (targets available)
                    ZapTarget(other.gameObject);

                    float damageAfterBounceReduction = towerDmg * damagePercent;
                    targets[i].HitByNonProjectile(damageAfterBounceReduction, TowerTypeName);

                    if (damagePercent > .06f)
                    {
                        damagePercent -= damageReductionNumber;
                    }
                } catch (Exception e)
                {
                    //Do nothing, enemy may have died in this time (cant find it)
                }
            }
            damagePercent = 1.0f;
            currentChargeTime = 0;
            isCharged = false;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentChargeTime < chargeTime)
        {
            currentChargeTime += Time.deltaTime;
            charge.intensity = currentChargeTime / chargeTime;
        }
        else
        {
            isCharged = true;
            //ExplosionDamage();
        }

        // Lightning line renderer

        //ZapTarget(FindObjectOfType<EnemyMovement>().gameObject);

        if (zapTimer > 0)
        {
            Vector3 lastPoint = transform.position;
            int i = 1;
            lineRend.SetPosition(0, transform.position);//make the origin of the LR the same as the transform
            foreach (EnemyHealth target in targets)
            {
                try
                {
                    if (target.burrowed)
                    {
                        continue;
                    }
                    while (Vector3.Distance(target.transform.position, lastPoint) > 3.0f)
                    {//was the last arc not touching the target?
                        lineRend.SetVertexCount(i + 1);//then we need a new vertex in our line renderer
                        Vector3 fwd = target.transform.position - lastPoint;//gives the direction to our target from the end of the last arc
                        fwd.Normalize();//makes the direction to scale
                        fwd = Randomize(fwd, inaccuracy);//we don't want a straight line to the target though
                        fwd *= UnityEngine.Random.Range(arcLength * arcVariation, arcLength);//nature is never too uniform
                        fwd += lastPoint;//point + distance * direction = new point. this is where our new arc ends
                        lineRend.SetPosition(i, fwd);//this tells the line renderer where to draw to
                        i++;
                        lastPoint = fwd;//so we know where we are starting from for the next arc
                    }
                    lineRend.SetVertexCount(i + 1);
                    lineRend.SetPosition(i, target.transform.position);
                    //lightTrace.TraceLight(gameObject.transform.position, target.transform.position);
                    zapTimer = zapTimer - Time.deltaTime;
                }
                catch (Exception)
                {
                    // nothing, enemy maybe died while bolt is still being drawn.
                }
            }

            //Second Beam drawing
            Vector3 lastPoint2 = transform.position;
            int i2 = 1;
            secondLightning.SetPosition(0, transform.position);//make the origin of the LR the same as the transform
            foreach (EnemyHealth target in targets)
            {
                try
                {
                    if (target.burrowed)
                    {
                        continue;
                    }
                    while (Vector3.Distance(target.transform.position, lastPoint2) > 3.0f)
                    {//was the last arc not touching the target?
                        secondLightning.SetVertexCount(i2 + 1);//then we need a new vertex in our line renderer
                        Vector3 fwd = target.transform.position - lastPoint2;//gives the direction to our target from the end of the last arc
                        fwd.Normalize();//makes the direction to scale
                        fwd = Randomize(fwd, inaccuracy);//we don't want a straight line to the target though
                        fwd *= UnityEngine.Random.Range(arcLength * arcVariation, arcLength);//nature is never too uniform
                        fwd += lastPoint2;//point + distance * direction = new point. this is where our new arc ends
                        secondLightning.SetPosition(i2, fwd);//this tells the line renderer where to draw to
                        i2++;
                        lastPoint2 = fwd;//so we know where we are starting from for the next arc
                    }
                    secondLightning.SetVertexCount(i2 + 1);
                    secondLightning.SetPosition(i2, target.transform.position);
                    //lightTrace.TraceLight(gameObject.transform.position, target.transform.position);
                }
                catch (Exception)
                {
                    // nothing, enemy maybe died while bolt is still being drawn.
                }
            }
        }
        else
        {
            lineRend.SetVertexCount(1);
            secondLightning.SetVertexCount(1);
        }
            
    }




    public override void GetStringStats()
    {
        TowerStatsTxt = "Lightning Tower Stats \n" +
            "Attack Range = " + currentAttackRange + "\n" +
            "Attack Damage = " + currentTowerDmg + "\n" +
            "Attack speed = " + chargeTime.ToString() + " second charge. \n" +
            "Each bounce loses " + damageReductionNumber + "% total damage\n" +
            "Damage Type = Lightning, instant. \n" +
            "Targetting = AOE centered on tower.";
    }

    public override float GetTowerCost()
    {
        float towerCost = 0;
        singleton = Singleton.Instance;

        towerCost = (int)TowerCosts.LighteningTowerCost;

        float percentToPay = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.alloyResearch);

        towerCost = towerCost * percentToPay;

        return towerCost;
    }

    new public void SetHead(Transform towerHead)
    {
        //Do nothing, this tower doesnt have a swivelHead so doesnt matter
    }


    private Vector3 Randomize(Vector3 newVector, float devation)
    {
        newVector += new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)) * devation;
        newVector.Normalize();
        return newVector;
    }

    public void ZapTarget(GameObject newTarget)
    {
        print("zap called");
        target = newTarget;
        zapTimer = timeOfZap;
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
        int currentUpgradeCost = Mathf.RoundToInt((baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeOneUsed * thisUpgradeCostInc)) + (baseCost * baseUpgradePercent));
        string newExplanation = towerUpgradeDescriptionOne + "\nCost: " + currentUpgradeCost;
        upgradeTextOne = newExplanation;

        currentUpgradeCost = Mathf.RoundToInt((baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeTwoUsed * thisUpgradeCostInc)) + (baseCost * baseUpgradePercent));
        newExplanation = towerUpgradeDescriptionTwo + "\nCost: " + currentUpgradeCost;
        upgradeTextTwo = newExplanation;

        currentUpgradeCost = Mathf.RoundToInt((baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc)) + (baseCost * baseUpgradePercent));
        newExplanation = towerUpgradeDescriptionThree + "\nCost: " + currentUpgradeCost;
        upgradeTextThree = newExplanation;
    }

    public override void UpgradeBtnOne(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt((baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeOneUsed * thisUpgradeCostInc)) + (baseCost * baseUpgradePercent));

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
        int currentUpgradeCost = Mathf.RoundToInt((baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeTwoUsed * thisUpgradeCostInc)) + (baseCost * baseUpgradePercent));

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
        stats = TowerStatsTxt;

        if (!CanPurchaseUpgrade(currentUpgradeCost))
        {
            return;
        }

        // TODO this doesnt work with crystal tower.
        chargeTime = (.8f * chargeTime);

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
        int currentUpgradeCost = Mathf.RoundToInt((baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc)) + (baseCost * baseUpgradePercent));

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
        stats = TowerStatsTxt;

        if (!CanPurchaseUpgrade(currentUpgradeCost))
        {
            return;
        }

        currentAttackRange += (.15f * attackRange);

        gold.UpgradeCost(currentUpgradeCost);
        upgradeThreeUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }

}