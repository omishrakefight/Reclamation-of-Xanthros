using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimerHealth : EnemyHealth {

    // Use this for initialization
    protected bool slimeOnDeath = false;
    //protected bool initialized = false;

    [SerializeField] protected ParticleSystem PaticleSlimeOnDeath;


	override protected void Start () {
        base.Start();
        hitPoints = hitPoints * 1.2f;
        hitPointsMax = hitPoints;
        goldForMyHead = goldForMyHead * 2.1f;
        enemyName = "Slimer";
    }
	
	// Update is called once per frame
	override protected void Update () {
        base.Update();
	}

    override public void GiveEnhancement(int biome)
    {
        switch (biome)
        {
            case (int)Biomes.Volcanic:
                hitPoints = hitPoints * 1.2f;
                hitPointsMax = hitPoints;
                volcanicEnhanced = true;

                slimeOnDeath = true;
                break;
        }
    }

    public override void KillEnemy()
    {
        if (slimeOnDeath)
        {
            ParticleSystem PS = Instantiate(PaticleSlimeOnDeath, transform.position, Quaternion.identity);//transform.position, Quaternion.identity);
            PS.transform.Rotate(-180, 0, 0);//.RotateAround();// = new Vector3(-90, 0, 0);
            GetComponentInChildren<SlimerMovement>().SlimerDeathExplosion();
        }

        Destroy(gameObject);
    }


}
