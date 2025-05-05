using UnityEngine;
using TMPro;

public class StoreIndicator : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform storeModel;      
    public GameObject storeUI;        
    public TextMeshProUGUI label;

    public Vector2 offset = new Vector2(0, 50);

    RectTransform rt;
    Camera cam;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    void OnEnable()
    {
        // set initial text
        label.text = "1. Go to Store";
    }

    void Update()
    {
        // 1) Position the arrow over the store in screen space
        Vector3 screenPos = cam.WorldToScreenPoint(storeModel.position);
        rt.position = screenPos + (Vector3)offset;

        // 2) If the Store UI is open, hide this indicator
        if (storeUI.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
}
