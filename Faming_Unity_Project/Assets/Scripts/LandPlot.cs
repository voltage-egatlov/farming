using UnityEngine;

public class LandPlot : MonoBehaviour
{
    public bool isPurchased = false;
    public int price = 100;

    private Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
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
        if (col != null)
        {
            col.isTrigger = isPurchased; // Block if not purchased
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPurchased && other.CompareTag("Tractor"))
        {
            Debug.Log("This land is not purchased yet!");
            // Optional: bounce back the tractor a bit?
        }
    }
}
