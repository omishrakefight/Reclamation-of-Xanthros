using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerHealth : EnemyHealth {

    public bool canDodge;
    protected float dodgeTimer = 0f;
	// Use this for initialization
	override protected void Start () {
        base.Start();
        canDodge = true;
        hitPointsMax = hitPointsMax * .65f;
        hitPoints = hitPointsMax;
        enemyName = "Roller";
    }
	
	// Update is called once per frame
	override protected void Update () {
        base.Update();
        dodgeTimer += Time.deltaTime;
        if (!canDodge)
        {
            if (dodgeTimer > 6.0f)
            {
                canDodge = true;
            }
        }
	}


    override protected void ProcessHit(float dmg, string towerName)
    {
        //float dmg = 0;
        if (canDodge)
        {
            // todo add dodge effect / sound / W/E
            canDodge = false;
            dodgeTimer = 0;
        }
        else
        {
            dodgeTimer += .5f;
            //string towerName = "";
            //dmg = other.GetComponentInParent<Tower>().Damage(ref towerName);
            hitPoints = hitPoints - dmg;
            healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);
                
            hitparticleprefab.Play();

            damageLog.UpdateDamage(towerName, dmg);
            Singleton.AddTowerDamage(towerName, dmg);
        }
        
        //    print("Current hit points are : " + hitPoints);
    }

}
