using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyMovement : MonoBehaviour
{
    public bool willSlime = false;
    Camera camera;
    protected Canvas healthBar;

    //[SerializeField] Canvas enemyHealthBar;
    protected bool punchingBase = false;

    [SerializeField] public float enemySpeed = 5.75f;

    [SerializeField] public float enemyBaseSpeed = 5.75f;
    protected float enemySpeedMultiplier; // equal to chilledMultiplier, frenzy, and slimeMultiplier.

    public bool chilled = false;
    public float chillTimer = 1f;
    public float timer = 0f;
    public float chilledMultiplier = 1f;

    public float frenzyMultiplier = 1f;
    public float slimeMultiplier = 1f; /// <summary>
                                       ///  The path is generated at enemy spawn.
                                       ///  They only get speed buffs for each tile that  was slimed AT time of spawn.
                                       ///  need to dynamically check?
                                       /// </summary>

    protected List<Waypoint> path;

    protected int currentPathNode = 0;

    protected Vector3 heightOffset = new Vector3(0f, 0f, 0f);


    protected virtual void Start()
    {
        camera = Camera.main; 
        PathFinder pathFinder = FindObjectOfType<PathFinder>();
        path = pathFinder.GivePath();
        transform.position = path[0].transform.position;

        chilledMultiplier = 1f;
        frenzyMultiplier = 1f;
        slimeMultiplier = 1f;
        // this is hard coded for the worms, need to det up dynamically if i can find the height from volume.  That or add rando field to enemy that contains the numbers.
        heightOffset = new Vector3(0f, 1f, 0f);
        //heightOffset = new Vector3(0f, gameObject.GetComponent<Volume>().Pivot.y, 0f);
        // StartCoroutine(FollowWaypoints(path));
        //get healthbar for rotations
        healthBar = GetComponent<EnemyHealth>().enemyHealthBar;

        CheckEnemyDirection();
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (punchingBase)
        {
            //if iv'e made it to base, stop trying to move and just hit it.
            return;
        }

        if (chilled)
        {
            StartCoroutine(Chilled(chilledMultiplier));
        }

        float enemySpeedASecond = enemySpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, (path[currentPathNode + 1].transform.position + heightOffset), enemySpeedASecond);

        if (transform.position == path[currentPathNode + 1].transform.position + heightOffset)
        {
            if (transform.position == path[path.Count - 1].transform.position + heightOffset && !punchingBase)
            {
                GotToEndOfPath();
                //GetComponent<EnemyHealth>().GotToEnd();
                //FindObjectOfType<MyHealth>().AnEnemyFinishedThePath();
            }
            else
            {
                punchingBase = false;
                if (willSlime)
                {
                    // todo change movement to a abstract and so this will be in start specific to monster. (not every time)
                    GetComponent<SlimeBug>().SpawnSlime(path[currentPathNode].transform.position, path[currentPathNode + 1].transform.position);
                    path[currentPathNode].isSlimed = true;
                }

                // increments the path node (go to next one) and turns them if need be.
                // TODO turn this into a function 'Fix Enemy Direction() and have it activate on start.
                // Todo move the model part of the enemy so that only it spins not the whole enemy.
                ++currentPathNode;
                CheckEnemyDirection();

                // chilled is 0?
                enemySpeed = enemyBaseSpeed * chilledMultiplier * frenzyMultiplier * slimeMultiplier;
                //print(enemySpeed + "is speed   " + chilledMultiplier + frenzyMultiplier + slimeMultiplier);

            }
        }


    }

    virtual protected void GotToEndOfPath()
    {
        punchingBase = true;
        GetComponent<EnemyDamage>().startPunchingBase();
    }

    protected void CheckEnemyDirection()
    {
        if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).x > 1f)
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).x < -1f)
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).z > 1f)
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        }
        if ((path[currentPathNode].transform.position - path[currentPathNode + 1].transform.position).z < -1f)
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }

        healthBar.transform.LookAt(healthBar.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        //enemyHealthBar.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f)); 
        //enemyHealthBar.transform.LookAt(Camera.main.transform);

        if (path[currentPathNode + 1].isSlimed)
        {
            slimeMultiplier = 1.4f;
        }
        else
        {
            slimeMultiplier = 1.0f;
        }
    }

    //private List<Waypoint> FindNextNode()
    //{
    //    PathFinder pathFinder = FindObjectOfType<PathFinder>();
    //    var path = pathFinder.GivePath();
    //    return path;
    //}

    public IEnumerator Chilled(float chilledMultiplier)
    {
        // change this math for enemy speed = basespeed times multiplier.   
        if (chilled && timer < chillTimer)
        {
            timer += 1 * Time.deltaTime;
            enemySpeed = enemyBaseSpeed * chilledMultiplier * frenzyMultiplier * slimeMultiplier;
        }
        else
        {
            chilled = false;
            chilledMultiplier = 1f;
            enemySpeed = enemyBaseSpeed * chilledMultiplier * frenzyMultiplier * slimeMultiplier;
        }

        yield return new WaitForSeconds(1f);
    }

    public void gotChilled(float chilledMultiplier, float chillDuration)
    {
        chilled = true;
        timer = 0;

        if (this.chilledMultiplier > chilledMultiplier)
        {
            this.chilledMultiplier = chilledMultiplier;
            chillTimer = chillDuration;
        }
    }
}

//private void OnTriggerStay(Collider other)
//{
//    if (other.gameObject.GetComponentInParent<EnemyHealth>())
//    {
//        other.GetComponentInParent<EnemyHealth>().CaughtFire(currentTowerDmg);
//    }
//}


