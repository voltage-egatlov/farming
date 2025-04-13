using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarnAdvancePhase : MonoBehaviour
{
    public GameObject Crop; // Reference to the crop prefab for replanting/harvest
    public TextMeshProUGUI PhaseText; // UI text component to display phase/harvest info

    // Start is called before the first frame update
    void Start()
    {
        // Optionally, initialize the UI text with the current phase if desired.
        PhaseText.text = "Phase 2: Farming in progress";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Only process if the tractor enters AND we are in Phase 2.
        if (other.CompareTag("TractorModel") && GameManager.Instance.currentPhase == GameManager.Phase.Phase2)
        {
            PlantingBoxScript[] plantingBoxes = FindObjectsOfType<PlantingBoxScript>();
            int numHarvested = 0;

            foreach (PlantingBoxScript plantingBox in plantingBoxes)
            {
                // Only process boxes that have been watered (ready to be harvested)
                if (plantingBox.currentState == PlantingBoxScript.BoxState.Watered)
                {
                    // Clear existing crops from the planting box
                    clearPlot(plantingBox.gameObject);

                    float plantingWidth = plantingBox.transform.localScale.x; // width of the box
                    float plantingLength = plantingBox.transform.localScale.z; // length of the box

                    // Determine how many crops can be placed in width and length (adjust scaling as needed)
                    float numCropsWidth = Mathf.Round(plantingWidth * 25);
                    float numCropsLength = Mathf.Round(plantingLength * 25);

                    // Instantiate new crops to simulate harvesting/replanting:
                    for (int i = 0; i < numCropsWidth; i++)
                    {
                        for (int j = 0; j < numCropsLength; j++)
                        {
                            GameObject newCrop = Instantiate(Crop, Vector3.zero, Quaternion.identity, plantingBox.transform);
                            newCrop.transform.localScale = new Vector3(0.75f, 7.5f, 0.75f);
                            newCrop.transform.localPosition = new Vector3(-0.5f + (i + 0.5f) * (1 / numCropsWidth), 0, -0.5f + (j + 0.5f) * (1 / numCropsLength));
                            // Optionally, tag the new crop as "Crop" if needed:
                            newCrop.tag = "Crop";
                        }
                    }
                    
                    // Set the box state to Ready after processing
                    plantingBox.currentState = PlantingBoxScript.BoxState.Ready;
                    numHarvested++;
                }
                // For boxes that are not in an empty or ready state, clear them and mark as empty.
                if (plantingBox.currentState != PlantingBoxScript.BoxState.Empty &&
                    plantingBox.currentState != PlantingBoxScript.BoxState.Ready)
                {
                    clearPlot(plantingBox.gameObject);
                    plantingBox.currentState = PlantingBoxScript.BoxState.Empty;
                }
            }

            // Update PhaseText with harvested plot count
            PhaseText.text = numHarvested + " plots ready to harvest";
            
            // Advance the game phase to Phase 3
            GameManager.Instance.currentPhase = GameManager.Phase.Phase3;
            Debug.Log("Transitioned to Phase 3: Barn reached.");
            
            // Optionally, you can update the PhaseText to indicate the new phase.
            PhaseText.text += "\nPhase 3 initiated.";
        }
    }

    // Function to clear the children (crops) of a planting box.
    // Assumes that the first two children are reserved (e.g., for visual elements).
    void clearPlot(GameObject plot)
    {
        for (int i = plot.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(plot.transform.GetChild(i).gameObject);
        }
    }
}
