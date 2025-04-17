using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoCamScript : MonoBehaviour
{
    //Camera script to shake the camera when the tornado is active
    public Camera mainCam; // Reference to the main camera
    public Camera TornadoCam; // Reference to the land camera

    public bool isTornadoExisting;

    public float shakeMagnitude = 0.1f; // Magnitude of the shake effect

    void Start()
    {
        mainCam = Camera.main; // Get the main camera
        TornadoCam = GetComponent<Camera>(); // Get the camera component
    }

    // Update is called once per frame
    void Update()
    {
        isTornadoExisting = GameObject.FindGameObjectWithTag("Tornado") != null; // Check if the tornado exists
        Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
        transform.position += shakeOffset;

        if (isTornadoExisting)
        {
            SwitchToTornadoCam();
        }
        else if (!isTornadoExisting && TornadoCam.enabled == true)
        {
            SwitchToMainCam();
        }
        
        
    }
    private void SwitchToTornadoCam()
    {
        // Switch to the tornado camera
        mainCam.enabled = false; // Disable the main camera
        TornadoCam.enabled = true; // Enable the tornado camera
    }

    private void SwitchToMainCam()
    {
        // Switch back to the main camera
        mainCam.enabled = true; // Enable the main camera
        TornadoCam.enabled = false; // Disable the tornado camera
    }
}




