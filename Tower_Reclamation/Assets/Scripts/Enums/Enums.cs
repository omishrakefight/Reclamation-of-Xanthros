using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum Towers
{
    RifledTower = 0,
    AssaultTower = 1,
    FlameTower = 2,
    LighteningTower = 3,
    PlasmaTower = 4,
    SlowTower = 5
}

public enum Layer
{
    UI = 5,
    Tower = 8,
    Waypoint = 9,
    Enemy = 10,
    RaycastEndStop = -1

}

public enum LoadBase
{
    NewGame = 0,
    LoadANewBase = 1,
    LoadInUseBase = 2
}


public enum TowerCosts
{
    RifledTowerCost = 55,
    AssaultTowerCost = 50,
    FlameTowerCost = 60,
    LighteningTowerCost = 80,
    PlasmaTowerCost = 70,
    SlowTowerCost = 60
}

public enum TinkerUpgradePercent
{
    mark1 = 92,
    mark2 = 84,
    mark3 = 76,
    mark4 = 68
}

public enum TinkerUpgradeNumbers //silver o, alloy 1, pressurized tank 2, heavy shelling 3, tower engineer 4
{
    targettingModule = 0,
    alloyResearch = 1,
    pressurizedTank = 2,
    heavyShelling = 3,
    towerEngineer = 4
}

public enum FlameHead
{
    Basic = 0,
    FlameThrower = 1,
    Mortar = 2
}

public enum FlameBase //"Basic Base", "Tall Base", "Heavy Base", "Light Base", "Alien Base" };
{
    Basic = 0,
    Tall = 1,
    Heavy = 2,
    Light = 3,
    Alien = 4
}

public enum RifledHead
{
    Basic = 0,
    Sniper = 1
}

public enum RifledBase
{
    Basic = 0,
    Rapid = 1
}

public enum PlasmaHead
{
    Basic = 0,
    Crystal = 1
}

public enum PlasmaBase
{
    Basic = 0
}

public enum IceHead
{
    Basic = 0
}

public enum IceBase
{
    Basic = 0,
    Industrial = 1
}

public enum LightningHead
{
    Basic = 0,
    Static = 1

}

public enum LightningBase
{
    Basic = 0,
    Rapid = 1
}

public enum Enemies
{
    generic = 1,
    burrower = 2,
    roller = 3,
    doubles = 4,

    //specials
    slimer = 20,
    healer = 21
}

public enum Biomes
{
    Ice = 0,
    Volcanic = 1,
    Forest = 2
}
