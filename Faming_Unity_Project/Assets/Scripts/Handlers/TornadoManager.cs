using System.Collections;
using UnityEngine;
using TMPro;

public class TornadoManager : MonoBehaviour
{
    public static TornadoManager Instance;
    
    [Header("Tornado Setup")]
    public GameObject tornadoPrefab;        // Your VFX prefab
    public float tornadoDuration = 30f;     // 30-second run

    [Header("Timer UI")]
    public TextMeshProUGUI tornadoTimerText; // Assign your TornadoTimerText here

    private GameObject activeTornado;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Call this to begin the Phase 3 tornado event.
    /// </summary>
    public void StartTornadoEvent()
    {
        StartCoroutine(TornadoRoutine());
    }

    private IEnumerator TornadoRoutine()
    {
        // 1) Instantiate tornado
        activeTornado = Instantiate(
            tornadoPrefab,
            /* spawn at center of your field: */ Vector3.zero,
            Quaternion.identity
        );

        // 2) Countdown loop
        float timeLeft = tornadoDuration;
        while (timeLeft > 0f)
        {
            // Update UI
            if (tornadoTimerText != null)
                tornadoTimerText.text = $"Tornado in: {Mathf.CeilToInt(timeLeft)}s";

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        // 3) Time’s up: remove tornado
        if (activeTornado != null) Destroy(activeTornado);

        // 4) Destroy any crops still tagged "Crop"
        var remaining = GameObject.FindGameObjectsWithTag("Crop");
        foreach (var c in remaining) Destroy(c);

        // 5) Clear timer UI
        if (tornadoTimerText != null)
            tornadoTimerText.text = "";

        // 6) Advance to Phase 4
        GameManager.Instance.currentPhase = GameManager.Phase.Phase4;
        Debug.Log("Tornado over → Phase 4 harvesting unlocked");
    }
}
