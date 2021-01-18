using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerHealth : EnemyHealth {

	// Use this for initialization
	void Start () {
        base.Start();
        hitPoints = hitPoints * 1.1f;
        hitPointsMax = hitPoints;
        goldForMyHead = goldForMyHead * 2.1f;
        enemyName = "Healer";
    }
	
	// Update is called once per frame
	void Update () {
        if (onFire)
        {
            StartCoroutine(Burning(burnDmg));
        }
    }

    public override void KillEnemy()
    {
        GetComponentInChildren<HealingBugs>().DiedSpawnCloud();
        Destroy(gameObject);
    }

    //new public IEnumerator Healing(float healPercent)
    //{
    //    yield return new WaitForSeconds(1f);
    //}
}
