using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericHealth : EnemyHealth {

    // Use this for initialization
    override protected void Start () {
        base.Start();
        enemyName = "Generic";
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}
}
