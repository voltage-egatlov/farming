using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int bankBalance = 1000;  // Starting funds

    public enum Phase
    {
        Phase1,  // Store available; no farming yet
        Phase2,  // Farming actions allowed; store locked
        Phase3   // (Transitioned via barn)
    }
    public Phase currentPhase = Phase.Phase1;

    private Dictionary<string, int> seedInventory = new Dictionary<string, int>();
    private Dictionary<string, int> cropInventory = new Dictionary<string, int>();

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    
    public void AddFunds(int amount)
    {
        bankBalance += amount;
    }
    
    public bool DeductFunds(int amount)
    {
        if(bankBalance >= amount)
        {
            bankBalance -= amount;
            return true;
        }
        return false;
    }
    
    public int GetCurrentBalance()
    {
        return bankBalance;
    }

    public void AddSeed(string seedType, int quantity = 1)
    {
        if (seedInventory.ContainsKey(seedType))
        {
            seedInventory[seedType] += quantity;
        }
        else
        {
            seedInventory[seedType] = quantity;
        }
        Debug.Log("Added " + quantity + " " + seedType + " seed(s). Total: " + seedInventory[seedType]);
    }

    public int GetSeedCount(string seedType)
    {
        if (seedInventory.ContainsKey(seedType))
            return seedInventory[seedType];
        return 0;
    }

    public void AddCrop(string cropType, int quantity = 1)
    {
        if (cropInventory.ContainsKey(cropType))
            cropInventory[cropType] += quantity;
        else
            cropInventory[cropType] = quantity;

        Debug.Log("Added " + quantity + " " + cropType + " crop(s). Total: " + cropInventory[cropType]);
    }

    // Method to retrieve the count for a crop
    public int GetCropCount(string cropType)
    {
        if (cropInventory.ContainsKey(cropType))
            return cropInventory[cropType];
        return 0;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Aiden_Scene_1"); // Modified: Redirect to Level 1 ALE scene
    }
    
    // Return to MainMenu
    public void RestartGame() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame() 
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
