using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBars : MonoBehaviour {

    [SerializeField] Image healthBar;
    [SerializeField] Image armorBar;
    [SerializeField] Image burnTimeBar;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // HP
    public void SetHealthBarPercent(float healthPercent)
    {
        healthBar.fillAmount = healthPercent;
    }
    public float GetHPPercent()
    {
        return healthBar.fillAmount;
    }


    // ARMOR
    public void SetArmorBarPercent(float healthPercent)
    {
        armorBar.fillAmount = healthPercent;
    }
    public float GetArmorPercent()
    {
        return armorBar.fillAmount;
    }

    
    // BURN
    public void SetBurnBarPercent(float burnTimePercent)
    {
        burnTimeBar.fillAmount = burnTimePercent;
    }
    public float GetBurnTimePercent()
    {
        return burnTimeBar.fillAmount;
    }
}
