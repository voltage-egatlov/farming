using UnityEngine;

public class SeedStore : MonoBehaviour
{
    public void BuySeed(string seedType)
    {
        int cost = GetSeedCost(seedType);

        // Step 2: Check the player's bank balance and deduct funds.
        if (GameManager.Instance.DeductFunds(cost))
        {
            // Purchase successful: update seed inventory.
            GameManager.Instance.AddSeed(seedType);
            Debug.Log("Purchased " + seedType + " seed.");
            // Optionally update UI to show new balance or inventory.
        }
        else
        {
            // Purchase failed: not enough funds.
            Debug.Log("Insufficient funds to purchase " + seedType + " seed.");
            // Optionally, show a UI message to the player.
        }
    }

    // Helper method to get cost based on seed type.
    private int GetSeedCost(string seedType)
    {
        switch (seedType)
        {
            case "Tomato":
                return 120;
            case "Carrot":
                return 110;
            case "Corn":
                return 150;
            case "Cabbage":
                return 130;
            case "Wheat":
                return 100;
            default:
                return 100; 
        }
    }

}
