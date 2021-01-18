using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class TowerButton3 : MonoBehaviour {

    //[SerializeField] public Button button;
    [SerializeField] Text buttonName3;
    Singleton singleton;
    TowerFactory towerFactory;

    public void UpdateName()
    {
        buttonName3.text = (singleton.towerThreeBase.name + "   cost: " + singleton.towerThreeBase.GetTowerCost().ToString());
    }
    // Use this for initialization
    void Start()
    {
        towerFactory = FindObjectOfType<TowerFactory>();
        singleton = Singleton.Instance;
        //try
        //{
        //    buttonName3.text = singleton.towerThree.buttonName;
        //}
        //catch (Exception e)
        //{
        //    // no buttonName, then it is unassigned as of yet.
        //    buttonName3.text = "Unassigned";
        //}

        if (singleton.towerThreeBase != null)
        {
            buttonName3.text = (singleton.towerThreeBase.name + "   cost: " + singleton.towerThreeBase.GetTowerCost().ToString());
        }
        else
        {
            buttonName3.text = "Unassigned";
        }

    }

    public void BuildTower()
    {
        //towerFactory.AddTower(singleton.towerThree);
        towerFactory.CreateAndStackTower(singleton.towerThreeBase, singleton.towerThreeHead, singleton.towerThreeBaseType, singleton.towerThreeHeadType);

    }
}
