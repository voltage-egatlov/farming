using System.Collections;
using UnityEngine;
using TMPro;

public class InventoryManager_NEW : MonoBehaviour
{
    public GameObject inventoryPanel; 
    public TMP_Text balanceText;
    public TMP_Text seedsText;
    public TMP_Text cropsText;

    private bool isOpen = false;


    public void ToggleInventory()
    {
         if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            UpdateInventoryDisplay();
            inventoryPanel.SetActive(true);
        }
    }

    private void UpdateInventoryDisplay()
    {
        balanceText.text = "Balance: $" + GameManager.Instance.GetCurrentBalance();
        seedsText.text = "Seeds: " + GameManager.Instance.GetSeedCount("Corn");
        cropsText.text = "Crops: " + GameManager.Instance.GetCropCount("Corn");
    }

    void Update()
    {
        if (inventoryPanel.activeSelf)
        {
            UpdateInventoryDisplay();
        }
    }
}
