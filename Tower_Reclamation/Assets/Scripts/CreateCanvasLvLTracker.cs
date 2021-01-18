using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCanvasLvLTracker : MonoBehaviour {

    [SerializeField] Canvas lvlTrackingPrefab;
	// Use this for initialization
	void Start () {
		if (!FindObjectOfType<LevelTracker>())
        {
            Instantiate(lvlTrackingPrefab);
        }
        else
        {
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
