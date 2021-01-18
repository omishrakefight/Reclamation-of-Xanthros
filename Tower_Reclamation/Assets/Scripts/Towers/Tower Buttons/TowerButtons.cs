using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtons : MonoBehaviour {

    [SerializeField] public Button button;
    [SerializeField] Text buttonName;
    Singleton singleton;
    
    public void UpdateName()
    {
        buttonName.text = singleton.towerOne.name;
    }
	// Use this for initialization
	void Start () {
        singleton = Singleton.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
