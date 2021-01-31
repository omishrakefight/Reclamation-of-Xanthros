using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferedEnemyPanel : MonoBehaviour {

    [SerializeField] Image health;
    [SerializeField] Image burnTimer
        ;
    [SerializeField] Text enemyName;
    //[SerializeField] Image enemyPicature;
    public float maxEnemyHP = 0;
    private EnemyHealth targetEnemy;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ((targetEnemy != null))
        {
            health.fillAmount = targetEnemy.getHPPercent();
            burnTimer.fillAmount = targetEnemy.getBurnPercent();

        } else
        {
            //if (health.fillAmount != 0f)
            //{
                enemyName.text = "Dead";
                health.fillAmount = 0f;
            //}

        }
    }

    public void SetTargetEnemy(EnemyHealth enemy)
    {
        targetEnemy = enemy;
        enemyName.text = enemy.gameObject.name;
        //maxEnemyHP = targetEnemy.hitPointsMax;
        health.fillAmount = targetEnemy.getHPPercent();
    }
}
