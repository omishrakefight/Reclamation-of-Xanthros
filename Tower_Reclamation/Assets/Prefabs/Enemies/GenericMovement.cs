using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMovement : EnemyMovement {

    /// <summary>
    ///  THIS EXISTS TO INHERIT ENEMYMONVEMENT
    ///  enemy movement is already the generic information
    ///  therefore, nothing need be done but inherit and attach.
    /// </summary>

	// Use this for initialization
	override protected void Start () {
        base.Start();
	}

    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}
}
