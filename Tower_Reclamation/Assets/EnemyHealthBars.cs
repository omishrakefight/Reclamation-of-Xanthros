using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBars : MonoBehaviour {

    [SerializeField] Image healthBar;
    [SerializeField] Image burnTimeBar;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHealthBarPercent(float healthPercent)
    {
        healthBar.fillAmount = healthPercent;
    }
    public float GetHPPercent()
    {
        return healthBar.fillAmount;
    }

    public void SetBurnBarPercent(float burnTimePercent)
    {
        burnTimeBar.fillAmount = burnTimePercent;
    }
}
