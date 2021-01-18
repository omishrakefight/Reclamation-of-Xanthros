using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerButton1 : MonoBehaviour {

    //[SerializeField] public Button button;
    [SerializeField] Text buttonName1;
    Singleton singleton;
    TowerFactory towerFactory;

    public void UpdateName()
    {
        towerFactory = FindObjectOfType<TowerFactory>();
        buttonName1.text = (singleton.towerOneBase.name + "   cost: " + singleton.towerOneBase.GetTowerCost().ToString());
    }
    // Use this for initialization
    void Start()
    {
        towerFactory = FindObjectOfType<TowerFactory>();
        singleton = Singleton.Instance;
        if (singleton.towerOneBase != null)
        {
            buttonName1.text = (singleton.towerOneBase.name + "   cost: " + singleton.towerOneBase.GetTowerCost().ToString());
        } else
        {
            // first button defaults to rifled tower
            buttonName1.text = "Basic Rifled Tower: 50";
            //buttonName1.text = "Unassigned";
        }
    }

    public void BuildTower()
    {
        //singleton = FindObjectOfType<Singleton>();
        //towerFactory.AddTower(singleton.towerOne);
        towerFactory.CreateAndStackTower(singleton.towerOneBase, singleton.towerOneHead, singleton.towerOneBaseType, singleton.towerOneHeadType);

    }
}
