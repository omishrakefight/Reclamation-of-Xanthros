using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameObject : MonoBehaviour {

    public Dictionary<string, int> enemiesKilled = new Dictionary<string, int>();
    float damage = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddDamage(float damageToAdd)
    {
        damage += damageToAdd;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void AddAKill(string enemyName)
    {
        int killAmount = 1;
        if (enemiesKilled.ContainsKey(enemyName))
        {
            //killAmount = enemiesKilled[enemyName];
            enemiesKilled[enemyName]++;
            //killAmount++;
        } else
        {
            enemiesKilled.Add(enemyName, killAmount);
        }
        
    }

    public int GetKills(string enemy)
    {
        return enemiesKilled[enemy];
    }
}
