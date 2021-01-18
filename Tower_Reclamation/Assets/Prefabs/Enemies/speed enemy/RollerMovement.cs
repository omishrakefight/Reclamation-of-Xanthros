using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerMovement : EnemyMovement {

    // Use this for initialization
    override protected void Start () {
        base.Start();
        enemyBaseSpeed = enemyBaseSpeed * 1.4f;
	}

    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}
}
