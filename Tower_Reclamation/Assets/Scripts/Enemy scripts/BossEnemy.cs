using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

    [SerializeField] EnemyMovement bossEnemy;
    [SerializeField] Transform enemiesLocation;
    EnemyMovement boss;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnBoss()
    {
        boss = Instantiate(bossEnemy, transform.position, Quaternion.identity);
        boss.transform.parent = enemiesLocation;
        boss.GetComponentInChildren<EnemyHealth>().IsBoss();

    }

    public void BuffBossMob()
    {
        boss.transform.localScale = (new Vector3(2f, 2f, 2f));

        boss.enemyBaseSpeed = (bossEnemy.enemySpeed / 3f);
        boss.enemySpeed = boss.enemyBaseSpeed;

        var enemyhealth = boss.GetComponent<EnemyHealth>();
        print(enemyhealth);
        enemyhealth.hitPointsMax = (enemyhealth.hitPoints * 11);
        enemyhealth.hitPoints = (enemyhealth.hitPoints * 10);
        print("boss HP: " + enemyhealth.hitPointsMax);

        enemyhealth.DontResethealthPlease();
    }

}
