using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryGateExit1 : MonoBehaviour {


    [SerializeField] Text text;
    string talking;
    List<string> conversations;
    int conversationTracker = 0;
    int conversationLimit = 0;

    [SerializeField] Canvas talkingCanvas;

    public bool timeToRun = false;
    private bool spawnEnemies = true;

    [SerializeField] RawImage personTalking;
    [SerializeField] GameObject fadeCube;

    Scene t1_currentScene;
    Scene t1_nextScene;
    bool t1_sceneSwapTime = false;

    [Header("Soldier")]
    [SerializeField]
    Texture soldierNeutral;
    [SerializeField] Texture soldierScared;

    [Header("General")]
    [SerializeField]
    Texture general;
    [SerializeField] Texture generalShouting;



    string string0;
    string string1;
    string string2;
    string string3;
    string string4;
    string string5;

    // Use this for initialization
    void Start()
    {
        t1_currentScene = SceneManager.GetActiveScene();
        StartCoroutine(DetermineConversations_AndNextArea());


        // initializing strings
        text.text = "";
        string0 = "Ok people, we need to get out there and scavenge some metal.  That last wandering group of bugs damaged " +
            "the gate a bit.  Let's bring some back and repair before more arrive.";
        string1 = " Yes sir.  I’ll head out now.";
        string2 = "2";
        string3 = "3";
        string4 = "4";
        conversations = new List<string>();
        conversations.AddRange(new string[] { string0, string1, string2, string3, string4 });
        conversationTracker = 0;


        StartCoroutine(SlowMessageTyping());
    }

    IEnumerator DetermineConversations_AndNextArea()
    {
        if (t1_currentScene.name == "Base Exit Doorway")
        {
            conversationTracker = 0;
            conversationLimit = 2;
            if (t1_sceneSwapTime)
            {
                //FindObjectOfType<SaveAndLoad>().test();
                //AsyncOperation loadingBase;
                //loadingBase = SceneManager.LoadSceneAsync("_Scenes/Level_ 1");

                //while (!loadingBase.isDone)
                //{
                //    yield return new WaitForSeconds(.50f);
                //}
                //yield return new WaitForSeconds(2.0f);
                //print("I waited?");
                SceneManager.LoadSceneAsync("_Scenes/Level_ 1");
            }
            t1_sceneSwapTime = true;
        }
        yield break;
    }


    private void TutorialSpeech()
    {
        text.text = "";
        SlowMessageTyping();

    }

    IEnumerator ConversationPicker()
    {
        if (conversationTracker == 0)
        {
            personTalking.texture = general;
            talking = conversations[conversationTracker];
        }
        if (conversationTracker == 1)
        {
            personTalking.texture = soldierNeutral;
            talking = conversations[conversationTracker];
        }
        if (conversationTracker == 2)
        {
            personTalking.texture = soldierNeutral;
            talking = conversations[conversationTracker];
        }
        if (conversationTracker == 3)
        {
            personTalking.texture = generalShouting;
            talking = conversations[conversationTracker];
        }
        if (conversationTracker == 4)
        {
            // To make the character run in scene
            timeToRun = true;

            personTalking.texture = soldierScared;
            talking = conversations[conversationTracker];
        }

        conversationTracker++;
        yield break;
    }
    IEnumerator SlowMessageTyping()
    {
        StartCoroutine(ConversationPicker());
        text.text = "";
        SpawnTheEnemiesAtScreem();
        // Loop the converstation 1 char at a time.
        for (int i = 0; i < talking.Length; i++)
        {
            var letter = talking.ToCharArray(i, 1);
            text.text += new string(letter);
            yield return new WaitForSeconds(.02f);
        }
        yield return new WaitForSeconds(2);
        if (conversationTracker < conversationLimit)
        {
            //print(conversationTracker);
            yield return StartCoroutine(SlowMessageTyping());
        }


        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        // fadeCube.SetActive(true);
        FindObjectOfType<FadeScript>().FadeIn();
        yield return new WaitForSeconds(FindObjectOfType<FadeScript>().fadeTime);
        yield return StartCoroutine(DetermineConversations_AndNextArea());
    }
    private void SpawnTheEnemiesAtScreem()
    {
        if (conversationTracker == conversations.Count && spawnEnemies)
        {
            //StartCoroutine(FindObjectOfType<EnemySpawner>().ContinualSpawnEnemies());
            FindObjectOfType<EnemySpawner>().StartBattle();
            spawnEnemies = false;
        }
    }



}
      ////////////////////////// Divisor /////////////////////////////////////
