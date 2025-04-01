// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class StoreManager : MonoBehaviour
// {
//     public static StoreManager Instance;
    
//     public GameObject storeContainer; // the main store UI panel
//     public GameObject tractorPanel;   // Page 1 panel
//     public GameObject seedsPanel;     // Page 2 panel
//     public GameObject futurePanel;    // Page 3 panel
//     public GameObject closeButton;
//     public LandPlot selectedPlot;
//     public GameObject purchaseButtonsPanel;

//     public Camera mainCam;
//     public Camera landCam;

//     void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//     }

//     public void OpenStore()
//     {
//         print("Opening store");
//         storeContainer.SetActive(true);
//         ShowTractorPanel();
//         // Pause game or disable player movement if needed.
//     }

//     public void CloseStore()
//     {
//         storeContainer.SetActive(false);

//         if (purchaseButtonsPanel != null)
//             purchaseButtonsPanel.SetActive(false); // Hide the land buttons

//         SwitchToMainCam();
//     }


//     public void ShowTractorPanel()
//     {
//         tractorPanel.SetActive(true);
//         seedsPanel.SetActive(false);
//         futurePanel.SetActive(false);
//     }

//     public void ShowSeedsPanel()
//     {
//         tractorPanel.SetActive(false);
//         seedsPanel.SetActive(true);
//         futurePanel.SetActive(false);
//     }

//     public void ShowFuturePanel()
//     {
//         tractorPanel.SetActive(false);
//         seedsPanel.SetActive(false);
//         futurePanel.SetActive(true);
//         // SwitchToLandCam();
//     }

//     public void ActivateLandView()
//     {
//         Debug.Log("🗺️ Switching to LandCam and showing land buttons");

//         if (futurePanel != null)
//             futurePanel.SetActive(false);

//         if (closeButton != null)
//             closeButton.SetActive(true);

//         if (purchaseButtonsPanel != null)
//             purchaseButtonsPanel.SetActive(true); // ✅ SHOW buttons!

//         SwitchToLandCam();
//     }


//     private void SwitchToLandCam()
//     {
//         if (mainCam != null && landCam != null)
//         {
//             mainCam.gameObject.SetActive(false); // Full GameObject disable
//             landCam.gameObject.SetActive(true);

//             // AudioListener safety
//             AudioListener mainAudio = mainCam.GetComponent<AudioListener>();
//             if (mainAudio) mainAudio.enabled = false;

//             AudioListener landAudio = landCam.GetComponent<AudioListener>();
//             if (landAudio) landAudio.enabled = true;

//             Debug.Log("✅ Switched to LandCam!");
//         }
//         else
//         {
//             Debug.LogWarning("🚫 One or both cameras are not assigned in StoreManager.");
//         }
//     }

//     private void SwitchToMainCam()
//     {
//         if (mainCam != null && landCam != null)
//         {
//             mainCam.gameObject.SetActive(true);
//             landCam.gameObject.SetActive(false);

//             AudioListener mainAudio = mainCam.GetComponent<AudioListener>();
//             if (mainAudio) mainAudio.enabled = true;

//             AudioListener landAudio = landCam.GetComponent<AudioListener>();
//             if (landAudio) landAudio.enabled = false;

//             Debug.Log("✅ Switched to MainCam!");
//         }
//         else
//         {
//             Debug.LogWarning("🚫 One or both cameras are not assigned in StoreManager.");
//         }
//     }

//     public void SetSelectedPlot(LandPlot plot)
//     {
//         selectedPlot = plot;
//     }

//     public void BuySelectedPlot()
//     {
//         if (selectedPlot != null)
//             selectedPlot.Purchase();
//     }

// }



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // required for button access
using TMPro; // required if you're using TextMeshProUGUI

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    public GameObject storeContainer;
    public GameObject tractorPanel;
    public GameObject seedsPanel;
    public GameObject futurePanel;
    public GameObject closeButton;
    public GameObject purchaseButtonsPanel;

    public Button[] landPurchaseButtons;  // drag buttons here
    public LandPlot[] landPlots;          // drag land plots here
    public LandPlot selectedPlot;

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
        Debug.Log("🛒 Opening store...");
        storeContainer.SetActive(true);
        ShowTractorPanel();
    }

    public void CloseStore()
    {
        storeContainer.SetActive(false);

        if (purchaseButtonsPanel != null)
            purchaseButtonsPanel.SetActive(false); // Hide land purchase UI

        SwitchToMainCam();
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
    }

    public void ActivateLandView()
    {
        Debug.Log("🗺️ Switching to LandCam and showing land buttons");

        if (futurePanel != null)
            futurePanel.SetActive(false);

        if (closeButton != null)
            closeButton.SetActive(true);

        if (purchaseButtonsPanel != null)
            purchaseButtonsPanel.SetActive(true);

        UpdateLandButtons();

        SwitchToLandCam();
    }

    private void UpdateLandButtons()
    {
        for (int i = 0; i < landPurchaseButtons.Length && i < landPlots.Length; i++)
        {
            int index = i;
            LandPlot plot = landPlots[i];
            Button button = landPurchaseButtons[i];

            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = plot.isPurchased ? "Purchased" : $"Purchase ${plot.price}";
            }

            // Reset and assign listener
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => PurchaseLand(index));

            button.interactable = !plot.isPurchased;
        }
    }


    private void PurchaseLand(int index)
    {
        if (index >= 0 && index < landPlots.Length)
        {
            landPlots[index].Purchase();
            UpdateLandButtons(); // Refresh text & interactability
        }
    }


    private void SwitchToLandCam()
    {
        if (mainCam != null && landCam != null)
        {
            mainCam.gameObject.SetActive(false);
            landCam.gameObject.SetActive(true);

            AudioListener mainAudio = mainCam.GetComponent<AudioListener>();
            if (mainAudio) mainAudio.enabled = false;

            AudioListener landAudio = landCam.GetComponent<AudioListener>();
            if (landAudio) landAudio.enabled = true;

            Debug.Log("✅ Switched to LandCam!");
        }
        else
        {
            Debug.LogWarning("🚫 Cameras not assigned!");
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
            Debug.LogWarning("🚫 Cameras not assigned!");
        }
    }

    public void SetSelectedPlot(LandPlot plot)
    {
        selectedPlot = plot;
    }

    public void BuySelectedPlot()
    {
        if (selectedPlot != null)
        {
            selectedPlot.Purchase();
            UpdateLandButtons();
        }
    }
}
