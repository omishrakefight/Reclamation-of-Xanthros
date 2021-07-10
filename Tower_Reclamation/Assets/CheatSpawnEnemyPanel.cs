using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class CheatSpawnEnemyPanel : MonoBehaviour
{
    [SerializeField] Toggle volcanicEnhancement;
    [SerializeField] Toggle forestEnhancement;
    [SerializeField] InputField moneyToAdd;
    [SerializeField] InputField upgradesToAdd;


    [SerializeField] GameObject MySelf;

    protected EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney()
    {
        try
        {
            int money = int.Parse(moneyToAdd.text);
            GoldManagement goldM = FindObjectOfType<GoldManagement>();
            goldM.AddGold(money);
        }
        catch (Exception BadParse)
        {
            print("failed addMoney parse");
        }
    }

    public void AddUpgradeParts()
    {
        try
        {
            int money = int.Parse(moneyToAdd.text);
            GoldManagement goldM = FindObjectOfType<GoldManagement>();
            goldM.AddUpgradeParts(money);
        }
        catch (Exception BadParse)
        {
            print("failed addMoney parse");
        }
    }

    public void SetMyselfActive()
    {
        MySelf.SetActive(true);
    }

    public void SpawnTankEnemy()
    {
        //if (enemySpawner == null)
        //{
        //    enemySpawner = FindObjectOfType<EnemySpawner>();
        //}
        bool volcanicBuff = volcanicEnhancement.isOn;
        bool forestBuff = forestEnhancement.isOn;

        enemySpawner.CheatSpawnTank(volcanicBuff, forestBuff);
    }

    public void SpawnBurrowerEnemy()
    {
        bool volcanicBuff = volcanicEnhancement.isOn;
        bool forestBuff = forestEnhancement.isOn;

        enemySpawner.CheatSpawnBurrower(volcanicBuff, forestBuff);
    }

    public void SpawnRollerEnemy()
    {
        bool volcanicBuff = volcanicEnhancement.isOn;
        bool forestBuff = forestEnhancement.isOn;

        enemySpawner.CheatSpawnRoller(volcanicBuff, forestBuff);
    }

    public void SpawnDoublesEnemy()
    {
        bool volcanicBuff = volcanicEnhancement.isOn;
        bool forestBuff = forestEnhancement.isOn;

        enemySpawner.CheatSpawnDouble(volcanicBuff, forestBuff);
    }

    public void SpawnSlimerEnemy()
    {
        bool volcanicBuff = volcanicEnhancement.isOn;
        bool forestBuff = forestEnhancement.isOn;

        enemySpawner.CheatSpawnSlimer(volcanicBuff, forestBuff);
    }

    public void SpawnHealerEnemy()
    {
        bool volcanicBuff = volcanicEnhancement.isOn;
        bool forestBuff = forestEnhancement.isOn;

        enemySpawner.CheatSpawnHealer(volcanicBuff, forestBuff);
    }





    public void Close()
    {
        MySelf.SetActive(false);
    }
}
