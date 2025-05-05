using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxState
{
    Empty,
    Planted,
    Watered,
    Fertilized,
    Ready
}

public class PlantingBoxScript : MonoBehaviour
{
    public GameObject PlantingVisual; // Reference to the planting visual object
    private Renderer canSee; // Renderer component of the planting visual object
    public GameObject IconsPrefab;
    private GameObject floatingIcons; // Instance of the icons prefab

    [SerializeField]
    public BoxState currentState = BoxState.Empty;

    void Start()
    {
        // Assign the current game object as the planting visual if not set
        if (PlantingVisual == null)
            PlantingVisual = this.gameObject;

        // Get the Renderer if available
        canSee = PlantingVisual.GetComponent<Renderer>();

        floatingIcons = Instantiate(IconsPrefab, transform.position, Quaternion.identity);
        floatingIcons.transform.SetParent(transform); // Set as child of the planting box
        floatingIcons.transform.localPosition = new Vector3(0, 2f, 0); // Adjust height as needed
        floatingIcons.GetComponent<PlantingIconsOverAreaScript>().currentBoxState = currentState;

        // Hide visual until triggered
        if (canSee != null)
        {
            canSee.enabled = false;
        }
    }

    void Update()
    {
        if (floatingIcons != null)
        {
            var iconsScript = floatingIcons.GetComponent<PlantingIconsOverAreaScript>();
            if (iconsScript != null)
            {
                iconsScript.currentBoxState = currentState;
            }
        }
        // Make text face the main camera safely
        if (Camera.main != null)
        {
            floatingIcons.transform.LookAt(Camera.main.transform.position);
            floatingIcons.transform.Rotate(0, 180, 0); // Flip the text
            floatingIcons.transform.rotation = Quaternion.Euler(
                0,
                floatingIcons.transform.rotation.eulerAngles.y,
                0
            );
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TractorModel") && canSee != null)
        {
            canSee.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TractorModel") && canSee != null)
        {
            canSee.enabled = false;
        }
    }

    /// <summary>
    /// Destroys roughly half of the crop objects (child objects tagged "Crop") in this planting box.
    /// Assumes crop GameObjects are added as children (starting from index 2, preserving essential objects).
    /// </summary>
    public void DestroyHalfOfCrops()
    {
        // Create a list to hold crop GameObjects
        List<GameObject> cropObjects = new List<GameObject>();

        // Assume first two children are reserved (e.g., the visual and floating text)
        for (int i = 2; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.CompareTag("Crop"))
            {
                cropObjects.Add(child);
            }
        }

        int totalCrops = cropObjects.Count;
        int cropsToDestroy = totalCrops / 2;  // Destroy approximately half

        // Create a list of indices for random selection
        List<int> indices = new List<int>();
        for (int i = 0; i < cropObjects.Count; i++)
        {
            indices.Add(i);
        }

        // Randomly destroy a selected number of crops
        for (int i = 0; i < cropsToDestroy; i++)
        {
            int randIndex = Random.Range(0, indices.Count);
            int cropIndex = indices[randIndex];
            Destroy(cropObjects[cropIndex]);
            indices.RemoveAt(randIndex);
        }

        Debug.Log($"Destroyed {cropsToDestroy} crops in planting box '{gameObject.name}'.");
    }
}
