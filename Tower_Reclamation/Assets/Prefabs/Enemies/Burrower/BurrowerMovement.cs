using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowerMovement : EnemyMovement {

    public bool recentlyHit = false;
    public float hitCountdown = 1f;
    public float currentHitCountdown = 0f;

    public bool burrowed = false;
    public float currentBurrowTime = 0f;
    public float burrowTime = 2.25f;

    public float currentDiggingTime = 0f;
    public float digTime = .25f;

    private float distanceToBurrow;

    protected bool canBurrow = true;
    protected Vector3 currentDigSite;

    public bool canBeHit = true;
    public float trialSpeedForDigging;

    protected BurrowerHealth myHealth;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        canBeHit = true;
        digTime = .35f;
        burrowTime = 2f;
        distanceToBurrow = 5f;
        trialSpeedForDigging = (distanceToBurrow * ((1/digTime) * Time.deltaTime));
        myHealth = GetComponent<BurrowerHealth>();
    }

    // Update is called once per frame
    override protected void Update()
    {
        if (recentlyHit && !burrowed)
        {
            currentHitCountdown += 1 * Time.deltaTime; // Add this gets set to 0 bool is false, and start burrow *************
        }
        if (burrowed)
        {
            currentBurrowTime += 1 * Time.deltaTime;
        }

        if (chilled)
        {
            StartCoroutine(Chilled(chilledMultiplier));
        }

        float enemySpeedASecond = enemySpeed * Time.deltaTime;

        //Burrow if timed, otherwise move.

        if (currentHitCountdown > 1.0f && currentDiggingTime < digTime)
        {
            //print("failing to dig?");
            //currentHitCountdown = 0f;
            // add the time to burrowed here, then subtrat in unburrow.  great way to manage it and withotu an extra if loop
            currentDiggingTime += 1 * Time.deltaTime;
            if (canBurrow)
            {
                Burrow();
            }
            if (currentDiggingTime >= digTime)
            {
                burrowed = true;
                heightOffset.y -= distanceToBurrow;
                myHealth.isTargetable = false;
                myHealth.Burrowed();
                UntargetMeFools();
            }

            transform.position = Vector3.MoveTowards(transform.position, (new Vector3(transform.position.x, (currentDigSite.y - distanceToBurrow - distanceToBurrow), transform.position.z) + heightOffset), trialSpeedForDigging);
            if (punchingBase)
                return;
        }
        else if (currentBurrowTime > burrowTime && currentDiggingTime > 0)
        {
            //print(currentBurrowTime + " > " + burrowTime + " && " + currentDiggingTime);
            currentDiggingTime -= 1 * Time.deltaTime;
            if (burrowed)
            {
                Unburrow();
            }
            if (currentDiggingTime <= 0)
            {
                currentBurrowTime = 0;
                canBurrow = true;
                burrowed = false;
                canBeHit = true;
                heightOffset.y += distanceToBurrow;
                
                myHealth.isTargetable = true;
                myHealth.Unburrowed();

            }
            transform.position = Vector3.MoveTowards(transform.position, (new Vector3(transform.position.x, (currentDigSite.y + distanceToBurrow + distanceToBurrow), transform.position.z) + heightOffset), trialSpeedForDigging);
            if (punchingBase)
                return;
        }
        else if (punchingBase)
        {
            return;
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, (path[currentPathNode + 1].transform.position + heightOffset), enemySpeedASecond);
        }

        if (transform.position == path[currentPathNode + 1].transform.position + heightOffset && !punchingBase)
        {
            if (transform.position == path[path.Count - 1].transform.position + heightOffset)
            {
                GotToEndOfPath();
                //punchingBase = true;
                //GetComponent<EnemyDamage>().startPunchingBase();
                //GetComponent<EnemyHealth>().GotToEnd();
                //FindObjectOfType<MyHealth>().AnEnemyFinishedThePath();
            }
            else
            {
                punchingBase = false;
                // increments the path node (go to next one) and turns them if need be.
                ++currentPathNode;
                //if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).x > 1f)
                //{
                //    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                //}
                //if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).x < -1f)
                //{
                //    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                //}
                //if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).z > 1f)
                //{
                //    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                //}
                //if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).z < -1f)
                //{
                //    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                //}

                //if (path[currentPathNode + 1].isSlimed)
                //{
                //    slimeMultiplier = 1.4f;
                //}
                //else
                //{
                //    slimeMultiplier = 1.0f;
                //}
                CheckEnemyDirection();

                // chilled is 0?
                enemySpeed = enemyBaseSpeed * chilledMultiplier * frenzyMultiplier * slimeMultiplier;
                //print(enemySpeed + "is speed   " + chilledMultiplier + frenzyMultiplier + slimeMultiplier);

            }
        }
    }

    public void UntargetMeFools()
    {
        var sceneTowers = FindObjectsOfType<Tower>();
        //print("i command you to Untarget ME!, UntargetMeFools!");
        foreach(Tower foundTower in sceneTowers)
        {
            //print("I am telling you, in my list, to stop!");
            if (foundTower.targetEnemy == this.gameObject.transform)
            {
                //print("Im going to ask you to stop shooting me, please.");
                foundTower.SetTargetEnemy();
            }
        }
    }

    public void IWasHit()
    {
        if (canBeHit) {
            recentlyHit = true;
            canBeHit = false;
        }
    }

    public void Unburrow()
    {
        burrowed = false;
        currentHitCountdown = 0;
        currentDigSite = transform.position;
    }

    public void Burrow()
    {
        
        //burrowed = true;
        recentlyHit = false;
        canBurrow = false;
        currentDigSite = transform.position;
        
    }
}
