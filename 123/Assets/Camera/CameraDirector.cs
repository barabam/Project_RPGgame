using UnityEngine;
using System.Collections;

public class CameraDirector : MonoBehaviour
{
    public GameObject cameraPathA;
    public GameObject cameraPathB;
    public GameObject currentCameraPath = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentCameraPath = cameraPathA;
            currentCameraPath.SendMessage("Play");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            currentCameraPath = cameraPathB;
            currentCameraPath.SendMessage("Play");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentCameraPath.SendMessage("Pause");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            currentCameraPath.SendMessage("Stop");
        }
    }
}
