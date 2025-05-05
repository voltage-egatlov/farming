using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Shows an arrow and label pointing at the Inventory button when the player
/// is within a certain distance of a specified world object. Hides itself
/// when the inventory button is clicked.
/// </summary>
public class InventoryIndicator : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform tractorTransform;           // the player/tractor Transform
    public Transform proximityTarget;            // world object to approach
    public float enableDistance = 8f;           // distance to start showing

    [Header("UI Elements")]
    public Button inventoryButton;              // the Inventory UI Button
    public RectTransform inventoryButtonTransform; // RectTransform of the Button
    public Image arrowImage;                    // arrow sprite to point
    public TextMeshProUGUI label;               // label TextMeshProUGUI
    public Vector2 offset = new Vector2(0, 20); // screen offset from the button

    RectTransform rt;
    Camera cam;

    void Awake()
    {
        // cache components
        rt = GetComponent<RectTransform>();
        cam = Camera.main;

        // fallback find tractor by tag if not assigned
        if (tractorTransform == null)
        {
            var go = GameObject.FindGameObjectWithTag("Tractor");
            if (go != null) tractorTransform = go.transform;
        }

        // subscribe to inventory button click to hide indicator
        if (inventoryButton != null)
            inventoryButton.onClick.AddListener(HideIndicator);
    }

    void Start()
    {
        // set the message text
        label.text = "Click here to check the remaining seeds";
        // initialize hidden
        arrowImage.enabled = false;
        label.enabled = false;
    }

    void Update()
    {
        if (tractorTransform == null || proximityTarget == null) return;

        // show when within range, hide otherwise
        float dist = Vector3.Distance(tractorTransform.position, proximityTarget.position);
        if (dist <= enableDistance)
            ShowIndicator();
        else
            HideIndicator();

        // position arrow over the Inventory button if visible
        if (arrowImage.enabled)
        {
            rt.position = (Vector3)inventoryButtonTransform.position + (Vector3)offset;
        }
    }

    void ShowIndicator()
    {
        arrowImage.enabled = true;
        label.enabled = true;
    }

    void HideIndicator()
    {
        arrowImage.enabled = false;
        label.enabled = false;
    }
}

