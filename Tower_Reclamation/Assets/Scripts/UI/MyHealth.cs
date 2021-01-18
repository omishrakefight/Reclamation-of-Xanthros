using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyHealth : MonoBehaviour {

    [SerializeField] public int myNumericalHealth = 100;
    [SerializeField] public Text myHealth;
    [SerializeField] public Text myHealthNumber;
    [SerializeField] Image HPbar;
    [SerializeField] AudioClip baseHurtAudio;

    [SerializeField] Text defeat;
    [SerializeField] Text worldFalls;
    // Use this for initialization
    void Start () {
        myNumericalHealth = 100;
        myHealth.text = "Base HP : ";// + myNumericalHealth.ToString() + " / 10";
        myHealthNumber.text = myNumericalHealth.ToString() + " / 100";

        // Setting text to invisible until triggered.
        defeat.enabled = false;
        defeat.enabled = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void AnEnemyIsHittingBase(float initialDMG)
    {
        int dmg = Mathf.RoundToInt(initialDMG);
        myNumericalHealth -= dmg;
        HPbar.fillAmount -= ((float)dmg/100);
        GetComponent<AudioSource>().PlayOneShot(baseHurtAudio);
        myHealthNumber.text = myNumericalHealth.ToString() + " / 100";
        if (myNumericalHealth <= 0)
        {
            FindObjectOfType<EnemySpawner>().stillAlive = false;
            defeat.enabled = true;
            worldFalls.enabled = true;
        }
    }

    // obsolete until exploders.
    public void AnEnemyFinishedThePath()
    {
        myNumericalHealth -= 1;
        myHealthNumber.text = myNumericalHealth.ToString() + " / 100";
        GetComponent<AudioSource>().PlayOneShot(baseHurtAudio);
        HPbar.fillAmount -= .1f;
        if (myNumericalHealth <= 0)
        {
            FindObjectOfType<EnemySpawner>().stillAlive = false;
            defeat.enabled = true;
            worldFalls.enabled = true;
        }
    }
}
