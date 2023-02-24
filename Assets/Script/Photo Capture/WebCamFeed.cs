using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamFeed : MonoBehaviour
{
    [SerializeField] string cameraName;

    Texture2D webcamTexture2D;

    int currentCameraIndex = 0;

    private WebCamTexture webcamTexture;

    public RawImage rawImage;

    [Space]
    [SerializeField] Vector2Int cameraResolution = new Vector2Int(1280, 720);
    [SerializeField] int cameraFPS = 30;

    public Texture2D WebcamTexture2D
    {
        get
        {
            return webcamTexture2D;
        }
    }


    [ContextMenu("List device names")]
    public void ListCameras()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (var i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
        }
    }


    [ContextMenu("Current Using Camera")]
    public void CurrentCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        Debug.Log(devices[currentCameraIndex].name);
    }


    private void Start()
    {
        if (cameraName != "" && DoesCameraExist(cameraName))
        {
            PlaySpecificCam(cameraName);
        }
        else
        {
            PlayWebCamOnImage(currentCameraIndex);
        }
    }

    public void StopCam()
    {
        webcamTexture.Stop();
    }

    public void PlaySpecificCam(string cameraName)
    {
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            if (WebCamTexture.devices[i].name == cameraName)
            {
                PlayWebCamOnImage(i);
            }
        }
    }



    void PlayWebCamOnImage(int cameraIndex)
    {

        webcamTexture = new WebCamTexture(WebCamTexture.devices[cameraIndex].name, cameraResolution.x, cameraResolution.y, cameraFPS);

        rawImage.texture = webcamTexture;
        //rawImage.material.mainTexture = webcamTexture;

        webcamTexture.Play();
    }

    [ContextMenu("Next camera")]
    public void NextCamera()
    {
        currentCameraIndex = ++currentCameraIndex % WebCamTexture.devices.Length;
        webcamTexture.Stop();
        PlayWebCamOnImage(currentCameraIndex);
    }

    private bool DoesCameraExist(string cameraName)
    {
        bool exists = false;
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            if (!exists)
            if (WebCamTexture.devices[i].name == cameraName)
            {
                    exists = true;
            }
        }

        return exists;
    }
}


