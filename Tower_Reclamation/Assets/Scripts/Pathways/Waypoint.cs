using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour {

    public bool isSlimed = false;

    //  Public because it is a data structure.
    public bool isnotExplored = true;
    public Waypoint ExploredFrom;

    Vector2Int gridPos;
    const int gridSize = 10;

    //  For Lights
    [SerializeField] Waypoint lastWaypoint;
    [SerializeField] Light waypointSpotLight;
    static Light currentWaypointLight;
    static float lightIntensity;
    static bool madeLight = false;

    // tower placer
    public bool isPlaceable = true;
    public bool isAvailable = true;

    private void Start()
    {
        madeLight = false;
    }

    public int GetGridSize()
    {
        return gridSize;
    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.z / gridSize)
            );
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(madeLight)
            currentWaypointLight.GetComponent<Light>().intensity = 0;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isPlaceable)
            {
                FindObjectOfType<TowerFactory>().LastWaypointClicked(this);

                Vector3 lightHeightAdjustment = new Vector3(0f, 16f, 0);
                if (!madeLight)
                {
                    currentWaypointLight = Instantiate(waypointSpotLight, this.transform.position + lightHeightAdjustment, Quaternion.Euler(90, 0, 0));
                    madeLight = true;
                    lightIntensity = currentWaypointLight.GetComponent<Light>().intensity;
                }
                else
                {
                    currentWaypointLight.transform.position = this.transform.position + lightHeightAdjustment;
                    currentWaypointLight.GetComponent<Light>().intensity = lightIntensity;
                }
            }
        }  
    }
}
