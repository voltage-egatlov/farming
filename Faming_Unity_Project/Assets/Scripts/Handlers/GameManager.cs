using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI phase2TimerText;
    public int targetBalance = 5000; 
    public TextMeshProUGUI bankBalanceText; // Reference to the UI Text element for displaying the bank balance
    public TextMeshProUGUI phaseText; // Reference to the UI Text element for displaying the current phase
    
    public int bankBalance = 1000;  // Starting funds
    public int harvestedCrops = 0; // Amount of crops harvested

    public enum Phase
    {
        Phase1,  // Store available; no farming yet
        Phase2,  // Farming actions allowed; store locked
        Phase3,   // (Transitioned via barn)
        Phase4   // Time to Harvest
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

    public void Update()
    {
        switch (currentPhase)
        {
            case Phase.Phase1:
                phaseText.text = "Phase 1: Store Open";
                break;
            case Phase.Phase2:
                phaseText.text = "Phase 2: Farming Open";
                break;
            //case Phase.Phase3:
            //    phaseText.text = "Phase 3: Tornado Strikes!";
            //    break;
            case Phase.Phase4:
                phaseText.text = "Phase 3: Time to Harvest";
                break;
        }
        bankBalanceText.text = "$" + bankBalance.ToString();
    }
    
    public void AddFunds(int amount)
    {
        bankBalance += amount;
        CheckWinCondition();
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
    public int GetCropCount(string cropType)
    {
        if (cropInventory.ContainsKey(cropType))
            return cropInventory[cropType];
        return 0;
    }

    public bool UseSeed(string seedType, int quantity = 1)
    {
        // Check if enough seeds exist
        if (GetSeedCount(seedType) >= quantity)
        {
            // Deduct the seeds from inventory
            seedInventory[seedType] -= quantity;
            Debug.Log("Used " + quantity + " " + seedType + " seed(s). Remaining: " + seedInventory[seedType]);
            return true;
        }
        else
        {
            Debug.Log("Not enough " + seedType + " seeds in inventory.");
            return false;
        }
    }

    public void StartPhase2Timer()
    {
        Debug.Log("TimeStartRecieved");
        StartCoroutine(Phase2TimerRoutine());
    }

    private IEnumerator Phase2TimerRoutine()
    {
        
        Debug.Log("TimeStarting");
        float timeRemaining = 120f;  // seconds

        while (timeRemaining > 0f)
        {
            // Format MM:SS
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            if (phase2TimerText != null)
                phase2TimerText.text = $"Time left: {minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
        }

        // Countdown is done → transition to Phase 3
        currentPhase = Phase.Phase3;
        Debug.Log("Phase 2 timer expired → now Phase 3");

        // Clear the timer display
        if (phase2TimerText != null)
            phase2TimerText.text = "";
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Nina_Scene3"); // Modified: Redirect to Level 1 scene
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

    private void CheckWinCondition()
    {
        if (bankBalance >= targetBalance)
        {
            Debug.Log($"Win condition met! Balance {bankBalance} ≥ {targetBalance}. Loading WinScene.");
            SceneManager.LoadScene("WinScene");
        }
    }
}
