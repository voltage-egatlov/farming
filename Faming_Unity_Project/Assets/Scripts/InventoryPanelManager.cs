using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryPanelManager : MonoBehaviour
{
    public GameObject inventoryPanel; // assign your InventoryPanel in the Inspector
    public TMP_Text balanceText;
    
    // Seed texts
    public TMP_Text tomatoSeedText;
    public TMP_Text carrotSeedText;
    public TMP_Text cornSeedText;
    public TMP_Text cabbageSeedText;
    public TMP_Text wheatSeedText;
    
    // Crop texts
    // public TMP_Text tomatoCropText;
    // public TMP_Text carrotCropText;
    // public TMP_Text cornCropText;
    // public TMP_Text cabbageCropText;
    // public TMP_Text wheatCropText;

    // Method to open or refresh the inventory panel
    public void OpenInventory()
    {
        UpdateInventoryDisplay();
        inventoryPanel.SetActive(true);
    }

    // Method to close the panel
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    // Update all UI text fields with data from GameManager
    private void UpdateInventoryDisplay()
    {
        // Update bank balance
        balanceText.text = "Balance: $" + GameManager.Instance.GetCurrentBalance().ToString();
        
        // Update seed inventory
        tomatoSeedText.text = "Tomato Seeds: " + GameManager.Instance.GetSeedCount("Tomato");
        carrotSeedText.text = "Carrot Seeds: " + GameManager.Instance.GetSeedCount("Carrot");
        cornSeedText.text = "Corn Seeds: " + GameManager.Instance.GetSeedCount("Corn");
        cabbageSeedText.text = "Cabbage Seeds: " + GameManager.Instance.GetSeedCount("Cabbage");
        wheatSeedText.text = "Wheat Seeds: " + GameManager.Instance.GetSeedCount("Wheat");

        // // Update crop inventory
        // tomatoCropText.text = "Tomatoes: " + GameManager.Instance.GetCropCount("Tomato");
        // carrotCropText.text = "Carrots: " + GameManager.Instance.GetCropCount("Carrot");
        // cornCropText.text = "Corn: " + GameManager.Instance.GetCropCount("Corn");
        // cabbageCropText.text = "Cabbage: " + GameManager.Instance.GetCropCount("Cabbage");
        // wheatCropText.text = "Wheat: " + GameManager.Instance.GetCropCount("Wheat");
    }
}
