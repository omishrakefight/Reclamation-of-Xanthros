using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimerMovement : EnemyMovement {

    
    // Use this for initialization
    override protected void Start () {
        base.Start();
    }


    public void SlimerDeathExplosion()
    {
        //Transform here = this.transform;

        int nextNode = currentPathNode++;
        // sparays the NEXT 2 nodes, therefore, new variable and +1 twice
        if (nextNode < (path.Count - 2) && (!path[nextNode].isSlimed))
        {
            GetComponent<SlimeBug>().SpawnSlime(path[nextNode].transform.position, path[nextNode + 1].transform.position, path[nextNode + 2].transform.position);
            path[nextNode].isSlimed = true;
        }

        nextNode++;
        if (nextNode < (path.Count - 2) && (!path[nextNode].isSlimed))
        {
            GetComponent<SlimeBug>().SpawnSlime(path[nextNode].transform.position, path[nextNode + 1].transform.position, path[nextNode + 2].transform.position);
            path[nextNode].isSlimed = true;
        }
        //foreach (Waypoint WP in path)
        //{
        //    if (!WP.isSlimed)
        //    {
        //        if ((Vector3.Distance(here.position, WP.transform.position)) < 18)
        //        {

        //        }
        //    }
        //}
    }

    // Update is called once per frame
    override protected void Update () {

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
                //punchingBase = true;
                //GetComponent<EnemyDamage>().startPunchingBase();
                //GetComponent<EnemyHealth>().GotToEnd();
                //FindObjectOfType<MyHealth>().AnEnemyFinishedThePath();
            }
            else
            {
                punchingBase = false;
                if (path[currentPathNode].isSlimed == false) {
                    if (currentPathNode < (path.Count - 2))
                    {
                        GetComponent<SlimeBug>().SpawnSlime(path[currentPathNode].transform.position, path[currentPathNode + 1].transform.position, path[currentPathNode + 2].transform.position);
                        path[currentPathNode].isSlimed = true;
                    } else
                    {
                        GetComponent<SlimeBug>().SpawnSlime(path[currentPathNode].transform.position, path[currentPathNode + 1].transform.position);
                        path[currentPathNode].isSlimed = true;
                    }
                }
                // increments the path node (go to next one) and turns them if need be.
                ++currentPathNode;
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

                healthBar.transform.LookAt(healthBar.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

                if (path[currentPathNode + 1].isSlimed)
                {
                    slimeMultiplier = 1.8f;
                }
                else
                {
                    slimeMultiplier = 1.0f;
                }

                // chilled is 0?
                enemySpeed = enemyBaseSpeed * chilledMultiplier * frenzyMultiplier * slimeMultiplier;
                //print(enemySpeed + "is speed   " + chilledMultiplier + frenzyMultiplier + slimeMultiplier);

            }
        }
    }
}
