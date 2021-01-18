using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;
using System;

public class GoldManagement : MonoBehaviour {

    //inside enemy health dmg
    [SerializeField] public float goldCount = 150f;
    public float upgradeCount = 0f;
    private bool setGoldManually = false;
    public Text gold;
    [SerializeField] public Text parts;
    [SerializeField] public Text partsPerIncrementText;
    [SerializeField] public Text goldPerIncrementText;
    float goldTimer = 0f;
    float goldInterval = 5f;
    int baseIncreaseInterval = 4;
    bool roundStarted = false;
    [SerializeField] Slider towerAndUpgradePartsSlider;
    [SerializeField] Slider towerAndUpgradeTimer;
    [SerializeField] Image towerAndUpgradeTimerI;


    // Use this for initialization
    void Start () {
        if (!setGoldManually)
        {
            goldCount = 150;
        }
        GoldCounter();
    }
	
	// Update is called once per frame
	void Update () {
        //GoldCounter();
        if (roundStarted) // set in tutrial and normal lvls
        {
            goldTimer += Time.deltaTime;
            //towerAndUpgradeTimer.value = (goldTimer / goldInterval);
            towerAndUpgradeTimerI.fillAmount = (goldTimer / goldInterval);
        }

        if (goldTimer > goldInterval)
        {
            goldTimer -= goldInterval;
            //AddGold();
            //towerAndUpgradeTimer.value = 0f;
            towerAndUpgradeTimerI.fillAmount = 0f;
            AddTowerAndUpgradeParts();
        }
    }

    private void AddTowerAndUpgradeParts()
    {
        switch ((int)towerAndUpgradePartsSlider.value)
        {
            case 0:
                AddGold(baseIncreaseInterval * 1.75f);
                AddUpgradeParts(baseIncreaseInterval * 0);
                break;
            case 1:
                AddGold(baseIncreaseInterval * 1.5f);
                AddUpgradeParts(baseIncreaseInterval * .5f);
                break;
            case 2:
                AddGold(baseIncreaseInterval);
                AddUpgradeParts(baseIncreaseInterval);
                break;
            case 3:
                AddGold(baseIncreaseInterval * .5f);
                AddUpgradeParts(baseIncreaseInterval * 1.5f);
                break;
            case 4:
                AddGold(baseIncreaseInterval * 0);
                AddUpgradeParts(baseIncreaseInterval * 1.75f);
                break;
        }
    }

    public void Started()
    {
        roundStarted = true;
    }

    public void SliderChangeResetValuesForGoldAndParts()
    {
        switch ((int)towerAndUpgradePartsSlider.value)
        {
            case 0:
                ChangeGoldIncrementText((int)(baseIncreaseInterval * 1.75f));
                ChangeUpgradeIncrementText(baseIncreaseInterval * 0);
                break;
            case 1:
                ChangeGoldIncrementText((int)(baseIncreaseInterval * 1.5f));
                ChangeUpgradeIncrementText((int)(baseIncreaseInterval * .5f));
                break;
            case 2:
                ChangeGoldIncrementText((int)(baseIncreaseInterval));
                ChangeUpgradeIncrementText((int)(baseIncreaseInterval));
                break;
            case 3:
                ChangeGoldIncrementText((int)(baseIncreaseInterval * .5f));
                ChangeUpgradeIncrementText((int)(baseIncreaseInterval * 1.5f));
                break;
            case 4:
                ChangeGoldIncrementText((int)(baseIncreaseInterval * 0));
                ChangeUpgradeIncrementText((int)(baseIncreaseInterval * 1.75f));
                break;
        }
    }

    public void ChangeGoldIncrementText(int newValue)
    {
        goldPerIncrementText.text = "+" + newValue;
    }
    public void ChangeUpgradeIncrementText(int newValue)
    {
        partsPerIncrementText.text = "+" + newValue;
    }

    public void AddExtraGoldTimer(float time)
    {
        goldTimer += time; // actually I can let it add frame by frame in update...

        //int divisions = (int)(goldTimer / 2f);

        //AddGold(divisions);

        //goldTimer -= (2 * divisions);
    }

    public void SetGoldAmount(int newAmount)
    {
        goldCount = newAmount;
        setGoldManually = true;
        GoldCounter();
    }

    public void AddGold(float money)
    {
        // maybe do this so that goldcount is float, but onyl displays int? round to 2?
        goldCount = goldCount + money;
        GoldCounter();
    }

    public void AddUpgradeParts(float parts)
    {
        upgradeCount += Mathf.RoundToInt(parts);
        PartsCounter();
    }

    private void PartsCounter()
    {
        parts.text = "Upgrades :  " + Mathf.RoundToInt(upgradeCount);
    }

    public void GoldCounter()
    {
        gold.text = "Gold :  " + (int)goldCount;
    }

    public int CurrentGold()
    {
        return (int)goldCount;
    }
    // change this to take in an int and minus tower cost (input int)
    public void TowerCost(int towerCost)
    {
        goldCount -= towerCost;
        //print("i just bough a tower i am now " + goldCount);
        GoldCounter();
    }

    public void UpgradeCost(int upgradeCost)
    {
        upgradeCount -= upgradeCost;
        PartsCounter();
    }
}
