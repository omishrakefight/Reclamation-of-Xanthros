using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    FadeScript fader;
    Singleton _singleton = null;

    [Header ("Canvases")]
    [SerializeField] Canvas computerBase;
    [SerializeField] Canvas turretFactory;
    [SerializeField] Canvas engineerer;
    [SerializeField] Canvas meetingRoom;
    [SerializeField] Canvas Tinker;
    Canvas currentActiveCanvas;

    [Header ("Fade Filters")]
    [SerializeField] GameObject computerFader;
    [SerializeField] GameObject engineerFader;
    [SerializeField] GameObject tinkerFader;
    [SerializeField] GameObject turretFader;
    [SerializeField] GameObject meetingRoomFader;
    public GameObject currentScreenFader;

    [Header("Buttons")]
    [SerializeField] Button computerBtn;
    [SerializeField] Button engineerBtn;
    [SerializeField] Button tinkerBtn;
    [SerializeField] Button turretBtn;
    [SerializeField] Button meetingRoomBtn;

    // Use this for initialization
    void Start () {

        // Only have 1 Canvas going at a time
        turretFactory.gameObject.SetActive(true);
        computerBase.gameObject.SetActive(true);
        engineerer.gameObject.SetActive(true);
        Tinker.gameObject.SetActive(true);
        meetingRoom.gameObject.SetActive(true);

        // UI being turned off but the object is on (just canvas part).
        turretFactory.GetComponent<Canvas>().enabled = false;
        computerBase.GetComponent<Canvas>().enabled = false;
        engineerer.GetComponent<Canvas>().enabled = false;
        Tinker.GetComponent<Canvas>().enabled = false;


        computerFader.SetActive(true);
        engineerFader.SetActive(true);
        tinkerFader.SetActive(true);
        turretFader.SetActive(true);
        meetingRoomFader.SetActive(true);

        currentActiveCanvas = meetingRoom;
        currentScreenFader = meetingRoomFader;

        CheckWhatCanBeDone();

        //IEnumerator start;
        //start = DelayedStart();

        //StartCoroutine(start);
        //StopCoroutine(start);

        //StartupLoad();
    }

    public IEnumerator DelayedStart()
    {

        yield return new WaitForFixedUpdate();

        if (_singleton == null)
        {
            _singleton = FindObjectOfType<Singleton>();
        }

        // Only have 1 Canvas going at a time
        turretFactory.gameObject.SetActive(true);
        computerBase.gameObject.SetActive(true);
        engineerer.gameObject.SetActive(true);
        Tinker.gameObject.SetActive(true);
        meetingRoom.gameObject.SetActive(true);

        // UI being turned off but the object is on (just canvas part).
        turretFactory.GetComponent<Canvas>().enabled = false;
        computerBase.GetComponent<Canvas>().enabled = false;
        engineerer.GetComponent<Canvas>().enabled = false;
        Tinker.GetComponent<Canvas>().enabled = false;


        computerFader.SetActive(true);
        engineerFader.SetActive(true);
        tinkerFader.SetActive(true);
        turretFader.SetActive(true);
        meetingRoomFader.SetActive(true);

        currentActiveCanvas = meetingRoom;
        currentScreenFader = meetingRoomFader;

        CheckWhatCanBeDone();

        yield return null;
    }
	
    void FadeIn_DisableOldCanvas()
    {
        currentScreenFader.GetComponent<FadeScript>().FadeIn();
        currentActiveCanvas.GetComponent<Canvas>().enabled = false;
    }

    //public void StartupLoad()
    //{
    //    meetingRoom.GetComponent<Canvas>().enabled = false;  //set unenabled and see if the scrips load
    //}

    // room buttons, first checks if the active room is the one clicked.
    public void ChooseComputerRoom()
    {
        CheckWhatCanBeDone();
        if (currentActiveCanvas != computerBase)
        {
            StartCoroutine(ComputerRoom());
        }
    }
    public void ChooseTurretRoom()
    {
        CheckWhatCanBeDone();
        if (currentActiveCanvas != turretFactory)
        {
            StartCoroutine(TurretRoom());
        }
    }
    public void ChooseEngineerRoom()
    {
        CheckWhatCanBeDone();
        if (currentActiveCanvas != engineerer)
        {
            StartCoroutine(EngineerRoom());
        }
    }
    public void ChooseTinkerRoom()
    {
        CheckWhatCanBeDone();
        if (currentActiveCanvas != Tinker)
        {
            StartCoroutine(TinkerRoom());
        }
    }
    public void ChooseMeetingRoom()
    {
        CheckWhatCanBeDone();
        if (currentActiveCanvas != meetingRoom)
        {
            StartCoroutine(MeetingRoom());
        }
    }

    // Check the singleton to see what bools are available, change the buttons to gold to indicate stuff can be done.
    // this is to be called every time a button is click to room change to update the colors.
    public void CheckWhatCanBeDone()
    {
        if (_singleton == null)
        {
            _singleton = FindObjectOfType<Singleton>();
        }

        if (_singleton.isHasLearnedATower == false)
        {
            var color = computerBtn.colors;
            color.normalColor = Color.yellow;
            computerBtn.colors = color;
        }
        else
        {
            //turn it white.
            var color = computerBtn.colors;
            color.normalColor = Color.white;
            computerBtn.colors = color;
        }

        if (_singleton.ishasLearnedTinker == false)
        {
            var color = tinkerBtn.colors;
            color.normalColor = Color.yellow;
            tinkerBtn.colors = color;
        }
        else
        {
            var color = tinkerBtn.colors;
            color.normalColor = Color.white;
            tinkerBtn.colors = color;
        }

        if (_singleton.isHasPickedAPath == false)
        {
            var color = meetingRoomBtn.colors;
            color.normalColor = Color.yellow;
            meetingRoomBtn.colors = color;
        }
        else
        {
            //turn it white.
            var color = meetingRoomBtn.colors;
            color.normalColor = Color.white;
            meetingRoomBtn.colors = color;
        }
    }

    IEnumerator MeetingRoom()
    {
        //CheckForSingleton();

        FadeIn_DisableOldCanvas();
        var delay = currentScreenFader.GetComponent<FadeScript>().fadeTime;
        yield return new WaitForSeconds(delay);

        meetingRoom.GetComponent<Canvas>().enabled = true;
        //meetingRoom.gameObject.SetActive(true);
        meetingRoomFader.gameObject.SetActive(true);

        currentActiveCanvas = meetingRoom;
        currentScreenFader = meetingRoomFader;
        currentScreenFader.GetComponent<FadeScript>().FadeOut();
        yield break;
    }
    IEnumerator TinkerRoom()
    {
        //CheckForSingleton();

        FadeIn_DisableOldCanvas();
        var delay = currentScreenFader.GetComponent<FadeScript>().fadeTime;
        yield return new WaitForSeconds(delay);

        Tinker.GetComponent<Canvas>().enabled = true;
        //Tinker.gameObject.SetActive(true);
        tinkerFader.gameObject.SetActive(true);

        currentActiveCanvas = Tinker;
        currentScreenFader = tinkerFader;
        currentScreenFader.GetComponent<FadeScript>().FadeOut();
        yield break;
    }
    IEnumerator EngineerRoom()
    {
        //CheckForSingleton();

        FadeIn_DisableOldCanvas();
        var delay = currentScreenFader.GetComponent<FadeScript>().fadeTime;
        yield return new WaitForSeconds(delay);

        engineerer.GetComponent<Canvas>().enabled = true;
        //engineerer.gameObject.SetActive(true);
        engineerFader.gameObject.SetActive(true);

        currentActiveCanvas = engineerer;
        currentScreenFader = engineerFader;
        currentScreenFader.GetComponent<FadeScript>().FadeOut();

        yield break;
    }



    IEnumerator TurretRoom()
    {
        //CheckForSingleton();

        FadeIn_DisableOldCanvas();
        var delay = currentScreenFader.GetComponent<FadeScript>().fadeTime;
        yield return new WaitForSeconds(delay);

        turretFactory.GetComponent<Canvas>().enabled = true;
        //turretFactory.gameObject.SetActive(true);
        turretFader.gameObject.SetActive(true);

        currentActiveCanvas = turretFactory;
        currentScreenFader = turretFader;
        currentScreenFader.GetComponent<FadeScript>().FadeOut();
        yield break;
    }
    IEnumerator ComputerRoom()
    {
        //CheckForSingleton();

        FadeIn_DisableOldCanvas();
        var delay = currentScreenFader.GetComponent<FadeScript>().fadeTime;
        yield return new WaitForSeconds(delay);

        computerBase.GetComponent<Canvas>().enabled = true;
        //computerBase.gameObject.SetActive(true);
        computerFader.gameObject.SetActive(true);

        currentActiveCanvas = computerBase;
        currentScreenFader = computerFader;
        currentScreenFader.GetComponent<FadeScript>().FadeOut();

        yield break;
    }


    //private void CheckForSingleton()
    //{
    //    if (_singleton == null)
    //    {
    //        _singleton = FindObjectOfType<Singleton>();
    //    }
    //}

    /*
    public IEnumerator Wait()
    {
        var delay = currentScreenFader.GetComponent<FadeScript>().fadeTime;
        yield return new WaitForSeconds(delay);
        currentScreenFader.GetComponent<FadeScript>().FadeOut();
    }  */
}