/*
{

    [SerializeField] Text text;
string talking;
List<string> conversations;
int conversationTracker = 0;
int conversationLimit = 0;

[SerializeField] Canvas talkingCanvas;

public bool timeToRun = false;
private bool spawnEnemies = true;

[SerializeField] RawImage personTalking;

Scene t1_currentScene;
Scene t1_nextScene;
bool t1_sceneSwapTime = false;

[Header("Soldier")]
[SerializeField]
Texture soldierNeutral;
[SerializeField] Texture soldierScared;

[Header("General")]
[SerializeField]
Texture general;
[SerializeField] Texture generalShouting;



string string0;
string string1;
string string2;
string string3;
string string4;
string string5;

// Use this for initialization
void Start()
{
    t1_currentScene = SceneManager.GetActiveScene();
    DetermineConversations();

    // initializing strings
    string0 = "Ok people, we need to get out there and scavenge some metal.  That last wandering group of bugs damaged " +
        "the gate a bit.  Let's bring some back and repair before more arrive.";
    string1 = " Yes sir.  I’ll head out now.";
    string2 = "Sir!!!  There's almost nothing left! I can only find one good piece, we’ve already scavenged most of the metal here.";
    string3 = "SHUT UP! THE BUGS ARE BACK, GET BACK HERE NOW!!";
    string4 = "?!?!?!?? HNNGGG";
    conversations = new List<string>();
    conversations.AddRange(new string[] { string0, string1, string2, string3, string4 });
    conversationTracker = 0;


    StartCoroutine(TextTyper());
}

private void DetermineConversations()
{
    if (t1_currentScene.buttonName == "Base Exit Doorway")
    {
        conversationTracker = 0;
        conversationLimit = 2;
        if (t1_sceneSwapTime)
        {
            SceneManager.LoadSceneAsync("Level_1");
        }
        t1_sceneSwapTime = true;
    }
}

IEnumerator TextTyper()
{

    text.text = "";
    talking = "Welcome, Commander.  It is great to see that you have yet lived you awesome son of a bitch!!";
    StartCoroutine(SlowMessageTyping());

    yield break;
}

private void TutorialSpeech()
{
    text.text = "";
    SlowMessageTyping();

}

IEnumerator ConversationPicker()
{
    if (conversationTracker == 0)
    {
        personTalking.texture = general;
        talking = conversations[conversationTracker];
    }
    if (conversationTracker == 1)
    {
        personTalking.texture = soldierNeutral;
        talking = conversations[conversationTracker];
    }
    if (conversationTracker == 2)
    {
        personTalking.texture = soldierNeutral;
        talking = conversations[conversationTracker];
    }
    if (conversationTracker == 3)
    {
        personTalking.texture = generalShouting;
        talking = conversations[conversationTracker];
    }
    if (conversationTracker == 4)
    {
        // To make the character run in scene
        timeToRun = true;

        personTalking.texture = soldierScared;
        talking = conversations[conversationTracker];
    }

    conversationTracker++;
    yield break;
}
IEnumerator SlowMessageTyping()
{
    StartCoroutine(ConversationPicker());
    text.text = "";
    SpawnTheEnemiesAtScreem();
    // Loop the converstation 1 char at a time.
    for (int i = 0; i < talking.Length; i++)
    {
        var letter = talking.ToCharArray(i, 1);
        text.text += new string(letter);
        yield return new WaitForSeconds(.02f);
    }
    yield return new WaitForSeconds(3);
    if (conversationTracker < conversationLimit)
    {
        //print(conversationTracker);
        yield return StartCoroutine(SlowMessageTyping());
    }



    StartCoroutine(DisableText());
}

private void SpawnTheEnemiesAtScreem()
{
    if (conversationTracker == conversations.Count && spawnEnemies)
    {
        StartCoroutine(FindObjectOfType<EnemySpawner>().ContinualSpawnEnemies());
        spawnEnemies = false;
    }
}

private IEnumerator DisableText()
{
    yield return new WaitForSeconds(4);
    talkingCanvas.enabled = false;
}


// Update is called once per frame
void Update()
{

}
}
*/