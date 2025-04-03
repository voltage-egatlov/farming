using UnityEngine;

public class LandPlot : MonoBehaviour
{
    public bool isPurchased = false;
    public int price = 100;

    public GameObject boundaryObject; // Assign the visual + collider object here

    private void Awake()
    {
        UpdateAccess();
    }

    public void Purchase()
    {
        if (!isPurchased && GameManager.Instance.DeductFunds(price))
        {
            isPurchased = true;
            UpdateAccess();

            Debug.Log(gameObject.name + " has been purchased!");
        }
        else
        {
            Debug.Log("Not enough funds to buy " + gameObject.name);
        }
    }

    private void UpdateAccess()
    {
        // Hide the boundary when land is purchased
        if (boundaryObject != null)
            boundaryObject.SetActive(!isPurchased); // ON if not purchased, OFF if purchased
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isPurchased && other.collider.CompareTag("Tractor"))
        {
            Debug.Log("This land is not purchased yet!");
        }
    }
}
