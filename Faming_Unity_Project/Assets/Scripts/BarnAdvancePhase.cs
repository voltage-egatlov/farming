using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarnAdvancePhase : MonoBehaviour
{
    public GameObject Crop; // Reference to the crop prefab for replanting/harvest
    public TextMeshProUGUI PhaseText; 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

void OnTriggerEnter(Collider other)
{
    // Only the tractor in Phase2 can advance to Phase4 (harvesting)
    if (other.CompareTag("TractorModel") 
        && GameManager.Instance.currentPhase == GameManager.Phase.Phase2)
    {
        // 1) Advance to Phase4
        GameManager.Instance.currentPhase = GameManager.Phase.Phase4;
        PhaseText.text = "Phase 4: Harvesting mode";

        // 2) Find all planting boxes and mark any "Watered" ones as "Ready"
        var plantingBoxes = FindObjectsOfType<PlantingBoxScript>();
        foreach (var box in plantingBoxes)
        {
            if (box.currentState == PlantingBoxScript.BoxState.Watered)
                box.currentState = PlantingBoxScript.BoxState.Ready;
        }

        Debug.Log("Entered barn → Phase4. All watered boxes are now Ready.");
    }
}




    void clearPlot(GameObject plot)
    {
        for (int i = plot.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(plot.transform.GetChild(i).gameObject);
        }
    }
}
