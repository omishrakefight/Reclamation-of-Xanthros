using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TutorialMissionStart : MonoBehaviour {
    [SerializeField] Text text;
    [SerializeField] string talking;
    [SerializeField] MoreInformationPanel moreInformationPrompt;
    MoreInformationPanel prompt;
    List<string> conversations;
    int conversationTracker = 0;

    [SerializeField] GameObject TalkingCanvas;
    [SerializeField] Canvas talkingCanvas;
    [SerializeField] RawImage personTalking;

    public bool timeToRun = false;
    private bool spawnEnemies = true;
    private bool isLastChatSegment = false;
    private bool isEnableingChatTurnOff = false;

    [SerializeField] float typingSpeed = .02f;

    // kinda dumb but I dont have another way to spawn text off enemy spawns.  This way I can track what I send and times to prompt messages.
    float timeBetweenWaves = 0f;
    float timeBetweenEnemies = 0f;

    float timerForEvents = 0f;
    float eventTalkABoutBigPack;
    bool talkedAboutBigPack = false;
    float eventSeeTheSwarmEnemies;
    bool talkedAboutTheSwarm = false;

    private GoldManagement GM = null;

    [SerializeField] Button towerButton;
    [SerializeField] Light spotLight;

    [Header("Soldier")]
    [SerializeField] Texture soldierNeutral;
    [SerializeField] Texture soldierScared;

    [Header("General")]
    [SerializeField] Texture general;
    [SerializeField] Texture generalShouting;

    [Header("WayPoints")]
    [SerializeField] Waypoint waypoint1;
    [SerializeField] Waypoint waypoint2;

    [Header("Tutorial Pictures")]
    [SerializeField] Texture goldImage;
    [SerializeField] Texture waveImage;
    [SerializeField] Texture healthImage;
    [SerializeField] Texture towerButtonImage;
    [SerializeField] Texture enemyDoubleImage;
    [SerializeField] Texture enemyPathwayImage;
    private int lightHeight = 19;
    private int towerSpawnCount = 0;

    const string goldExplanation = "The people inside the base constantly build tower parts that are consumed to build different towers." +
        "This process is slow but constant.  When an alien is killed, they are reinvigorated and create a burst of parts instantly.  The amount of parts currently" +
        " stockpiled are shown here.";
    const string waveExplanation = "The wave count location is shown above.  The enemies in front are generally weaker, but as the wave count goes higher " +
        "the stronger enemies come out, which have more base life.";
    const string healthExplanation = "The health bar shows the bases defense.  When this hits 0, the aliens have broken past the gate and it is game over.  " +
        "The gate life is broken into two different sections.  One is the outer doors of the base, and the second is the inner barricade.  The barricade life is taken with " +
        "the team when they move to a new location, while the base doors are not.  The barricade can be replenished after some levels.";
    const string towerButtonExplanation = "The tower buttons are located at the bottom right of the screen.  This has the tower name, and parts cost.  " +
        "You first click on a buildable tile and then click the button for the tower you wish to build.  Tiles with flashing red cannot be build on.";
    const string enemyPathwayExplanation = "The tiles that flash red are actually tiles with a mild pheromone that attracts the Xeno.  It isn't strong enough" +
        " to do anything too clever, but it can make sure they follow a specific path, given they are already coming our way.";
    const string enemyTypesExplanation = "Swarm strain.  If enemies look different, then they are different.  Try to diversify towers to cover a bigger range of enemies easily. " +
        "This swarm strain (as the name implies) swarms.  Each one has less life than the normal Xenos, but together they can outperform it.  Use Area of Effect towers to more " +
        "efficiently kill these.";

    // Enemy promps
    const string genericEnemyExplanation = "The generic enemy has the most life out of all the basic enemies.";
    const string rollerEnemyExplanation = "The rolling enemy has increased movespeed and will dodge a projectile every few seconds.";
    const string doublesEnemyExplanation = "The swarm type enemies spawns in groups of 2.  Each one has less life than the basic enemy, however, " +
        "together their combined life is greater than the basic enemy.";
    const string burrowerEnemyExplanation = "The burrower strain will dig underground to avoid danger after taking damage.";

    const string healerEnemyExplanation = "The healer strain is an advanced type enemy that releases a healing gas while alive.  When it dies, it releases a more concentrated " +
        "blast, doing double healing for a few seconds.  Enemeis with full life receive half healing as it forms a protective coating over their carapace.  Healers are " +
        "immune to the healing gas.";
    const string slimerEnemyExplanation = "The slimer strand is a slower advanced enemy.  When it touches a tile that is not slimed it will expulse a permanent sludge " +
        "that increases the movespeed of all enemeis that are in it.  Slimes recive a more significant speed buff.";


    string string0 = ". . . ";
    string string1 = "It shouldn't take this long to find scraps.";
    string string2 = "Sir!!!  There's almost nothing left! I can only find one good piece, we’ve already scavenged most of the metal here.";
    string string3 = "SHUT UP! THE BUGS ARE BACK, GET BACK HERE NOW!!";
    string string4 = "?!?!?!?? HNNGGG";
    string string5 = "NO!! no, we WILL save him, it will have to be enough... ";
    string string6 = "THROW THE TOWER!  Up there! on that node, the base should give the tower increased range and damage.";
    string string7 = "He WILL make it back.";
    string string8 = "Good job.";
    string string9 = "Now, we wait and see if it is enough.";
    string string10 = "Ugh.  I hope this pack ends soon... ";
    string string11 = "The ones up front are always starved and malnurished, those are easy to kill.  But, as it goes on longer" +
    " the ones in the back are healthy and strong.  Those ones take a lot more firepower to stop."; //They have no need to rush for food, and they take many more bullets to kill.";
    string string12 = "Tobias!  There are a LOT more than usual. HELP!!!.";
    string string13 = "HELP!!!.";
    string string14 = "This isn't good.. And it has the swarm strain in its pack??  I dont know if it will be enough but.. THROW THE LAST TOWER"; //maybe popup explanations as well?
    //string string14 = "This isn't good.. That tower alone wont be enough.";
    string string15 = "We will need to utilize the base doors to hold them back, hopefully they hold long enough."; //Fire the last tower over there.  


    string stringy = "Oh my god thats a big wave! WHAT??";
    string stringz = "This isn't good.. And it has the swarm strain in its pack??  I dont know if it will be enough but.. THROW THE LAST TOWER"; //maybe popup explanations as well?

    // O SHIT! they have Rorendurs? something*.  We will need more than one tower for those enemies for sure. ?  those are swarm enemies that have less HP than the normal
    // guys but they spawn in a brood of 2, making the single shot rifled tower less effective.
    // Use this for initialization
    void Start()
    {
        //towerButton.enabled = false;
        towerButton.gameObject.SetActive(false);
        spotLight.gameObject.SetActive(false);

        Singleton singleton = Singleton.Instance;
        singleton.enemyList = new List<int> { 1, -1,   1, -1,   1, -1,   1, 1, (int)Enemies.doubles, (int)Enemies.doubles, 1};
        //enemySpawner.enemyList = new List<int> { 1, 1, -1, 1, 1, 1};

        singleton.SetLevel(1);

        // initializing strings
        text.text = "";
        
        conversations = new List<string>();
        conversations.AddRange(new string[] { string0, string1, string2, string3, string4, string5, string6, string7 });
        conversationTracker = 0;

        //GM.GoldCounter();
        GM = FindObjectOfType<GoldManagement>();

        StartCoroutine(LateInitialization());
        StartCoroutine(SlowMessageTyping());
    }

    IEnumerator LateInitialization()
    {
        yield return null;

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.SetDelayedSpawnTime(25f);
        GoldManagement GM = FindObjectOfType<GoldManagement>();
        GM.SetGoldAmount(94);
    }

    // I can make it whe ni build a tower start slow message again, but change the converstaions item, clear and add new dialogues.
    //NOPE IM STUPID  this is a function based off a button, a button that is not a necessary prefab.  Just have the button also do everything I want.

    IEnumerator SlowMessageTyping()
    {
        // TODO maybe put this in a while loop instead of calling itself?
        StartCoroutine(ConversationPicker());
        text.text = "";
        isEnableingChatTurnOff = false;
        //SpawnTheEnemiesAtScreem();  NOPE want more control here, removing the if and spawning on demand.
        // Loop the converstation 1 char at a time.
        for (int i = 0; i < talking.Length; i++)
        {
            var letter = talking.ToCharArray(i, 1);
            text.text += new string(letter);
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        yield return new WaitForSecondsRealtime(2);
        if (conversationTracker < conversations.Count)
        {
            //print(conversationTracker);
            yield return StartCoroutine(SlowMessageTyping());
        }

        isEnableingChatTurnOff = true;
        StartCoroutine(DisableText());
    }

    IEnumerator ConversationPicker()
    {
        List<string> promptTexts = new List<string>();
        List<Texture> prompImages = new List<Texture>();
        switch (conversationTracker)
        {
            case 0:
                typingSpeed = .5f;
                personTalking.texture = soldierNeutral;
                talking = conversations[conversationTracker];
                conversationTracker++;

                //testing
                //List<string> x = new List<string>() { "x", "y" };
                //List<Texture> y = new List<Texture>() { soldierNeutral, soldierScared };
                //var prompt = Instantiate(moreInformationPrompt, transform.position, Quaternion.identity, gameObject.transform);
                //prompt.GetComponentInChildren<MoreInformationPanel>().DelayedInitialization(y, x);
                break;
            case 1:
                typingSpeed = .02f;
                personTalking.texture = soldierNeutral;
                talking = conversations[conversationTracker];
                conversationTracker++;
                break;
            case 2:
                personTalking.texture = soldierNeutral;
                talking = conversations[conversationTracker];
                conversationTracker++;
                break;
            case 3:
                personTalking.texture = generalShouting;
                talking = conversations[conversationTracker];
                conversationTracker++;
                break;
            case 4:
                // To make the character run in scene
                timeToRun = true;
                SpawnTheEnemiesAtScreem();
                
                GM.Started();

                personTalking.texture = soldierScared;
                talking = conversations[conversationTracker];
                conversationTracker++;
                break;
            case 5:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;
                Time.timeScale = .2f;
                break;
            case 6:
                personTalking.texture = generalShouting;
                talking = conversations[conversationTracker];
                conversationTracker++;
                GM.SetGoldAmount(65);
                Time.timeScale = 0f;
                break;
            case 7:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;

                towerButton.gameObject.SetActive(true);
                spotLight.gameObject.SetActive(true);
                spotLight.transform.position = (waypoint1.transform.position + new Vector3(0f, lightHeight, 0f));
                isLastChatSegment = true;
                towerButton.enabled = true;

                promptTexts = new List<string>() { waveExplanation, goldExplanation, towerButtonExplanation, enemyPathwayExplanation };
                prompImages = new List<Texture>() { waveImage, goldImage, towerButtonImage, enemyPathwayImage };
                prompt = Instantiate(moreInformationPrompt, transform.position, Quaternion.identity, gameObject.transform);
                prompt.DelayedInitialization(prompImages, promptTexts);

                break;
            case 8:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;
                break;
            case 9:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;
                isLastChatSegment = true; // SIGH maybe use this bool to make an if in the slowtalk function so IF this is enabled, it swaps the one that it looks at in the function to disable below... w/e
                break;
            case 10:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;
                break;
            case 11:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;
                isLastChatSegment = true;
                break;
            case 12:
                personTalking.texture = soldierNeutral;
                talking = conversations[conversationTracker];
                conversationTracker++;
                Time.timeScale = .2f;
                break;
            case 13:
                personTalking.texture = soldierScared;
                talking = conversations[conversationTracker];
                conversationTracker++;
                break;
            case 14:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;
                spotLight.gameObject.SetActive(true);
                spotLight.transform.position = (waypoint2.transform.position + new Vector3(0f, lightHeight, 0f));
                break;
            case 15:
                personTalking.texture = general;
                talking = conversations[conversationTracker];
                conversationTracker++;
                GM.SetGoldAmount(65);
                Time.timeScale = 0f;
                isLastChatSegment = true;

                promptTexts = new List<string>() { enemyTypesExplanation };
                prompImages = new List<Texture>() { enemyDoubleImage };
                prompt = Instantiate(moreInformationPrompt, transform.position, Quaternion.identity, gameObject.transform);
                prompt.DelayedInitialization(prompImages, promptTexts);
                break;

        }

        yield break;
    }


    public void BuiltTowerNextConversation()
    {

        switch (towerSpawnCount)
        {
            // Can do checks here to see if they are placed in the right spot
            case 0:
                conversations.AddRange(new string[] { string8, string9 });
                conversationTracker = 8;
                break;
            case 1:
                //TODO treat differently.  This is
                Time.timeScale = 1.0f;
                spotLight.gameObject.SetActive(false);
                return;
            default:
                Time.timeScale = 1.0f;
                spotLight.gameObject.SetActive(false);
                break;
        }
        isEnableingChatTurnOff = false;
        Time.timeScale = 1.0f;
        //conversations.Clear();

        //towerButton.enabled = false;
        spotLight.gameObject.SetActive(false);
        TalkingCanvas.SetActive(true);
        StartCoroutine(SlowMessageTyping());
        towerSpawnCount++;

        StartCoroutine(DisableText());
    }

    private void SpawnTheEnemiesAtScreem()
    {
        //if (conversationTracker == conversations.Count && spawnEnemies)
        //{
        //StartCoroutine(FindObjectOfType<EnemySpawner>().ContinualSpawnEnemies());
        EnemySpawner enemyObj = FindObjectOfType<EnemySpawner>();
        enemyObj.StartBattle();

        float betweenSpawns = enemyObj.secondsBetweenSpawns;
        float betweenWaves = enemyObj.timeBetweenWaves;

        eventTalkABoutBigPack = ((2 * betweenWaves) + (2 * betweenSpawns) + 2f); // the 2f is just so you have time to see that they are tankier.
        eventSeeTheSwarmEnemies = ((3 * betweenWaves) + (6 * betweenSpawns));


        spawnEnemies = false;
        //}
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSecondsRealtime(3);
        if (isEnableingChatTurnOff)
        {
            //talkingCanvas.active = false;
            TalkingCanvas.SetActive(false);
        }       
    }


    // Update is called once per frame
    void Update()
    {
        if (!spawnEnemies)
        {
            timerForEvents += (1 * Time.deltaTime);
        }

        if (timerForEvents > eventTalkABoutBigPack && !talkedAboutBigPack)
        {
            talkedAboutBigPack = true;
            conversations.AddRange(new string[] { string10, string11 });
            conversationTracker = 10;

            TalkingCanvas.SetActive(true);
            StartCoroutine(SlowMessageTyping());

            StartCoroutine(DisableText());
        }
        if (timerForEvents > eventSeeTheSwarmEnemies && !talkedAboutTheSwarm)
        {
            talkedAboutTheSwarm = true;
            conversations.AddRange(new string[] { string12, string13, string14, string15 });
            conversationTracker = 12;

            TalkingCanvas.SetActive(true);
            StartCoroutine(SlowMessageTyping());

            StartCoroutine(DisableText());
        }


    } 
}
