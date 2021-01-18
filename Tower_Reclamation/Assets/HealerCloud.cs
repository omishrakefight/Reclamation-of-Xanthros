using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerCloud : MonoBehaviour {


    float healPercent = .07f;

    float lifeTime = 4f;
    float counter = 0f;
    float increaseSizePerSecond = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(counter > lifeTime)
        {
            Destroy(this.gameObject);
        } else
        {
            float sizeIncrease = (increaseSizePerSecond * Time.deltaTime);
            this.gameObject.transform.localScale += new Vector3(sizeIncrease, 0, sizeIncrease);
        }
        DeathHealAura();
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<EnemyHealth>())
        {
            other.gameObject.GetComponentInParent<EnemyHealth>().HealingBuffed(healPercent);
            //print("Feast upon my blood and heal thyself!!!!!!!!!!");
        }
    }

    public void SetHealFactor(float healPercent)
    {
        this.healPercent = healPercent;
    }

    void DeathHealAura()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 15f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponentInParent<EnemyHealth>())
            {
                hitColliders[i].gameObject.GetComponentInParent<EnemyHealth>().HealingBuffed(healPercent);
                //print("Feast upon my blood and heal thyself!!!!!!!!!!");
            }
            i++;
        }
    }
}
