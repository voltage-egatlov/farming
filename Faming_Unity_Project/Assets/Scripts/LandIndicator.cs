using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LandIndicator : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform landTarget;        
    public Transform tractorTransform;  
    public GameObject storeUI;         
    public float closeDistance = 5f;    // how close before hiding permanently

    [Header("UI Parts")]
    public Image arrowImage;            // child Image component
    public TextMeshProUGUI label;       // child TMP text

    public Vector2 offset = new Vector2(0, 50);

    RectTransform rt;
    Camera cam;
    bool storeHasOpened = false;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        cam = Camera.main;

        // fallback if tractorTransform is null
        if (tractorTransform == null)
        {
            var go = GameObject.FindGameObjectWithTag("Tractor");
            if (go != null) tractorTransform = go.transform;
        }
    }

    void Start()
    {
        // Show at the very start
        Show();
    }

    void Update()
    {
        // 1) If the store UI is open, hide the indicator (but keep script alive)
        if (storeUI.activeInHierarchy)
        {
            storeHasOpened = true;
            Hide();
            return;
        }

        // 2) If the store has been opened and is now closed, re-show
        if (storeHasOpened && !arrowImage.enabled)
        {
            Show();
        }

        // 3) Update the UI position to point at the land target
        Vector3 screenPos = cam.WorldToScreenPoint(landTarget.position);
        rt.position = screenPos + (Vector3)offset;

        // 4) If you’re close enough, hide permanently
        if (tractorTransform != null &&
            Vector3.Distance(tractorTransform.position, landTarget.position) <= closeDistance)
        {
            Debug.Log("[LandIndicator] hiding because store is open");
            Hide();
     
            return;
        }
    }

    void Show()
    {
        label.text = "2. Go to the land";
        arrowImage.enabled = true;
        label.enabled = true;
    }

    void Hide()
    {
        arrowImage.enabled = false;
        label.enabled = false;
    }
}
