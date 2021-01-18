using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TowerButton2 : MonoBehaviour {

    //[SerializeField] public Button button;
    [SerializeField] Text buttonName2;
    Singleton singleton;
    TowerFactory towerFactory;

    GameObject container;
    [SerializeField] Tower towerBase;
    [SerializeField] GameObject towerHead;

    public void UpdateName()
    {
        buttonName2.text = (singleton.towerTwoBase.name + "   cost: " + singleton.towerTwoBase.GetTowerCost().ToString());
    }
    // Use this for initialization
    void Start()
    {
        towerFactory = FindObjectOfType<TowerFactory>();
        singleton = Singleton.Instance;
        if (singleton.towerTwoBase != null)
        {
            buttonName2.text = (singleton.towerTwoBase.name + "   cost: " + singleton.towerTwoBase.GetTowerCost().ToString());
        }
        else
        {
            buttonName2.text = "Unassigned";
        }
    }

    public void TestInstantiationUnderObj()
    {
        container = new GameObject();
        container.name = buttonName2.text;

        //  FOR FUTURE  
        // maybe do a case statement, that returns a vector 3.  This fills in the instantiation place.
        // this can be done with the generic being the 'head' location.  This case, though, allows for more tower combinations. 
        // IE this case could supply 'back attachment' for lightening tower.  That or I could make them 1 part only, light towers are full peices only?


        // FOR NOW  I could go into the Tower Selecter and make that one function first.  I get info from there, so it needs to work first (also fastest to test.
        // I CAN hardcode stuff i dont have yet with this hack.  For slow and light tower, put it as an empty object.  It will get added, not throw an excepttion  AND be invisible and take low power.
        float headHeight = ((towerBase.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y) * .94f); //This is to account for bigger meshes    // + (obj2.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y));
        //Instantiate(container, new Vector3(0, 0, 0), Quaternion.identity);
        var tBase = Instantiate(towerBase, new Vector3(0,0,0), Quaternion.identity);
        var tHead = Instantiate(towerHead, new Vector3(0, headHeight, 0), Quaternion.identity);


        tHead.transform.parent = tBase.transform;
        tBase.transform.parent = container.transform;

        tBase.SetHead(tHead.transform);


        //container.ad
    }

    public void BuildTower()
    {
        //towerFactory.AddTower(singleton.towerTwo);
        towerFactory.CreateAndStackTower(singleton.towerTwoBase, singleton.towerTwoHead, singleton.towerTwoBaseType, singleton.towerTwoHeadType);

    }
}

