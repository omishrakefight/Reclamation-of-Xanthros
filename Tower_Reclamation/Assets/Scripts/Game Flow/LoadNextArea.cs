using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextArea : MonoBehaviour {

    bool loadNextArea;
    EnemySpawner enemySpawner;

    [SerializeField] Button nextLevelButton;
    ChooseNextMissionPath nextPath;
    private bool pickedPath = false;

    [SerializeField] Text pickALane;

    Singleton singleton;
    SaveAndLoad save;
    int currentLevel;
    int timesClickedNextLevel = 0;

	// Use this for initialization
	void Start () {
        timesClickedNextLevel = 0;
        enemySpawner = FindObjectOfType<EnemySpawner>();
        singleton = Singleton.Instance;// FindObjectOfType<Singleton>().Ins;
        save = FindObjectOfType<SaveAndLoad>();
        try
        {
            pickALane.enabled = false;
        }
        catch (Exception e)
        {
            // nothing, if in a level, I dont have this.
        }
    }
	
	// Update is called once per frame
	void Update () {
       // if (enemySpawner.stillAlive == true && enemySpawner.level)
       // {

        //}
	}
    
    public void LoadNextAreaPostBattle(int level)
    {
        singleton.LevelCleared();
        
        switch (level)
        {
            case 1:
                save.LoadNewGameBase();
                break;

            default:               
                singleton.isHasLearnedATower = false;
                singleton.ishasLearnedTinker = false;
                singleton.isHasPickedAPath = false;
                save.LoadNewBase();
                break;
        }

        //SceneManager.LoadSceneAsync("_Scenes/_Base");
    }

    public void LoadNextLevelFromBase() // checks next level / wave HAS been chosen first.
    {
        //save on next wave start, that way they have the updated towers list saved for next base section.  Otherwise it wont treat them as learned.
        save.Save();

        //singleton = FindObjectOfType<Singleton>();
        nextPath = FindObjectOfType<ChooseNextMissionPath>();
        pickedPath = nextPath.isHasChosen;

        singleton.GetUpdateTinkerUpgrades();

        timesClickedNextLevel++;

        // they either need to do everything in base, or click to proceed 4 times.  More for me (developer) to skip
        if (CheckDoneInBase() || timesClickedNextLevel > 3)
        {
            //print("level is currently: " + FindObjectOfType<LevelTracker>().currentLevel);
            //FindObjectOfType<LevelTracker>().IncreaseLevel();
            //print("level is now : " + FindObjectOfType<LevelTracker>().currentLevel);

            currentLevel = singleton.level;

            SceneManager.LoadSceneAsync("_Scenes/Level_ " + currentLevel.ToString());
            nextLevelButton.enabled = false;
            //testing purposes
            Singleton.Instance.scenesChanged++;
        }
        else
        {
            StartCoroutine(YouMustPickAPathTextShowing());
        }
    }

    private bool CheckDoneInBase()
    {
        bool canProceed = true;
        pickALane.text = "";

        if (!singleton.isHasPickedAPath)
        {
            canProceed = false;
            pickALane.text += "You must pick a path! \n";
        }
        if (!singleton.isHasLearnedATower)
        {
            canProceed = false;
            pickALane.text += "You can learn a new tower! \n";
        }
        if (!singleton.ishasLearnedTinker)
        {
            canProceed = false;
            pickALane.text += "You can research a tinker upgrade! \n";
        }

        return canProceed;
    }

    public IEnumerator YouMustPickAPathTextShowing()
    {
        //Joey
        pickALane.enabled = true;
        yield return new WaitForSeconds(4);

        pickALane.enabled = false;
        yield break;
    }

    // get a co-routine going to display message for pick a lane.
}
