using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWonExit : MonoBehaviour
{

    float time = 0f;
    float clickRampupCounter = 5;

    IEnumerator end;
    [SerializeField] ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        end = DelayedStart();
        StartCoroutine(end);

        if (Input.GetMouseButtonUp(0))
        {
            SpawnBonusParticles();           
        }

        if (clickRampupCounter > 5)
        {
            //clickRampupCounter -= (1 * Time.deltaTime);
        }
    }

    private void SpawnBonusParticles()
    {
        clickRampupCounter += .5f;

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startColor = Color.red;
        emitParams.startSize = 0.2f;
        emitParams.startLifetime = 1.5f;
        
        particles.Emit(emitParams, (int)clickRampupCounter);
        
        //particles.Emit();
    }



    public IEnumerator DelayedStart()
    {
        time += (1 * Time.deltaTime);

        if(time > 8.0f)
        {
            Application.Quit(0);
            UnityEditor.EditorApplication.isPlaying = false;
        }

        yield return new WaitForSeconds(1f);
    }


}
