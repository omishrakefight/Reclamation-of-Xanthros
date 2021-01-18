using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalWave : MonoBehaviour {

    string youHaveWon = "You destroyed the giant alien!! and the small ones scattered to the winds, to slowly die out.  Xanthros " +
        "has been reclaimed.  Congratulations!!";
    [SerializeField] Text displayText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // roundabout still calls twice for the delay in enemyspawner.
    public void DisplayWinningCongratulations()
    {
        displayText.text = youHaveWon;

    }

}
