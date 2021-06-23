using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerMovement : EnemyMovement {

    public bool turnedBaneling = false;

    // Use this for initialization
    override protected void Start () {
        base.Start();
        enemyBaseSpeed = enemyBaseSpeed * 1.4f;
	}

    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    public void IsBaneling()
    {
        turnedBaneling = true;
    }

    override protected void GotToEndOfPath()
    {
        if (!turnedBaneling)
        {
            punchingBase = true;
            GetComponent<EnemyDamage>().startPunchingBase();
        } else
        {
            EnemyDamage dmg = GetComponent<EnemyDamage>();
            float baseDmg = dmg.GetDamage();

            // baneling explode 4x dmg (20 ATM) then dies.
            dmg.HitTheBaseOnce(baseDmg * 4);
            GetComponent<RollerHealth>().KillsEnemyandAddsGold();
        }
    }
}
