using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerFactory : MonoBehaviour {  

    [SerializeField] RifledTower rifledTowerPrefab;
    [SerializeField] RifledTower assaultTowerPrefab;
    [SerializeField] Tower_Flame flameTowerPrefab;
    [SerializeField] LighteningTower lighteningTowerPrefab;
    [SerializeField] Tower_Plasma plasmaTowerPrefab;
    [SerializeField] Transform towerParentTransform;

    [SerializeField] Tower rifledTowerBase;
    [SerializeField] GameObject rifledTowerHead;

    // For Lights and last waypoint
    [SerializeField] Waypoint lastWaypoint;
    Singleton singleton;


    private void Start()
    {
        singleton = Singleton.Instance;
        // this is how I will change the tower summons.
    }


    /// <summary>
    ///  Gold cost === how do I find a variable on an object not yet instantiated?  for finding tower gold cost.
    /// </summary>
    public void AddTower(Tower tower)
    {
        int currentGold = FindObjectOfType<GoldManagement>().CurrentGold();
        int cost = (int)FindGoldCost(tower);
        if (lastWaypoint.isAvailable && currentGold >= cost)
        {
            var newTower = Instantiate(tower, lastWaypoint.transform.position, Quaternion.identity);
            newTower.transform.parent = towerParentTransform;
            lastWaypoint.isAvailable = false;
            FindObjectOfType<GoldManagement>().TowerCost(cost);
            if (lastWaypoint.CompareTag("Buff Tile"))
            {
                newTower.TowerBuff();
            }
            //CheckWhichUpgradesAreApplicable(tower);
        }
        else
        {
            print("Unable to build here.");
        }
    }

    public void CreateAndStackTower(Tower towerBase, GameObject towerHead, int baseType, int headType)
    {
        int currentGold = FindObjectOfType<GoldManagement>().CurrentGold();
        int cost = (int)FindGoldCost(towerBase);
        if (lastWaypoint.isAvailable && currentGold >= cost)
        {
            bool buffTower = false;
            if (lastWaypoint.CompareTag("Buff Tile"))
            {
                buffTower = true;
            }
            GameObject newTower = StackTower(towerBase, towerHead, baseType, headType, buffTower);
            newTower.transform.parent = towerParentTransform;
            lastWaypoint.isAvailable = false;
            FindObjectOfType<GoldManagement>().TowerCost(cost);
            //CheckWhichUpgradesAreApplicable(tower);
        }
        else
        {
            print("Unable to build here.");
        }
    }

    private GameObject StackTower(Tower towerBase, GameObject towerHead, int baseType, int headType, bool bufftower)
    {
        var container = new GameObject();
        container.name = "Viewing Tower";
        container.transform.position = lastWaypoint.transform.position;

        float headHeight = ((towerBase.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.extents.y) * .95f); //This is to account for bigger meshes    // + (obj2.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y));
        var tBase = Instantiate(towerBase, lastWaypoint.transform.position, Quaternion.identity);
        // use this for the placement
        var tHead = Instantiate(towerHead, (lastWaypoint.transform.position + new Vector3(0, headHeight, 0)), Quaternion.identity); //new Vector3(0, headHeight, 0)
        tBase.transform.parent = container.transform;
        tHead.transform.parent = tBase.transform;

        if (bufftower)
        {
            tBase.TowerBuff();
        }
        tBase.DelayedStart(baseType, headType);
        tBase.DetermineTowerTypeBase(baseType);
        tBase.DetermineTowerHeadType(headType);

        //not needed in base but w/e
        tBase.SetHead(tHead.transform);

        return container;
    }

    
    public float FindGoldCost(Tower tower)
    {
        float cost = tower.GetTowerCost();
        
        return cost;
    }

    public void AddRifledTower()
    {
        int currentGold = FindObjectOfType<GoldManagement>().CurrentGold();

        // this was used to get access to inactive game objects, but upon load it was working correctly anyways.... w/e
        if (lastWaypoint.isAvailable && currentGold >= rifledTowerPrefab.goldCost)
        {
            var newTower = Instantiate(rifledTowerPrefab, lastWaypoint.transform.position, Quaternion.identity);
            newTower.transform.parent = towerParentTransform;
            lastWaypoint.isAvailable = false;
            FindObjectOfType<GoldManagement>().TowerCost(rifledTowerPrefab.goldCost);
            if (lastWaypoint.CompareTag("Buff Tile"))
            {
                newTower.TowerBuff();
            }
        }
        else
        {
            print("Unable to build here.");
        }
    }

    public void AddAssaultTower()
    {

        Tower towerBod = null;
        GameObject towerHead = null;
        int towerHeadInt = (int)RifledHead.Basic;
        int towerBaseInt = (int)RifledBase.Basic;
        //TowerSelecter towerSc = FindObjectOfType<TowerSelecter>();

        //towerSc.CheatGetRifledTowerPartsDefault(ref towerHead, ref towerBod, ref towerHeadInt, ref towerBaseInt);

        CreateAndStackTower(rifledTowerBase, rifledTowerHead, towerBaseInt, towerHeadInt);


        //int currentGold = FindObjectOfType<GoldManagement>().CurrentGold();
        //if (lastWaypoint.isAvailable && currentGold >= assaultTowerPrefab.goldCost)
        //{
        //    var newTower = Instantiate(assaultTowerPrefab, lastWaypoint.transform.position, Quaternion.identity);
        //    newTower.transform.parent = towerParentTransform;
        //    lastWaypoint.isAvailable = false;
        //    print("hi" + GetComponentInChildren<RifledTower>(true).goldCost);
        //    FindObjectOfType<GoldManagement>().TowerCost(assaultTowerPrefab.goldCost);
        //    if (lastWaypoint.CompareTag("Buff Tile"))
        //    {
        //        newTower.TowerBuff();
        //    }
        //}
        //else
        //{
        //    print("Unable to build here.");
        //}
    }

    public void AddLighteningTower()
    {
        int currentGold = FindObjectOfType<GoldManagement>().CurrentGold();
        if (lastWaypoint.isAvailable && currentGold >= lighteningTowerPrefab.goldCost)
        {
            var newTower = Instantiate(lighteningTowerPrefab, lastWaypoint.transform.position, Quaternion.identity);
            newTower.transform.parent = towerParentTransform;
            lastWaypoint.isAvailable = false;
            FindObjectOfType<GoldManagement>().TowerCost(lighteningTowerPrefab.goldCost);
            if (lastWaypoint.CompareTag("Buff Tile"))
            {
                newTower.TowerBuff();
            }
        }
        else
        {
            print("Unable to build here.");
        }
    }

    public void AddPlasmaTurret()
    {
        int currentGold = FindObjectOfType<GoldManagement>().CurrentGold();
        if (lastWaypoint.isAvailable && currentGold >= plasmaTowerPrefab.goldCost)
        {
            var newTower = Instantiate(plasmaTowerPrefab, lastWaypoint.transform.position, Quaternion.identity);
            newTower.transform.parent = towerParentTransform;
            lastWaypoint.isAvailable = false;
            FindObjectOfType<GoldManagement>().TowerCost(plasmaTowerPrefab.goldCost);
            if (lastWaypoint.CompareTag("Buff Tile"))
            {
                newTower.TowerBuff();
            }
        }
        else
        {
            print("Unable to build here.");
        }
    }

    public void AddFlameTower()
    {
        int currentGold = FindObjectOfType<GoldManagement>().CurrentGold();
        if (lastWaypoint.isAvailable && currentGold >= flameTowerPrefab.goldCost)
        {
            var newTower = Instantiate(flameTowerPrefab, lastWaypoint.transform.position, Quaternion.identity);
            newTower.transform.parent = towerParentTransform;
            lastWaypoint.isAvailable = false;
            FindObjectOfType<GoldManagement>().TowerCost(flameTowerPrefab.goldCost);
            if (lastWaypoint.CompareTag("Buff Tile"))
            {
                 newTower.TowerBuff();
            }
        }
        else
        {
            print("Unable to build here.");
        }
    }


    public void LastWaypointClicked(Waypoint waypoint)
    {
        lastWaypoint = waypoint;

    }


}

//Button baseTowerButton = Canvas.FindObjectOfType<Button>();
//baseTowerButton.transform.position = this.transform.position;