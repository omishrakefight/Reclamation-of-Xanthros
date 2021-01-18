using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombBurnAOE : MonoBehaviour {

    float burnDmg = 0f;
    float healReduction = 0f;
    float lifeCycle = 2.75f;
    float currentTime = 0f;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if (currentTime > lifeCycle)
        {
            Destroy(this.gameObject);
        }
	}

    public void Initialize(float _burnDmg, float sizeIncrease, float durationIncrease)
    {
        lifeCycle += durationIncrease;

        burnDmg = _burnDmg;

        Vector3 currentSize = this.transform.localScale;// += new Vector3(0f, heightOffset, 0f);

        // This math is OK, because I handle it in Mortarshell / flametower.  It handles multiple increases and will always (normally) be over 1.
        currentSize.x = currentSize.x * sizeIncrease;
        currentSize.z = currentSize.z * sizeIncrease;
        this.transform.localScale = currentSize;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<EnemyHealth>())
        {
            other.GetComponentInParent<EnemyHealth>().CaughtFire(burnDmg, healReduction);
        }
    }
}
