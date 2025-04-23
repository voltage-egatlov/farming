using System.Collections;
using UnityEngine;
using TMPro;

public class InventoryManager_NEW : MonoBehaviour
{
    public GameObject inventoryPanel; // Drag your panel's RectTransform here
    public TMP_Text balanceText;

    public TMP_Text tomatoSeedText;
    public TMP_Text carrotSeedText;
    public TMP_Text cornSeedText;
    public TMP_Text cabbageSeedText;
    public TMP_Text wheatSeedText;

    public TMP_Text cornCropText;

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
        cornSeedText.text = "Corn Seeds: " + GameManager.Instance.GetSeedCount("Corn");
        cornCropText.text = "Corn: " + GameManager.Instance.GetCropCount("Corn");
    }

    void Update()
    {
        if (inventoryPanel.activeSelf)
        {
            UpdateInventoryDisplay();
        }
    }
}
