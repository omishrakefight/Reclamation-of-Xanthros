using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using TMPro;
using System;

public class PostLevelSummaryScreen : MonoBehaviour {

    
    [SerializeField] TextMesh towerText; //TextMeshProUGUI this was from TMPro

    [SerializeField] Text proceedText;
    [SerializeField] Image loadingBar;
    Dictionary<string, PostGameObject> towerLogs = new Dictionary<string, PostGameObject>();
    [SerializeField] GameObject panel;
    private bool closing = false;
    AsyncOperation asyncOperation;

    // Use this for initialization
    void Start () {
        towerText.text = "";
        //proceedText.text = "space it out my man";
        proceedText.enabled = false;
        panel.SetActive(false);
    }

    public void TurnOnSummaryScreen()
    {
        panel.SetActive(true);
        PrintTowerInformation();
        GetComponentInChildren<Canvas>().overrideSorting = true;
        StartCoroutine(LoadScene());
    }

    /// <summary>
    /// This function prints out the tower damage and kills.
    /// </summary>
    private void PrintTowerInformation()
    {
        foreach (string tower in towerLogs.Keys)
        {
            int totalKills = 0;
            PostGameObject currentTower = towerLogs[tower];
            towerText.text += tower + ": \n";
            towerText.text += "\t Damage: " + currentTower.GetDamage() + " \n";
            towerText.text +=  "Enemies killed: \n";

            foreach (string enemy in currentTower.enemiesKilled.Keys)
            {
                towerText.text += "\t" + enemy + ":  " + currentTower.GetKills(enemy) + "\n";
                totalKills += currentTower.GetKills(enemy);
            }

            // if the tower has killed no enemies print NONE, else, give a total
            if (currentTower.enemiesKilled.Keys.Count == 0)
            {
                towerText.text += "\t NONE \n";
            }
            else
            {
                towerText.text += "Total kills: " + totalKills + "\n \n";
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (asyncOperation != null) {
            //if (asyncOperation.progress >= 0.9f)
            //{
            //    //Change the Text to show the Scene is ready
            //    proceedText.enabled = true;
            //    //Wait to you press the space key to activate the Scene
            //    if (Input.GetKeyDown(KeyCode.Space) && !closing)
            //    {
            //        //Activate the Scene
            //        //asyncOperation.allowSceneActivation = true;
            //        closing = true;
            //        FindObjectOfType<SaveAndLoad>().ClosedSummary(asyncOperation);
            //    }
            //}
        }
    }

    public void UpdateDamage(string tower, float damage)
    {
        if (towerLogs.ContainsKey(tower))
        {
            towerLogs[tower].AddDamage(damage);
        } else
        {
            PostGameObject newTower = new PostGameObject();
            newTower.AddDamage(damage);
            towerLogs.Add(tower, newTower);
        }
    }

    public void UpdateDamageAndKills(string tower, float damage, string enemy)
    {
        UpdateDamage(tower, damage);

        //if (towerLogs[tower].enemiesKilled.ContainsKey(enemy))
        //{
            towerLogs[tower].AddAKill(enemy);
        //}
        //else
        //{
        //    PostGameObject newTower = new PostGameObject();
        //    newTower.AddDamage(damage);
        //    towerLogs.Add(tower, newTower);
        //}
    }

    public void UpdateKills(string tower, string enemy)
    {
        if (towerLogs.ContainsKey(tower))
        {
            towerLogs[tower].AddAKill(enemy);
        }
        else
        {
            PostGameObject newTower = new PostGameObject();
            newTower.AddAKill(enemy);
            towerLogs.Add(tower, newTower);
        }
    }

    IEnumerator LoadScene()
    {
        //yield return null;

        //Begin to load the Scene you specify
        asyncOperation = SceneManager.LoadSceneAsync("_Scenes/_Base");
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            loadingBar.fillAmount = (asyncOperation.progress);

            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                proceedText.enabled = true;
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space) && !closing)
                {
                    //Activate the Scene
                    //asyncOperation.allowSceneActivation = true;
                    closing = true;
                    FindObjectOfType<SaveAndLoad>().ClosedSummary(asyncOperation);
                }
            }
            // Check if the load has finished


            yield return null;
        }
    }
}

