using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

    [Range(0f, 1.25f)]    [SerializeField]    float lightFluctuation;
    [Range(5f, 20f)] [SerializeField] float lightMax = 15;
    [SerializeField] Light lightToDim;

    [SerializeField] float period = 2.5f;


    float startingIntensity;
	// Use this for initialization
	void Start () {
        startingIntensity = lightToDim.intensity;
	}
	
	// Update is called once per frame
	void Update () {

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        lightFluctuation = rawSinWave / 2f +.5f;
        float dimmage = lightFluctuation * lightMax;
        lightToDim.intensity = startingIntensity + dimmage;

	}
}
