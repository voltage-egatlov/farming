using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int bankBalance = 1000;  // Starting funds

    private Dictionary<string, int> seedInventory = new Dictionary<string, int>();

    
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
}
