using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    public Tractor_Handler tractorHandler;

    public GameObject storeContainer;
    public GameObject tractorPanel;
    public GameObject seedsPanel;
    public GameObject futurePanel;
    public GameObject closeButton;
    public GameObject mapPanel;
    public GameObject purchaseButtonsPanel;

    public Button[] landPurchaseButtons;
    public LandPlot[] landPlots;
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
        if (GameManager.Instance.currentPhase != GameManager.Phase.Phase1)
        {
            Debug.Log("Store is not accessible in the current phase.");
            return;
        }

        Debug.Log("Opening store...");
        // (Your existing OpenStore code)
        if (mapPanel != null)
            mapPanel.SetActive(true);
        if (purchaseButtonsPanel != null)
            purchaseButtonsPanel.SetActive(false);
        if (closeButton != null)
            closeButton.SetActive(false);

        SwitchToMainCam(); // Always start from main camera
        storeContainer.SetActive(true);
        ShowTractorPanel(); // Start on the Tractor panel
    }



    public void CloseStore()
    {
        storeContainer.SetActive(false);

        // Optional: hide close button when closing store
        if (closeButton != null)
            closeButton.SetActive(false);

        // Transition to Phase 2 if currently in Phase 1
        if (GameManager.Instance.currentPhase == GameManager.Phase.Phase1)
        {
            GameManager.Instance.currentPhase = GameManager.Phase.Phase2;
            Debug.Log("Transitioned to Phase 2: Farming mode.");
        }

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
        Debug.Log("Switching to LandCam, hiding entire MapPanel, showing purchase buttons");

        // Disable the entire MapPanel
        if (mapPanel != null)
        {
            mapPanel.SetActive(false);
            Debug.Log("MapPanel has been fully deactivated.");
        }

        if (closeButton != null)
        {
            closeButton.SetActive(true);
            Debug.Log("Close button activated: " + closeButton.name);
        }

        // Show land purchase buttons
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
                text.text = plot.isPurchased ? "Purchased" : $"Purchase ${plot.price}";

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
            UpdateLandButtons(); // Refresh the UI
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

            Debug.Log("Switched to LandCam!");
        }
        else
        {
            Debug.LogWarning("Cameras not assigned!");
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

            Debug.Log("Switched to MainCam!");
        }
        else
        {
            Debug.LogWarning("Cameras not assigned!");
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

    public void CloseLandView()
    {
        Debug.Log("Closing land view and returning to main cam.");

        // Hide the land purchase buttons
        if (purchaseButtonsPanel != null)
            purchaseButtonsPanel.SetActive(false);

        // Hide the close button itself
        if (closeButton != null)
            closeButton.SetActive(false);

        // Switch back to the main camera
        SwitchToMainCam();
    }

    public void UpgradeTractor()
    {
        int upgradeCost = 200;
        float speedBoost = 1f;

        if (GameManager.Instance.DeductFunds(upgradeCost))
        {
            tractorHandler.forwardSpeed += speedBoost;
            tractorHandler.reverseSpeed += speedBoost * 0.4f;
            Debug.Log($"Upgraded tractor! New forward speed: {tractorHandler.forwardSpeed}");
        }
        else
        {
            Debug.Log("Not enough money to upgrade the tractor.");
        }
    }

    public void DowngradeTractor()
    {
        int refundAmount = 50;
        float speedReduction = 1f;
        float minSpeed = 1f;

        if (tractorHandler.forwardSpeed > minSpeed)
        {
            tractorHandler.forwardSpeed = Mathf.Max(minSpeed, tractorHandler.forwardSpeed - speedReduction);
            tractorHandler.reverseSpeed = Mathf.Max(minSpeed * 0.4f, tractorHandler.reverseSpeed - speedReduction * 0.4f);
            GameManager.Instance.AddFunds(refundAmount);
            Debug.Log($"Downgraded tractor. New forward speed: {tractorHandler.forwardSpeed}");
        }
        else
        {
            Debug.Log("Tractor is already at minimum speed.");
        }
    }


}

