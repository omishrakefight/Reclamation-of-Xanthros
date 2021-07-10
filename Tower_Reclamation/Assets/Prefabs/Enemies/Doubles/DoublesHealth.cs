using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublesHealth : EnemyHealth {

	// Use this for initialization
	override protected void Start () {
        base.Start();
        hitPoints = (.75f * hitPoints);
        hitPointsMax = hitPoints;
        enemyName = "Doubles";

        // They spawn 2 at once, get half the gold.
        goldForMyHead = goldForMyHead * .5f;
	}
	
	// Update is called once per frame
	override protected void Update () {
        base.Update();
	}

    public override void KillEnemy()
    {
        if (volcanicEnhanced)
        {
            GameObject littleSpawn = Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    override public void GiveEnhancement(int biome)
    {
        switch (biome)
        {
            case (int)Biomes.Volcanic:
                hitPoints = hitPoints * 1.15f;
                hitPointsMax = hitPoints;
                volcanicEnhanced = true;

                break;
        }
    }

}
