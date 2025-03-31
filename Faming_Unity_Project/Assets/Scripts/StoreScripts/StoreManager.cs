using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;
    
    public GameObject storeContainer; // the main store UI panel
    public GameObject tractorPanel;   // Page 1 panel
    public GameObject seedsPanel;     // Page 2 panel
    public GameObject futurePanel;    // Page 3 panel

    public Camera mainCam;
    public Camera landCam;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenStore()
    {
        print("Opening store");
        storeContainer.SetActive(true);
        ShowTractorPanel();
        // Pause game or disable player movement if needed.
    }

    public void CloseStore()
    {
        storeContainer.SetActive(false);
        SwitchToMainCam();
        // Resume game or enable player movement.
    }

    public void ShowTractorPanel()
    {
        tractorPanel.SetActive(true);
        seedsPanel.SetActive(false);
        futurePanel.SetActive(false);
    }

    public void ShowSeedsPanel()
    {
        tractorPanel.SetActive(false);
        seedsPanel.SetActive(true);
        futurePanel.SetActive(false);
    }

    public void ShowFuturePanel()
    {
        tractorPanel.SetActive(false);
        seedsPanel.SetActive(false);
        futurePanel.SetActive(true);
        // SwitchToLandCam();
    }

    public void ActivateLandView()
    {
        Debug.Log("🗺️ Switching to LandCam and hiding Map UI");

        // Hide the UI panel that contains "Purchase Land!" and "MAP HERE"
        if (futurePanel != null)
            futurePanel.SetActive(false); // This hides the map UI

        // Switch to top-down LandCam
        SwitchToLandCam();
    }

    private void SwitchToLandCam()
    {
        if (mainCam != null && landCam != null)
        {
            mainCam.gameObject.SetActive(false); // Full GameObject disable
            landCam.gameObject.SetActive(true);

            // AudioListener safety
            AudioListener mainAudio = mainCam.GetComponent<AudioListener>();
            if (mainAudio) mainAudio.enabled = false;

            AudioListener landAudio = landCam.GetComponent<AudioListener>();
            if (landAudio) landAudio.enabled = true;

            Debug.Log("✅ Switched to LandCam!");
        }
        else
        {
            Debug.LogWarning("🚫 One or both cameras are not assigned in StoreManager.");
        }
    }

    private void SwitchToMainCam()
    {
        if (mainCam != null && landCam != null)
        {
            mainCam.gameObject.SetActive(true);
            landCam.gameObject.SetActive(false);

            AudioListener mainAudio = mainCam.GetComponent<AudioListener>();
            if (mainAudio) mainAudio.enabled = true;

            AudioListener landAudio = landCam.GetComponent<AudioListener>();
            if (landAudio) landAudio.enabled = false;

            Debug.Log("✅ Switched to MainCam!");
        }
        else
        {
            Debug.LogWarning("🚫 One or both cameras are not assigned in StoreManager.");
        }
    }

}
