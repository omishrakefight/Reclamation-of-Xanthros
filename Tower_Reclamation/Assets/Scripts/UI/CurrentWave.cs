using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CurrentWave : MonoBehaviour {

    [SerializeField] public Text wave;
    [SerializeField] public int scoreCount = 0;
    public int waveCount = 1;

	// Use this for initialization
	void Start () {
        wave.text = "Wave : " + waveCount.ToString();
	}
	

    public void WaveUpOne()
    {
        ++waveCount;
        wave.text = "Wave : " + waveCount.ToString();
    }
}
