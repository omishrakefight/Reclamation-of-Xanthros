using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {


    [SerializeField] Camera camera;
    Vector3 cameraLocation;
    Vector3 newCameraLocation;

    float delay;

    private void Start()
    {
        //camera.GetComponent<Camera>()
        cameraLocation = camera.transform.position;
    }

    public void MoveCameraToComputer()
    {
        newCameraLocation = cameraLocation - new Vector3(80, 0, 0);
        StartCoroutine(CameraDelay());
        //newCameraLocation = cameraLocation;
        //StartCoroutine(CameraDelay());
    }
    public void MoveCameraToTurrets()
    {
        newCameraLocation = cameraLocation - new Vector3(40, 0, 0);
        StartCoroutine(CameraDelay());
    }
    public void MoveCameraToEngineerer()
    {
        newCameraLocation = cameraLocation - new Vector3(120, 0, 0);
        StartCoroutine(CameraDelay());
    }
    public void MoveCameraToMeeting()
    {
        newCameraLocation = cameraLocation;
        StartCoroutine(CameraDelay());
        //newCameraLocation = cameraLocation + new Vector3(80, 0, 0);
        //StartCoroutine(CameraDelay());
    }
    public void MoveCameraToTinker()
    {
        newCameraLocation =  cameraLocation - new Vector3(160, 0, 0); //camera.transform.position =
        StartCoroutine(CameraDelay());
    }

    IEnumerator CameraDelay()
    {
        //  Get the time of fades for camera switching
        delay = FindObjectOfType<CanvasManager>().currentScreenFader.GetComponent<FadeScript>().fadeTime;
        yield return new WaitForSeconds(delay);
        camera.transform.position = newCameraLocation;

    }

}
