using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour {

    
    Vector3 snapPos;
    Waypoint waypoint;

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        SnapToGrid();
        UpdateLabel();

    }
    private void SnapToGrid()
    {
        int gridSize = waypoint.GetGridSize();
        transform.position = new Vector3(
            waypoint.GetGridPos().x * gridSize, 
            0f, 
            waypoint.GetGridPos().y * gridSize);
    }
    private void UpdateLabel()
    {
        string cubeLabel = (
            waypoint.GetGridPos().x) +
            ", " + 
            (waypoint.GetGridPos().y);
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = cubeLabel;

        gameObject.name = "Cube " + cubeLabel;
    }
}
