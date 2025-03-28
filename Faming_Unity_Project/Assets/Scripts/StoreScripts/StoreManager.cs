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

    void Awake()
    {
        if(Instance == null)
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
    }
}

