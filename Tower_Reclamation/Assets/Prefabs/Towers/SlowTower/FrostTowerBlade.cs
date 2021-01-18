using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTowerBlade : MonoBehaviour {

    [SerializeField] public float rotationSpeed = 280f;
    // Use this for initialization
    void Start () {
        rotationSpeed = 280f;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        //this.transform.rotation = new Vector3(0f, (90f * Time.deltaTime), 0f);
	}
}
