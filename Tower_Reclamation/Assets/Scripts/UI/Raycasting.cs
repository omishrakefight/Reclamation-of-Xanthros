using UnityEngine;
using System;

public class Raycasting : MonoBehaviour {


    public Layer[] layerPriorities =
    {
    Layer.UI,
    Layer.Enemy,
    Layer.Tower,
    Layer.Waypoint
    };

    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;
    Singleton singleton;


    RaycastHit raycastHit;
    public RaycastHit hit
    {
        get { return raycastHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }

    public delegate void OnLayerChange();  //  declare the delegate type
    public OnLayerChange layerChangeObservers;

    private void Start()
    {
        singleton = Singleton.Instance;
        viewCamera = Camera.main;
    }

    void Update()
    {
        // on raycasting, if the click is on an enemy send it to the singleton to disperse to the towers.
        // hsould be after the foreach, but i had trouble and it will be rare someone will click the same frame.
        //TODO move all my click crap somehow here? consolidate code.  Waypoint clicks maybe.

        //if(Input.bu) /// onbutton up do the below? on button down if on a tower set bool to setting individual enemy.
        //if (Input.GetMouseButtonDown(0))
        if(Input.GetMouseButtonUp(0))
        {
            try
            {
                // make it a switch?
                //switch ()
                //{

                //}
                if (raycastHit.collider.GetComponentInChildren<EnemyHealth>() != null)
                {
                    singleton.SetPreferedEnemy(raycastHit.collider.GetComponentInChildren<EnemyHealth>());
                }
                else if (raycastHit.collider.gameObject.layer.Equals(5))
                {

                }
                else if (raycastHit.collider.GetComponentInParent<Tower>() != null)
                {
                    var sightRange = FindObjectOfType<TowerUpgradeAndRangeSight>();
                    if (sightRange != null)
                    {
                        sightRange.gameObject.SetActive(true);
                        sightRange.ShowInfoPanel();
                        sightRange.InitializeFromTower(raycastHit.collider.GetComponentInParent<Tower>());
                        sightRange.CreatePoints(raycastHit.collider.GetComponentInParent<Tower>());
                    }
                }
                else
                {
                    var sightRange = FindObjectOfType<TowerUpgradeAndRangeSight>();
                    if (sightRange != null)
                    {
                        sightRange.DestroyRangeCircle();
                        //sightRange.HideInfoPanel();
                    }
                }
            }
            catch (Exception clickError)
            {
                print("Error when clicking : " + clickError.Message);
            }
            //preferedTargetEnemy = raycastHit.collider.GetComponentInChildren<EnemyHealth>();
        }

        // Look for and return priority layer hit
        try
        {
            foreach (Layer layer in layerPriorities)
            {
                var hit = RaycastForLayer(layer);
                if (hit.HasValue)
                {
                    raycastHit = hit.Value;
                    if (layerHit != layer) // if layer has changed
                    {
                        layerHit = layer;
                        layerChangeObservers(); // call the delegates
                    }
                    layerHit = layer;
                    return;
                }
            }
        }
        catch (Exception raycastEx)
        {
            print("Error with raycast (moving mouse): " + raycastEx.Message);
        }

        //FindObjectOfType<CursorIcons>().PrintLayerHit();
        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;
    }



    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer;
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }


    //private void OnMouseDown()
    //{
    //    print(hit.collider.name);
    //}
}







/*
private void Start()
{
    viewCamera = Camera.main;
    layerChangeObservers += SomeLayerChangeHandler;
    layerChangeObservers();
}

public Layer[] layerPriorities =
{
    Layer.Enemy,
    Layer.Tower,
    Layer.Waypoint
};

[SerializeField] float distanceToBackground = 100f;
Camera viewCamera;

RaycastHit m_hit;
public RaycastHit Hit
{
    get { return m_hit; }
}

Layer m_layerHit; 
public Layer LayerHit
{
    get { return m_layerHit;  }
}



public delegate void OnLayerChange();  //  declare the delegate type
public OnLayerChange layerChangeObservers;

void SomeLayerChangeHandler()
{
    print("I handled it");
}


private void OnMouseDown()
{
    foreach (Layer layer in layerPriorities)
    {
        var Hit = RaycastForLayer(layer);
        if (Hit.HasValue)
        {
            m_hit = Hit.Value;
            m_layerHit = layer;
            print(m_hit);
            return;
        }
    }

    m_hit.distance = distanceToBackground;
    m_layerHit = Layer.RaycastEndStop;
    FindObjectOfType<CursorIcons>().PrintLayerHit();
} 


private void Update()
{
    foreach (Layer layer in layerPriorities)
    {
        var Hit = RaycastForLayer(layer);
        if (Hit.HasValue)
        {
            m_hit = Hit.Value;
            m_layerHit = layer;
            return;
        }
    }

    m_hit.distance = distanceToBackground;
    m_layerHit = Layer.RaycastEndStop;
    FindObjectOfType<CursorIcons>().PrintLayerHit();
}

RaycastHit? RaycastForLayer(Layer layer)
{
    int layerMask = 1 << (int)layer;
    Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

    RaycastHit hit;
    bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
    if (hasHit)
    {
        return hit;
    }
    return null;
}
} */
