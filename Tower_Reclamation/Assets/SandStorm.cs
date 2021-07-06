using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStorm : MonoBehaviour
{
    private float aliveTime = 3f;
    private float currentTime = 0f;

    private bool shuttingDown = false;

    // Start is called before the first frame update
    void Start()
    {
        shuttingDown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!shuttingDown)
        other.GetComponent<EnemyHealth>().SetUntagetable();
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<EnemyHealth>().SetTargetable();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += (1 * Time.deltaTime);

        if (currentTime > aliveTime)
        {
            DestroyThisAndSetEnemiesTargetable();
        }
    }

    private void DestroyThisAndSetEnemiesTargetable()
    {
        shuttingDown = true;
        //MakeAllEnemiesTrgetableAgain();

        Destroy(this.gameObject);
    }

    private void MakeAllEnemiesTrgetableAgain()
    {
        Collider[] enemies = Physics.OverlapCapsule(this.gameObject.transform.position, (this.gameObject.transform.position + new Vector3(0f, 5f, 0f)), .6f);

        foreach (Collider enemy in enemies)
        {
            print("GOTCHA! " + enemy.name);
            //EnemyHealth enemyHealth = GetComponentInParent<EnemyHealth>();//.isTargetable = true;
            //if (enemyHealth.burrowed)
            //{
            //    return; //dont make burrowed guys targetable.
            //}
            //enemyHealth.SetTargetable();

        }
    }
}
