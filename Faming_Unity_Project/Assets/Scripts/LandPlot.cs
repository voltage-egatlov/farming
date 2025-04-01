using UnityEngine;

public class LandPlot : MonoBehaviour
{
    public bool isPurchased = false;
    public int price = 100;

    public GameObject boundaryObject; // <-- NEW: drag the boundary here in Inspector

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

            // NEW: Hide the boundary
            if (boundaryObject != null)
                boundaryObject.SetActive(false);

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
            col.isTrigger = isPurchased;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPurchased && other.CompareTag("Tractor"))
        {
            Debug.Log("This land is not purchased yet!");
        }
    }
}
