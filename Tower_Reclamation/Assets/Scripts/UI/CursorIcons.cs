using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorIcons : MonoBehaviour {

    [SerializeField] Texture2D regularCursor = null;
    [SerializeField] Texture2D enemyCursor = null;
    [SerializeField] Texture2D towerCursor = null;
    [SerializeField] Texture2D waypointCursor = null;

    [SerializeField] Vector2 cursorHotspot = new Vector2(96, 96);
    Vector2 cursorOutOfBoundsHotspot = new Vector2(48, 48);


    Raycasting raycasting;
	// Use this for initialization
	void Start () {
        raycasting = GetComponent<Raycasting>();
        raycasting.layerChangeObservers += SetCursorOnLayerChange;
	}
	
	// Update is called once per frame
	void Update () {

        //switch (raycasting.currentLayerHit)
        //{
        //    case Layer.Enemy:
        //        Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
        //        break;
        //    case Layer.Tower:
        //        Cursor.SetCursor(towerCursor, cursorHotspot, CursorMode.Auto);
        //        break;
        //    case Layer.Waypoint:
        //        Cursor.SetCursor(waypointCursor, cursorHotspot, CursorMode.Auto);
        //        break;
        //    case Layer.RaycastEndStop:
        //        Cursor.SetCursor(regularCursor, cursorOutOfBoundsHotspot, CursorMode.Auto);
        //        break;
        //    default:
        //        Cursor.SetCursor(regularCursor, cursorOutOfBoundsHotspot, CursorMode.Auto);
        //        //Debug.LogError("Error in cursorIcons script targetting");
        //        break;
        //}
    }

    public void PrintLayerHit()
    {
        print(raycasting.currentLayerHit);
    }

    // Delegate listener
    void SetCursorOnLayerChange()
    {
       // /*
        switch (raycasting.currentLayerHit)
        {
            case Layer.Enemy:
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Tower:
                Cursor.SetCursor(towerCursor, cursorHotspot, CursorMode.Auto);
                    break;
            case Layer.Waypoint:
                Cursor.SetCursor(waypointCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(regularCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(regularCursor, cursorHotspot, CursorMode.Auto);
                Debug.LogError("Error in cursorIcons script targetting");
                break;
        } //*/
    }
}
