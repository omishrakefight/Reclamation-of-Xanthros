using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {

    private float damage = 5;
    private float attackSpeed = 2.0f;
    private MyHealth baseHealth;

	// Use this for initialization
	void Start () {
        baseHealth = FindObjectOfType<MyHealth>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetDamage()
    {
        return damage;
    }

    public void startPunchingBase()
    {
        StartCoroutine(AttckTheBase());
    }

    public IEnumerator AttckTheBase()
    {
        while (true)
        {
            baseHealth.AnEnemyIsHittingBase(damage);
            yield return new WaitForSeconds(attackSpeed);
        }
    }
    
    /// <summary>
    /// For special hits aka banelings or a charge bonus.  This does a single hit non repeating.
    /// </summary>
    /// <param name="dmg"></param>
    public void HitTheBaseOnce(float dmg)
    {
        baseHealth.AnEnemyIsHittingBase(dmg);
    }

}
