using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingBoxScript : MonoBehaviour
{
    public GameObject PlantingVisual; // Reference to the planting visual object
    private Renderer canSee; // Renderer component of the planting visual object
    private GameObject floatingText; // Floating text object to show the current state of the planting box
    private TextMesh floatingTextMesh; // Cached TextMesh component

    public enum BoxState
    {
        Empty,
        Planted,
        Watered,
        Fertilized,
        Ready
    }

    [SerializeField]
    public BoxState currentState = BoxState.Empty;

    void Start()
    {
        // Assign the current game object as the planting visual
        PlantingVisual = this.gameObject;

        // Get the Renderer if available
        canSee = PlantingVisual.GetComponent<Renderer>();

        // Create floating text
        floatingText = new GameObject("FloatingText");
        floatingTextMesh = floatingText.AddComponent<TextMesh>();

        // Set text and appearance
        floatingTextMesh.text = currentState.ToString();
        floatingTextMesh.fontSize = 12;
        floatingTextMesh.color = Color.white;
        floatingTextMesh.anchor = TextAnchor.MiddleCenter;

        // Position above box
        floatingText.transform.SetParent(PlantingVisual.transform);
        floatingText.transform.localPosition = new Vector3(0, 3, 0);

        // Hide visual until triggered
        if (canSee != null)
        {
            canSee.enabled = false;
        }
    }

    void Update()
    {
        // Update state label
        if (floatingTextMesh != null)
        {
            floatingTextMesh.text = currentState.ToString();

            // Make text face the main camera safely
            if (Camera.main != null)
            {
                floatingText.transform.LookAt(Camera.main.transform.position);
                floatingText.transform.Rotate(0, 180, 0); // flip
                floatingText.transform.rotation = Quaternion.Euler(
                    0,
                    floatingText.transform.rotation.eulerAngles.y,
                    0
                );
            }
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
}
