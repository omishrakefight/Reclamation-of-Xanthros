using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWonExit : MonoBehaviour
{

    float time = 0f;
    IEnumerator end;
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
