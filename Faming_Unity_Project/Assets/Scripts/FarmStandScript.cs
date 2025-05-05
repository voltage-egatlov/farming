using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Ensure you have this for UI components

public class FarmStandScript : MonoBehaviour
{
    public GameObject farmStandPanel; // Assign your FarmStandPanel in the Inspector
    public GameObject tractor;

    public TMP_Text cornHarvestedText;
    public TMP_Text cornPriceText;
    public TMP_Text totalBankText;

    private int cropPrice;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the farm stand panel is inactive at the start
        if (farmStandPanel != null)
        {
            farmStandPanel.SetActive(false);
        }
    }

    void Update()
    {
        if(farmStandPanel.activeSelf)
        {
            tractor.gameObject.GetComponent<Tractor_Handler>().canMove = false; // Prevent tractor movement while farm stand is open
        }
        else
        {
            tractor.gameObject.GetComponent<Tractor_Handler>().canMove = true; // Allow movement when farm stand is closed
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger area
        if (other.CompareTag("Tractor") && GameManager.Instance.currentPhase == GameManager.Phase.Phase4)
        {
            GameManager.Instance.currentPhase = GameManager.Phase.Phase4; // Advance to Phase 5
            // Activate the farm stand panel
            if (farmStandPanel != null)
            {
                cropPrice = GameManager.Instance.GetCropPrice();
                if (cornHarvestedText != null)
                {
                    cornHarvestedText.text = "Corn Harvested: " + GameManager.Instance.GetCropCount("Corn").ToString();
                }
                
                if (cornPriceText != null)
                {
                    cornPriceText.text = "Corn Price: $" + cropPrice.ToString();
                    GameManager.Instance.currentCropPrice = cropPrice; // Update the current crop price in GameManager
                }
                
                if (totalBankText != null)
                {
                    totalBankText.text = "Total Bank: $" + GameManager.Instance.GetCurrentBalance().ToString();
                }
                farmStandPanel.SetActive(true);
            }
        }
    }

    public void CloseFarmStandPanel()
    {
        // Deactivate the farm stand panel
        if (farmStandPanel != null)
        {
            farmStandPanel.SetActive(false);
        }
    }

    public void backToPhaseOne()
    {
        // Reset the game to Phase 1
        GameManager.Instance.currentPhase = GameManager.Phase.Phase1;

        tractor.transform.position = new Vector3(-37.5f, 1f, 115f); // Reset tractor position (example)
        tractor.transform.rotation = Quaternion.Euler(0, 180f, 0); // Reset tractor rotation (example)
        CloseFarmStandPanel(); // Close the farm stand panel
    }
}
