using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour {

    [SerializeField] float enemySpeed = 1.48f;

    int moveingBackwards = 0;

    int currentPathNode = 8;
    private bool stoppedSearching = false;
    private bool atTheEnd = false;


    void Start()
    {
        PathFinder pathFinder = FindObjectOfType<PathFinder>();
        var path = pathFinder.GivePath();
        transform.position = path[8].transform.position;


    }



    IEnumerator SearchingForMetal()
    {
        List<Waypoint> path = FindNextNode();

        float enemySpeedASecond = enemySpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, path[currentPathNode - 1].transform.position, enemySpeedASecond);

        if (transform.position == path[currentPathNode - 1].transform.position)
        {
             --currentPathNode;
           // ++moveingBackwards;
             yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (!stoppedSearching)
        {
            StartCoroutine(SearchingForMetal());
        } 
        //else
        if (FindObjectOfType<TutorialMissionStart>().timeToRun && !stoppedSearching)
        {
            StopCoroutine(SearchingForMetal());
            stoppedSearching = true;
        }
        if (FindObjectOfType<TutorialMissionStart>().timeToRun)
        {
            List<Waypoint> path = FindNextNode();
            float enemySpeedASecond = enemySpeed * Time.deltaTime * 5;
            transform.position = Vector3.MoveTowards(transform.position, path[currentPathNode + 1].transform.position, enemySpeedASecond);

            if (transform.position == path[currentPathNode + 1].transform.position)
            {
                if (transform.position == path[path.Count - 1].transform.position)
                {
                    if (atTheEnd)
                    {
                        return;
                    }
                    atTheEnd = true;
                    StartCoroutine(WaitForDoorsToOpen());
                    //Destroy(this.gameObject);
                }
                else
                {
                    ++currentPathNode;
                }
            }
        }
    }

    IEnumerator WaitForDoorsToOpen()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

        private List<Waypoint> FindNextNode()
    {
        PathFinder pathFinder = FindObjectOfType<PathFinder>();
        var path = pathFinder.GivePath();
        return path;
    }

}
