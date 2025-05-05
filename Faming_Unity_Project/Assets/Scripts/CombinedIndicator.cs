using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Sequentially guides the player through equipping seeds (1), planting (Space),
/// equipping fertilizer (2), fertilizing (Space), equipping water (3), and watering (Space).
/// Displays an arrow+label for equip steps and a central plant prompt for planting steps.
/// </summary>
public class CombinedIndicator : MonoBehaviour
{
    [Header("Assign in Inspector – Transforms")]
    public Transform tractorTransform;   
    public Transform landTarget;          

    [Header("Assign in Inspector – UI Elements")]
    public RectTransform num1Box;         // Hotkey '1' UI box
    public RectTransform num2Box;         // Hotkey '2' UI box
    public RectTransform num3Box;         // Hotkey '3' UI box
    public Image arrowImage;              // Arrow graphic
    public TextMeshProUGUI label;         // Arrow label
    public TextMeshProUGUI plantPrompt;   // Central planting prompt text
    public Vector2 equipOffset = new Vector2(0, 20);

    [Header("Distance Settings")]
    public float enableDistance = 10f;    // Distance to land to enable equip arrows

    private int stage = 0;
    private RectTransform rt;
    private Camera cam;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        cam = Camera.main;
        // Fallback: find tractor by tag if not assigned
        if (tractorTransform == null)
        {
            var go = GameObject.FindGameObjectWithTag("Tractor");
            if (go) tractorTransform = go.transform;
        }
    }

    void Start()
    {
        // Begin the sequence
        NextStage();
    }

    void NextStage()
    {
        stage++;
        // Hide all visuals initially
        arrowImage.enabled = false;
        label.enabled = false;
        plantPrompt.enabled = false;
    }

    void Update()
    {
        // Ensure we have both required Transforms
        if (tractorTransform == null || landTarget == null)
            return;

        float dist = Vector3.Distance(tractorTransform.position, landTarget.position);

        switch (stage)
        {
            case 1: // Equip seeds
                if (dist <= enableDistance)
                {
                    arrowImage.enabled = true;
                    label.enabled = true;
                    label.text = "Press '1' to equip seeds";
                    rt.position = num1Box.position + (Vector3)equipOffset;
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                        NextStage();
                }
                break;

            case 2: // Plant seeds
                plantPrompt.enabled = true;
                plantPrompt.text = "Press Spacebar to plant the seed";
                if (Input.GetKeyDown(KeyCode.Space))
                    NextStage();
                break;

            case 3: // Equip fertilizer
                if (dist <= enableDistance)
                {
                    arrowImage.enabled = true;
                    label.enabled = true;
                    label.text = "Press '2' to equip fertilizer";
                    rt.position = num2Box.position + (Vector3)equipOffset;
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                        NextStage();
                }
                break;

            case 4: // Fertilize
                plantPrompt.enabled = true;
                plantPrompt.text = "Press Spacebar to fertilize the field";
                if (Input.GetKeyDown(KeyCode.Space))
                    NextStage();
                break;

            case 5: // Equip water
                if (dist <= enableDistance)
                {
                    arrowImage.enabled = true;
                    label.enabled = true;
                    label.text = "Press '3' to equip water";
                    rt.position = num3Box.position + (Vector3)equipOffset;
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                        NextStage();
                }
                break;

            case 6: // Water
                plantPrompt.enabled = true;
                plantPrompt.text = "Press Spacebar to water the field";
                if (Input.GetKeyDown(KeyCode.Space))
                    NextStage();
                break;

            default:
                // Sequence complete: ensure all are hidden
                arrowImage.enabled = false;
                label.enabled = false;
                plantPrompt.enabled = false;
                break;
        }
    }
}
